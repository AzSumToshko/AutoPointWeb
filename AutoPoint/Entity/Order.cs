namespace AutoPoint.Entity
{
    /// <summary>
    ///         This is the class that stores all the information about the Order that a person have made
    /// </summary>
    public class Order : BaseEntity
    {
        public int userID { get; set; }
        public string productIDs { get; set; }
        public string productQuantities { get; set; }
        public int productsCount { get; set; }
        public string fullName { get; set; }
        public string companyName { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string addressOne { get; set; }
        public string addressTwo { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }
        public string details { get; set; }
        public string paymentMethod { get; set; }
        public string deliveryType { get; set; }
        public double total { get; set; }
        public DateTime deliveryDate { get; set; }
		public string status { get; set; }

	}
}
