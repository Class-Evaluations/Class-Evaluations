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

            var Sent = (from requests in _db.SURVEY_REQUEST_SENT select requests).Count();

            var surveyAnswered = from s in _db.SURVEY_REQUEST_SENT select s.survey_request_sent_id;

            var Answered = (from answers in _db.ANSWERs where surveyAnswered.Contains(answers.survey_request_sent_id) select answers.survey_request_sent_id).Distinct().Count();

            ViewBag.Answered = Answered;
            ViewBag.Sent = Sent;
            //calculate the percent answered
            double percentAnswered = (Convert.ToDouble(Answered) / Convert.ToDouble(Sent)) * 100;

            ViewBag.Percent = Math.Round(percentAnswered);
            return View();
       }

        public ActionResult About()
        {

            return View();
        }
    }
}
