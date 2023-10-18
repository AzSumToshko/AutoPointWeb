using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPoint.Entity
{
    /// <summary>
    ///         This class contains all the comments of all the products
    /// </summary>
    
    [Table("Comments")]
    public class Comment : BaseEntity
    {
        public int productID { get; set; }
        public string fullName { get; set; }
        public string message { get; set; }
    }
}
