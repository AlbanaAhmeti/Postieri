using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Postieri.Data;
using Postieri.DTO;
using Postieri.Mappings;
using Postieri.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Postieri.Services
{
    public class BusinessIntegrationService : IBusinessIntegrationService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public BusinessIntegrationService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public bool SaveBusiness(BusinessDto request)
        {
            if (request == null)
            {
                return false;
            }

            var alreadyExist = _context.Businesses
                .Where(x => x.BusinessName == request.BusinessName & x.Email == request.Email)
                .FirstOrDefault();

            if (alreadyExist != null)
            {
                return false;
            }

            var business = new Business();
            business.BusinessName = request.BusinessName;
            business.Email = request.Email;
            business.PhoneNumber = request.PhoneNumber;
            business.BusinessToken = CreateToken(business);

            _context.Businesses.Add(business);
            _context.SaveChangesAsync();
            return true;

        }
        private string CreateToken(Business business)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, business.BusinessName),
                new Claim(ClaimTypes.Email, business.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
        public List<Business> GetBusinesses()
        {
            return _context.Businesses.ToList();
        }
        public List<Business> GetBusinessesByEmail(string email)
        {
            return _context.Businesses.Where(x => x.Email.ToLower().Equals(email.ToLower())).ToList();
        }
        public List<Business> GetBusinessByToken(string token)
        {
            return _context.Businesses.Where(x => x.BusinessToken.Equals(token)).ToList();
        }
    }
}
