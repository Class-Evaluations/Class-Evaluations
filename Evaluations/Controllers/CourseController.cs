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
using System.Runtime.Caching;
using System.IO;
using Evaluations.Models;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using FluentMongo.Linq;
using Evaluations.ViewModel;


namespace Evaluations.Controllers
{ 
    public class CourseController : Controller
    {
        const int RecordsPerPage = 25; 
        private MongoCollection<Courses> _course;

        public CourseController()
        {

            MongoDatabase database = MongoDatabase.Create("mongodb://localhost:27017/MongoClassDb");
            _course = database.GetCollection<Courses>("Courses");
        }

        //public ActionResult Index(SearchResult model)


        public ActionResult Index (string course, int? page, string SearchString, string SearchType, string Disable, string Display)
        {
            var pageNumber = page ?? 1;
            //if (!string.IsNullOrEmpty(model.SearchButton) || model.Page.HasValue)
            //{
            //   //model.SearchResults = results.ToPagedList(pageIndex, 25);

            //}
            var onePageOfCourse = _course.AsQueryable().ToPagedList(pageNumber, 25); // will only contain 25 people max because of the pageSize
            ViewBag.onePageOfCourse = onePageOfCourse;
            ViewBag.SurveyStatus = "S";
            ViewBag.SurveyExp = "Now";
            ViewBag.searchString = SearchString;
            ViewBag.display = Display;
            ViewBag.SearchType = SearchType;
            ViewBag.Disable = Disable;

            return View(_course.AsQueryable().ToPagedList(pageNumber, 25));
            //return View();

        }


        //
        // GET: /Course/Details/5

        public ViewResult Details(int id)
        {
            //COURSE course = db.COURSEs.Single(c => c.course_id == id);
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            //db.Dispose();
            //base.Dispose(disposing);
        }
    }
}