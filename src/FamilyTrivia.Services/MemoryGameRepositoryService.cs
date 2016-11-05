using FamilyTrivia.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTrivia.Contracts.Models;

namespace FamilyTrivia.Services
{
    // Memo service
    public class MemoryGameRepositoryService : IGamesRepositoryService
    {
        private Dictionary<Guid, TriviaGame> _dictionaryDb;

        public MemoryGameRepositoryService()
        {
            _dictionaryDb = new Dictionary<Guid, TriviaGame>();

        }

       
        public TriviaGame GetById(Guid id)
        {
            return _dictionaryDb[id];
        }

        public IEnumerable<TriviaGame> GetByOwner(string owner)
        {
            return _dictionaryDb.Values.Where(tg => owner == null || String.Equals(tg.OwnerId, owner, StringComparison.OrdinalIgnoreCase)).OrderBy(g => g.UpdateTime);
        }

        Task<Guid> IGamesRepositoryService.AddUpdate(TriviaGame game)
        {
            // add a new game
            if (game.Id == Guid.Empty)
            {
                game.Id = Guid.NewGuid();
                game.CreateTime = game.UpdateTime = DateTime.Now;

                _dictionaryDb.Add(game.Id, game);

            }
            // else - update
            else
            {
                _dictionaryDb[game.Id] = game;
                game.UpdateTime = DateTime.Now;                
            }
            return Task.FromResult(game.Id);
        }

        Task<TriviaGame> IGamesRepositoryService.GetById(Guid id)
        {
            return Task.FromResult(_dictionaryDb[id]);
        }

        Task<IEnumerable<TriviaGame>> IGamesRepositoryService.GetByOwner(string owner)
        {
            IEnumerable<TriviaGame> res = _dictionaryDb.Values.ToList();
            return Task.FromResult(res);
        }
    }
}
