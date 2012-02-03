using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Survey.ViewModels
{
    public class EmailStats
    {
        public int EmailCountSent { get; set;}
        public int NoEmailAdddress { get; set;}
        public int SurveyAlreadySent { get; set; }
    }
}

