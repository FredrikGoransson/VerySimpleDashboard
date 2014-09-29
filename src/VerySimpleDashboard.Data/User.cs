using System;
using System.Collections.Generic;

namespace VerySimpleDashboard.Data
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid Name { get; set; }
        public IList<Project> Projects { get; set; }
    }
}