using AutoPoint.Entity;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AutoPoint.Tools
{
    public class MailProcessing
    {
        private readonly SmtpClient client;

        /// <summary>
        ///         In the constructor we initialize the smtp client where we set the properties such as the
        ///         port, host , the credentials and do we want some options or not.
        /// </summary>
        public MailProcessing()
        {
            this.client = new SmtpClient
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Host = Constants.SMTP_HOST,
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(Constants.SMTP_EMAIL, Constants.SMTP_EMAIL_PASSWORD)//tuka gmaila
            };
        }
        /// <summary>
        ///         SendEmailToSite method gets parameters such as from whos the message, the users name , the subject and the body
        ///         converts them into a mail message and then this message is being send by the client.
        /// </summary>
        public void SendEmailToSite(string from , string personName , string mailSubject , string mailBody)
        {
            var fromAddress = new MailAddress(Constants.SMTP_EMAIL);
            var toAddress = new MailAddress(Constants.SMTP_EMAIL);
            string subject = personName + " wants to ask a question : " + mailSubject;

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = mailBody + $" Email for contact : {from}"
            })
            {
                client.Send(message);
            }
        }



        /// <summary>
        ///         SendForgotenPasswordEmail method gets the email and the id of the user creates the mail message 
        ///         with the link for the password change and then the client sends it to the user.
        /// </summary>
        public void SendForgotenPasswordEmail(string to , int ID)
        {
            var fromAddress = new MailAddress(Constants.SMTP_EMAIL);
            var toAddress = new MailAddress(to);

            string passwordChangeUrl = Constants.SITE_URL + "/User/ChangePassword?ID=" + ID;

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "AutoPoint : Sending link to change forgoten password.",
                Body = "The link to change your password is : " + Constants.SITE_URL + "/User/ChangePassword?ID=" + ID
            })
            {
                client.Send(message);
            }
        }

        /// <summary>
        ///         SendOrderDetails method receaves email, person name, count of products, delivery date and total sum,
        ///         creates a mail message with those parameters and after that the client sends the message to the user.
        /// </summary>
        public void SendOrderDetails(int orderId, Order order)
        {
            var fromAddress = new MailAddress(Constants.SMTP_EMAIL);
            var toAddress = new MailAddress(order.email);
            string subject = "Succesfull order !";

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = $"{order.fullName} you have successfully made an order for {order.productsCount} products with a total price of {Math.Round(order.total,2)}bgn.",
                IsBodyHtml = true,
                BodyEncoding = Encoding.UTF8,
            })
            {
                client.Send(message);
            }
        }

        /// <summary>
        ///         sendUpdatedStatus method receaves email, full name, count of products and total sum, 
        ///         converts the parameters into mail message and after that the client sends the message to the user.
        /// </summary>
		public void sendUpdatedStatus(string email, string fullName, int productsCount, double total)
		{
            var fromAddress = new MailAddress(Constants.SMTP_EMAIL);
            var toAddress = new MailAddress(email);
            string subject = "Order status changed.";

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = $"{fullName} the status of your order with {productsCount} items and total of {Math.Round(total,2)}bgn have changed.",
                IsBodyHtml=true,
            })
            {
                client.Send(message);
            }
        }
	}
}
