using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;
using System.Data.EntityModel;
using Survey.ViewModels;
using System.Web.Helpers;

namespace Survey.Controllers
{
    public class ReportsController : Controller
    {
        private CLASSEntities _db = new CLASSEntities();
        private Survey_DBEntities survey_db = new Survey_DBEntities();

        //
        // GET: /Reports/

        public ActionResult Index()
        {
            
            var surveyedCourses = (from c in survey_db.COURSE_STATUS where c.course_status1 == "S" || c.course_status1 == "X" select c.course_id).ToList();

            ViewBag.sups = new SelectList((from s in _db.SUPERVISORs.Where(s => s.PERSON.first_name == "Supervisor").ToList() select new { ID =s.PERSON.person_id, FullName = s.PERSON.last_name + " " + s.PERSON.first_name}), "ID", "FullName");
            ViewBag.location = new SelectList((from f in _db.FACILITY_MASTER.ToList() select new { ID = f.facility_master_id, Location = f.title}), "ID", "Location");
            ViewBag.activity = new SelectList((from c in _db.COURSEs.Where(c => surveyedCourses.Contains(c.course_id)).ToList().Distinct() select new { ID = c.activity_id, Activity = c.ACTIVITY.title }), "ID", "Activity");

            return View();
        }

        public ViewResult AnswerDetails(int? id, string txtBarcode, string txtFromDate, string txtToDate, int? person_id, int? location, int? activity, string title)
        {
            //id = Convert.ToInt32(txtBarcode);
            var surveyedCourses = (from c in survey_db.COURSE_STATUS where c.course_status1 == "S" || c.course_status1 == "X" select c.course_id).ToList();
            var courselist = (from c in _db.COURSEs where c.supervisor == person_id && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();

            //Create the query based on conditions selected by user if barcode is enter all other options are ingored
            if (!string.IsNullOrEmpty(txtBarcode))
            {
                courselist = (from c in _db.COURSEs where (c.barcode_number == txtBarcode) select c.course_id).ToList();
            }
            else
            {
                if ((!string.IsNullOrEmpty(txtFromDate)) && (!String.IsNullOrEmpty(txtToDate)))
                {
                    var FromDate = Convert.ToDateTime(txtFromDate);
                    var ToDate = Convert.ToDateTime(txtToDate);
                    ToDate = ToDate.AddDays(1);
                    courselist = (from c in _db.COURSEs where c.last_end_datetime > FromDate && c.last_end_datetime < ToDate && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();

                    if (person_id > 0)
                    {
                        if (location > 0)
                        {
                            if (activity > 0)
                            {
                                courselist = (from c in _db.COURSEs where c.last_end_datetime > FromDate && c.last_end_datetime < ToDate && c.activity_id == activity && c.SUPERVISOR1.facility_master_id == location && c.SUPERVISOR1.person_id == person_id && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                            }
                            else
                            {
                                courselist = (from c in _db.COURSEs where c.last_end_datetime > FromDate && c.last_end_datetime < ToDate && c.SUPERVISOR1.facility_master_id == location && c.SUPERVISOR1.person_id == person_id && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                            }
                        }
                        else if ((person_id > 0) && (activity > 0))
                        {
                            courselist = (from c in _db.COURSEs where c.last_end_datetime > FromDate && c.last_end_datetime < ToDate && c.SUPERVISOR1.person_id == person_id && c.activity_id == activity && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                        }
                        else
                        {
                            courselist = (from c in _db.COURSEs where c.last_end_datetime > FromDate && c.last_end_datetime < ToDate && c.SUPERVISOR1.person_id == person_id && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                        }
                    }
                    else if (location > 0)
                    {
                        if (activity > 0)
                        {
                            courselist = (from c in _db.COURSEs where c.last_end_datetime > FromDate && c.last_end_datetime < ToDate && c.SUPERVISOR1.facility_master_id == location && c.activity_id == activity && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                        }
                        else
                        {
                            courselist = (from c in _db.COURSEs where c.last_end_datetime > FromDate && c.last_end_datetime < ToDate && c.SUPERVISOR1.facility_master_id == location && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                        }
                    }

                }
                else
                {
                    if (person_id > 0)
                    {
                        if (location > 0)
                        {
                            if (activity > 0)
                            {
                                courselist = (from c in _db.COURSEs where c.activity_id == activity && c.SUPERVISOR1.facility_master_id == location && c.SUPERVISOR1.person_id == person_id && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                            }
                            else
                            {
                                courselist = (from c in _db.COURSEs where c.SUPERVISOR1.facility_master_id == location && c.SUPERVISOR1.person_id == person_id && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                            }
                        }
                        else if ((person_id > 0) && (activity > 0))
                        {
                            courselist = (from c in _db.COURSEs where c.SUPERVISOR1.person_id == person_id && c.activity_id == activity && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                        }
                        else
                        {
                            courselist = (from c in _db.COURSEs where c.SUPERVISOR1.person_id == person_id && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                        }
                    }
                    else if (location > 0)
                    {
                        if (activity > 0)
                        {
                            courselist = (from c in _db.COURSEs where c.SUPERVISOR1.facility_master_id == location && c.activity_id == activity && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                        }
                        else
                        {
                            courselist = (from c in _db.COURSEs where c.SUPERVISOR1.facility_master_id == location && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                        }
                    }
                    else
                    {
                        courselist = (from c in _db.COURSEs where c.activity_id == activity && surveyedCourses.Contains(c.course_id) select c.course_id).ToList();
                    }
                }
            }
            //Totals
            //id = Convert.ToInt32(txtBarcode);
            var Sent = (from requests in survey_db.SURVEY_REQUEST_SENT where courselist.Contains(requests.course_id) select requests).Count();
            var surveyCount = courselist.Count();
            var surveyAnswered = from s in survey_db.SURVEY_REQUEST_SENT where courselist.Contains(s.course_id) select s.survey_request_sent_id;
            var Answered = (from answers in survey_db.ANSWERs where surveyAnswered.Contains(answers.survey_request_sent_id) select answers.survey_request_sent_id).Distinct().Count();

            //Details
            var QuestionsUsed = from answers in survey_db.ANSWERs where surveyAnswered.Contains(answers.survey_request_sent_id) select answers.question_id;
            var Questions = from q in survey_db.QUESTIONs
                            where QuestionsUsed.Contains(q.question_id)
                            from at in survey_db.ANSWER_TYPE.Where(x => q.answer_type_id == x.answer_type_id).DefaultIfEmpty()
                            orderby q.question_id
                            select new ReportQuestions
                            {
                                questionID = q.question_id,
                                questionText = q.question_text,
                                answerType = at.answer_type_name
                            };

            //Average Scale Answer
            var scale = from Ans in survey_db.ANSWERs
                        from AnScale in survey_db.ANSWER_SCALE.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
                        where surveyAnswered.Contains(Ans.survey_request_sent_id) && QuestionsUsed.Contains(Ans.question_id) && AnScale.submitted_answer > 0
                        orderby Ans.question_id
                        group new { Ans.question_id, AnScale.submitted_answer } by new { Ans.question_id }
                            into grp
                            let numItems = grp.Select(x => x.submitted_answer).Average()
                            select new ReportScale
                            {
                                questionID = grp.Key.question_id,
                                responseScale = numItems,
                                answerType = "Scale"
                            };

            //Multi Choice &  Multi-Choice-multi Counts 
            var multi = from Ans in survey_db.ANSWERs
                        from AnMulti in survey_db.ANSWER_MULTIPLE_CHOICE.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
                        where surveyAnswered.Contains(Ans.survey_request_sent_id) && QuestionsUsed.Contains(Ans.question_id) && AnMulti.submitted_answer != ""
                        orderby Ans.question_id
                        group new { Ans.question_id, AnMulti.submitted_answer } by new { Ans.question_id, AnMulti.submitted_answer }
                            into grp
                            let ansCount = grp.Select(x => x.submitted_answer).Count()
                            select new ReportMultiChoice
                            {
                                questionID = grp.Key.question_id,
                                count = ansCount,
                                responseMultiChoice = grp.Key.submitted_answer,
                                answerType = "Multi-Choice"
                            };

            //Short
            var ashort = from Ans in survey_db.ANSWERs
                         from AnShort in survey_db.ANSWER_SHORT.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
                         where surveyAnswered.Contains(Ans.survey_request_sent_id) && QuestionsUsed.Contains(Ans.question_id) && AnShort.submitted_answer != ""
                         orderby Ans.question_id, AnShort.submitted_answer
                         select new ReportShort
                         {
                             questionID = Ans.question_id,
                             responseShort = AnShort.submitted_answer,
                             answerType = "Answer_short"
                         };

            //Long
            var along = from Ans in survey_db.ANSWERs
                        from AnLong in survey_db.ANSWER_LONG.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
                        where surveyAnswered.Contains(Ans.survey_request_sent_id) && QuestionsUsed.Contains(Ans.question_id) && AnLong.submitted_answer != ""
                        orderby Ans.question_id, AnLong.submitted_answer
                        select new ReportLong
                        {
                            questionID = Ans.question_id,
                            responseLong = AnLong.submitted_answer,
                            answerType = "Answer_long"
                        };



            //--True/False
            var trueFalse = from Ans in survey_db.ANSWERs
                            from tf in survey_db.ANSWER_TRUE_FALSE.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
                            where surveyAnswered.Contains(Ans.survey_request_sent_id) && QuestionsUsed.Contains(Ans.question_id)
                            orderby Ans.question_id
                            group new { Ans.question_id, tf.submitted_answer } by new { Ans.question_id, tf.submitted_answer }
                                into grp
                                let ansCount = grp.Select(x => x.submitted_answer).Distinct().Count()
                                select new ReportTF
                                {
                                    questionID = grp.Key.question_id,
                                    count = ansCount,
                                    responseTF = grp.Key.submitted_answer,
                                    answerType = "True_False"
                                };

            //Report Title
            ViewBag.Title = title;

           
             //Question data entity
            ViewBag.questionText = Questions;

            //Answer entity
            ViewBag.Scale = scale;
            ViewBag.Short = ashort;
            ViewBag.Long = along;
            ViewBag.TF = trueFalse;
            ViewBag.Multi = multi;

            //ViewBag.ExpirationDate = Xdate;
            ViewBag.Answered = Answered;
            ViewBag.Sent = Sent;
            ViewBag.surveyCount = surveyCount;
            double TotalpercentAnswered = 0;
            //calculate the percent answered
             if (Answered <= 0 ||  Sent <= 0)
             {
                 TotalpercentAnswered = 0;
             }
             else{TotalpercentAnswered = (Convert.ToDouble(Answered) / Convert.ToDouble(Sent)) * 100;}    
             ViewBag.Percent = Math.Round(TotalpercentAnswered);
             ViewBag.NoResponse = (Convert.ToDouble(Sent) - Convert.ToDouble(Answered));
             return View();


        }        //
        // POST: /Reports/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        //Reporting
        [HttpPost]
        public void ShowGenericRptInNewWin(string FromDate, string ToDate, int survey)
        {
            //this.HttpContext.Session["ReportName"] = "generic.rpt";
            this.HttpContext.Session["rptFromDate"] = FromDate;
            this.HttpContext.Session["rptToDate"] = ToDate;
            this.HttpContext.Session["survey_id"] = survey;
            this.HttpContext.Session["ReportName"] = "generic.rpt";
            //this.HttpContext.Session["rptSource"] = GetStudents();

        }

        public ActionResult Chart()
        {
            var bytes = new Chart(width: 400, height: 200)
                .AddSeries(
                    chartType: "bar",
                    xValue: new[] {"Math", "English", "Computer", "Urdu"},
                    yValues: new[] {"60", "70", "68", "88"})
                .GetBytes("png");
            return File(bytes, "image/png");
        }

        public ActionResult GenerateChart(string sent, string answered)
        {
            var key = new Chart(width: 600, height: 400)
                .AddSeries(
                    chartType: "area",
                    legend: "Rainfall",
                    xValue: new[] { "Sent", "Answered" },
                    yValues: new[] { sent, answered })
                .Write("png");

            return null;
        }

      }
    }
