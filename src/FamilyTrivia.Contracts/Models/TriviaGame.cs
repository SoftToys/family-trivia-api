using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace FamilyTrivia.Contracts.Models
{
    public class TriviaGame
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [JsonProperty("_id")]
        public Guid Id { get; set; }

        [JsonProperty("_name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("_owner")]
        public string OwnerId { get; set; }

        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The Minimal start time in which the game would be active
        /// </value>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Gets or sets the TriviaItem items.
        /// </summary>
        /// <value>
        /// The Trivia Items.
        /// </value>
        [JsonProperty("_items")]
        public IEnumerable<TriviaItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the list of participates that can play this game
        /// </summary>
        /// <value>
        /// The participates.
        /// </value>
        public IEnumerable<User> Participates { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
        public int Level { get; set; }

    }
}
