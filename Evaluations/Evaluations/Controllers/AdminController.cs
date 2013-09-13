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

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoadMongoDB()
        {

            var GreaterThan2013 = Convert.ToDateTime("2013/04/01");
            var courseList = (from c in _db.COURSEs
                              where (c.last_end_datetime > GreaterThan2013) && c.course_status_id == "C" && c.course_status_id != "X" && c.session_title_id != 9
                              orderby c.barcode_number
                              select c).ToList();

            MongoDatabase database = MongoDatabase.Create("mongodb://localhost:27017/MongoClassDb");
            _course = database.GetCollection<Courses>("Courses");
  
            Courses Courses = new Courses();

            //Delete the MongoDb Collection first to proceed with data insertion
            if (database.CollectionExists("Courses"))
            {
                MongoCollection<Courses> collection = database.GetCollection<Courses>("Courses");
                collection.Drop();
            }

            //BsonDocument CourseDoc new BsonDocument();
            
            FileStream fs = new FileStream("c:\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".log", FileMode.OpenOrCreate);
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            string json;
            BsonDocument CourseDoc = new BsonDocument(); 
            MongoDB.Bson.BsonDocument document
                = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<BsonDocument>(CourseDoc);
            String strng;
            Byte[] bytes;
            int count = 0;

            foreach (var item in courseList)

            {

                Courses.ActivityId = item.activity_id;
                Courses.ActivityTitle = item.ACTIVITY.title;
                Courses.Barcode = item.barcode_number;
                Courses.CourseId = item.course_id;
                Courses.CourseStatus = item.course_status_id;
                Courses.CourseTitle = item.title;
                Courses.EndDate = item.last_end_datetime;
                Courses.StartDate = item.last_start_datetime;
                Courses.Supervisor = item.SUPERVISOR1.PERSON.last_name + ", " + item.SUPERVISOR1.PERSON.first_name;
                Courses.SurveyStatus = "S";

                _course.Insert(Courses); 
                //Bson.Add(CourseDoc);

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
            _course.Insert(CourseDoc); 

            return View();
        }

    }
}
