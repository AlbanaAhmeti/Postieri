using Postieri.Data;
using Postieri.DTOs;


namespace Postieri.Services
{
    public class CourierService : ICourierService
    {
        private readonly DataContext _dbcontext;

        public CourierService(DataContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
     
        public void UpdateStatus(Guid orderId, string status)
        {
            var order = _dbcontext.Orders.Find(orderId);
            if(order != null)
            {
                 order.Status = status;
                _dbcontext.SaveChanges();
            }
        }
        public bool AcceptOrder(Guid order, Guid courierId)
        {
            var orderId = _dbcontext.Orders.Find(order);

            if (orderId != null && orderId.CourierId == courierId)
            {
                orderId.Status = "accepted";
                _dbcontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool DeclineOrder(Guid orderId, Guid courierId)
        {
            var order = _dbcontext.Orders.Find(orderId);

            if (order != null && order.CourierId == courierId)
            {
                order.Status = "declined";
                order.CourierId = null;
                _dbcontext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
