using AutoPoint.Entity;

namespace AutoPoint.ViewModel.UserVM
{
    public class AdminVM
    {
		public List<Order> orders { get; set; }
		public List<Order> pendingOrders { get; set; }
        public User user { get; set; }

        public string categories { get; set; }

        public int raceChipsCount { get; set; }
        public int carInteriorCount { get; set; }
        public int ExhaustSystemsCount { get; set; }
        public int gearBoxesCount { get; set; }
        public int enginePartsCount { get; set; }

        public int todaysSales { get; set; }
        public int allSales { get; set; }
        public double todaysRevenue { get; set; }
        public double allRevenue { get; set; }

        public string todaysOrdersHours { get; set; }
        public string todaysOrdersCounts { get; set; }

        public string orderDates { get; set; }
		public string ordersCountByDate { get; set; }
		public string ordersValueByDate { get; set; }

        public int cashPayments { get; set; }
        public int cardPayments { get; set; }
    }
}
