using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Survey.Models;

namespace Survey.ViewModels
{
    public class PersonInformation
    {
        public DbSet<CLIENT> CLEINT {get; set;}
        public DbSet<REGISTRATION> REGISTRATION {get; set;}
        public DbSet<PERSON> PERSON {get; set;}
    }
}

