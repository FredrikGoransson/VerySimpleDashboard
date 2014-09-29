using System;

namespace VerySimpleDashboard.Data
{
    public class Column
    {
        public Column()

        {
            Id = Guid.NewGuid();
            DataType = DataType.Unknown;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public DataType DataType { get; set; }
    }
}