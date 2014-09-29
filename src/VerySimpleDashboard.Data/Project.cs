﻿using System;
using System.Collections.Generic;

namespace VerySimpleDashboard.Data
{
    public class Project
    {
        public Project()
        {
            Id = Guid.NewGuid();
            Tables = new List<Table>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public IList<Table> Tables { get; set; }
    }
}
