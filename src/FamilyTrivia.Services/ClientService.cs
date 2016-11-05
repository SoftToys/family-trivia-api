using FamilyTrivia.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTrivia.Contracts.Models;

namespace FamilyTrivia.Services
{
    public class ClientService : IClientService
    {
        public Task<User> GetOrCreate(string clientMail)
        {
            return Task.FromResult(new User() { Email = clientMail, Id = clientMail });
        }

        public Task<User> UpdateClientDetails(User c)
        {
            return Task.FromResult(c);
        }
    }
}
