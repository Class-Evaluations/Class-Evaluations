using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.ViewModels
{
    public class ReportQuestions
    {
        public int questionID { get; set; }
        public string questionText { get; set; }
        public string answerType { get; set; }
    }
}