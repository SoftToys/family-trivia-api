using FamilyTrivia.Contracts;
using FamilyTrivia.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTrivia.Services
{
    public class GamesRepositoryService : IGamesRepositoryService
    {

        public Guid AddUpdate(TriviaGame game)
        {
            throw new NotImplementedException();
        }
        public TriviaGame GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TriviaGame> GetByOwner(string owner)
        {
            throw new NotImplementedException();
        }



    }
}
