using AutoPoint.Entity;

namespace AutoPoint.ViewModel.ProductVM
{
    public class IndexVM
    {
        public string categoryType { get; set; }
        public List<Product> products { get; set; }
        public string searchInput { get; set; }

        public int allItemsCount { get; set; }
        public int raceChipsCount { get; set; }
        public int carInteriorCount { get; set; }
        public int ExhaustSystemsCount { get; set; }
        public int gearBoxesCount { get; set; }
        public int enginePartsCount { get; set; }

        public int minSortingValue { get; set; }
        public int maxSortingValue { get; set; }
    }
}
