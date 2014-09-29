using System;

namespace VerySimpleDashboard.Data
{
    public class DataRow
    {
        public Guid Id { get; set; }
        public object[] Data { get; set; }
    }
}