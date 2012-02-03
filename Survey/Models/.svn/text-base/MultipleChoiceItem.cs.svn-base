using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Survey.Models
{
    public class MultipleChoiceItem
    {
        [HiddenInput(DisplayValue=false)]
        public Int32 QID { get; set; }
        [HiddenInput(DisplayValue = false)]
        public Int32 Answertype { get; set; }
        [HiddenInput(DisplayValue = false)]
        public string Questiontext { get; set; }
        public string ChoiceText1 { get; set; }
        public string ChoiceText2 { get; set; }
        public string ChoiceText3 { get; set; }
        public string ChoiceText4 { get; set; }
        public string ChoiceText5 { get; set; }
    }

    public class ListMultipleChoice
    {
        public List<MultipleChoiceItem> ChoiceList { get; set; }
    }
}


