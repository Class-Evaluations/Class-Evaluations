using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Survey.ViewModels
{
    public class CourseStatusGroup
    {
        public string CourseStatus { get; set;}
        public int SurveySent { get; set; }
        public int SurveyAnswered { get; set; }
        public double PercentComplete { get; set; }

    }
}