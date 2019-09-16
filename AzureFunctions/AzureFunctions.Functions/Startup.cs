using System;
using AzureFunctions.Functions;
using AzureFunctions.Functions.DbContext;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(StartUp))]

namespace AzureFunctions.Functions
{
    public class StartUp : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            // Add environment variable: sqlConnection
            // This must hold the connection string to a database.
            // In that database, run the following query:
            /*
             CREATE TABLE [dbo].[TestObjects] (
                [Id]    INT              NOT NULL IDENTITY,
                [Key]   UNIQUEIDENTIFIER NOT NULL,
                [Value] NVARCHAR (50)    NOT NULL,
                PRIMARY KEY CLUSTERED ([Id] ASC)
            );
             */
            var sqlConnection = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<FunctionDbContext>(
                options => options.UseSqlServer(sqlConnection)
            );
        }
    }
}
