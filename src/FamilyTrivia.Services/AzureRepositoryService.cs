using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyTrivia.Contracts;
using FamilyTrivia.Contracts.Models;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Microsoft.AspNetCore.Mvc;

namespace FamilyTrivia.Services

{
    public class AzureRepositoryService : IGamesRepositoryService
    {

        // Members

        private CloudStorageAccount _storageAccount;  // holds azure storage account 


        // Methods
        public AzureRepositoryService()
        {
            // Parse the connection string and return a reference to the storage account.
            _storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("TRIVIA_CONNECTION_STRING"));
        }

        public async Task<Guid> AddUpdate(TriviaGame game)
        {
            // in case of new game
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


            return game.Id;
        }


        public async Task<TriviaGame> GetById(Guid id)
        {
            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("Games");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<TriviaGameEntity> query = new TableQuery<TriviaGameEntity>().Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, id));

            // Create a list to hold query result
            List<TriviaGame> triviaGameList = new List<TriviaGame>();

            foreach (TriviaGameEntity triviaGameEntity in table.ExecuteQuery(query))
            {
                triviaGameList.Add(new TriviaGame()
                {
                    Id = triviaGameEntity.Id,
                    Items = triviaGameEntity.Items,
                    Name = triviaGameEntity.Name,
                    CreateTime = triviaGameEntity.CreateTime,
                    OwnerId = triviaGameEntity.OwnerId,
                    Participates = triviaGameEntity.Participates,
                    StartTime = triviaGameEntity.StartTime,
                    UpdateTime = triviaGameEntity.UpdateTime
                }
                     );

                // TODO: ugly return...how to skip the for loop
                return triviaGameList[0];
            }

            return null;
        }

        public async Task<IEnumerable<TriviaGame>> GetByOwner(string owner)
        {
            // input validation
            // TODO: how come no owner ? no login?
            if (owner == null)
            {
                var sevenItems = new byte[] { 0x20, 0x20, 0x20, 0x20, 0x20, 0x20, 0x20 };
                Guid id = new Guid();
                _InsertImageByQuestionId(id, sevenItems);


                _GetImageByQuestionId(id);

                return null;
            }

            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("Games");

            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<TriviaGameEntity> query = new TableQuery<TriviaGameEntity>().Where(TableQuery.GenerateFilterCondition("OwnerId", QueryComparisons.Equal, owner));

            List<TriviaGame> triviaGameList = new List<TriviaGame>();

            foreach (TriviaGameEntity triviaGameEntity in table.ExecuteQuery(query))
            {
                triviaGameList.Add(new TriviaGame()
                {
                    Id = triviaGameEntity.Id,
                    Items = triviaGameEntity.Items,
                    Name = triviaGameEntity.Name,
                    CreateTime = triviaGameEntity.CreateTime,
                    OwnerId = triviaGameEntity.OwnerId,
                    Participates = triviaGameEntity.Participates,
                    StartTime = triviaGameEntity.StartTime,
                    UpdateTime = triviaGameEntity.UpdateTime
                }
                     );
            }

            return triviaGameList;
        }

        public ContentResult GetByQuestionId(Guid id)
        {
            throw new NotImplementedException();
        }

        private string _GetImageByQuestionId(Guid id)
        {
            // Create the blob client.
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("questionsimages");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(id +".jpg");

            // redirect
            return blockBlob.Uri.AbsoluteUri;

            // download
            //return blockBlob.DownloadToStream(new OutputStream());
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + name); // force download
            //container.GetBlobReference(name).DownloadToStream(Response.OutputStream);
    

            // Save blob contents to a local file 
            //using (var fileStream = System.IO.File.OpenWrite(@"path\myfile"))
            //{
            //    blockBlob.DownloadToStream(fileStream);
            //}         
        
        }

        private void _InsertImageByQuestionId(Guid id, byte[] image)
        {
            // Create the blob client.
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("questionsimages");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(id + ".jpg");

            // upload            
            using (var stream = new MemoryStream(image, writable: false))
            {
                blockBlob.UploadFromStream(stream);
            }
        }

        public async void testblob(TriviaItem itemId)
        {

            // blob
            // Create the blob client.
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            CloudBlobContainer container = blobClient.GetContainerReference("myblob");

            // Create the container if it doesn't already exist.
            await container.CreateIfNotExistsAsync();

            await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Retrieve reference to a blob named "myblob".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(itemId.Id + ".jpg");


            // Create or overwrite the "myblob" blob with contents from a local file.
            using (var fileStream = System.IO.File.OpenRead(@"d:\DSC03897.JPG"))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }

            // list container references
            // Loop over items within the container and output the length and URI.
            foreach (IListBlobItem item in container.ListBlobs())
            {
                if (item.GetType() == typeof(CloudBlockBlob))
                {
                    CloudBlockBlob blob = (CloudBlockBlob)item;

                    Console.WriteLine("Block blob of length {0}: {1}", blob.Properties.Length, blob.Uri);

                }
                else if (item.GetType() == typeof(CloudPageBlob))
                {
                    CloudPageBlob pageBlob = (CloudPageBlob)item;

                    Console.WriteLine("Page blob of length {0}: {1}", pageBlob.Properties.Length, pageBlob.Uri);

                }
                else if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;

                    Console.WriteLine("Directory: {0}", directory.Uri);
                }
            }
        }
    }
}
