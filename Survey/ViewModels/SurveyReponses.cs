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
        public string submittedAnswer { get; set; }
        public int surveyRequestID { get; set; }
        public double responseScale { get; set; }
        public List<AnswerLong> responseLong { get; set; }
        public List<AnswerShort> responseShort { get; set; }
        public int responseMultiChoice { get; set; }
        public int responseTF { get; set; }
    }
}