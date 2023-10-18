using System.ComponentModel.DataAnnotations;

namespace AutoPoint.Entity
{
    /// <summary>
    ///         This is the base entity that is going to be inherited from all other entity classes
    /// </summary>
    /// <param name="ID">The identification number that makes all the entities unique</param>
    public class BaseEntity
    {
        [Key]
        public int ID { get; set; }
    }
}
