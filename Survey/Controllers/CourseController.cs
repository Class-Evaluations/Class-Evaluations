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
using Survey.Helper;


namespace Survey.Controllers
{
    public class CourseController : Controller
    {
        private CLASSEntities _db = new CLASSEntities();
        private Survey_DBEntities survey_db = new Survey_DBEntities();

        //[AcceptVerbs(HttpVerbs.Post)]
        public ViewResult Index(string currentFilter, string searchString, string Display, string SearchType, bool? Disable, int? page)
        {
            //sorting functionality
            DateTime Today = DateTime.Now.Date;

            if (Request.HttpMethod == "GET")
            {
                //searchString = currentFilter;
            }
            else
            {
                page = 1;
            }

            //if (!String.IsNullOrEmpty(Convert.ToString(Disable)))
            //{
            //    if (Disable == true)
            //    {
                   
            //    }
            //}

            //statusIDs = X=cancelled, A=active, I=incomplete and c=complete
            //Need to start the barcodes >= 120350 which is course is 120965 
            var query = from c in _db.COURSEs
                        where c.course_id > 120965 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X")
                        orderby c.barcode_number
                        select c;

            //searching functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                if (SearchType == "Barcode")
                {
                    query = from c in _db.COURSEs where c.course_id > 120965 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") && c.barcode_number == searchString orderby c.barcode_number select c;
                }
                if (SearchType == "Title")
                {

                    query = from c in _db.COURSEs where c.course_id > 120965 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") && (c.title).Contains(searchString) orderby c.barcode_number select c;
                }
            }

            switch (Display)
            {
                case "sent":
                    var sent = (from s in survey_db.COURSE_STATUS where s.course_status1 == "S" select s.course_id).ToList();

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        if (SearchType == "Barcode")
                        {
                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (sent).Contains(c.course_id) && c.barcode_number == searchString
                                    orderby c.barcode_number
                                    select c;

                        }
                        if (SearchType == "Title")
                        {

                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (sent).Contains(c.course_id) && (c.title).Contains(searchString)
                                    orderby c.barcode_number
                                    select c;

                        }
                    }
                    else
                    {
                        query = from c in _db.COURSEs
                                where c.course_id > 120965 && (sent).Contains(c.course_id)
                                orderby c.barcode_number
                                select c;
                    }
                    break;

                case "inProgress":
                    var inProgress = (from a in survey_db.ANSWERs
                                    join s in survey_db.SURVEY_REQUEST_SENT on a.survey_request_sent_id equals s.survey_request_sent_id
                                    join cs in survey_db.COURSE_STATUS on s.course_id equals cs.course_id
                                    where cs.course_status1 == "S"
                                    select s.course_id).ToList();


                    if (!String.IsNullOrEmpty(searchString))
                    {
                        if (SearchType == "Barcode")
                        {
                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (inProgress).Contains(c.course_id) && c.barcode_number == searchString
                                    orderby c.barcode_number
                                    select c;

                        }
                        if (SearchType == "Title")
                        {

                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (inProgress).Contains(c.course_id) && (c.title).Contains(searchString)
                                    orderby c.barcode_number
                                    select c;

                        }
                    }
                    else
                    {
                        query = from c in _db.COURSEs
                                where c.course_id > 120965 && (inProgress).Contains(c.course_id) 
                                orderby c.barcode_number
                                select c;
                    }
                    break;

                case "completed":
                    var completed = (from a in survey_db.COURSE_STATUS
                                    where a.course_status1 == "X"
                                    select a.course_id).ToList();

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        if (SearchType == "Barcode")
                        {
                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (completed).Contains(c.course_id) && c.barcode_number == searchString
                                    orderby c.barcode_number
                                    select c;

                        }
                        if (SearchType == "Title")
                        {

                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (completed).Contains(c.course_id) && (c.title).Contains(searchString)
                                    orderby c.barcode_number
                                    select c;

                        }
                    }
                    else
                    {
                        query = from c in _db.COURSEs
                                where c.course_id > 120965 && (completed).Contains(c.course_id)
                                orderby c.barcode_number
                                select c;
                    }

                    break;

                case "notSent":  //not finished
                    var notSent = (from s in survey_db.COURSE_STATUS where s.course_status1 == "S" select s.course_id).ToList();

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        if (SearchType == "Barcode")
                        {
                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") && !(notSent).Contains(c.course_id) && c.barcode_number == searchString
                                    orderby c.barcode_number
                                    select c;


                        }
                        if (SearchType == "Title")
                        {
                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") && !(notSent).Contains(c.course_id) && (c.title).Contains(searchString)
                                    orderby c.barcode_number
                                    select c;

                        }
                    }
                    else
                    {
                        query = from c in _db.COURSEs
                                where c.course_id > 120965 && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 7) < Today) && c.course_status_id != "X") && !(notSent).Contains(c.course_id)
                                orderby c.barcode_number
                                select c;
                    }


                    break;
                case "doNotsent":  //not finished
                    var doNotsent = (from s in survey_db.COURSE_STATUS where s.course_status1 == "N" select s.course_id).ToList();

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        if (SearchType == "Barcode")
                        {
                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (doNotsent).Contains(c.course_id) && c.barcode_number == searchString
                                    orderby c.barcode_number
                                    select c;  

                        }
                        if (SearchType == "Title")
                        {

                            query = from c in _db.COURSEs
                                    where c.course_id > 120965 && (doNotsent).Contains(c.course_id) && (c.title).Contains(searchString)
                                    orderby c.barcode_number
                                    select c;  

                        }
                    }
                    else
                    {
                        query = from c in _db.COURSEs
                                where c.course_id > 120965 && (doNotsent).Contains(c.course_id)
                                orderby c.barcode_number
                                select c;
                    }

                    break;
                default:
                    break;
            }


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
            var CourseDetails = query.ToList();

            ViewBag.CourseList = CourseDetails;
            ViewBag.onePageOfCourses = CourseDetails.ToPagedList(pageNumber, 25); // will only contain 25 products max because of the pageSize
            ViewBag.SurveyStatus = SurveyStatus;
            ViewBag.SurveyExp = SurveyExp;
            ViewBag.searchString = searchString;
            ViewBag.display = Display;
            ViewBag.SearchType = SearchType;
            ViewBag.Disable = Disable;
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
            double TotalpercentAnswered = (Convert.ToDouble(Answered) / Convert.ToDouble(Sent)) * 100;
            ViewBag.Percent = Math.Round(TotalpercentAnswered);

            return View();
        }

    }
}
