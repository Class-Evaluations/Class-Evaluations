using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;
using System.Web.Routing;

namespace Survey.Controllers
{ 
    public class SurveySectionsController : Controller
    {
        private Survey_DBEntities db = new Survey_DBEntities();

        //
        // GET: /SurveySections/

        public ViewResult Index()
        {
            var survey_questions = db.SURVEY_QUESTIONS.Include("QUESTION").Include("SURVEY").Include("SURVEY_SECTION");
            return View(survey_questions.ToList());
        }

        //
        // GET: /SurveySections/Details/5

        public ViewResult Details(int id)
        {
            SURVEY_QUESTIONS survey_questions = db.SURVEY_QUESTIONS.Single(s => s.survey_questions_id == id);
            return View(survey_questions);
        }


        //
        // GET: /SurveySections/Create
        public ActionResult Create(int? id)
        {
            SURVEY surveys = db.SURVEYs.Single(s => s.survey_id == id);
            ViewBag.survey_id = new SelectList(db.SURVEYs, "survey_id", "title", surveys.survey_id);
            ViewBag.question_id = new SelectList(db.QUESTIONs, "question_id", "question_text");
            ViewBag.section_id = new SelectList(db.SURVEY_SECTION, "survey_section_id", "title");
            return View();
        }


        //
        // POST: /SurveySections/Create
        [HttpPost]
        public ActionResult Create(SURVEY_QUESTIONS survey_questions)
        {
            if (survey_questions.survey_id == 0)
            { ModelState.AddModelError("survey_id", "Survey is a required field."); }
            if (survey_questions.question_id == 0)
            { ModelState.AddModelError("question_id", "Question is a required field."); }
            if (survey_questions.section_id == 0)
            { ModelState.AddModelError("section_id", "Survey Section is a required field."); }
            //check if the question already exits in the survey
            if (survey_questions.question_id > 0 && survey_questions.survey_id > 0)
            {
                SURVEY_QUESTIONS survey_questions_check = db.SURVEY_QUESTIONS.SingleOrDefault(s => s.survey_id == survey_questions.survey_id && s.question_id == survey_questions.question_id);
                string checkforNulls = Convert.ToString(survey_questions_check);
                if (!String.IsNullOrEmpty(checkforNulls))
                {
                    //var survey_quesinons_check 
                    if (survey_questions_check.question_id == survey_questions.question_id) { ModelState.AddModelError("survey_id", "Dupicate questions are not allowed on the same survey."); }
                }
            }

            if (ModelState.IsValid)
            {
                db.SURVEY_QUESTIONS.AddObject(survey_questions);
                db.SaveChanges();
                //return RedirectToAction("Details", "Survey", survey_questions.survey_id);
                return RedirectToAction("Details", new RouteValueDictionary(new { controller = "Survey", action = "Details", Id = survey_questions.survey_id }));
            }

            ViewBag.question_id = new SelectList(db.QUESTIONs, "question_id", "question_text", survey_questions.question_id);
            ViewBag.survey_id = new SelectList(db.SURVEYs, "survey_id", "title", survey_questions.survey_id);
            ViewBag.section_id = new SelectList(db.SURVEY_SECTION, "survey_section_id", "title", survey_questions.section_id);
            return View(survey_questions);
        }        
        //
        // GET: /SurveySections/Edit/5
 
        public ActionResult Edit(int id)
        {
            SURVEY_QUESTIONS survey_questions = db.SURVEY_QUESTIONS.Single(s => s.survey_questions_id == id);
            ViewBag.question_id = new SelectList(db.QUESTIONs, "question_id", "question_text", survey_questions.question_id);
            ViewBag.survey_id = new SelectList(db.SURVEYs, "survey_id", "title", survey_questions.survey_id);
            ViewBag.section_id = new SelectList(db.SURVEY_SECTION, "survey_section_id", "title", survey_questions.section_id);
            return View(survey_questions);
        }

        //
        // POST: /SurveySections/Edit/5

        [HttpPost]
        public ActionResult Edit(SURVEY_QUESTIONS survey_questions)
        {
            if (ModelState.IsValid)
            {
                db.SURVEY_QUESTIONS.Attach(survey_questions);
                db.ObjectStateManager.ChangeObjectState(survey_questions, EntityState.Modified);
                db.SaveChanges();

                return RedirectToAction("Details", "Survey", survey_questions.survey_id);
                //return RedirectToAction(".../Survey/Details", survey_questions.section_id);
            }
            ViewBag.question_id = new SelectList(db.QUESTIONs, "question_id", "question_text", survey_questions.question_id);
            ViewBag.survey_id = new SelectList(db.SURVEYs, "survey_id", "title", survey_questions.survey_id);
            ViewBag.section_id = new SelectList(db.SURVEY_SECTION, "survey_section_id", "title", survey_questions.section_id);
            return View(survey_questions);
        }

        //
        // GET: /SurveySections/Delete/5
 
        public ActionResult Delete(int id)
        {
            SURVEY_QUESTIONS survey_questions = db.SURVEY_QUESTIONS.Single(s => s.survey_questions_id == id);
            return View(survey_questions);
        }

        //
        // POST: /SurveySections/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            SURVEY_QUESTIONS survey_questions = db.SURVEY_QUESTIONS.Single(s => s.survey_questions_id == id);
            db.SURVEY_QUESTIONS.DeleteObject(survey_questions);
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