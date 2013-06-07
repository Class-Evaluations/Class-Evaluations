using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;
using PagedList;

namespace Survey.Controllers
{ 
    public class SurveyController : Controller
    {
        private Survey_DBEntities db = new Survey_DBEntities();

        //
        // GET: /Survey/
        [Authorize(Roles = "Admin")]
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //sorting functionality 
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitSortParm = String.IsNullOrEmpty(sortOrder) ? "Title Desc" : "";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            var query = from p in db.SURVEYs select p;
            //searching functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Title Desc":
                    query = query.OrderByDescending(s => s.title);
                    break;
                default:
                    query = query.OrderBy(s => s.header_text);
                    break;
            }
            
            var survey = query.ToList();
            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfSurveys = survey.ToPagedList(pageNumber, 25); // will only contain 25 people max because of the pageSize
            ViewBag.onePageOfSurveys = onePageOfSurveys;

            return View(survey);
        }

        //
        // GET: /Survey/Details/5
        [Authorize(Roles = "Admin")]
        public ViewResult Details(int? id)
        {
            var surveyDetails = db.BuildSurvey(id);
            //Due to the stored procedure I have to make a 
            //trip tp the db to get the title, introduction

            SURVEY survey = db.SURVEYs.Single(s => s.survey_id == id);
            ViewBag.survey_id = id;
            ViewBag.surveyTitle = survey.header_text;
            ViewBag.surveyIntro = survey.survey_introduction;
            return View(surveyDetails);
        }

        //
        // GET: /Survey/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Survey/Create

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(SURVEY survey)
        {
            survey.datestamp = DateTime.Now;
            survey.user_stamp = 1;
            survey.survey_status = "A";

            if (ModelState.IsValid)
            {
                db.SURVEYs.AddObject(survey);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(survey);
        }
        
        //
        // GET: /Survey/Edit/5
        [Authorize(Roles = "Admin")] 
        public ActionResult Edit(int id)
        {
            SURVEY survey = db.SURVEYs.Single(s => s.survey_id == id);
            ViewBag.SurveyName = survey.title;
            return View(survey);
        }

        //
        // POST: /Survey/Edit/5

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(SURVEY survey)
        {
            if (ModelState.IsValid)
            {
                db.SURVEYs.Attach(survey);
                db.ObjectStateManager.ChangeObjectState(survey, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(survey);
        }

        //
        // GET: /Survey/Delete/5
        [Authorize(Roles = "Admin")] 
        public ActionResult Delete(int id)
        {
            SURVEY survey = db.SURVEYs.Single(s => s.survey_id == id);
            return View(survey);
        }

        //
        // POST: /Survey/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {            
            SURVEY survey = db.SURVEYs.Single(s => s.survey_id == id);
            db.SURVEYs.DeleteObject(survey);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}