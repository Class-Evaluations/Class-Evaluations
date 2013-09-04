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



namespace Survey.Controllers
{
    public class CourseController : Controller
    {
        private CLASSEntities _db = new CLASSEntities();
        private Survey_DBEntities survey_db = new Survey_DBEntities();

        public ActionResult Index(int? course, string currentFilter, string searchString, string Display, bool? Disable, string SearchType, int? page)
        {
            //Changed on 4/10/2013 to default to the view of surveys not sent
            var FirstPass = 0;
            if (string.IsNullOrEmpty(currentFilter) && string.IsNullOrEmpty(searchString) && string.IsNullOrEmpty(Display) && string.IsNullOrEmpty(SearchType))
            { 
                Display = "notSent";
                FirstPass = 1;
            }

            if (FirstPass > 0)
            {
                //go get the data
            }

            //If the checkbox is checked than the user is trying to disable this survey as  do not survey
            //need to check to see if the survey already exists in the database and act accordingly.
            var checkBox = Request.Form["applyChanges"];
            if (!String.IsNullOrEmpty(checkBox))
            {
                if (ModelState.IsValid)
                {
                    string checkbox = Request.Form["applyChanges"];
                    string[] values = checkbox.Split(',');
                    int itemCount = values.Count();
                    while (itemCount > 0)
                    {
                        if (itemCount <= values.Count())
                        {
                             itemCount = itemCount - 1;
                        }
                        int courseNum = Convert.ToInt32(values[itemCount]);

                        // var courseNum = Convert.ToInt32(checkBox);
                        Survey_DBEntities dbContext = new Survey_DBEntities();
                        var rowCount = (from s in survey_db.COURSE_STATUS where s.course_id == courseNum select s).Count();
                        if (rowCount < 1)
                        {
                            //if course id does not exist in course status table then insert
                            COURSE_STATUS courseStatusAdd = new COURSE_STATUS();
                            courseStatusAdd.course_id = courseNum;
                            courseStatusAdd.course_status1 = "N";
                            courseStatusAdd.survey_exp_date = DateTime.Now.Date;

                            dbContext.AddToCOURSE_STATUS(courseStatusAdd);
                            //dbContext.SaveChanges();
                        }
                        else
                        {
                            COURSE_STATUS statusUpdate = dbContext.COURSE_STATUS.FirstOrDefault(s => s.course_id == courseNum);
                            //check to see if the course is already disables and enable it
                            if ((statusUpdate.course_status1).Trim() == "N")
                            {
                                var removeFromCouseStatus = dbContext.COURSE_STATUS.First(s => s.course_id == courseNum);
                                dbContext.COURSE_STATUS.DeleteObject(removeFromCouseStatus);
                                //dbContext.SaveChanges();
                            }
                            else
                            {
                                statusUpdate.course_status1 = "N";
                            }
                            //dbContext.SaveChanges();
                        }
                        dbContext.SaveChanges();
                    }
                }
            }

            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }


            var GreaterThan2013 = Convert.ToDateTime("2013/04/01");
            var surveyedCourses = (from sc in survey_db.COURSE_STATUS
                                   select sc.course_id).ToList();

            var query = new List<COURSE>();
            query = (from c in _db.COURSEs
                     where surveyedCourses.Contains(c.course_id) || (c.last_end_datetime > GreaterThan2013) && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 1) < GreaterThan2013)) && c.course_status_id != "X" && c.session_title_id != 9
                     orderby c.barcode_number
                     select c).ToList();
            //switch (Display)
            //{
            //    case "sent":
            //        //query = SurveysSent(AllCourses()).ToList();
            //        break;
            //    case "inProgress":
            //        //query = SurveysInProgress(AllCourses()).ToList();
            //        break;
            //    case "completed":
            //        //query = SurveysCompleted(AllCourses()).ToList();
            //        break;
            //    case "notSent":
            //        var doNotSend = (from sc in survey_db.COURSE_STATUS
            //                         where sc.course_status1 == "N"
            //                         select sc.course_id).ToList();
            //        break;
            //    case "doNotsent":
            //        query = DoNotSend(AllCourses()).ToList();
            //        break;
            //    default:
            //        break;
            //}

            var SurveyStatus = from k in survey_db.COURSE_STATUS
                               select k;


            var SurveyExp = from x in survey_db.SURVEY_REQUEST_SENT
                            select x;

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            //var CourseDetails = query.ToList();

            //ViewBag.CourseList = CourseDetails;
            ViewBag.onePageOfCourses = query.ToPagedList(pageNumber, 25); // will only contain 25 products max because of the pageSize
            ViewBag.SurveyStatus = SurveyStatus;
            ViewBag.SurveyExp = SurveyExp;
            ViewBag.searchString = searchString;
            ViewBag.display = Display;
            ViewBag.SearchType = SearchType;
            ViewBag.Disable = Disable;
            return View();
        }


        /// <summary>
        /// This is the new code to optimize the time to take to display the course information based on user response
        /// </summary>
        /// <returns></returns>

        //public IEnumerable<SURVEY> surveyedCourses()
        //{
        //    //return from sc in survey_db.COURSE_STATUS 
        //    //        select sc;
        //}

        public IEnumerable<COURSE> AllCourses()
        {
            //LINQ does not recognize Date objects
            var GreaterThan2013 = Convert.ToDateTime("2013/04/01");
            var surveyedCourses = (from sc in survey_db.COURSE_STATUS
                                   select sc.course_id).ToList();

            return (from c in _db.COURSEs
                    where surveyedCourses.Contains(c.course_id) || (c.last_end_datetime > GreaterThan2013) && (c.course_status_id == "C" || (EntityFunctions.AddDays(c.last_end_datetime, 1) < GreaterThan2013)) && c.course_status_id != "X" && c.session_title_id != 9
                    orderby c.barcode_number
                    select c).ToList();
        }

        public IEnumerable<COURSE> SurveysNotSent(IEnumerable<COURSE> surveys)
        {
            var surveyedCourses = (from sc in survey_db.COURSE_STATUS
                                   select sc.course_id).ToList();
            return from s in surveys
                   where !surveyedCourses.Contains(s.course_id)
                   select s;
        }

        public IEnumerable<COURSE> SurveysSent(IEnumerable<COURSE> surveys)
        {
            var surveysSent = (from sc in survey_db.COURSE_STATUS
                               select sc.course_id).ToList();
            return from s in surveys
                   where surveysSent.Contains(s.course_id)
                   select s;
        }
        public IEnumerable<COURSE> DoNotSend(IEnumerable<COURSE> surveys)
        {
            //(from s in survey_db.COURSE_STATUS where s.course_status1 == "N" select s.course_id).ToList();
            var doNotSend = (from sc in survey_db.COURSE_STATUS
                             where sc.course_status1 == "N"
                             select sc.course_id).ToList();
            return from s in surveys
                   where doNotSend.Contains(s.course_id)
                   select s;
        }
        public IEnumerable<COURSE> SurveysCompleted(IEnumerable<COURSE> surveys)
        {
            //var completed = (from a in survey_db.COURSE_STATUS where a.course_status1 == "X" select a.course_id).ToList();
            var surveyCompleted = (from sc in survey_db.COURSE_STATUS
                                   where sc.course_status1 == "X"
                                   select sc.course_id).ToList();
            return from s in surveys
                   where surveyCompleted.Contains(s.course_id)
                   select s;
        }
        public IEnumerable<COURSE> SurveysInProgress(IEnumerable<COURSE> surveys)
        {
            var inProgress = (from a in survey_db.ANSWERs
                              join s in survey_db.SURVEY_REQUEST_SENT on a.survey_request_sent_id equals s.survey_request_sent_id
                              join cs in survey_db.COURSE_STATUS on s.course_id equals cs.course_id
                              where cs.course_status1 == "S"
                              select s.course_id);
            return from s in surveys
                   where inProgress.Contains(s.course_id)
                   select s;
        }

        public ViewResult AnswerDetails(int id)
        {

            Survey_DBEntities context = new Survey_DBEntities();
            //Totals
            var Sent = (from requests in context.SURVEY_REQUEST_SENT where requests.course_id == id select requests).Count();
            var Xdate = from c in survey_db.COURSE_STATUS where c.course_id == id select c.survey_exp_date;
            var surveyAnswered = from s in context.SURVEY_REQUEST_SENT where s.course_id == id select s.survey_request_sent_id;
            var Answered = (from answers in context.ANSWERs where surveyAnswered.Contains(answers.survey_request_sent_id) select answers.survey_request_sent_id).Distinct().Count();

            //Details
            var QuestionsUsed = from answers in context.ANSWERs where surveyAnswered.Contains(answers.survey_request_sent_id) select answers.question_id;
            var Questions = from q in context.QUESTIONs
                            where QuestionsUsed.Contains(q.question_id)
                            from at in context.ANSWER_TYPE.Where(x => q.answer_type_id == x.answer_type_id).DefaultIfEmpty()
                            orderby q.question_id
                            select new ReportQuestions
                            {
                                questionID = q.question_id,
                                questionText = q.question_text,
                                answerType = at.answer_type_name
                            };

            //Average Scale Answer
            var scale = from Ans in context.ANSWERs
                        from AnScale in context.ANSWER_SCALE.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
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
            var multi = from Ans in context.ANSWERs
                        from AnMulti in context.ANSWER_MULTIPLE_CHOICE.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
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
            var ashort = from Ans in context.ANSWERs
                         from AnShort in context.ANSWER_SHORT.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
                         where surveyAnswered.Contains(Ans.survey_request_sent_id) && QuestionsUsed.Contains(Ans.question_id) && AnShort.submitted_answer != ""
                         orderby Ans.question_id, AnShort.submitted_answer
                         select new ReportShort
                         {
                             questionID = Ans.question_id,
                             responseShort = AnShort.submitted_answer,
                             answerType = "Answer_short"
                         };

            //Long
            var along = from Ans in context.ANSWERs
                        from AnLong in context.ANSWER_LONG.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
                        where surveyAnswered.Contains(Ans.survey_request_sent_id) && QuestionsUsed.Contains(Ans.question_id) && AnLong.submitted_answer != ""
                        orderby Ans.question_id, AnLong.submitted_answer
                        select new ReportLong
                        {
                            questionID = Ans.question_id,
                            responseLong = AnLong.submitted_answer,
                            answerType = "Answer_long"
                        };



            //--True/False
            var trueFalse = from Ans in context.ANSWERs
                            from tf in context.ANSWER_TRUE_FALSE.Where(x => Ans.answer_id == x.answer_id).DefaultIfEmpty()
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
            double TotalpercentAnswered;
            //calculate the percent answered
            if (Answered <= 0 || Sent <= 0)
            {
                TotalpercentAnswered = 0;
            }
            else { TotalpercentAnswered = (Convert.ToDouble(Answered) / Convert.ToDouble(Sent)) * 100; }

            ViewBag.Percent = Math.Round(TotalpercentAnswered);



            //return ExcelGenerator.SpreadSheet.Worksheet(ViewBag, "SurveyResults.xls");
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ViewResult DisableCourse(int id)
        {
            return View();
        }

        public ActionResult DataSearch(string conditions)
        {
            var GreaterThan2013 = Convert.ToDateTime("2013/04/01");

            //Show only the course that are greater than 2013 or in the above surveyed list
            var query = conditions;
            return View(query);
        }

    }
}
