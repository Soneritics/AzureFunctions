using System;
using System.IO;
using System.Threading.Tasks;
using AzureFunctions.Functions.DbContext;
using AzureFunctions.Functions.DbContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureFunctions.Functions
{
    public class SqlMiddlewareFunction
    {
        private FunctionDbContext _context;

        public SqlMiddlewareFunction(FunctionDbContext context)
        {
            _context = context;
        }

        [FunctionName("SqlMiddlewareFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var name = (string)req.Query["name"];
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            if (name == null)
                return new BadRequestObjectResult("Please pass a value on the query string or in the request body");

            var testObject = new TestObject() { Value = name };
            _context.Add(testObject);
            await _context.SaveChangesAsync();

            return (ActionResult)new OkObjectResult($"Value is saved as #{testObject.Id} - {testObject.Key}");
        }
    }
}
