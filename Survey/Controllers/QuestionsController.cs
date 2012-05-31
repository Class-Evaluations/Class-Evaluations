using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;
using System.Web.Routing;
using Survey.ViewModels;
using PagedList;

namespace Survey.Controllers
{ 
    public class QuestionsController : Controller
    {
       private Survey_DBEntities db = new Survey_DBEntities();

        // GET: /Questions/
       public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //sorting functionality 
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitSortParm = String.IsNullOrEmpty(sortOrder) ? "Title Desc" : "";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }

            var question_list = from q in db.QUESTIONs
                            join at in db.ANSWER_TYPE on q.answer_type_id equals at.answer_type_id 
                            orderby q.question_id
                            select new QuestionDetails
                            {
                                questionID = q.question_id,
                                questionText = q.question_text,
                                answerTypeId = q.answer_type_id,
                                answerTypeName = at.answer_type_name,
                                dateCreated = q.datestamp
                            };

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfQuestions = question_list.ToPagedList(pageNumber, 25); // will only contain 25 products max because of the pageSize
            ViewBag.onePageOfQuestions = onePageOfQuestions;

            return View(question_list);
        }

        // GET: /Questions/Details/5
        public ViewResult Details(Int32 id)
        {
            QUESTION question_details = db.QUESTIONs.Single(q => q.question_id == id);
            return View(question_details);
        }


        //Defines a questions multiple choices if applies
        public ActionResult MultipleChoiceCreate(MultipleChoiceItem choices)
        {

            if (!String.IsNullOrEmpty(choices.ChoiceText1))
            {
                var question_multiple_choice = new QUESTION_MULTIPLE_CHOICE();

                question_multiple_choice.question_id = choices.QID;
                question_multiple_choice.choice_text = choices.ChoiceText1;

                if (ModelState.IsValid)
                {
                    db.QUESTION_MULTIPLE_CHOICE.AddObject(question_multiple_choice);
                    db.SaveChanges();
                }

            }

            if (!String.IsNullOrEmpty(choices.ChoiceText2))
            {
                var question_multiple_choice = new QUESTION_MULTIPLE_CHOICE();

                question_multiple_choice.question_id = choices.QID;
                question_multiple_choice.choice_text = choices.ChoiceText2;

                if (ModelState.IsValid)
                {
                    db.QUESTION_MULTIPLE_CHOICE.AddObject(question_multiple_choice);
                    db.SaveChanges();
                }

            }

            if (!String.IsNullOrEmpty(choices.ChoiceText3))
            {
                var question_multiple_choice = new QUESTION_MULTIPLE_CHOICE();

                question_multiple_choice.question_id = choices.QID;
                question_multiple_choice.choice_text = choices.ChoiceText3;

                if (ModelState.IsValid)
                {
                    db.QUESTION_MULTIPLE_CHOICE.AddObject(question_multiple_choice);
                    db.SaveChanges();
                }

            }

            if (!String.IsNullOrEmpty(choices.ChoiceText4))
            {
                var question_multiple_choice = new QUESTION_MULTIPLE_CHOICE();

                question_multiple_choice.question_id = choices.QID;
                question_multiple_choice.choice_text = choices.ChoiceText4;

                if (ModelState.IsValid)
                {
                    db.QUESTION_MULTIPLE_CHOICE.AddObject(question_multiple_choice);
                    db.SaveChanges();
                }

            }

            if (!String.IsNullOrEmpty(choices.ChoiceText5))
            {
                var question_multiple_choice = new QUESTION_MULTIPLE_CHOICE();

                question_multiple_choice.question_id = choices.QID;
                question_multiple_choice.choice_text = choices.ChoiceText5;

                if (ModelState.IsValid)
                {
                    db.QUESTION_MULTIPLE_CHOICE.AddObject(question_multiple_choice);
                    db.SaveChanges();
                }
            }

            ModelState.Clear();    
            return View(choices);
        }

        // GET: /Questions/Create
        public ActionResult Create()
        {
            ViewBag.answer_type_id = new SelectList(db.ANSWER_TYPE, "answer_type_id", "answer_type_name");
            return View();
        } 

        // POST: /Questions/Create
        [HttpPost]
        public ActionResult Create(QUESTION question)
        {
            if (question.question_text == "")
            { ModelState.AddModelError("question_text", "This is a required field."); }
            if (question.answer_type_id == 0)
            { ModelState.AddModelError("answer_type_id", "This is a required field."); }

            question.user_stamp = 1;
            question.datestamp = DateTime.Now;
            int answerType = question.answer_type_id;

            if (ModelState.IsValid)
            {
                db.QUESTIONs.AddObject(question);
                db.SaveChanges();

                //NEEDS to bE FIXED
                //this needs to check the answer type name and not in index in the database
                //there is no guarentee the numbers are the same

                if ((answerType == 2) || (answerType == 3))
                {
                    var choices = new Survey.Models.MultipleChoiceItem();
                    choices.QID = question.question_id;
                    choices.Questiontext = question.question_text;

                    return RedirectToAction("MultipleChoiceCreate", choices);
                }
            }

            return RedirectToAction("Index"); 
        }
        
        // GET: /Questions/Edit/5
        public ActionResult Edit(int id)
        {
            
            ViewBag.answer_type_id = new SelectList(db.ANSWER_TYPE, "answer_type_id", "answer_type_name");

            QUESTION question = db.QUESTIONs.Single(q => q.question_id == id);
            question.datestamp = DateTime.Now;
            return View(question);
        }

        // POST: /Questions/Edit/5
        [HttpPost]
        public ActionResult Edit(QUESTION question)
        {
            question.datestamp = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.QUESTIONs.Attach(question);
                db.ObjectStateManager.ChangeObjectState(question, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(question);
        }

        // GET: /Questions/Delete/5
        public ActionResult Delete(int id)
        {
            QUESTION question = db.QUESTIONs.Single(q => q.question_id == id);
            return View(question);
        }

        // POST: /Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            QUESTION question = db.QUESTIONs.Single(q => q.question_id == id);
            db.QUESTIONs.DeleteObject(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //clean up
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}