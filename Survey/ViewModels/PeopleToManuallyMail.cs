using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.ViewModels
{
    public class PeopleToManuallyMail
    {
        public Int32 personID {get; set; }
        public Int32 courseID {get; set; }
        public Int32? addressID {get; set; }
        public string firstName {get; set; }
        public string lastName {get; set; }
        public string email {get; set; }
        public Byte? emailPrivate {get; set; }
        public string barcode {get; set; }


    }
}