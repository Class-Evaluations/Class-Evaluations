using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mvc.Mailer;
using System.Net.Mail;

namespace Survey.Mailers
{ 

    public class UserMailer : MailerBase, IUserMailer     
	{
		public UserMailer():
			base()
		{
			MasterName="_Layout";
		}

		
		public virtual MailMessage Welcome()
		{
			var mailMessage = new MailMessage{Subject = "Survey Email Testing"};
			
			mailMessage.To.Add("dtaylor1852@gmail.com.com");
			//ViewBag.Data = someObject;
			PopulateBody(mailMessage, viewName: "Welcome");

			return mailMessage;
		}

		
		public virtual MailMessage PasswordReset()
		{
			var mailMessage = new MailMessage{Subject = "PasswordReset"};
			
			//mailMessage.To.Add("some-email@example.com");
			//ViewBag.Data = someObject;
			PopulateBody(mailMessage, viewName: "PasswordReset");

			return mailMessage;
		}

		
	}
}