using Microsoft.AspNetCore.Mvc;
using Postieri.DTO;
using Postieri.Models;
using Postieri.Services;

namespace Postieri.Interfaces
{
    public interface IBusinessIntegrationService
    {
        bool SaveBusiness(BusinessDto request);
        Order GetOrders(Guid id);
        ActionResult<List<Order>> GetAllOrders();
        bool PostOrder(OrderDto order);
        List<Business> GetBusinesses();
        List<Business> GetBusinessesByEmail(string email);
        List<Business> GetBusinessByToken(string token);
    }
}
