using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.EntityModel;
using System.Data.Entity;
using Evaluations.Controllers;
using System.Data.Objects;
using System.Data.SqlClient;



namespace Evaluations.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public DateTime? Today { get; set; }
    }
}
