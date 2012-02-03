using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Survey.ViewModels
{
    public class CourseList
    {
        public Int32 courseid { get; set; }
        public Int32 activityid { get; set; }
        public string title { get; set;}
        public string coursetitle { get; set; }
        public string barcode { get; set; }
        public string coursestatusID { get; set; }

        //[DataType(DataType.Date)]
        public DateTime lastStartdate { get; set; }
        //[DataType(DataType.Date)]
        public DateTime lastEnddate { get; set; }
        
        public string course_status { get; set; } 
    }
}