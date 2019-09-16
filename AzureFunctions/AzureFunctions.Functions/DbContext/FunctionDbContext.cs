using System;
using System.Collections.Generic;
using System.Text;
using AzureFunctions.Functions.DbContext.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureFunctions.Functions.DbContext
{
    public class FunctionDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public FunctionDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TestObject> TestObjects { get; set; }
    }
}
