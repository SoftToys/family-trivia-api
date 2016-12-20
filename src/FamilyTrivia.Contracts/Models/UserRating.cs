using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTrivia.Contracts.Models
{
    public class UserRating
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>


        [JsonProperty("_username")]
        public string UserName { get; set; }

        [JsonProperty("_attempted")]
        public int Attempted { get; set; }
        /// <summary>
        /// Gets or sets the owner identifier.
        /// </summary>
        /// <value>
        /// The owner identifier.
        /// </value>
        [JsonProperty("_scored")]
        public int  Scored { get; set; }
    }
}
