using AutoPoint.Entity;

namespace AutoPoint.ViewModel.UserVM
{
	public class ProfileVM
	{
		public List<Order> orders { get; set; }
        public string fullName { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public int userID { get; set; }
    }
}
