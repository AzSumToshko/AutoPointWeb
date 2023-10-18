using AutoPoint.DataBaseAccess;
using AutoPoint.Entity;
using AutoPoint.Tools;
using System.Globalization;

namespace AutoPoint.Repository
{
    public class OrderRepository
    {
        private readonly Context context;
        private readonly ProductRepository productRepo;

        public OrderRepository()
        {
            context = new Context();
            productRepo = new ProductRepository();
        }

        /// <summary>
        ///         addOrder receaves order as a parameter and if its not null
        ///         it gets inserted into the database.
        /// </summary>
        public void addOrder(Order order)
        {
            if (order != null )
            {
                context.Orders.Add(order);
                context.SaveChanges();
            }
        }

        /// <summary>
        ///         getAllOrders returns a list with all the orders in the database
        /// </summary>
		public List<Order> getAllOrders()
		{
			return context.Orders.ToList();
		}

        /// <summary>
        ///         removeAll removes all the orders from the database.
        /// </summary>
        public void removeAll()
        {
            foreach (Order order in context.Orders)
            {
                context.Orders.Remove(order);
            }
            context.SaveChanges();
        }

		/// <summary>
		///         This method returns a specific order by email,
        ///         products count and the string containing the product ids
		/// </summary>
		public Order getSpecificOrder(string email, int productsCount, string productIDs)
        {
            return context.
                Orders.FirstOrDefault
                    (x => x.email.Equals(email) && x.productsCount == productsCount && x.productIDs.Equals(productIDs));
        }

        /// <summary>
        ///         This method returns an order found by the unique id of the product
        /// </summary>
        public Order findOrderById(int id)
        {
            return context.Orders.Find(id);
        }


        /// <summary>
        ///         This method returns a list of orders all to a specific user
        /// </summary>
        public List<Order> getOrdersByUserId(int userID)
        {
            return context.Orders.Where(x => x.userID == userID && !x.status.Equals(Constants.STATUSS_IN_PROCCESSING)).ToList();
        }

        /// <summary>
        ///         getAllPendingOrders returns a list with all the order with status pending
        /// </summary>
		public List<Order> getAllPendingOrders()
		{
            return context.Orders.Where(x => x.status.Equals(Constants.STATUSS_PENDING)).ToList();
		}

        public List<Order> getAllOrdersInProccessExcluded()
        {
            return context.Orders.Where(x => !x.status.Equals(Constants.STATUSS_IN_PROCCESSING)).ToList();
        }

        public List<string> GetPurchaseDates(List<Order> orders)
        {
            var currentYear = DateTime.Now.Year;

            var purchaseDates = orders
                .Where(o => o.deliveryDate.Year == currentYear)
                .Select(o => o.deliveryDate.Date)
                .Distinct()
                .OrderBy(d => d.Month)
                .ThenBy(d => d.Day)
                .Select(d => d.ToString("MM:dd", CultureInfo.InvariantCulture))
                .ToList();

            return purchaseDates;
        }

        public List<int> GetPurchaseCountsByDate(List<string> purchaseDates, List<Order> orders)
        {
            var purchaseCounts = purchaseDates
                .Select(d => orders.Count(o => o.deliveryDate.ToString("MM:dd", CultureInfo.InvariantCulture) == d))
                .ToList();

            return purchaseCounts;
        }

        public List<double> GetTotalPriceByDate(List<string> dates, List<Order> orders)
        {
            List<double> totalPriceList = new List<double>();

            foreach (string dateStr in dates)
            {
                DateTime date = DateTime.ParseExact(dateStr, "MM:dd", CultureInfo.InvariantCulture);

                double totalPrice = 0;

                foreach (Order order in orders)
                {
                    if (order.deliveryDate.Date == date.Date)
                    {
                        totalPrice += order.total;
                    }
                }

                totalPrice = Math.Round(totalPrice, 2); // Round totalPrice to 2 digits after the decimal point
                totalPriceList.Add(totalPrice);
            }

            return totalPriceList;
        }

        /// <summary>
        ///         This method returns boolean value representing true if the cart is empty
        ///         and false if there is item in the cart
        /// </summary>
        public bool isCartEmptyForLoggedUser(int userID)
        {
            foreach (var item in context.CartProducts)
            {
                if (userID == item.userID)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///         This method checks if the order is existing and if so it will return true
        ///         otherwise false
        /// </summary>
        public bool isExisting(Order order)
        {
            foreach (var item in context.Orders)
            {
                if (order.userID == item.userID 
                    && order.addressOne == item.addressOne
                    && order.city == item.city
                    && order.total == item.total
                    && order.productQuantities == item.productQuantities
                    && order.productIDs == item.productIDs
                    && order.productsCount == item.productsCount)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///         updateStatus gets order and status and updates the orders status with the value 
        ///         and after that it gets saved in the database
        /// </summary>
		public void updateStatus(Order order , string status)
		{
			order.status = status;
            context.Entry(order).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
		}
	}
}
