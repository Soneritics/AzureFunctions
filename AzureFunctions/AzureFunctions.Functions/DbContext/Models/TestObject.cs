using System;
using System.Collections.Generic;
using System.Text;

namespace AzureFunctions.Functions.DbContext.Models
{
    public class TestObject
    {
        public int Id { get; set; }
        public Guid Key { get; set; } = Guid.NewGuid();
        public string Value { get; set; }
    }
}
