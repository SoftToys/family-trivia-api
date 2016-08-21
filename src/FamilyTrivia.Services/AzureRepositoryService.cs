using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTrivia.Contracts;
using FamilyTrivia.Contracts.Models;
//using Microsoft.Win; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
//using Microsoft.WindowsAzure.


namespace FamilyTrivia.Services

{
    public class AzureRepositoryService : IGamesRepositoryService
    {
        private CloudStorageAccount _storageAccount;
        public class TriviaGameEntity : TableEntity
        {
            public TriviaGameEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey)
            {
            }
        }


        public AzureRepositoryService()
        {
            // Parse the connection string and return a reference to the storage account.
            _storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("TRIVIA_CONNECTION_STRING"));


            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("people");

            // Create the table if it doesn't exist.
            table.CreateIfNotExistsAsync();
        }
        public async Task<Guid> AddUpdate(TriviaGame game)
        {  
            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

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
            //_storageAccount;

            throw new NotImplementedException();
        }

        public IEnumerable<TriviaGame> GetByOwner(string owner)
        {
            //_storageAccount;
            throw new NotImplementedException();
        }

        Task<Guid> IGamesRepositoryService.AddUpdate(TriviaGame game)
        {
            //_storageAccount;
            throw new NotImplementedException();
        }
    }
}
