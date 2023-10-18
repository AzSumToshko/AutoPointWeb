using AutoPoint.ViewModel.CommentVM;
using AutoPoint.ViewModel.OrderVM;
using AutoPoint.ViewModel.ProductVM;
using AutoPoint.ViewModel.UserVM;
using AutoPoint.Entity;
using AutoPoint.Repository;

namespace AutoPoint.Tools
{

    /// <summary>
    ///         This is the class that is going to convert
    ///         the View Model to a entity type
    /// </summary>
    public class ModelMapper
    {
        private readonly ProductRepository productRepository;
        private readonly UserRepository userRepository;
        public ModelMapper()
        {
            productRepository = new ProductRepository();
            userRepository = new UserRepository();
        }


        //--------------------------------|| USER MAPPING ||--------------------------------


        public User mapCreateVMToUser(RegisterVM model)
        {
            if (model == null)
                return null;

            User user = new User();

            user.firstName = model.firstName;
            user.lastName = model.lastName;
            user.email = model.email;
            user.password = model.password;
            user.isAdmin = model.isAdmin;

            return user;
        }

        public User mapCreateVMToUser(ViewModel.UserVM.CreateVM model)
        {
            if (model == null)
                return null;

            User user = new User();

            user.firstName = model.firstName;
            user.lastName = model.lastName;
            user.email = model.email;
            user.password = model.password;
            user.address = model.address;
            user.isAdmin = model.isAdmin;

            return user;
        }

        public User mapUpdateVMToUser(ViewModel.UserVM.UpdateVM model)
        {
            User user = userRepository.FindById(model.id);

            user.firstName = model.firstName;
            user.lastName = model.lastName;
            user.address = model.address;
            user.email = model.email;
            user.isAdmin = model.isAdmin;

            if (!string.IsNullOrEmpty(model.password))
            {
                user.password = model.password;
            }

            return user;
        }


        //----------------------------------------------------------------------------------
        //-------------------------------|| COMMENT MAPPING ||------------------------------


        public Comment mapCommentVMToComment(CreateCommentVM model)
        {
            Comment comment = new Comment();

            comment.productID = model.productID;
            comment.fullName = model.fullName;
            comment.message = model.message;
            
            return comment;
        }


        //----------------------------------------------------------------------------------
        //-------------------------------|| PRODUCT MAPPING ||------------------------------


        public Product mapCreateVMToProduct(ViewModel.ProductVM.CreateVM model, IFormFile file)
        {
            //sus . na price ne priema trqq da e ,
            Product product = new Product();

            using (var memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                product.image = memoryStream.ToArray();
            };

            product.price = model.price;
            product.name = model.name;
            product.description = model.description;
            product.typeOfProduct = model.typeOfProduct;

            return product;
        }

        public Product mapUpdateVMToProduct(ViewModel.ProductVM.UpdateVM model, IFormFile file)
        {
            Product product = productRepository.findById(model.id);

            product.name = model.name;
            product.description = model.description;
            product.price = model.price;
            product.typeOfProduct = model.typeOfProduct;

            if (model.file != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    product.image = memoryStream.ToArray();
                };
            }

            return product;
        }

        public List<CartProductVM> mapCartItemsDictionaryToCartProductVM(Dictionary<Product , int> items)
        {
            List < CartProductVM > cart = new List <CartProductVM >();
            foreach (var item in items)
            {
                CartProductVM cartProductVM = new CartProductVM();
                cartProductVM.product = item.Key;
                cartProductVM.Quantity = item.Value;
                cart.Add(cartProductVM);
            }
            return cart;
        }


        //----------------------------------------------------------------------------------
        //--------------------------------|| ORDER MAPPING ||-------------------------------


        public Order mapOrderVMToOrder(CreateOrderVM model , int userID)
        {
            Order order = new Order();

            order.userID = userID;
            order.productsCount = model.orderProductsIDs.Split(Constants.SINGLE_WHITE_SPACE).Count();
            order.productIDs = model.orderProductsIDs;
            order.productQuantities = model.productQuantities;
            order.fullName = model.firstName + Constants.SINGLE_WHITE_SPACE + model.lastName;
            order.companyName = model.companyName;
            order.email = model.email;
            order.phoneNumber = model.phoneNumber;
            order.addressOne = model.addressOne;
            order.addressTwo = model.addressTwo;
            order.postcode = model.postcode;
            order.city = model.city;
            order.details = model.details;
            order.paymentMethod = model.paymentMethod;
            order.deliveryType = model.deliveryType;
            order.total = model.total;
            order.status = Constants.STATUSS_PENDING;

            if (model.deliveryType.Equals(Constants.FREE))
                order.deliveryDate = DateTime.Now.AddDays(7);
            else
                order.deliveryDate = DateTime.Now.AddDays(2);

            return order;
        }


        //----------------------------------------------------------------------------------
    }
}
