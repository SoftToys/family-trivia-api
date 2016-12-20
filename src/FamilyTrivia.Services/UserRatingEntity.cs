using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTrivia.Services
{
    public class UserRatingEntity : TableEntity
    {
        // user name - partition key
        // raw key - GUID
        public UserRatingEntity(string userNamePartitionKey, string userNameRowKey) : base(userNamePartitionKey, userNameRowKey)
        {
            this.PartitionKey = userNamePartitionKey;
            this.RowKey = userNameRowKey;
        }

        public UserRatingEntity() { }

        public string UserName { get; set; }
        public int Attempted { get; set; }
        public int Scored { get; set; }
    }
}

   
        
