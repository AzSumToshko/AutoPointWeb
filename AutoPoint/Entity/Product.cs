using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPoint.Entity
{
    /// <summary>
    ///         This is the class that stores the information about the products
    /// </summary>
    [Table("Products")]
    public class Product : BaseEntity
    {
        public string name { get; set; }
        public double price { get; set; }
        public string typeOfProduct { get; set; }
        public string description { get; set; }
        public byte[] image { get; set; }
    }
}
