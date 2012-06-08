using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Survey.Models;
using PagedList;

namespace Survey.ViewModels
{
    public class CourseList
    {
        public Int32 courseid { get; set; }
        public Int32 activityid { get; set; }
        public string title { get; set; }
        public string coursetitle { get; set; }
        public string barcode { get; set; }
        public string coursestatusID { get; set; }

        //[DataType(DataType.Date)]
        public string lastStartdate { get; set; }
        //[DataType(DataType.Date)]
        public string lastEnddate { get; set; }

        internal static object ToPagedList(int pageNumber, int p)
        {
            throw new NotImplementedException();
        }
    }

    public class CourseStatus 
    {
        public Int32 courseStatusid { get; set; }
        public Int32 courseid { get; set; }
        public string courseStatus { get; set; }
        public DateTime surveyExpDate { get; set;}

    }

    public class CourseViewModel 
    {
        public IEnumerable<CourseList> Courses { get; set; }
        public IEnumerable<CourseStatus> Statuses { get; set; }     
    } 
    
    //class Student 
    //{ 
    //    public int Id { get; set; } 
    //}
    
    //class StudentPropertyDefintion 
    //{ 
    //    public int Id { get; set; } 
    //    public int StudentId { get; set; } 
    //    public string PropertyName { get; set; } 
    //}
    
    //class StudentPropertyValue 
    //{ 
    //    public int PropertyDefinitionId { get; set; } 
    //    public string PropertyValue { get; set; } 
    //}
    
    //class Context 
    //{ 
    //    public ISet<Student> Students { get; set; } 
    //    public ISet<StudentPropertyDefintion> PropertyDefinitions { get; set; } 
    //    public ISet<StudentPropertyValue> PropertyValues { get; set; } 
    //}  
   
}