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

namespace Survey.Controllers
{
    public class EmailController : Controller
    {
        private Class_701Entities _db = new Class_701Entities();
        private Survey_DBEntities Surveydb = new Survey_DBEntities();
        
        //
        // GET: /Email/

        public ActionResult Index()
        {
            return View();
        }

        public void sendWelcome()
        {
            int id = (int)TempData["course"];
            int survey_id = (int)TempData["survey"];

            var person = from r in _db.REGISTRATIONs
                         where r.course_id == id
                         select new
                         {
                             person_id = r.CLIENT.PERSON.person_id,
                             first_name = r.CLIENT.PERSON.first_name,
                             last_name = r.CLIENT.PERSON.last_name,
                             activity_title = r.COURSE.ACTIVITY.title,
                             course_title = r.COURSE.title,
                             course_id = r.COURSE.course_id,
                             email_address = r.CLIENT.PERSON.email_address,
                             email_private = r.CLIENT.PERSON.email_private,
                             barcode_number = r.COURSE.barcode_number
                         };


            byte[] data = new byte[256];

            // This is one implementation of the abstract class SHA1.
            //result = sha.ComputeHash();
            int EmptyAddresses = 0;
            int EmailsSent = 0;
            foreach (var item in person)
            {
                if (string.IsNullOrEmpty(item.email_address))
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

                        string subjectline = "Park and Recreation Survey for ";

                        //check for nulls or empty names before using Trim
                        if (!String.IsNullOrEmpty(item.activity_title))
                        { subjectline = subjectline + (item.activity_title).Trim(); }
                        if (!String.IsNullOrEmpty(item.course_title))
                        { subjectline = subjectline + ", " + (item.course_title).Trim(); }

                        string emailBody = "";
                        devemail.Subject = subjectline;


                        devemail.Priority = System.Net.Mail.MailPriority.High;
                        devemail.IsBodyHtml = true;

                        string SurveyUrl = String.Concat("http://localhost:1777/WebPages/BuildTheSurvey.aspx/", personhash);

                        emailBody = "<p> Hello " + item.first_name + ", </p> <p></P> <p>The goal of Raleigh Parks and Recreation is to offer the best" +
                                    " programming possible. The purpose of this survey is to gather information from residents in the community concerning" +
                                    " various programs offered. We are interested in improving services and programs offered in the future and value your input." +
                                    " Please take the time to answer the following questions and be as honest as possible. All answers to this survey will" +
                                    " remain anonymous. </p> </br><p> Click on the link below to begin your survey.</p> " + SurveyUrl;
                        devemail.Body = emailBody;

                        //FileReader returns an array of recipients from a text file

                        //string recipients = "dtaylor1852@gmail.com; donna.taylor@raleighnc.gov";
                        string recipients = item.email_address;

                        devemail.To.Add(recipients);

                        mailClient.Send(devemail);

                        //Insert into tables after email is sent.

                        SURVEY_REQUEST_SENT EmailSurvey = new SURVEY_REQUEST_SENT();

                        EmailSurvey.survey_id = survey_id;
                        EmailSurvey.person_hash = personhash;
                        EmailSurvey.expiration_date = DateTime.Now.AddDays(90);
                        EmailSurvey.status_flag = "S";
                        EmailSurvey.date_sent = DateTime.Now;
                        EmailSurvey.user_stamp = 1;
                        EmailSurvey.course_id = Convert.ToInt32(id);


                        //Check if the hash code is already in the table, email already sent.

                        Surveydb.AddToSURVEY_REQUEST_SENT(EmailSurvey);
                        Surveydb.SaveChanges();

                        EmailsSent++;
                    }
                }
            }

            //After the surveys have been sent out set the flag in Course Status as "S"urveyed

            COURSE_STATUS surveySent = new COURSE_STATUS();

            surveySent.course_id = Convert.ToInt32(id);
            surveySent.course_status1 = "S";
            Surveydb.AddToCOURSE_STATUS(surveySent);
            Surveydb.SaveChanges();

            var emailview = new EmailStats();
            emailview.EmailCountSent = EmailsSent;
            emailview.NoEmailAdddress = EmptyAddresses;

            View(emailview);
        }

    }
}
