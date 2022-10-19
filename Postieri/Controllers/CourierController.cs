﻿using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Postieri.DTOs;
using Postieri.Models;

namespace Postieri.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourierController : ControllerBase
    {
        public ICourierService _courierService;

        public CourierController(ICourierService courierService)
        {
            _courierService = courierService;
        }

        [HttpPut("changestatus")]
        public ActionResult<List<StatusOrderDto>> ChangeStatus(Guid orderId, string status)
        {
             _courierService.UpdateStatus(orderId, status);
            return Ok();
        }
        [HttpPut("accept-order")]
        public ActionResult<List<StatusOrderDto>> AcceptOrder(Guid order, Guid courierId)
        {
            _courierService.AcceptOrder(order, courierId);
            return Ok();
        }
        [HttpPut("decline-order")]
        public ActionResult<List<StatusOrderDto>> DeclineOrder(Guid orderId, Guid courierId)
        {
            _courierService.DeclineOrder(orderId, courierId);
            return Ok();
        }

    }
}
