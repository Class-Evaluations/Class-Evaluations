using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;
using System.Security.Cryptography;

namespace Survey.Controllers
{
    public class ManualResponseEntryController : Controller
    {
        private CLASSEntities _db = new CLASSEntities();
        private Survey_DBEntities db = new Survey_DBEntities();
        //
        // GET: /ManualResponseEntry/
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.survey_id = new SelectList(db.SURVEYs, "survey_id", "title", db.SURVEYs);
            return View();
        }

        //
        // POST: /ManualResponseEntry/Create

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ManualEntry(int survey_id, string barcode)
        {

            //var id = Convert.ToInt32(barcode); 
            var courseInfo = from c in _db.COURSEs
                             where c.barcode_number == barcode
                             select c.course_id;
            if (courseInfo.Count() == 1)
            {
                //Need to validate the user entered a survey id and a course id

                //Need to verify that the course id has been send out

                byte[] data = new byte[256];

                // This is one implementation of the abstract class SHA1.
                //result = sha.ComputeHash();
                var course = Convert.ToString(courseInfo.First());
                var date = Convert.ToString(DateTime.Now);
                var personhash = String.Concat(course, date);

                SHA1 sha = new SHA1CryptoServiceProvider();

                // This is one implementation of the abstract class SHA1.
                data = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(personhash));
                personhash = BitConverter.ToString(data);

                //strip put the dash from person
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < personhash.Length; i++)
                {
                    if ((personhash[i] >= '0' && personhash[i] <= '9') || (personhash[i] >= 'A' && personhash[i] <= 'z'))
                        sb.Append(personhash[i]);
                }

                personhash = sb.ToString();
                //var surveyInfo = from a in db.SURVEYs
                //                 where a.survey_id == survey_id
                //                 select a;


                //int lifetimeInDay = surveyInfo.;
                SURVEY_REQUEST_SENT UpdateSurveySent = new SURVEY_REQUEST_SENT();
                UpdateSurveySent.survey_id = survey_id;
                UpdateSurveySent.person_hash = personhash;
                UpdateSurveySent.expiration_date = DateTime.Now;
                UpdateSurveySent.status_flag = "S";
                UpdateSurveySent.date_sent = DateTime.Now;
                UpdateSurveySent.user_stamp = 1;
                UpdateSurveySent.course_id = Convert.ToInt32(course);


                //Check if the hash code is already in the table, email already sent.
                db.AddToSURVEY_REQUEST_SENT(UpdateSurveySent);
                db.SaveChanges();
                return Redirect("http://reclink.raleighnc.gov/Survey/BuildTheSurvey.aspx/" + personhash);

            }
            return View();
            //else
            //{ //Error
            //                return View();}



                //try
                //{
                //    // TODO: Add insert logic here

                //    return View();
                //}
                //catch
                //{
                //    return View();
                //}
            }

        }
    }
