namespace AutoPoint.ViewModel.ProductVM
{
    public class CreateVM
    {
        public string name { get; set; }
        public double price { get; set; }
        public IFormFile file { get; set; }
        public string typeOfProduct { get; set; }
        public string description { get; set; }
        public int userID { get; set; }
    }
}
