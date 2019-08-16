using Classes;
using Mvc.Mailer;
using System.Configuration;

namespace costoracle2.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
        EncryptDecrypt enc = new EncryptDecrypt();
        public string serverpath = ConfigurationManager.AppSettings["ServerPath"].ToString();

        public string adminEmail = ConfigurationManager.AppSettings["adminemail"].ToString();

		public UserMailer()
		{
			MasterName="_Layout";
		}

        public virtual MvcMailMessage Useractivation(string UserId, string Email, string Username)
        {
            ViewBag.Email = Email;
            ViewBag.UserId = enc.Decrypt(UserId);
            ViewBag.Username = Username;

            string path = ViewBag.Serverurl = serverpath + "/account/activation?uid=" + UserId;
            ViewBag.Serverurl = path;

            return Populate(x =>
            {
                x.Subject = "Email verification from costoracle";
                x.ViewName = "Useractivation";
                x.To.Add((Email));
                //x.To.Add((useremail));
            });
        }

        public virtual MvcMailMessage welcomemail(string Email, string Username)
        {
            ViewBag.Username = Username;
            ViewBag.Email = Email;

            return Populate(x =>
            {
                x.Subject = "Welcome to costoracle";
                x.ViewName = "WelcomeGHT";
                x.To.Add("web5@goigi.com");
            });
        }

        public virtual MvcMailMessage Userforgetpassword(string password, string UserId, string EmailId, string FName, string LName)
        {
            ViewBag.password = (password);
            ViewBag.FName = (FName);
            ViewBag.LName = (LName);
            ViewBag.UserId = enc.Decrypt(UserId);
            ViewBag.EmailId = enc.Decrypt(EmailId);
            string path = ViewBag.Serverurl = serverpath + "/Account/Resetpassword?uid=" + UserId;
            ViewBag.Serverurl = path;

            return Populate(x =>
            {
                x.Subject = "Reset Password for costoracle";
                x.ViewName = "ForgetPassworduser";
                x.To.Add(ViewBag.EmailId);
            });
        }

 	}
}