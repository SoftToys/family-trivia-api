using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
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
        public IEnumerable<TriviaItem> Items { get; set; }

        /// <summary>
        /// Gets or sets the list of participates that can play this game
        /// </summary>
        /// <value>
        /// The participates.
        /// </value>
        public IEnumerable<User> Participates { get; set; }
    }
}
