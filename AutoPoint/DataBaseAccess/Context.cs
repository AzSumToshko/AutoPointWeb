using AutoPoint.Entity;
using AutoPoint.Tools;
using System.Data.Entity;

namespace AutoPoint.DataBaseAccess
{
    /// <summary>
    ///         This class that initializes the database inheriting the DbContext class which 
    ///         posseses all the methods that are going to be used in the repositories later.
    /// </summary>
    /// <param name="Users">The container that stores the information about the users in the database table</param>
    /// <param name="Products">The container that stores the information about the products in the database table</param>
    /// <param name="Comments">The container that stores the information about the comments in the database table</param>
    /// <param name="CartProducts">The container that stores the information about the cart products in the database table</param>
    /// <param name="Orders">The container that stores the information about the orders in the database table</param>
    /// <param name="Constructor">
    ///         The constructor uses the base constructor to set the connection string and inside of the constructor
    ///         the tables are being initialized
    /// </param>
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CartProduct> CartProducts { get; set; }
        public DbSet<Order> Orders { get; set; }

        public Context() : base(Constants.CONNECTION_STRING)
        {
            Users = this.Set<User>();
            Products = this.Set<Product>();
            Comments = this.Set<Comment>();
            CartProducts = this.Set<CartProduct>();
            Orders = this.Set<Order>();
        }
    }
}
