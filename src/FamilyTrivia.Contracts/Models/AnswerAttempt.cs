using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTrivia.Contracts.Models
{

    public class AnswerAttempt
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }
        [JsonProperty("isCorrect")]
        public bool IsCorrect { get; set; }
    }
}
