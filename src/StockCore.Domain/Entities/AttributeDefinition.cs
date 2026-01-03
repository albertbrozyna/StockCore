using System;
using System.Collections.Generic;
using System.Text;

namespace StockCore.Domain.Entities
{
    public class AttributeDefinition
    {
        public int Id { get; set; }
        public string Name { get; set; } // ex color
        public string DataType { get; set; } // ex. string, int
    }
}
