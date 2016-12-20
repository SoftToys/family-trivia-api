using FamilyTrivia.Contracts;
using FamilyTrivia.Contracts.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FamilyTrivia.Services
{
    public class GamesRepositoryService : IGamesRepositoryService
    {

        public async Task<Guid> AddUpdate(TriviaGame game)
        {
            // TODO: read from persistency and return the game
            throw new NotImplementedException();
        }

        public Task<TriviaGame> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TriviaGame>> GetByOwner(string owner)
        {
            // implement   
            throw new NotImplementedException();
        }

        public ContentResult GetByQuestionId(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserRating>> GetUserRating()
        {
            throw new NotImplementedException();
        }
    }
}
