using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;

namespace Survey.Controllers
{
    public class BuildSurveyController : Controller
    {
        //
        //Preview the selected survey in a browser/

        public ActionResult Index(int? surveyid)
        {
            var PreviewSurvey = new Survey_DBEntities();
            PreviewSurvey.BuildSurvey(surveyid);
            return View(PreviewSurvey);
        }
    }
}
