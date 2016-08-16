using FamilyTrivia.Contracts;
using FamilyTrivia.Contracts.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyTrivia.Services
{
    public class TriviaGameEntity : TableEntity
    {
        public TriviaGameEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }
    }

    public class GamesRepositoryService : IGamesRepositoryService
    {

        public async Task<Guid> AddUpdate(TriviaGame game)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("SOME StorageConnectionString");
            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("people");
            await table.CreateIfNotExistsAsync();

            // Create a new customer entity. 
            TriviaGameEntity g1 = new TriviaGameEntity(game.OwnerId, game.Id.ToString());


            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(g1);

            // Execute the insert operation.
            await table.ExecuteAsync(insertOperation);

            return game.Id;
        }
        public TriviaGame GetById(Guid id)
        {
            // TODO: read from persistency and return the game
            throw new NotImplementedException();
        }

        public IEnumerable<TriviaGame> GetByOwner(string owner)
        {
            // TODO: read from persistency and return list 
            throw new NotImplementedException();
        }



    }
}
