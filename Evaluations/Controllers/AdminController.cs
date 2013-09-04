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
using MongoDB.Driver.Builders;
using FluentMongo.Linq;

namespace Evaluations.Controllers
{
    public class AdminController : Controller
    {
        private CLASSEntities _db = new CLASSEntities();
        private MongoCollection<Courses> _course;
        private Survey_DB_20Entities _survey = new Survey_DB_20Entities();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadMongoDB()
        {

            //get the courses from Class that course end date is greater that enter value
            var GreaterThan2013 = Convert.ToDateTime("2013/04/01");
            var courseList = (from c in _db.COURSEs
                              where (c.last_end_datetime > GreaterThan2013) && c.course_status_id == "C" && c.course_status_id != "X" && c.session_title_id != 9
                              orderby c.barcode_number
                              select c).ToList();

            //get the survey sent table to import the survey information
            var survyeList = (from s in _survey.SURVEY_REQUEST_SENT select s).ToList();

            MongoDatabase database = MongoDatabase.Create("mongodb://localhost:27017/MongoClassDb");
            _course = database.GetCollection<Courses>("Courses");
  
            Courses Courses = new Courses();

            //Delete the MongoDb Collection first to proceed with data insertion
            if (database.CollectionExists("Courses"))
            {
                MongoCollection<Courses> collection = database.GetCollection<Courses>("Courses");
                collection.Drop();
            }

            FileStream fs = new FileStream("c:\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.OpenOrCreate);
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            String strng;
            Byte[] bytes;
            int count = 0;

            foreach (var item in courseList)

            {
                BsonDocument CourseDoc = new BsonDocument(); 

                CourseDoc.Add("ActivityId", item.activity_id);
                CourseDoc.Add("ActivityTitle", item.ACTIVITY.title);
                CourseDoc.Add("Barcode", item.barcode_number);
                CourseDoc.Add("CourseId", item.course_id);
                CourseDoc.Add("CourseStatus", item.course_status_id);
                CourseDoc.Add("CourseTitle", item.title);
                CourseDoc.Add("EndDate", item.last_end_datetime);
                CourseDoc.Add("StartDate", item.last_start_datetime);
                CourseDoc.Add("Supervisor", item.SUPERVISOR1.PERSON.last_name + ", " + item.SUPERVISOR1.PERSON.first_name);
                CourseDoc.Add("SurveyStatus", "S"); 
                _course.Insert(CourseDoc);

                // Debugging log for insertions
                strng = "Passed insertion of --> " + item.ACTIVITY.title + " -===- " + item.title + "\n";
                bytes = encoding.GetBytes(strng);
                count = bytes.Length;
                fs.Write (bytes, 0, count);

                strng = "  ---> " + Courses.ActivityId + " -=- " + Courses.ActivityTitle + " -=- " + Courses.CourseId + "\n";
                bytes = encoding.GetBytes(strng);
                count = bytes.Length;
                fs.Write (bytes, 0, count);
                // End Debugging log code
            }
            return View();
        }

    }
}
