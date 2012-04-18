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


            //searching functionality
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    courses = courses.Where(s => s.barcode_number.Contains(searchString));
           // }
            //else
            //{
            //    courses = courses.Where(s => s.course_id > 111460);
                //For testing we cut down the amount of records returned.
                //courses = courses.Where(s => s.cancel_reason = Convert.ToChar('Course Completed')) or (s => s.last_end_datetime > DateTime.Today.AddDays(+7); 
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


            DateTime Today = DateTime.Now.Date;

            var CourseDetails = from c in _db.COURSEs
                                where c.session_title_id == 9 && (c.cancel_reason == "Course Completed" || EntityFunctions.AddDays(c.last_end_datetime, 7) < Today)
                                orderby c.barcode_number descending
                                select c;

            var SurveyStatus = from k in survey_db.COURSE_STATUS
                               select k;

            //CourseViewModel context = new CourseViewModel();

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

            var onePageOfCourses = CourseDetails.ToPagedList(pageNumber, 25); // will only contain 25 products max because of the pageSize
            ViewBag.CourseList = CourseDetails;
            ViewBag.SurveyStatus = SurveyStatus;


            ViewBag.onePageOfCourses = onePageOfCourses;


            return View();
        }

        public ViewResult AnswerDetails(int id, int? survey)
        {
            var surveyid = 1;
            var questionid = 0;

            var surveySent = from s in survey_db.SURVEY_REQUEST_SENT
                             where s.course_id == id
                             select new { surveyid = s.survey_id };


            var answerDetails = from a in survey_db.SURVEY_QUESTIONS
                                join q in survey_db.QUESTIONs on a.question_id equals q.question_id
                                where a.survey_id == surveyid
                                orderby a.question_id ascending
                                select new SurveyReponses
                                {
                                    questionID = a.question_id,
                                    questionText = q.question_text
                                };

            return View(answerDetails);

//Select	l.answer_id,
//        l.submitted_answer as LongAnswer, 
//        s.answer_id,
//        s.submitted_answer as ShortAnswer,
//        s.answer_id,
//        mc.submitted_answer as Multiple_CHIOCE 
//from Survey_DB.dbo.ANSWER_LONG l
//join Survey_DB.dbo.ANSWER_SHORT s ON s.survey_request_sent_id = l.survey_request_sent_id  
//join Survey_DB.dbo.ANSWER_MULTPLE_CHOICE mc ON s.survey_request_sent_id = mc.survey_request_sent_id  
//where l.survey_request_sent_id in (83,84)

        }

    }
}
