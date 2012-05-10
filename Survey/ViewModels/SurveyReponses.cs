using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.ViewModels
{
    public class SurveyReponses
    {
        public Int32? questionID { get; set; }
        public string questionText { get; set; }
        public Int32? answerID { get; set; }
        public string answerType { get; set; }
        public int submittedAnswer { get; set; }
        public int surveyRequestID { get; set; }
    }
}