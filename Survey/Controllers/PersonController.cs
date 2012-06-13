using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Survey.Models;
using Survey.ViewModels;
using PagedList;
using System.Diagnostics;
using Survey.Mailers;
using Mvc.Mailer;
using System.Configuration;
using System.Security.Cryptography;
using System.Web.Routing;


namespace Survey.Controllers
{
    public class PersonController : Controller
    {
        int surveyLife;
        DateTime expDate;

        private CLASSEntities _db = new CLASSEntities();
        private Survey_DBEntities Surveydb = new Survey_DBEntities();
        
        // GET: /Person/
        public ViewResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            //sorting functionality functionality
            ViewBag.CurrentSort = sortOrder;
            ViewBag.LastnameSortParm = String.IsNullOrEmpty(sortOrder) ? "Last Name Desc" : "";
            ViewBag.EmailSortParm = String.IsNullOrEmpty(sortOrder) ? "Email Desc" : "";

            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            var query = from p in _db.People where p.person_id > 250000 select p;
            //searching functionality
            if (!String.IsNullOrEmpty(searchString))
            {
                query = query.Where(s => s.last_name.Contains(searchString));
            }
            else
            {
                query = query.Where(s => s.person_id > 250000);
            }


            switch (sortOrder)
            {
                case "Last Name Desc":
                    query = query.OrderByDescending(s => s.last_name);
                    break;
                case "Email Desc":
                    query = query.OrderByDescending(s => s.email_address);
                    break;
                default:
                    query = query.OrderBy(s => s.person_id);
                    break;
            }
            var people = query.ToList();
            //setting a limit for testing, will need to decide how to exclude the older

            var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
            var onePageOfPeople = people.ToPagedList(pageNumber, 25); // will only contain 25 people max because of the pageSize
            ViewBag.onePageOfPeople = onePageOfPeople;

            return View(people);
        }

        //passing the selelcted courseID and get the clients
        //associated with this courseID and send to page.

        public ViewResult Details(int? id, int? survey_id)
        {

            ViewBag.survey_id = new SelectList(Surveydb.SURVEYs, "survey_id", "title", Surveydb.SURVEYs);

            var person = from r in _db.REGISTRATIONs
                         where r.course_id == id && (r.registration_status_id == "A" || r.registration_status_id == "C")
                         select new ParticipateData
                         {
                             person_id = r.CLIENT.PERSON.person_id,
                             first_name = r.CLIENT.PERSON.first_name,
                             last_name = r.CLIENT.PERSON.last_name,
                             activity_title = r.COURSE.ACTIVITY.title,
                             course_title = r.COURSE.title,
                             course_id = r.COURSE.course_id,
                             email_address = r.CLIENT.PERSON.email_address,
                             account_email = r.CLIENT.ACCOUNT.email_address,
                             email_private = r.CLIENT.PERSON.email_private,
                             barcode_number = r.COURSE.barcode_number
                         };
            ViewBag.course = id;
            return View(person);
       }


       protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);

        }

       [HttpPost]
       public ViewResult sendWelcome(FormCollection col)
       {

            int survey_id = Convert.ToInt32(col["survey_id"]);

            //Get the values sent by form that are comma delimited and put them in an
            //array, this will help when the ability to select more than one course to send out.

            string s = col["item.course_id"];
            string[] values = s.Split(',');

            int id = Convert.ToInt32(values[0]);
            var person = from r in _db.REGISTRATIONs
                            where r.course_id == id  && (r.registration_status_id == "A" || r.registration_status_id =="C")
                            select new
                            {
                                person_id = r.CLIENT.PERSON.person_id,
                                first_name = r.CLIENT.PERSON.first_name,
                                last_name = r.CLIENT.PERSON.last_name,
                                account_owner = r.CLIENT.ACCOUNT.title,
                                activity_title = r.COURSE.ACTIVITY.title,
                                course_title = r.COURSE.title,
                                course_id = r.COURSE.course_id,
                                email_address = r.CLIENT.PERSON.email_address,
                                account_email = r.CLIENT.ACCOUNT.email_address,
                                address_id = r.CLIENT.PERSON.address_id,
                                email_private = r.CLIENT.PERSON.email_private,
                                barcode_number = r.COURSE.barcode_number
                            };


            var personsToPrint = from p in person
                                 where ((string.IsNullOrEmpty(p.email_address) && string.IsNullOrEmpty(p.account_email))) || p.email_private > 0
                                 select new PeopleToManuallyMail
                                 {
                                     personID = p.person_id,
                                     firstName = p.first_name,
                                     lastName = p.last_name,
                                     email = p.email_address,
                                     emailPrivate = p.email_private,
                                     barcode = p.barcode_number,
                                     courseID = p.course_id,
                                     addressID = p.address_id,
                                     accountOwner = p.account_owner,
                                     accountEmail = p.account_email

                                 };
                                 


            byte[] data = new byte[256];

            // This is one implementation of the abstract class SHA1.
            //result = sha.ComputeHash();
            int EmptyAddresses = 0;
            int EmailsSent = 0;;
            int EmailPrivacy = 0;

            foreach (var item in person)
            {
                if ((string.IsNullOrEmpty(item.email_address)) && (string.IsNullOrEmpty(item.account_email)))
                { EmptyAddresses++; }
                else
                {
                    if (item.email_private == 0)
                    {
                        string course;
                        string personid;
                        string personhash;
                        course = Convert.ToString(item.course_id);
                        personid = Convert.ToString(item.person_id);
                        personhash = String.Concat(course, personid);

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

                        string emailserver = ConfigurationManager.AppSettings["mailSettings"];
                        System.Net.Mail.MailMessage devemail = new System.Net.Mail.MailMessage();
                        System.Net.Mail.SmtpClient mailClient = new System.Net.Mail.SmtpClient(emailserver);

                        string subjectline = "Park and Recreation Survey  for ";

                        //check for nulls or empty names before using Trim
                        if (!String.IsNullOrEmpty(item.activity_title))
                        { subjectline = subjectline + (item.activity_title).Trim(); }
                        if (!String.IsNullOrEmpty(item.course_title))
                        { subjectline = subjectline + ", " + (item.course_title).Trim(); }

                        string emailBody = "";
                        devemail.Subject = subjectline;

                        devemail.IsBodyHtml = true;

                        string SurveyUrl = String.Concat("http://reclink.raleighnc.gov/Survey/BuildTheSurvey.aspx/", personhash);

                        //check id participate email address is NULL, is so change the  email verbage and use the account email address.
                        var recipients = "";
                        var toPerson = "";
                        if (string.IsNullOrEmpty(item.email_address))
                        {
                            recipients = item.account_email;
                            toPerson = item.account_owner;
                        }
                        else
                        {
                            recipients = item.email_address;
                            toPerson = item.first_name;
                        }
                            
                        emailBody = "<p> Hello " + (toPerson).Trim() + ": </p> <p></P> <p>The goal of Raleigh Parks and Recreation is to offer the best" +
                                    " programming possible. The purpose of this survey is to gather information from residents in the community concerning" +
                                    " various programs offered. We are interested in improving services and programs offered in the future and value your input." +
                                    " Please take the time to answer the following questions and be as honest as possible. All answers to this survey will" +
                                    " remain anonymous. </p> </br><p> Click on the link below to begin your survey.</p> " + SurveyUrl;
                        devemail.Body = emailBody;

                        //FileReader returns an array of recipients from a text file

                        //REMOVE BEFORE SAVING UP TO GIT  TESTING ONLY
                        //recipients = "donna.taylor@raleighnc.gov";

                        devemail.To.Add(recipients);
                            
                        mailClient.Send(devemail);

                        //Insert into tables after email is sent.
                        SURVEY_REQUEST_SENT EmailSurvey = new SURVEY_REQUEST_SENT();

                        //Get the survey lifetime from Survey
                        //Get lifetime from database is not working need to working on this

                        var surveyInfo = from a in Surveydb.SURVEYs
                                         where a.survey_id == survey_id
                                         select a;


                        //int lifetimeInDay = surveyInfo.;

                        EmailSurvey.survey_id = survey_id;
                        EmailSurvey.person_hash = personhash;
                        EmailSurvey.expiration_date = DateTime.Now.AddDays(30);
                        expDate = EmailSurvey.expiration_date;
                        EmailSurvey.status_flag = "S";
                        EmailSurvey.date_sent = DateTime.Now;
                        EmailSurvey.user_stamp = 1;
                        EmailSurvey.course_id = Convert.ToInt32(id);


                        //Check if the hash code is already in the table, email already sent.
                        Surveydb.AddToSURVEY_REQUEST_SENT(EmailSurvey);
                        Surveydb.SaveChanges();
                        EmailsSent++;
                    }
                    else
                    {
                        EmailPrivacy++;
                    }
                }

            }

            //after the emails are sent out update the Course Status table that survey for this course was sent.
            COURSE_STATUS CourseSurveySent = new COURSE_STATUS();
            CourseSurveySent.course_id = Convert.ToInt32(id);
            CourseSurveySent.course_status1 = "S";
            CourseSurveySent.survey_exp_date = expDate;
            Surveydb.AddToCOURSE_STATUS(CourseSurveySent);
            Surveydb.SaveChanges();

            //var emailview = new EmailStats();
            ViewBag.EmailCountSent = EmailsSent;
            ViewBag.NoEmailAdddress = EmptyAddresses;
            ViewBag.EmailPrivacyFlag = EmailPrivacy;
            ViewBag.courseID = id;

            //ViewBag.people = personsToPrint;
            return View(personsToPrint);
        }
    
       [HttpPost]
       public ViewResult printPDF( object peopleToPrint)
        {

           //Also the Course Status table will need to be updated as survey sent or printed.  
           return View();
            
        }

       public ViewResult EmailPreview(int id)
       {
           string subjectline = "Park and Recreation Survey  for ...Course Name ";
           string emailBody = "";
           string SurveyUrl = String.Concat("http://reclink.raleighnc.gov/Survey/BuildTheSurvey.aspx/");
           string Greeting = "Hello (First Name)";

           emailBody = " Thank you for participating in a Raleigh Parks and Recreation program.  In an effort to " +
                       "provide outstanding programs and service, we would appreciate your valuable input.  All " +
                       "answers will remain anonymous unless you indicate that you would like to be contacted. " +
                       "                                                                                        " +
                       "Click on the link below to begin your survey: " + SurveyUrl;

           string recipients = "donna.taylor@raleighnc.gov";

           ViewBag.subjectline = subjectline;
           ViewBag.greeting = Greeting;
           ViewBag.emailBody = emailBody;
           ViewBag.recipents = recipients;
           ViewBag.SurveyUrl = SurveyUrl;
           ViewBag.courseid = id;
           return View();
       }
   }
}