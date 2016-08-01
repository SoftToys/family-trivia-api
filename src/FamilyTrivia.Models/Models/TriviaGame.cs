using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTrivia.Contracts.Models
{
    public class TriviaGame
    {
        public Guid Id { get; set; }
        public IEnumerable<TriviaItem> Items { get; set; }
    }
}
