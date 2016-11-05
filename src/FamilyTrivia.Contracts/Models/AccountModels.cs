using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace FamilyTrivia.Contracts.Models
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class FacebookAuthResponse
    {
        public string accessToken { get; set; }
        public string UserId { get; set; }
    }

    public class AuthRequest
    {
        public string Phone { get; set; }
    }

    public class VerifyCodeRequest
    {
        public string Code { get; set; }
    }

    public class TokenAuthOptions
    {      
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}