using System;
using System.Collections.Generic;
using FamilyTrivia.Contracts.Models;
using System.Threading.Tasks;

namespace FamilyTrivia.Contracts
{
    public interface IGamesRepositoryService
    {
        Task<Guid> AddUpdate(TriviaGame game);
        TriviaGame GetById(Guid id);
        IEnumerable<TriviaGame> GetByOwner(string owner);
    }
}