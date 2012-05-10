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
            //Need to check the expiration date to expire the survey
            var SurveyExpiration = from x in survey_db.SURVEY_REQUEST_SENT
                                   select x;
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

        public ViewResult AnswerDetails(int id, int? surveyid)
        {
            surveyid = 1;
            
            var surveySent =  (from s in survey_db.SURVEY_REQUEST_SENT
                          where s.course_id == id 
                          select s).ToList();

            var surveyCount = surveySent.Count();

            var answerDetails = from a in survey_db.SURVEY_QUESTIONS
                                join q in survey_db.QUESTIONs on a.question_id equals q.question_id
                                join an in survey_db.ANSWERs on a.question_id equals an.question_id
                                join at in survey_db.ANSWER_TYPE on an.answer_type_id equals at.answer_type_id
                                join sa in survey_db.ANSWER_SCALE on an.answer_id equals sa.answer_id 
                                where a.survey_id == surveyid
                                orderby sa.survey_request_sent_id ascending
                                select new SurveyReponses
                                {
                                    questionID = a.question_id,
                                    questionText = q.question_text,
                                    answerID = an.answer_id,
                                    answerType = at.answer_type_name,
                                    submittedAnswer = sa.submitted_answer,
                                    surveyRequestID = sa.survey_request_sent_id
                                };

        //iterate through the answerDetails to calculate the answer statistics

            int? currentQuestion = 0;
            foreach (var item in answerDetails)
            {
                currentQuestion = item.questionID;

                switch ((item.answerType).Trim())
                {
                    case "Scale":
                        {
                            var d = from x in survey_db.ANSWER_SCALE
                                    join s in survey_db.ANSWERs on x.answer_id equals s.answer_id
                                    //where surveySent.Contains(x.survey_request_sent_id) && 
                                    where x.submitted_answer > 0 && s.question_id == item.questionID
                                    orderby s.question_id
                                    group x by x.submitted_answer into g
                                    select new ReportData
                                    {
                                        questionID = item.questionID,
                                        questionText = item.questionText,
                                        responseStat = g.Average(x => x.submitted_answer)
                                    };
                        }
                        //ViewBag.questionID = item.questionID;
                        //ViewBag.questionText = item.questionText;
                        //ViewBag.responseStat = ReportData.responseStat;

                        break;
                    case "Multi-Choice":
                        //    {  var d = from x in survey_db.ANSWER_MULTIPLE_CHOICE
                        //                where surveySent.Contains(x.survey_request_sent_id)
                        //                group x by x.submitted_answer into g
                        //                select new {averageAnswer = g.Count(x => x.submitted_answer)};
                        { break; }
                    case "Multi-Choice-multi":
                        //{  var d = from x in survey_db.ANSWER_MULTIPLE_CHOICE
                        //            where surveySent.Contains(x.survey_request_sent_id)
                        //            group x by x.submitted_answer into g
                        //            select new {averageAnswer = g.Average(x => x.submitted_answer)};
                        { break; }
                    case "Answer_long":
                        //{  var d = from x in survey_db.ANSWER_SCALE
                        //            where surveySent.Contains(x.survey_request_sent_id)
                        //            select new {averageAnswer = x.submitted_answer};

                        //    ViewBag.questionID = item.questionID;
                        //    ViewBag.questionText = item.questionText;
                        //    ViewBag.responseStat = x.submitted_answer;

                        { break; }
                    case "Answer_short":
                        //{  var d = from x in survey_db.ANSWER_SCALE
                        //            where surveySent.Contains(x.survey_request_sent_id)
                        //            select new { averageAnswer = x.submitted_answer };

                        //    ViewBag.questionID = item.questionID;
                        //    ViewBag.questionText = item.questionText;
                        //    ViewBag.responseStat = x.submitted_answer;

                        { break; }
                    case "True_False":
                        //{  var d = from x in survey_db.ANSWER_SCALE
                        //            where surveySent.Contains(x.survey_request_sent_id)
                        //            group x by x.submitted_answer into g
                        //            select new {averageAnswer = g.Count(x => x.submitted_answer)};
                        { break; }
                }
            }

            ViewBag.surveyAnswered = surveySent;
            return View(answerDetails);

            //ReportData

            //var categories =
            //    from p in products
            //    group p by p.Category into g
            //    select new { Category = g.Key, AveragePrice = g.Average(p => p.UnitPrice) }; 


        }

    }
}
