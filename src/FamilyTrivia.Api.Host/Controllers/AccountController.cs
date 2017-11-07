using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using FamilyTrivia.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Facebook;
using Facebook;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using FamilyTrivia.Contracts.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FamilyTrivia.Api.Host.Controllers
{
    [EnableCors("Light")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private IClientService _clientService;
        private TokenAuthOptions _tokenOptions;
        private ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger, IClientService clientService, TokenAuthOptions tokenOptions)
        {
            _clientService = clientService;
            _tokenOptions = tokenOptions;
            _logger = logger;

        }


        [HttpPost]
        [Route("FacebookLogin")]
        public async Task<IActionResult> FacebookLogin([FromBody]FacebookAuthResponse facebookAuthResponse)
        {
            var fb = new FacebookClient(facebookAuthResponse.accessToken);
            dynamic me = fb.Get("me?fields=id,name,email");
            
            string clientMail = me["email"];
            string facebookID = me["id"];

            string imgUrl = $"https://graph.facebook.com/{ me["id"] }/picture";
            string lastKnownIP = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            Contracts.Models.User c = await _clientService.GetOrCreate(clientMail);

            await _clientService.UpdateClientDetails(c);

            var token = IssueAuthenticated(c.Id.ToString(), c.Email);
            return Json(new { ErrorCode = 0, ErrorDescription = "Ok", Id = c.Id, token = token, Icon = imgUrl });
        }

        private string IssueAuthenticated(string id, string email)
        {
            TimeSpan expiration = TimeSpan.FromDays(30);
            long expirationSeconds = (long)expiration.TotalSeconds;

            ClaimsIdentity identity = new ClaimsIdentity(new GenericIdentity(email, "TokenAuth"),
                new[] {
                    new System.Security.Claims.Claim(ClaimTypes.Sid,id.ToString(), ClaimValueTypes.Integer),
                    new System.Security.Claims.Claim(ClaimTypes.Email,email.ToString(), ClaimValueTypes.String),
                    new System.Security.Claims.Claim("id",email, ClaimValueTypes.String),
                    new System.Security.Claims.Claim("UserIP",Request.HttpContext.Connection.RemoteIpAddress.ToString(), ClaimValueTypes.String)
                });

            return BuildJWT(expirationSeconds, identity);
        }


        private string BuildJWT(long expirationSeconds, ClaimsIdentity identity)
        {
            var handler = new JwtSecurityTokenHandler();
            var securityToken = handler.CreateJwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                signingCredentials: _tokenOptions.SigningCredentials,
                subject: identity,
                expires: DateTime.Now.AddSeconds(expirationSeconds)
                );
            return handler.WriteToken(securityToken);
        }


    }


}
