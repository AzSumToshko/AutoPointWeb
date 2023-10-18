namespace AutoPoint.Entity
{
    /// <summary>
    ///         This is the class that contains the information about all the users items in theyr carts
    /// </summary>
    public class CartProduct : BaseEntity
    {
        public int productID { get; set; }
        public int userID { get; set; }
        public int productQuantity { get; set; }
    }
}
