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
            // TODO: Add to persistency and return the id
            throw new NotImplementedException();
        }
        public TriviaGame GetById(Guid id)
        {
            // TODO: read from persistency and return the game
            throw new NotImplementedException();
        }

        public IEnumerable<TriviaGame> GetByOwner(string owner)
        {
            // TODO: read from persistency and return list 
            throw new NotImplementedException();
        }



    }
}
