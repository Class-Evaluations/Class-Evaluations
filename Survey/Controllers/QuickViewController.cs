using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;

namespace Survey.Controllers
{ 
    public class QuickViewController : Controller
    {
        private Survey_DBEntities db = new Survey_DBEntities();

        //
        // GET: /QuickView/

        //public ViewResult Index(int id)
        //{
        //    SURVEY_REQUEST_SENT RequestSent = new SURVEY_REQUEST_SENT();

        //    var course = db.SURVEY_REQUEST_SENT.Single(a => a.course_id == id);

        //    //var answer_scale = db.ANSWER_SCALE.Select(c=> c.survey_request_sent_id = course.survey_request_sent_id);
        //    //return View(answer_scale.ToList());
        //    return View();
        //}

        ////
        //// GET: /QuickView/Details/5

        //public ViewResult Details(int id)
        //{

        //    //ANSWER_SCALE answer_scale = db.ANSWER_SCALE.Single(a => a.answer_scale_id == id);
        //    //return View(answer_scale);
        //}

        ////
        //// GET: /QuickView/Create

        //public ActionResult Create()
        //{
        //    ViewBag.answer_id = new SelectList(db.ANSWERs, "answer_id", "answer_id");
        //    return View();
        //} 

        ////
        //// POST: /QuickView/Create

        //[HttpPost]
        //public ActionResult Create(ANSWER_SCALE answer_scale)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ANSWER_SCALE.AddObject(answer_scale);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");  
        //    }

        //    ViewBag.answer_id = new SelectList(db.ANSWERs, "answer_id", "answer_id", answer_scale.answer_id);
        //    return View(answer_scale);
        //}
        
        ////
        //// GET: /QuickView/Edit/5
 
        //public ActionResult Edit(int id)
        //{
        //    ANSWER_SCALE answer_scale = db.ANSWER_SCALE.Single(a => a.answer_scale_id == id);
        //    ViewBag.answer_id = new SelectList(db.ANSWERs, "answer_id", "answer_id", answer_scale.answer_id);
        //    return View(answer_scale);
        //}

        ////
        //// POST: /QuickView/Edit/5

        //[HttpPost]
        //public ActionResult Edit(ANSWER_SCALE answer_scale)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ANSWER_SCALE.Attach(answer_scale);
        //        db.ObjectStateManager.ChangeObjectState(answer_scale, EntityState.Modified);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.answer_id = new SelectList(db.ANSWERs, "answer_id", "answer_id", answer_scale.answer_id);
        //    return View(answer_scale);
        //}

        ////
        //// GET: /QuickView/Delete/5
 
        //public ActionResult Delete(int id)
        //{
        //    ANSWER_SCALE answer_scale = db.ANSWER_SCALE.Single(a => a.answer_scale_id == id);
        //    return View(answer_scale);
        //}

        ////
        //// POST: /QuickView/Delete/5

        //[HttpPost, ActionName("Delete")]
        //public ActionResult DeleteConfirmed(int id)
        //{            
        //    ANSWER_SCALE answer_scale = db.ANSWER_SCALE.Single(a => a.answer_scale_id == id);
        //    db.ANSWER_SCALE.DeleteObject(answer_scale);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}