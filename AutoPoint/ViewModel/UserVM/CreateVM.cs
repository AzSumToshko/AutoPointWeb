namespace AutoPoint.ViewModel.UserVM
{
	public class CreateVM
	{
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public bool isAdmin { get; set; }
        public int userID { get; set; }
    }
}
