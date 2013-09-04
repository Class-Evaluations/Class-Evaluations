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


namespace Evaluations.Controllers
{ 
    public class CourseController : Controller
    {
        private MongoCollection<Courses> _course;

        public CourseController(int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)

            MongoDatabase database = MongoDatabase.Create("mongodb://localhost:27017/MongoClassDb");
            _course = database.GetCollection<Courses>("Courses");
            _course.ToPagedList(pageNumber, 25);
        }

        public ActionResult Index()
        {

            return View(_course.AsQueryable());            
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