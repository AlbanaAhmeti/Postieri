using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
//using System.Web.Http;
using System.Web;
//using System.Runtime.Remoting.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Postieri.Data;
using Postieri.DTO;
using Postieri.Models;
using Postieri.Services;

namespace Postieri.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessIntegrationController : ControllerBase

    {
        private readonly IBusinessIntegrationService _businessIntegration;

        public BusinessIntegrationController(IBusinessIntegrationService businessIntegrationService)
        {
            _businessIntegration = businessIntegrationService;
        }

        [HttpPost("SaveBusiness")]
        public async Task<ActionResult<string>> SaveBusiness(BusinessDto request)
        {
            _businessIntegration.SaveBusiness(request);
            return Ok();
        }
        [HttpGet("GetAllBusinesses")]
        public ActionResult<List<Business>> GetBusinesses()
        {
            return Ok(_businessIntegration.GetBusinesses());
        }
        [HttpGet("GetBusinessesByEmail")]
        public ActionResult<List<Business>> GetBusinessesByEmail(string email)
        {
            return Ok(_businessIntegration.GetBusinessesByEmail(email));
        }
        [HttpGet("GetBusinessesByToken")]
        public ActionResult<List<Business>> GetBusinessByToken(string token)
        {
            return Ok(_businessIntegration.GetBusinessByToken(token));
        }
    }
}
