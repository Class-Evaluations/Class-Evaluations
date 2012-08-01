using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.ViewModels
{
    public class ReportScale
    {
        public int questionID { get; set; }
        public double responseScale { get; set; }
        public string answerType { get; set; }
    }
}
