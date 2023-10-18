using AutoPoint.ViewModel.ProductVM;
using AutoPoint.Entity;
using System.ComponentModel.DataAnnotations;

namespace AutoPoint.ViewModel.OrderVM
{
    public class CreateOrderVM
    {
        public List<CartProductVM> orderProducts { get; set; }
        public double subtotal { get; set; }
        public string productQuantities { get; set; }



        public string orderProductsIDs { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        public string companyName { get; set; }//
        [Required]
        public string phoneNumber { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string addressOne { get; set; }
        public string addressTwo { get; set; }//
        [Required]
        public string city { get; set; }
        [Required]
        public string postcode { get; set; }
        public string details { get; set; }//
        [Required]
        public string paymentMethod { get; set; }
        [Required]
        public string deliveryType { get; set; }

        public double total { get; set; }
    }
}
