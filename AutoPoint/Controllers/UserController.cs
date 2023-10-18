using Microsoft.AspNetCore.Mvc;
using AutoPoint.Repository;
using AutoPoint.ViewModel.UserVM;
using AutoPoint.Entity;
using AutoPoint.Tools;
using Scrypt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace AutoPoint.Controllers
{
    public class UserController : Controller
    {
        private readonly ModelCreator modelCreator;
        private readonly ModelMapper modelMapper;
        private readonly UserRepository userRepository;
        private readonly ScryptEncoder encoder;

        public UserController()
        {
            this.modelCreator = new ModelCreator();
            this.modelMapper = new ModelMapper();
            this.userRepository = new UserRepository();  
            this.encoder = new ScryptEncoder();
        }


        //-------------------------------|| AUTHENTICATION ACTIONS ||-----------------------

        /// <summary>
        ///         This action returns the user to the login page
        /// </summary>

        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.isUserFound = true;
            return View();
        }

        /// <summary>
        ///         This action checks if theres user with that information 
        ///         and if so the user is being loged
        /// </summary>

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            //checks if the model is valid
            if (!ModelState.IsValid)
            {
                ViewBag.isUserFound = false;
                return View(model);
            }
                

            //finds the user with that email and password
            User user = userRepository.FindByEmailAndPassword(model.email, model.password);
            if (user == null)
            {
                //if the user is null there are no users with that info found
                //and the user is being pushed back to the login page
                ViewBag.isUserFound = false;
                return View(model);
            }
            else
            {
                //if user is found a list of claims with the information is being created
                List<Claim> claims = new List<Claim>() { 
                    new Claim(ClaimTypes.NameIdentifier , $"{user.firstName} {user.lastName}"),
                    new Claim(ClaimTypes.Email , user.email),
                    new Claim(ClaimTypes.Name , user.isAdmin ? Constants.ADMIN: Constants.USER),
                    new Claim(ClaimTypes.Sid , user.ID.ToString()),

                };

                //creating the identity
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

                //creating the authentication properties
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true,
                    IsPersistent = model.keepMeLoged == 1 ? true : false
                };

                //logs the user in
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity), properties);
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        ///         This action returns the user to the register form
        /// </summary>

        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.isEmailTaken = false;
            return View();
        }

        /// <summary>
        ///         This action registrates the user and returns him to the index page
        /// </summary>

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            //checks if the model is valid
            if (!ModelState.IsValid)
            {
                ViewBag.isEmailTaken = true;
                return View(model);
            }

            //checks if theres already user with that email
            if (userRepository.findByEmail(model.email) != null)
            {
                ViewBag.isEmailTaken = true;
                return View(model);
            }

            //hashing the password and maping the model
            model.password = encoder.Encode(model.password);
            User userToRegister = modelMapper.mapCreateVMToUser(model);

            //inserts the user to the database
            userRepository.AddToDataBase(userToRegister);

            return RedirectToAction("Index", "Home");
        }


        //----------------------------------------------------------------------------------
        //-------------------------------|| PROFILE ACTIONS ||------------------------------


        /// <summary>
        ///         This action returns the user to the profile page
        /// </summary>

        public IActionResult Profile()
        {
            //checks if the user is authenticated
            if (HttpContext.User.Identity == null)
            {
                return RedirectToAction("Login", "User");
            }

            return View(modelCreator.createProfileVM(int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value)));
        }

        /// <summary>
        ///         This action returns the user to the edit profile page
        /// </summary>

        [HttpGet]
        public IActionResult EditProfile(int id)
        {
            //checks if the user is authenticated
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "User");

            int userID = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            return View(modelCreator.createEditProfileVM(id , userID));
        }

        /// <summary>
        ///         This action edits the users profile
        /// </summary>

        [HttpPost]
        public IActionResult EditProfile(EditProfileVM model)
        {
            //checks if the user is authenticated
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "User");

            //map the new value
            User userToEdit = userRepository.FindById(int.Parse(User.FindFirst(ClaimTypes.Sid).Value));

            userToEdit.firstName = model.user.firstName;
            userToEdit.lastName = model.user.lastName;
            userToEdit.email = model.user.email;
            userToEdit.address = model.user.address;

            //updates the user
            userRepository.Update(userToEdit);

            return RedirectToAction("Profile", "User");
        }

        /// <summary>
        ///         ChangePassword accepts id and redirects the user to the page
        ///         where he can change his forgoten password
        /// </summary>
        
        [HttpGet]
        public IActionResult ChangePassword(int id)
        {
            //checks if theres valid id
            if (id <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            ChangePasswordVM model = new ChangePasswordVM();
            model.ID = id;
            return View(model);
        }

        /// <summary>
        ///         Here the password is being hashed changed and updated in the database
        /// </summary>

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordVM model)
        {
            //checks if the password and the retyped one are equal
            if (model.password.Equals(model.password2))
            {
                User user = userRepository.FindById(model.ID);

                user.password = encoder.Encode(model.password);

                userRepository.Update(user);

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        /// <summary>
        ///         This method logs the user out
        /// </summary>

        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index","Home");
        }

        /// <summary>
        ///         This action return the user to the cart
        /// </summary>

        public IActionResult Cart()
        {
            int userID = User.Identity.IsAuthenticated ? int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value) : 0;
            List<string> cookieIds = Request.Cookies.Where(x => x.Key.Contains("cartItem")).Select(x => x.Value).ToList();
            return View(modelCreator.createCartVM(cookieIds,userID));
        }


        //----------------------------------------------------------------------------------
        //--------------------------------|| ADMIN ACTIONS ||-------------------------------

        /// <summary>
        ///         This action returns the user to the admin panel
        /// </summary>

        public IActionResult AdminPanel()
        {
            //checks if the user is admin
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            return View(modelCreator.createAdminVM(int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value)));
        }

        public IActionResult OrdersDisplay(bool pendingOnly)
        {
            //checks if the user is admin
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            return View(modelCreator.createOrderDisplayForUserVM(int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value),pendingOnly));
        }

        public IActionResult UsersDisplay()
        {
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            return View(modelCreator.createUserDisplayVM(int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value)));
        }

        public IActionResult ProductsDisplay()
        {
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            return View(modelCreator.createProductsDisplayVM(int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value)));
        }

        /// <summary>
        ///         This action redirects the user to the user creation form
        /// </summary>

        [HttpGet]
        public IActionResult Create()
        {
            //checks if the user is admin
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            CreateVM model = new CreateVM();
            model.userID = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            return View(model);
        }

        /// <summary>
        ///         This action creates the user from the form
        /// </summary>

        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            //checks if the model is valid
            if (!ModelState.IsValid)
                return View(model);

            //checks if the user is admin
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            //checks if the database already have user with that email
            if (userRepository.findByEmail(model.email) != null)
                return View(model);

            //here the program maps and creates the user
            User user = modelMapper.mapCreateVMToUser(model);
            user.password = encoder.Encode(model.password);
            userRepository.AddToDataBase(user);

            return RedirectToAction("AdminPanel", "User");
        }

        /// <summary>
        ///         This action redirects the user to the update user form
        /// </summary>

        [HttpGet]
        public IActionResult Update(int id)
        {
            //checks if the user is admin
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            int userID = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);

            return View(modelCreator.createUserUpdateVM(id , userID));
        }

        /// <summary>
        ///         This action updates the user with the info from the form
        /// </summary>

        [HttpPost]
        public IActionResult Update(UpdateVM model)
        {
            //checks if the user is admin
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            if (!string.IsNullOrEmpty(model.password))
            {
                model.password = encoder.Encode(model.password);
            }
            
            userRepository.Update(modelMapper.mapUpdateVMToUser(model));
            return RedirectToAction("AdminPanel","User");
        }

        /// <summary>
        ///         This action deletes user
        /// </summary>

        public IActionResult Delete(int id)
        {
            //checks if the user is admin
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            userRepository.RemoveById(id);

            return RedirectToAction("UsersDisplay", "User");
        }


        //----------------------------------------------------------------------------------
    }
}
