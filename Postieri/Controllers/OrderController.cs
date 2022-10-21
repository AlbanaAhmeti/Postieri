using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Postieri.Data;
using Postieri.DTO;
using Postieri.Models;
using Postieri.Services;

namespace Postieri.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("PostOrder")]
        public ActionResult<List<Order>> PostOrder(OrderDto order)
        {
            _orderService.PostOrder(order);
            return Ok();
        }

        [HttpGet("GetAllOrders")]
        public ActionResult<List<Order>> GetAllOrders()
        {
            return Ok(_orderService.GetAllOrders());
        }

        [HttpGet("GetOrderById")]
        public Order GetOrder(Guid id)
        {
            return _orderService.GetOrder(id);
        }

        [HttpDelete]
        public ActionResult<List<Order>> DeleteOrder(Guid OrderId)
        {
            _orderService.DeleteOrder(OrderId);
            return Ok(_orderService.GetAllOrders());
        }

        [HttpPut("UpdateStatusOfOrder")]
        public ActionResult<List<Order>> setStatus(StatusOrderDto order, Guid courier)
        {
            _orderService.setStatus(order.OrderId, order.Status, courier);
            return Ok();
        }

        [HttpPut("assignCourierToOrder")]
        public ActionResult<List<Order>> assignCourierToOrder(Guid orderId, Guid courierId)
        {
            _orderService.assignCourierToOrder(orderId, courierId);
            return Ok();
        }
        [HttpGet("CalculateSize")]
        public string CalculateSize(double length, double width, double height)
        {
            return _orderService.CalculateSize(length, width, height);

        }
        [HttpGet("getordersbyrole"), Authorize]
        public ActionResult<List<Order>> GetOrders()
        {
            return Ok(_orderService.GetOrdersByRole());
        }
    }
}
