using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureFunctions.Functions
{
    public static class DurableOrchestrator
    {
        [FunctionName("DurableOrchestrator")]
        public static async Task RunOrchestrator(
            [OrchestrationTrigger] DurableOrchestrationContext context,
            ILogger log
        ) {
            for (var i = 0; i <= 100; i++)
            {
                var extra = context.IsReplaying ? "[REPLAY]" : "";

                log.LogInformation($"{extra}Starting activity..");
                var percentage = await context.CallActivityAsync<int>("DurableOrchestrator_Activity_DoSomething", i);
                log.LogInformation($"{extra}Orchestrator received: {percentage}%");

                context.SetCustomStatus(new { percentage = percentage });
            }
        }

        [FunctionName("DurableOrchestrator_Activity_DoSomething")]
        public static int DoSomething([ActivityTrigger] int percentage, ILogger log)
        {
            log.LogInformation($"Waiting some time to return {percentage}");
            Thread.Sleep(100);
            return percentage;
        }

        [FunctionName("DurableOrchestrator_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")]HttpRequestMessage req,
            [OrchestrationClient]DurableOrchestrationClient starter,
            ILogger log)
        {
            log.LogInformation($"Trigger function has been called via HTTP.");
            var instanceId = await starter.StartNewAsync("DurableOrchestrator", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");
            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}