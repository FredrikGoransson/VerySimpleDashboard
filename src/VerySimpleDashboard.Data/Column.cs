using System;

namespace VerySimpleDashboard.Data
{
    public class Column
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DataType DataType { get; set; }
    }
}