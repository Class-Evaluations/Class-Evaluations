using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.ViewModels;
using Survey.Models;
using System.Data.Objects;
using System.Web.UI.DataVisualization.Charting;

namespace Survey.Controllers
{
    
    public class HomeController : Controller
    {
        private Survey_DBEntities _db = new Survey_DBEntities();

        [Authorize]
        public ActionResult Index()
        {

            var Overallsatisfaction = (from s in _db.ANSWERs 
                                       join d in _db.ANSWER_SCALE on s.answer_id equals d.answer_id
                                       where d.submitted_answer > 0 && s.question_id == 11
                                       select d.submitted_answer).Average();
            
            var totalSurveysExpiredCount = (from c in _db.COURSE_STATUS where c.course_status1 == "X" 
                                       join xc in _db.SURVEY_REQUEST_SENT on c.course_id equals xc.course_id select xc).Count();
            var ListofSurveysExp = (from c in _db.COURSE_STATUS where c.course_status1 == "X"
                                    join xc in _db.SURVEY_REQUEST_SENT on c.course_id equals xc.course_id
                                    select xc.survey_request_sent_id).ToList();
            var TotalSent = (from requests in _db.SURVEY_REQUEST_SENT select requests).Count();
            //var TotalExpiredAnswered = (from XpiredWithAns in _db.ANSWERs where totalSurveysExpired.Contains(XpiredWithAns.survey_request_sent_id) select XpiredWithAns.survey_request_sent_id).Distinct().Count();
            var surveyAnswered = from s in _db.SURVEY_REQUEST_SENT select s.survey_request_sent_id;
            //var activesurveys = (from s in _db.SURVEY_REQUEST_SENT where s.status_flag != "X" select s.survey_request_sent_id).Count();
            var TotalAnswered = (from answers in _db.ANSWERs where ListofSurveysExp.Contains(answers.survey_request_sent_id) select answers.survey_request_sent_id).Distinct().Count();

            //ViewBag.TotalCourses = totalCoursesSurveyed;
            ViewBag.Answered = TotalAnswered;
            //ViewBag.Sent = TotalSent;
            //ViewBag.Active = activesurveys;
            ViewBag.Overall = Math.Round(Overallsatisfaction, 2);


            //ViewBag.Other = Math.Round(5 - ViewBag.Overall);
            var SatPercent = ((Convert.ToDouble(Overallsatisfaction) / Convert.ToDouble(5)))* 100;
            ViewBag.OverallPercent = Math.Round(SatPercent, 2);
            //calculate the percent answered
            ViewBag.TotalAnswerd = TotalAnswered;
            ViewBag.totalSurveysExpiredCount = totalSurveysExpiredCount;

            double totpercentAnswered = (Convert.ToDouble(TotalAnswered) / Convert.ToDouble(totalSurveysExpiredCount)) * 100;

            ViewBag.totPercent = Math.Round(totpercentAnswered);

            //Get the course id number for all surveys sent
            //var courseSurveyded = from c in _db.COURSE_STATUS where c.course_status1 == "S" select c.course_id;

            return View();
       }

        [Authorize]
        public ActionResult About()
        {

            return View();
        }

    }
}
