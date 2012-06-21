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

        //[AcceptVerbs(HttpVerbs.Post)]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, string Display, string SearchType, int? page)
        {
            //sorting functionality
            DateTime Today = DateTime.Now.Date;

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
            var query = from c in _db.COURSEs where c.course_id > 120350 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") orderby c.barcode_number select c;
            
            //searching functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                if (SearchType == "Barcode")
                {
                    query = from c in _db.COURSEs where c.course_id > 120350 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") && c.barcode_number == searchString orderby c.barcode_number select c;
                }
                if (SearchType == "Title")
                {
                    query = from c in _db.COURSEs where c.course_id > 120350 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") && (c.title).Contains(searchString) orderby c.barcode_number select c;
                }
            }


            //switch (Display)
            //{
            //    case "sent":
            //        query = from c in _db.COURSEs where c. == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") orderby c.barcode_number select c;
            //        break;
            //    case "inProgress":
            //        courses = courses.OrderByDescending(s => s.activity_id);
            //        break;
            //    case "completed":
            //        courses = courses.OrderByDescending(s => s.course_id);
            //        break;
            //    case "notSent":
            //        courses = courses.OrderByDescending(s => s.course_id);
            //        break;
            //    case "doNotsent":
            //        courses = courses.OrderByDescending(s => s.course_id);
            //        break;
            //    default:
            //        courses = courses.OrderBy(s => s.course_id);
            //        break;
            //}

            var CourseDetails = query.ToList();

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

            ViewBag.Answered = Sent;
            ViewBag.Sent = Answered;
            //calculate the percent answered
            double TotalpercentAnswered = (Convert.ToDouble(Answered) / Convert.ToDouble(Sent)) * 100;
            ViewBag.Percent = Math.Round(TotalpercentAnswered);

            return View();
        }

    }
}
