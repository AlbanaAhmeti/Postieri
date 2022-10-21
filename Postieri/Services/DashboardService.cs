using Postieri.Data;
using Postieri.Models;

namespace Postieri.Services
{
    public class DashboardService : IDashboard
    {

        private readonly DataContext _dbcontext;

        public DashboardService(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public double GetTotal()
        {
            var orders = _dbcontext.Orders
                .Where(o=>o.Status =="transfer")
                .ToList();
            double total = 0;

            foreach (var order in orders)
            {
                total =+ order.Price;
            }
            return total;
        }

        public int AvailableCouriers()
        {
           return _dbcontext.Couriers
                .Where(c=>c.IsAvailable==true)
                .ToList()
                .Count();
        }

        public int OrdersInLastThreeMonths()
        {
         return _dbcontext.Orders
                .Where(o => o.Status == "accept" && o.OrderedOn>=DateTime.Now
                .AddMonths(-3))
                .ToList().Count;
        }

        public int AvailableVehicles()
        {
            return _dbcontext.Vehicles
                .Where(v=>v.IsAvailable==true && v.HasDefect==false)
                .ToList().Count;
        }

        public int OrdersAccepted()
        {
            var acceptedOrders = _dbcontext.Orders
                .Where(x => x.Status == "accept")
                .Count();
            return acceptedOrders;
        }
        public int OrdersAtTheCourier()
        {
            var atCourier = _dbcontext.Orders
                .Where(x => x.CourierId != null)
                .Count();
            return atCourier;
        }
        public int LastSevenDays()
        {
            var lastSevenDays = _dbcontext.Orders
                .Where(x => x.Date > DateTime.Now
                .AddDays(-7)).Count();
            return lastSevenDays;
        }
    }
}
