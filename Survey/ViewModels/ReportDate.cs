using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.ViewModels
{
    public class ReportData
    {
        public Int32? questionID { get; set; }
        public string questionText { get; set; }
        public Int32? answerID { get; set; }
        public string answerType { get; set; }
        public double responseStat { get; set; }
    }
}