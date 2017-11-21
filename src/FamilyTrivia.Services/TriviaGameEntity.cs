using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTrivia.Contracts.Models;
using Newtonsoft.Json;

namespace FamilyTrivia.Services
{
    public class TriviaGameEntity : TableEntity
    {
        // user name - partition key
        // raw key - GUID
        public TriviaGameEntity(string ownerIdPartitionKey, string gameIdRowKey) : base(ownerIdPartitionKey, gameIdRowKey)
        {
            this.PartitionKey = ownerIdPartitionKey;
            this.RowKey = gameIdRowKey;
            this.OwnerId = ownerIdPartitionKey;
            this.Id = new Guid(gameIdRowKey);            
        }

        public TriviaGameEntity() { }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public Guid Id { get; set; }

        public string Name { get; set; }

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
        //public string Items { get; set; }

        /// <summary>
        /// Gets or sets the list of participates that can play this game
        /// </summary>
        /// <value>
        /// The participates.
        /// </value>

        [IgnoreProperty]
        public IEnumerable<User> Participates { get; set; }

        public string ParticipatesStr
        {
            get
            {
                return JsonConvert.SerializeObject(Participates);
            }
            set
            {
                Participates = JsonConvert.DeserializeObject<IEnumerable<User>>(value);
            }
        }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }
        [IgnoreProperty]
        public IEnumerable<TriviaItem> Items { get; internal set; }
        public string ItemsStr
        {
            get
            {
                return JsonConvert.SerializeObject(Items);
            }
            set
            {
                Items = JsonConvert.DeserializeObject<IEnumerable<TriviaItem>>(value);
            }
        }


        /// <summary>
        /// Gets or sets the score of each of the users who play this game
        /// </summary>
        /// <value>
        /// The participates.
        /// </value>
        [IgnoreProperty]
        public IList<UserRating> UsersScore { get; internal set; }
        public string UsersScoreStr
        {
            get
            {
                return JsonConvert.SerializeObject(UsersScore);
            }
            set
            {
                UsersScore = JsonConvert.DeserializeObject<IList<UserRating>>(value);
            }
        }
    }
}
