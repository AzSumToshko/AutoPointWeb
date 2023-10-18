using AutoPoint.DataBaseAccess;
using AutoPoint.Entity;
using AutoPoint.Tools;
using System.Data.Entity;

namespace AutoPoint.Repository
{
    public class ProductRepository
    {
        private readonly Context context;

        public ProductRepository()
        {
            this.context = new Context();
        }

        /// <summary>
        ///         This method returns a list of products where all the products names
        ///         that contain a specific char sequence
        /// </summary>
        public List<Product> getByProductName(string productName)
        {
            return context.Products
                .Where(x => x.name.ToLower().Contains(productName.ToLower()))
                .OrderBy(x => x.price)
                .ToList();
        }

        /// <summary>
        ///         This method returns a list of products all from specific type
        /// </summary>
        public List<Product> getProducts(string filter)
        {
            switch (filter)
            {
                case "Engine parts":
                    return context.Products.Where(x => x.typeOfProduct.Equals(Constants.ENGINE_PARTS)).ToList();

                case "Gear boxes":
                    return context.Products.Where(x => x.typeOfProduct.Equals(Constants.GEAR_BOXES)).ToList();

                case "Car interior":
                    return context.Products.Where(x => x.typeOfProduct.Equals(Constants.CAR_INTERIOR)).ToList();

                case "Race chips":
                    return context.Products.Where(x => x.typeOfProduct.Equals(Constants.CHIP_TUNING)).ToList();

                case "Exhaust system":
                    return context.Products.Where(x => x.typeOfProduct.Equals(Constants.EXHAUST_SYSTEM)).ToList();

                default:
                    return context.Products.ToList();

            }
        }

        /// <summary>
        ///         This method returns count of products from specific category
        /// </summary>
        public int getCategoryItemsCount(string categoryType)
        {
            return context.Products.Where(x => x.typeOfProduct.Equals(categoryType)).ToList().Count();
        }

        /// <summary>
        ///         This method returns a product found by his unique id
        /// </summary>
        public Product findById(int id)
        {
            return context.Products.Find(id);
        }

        /// <summary>
        ///         This method returns a list with all the products
        /// </summary>
        public List<Product> getAll()
        {
            return context.Products.ToList();
        }

        /// <summary>
        ///         This method deletes product form the database
        /// </summary>
        public void DeleteProduct(int id)
        {
            Product product = context.Products.Find(id);

            if (product != null)
            {
                //this loop deletes all the comments
                foreach (var item in context.Comments)
                {
                    if (item.productID == id)
                    {
                        context.Comments.Remove(item);
                    }
                }

                //this loop deletes all aperances of the item in the users carts
                foreach (var item in context.CartProducts)
                {
                    if (item.productID == id)
                    {
                        context.CartProducts.Remove(item);
                    }
                }
                context.Products.Remove(product);
                context.SaveChanges();
            }
        }

        /// <summary>
        ///         removeAll removes all the products from the database
        /// </summary>
        public void removeAll()
        {
            foreach (var item in context.Products)
            {
                context.Products.Remove(item);
            }
            context.SaveChanges();
        }

        /// <summary>
        ///         removeAllCartItems removes all the items in every users cart
        /// </summary>
        public void removeAllCartItems()
        {
            foreach(var item in context.CartProducts)
            {
                context.CartProducts.Remove(item);
            }
            context.SaveChanges();
        }

        /// <summary>
        ///         This method inserts product into the database
        /// </summary>
        public void InsertProduct(Product product)
        {
            if (product != null)
            {
                context.Products.Add(product);
                context.SaveChanges();
            }
        }

        /// <summary>
        ///         This method updates the product from the database
        /// </summary>
        public void updateProduct(Product product)
        {
            if (product != null)
            {
                context.Entry(product).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        /// <summary>
        ///         This method adds product to users cart
        /// </summary>
        public void addCartItem(CartProduct item)
        {
            if (item != null)
            {
                CartProduct cartItem = context.CartProducts
                                            .FirstOrDefault(x => x.productID == item.productID 
                                                            && x.userID == item.userID);
                if (cartItem != null)
                {
                    
                    cartItem.productQuantity += item.productQuantity;
                    context.Entry(cartItem).State = EntityState.Modified;
                }
                else
                {
                    context.CartProducts.Add(item);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        ///         This method removes item from the users cart
        /// </summary>
        public void RemoveProductFromUser(int userID , int productID)
        {
            CartProduct cartProduct = 
                context.CartProducts
                .FirstOrDefault(x => x.productID == productID && x.userID == userID);

            if (cartProduct != null)
            {
                context.CartProducts.Remove(cartProduct);
                context.SaveChanges();
            }
        }

        /// <summary>
        ///         This method returns a dictionary with the products and theyr quantities
        /// </summary>
        public Dictionary<Product, int> getAllCartItemsQuantitiesForUser(int ID)
        {
            Dictionary<Product, int> items = new Dictionary<Product, int>();
            List<CartProduct> cartItems = context.CartProducts.Where(x => x.userID == ID).ToList();
            foreach (var item in cartItems)
            {
                items.Add(findById(item.productID), item.productQuantity);
            }

            return items;
        }

        /// <summary>
        ///         This method returns a string representing all the products ids.
        /// </summary>
        public string getProductIdsToString(List<Product> items)
        {
            string result = Constants.EMPTY_STRING;

            foreach (var item in items)
            {
                result += item.ID + Constants.SINGLE_WHITE_SPACE;
            }

            return result.Substring(0, result.Length-1);
        }

        /// <summary>
        ///         This method returns a list of products found by string of ids
        /// </summary>
        public List<Product> getProductByStringIds(string productIDs)
        {
            List<Product> products = new List<Product>();
            int[] ids = productIDs.Split(Constants.SINGLE_WHITE_SPACE).Select(n => int.Parse(n)).ToArray();
            foreach (var item in ids)
            {
                products.Add(context.Products.Find(item));
            }

            return products;
        }

        /// <summary>
        ///         This method removes the items in the users cart
        /// </summary>
        public void removeCartItemsForUser(int userID)
        {
            List<CartProduct> cartProducts = context.CartProducts.Where(x => x.userID == userID).ToList();
            foreach (var item in cartProducts)
            {
                context.CartProducts.Remove(item);
            }

            context.SaveChanges();
        }

        /// <summary>
        ///         This method creates and returns cart product (adds item to the users cart)
        /// </summary>
        public CartProduct createCartProduct(int id , int productQuantity , int userID)
        {
            CartProduct item = new CartProduct();

            item.productID = id;
            item.userID = userID;
            item.productQuantity = productQuantity;

            return item;
        }
    }
}
