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
            
            var totalCoursesSurveyed = (from c in _db.COURSE_STATUS where c.course_status1 == "S" select c).Count();
            var TotalSent = (from requests in _db.SURVEY_REQUEST_SENT select requests).Count();
            var surveyAnswered = from s in _db.SURVEY_REQUEST_SENT select s.survey_request_sent_id;
            var activesurveys = (from s in _db.SURVEY_REQUEST_SENT where s.status_flag != "X" select s.survey_request_sent_id).Count();
            var TotalAnswered = (from answers in _db.ANSWERs where surveyAnswered.Contains(answers.survey_request_sent_id) select answers.survey_request_sent_id).Distinct().Count();

            ViewBag.TotalCourses = totalCoursesSurveyed;
            ViewBag.Answered = TotalAnswered;
            ViewBag.Sent = TotalSent;
            ViewBag.Active = activesurveys;
            ViewBag.Overall = (Convert.ToDouble(Overallsatisfaction) / Convert.ToDouble(5)) * 100;
            ViewBag.Other = 100 - ViewBag.Overall;
            ViewBag.OverallPercent = Math.Round((Convert.ToDouble(Overallsatisfaction) / Convert.ToDouble(5)) * 100);
            //calculate the percent answered
            double totpercentAnswered = (Convert.ToDouble(TotalAnswered) / Convert.ToDouble(TotalSent)) * 100;

            ViewBag.totPercent = Math.Round(totpercentAnswered);

            //Get the course id number for all surveys sent
            var courseSurveyded = from c in _db.COURSE_STATUS where c.course_status1 == "S" select c.course_id;

            return View();
       }

        //public ActionResult GetRainfallChartPie()
        //{
        //    var key = new Chart(width: 400, height: 200)
        //        .AddSeries(
        //            chartType: "pie",
        //            legend: "Rainfall",
        //            xValue: new[] { "Jan", "Feb", "Mar", "Apr", "May" },
        //            yValues: new[] { "20", "20", "40", "10", "10" })
        //        .Write();

        //    return null;
        //}

        [Authorize]
        public ActionResult About()
        {

            return View();
        }

    }
}
