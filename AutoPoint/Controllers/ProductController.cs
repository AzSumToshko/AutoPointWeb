using AutoPoint.Repository;
using Microsoft.AspNetCore.Mvc;
using AutoPoint.ViewModel.ProductVM;
using AutoPoint.Tools;
using System.Security.Claims;

namespace AutoPoint.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductRepository productRepository;
        private readonly ModelCreator modelCreator;
        private readonly ModelMapper modelMapper;

        public ProductController()
        {
            this.modelCreator = new ModelCreator();
            this.modelMapper = new ModelMapper();
            this.productRepository = new ProductRepository();
        }

        /// <summary>
        ///         This action returns the user to the products index page
        /// </summary>
        public IActionResult Index(string searchInput,string category , int minSortingValue , int maxSortingValue)
        {
            return View(modelCreator.createIndexVM(searchInput, category, minSortingValue, maxSortingValue));
        }

        /// <summary>
        ///         This action returns the user to the details of specific product
        /// </summary>

        public IActionResult Details(int id)
        {
            string fullName = User.Identity.IsAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier).Value : Constants.EMPTY_STRING;
            return View(modelCreator.createDetailsVM(id , fullName));
        }

        /// <summary>
        ///         This action deletes product
        /// </summary>

        public IActionResult Delete(int id)
        {
            productRepository.DeleteProduct(id);
            return RedirectToAction("AdminPanel", "User");
        }

        /// <summary>
        ///         This action returns the user to the product creation page
        /// </summary>

        [HttpGet]
        public IActionResult Create()
        {
            CreateVM model = new CreateVM();
            model.userID = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            return View(model);
        }

        /// <summary>
        ///         This action creates the product and redirects to the admin panel
        /// </summary>

        [HttpPost]
        public IActionResult Create(CreateVM model)
        {
            //checks if the model is valid
            if (!ModelState.IsValid)
                return View(model);

            //checks if the file is null
            if (model.file == null)
            {
                return View(model);
            }

            //inserts the product and redirects to the admin panel
            productRepository.InsertProduct(modelMapper.mapCreateVMToProduct(model,model.file));
            return RedirectToAction("AdminPanel","User");
        }

        /// <summary>
        ///         This action returns the user to the product update form
        /// </summary>

        [HttpGet]
        public IActionResult Update(int id , string fromAction)
        {
            int userID = int.Parse(User.FindFirst(ClaimTypes.Sid).Value);
            return View(modelCreator.createUpdateVM(id,fromAction , userID));
        }

        /// <summary>
        ///         This action updates the product and redirects the user to the admin panel
        /// </summary>

        [HttpPost]
        public IActionResult Update(UpdateVM model)
        {
            //checks if the model is valid
            if (string.IsNullOrEmpty(model.name) ||
                string.IsNullOrEmpty(model.description) ||
                model.price < 1)
            {
                return View(model);
            }

            //updates the product
            productRepository.updateProduct(modelMapper.mapUpdateVMToProduct(model,model.file));

            //redirects the user
            if (model.fromAction.Equals(Constants.DETAILS))
                return RedirectToAction("Details","Product", new {id = model.id });
            else
                return RedirectToAction("AdminPanel", "User");
        }

        /// <summary>
        ///         This action adds product to cart
        /// </summary>

        public IActionResult AddToCart(int productId , string actionName, string controlerName , int productQuantity)
        {
            if (!User.Identity.IsAuthenticated)
            {
                //anonymous user
                CookieOptions cookieOptions = new CookieOptions();
                cookieOptions.Expires = DateTimeOffset.Now.AddHours(2);

                Response.Cookies.Append(Constants.CART_ITEM + productId, $"{productId}+{productQuantity}", cookieOptions);
            }
            else
            {
                //authenticated user
                productRepository.addCartItem(productRepository.
                    createCartProduct(productId,productQuantity,int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value)));
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        /// <summary>
        ///         This action removes item from cart
        /// </summary>

        public IActionResult RemoveCartItem(int productID)
        {
            if (User.Identity.IsAuthenticated)
            {
                //authenticated user
                productRepository.RemoveProductFromUser(int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value) , productID);
            }
            else
            {
                //anonymous user
                Response.Cookies.Delete(Constants.CART_ITEM+productID);
            }

            return RedirectToAction("Cart","User");
        }
    }
}
