using AutoPoint.ViewModel.ProductVM;
using AutoPoint.Entity;

namespace AutoPoint.ViewModel.UserVM
{
    public class CartVM
    {
        public List<CartProductVM> products { get; set; }
        public double totalPrice { get; set; }
    }
}
