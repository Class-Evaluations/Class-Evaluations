﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.ViewModels
{
    public class ReportMultiChoice
    {
        public int questionID { get; set; }
        public int count { get; set; }
        public string responseMultiChoice { get; set; }
        public string answerType { get; set; }
    }
}