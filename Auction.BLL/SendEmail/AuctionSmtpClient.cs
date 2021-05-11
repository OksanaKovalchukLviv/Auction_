using System;
using System.Net;
using System.Net.Mail;
using Auction.Common;

namespace Auction.BLL.DTO
{
    public class AuctionSmtpClient
    {
        private static System.Net.Mail.SmtpClient smtp;

        private AuctionSmtpClient()
        { }

        public static bool Send(MailMessage m)
        {
            bool result = true;

            try
            {
                if (smtp == null)
                    smtp = new System.Net.Mail.SmtpClient();
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(Configuration.Email, Configuration.password);
                }

                smtp.Send(m);
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }
    }
}
