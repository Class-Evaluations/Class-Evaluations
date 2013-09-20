using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Evaluations.Models;
using System.Data.Objects;
using System.IO;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Linq;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Evaluations.ViewModel;


namespace Evaluations.Controllers
{ 
    public class CourseController : Controller
    {
        const int RecordsPerPage = 25;
        private MongoCollection<Courses> _course;
        Survey_DB_20Entities dbContext = new Survey_DB_20Entities();

        public CourseController()
        {
            MongoDatabase database = MongoDatabase.Create("mongodb://localhost:27017/MongoClassDb");
            _course = database.GetCollection<Courses>("Courses");
        }

        public ActionResult Index (string course, int? page, string SearchString, string SearchType, string Disable, string Display)
        {
            var pageNumber = page ?? 1;
            var PageTitle = "";
            DateTime Today = DateTime.Now.Date;


            //if the checkbox is check disable/enable the course
            var checkBox = Request.Form["applyChanges"];
            if (!String.IsNullOrEmpty(checkBox))
            {
                if (ModelState.IsValid)
                {
                    string[] values = checkBox.Split(',');
                    int itemCount = values.Count();

                    while (itemCount > 0)
                    {
                        if (itemCount <= values.Count())
                        {
                            itemCount = itemCount - 1;
                        }
                        int courseNum = Convert.ToInt32(values[itemCount]);

                        // var courseNum = Convert.ToInt32(checkBox);
                        var rowCount = (from s in dbContext.COURSE_STATUS where s.course_id == courseNum select s).Count();
                        if (rowCount < 1)
                        {
                            //add a row to course status table
                            COURSE_STATUS courseStatusAdd = new COURSE_STATUS();
                            courseStatusAdd.course_id = courseNum;
                            courseStatusAdd.course_status1 = "N";
                            courseStatusAdd.survey_exp_date = Today;
                            dbContext.AddToCOURSE_STATUS(courseStatusAdd);
                            dbContext.SaveChanges();

                            //Update the status in the MongoDB
                            
                        }
                        else
                        {
                            COURSE_STATUS statusUpdate = dbContext.COURSE_STATUS.FirstOrDefault(s => s.course_id == courseNum);
                            //check to see if the course is already disables and enable it
                            if ((statusUpdate.course_status1).Trim() == "N")
                            {
                                var removeFromCouseStatus = dbContext.COURSE_STATUS.First(s => s.course_id == courseNum);
                                dbContext.COURSE_STATUS.DeleteObject(removeFromCouseStatus);
                                dbContext.SaveChanges();
                            }
                            else
                            {
                                statusUpdate.course_status1 = "N";
                            }
                            dbContext.SaveChanges();
                        }
                    }

                }

            }

            //Default to the courses that have not been survyed
            if (String.IsNullOrEmpty(Display))
            {
                Display = "all";
                PageTitle = "Course that need to be surveyed";
            }

            //Check to see if there is a SearchString
            if (!String.IsNullOrEmpty(SearchString))
            {
                if (SearchType == "Barcode")
                {
                    //SearchBarcode(SearchString);
                    PageTitle = "Search for barcode = " + SearchString;
                }
                else
                {
                    //SearchTitle(SearchString);
                    PageTitle = "Search for courses with " + SearchString + " in the title";
                }

            }

            switch (Display)
            {
                case "all":
                    AllSurveys(SearchString);
                    PageTitle = "All courses that ended after April 1, 2013";
                    break;
                case "sent":
                     DisplaySent(SearchString);
                     PageTitle = "All courses that have a survey sent";
                     break;
                case "inProgress":
                    DisplayInProcess(SearchString);
                    PageTitle = "Courses that the survey is in progress";
                    break;
                case "completed":
                    DisplayCompleted(SearchString);
                    PageTitle = "Courses that the survey is complete";
                    break;
                case "notSent":
                    DisplayNotSent(SearchString);
                    PageTitle = "Courses that have not been sent";
                    break;
                case "doNotsent":
                    DisplayNotSent(SearchString);
                    PageTitle = "Courses that are not to be surveyed";
                    break;
                default:
                    break;
            }

            
            //Set the page view property for display
            ViewBag.searchString = SearchString;
            ViewBag.display = Display;
            ViewBag.SearchType = SearchType;
            ViewBag.Disable = Disable;
            ViewBag.PageTitle = PageTitle;

            return View(_course.AsQueryable().ToPagedList(pageNumber, RecordsPerPage));
        }

        //search record by barcode
        public ViewResult SearchBarcode(string SearchString)
        {
            //var courseResults = (from e in _course.AsQueryable() where e.Barcode == SearchString select e).ToList();
            //var courseResults = _course.FindOne("CourseId":SearchString);
            return View();
        }

        //search record by title
        public ViewResult SearchTitle(string SearchString)
        {
            var courseResults = (from e in _course.AsQueryable<Courses>() where e.ActivityTitle.Contains(SearchString) || e.CourseTitle.Contains(SearchString) select e).ToList();
            return View(courseResults);
        }

        //display all courses
        public ViewResult AllSurveys(string SearchString)
        {
            var courseResults = _course.FindAll();
            return View(courseResults);
        }        
        
        //display courses that the survey has been sent
        public ViewResult DisplaySent(string SearchString)
        {
            var courseResults = (from c in _course.AsQueryable().Where (c=> c.SurveyStatus == "S") select c).ToList();      
            //var courseResults = _course.Find(where SurveyStatus=="S" );
            return View(courseResults);
        }        
        
        //display courses that the survey is in progress
        public ViewResult DisplayInProcess(string SearchString)
        {
            var courseResults = (from e in _course.AsQueryable<Courses>() where e.SurveyStatus == "S" select e).ToList();
            return View(courseResults);
        }        
        
        //display courses that the survey is completed
        public ViewResult DisplayCompleted(string SearchString)
        {
            var courseResults = (from e in _course.AsQueryable<Courses>() where e.SurveyStatus == "X" select e).ToList();
            return View(courseResults);
        }        
        
        //display courses that have not had a survey sent
        public ViewResult DisplayNotSent(string SearchString)
        {
            var courseResults = (from e in _course.AsQueryable<Courses>() where e.SurveyStatus == "" select e).ToList();
            return View(courseResults);
        }        
        
        //display courses that are not to be surveyed
        public ViewResult DisplayDoNotSurvey(string SearchString)
        {
            var courseResults = (from e in _course.AsQueryable<Courses>() where e.SurveyStatus == "N" select e).ToList();
            return View(courseResults);
        }

        //display courses that are not to be surveyed
        public ViewResult SurveyDisableEnable(string SearchString)
        {
            var courseResults = (from e in _course.AsQueryable<Courses>() where e.SurveyStatus == "N" select e).ToList();
            return View(courseResults);
        }


        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            //base.Dispose(disposing);
        }
    }
}