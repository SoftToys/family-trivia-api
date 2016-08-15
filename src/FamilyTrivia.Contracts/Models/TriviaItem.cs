using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTrivia.Contracts.Models
{
    public class TriviaItem
    {    
        [JsonProperty("_question")]    
        public TriviaQuestion ItemQuestion { get; set; }

        [JsonProperty("_answers")]
        public IEnumerable<Answer> Answers { get; set; }

    }
}
