using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTrivia.Contracts.Models;

namespace FamilyTrivia.Contracts
{
    public interface IClientService
    {
        Task<User> UpdateClientDetails(User c);
        Task<User> GetOrCreate(string clientMail);
    }
}
