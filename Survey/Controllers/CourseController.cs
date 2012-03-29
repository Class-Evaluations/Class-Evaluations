using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;
using PagedList;
using Survey.ViewModels;
using System.Data.Objects;
using System.Data.EntityModel;

namespace Survey.Controllers
{
    public class CourseController : Controller
    {
        private CLASSEntities _db = new CLASSEntities();
        private Survey_DBEntities survey_db = new Survey_DBEntities();
        
        //Set up paging for the list of courses

        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
           //sorting functionality functionality
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.ActivitySortParam = String.IsNullOrEmpty(sortOrder)? "Activity Desc" : "";
            //ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title Desc" : "";
            //ViewBag.CourseSortParm = String.IsNullOrEmpty(sortOrder) ? "Course Desc" : ""; 
            //ViewBag.BarcodeSortParm = String.IsNullOrEmpty(sortOrder)? "Barcode Desc" : "";

            //if (Request.HttpMethod == "GET")
            //{
            //    searchString = currentFilter;
            //}
            //else
            //{
            //    page = 1;
            //}

            //var CourseDetails = from c in _db.COURSEs
            //                    where c.session_title_id == 9 && (c.cancel_reason == "Course Completed")  
            //                    orderby c.course_id
            //                    select new CourseList
            //                    {
            //                        courseid = c.course_id, 
            //                        activityid = c.activity_id,
            //                        title = c.ACTIVITY.title,
            //                        coursetitle = c.title,
            //                        barcode = c.barcode_number,
            //                        coursestatusID = c.course_status_id,
            //                        lastStartdate = Convert.ToDateTime(c.last_start_datetime).Date.ToString(),
            //                        lastEnddate = Convert.ToDateTime(c.last_end_datetime).Date.ToString()

            //                    };

            //searching functionality
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
            //    default:
            //        courses = courses.OrderBy(s => s.course_id);
            //        break;
            //}


            //Context context = new Context();

            //var studentWithPropertiesQuery = from student in context.Students
            //                                 join propertyDefinition in context.PropertyDefinitions
            //                                 on student.Id equals propertyDefinition.StudentId
            //                                 join propertyValue in context.PropertyValues
            //                                 on propertyDefinition.Id equals propertyValue.PropertyDefinitionId
            //                                 select new
            //                                 {
            //                                     propertyValue,
            //                                     propertyDefinition,
            //                                     student
            //                                 }; 

            
            DateTime Today = DateTime.Now.Date;

            var CourseDetails = from c in _db.COURSEs
                                where c.session_title_id == 9 && (c.cancel_reason == "Course Completed" || EntityFunctions.AddDays(c.last_end_datetime, 7) < Today)
                                orderby c.course_id
                                select c;
                                //select new CourseList
                                //{
                                //    courseid = c.course_id,
                                //    activityid = c.activity_id,
                                //    title = c.ACTIVITY.title,
                                //    coursetitle = c.title,
                                //    barcode = c.barcode_number,
                                //    coursestatusID = c.course_status_id,
                                //    lastStartdate = c.last_start_datetime.HasValue ? c.last_start_datetime.Value.ToLongDateString() : c.last_start_datetime.ToString(),
                                //    lastEnddate = c.last_end_datetime.HasValue ? c.last_end_datetime.Value.ToLongDateString() : c.last_end_datetime.ToString()                               
                                //};

            var SurveyStatus = from k in survey_db.COURSE_STATUS
                               select k;
                               //select new CourseStatus 
                               //{
                               //    courseStatusid = k.course_status_id,
                               //    courseid = k.course_id,
                               //    courseStatus = k.course_status1
                               //};

            CourseViewModel context = new CourseViewModel();
            //context.Courses = CourseDetails;
            //context.Statuses = SurveyStatus;

            //var coursesWithSurveyStatus = (from CourseList in context.Courses
            //                               join CourseStatus in context.Statuses
            //                               on CourseList.courseid equals CourseStatus.courseid into ps
            //                               from CourseStatus in ps.DefaultIfEmpty()
            //                               select new { context }).AsEnumerable();


            ViewBag.CourseList = CourseDetails;
            ViewBag.SurveyStatus = SurveyStatus;
            //ViewBag.CourseWithSurvey = coursesWithSurveyStatus;
  
            //loop through the CourseDetails and add the status from the CourseStatus

            //foreach (var item in CourseDetails)
            //{
            //    foreach (var rec in SurveyStatus)
            //    {
            //        if (rec.course_id == item.courseid)
            //        {
            //            item.courseStatus = rec.course_status1;
            //            //item.surveyExpiration = Convert.ToString(rec.survey_exp_date);
            //        }
 
            //    }
 
            //}

            //CourseViewModel vm = new CourseViewModel();taylord

            //vm.Course = (from c in _db.COURSEs
            //                where c.session_title_id == 9 && (c.cancel_reason == "Course Completed" || EntityFunctions.AddDays(c.last_end_datetime, 7) < Today)
            //                orderby c.course_id
            //                select c; 

            //vm.CourseStatus = from k in survey_db.COURSE_STATUS
            //                  select k;  
                                  

            int pageSize = 10;
            int pageIndex = (page ?? 1) - 1;;
            return View();
        }

        public ViewResult AnswerDetails(int id)
        {


            var answerDetails = from a in survey_db.SURVEY_REQUEST_SENT
                                join b in survey_db.ANSWER_SCALE on a.survey_request_sent_id equals b.survey_request_sent_id
                                where a.course_id == id
                                select b;

            return View(answerDetails);

        }

    }
}
