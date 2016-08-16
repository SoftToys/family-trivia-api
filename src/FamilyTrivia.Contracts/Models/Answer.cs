using Newtonsoft.Json;

namespace FamilyTrivia.Contracts.Models
{
    public class Answer
    {
        [JsonProperty("txt")]
        public string Text { get; set; }
        [JsonProperty("isCorrect")]
        public bool IsCorrect { get; set; }

    }
}