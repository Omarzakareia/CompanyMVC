using DAL.Models;
using System.Net;
using System.Net.Mail;

namespace CompanyMVC.Helpers
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var Client = new SmtpClient("smtp.gmail.com", 587);
            Client.EnableSsl = true;
            Client.Credentials = new NetworkCredential("omarzakareia868@gmail.com", "hduifzzywhwuugsm");
            Client.Send("omarzakareia868@gmail.com", email.To, email.Subject, email.Body);
            
                    
        }
    }
}
