using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Survey.Models;
using System.Data.Entity;

namespace Survey.ViewModels
{
    public class ClientIndexData
    {
        public DbSet<ACTIVITY> Activities { get; set; }
        public DbSet<COURSE> Courses { get; set; }

    }
}
