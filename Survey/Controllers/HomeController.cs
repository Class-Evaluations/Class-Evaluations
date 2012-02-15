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
            ViewBag.Message = "Survey Results Matrix will be here.";
            var data = from courses in _db.COURSE_STATUS
                       group courses by courses.course_status1 into courseStatusGroup
                       select new CourseStatusGroup()
                       {
                           CourseStatus = courseStatusGroup.Key,
                           CourseCount = courseStatusGroup.Count()


                       };
            return View(data);

       }

        public ActionResult About()
        {

            return View();
        }
    }
}
