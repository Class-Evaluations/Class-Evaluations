using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.ViewModels
{
    public class ClientPersonMergeData
    {
        public string barcode {get; set; }
        public Int32 client_id {get; set; }
        public Int32 person_id { get; set; }
    }
}