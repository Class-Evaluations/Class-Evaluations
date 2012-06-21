using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.ViewModels;
using Survey.Models;

namespace Survey.Controllers
{
    
    public class HomeController : Controller
    {
        private Survey_DBEntities _db = new Survey_DBEntities();

        public ActionResult Index()
        {

            var TotalSent = (from requests in _db.SURVEY_REQUEST_SENT select requests).Count();
            var surveyAnswered = from s in _db.SURVEY_REQUEST_SENT select s.survey_request_sent_id;
            var TotalAnswered = (from answers in _db.ANSWERs where surveyAnswered.Contains(answers.survey_request_sent_id) select answers.survey_request_sent_id).Distinct().Count();

            ViewBag.Answered = TotalAnswered;
            ViewBag.Sent = TotalSent;
            //calculate the percent answered
            double totpercentAnswered = (Convert.ToDouble(TotalAnswered) / Convert.ToDouble(TotalSent)) * 100;

            ViewBag.totPercent = Math.Round(totpercentAnswered);

            //Get the course id number for all surveys sent
            var courseSurveyded = from c in _db.COURSE_STATUS where c.course_status1 == "S" select c.course_id;

            return View();
       }

        public ActionResult About()
        {

            return View();
        }
    }
}
