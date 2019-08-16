using Mvc.Mailer;

namespace costoracle2.Mailers
{ 
    public interface IUserMailer
    {
        MvcMailMessage Userforgetpassword(string password, string UserId, string EmailId, string FName, string LName);
        MvcMailMessage Useractivation(string UserId, string EmailId, string Username);
        MvcMailMessage welcomemail(string Email, string Username);
	}
}