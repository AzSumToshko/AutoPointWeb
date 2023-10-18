using System.ComponentModel.DataAnnotations.Schema;

namespace AutoPoint.Entity
{
    /// <summary>
    ///         This is the class that stores the infromation about the users
    /// </summary>
    [Table("Users")]
    public class User : BaseEntity
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public bool isAdmin { get; set; }
    }
}
