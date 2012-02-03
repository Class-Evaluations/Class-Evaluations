using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.ViewModels
{
    public class SurveySection
    {
        public Int32? surveyQuestionID { get; set; }
        public Int32 surveyID { get; set; }
        public Int32? questionID { get; set; }
        public Int32 sectionID { get; set; }
    }
}