using AutoPoint.ViewModel.OrderVM;
using AutoPoint.ViewModel.ProductVM;
using AutoPoint.ViewModel.UserVM;
using AutoPoint.Entity;
using AutoPoint.Repository;
using System.Text.Json;
using System.Text.Encodings.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AutoPoint.Tools
{

    /// <summary>
    ///         This is the class that generates the View Models that are going
    ///         to be used in the controllers when passing the information to the view
    /// </summary>
    /// 
    public class ModelCreator
    {
        private readonly ProductRepository productRepository;
        private readonly UserRepository userRepository;
        private readonly CommentRepository commentRepository;
        private readonly OrderRepository orderRepository;
        private readonly ModelMapper modelMapper;

        public ModelCreator()
        {
            this.productRepository = new ProductRepository();
            this.userRepository = new UserRepository();
            this.commentRepository = new CommentRepository();
            this.orderRepository = new OrderRepository();   
            this.modelMapper = new ModelMapper();
        }


        //--------------------------------|| USER VIEW MODELS ||--------------------------------


        public AdminVM createAdminVM(int userID)
        {
            List<string> orderHours = new List<string>();
            double todaysRevenue = 0.0;
            List<Order> todaysOrders = orderRepository.getAllOrdersInProccessExcluded().Where(o => o.deliveryDate.Date == DateTime.Now.Date).ToList();

            foreach (Order order in todaysOrders)
            {
                string orderHour = order.deliveryDate.ToString("HH");
                todaysRevenue += order.total;
                orderHours.Add(orderHour);
            }

            orderHours.Sort();

            Dictionary<int, int> orderCountsByHour = new Dictionary<int, int>();

            // Loop through the order hours and increment the count for each hour
            foreach (string orderHour in orderHours)
            {
                int hour = Int32.Parse(orderHour);
                if (orderCountsByHour.ContainsKey(hour))
                {
                    orderCountsByHour[hour]++;
                }
                else
                {
                    orderCountsByHour[hour] = 1;
                }
            }

            AdminVM model = new AdminVM();
            model.pendingOrders = orderRepository.getAllOrdersInProccessExcluded().Where(o => o.status.Equals(Constants.STATUSS_PENDING)).OrderBy(o => o.deliveryDate).Take(5).ToList();
            model.user = userRepository.FindById(userID);

            model.categories = JsonConvert.SerializeObject(new List<string> { Constants.CHIP_TUNING, Constants.GEAR_BOXES, Constants.ENGINE_PARTS, Constants.CAR_INTERIOR, Constants.EXHAUST_SYSTEM });

            model.raceChipsCount = productRepository.getCategoryItemsCount(Constants.CHIP_TUNING);
            model.carInteriorCount = productRepository.getCategoryItemsCount(Constants.CAR_INTERIOR);
            model.ExhaustSystemsCount = productRepository.getCategoryItemsCount(Constants.EXHAUST_SYSTEM);
            model.gearBoxesCount = productRepository.getCategoryItemsCount(Constants.GEAR_BOXES);
            model.enginePartsCount = productRepository.getCategoryItemsCount(Constants.ENGINE_PARTS);

            model.todaysSales = todaysOrders.Count;
            model.todaysRevenue = Math.Round(todaysRevenue,2);
            model.allSales = orderRepository.getAllOrdersInProccessExcluded().Count();
            model.allRevenue = Math.Round(orderRepository.getAllOrdersInProccessExcluded().Select(o => o.total).Sum(),2);

            model.todaysOrdersHours = JsonConvert.SerializeObject(orderHours);
            model.todaysOrdersCounts = JsonConvert.SerializeObject(new List<int>(orderCountsByHour.Values));

            model.orders = orderRepository.getAllOrdersInProccessExcluded();

            model.cardPayments = orderRepository.getAllOrdersInProccessExcluded().Where(x => x.paymentMethod.ToLower().Equals("card")).ToList().Count;
            model.cashPayments = orderRepository.getAllOrdersInProccessExcluded().Where(x => !x.paymentMethod.ToLower().Equals("card")).ToList().Count;

            List<string> orderDates = orderRepository.GetPurchaseDates(model.orders);

            model.orderDates = JsonConvert.SerializeObject(orderDates);
            model.ordersCountByDate = JsonConvert.SerializeObject(orderRepository.GetPurchaseCountsByDate(orderDates, model.orders));
            model.ordersValueByDate = JsonConvert.SerializeObject(orderRepository.GetTotalPriceByDate(orderDates, model.orders));
            return model;
        }

        public CartProductVM createCartProductVM(Product product , int  quantity)
        {
            CartProductVM model = new CartProductVM();
            model.product = product;
            model.Quantity = quantity;
            return model;
        }
        public CartVM createCartVM(List<string> cookieIds , int userID)
        {
            CartVM model = new CartVM();
            List<CartProductVM> items = new List<CartProductVM>();
            double totalPrice = 0.0;

            //this if statement checks wether theres a logged user and depending of that the products and total sum changes
            if (userID <= 0)
            {
                foreach (var item in cookieIds)
                {
                    Product product = productRepository.findById(int.Parse(item.Split(Constants.PLUS)[0]));
                    int quantity = int.Parse(item.Split(Constants.PLUS)[1]);
                    items.Add(createCartProductVM(product,quantity));
                    totalPrice += product.price * quantity;
                }
            }
            else
            {
                items = modelMapper.mapCartItemsDictionaryToCartProductVM(productRepository.getAllCartItemsQuantitiesForUser(userID));
                foreach (var item in items)
                {
                    totalPrice += item.product.price * item.Quantity;
                }
            }

            model.products = items;
            model.totalPrice = Math.Round(totalPrice, 2);

            return model;
        }

        public EditProfileVM createEditProfileVM(int id , int userID)
        {
            EditProfileVM model = new EditProfileVM();
            model.user = userRepository.FindById(userID);
            model.userID = userID;
            return model;
        }

        public ProfileVM createProfileVM(int id)
        {
            ProfileVM model = new ProfileVM();
            User user = userRepository.FindById(id);

            model.fullName = $"{user.firstName} {user.lastName}";
            model.email = user.email;
            model.address = user.address;
            model.userID = id;

            model.orders = orderRepository.getOrdersByUserId(id);

            return model;   
        }

        public ViewModel.UserVM.UpdateVM createUserUpdateVM(int id , int userID)
        {
            User user = userRepository.FindById(id);
            ViewModel.UserVM.UpdateVM model = new ViewModel.UserVM.UpdateVM();

            model.id = user.ID;
            model.firstName = user.firstName;
            model.lastName = user.lastName;
            model.email = user.email;
            model.address = user.address;
            model.isAdmin = user.isAdmin;
            model.userID = userID;

            return model;
        }


        //--------------------------------------------------------------------------------------
        //-------------------------------|| PRODUCT VIEW MODELS ||------------------------------


        public DetailsVM createDetailsVM(int id , string fullName)
        {
            DetailsVM model = new DetailsVM();
            model.item = productRepository.findById(id);
            model.comments = commentRepository.getComments(id);
            model.fullName=fullName;

            return model;
        }

        public IndexVM createIndexVM(string searchInput, string category, int minSortingValue, int maxSortingValue)
        {
            IndexVM model = new IndexVM();

            model.allItemsCount = productRepository.getProducts(Constants.ALL).Count;

            model.raceChipsCount = productRepository.getCategoryItemsCount(Constants.CHIP_TUNING);
            model.carInteriorCount = productRepository.getCategoryItemsCount(Constants.CAR_INTERIOR);
            model.ExhaustSystemsCount = productRepository.getCategoryItemsCount(Constants.EXHAUST_SYSTEM);
            model.gearBoxesCount = productRepository.getCategoryItemsCount(Constants.GEAR_BOXES);
            model.enginePartsCount = productRepository.getCategoryItemsCount(Constants.ENGINE_PARTS);

            model.categoryType = category;
            model.minSortingValue = minSortingValue;
            model.maxSortingValue = maxSortingValue;

            //this method checks if the input is from the search panel and if it is sorts the items containing only items
            //containing the input
            if (searchInput != null)
            {
                model.searchInput = searchInput;
                model.products = productRepository.getByProductName(searchInput);
            }
            else
            {
                model.products = productRepository.getProducts(category);
            }


            //here is the price sorting
            if ((maxSortingValue == 0 && minSortingValue == 0)
                || maxSortingValue == minSortingValue ||
                (minSortingValue > maxSortingValue && maxSortingValue != 0))// if no elements are given or they are equal or min is higher
            {
                model.products = model.products.OrderBy(x => x.price).ToList();
            }
            else
            {
                if (maxSortingValue == 0)// if no max value
                {
                    model.products = model.products
                        .Where(x => x.price >= minSortingValue)
                        .OrderBy(x => x.price)
                        .ToList();
                }
                else
                {
                    model.products = model.products
                        .Where(x => minSortingValue <= x.price && x.price <= maxSortingValue)
                        .OrderBy(x => x.price)
                        .ToList();
                }
            }

            return model;   
        }

        internal ProductsDisplayVM createProductsDisplayVM(int userID)
        {
            ProductsDisplayVM model = new ProductsDisplayVM();
            model.user = userRepository.FindById(userID);
            model.products = productRepository.getAll();
            return model;
        }

        public UsersDisplayVM createUserDisplayVM(int userID)
        {
            UsersDisplayVM model = new UsersDisplayVM();
            model.user = userRepository.FindById(userID);
            model.users = userRepository.GetAll().Where(x => x.ID != userID).ToList();
            return model;
        }

        public ViewModel.UserVM.OrderDisplayVM createOrderDisplayForUserVM(int userId , bool pendingOnly)
        {
            ViewModel.UserVM.OrderDisplayVM model = new ViewModel.UserVM.OrderDisplayVM();
            model.user = userRepository.FindById(userId);
            model.orders = pendingOnly ? orderRepository.getAllPendingOrders() : orderRepository.getAllOrdersInProccessExcluded().OrderBy(o => o.status).ToList();

            return model;
        }

        public ViewModel.ProductVM.UpdateVM createUpdateVM(int id, string fromAction ,int userID)
        {
            Product product = productRepository.findById(id);
            ViewModel.ProductVM.UpdateVM model = new ViewModel.ProductVM.UpdateVM();

            model.fromAction = fromAction;
            model.description = product.description;
            model.name = product.name;
            model.price = product.price;
            model.id = product.ID;
            model.typeOfProduct = product.typeOfProduct;
            model.userID = userID;

            return model;
        }


        //--------------------------------------------------------------------------------------
        //--------------------------------|| ORDER VIEW MODELS ||-------------------------------


        public CreateOrderVM createOrderVM(List<string> cookieIds, string deliveryType , int userID)
        {
            CreateOrderVM model = new CreateOrderVM();
            //this if statements checks for logged user and fills the model with products
            if (userID > 0)
            {
                model.orderProducts = modelMapper.mapCartItemsDictionaryToCartProductVM(productRepository.getAllCartItemsQuantitiesForUser(userID));
            }
            else
            {
                List<CartProductVM> items = new List<CartProductVM>();
                foreach (var item in cookieIds)
                {
                    Product product = productRepository.findById(int.Parse(item.Split(Constants.PLUS)[0]));
                    int quantity = int.Parse(item.Split(Constants.PLUS)[1]);
                    items.Add(createCartProductVM(product, quantity));
                }
                model.orderProducts = items;
            }

            //this loop gets the total sum, the products quantities and the product ids all in one string
            foreach (var item in model.orderProducts)
            {
                if (string.IsNullOrEmpty(model.orderProductsIDs))
                {
                    model.orderProductsIDs = $"{item.product.ID} ";
                }
                else
                {
                    model.orderProductsIDs += $"{item.product.ID} ";
                }

                if (string.IsNullOrEmpty(model.productQuantities))
                {
                    model.productQuantities = $"{item.Quantity} ";
                }
                else
                {
                    model.productQuantities += $"{item.Quantity} ";
                }
                
                if (model.total == 0)
                {
                    model.total = item.product.price * item.Quantity;
                }
                else
                {
                    model.total += item.product.price * item.Quantity;
                }
                
            }
            model.orderProductsIDs = model.orderProductsIDs.Substring(0, model.orderProductsIDs.Length - 1);
            model.productQuantities = model.productQuantities.Substring(0, model.productQuantities.Length - 1);
            model.subtotal = Math.Round(model.total, 2);
            model.total = Math.Round(model.total, 2);

            //if the payment method is fast shipping the total sum gets 12 bgn more
            if (!deliveryType.Equals(Constants.FREE))
                model.total += 12;

            model.deliveryType = deliveryType;

            return model;
        }


        public ViewModel.OrderVM.OrderDisplayVM createOrderDisplayVM(int id)
        {
            Order order = orderRepository.findOrderById(id);
            ViewModel.OrderVM.OrderDisplayVM model = new ViewModel.OrderVM.OrderDisplayVM();

            if (order.userID > 0)
                productRepository.removeCartItemsForUser(order.userID);

            model.ID = order.ID;
            model.date = order.deliveryDate.ToShortDateString();
            model.total = Math.Round(order.total, 2);
            model.paymentMethod = order.paymentMethod;

            model.fullName = order.status;
            model.addressOne = order.addressOne;
            model.city = order.city;
            model.postcode = order.postcode;

            List<CartProductVM> cartProductVMs = new List<CartProductVM>();
            int[] quantities = order.productQuantities.Split(Constants.SINGLE_WHITE_SPACE).Select(x => int.Parse(x)).ToArray();

            for(int i = 0; i < productRepository.getProductByStringIds(order.productIDs).Count; i++)
            {
                cartProductVMs.Add(createCartProductVM(productRepository.getProductByStringIds(order.productIDs)[i] , quantities[i]));
            } 

            model.products = cartProductVMs;
            model.deliveryType = order.deliveryType;
            model.subtotal = order.deliveryType.Equals(Constants.FREE) ? Math.Round(order.total, 2)  : Math.Round(order.total - 12, 2);

            return model;
        }

        //--------------------------------------------------------------------------------------
    }
}
