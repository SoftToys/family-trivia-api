using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTrivia.Contracts.Models
{
    public class TriviaItem
    {        
        public TriviaQuestion ItemQuestion { get; set; }

        public IEnumerable<Answer> Answers { get; set; }

    }
}
