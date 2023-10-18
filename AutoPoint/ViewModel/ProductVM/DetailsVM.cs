using AutoPoint.Entity;

namespace AutoPoint.ViewModel.ProductVM
{
    public class DetailsVM
    {
        public Product item { get; set; }
        public List<Comment> comments { get; set; }
        public string fullName { get; set; }
    }
}
