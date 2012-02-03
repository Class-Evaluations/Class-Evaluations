using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Survey.Models;

namespace Survey.ViewModels
{
    public class PersonSurveyInformation
    {
        //public DbSet<SURVEY> SURVEYs { get; set; }
        //public DbSet<PERSON> PERSON { get; set; }

        public int personid { get; set; }
        public string lastname { get; set; }
        public string firstname { get; set; }
        public int? addressid { get; set; }
        public int? userflag { get; set; }
        public int? caretakerflag { get; set; }
        public string emailaddress { get; set; }
        public int? emailflag { get; set; }

        public int surveyid { get; set; }
        public string title { get; set; }
        public string header { get; set; }
        public string surveystatus { get; set; }
        public int lifetime { get; set; }

    }
}

