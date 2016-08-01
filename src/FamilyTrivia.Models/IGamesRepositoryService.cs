using System;
using System.Collections.Generic;
using FamilyTrivia.Contracts.Models;

namespace FamilyTrivia.Contracts
{
    public interface IGamesRepositoryService
    {
        Guid AddUpdate(TriviaGame game);
        TriviaGame GetById(Guid id);
        IEnumerable<TriviaGame> GetByOwner(string owner);
    }
}