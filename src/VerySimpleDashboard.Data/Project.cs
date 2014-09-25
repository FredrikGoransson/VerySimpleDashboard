using System;
using System.Collections;
using System.Collections.Generic;

namespace VerySimpleDashboard.Data
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Table> Tables { get; set; }
    }

    public class Table
    {
        public string Name { get; set; }
        public IList<Column> Columns { get; set; }
    }

    public class Column
    {
        public string Name { get; set; }
        public DataType Type { get; set; }
    }

    public enum DataType
    {
        String,
        DateTime,
        Integer,
        Double,
        Boolean,
    }

    public class User
    {
        public Guid Id { get; set; }
        public Guid Name { get; set; }
        public IList<Project> Projects { get; set; }
    }


}
