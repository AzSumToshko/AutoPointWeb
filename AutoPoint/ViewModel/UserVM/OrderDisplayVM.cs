using AutoPoint.Entity;

namespace AutoPoint.ViewModel.UserVM
{
    public class OrderDisplayVM
    {
        public User user { get; set; }
        public List<Order> orders { get; set; }
    }
}
