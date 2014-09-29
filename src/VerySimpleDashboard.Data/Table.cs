using System;
using System.Collections.Generic;

namespace VerySimpleDashboard.Data
{
    public class Table
    {
        public Table()
        {
            Id = Guid.NewGuid();
            Columns = new List<Column>();
            Rows = new List<DataRow>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<DataRow> Rows { get; set; }
        public IList<Column> Columns { get; set; }
    }
}