using AutoPoint.Tools;
using AutoPoint.ViewModel.OrderVM;
using AutoPoint.Entity;
using AutoPoint.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AutoPoint.Controllers
{
    public class OrderController : Controller
    {
        private readonly ModelCreator modelCreator;
        private readonly ModelMapper modelMapper;
        private readonly OrderRepository orderRepository;
        private readonly MailProcessing mailProcessing;

        public OrderController()
        {
            this.modelCreator = new ModelCreator();
            this.modelMapper = new ModelMapper();
            this.orderRepository = new OrderRepository();
            this.mailProcessing = new MailProcessing();
        }


        /// <summary>
        ///         This action returns the user to the checkout form
        /// </summary>

        [HttpGet]
        public IActionResult Checkout(string deliveryType)
        {
            int userID = User.Identity.IsAuthenticated ? int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value) : 0;
            List<string> cookieIds = Request.Cookies
                                            .Where(x => x.Key.Contains(Constants.CART_ITEM))
                                            .Select(x => x.Value).ToList();

            //here we check if the logged user has any items in the cart and if not it gets redirected to the home page
            if (User.Identity.IsAuthenticated && orderRepository.isCartEmptyForLoggedUser(
                int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value)))
            {
                return RedirectToAction("Index", "Home");
            }
            //here we check if the user without logged account has any items in his cart and if not redirected to home page
            else if (!User.Identity.IsAuthenticated && cookieIds.Count <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(modelCreator.createOrderVM(cookieIds, deliveryType , userID));
        }


        /// <summary>
        ///         This action checks if the model is valid and then redirects to the order creation
        /// </summary>

        [HttpPost]
        public IActionResult Checkout(CreateOrderVM model)
        {
            //here we check if any of the required inputs is empty
            if (string.IsNullOrEmpty(model.firstName) ||
                string.IsNullOrEmpty(model.lastName) ||
                string.IsNullOrEmpty(model.phoneNumber) ||
                string.IsNullOrEmpty(model.email) ||
                string.IsNullOrEmpty(model.addressOne) ||
                string.IsNullOrEmpty(model.city) ||
                string.IsNullOrEmpty(model.postcode))
            {
                return RedirectToAction("Checkout", "Order", new { deliveryType = model.deliveryType });
            }


            return RedirectToAction("PayOrder", "Order", model);
        }

        public IActionResult PayOrder(CreateOrderVM model)
        {
            return View(model);
        }

        /// <summary>
        ///         This action creates the order and redirects the user to the confirmation
        /// </summary>

        public IActionResult CreateOrder(CreateOrderVM model)
        {
            //here we get the logged users identity map the model to a order and check if the order is existing
            int userID = User.Identity.IsAuthenticated ? int.Parse(HttpContext.User.FindFirst(ClaimTypes.Sid).Value) : 0;
            Order order = modelMapper.mapOrderVMToOrder(model , userID);
            if (!orderRepository.isExisting(order))
            {
                orderRepository.addOrder(order);
            }
            
            //here we get the order id and redirects the user to the confirmation page with that id
            int orderID = orderRepository.getSpecificOrder(order.email, order.productsCount, order.productIDs).ID;
            
            //here we send email to the user for the succesfull order
            mailProcessing.SendOrderDetails(orderID,order);

            return RedirectToAction("Confirmation", "Order", new { id = orderID });
        }

        /// <summary>
        ///         ShipOrder method finds the order updates the status of the order
        ///         sends email to the user and returns the admin to the admin panel
        /// </summary>
        public IActionResult ShipOrder(int id)
		{
            if (!User.Identity.IsAuthenticated || User.Identity.Name.Equals(Constants.USER))
                return RedirectToAction("Index", "Home");

            //here we update the order
            Order order = orderRepository.findOrderById(id);
            orderRepository.updateStatus(order,Constants.STATUSS_SHIPED);

            //here a message is being send to the user
            mailProcessing.sendUpdatedStatus(order.email, order.fullName, order.productsCount, order.total);

            //here the admin gets redirected to the current page
            return RedirectToAction("OrdersDisplay","User");
        }

        /// <summary>
        ///         This action retirects the user to the order confirmation page
        /// </summary>

        public IActionResult Confirmation(int id)
        {
            return View(modelCreator.createOrderDisplayVM(id));
        }
    }
}
