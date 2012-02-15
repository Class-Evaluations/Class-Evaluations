using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;
using PagedList;
using Survey.ViewModels;

namespace Survey.Controllers
{
    public class CourseController : Controller
    {
        private Class_701Entities _db = new Class_701Entities();
        private Survey_DBEntities survey_db = new Survey_DBEntities();
        
        //Set up paging for the list of courses
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
           //sorting functionality functionality
            ViewBag.CurrentSort = sortOrder;
            ViewBag.ActivitySortParam = String.IsNullOrEmpty(sortOrder)? "Activity Desc" : "";
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title Desc" : "";
            ViewBag.CourseSortParm = String.IsNullOrEmpty(sortOrder) ? "Course Desc" : ""; 
            ViewBag.BarcodeSortParm = String.IsNullOrEmpty(sortOrder)? "Barcode Desc" : "";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            //var CourseDetails = survey_db.GetCoursesWithStatus().ToList();
            var CourseDetails = survey_db.GetCoursesWithSurveyStatus().ToList();


            ////searching functionality
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    courses = courses.Where(s => s.barcode_number.Contains(searchString));
            //}
            //else
            //{
            //    courses = courses.Where(s => s.course_id > 111460);
            //    //For testing we cut down the amount of records returned.
            //    //courses = courses.Where(s => s.cancel_reason = Convert.ToChar('Course Completed')) or (s => s.last_end_datetime > DateTime.Today.AddDays(+7); 
            //}


            //switch (sortOrder) 
            //{ 
            //    case "Title Desc": 
            //        courses = courses.OrderByDescending(s => s.title); 
            //        break; 
            //    case "Barcode Desc": 
            //        courses = courses.OrderByDescending(s => s.barcode_number); 
            //        break; 
            //    case "Activity Desc":
            //        courses = courses.OrderByDescending(s => s.activity_id);
            //        break;
            //    case "Course Desc":
            //        courses = courses.OrderByDescending(s => s.course_id);
            //        break;
            //      default: 
            //        courses = courses.OrderBy(s => s.course_id); 
            //        break; 
            //}
 
            int pageSize = 10;
            int pageIndex = (page ?? 1) - 1;
            return View(CourseDetails.ToPagedList(pageIndex, pageSize));
        }
   }
}
