using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;
using PagedList;
using PagedList.Mvc;
using Survey.ViewModels;
using System.Data.Objects;
using System.Data.EntityModel;
using Survey.App_Data;


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
            DateTime Today = DateTime.Now.Date;
            ViewBag.CurrentSort = sortOrder;
            //ViewBag.searchString = String.IsNullOrEmpty(searchString) ? "Search for Barcode" : "";
            ViewBag.ActivitySortParam = String.IsNullOrEmpty(sortOrder) ? "Activity Desc" : "";
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title Desc" : "";
            ViewBag.CourseSortParm = String.IsNullOrEmpty(sortOrder) ? "Course Desc" : "";
            ViewBag.BarcodeSortParm = String.IsNullOrEmpty(sortOrder) ? "Barcode Desc" : "";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            //statusIDs = X=cancelled, A=active, I=incomplete and c=complete
            //Need to start the barcodes >= 120350
            var query = from c in _db.COURSEs where c.course_id > 120350 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") select c;
            
            //searching functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                query = from c in _db.COURSEs where c.course_id > 120350 && (c.course_status_id == "C" || EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.barcode_number == searchString select c;
            }

            //switch (sortOrder)
            //{
            //    case "Last Name Desc":

            
            //        query = query.OrderByDescending(s => s.last_name);
            //        break;
            //    case "Email Desc":
            //        query = query.OrderByDescending(s => s.email_address);
            //        break;
            //    default:
            //        query = query.OrderBy(s => s.person_id);
            //        break;
            //}
            var CourseDetails = query.ToList();

            //searching functionality
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    courses = courses.Where(s => s.barcode_number.Contains(searchString));
            //}
            //else
            //{
            //    courses = courses.Where(s => s.course_id > 111460);
            //    For testing we cut down the amount of records returned.
            //    courses = courses.Where(s => s.cancel_reason = Convert.ToChar('Course Completed')) or (s => s.last_end_datetime > DateTime.Today.AddDays(+7); 
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

            //Need to check the expiration date to expire the survey
            var SurveyExpiration = from x in survey_db.SURVEY_REQUEST_SENT
                                   select x;

            if (SurveyExpiration.Count() > 0)
            {
                foreach (var item in SurveyExpiration)
                {
                    if (item.expiration_date < DateTime.Now.Date)
                    {
                        item.status_flag = "X";
                    }
                }

                // Submit the changes to the database.
                try
                {
                    survey_db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                }
            }

            var SurveyStatus = from k in survey_db.COURSE_STATUS
                               select k;


            var SurveyExp = from x in survey_db.SURVEY_REQUEST_SENT
                                   select x;

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

            
            var onePageOfCourses = CourseDetails.ToPagedList(pageNumber, 25); // will only contain 25 products max because of the pageSize
            ViewBag.CourseList = CourseDetails;
            ViewBag.SurveyStatus = SurveyStatus;
            ViewBag.SurveyExp = SurveyExp;


            ViewBag.onePageOfCourses = onePageOfCourses;

            return View();
        }

        public ViewResult AnswerDetails(int id)
        {

            var Sent = (from requests in survey_db.SURVEY_REQUEST_SENT where requests.course_id == id select requests).Count();

            var surveyAnswered = from s in survey_db.SURVEY_REQUEST_SENT where s.course_id == id select s.survey_request_sent_id;

            var Answered = (from answers in survey_db.ANSWERs where surveyAnswered.Contains(answers.survey_request_sent_id) select answers.survey_request_sent_id).Distinct().Count();

            ViewBag.Answered = Answered;
            ViewBag.Sent = Sent;
            //calculate the percent answered
            double percentAnswered = (Convert.ToDouble(Answered) / Convert.ToDouble(Sent)) * 100;

            ViewBag.Percent = Math.Round(percentAnswered);
            return View();
        }

    }
}
