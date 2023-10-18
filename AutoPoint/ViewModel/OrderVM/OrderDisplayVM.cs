using AutoPoint.ViewModel.ProductVM;

namespace AutoPoint.ViewModel.OrderVM
{
    public class OrderDisplayVM
    {
        public int ID { get; set; }
        public string date { get; set; }
        public double total { get; set; }
        public string paymentMethod { get; set; }


        public string fullName { get; set; }
        public string addressOne { get; set; }
        public string city { get; set; }
        public string postcode { get; set; }


        public List<CartProductVM> products { get; set; }
        public double subtotal { get; set; }
        public string deliveryType { get; set; }
    }
}
