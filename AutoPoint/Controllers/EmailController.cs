using Microsoft.AspNetCore.Mvc;
using AutoPoint.Repository;
using AutoPoint.ViewModel.EmailVM;
using AutoPoint.Tools;

namespace AutoPoint.Controllers
{
    public class EmailController : Controller
    {

        private readonly UserRepository userRepository;
        private readonly MailProcessing mailProcessing;

        public EmailController()
        {
            this.userRepository = new UserRepository();
            this.mailProcessing = new MailProcessing();
        }

        /// <summary>
        ///         This action returns the user to the contact page
        /// </summary>
        public IActionResult Contact(bool? emailSent)
        {
            if (emailSent != null)
            {
                ViewBag.EmailSent = true;
            }
            return View();
        }

        /// <summary>
        ///         This action sends the email and redirects the user to the contact page
        /// </summary>
        
        [HttpPost]
        public IActionResult SendEmail(SendMailVM model)
        {
            //checks if the model is valid
            if (!ModelState.IsValid)
                return RedirectToAction("Contact");

            //here we send the email
            mailProcessing.SendEmailToSite(model.email, model.name, model.subject, model.message);

            //we get redirected to the action
            return RedirectToAction("Contact", "Email", new { emailSent = true });
        }

        /// <summary>
        ///         This action returns the user to the forgoten password page
        /// </summary>
        
        [HttpGet]
        public IActionResult ForgotenPassword()
        {
            return View();
        }

        /// <summary>
        ///         This action generates new password to the user with the specified email
        ///         sends it to his email and redirects the user to the login page
        /// </summary>
        
        [HttpPost]
        public IActionResult ForgotenPassword(string email)
        {
            //check wether the email is being used by any of the users
            if (userRepository.findByEmail(email) == null)
                return View();

            //sends the message with the link for a new password
            mailProcessing.SendForgotenPasswordEmail(email , userRepository.findByEmail(email).ID);

            //the user gets redirected to login page
            return RedirectToAction("Login", "User");
        }
    }
}
