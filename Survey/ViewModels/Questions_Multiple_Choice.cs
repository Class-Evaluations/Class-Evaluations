using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Survey.Models;

namespace Survey.ViewModels
{
    public class Questions_Multiple_Choice
    {
        public Int32? questionID { get; set; }
        public string questionText { get; set; }
        public Int32? answerTypeId { get; set; }
        public string answerTypeName { get; set; }
        public string choice_text { get; set; }

    }
}
