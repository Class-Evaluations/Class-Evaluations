using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.ViewModels
{
    public class ReportShort
    {
        public int questionID { get; set; }
        public string responseShort { get; set; }
        public string answerType { get; set; }
    }
}