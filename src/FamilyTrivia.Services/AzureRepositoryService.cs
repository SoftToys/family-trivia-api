using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTrivia.Contracts;
using FamilyTrivia.Contracts.Models;
//using Microsoft.Win; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using System.IO;
using System.Text;
using System.Xml.Serialization;
//using Microsoft.WindowsAzure.


namespace FamilyTrivia.Services

{



    public class AzureRepositoryService : IGamesRepositoryService
    {

        private CloudStorageAccount _storageAccount;

        public AzureRepositoryService()
        {
            // Parse the connection string and return a reference to the storage account.
            _storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("TRIVIA_CONNECTION_STRING"));
        }

        public async Task<Guid> AddUpdate(TriviaGame game)
        {
            if (game.Id == Guid.Empty)
            {
                game.Id = Guid.NewGuid();
                game.CreateTime = game.UpdateTime = DateTime.Now;
            }
            // else - update
            else
            {
                game.UpdateTime = DateTime.Now;
            }

            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("Games");
            await table.CreateIfNotExistsAsync();

            // Create a new customer entity. 
            TriviaGameEntity triviaGameEntity = new TriviaGameEntity(game.OwnerId, game.Id.ToString());
            //TriviaGameEntity triviaGameEntity = new TriviaGameEntity(game.OwnerId, game.OwnerId);

            triviaGameEntity.StartTime = game.StartTime;
            triviaGameEntity.UpdateTime = game.UpdateTime;
            triviaGameEntity.CreateTime = game.CreateTime;
            triviaGameEntity.Items = game.Items;
            triviaGameEntity.Participates = game.Participates;
            triviaGameEntity.Id = game.Id;
            triviaGameEntity.Name = game.Name;

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(triviaGameEntity);

            // Execute the insert operation.
            await table.ExecuteAsync(insertOperation);

            var x = await this.GetById(game.Id);

            var y = await this.GetByOwner(game.OwnerId);

            return game.Id;
        }


        public async Task<TriviaGame> GetById(Guid id)
        {
            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("Games");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            //TableQuery<TriviaGameEntity> query = new TableQuery<TriviaGameEntity>().Where(TableQuery.GenerateFilterCondition("Id", QueryComparisons.Equal, id.ToString()));
            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<TriviaGameEntity>("amir", id.ToString());

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            TriviaGameEntity triviaGameEntity = (TriviaGameEntity)(retrievedResult.Result);

            return new TriviaGame()
            {
                Id = triviaGameEntity.Id,
                Items = triviaGameEntity.Items,
                Name = triviaGameEntity.Name,
                CreateTime = triviaGameEntity.CreateTime,
                OwnerId = triviaGameEntity.OwnerId,
                Participates = triviaGameEntity.Participates,
                StartTime = triviaGameEntity.StartTime,
                UpdateTime = triviaGameEntity.UpdateTime
            };
        }

        public async Task<IEnumerable<TriviaGame>> GetByOwner(string owner)
        {
            // input validation
            if (owner == null)
            {
                return null;
            }

            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("Games");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<TriviaGameEntity> query = new TableQuery<TriviaGameEntity>().Where(TableQuery.GenerateFilterCondition("Owner", QueryComparisons.Equal, owner));

            // Create a retrieve operation that takes a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<TriviaGameEntity>(owner, owner);

            // Execute the retrieve operation.
            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            TriviaGameEntity triviaGameEntity = (TriviaGameEntity)(retrievedResult.Result);

            TriviaGame x = new TriviaGame()
            {
                Id = triviaGameEntity.Id,
                Items = triviaGameEntity.Items,
                Name = triviaGameEntity.Name,
                CreateTime = triviaGameEntity.CreateTime,
                OwnerId = triviaGameEntity.OwnerId,
                Participates = triviaGameEntity.Participates,
                StartTime = triviaGameEntity.StartTime,
                UpdateTime = triviaGameEntity.UpdateTime
            };

            List<TriviaGame> list = new List<TriviaGame>();
            // Print the fields for each customer.
            // foreach(TriviaGameEntity entity in await table.ExecuteAsync(query))
            {
                //   Console.WriteLine("{0}, {1}\t{2}\t{3}", entity.PartitionKey, entity.RowKey,
                //       entity.Email, entity.PhoneNumber);
            }

            list.Add(x);
            return list;
        }

    }
}
