﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FamilyTrivia.Contracts;
using FamilyTrivia.Contracts.Models;
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Linq;

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

                // create guid for questions
                // in case of images, insert to blob db
                foreach (var item in game.Items)
                {
                    item.Id = Guid.NewGuid();
                    if (item.ItemQuestion.IsImage)
                    {
                        // temporary - how to get uploaded picture ?
                        byte[] arr = new byte[] { 1, 1, 2 };

                        // insert picture to blob
                        _InsertImageByQuestionId(item.Id, arr);
                    }
                }
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
            triviaGameEntity.UsersScore = new List<UserRating>();

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.InsertOrReplace(triviaGameEntity);
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


        public async Task<IEnumerable<UserRating>> GetUserRating()
        {
            // Create the table client.
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference("UserRating");

            // Construct the query operation for all customer entities where PartitionKey="Smith".            
            List<UserRating> userRatingList = new List<UserRating>();
            //userRatingEntity = table.ExecuteQuery(new TableQuery<UserRatingEntity>()).ToList();
            // Create a list to hold query result


            // Construct the query operation for all customer entities where PartitionKey="Smith".
            TableQuery<UserRatingEntity> query = new TableQuery<UserRatingEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "user"));

            // Print the fields for each customer.
            foreach (UserRatingEntity userRatingEntity in table.ExecuteQuery(query))
            {
                UserRating userRating = new UserRating();

                userRating.UserName = userRatingEntity.RowKey;
                userRating.Attempted = userRatingEntity.Attempted;
                userRating.Scored = userRatingEntity.Scored;

                // add user rating 
                userRatingList.Add(userRating);

            }

            return userRatingList;
        }

        public async Task<IEnumerable<TriviaGame>> GetByOwner(string owner)
        {
            // input validation
            // TODO: how come no owner ? no login?
            if (owner == null)
            {
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
                TriviaGame tg = new TriviaGame();

                tg.Name = triviaGameEntity.Name;
                tg.CreateTime = triviaGameEntity.CreateTime;
                tg.OwnerId = triviaGameEntity.OwnerId;
                tg.Participates = triviaGameEntity.Participates;
                tg.StartTime = triviaGameEntity.StartTime;
                tg.UpdateTime = triviaGameEntity.UpdateTime;
                tg.Items = triviaGameEntity.Items;
                tg.Id = triviaGameEntity.Id;

                // for each question - set image uri if exist
                foreach (var item in tg.Items)
                {
                    if (item.ItemQuestion.IsImage)
                    {
                        item.ItemQuestion.ImageUri = _GetImageByQuestionId(item.Id);
                    }

                }

                // add game 
                triviaGameList.Add(tg);

            }

            return triviaGameList.OrderByDescending(t => t.UpdateTime);
        }

        private string _GetImageByQuestionId(Guid id)
        {
            // Create the blob client.
            CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("questionsimages");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(id + ".jpg");

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



        public void OnAnswerAttempt(string userName, AnswerAttempt aa)
        {
            // Update current user Score
            // First - get the TriviaGame from the DB
            CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Games");
            //TableQuery<TriviaGameEntity> query = new TableQuery<TriviaGameEntity>().Where(TableQuery.GenerateFilterConditionForGuid("Id", QueryComparisons.Equal, new Guid(aa.GameId)));
            TableOperation retrieveOperation = TableOperation.Retrieve<TriviaGameEntity>(userName, aa.GameId);
            TableResult retrievedResult = table.Execute(retrieveOperation);

            TriviaGameEntity triviaGameEnt;

            // if game exist - update user score
            if (retrievedResult.Result != null)
            {
                triviaGameEnt = ((TriviaGameEntity)retrievedResult.Result);

                // get the user rating object or create it
                UserRating ur;

                // if it is the first answer attempt
                if (triviaGameEnt.UsersScore.ToList().Count == 0)
                {
                    ur = new UserRating()
                    {
                        UserName = userName
                    };
                    triviaGameEnt.UsersScore.Add(ur);

                }
                else // user already attempted an answer
                {
                    ur = triviaGameEnt.UsersScore.SingleOrDefault(p => p.UserName == userName);
                }

                // update score and attempts.
                ur.Attempted++;
                if (aa.IsCorrect)
                {
                    ur.Scored++;
                }

                // update game
                TableOperation insertOperation = TableOperation.InsertOrReplace(triviaGameEnt);
                // Execute the insert operation.
                table.ExecuteAsync(insertOperation);


                // Now update the total rating 
                // Update user Rating
                // Create the CloudTable object that represents the "people" table.
                table = tableClient.GetTableReference("UserRating");
                //TableQuery<UserRatingEntity>  query1 = new TableQuery<UserRatingEntity>().Where(TableQuery.GenerateFilterCondition("RawKey", QueryComparisons.Equal, userName));
                retrieveOperation = TableOperation.Retrieve<UserRatingEntity>("user", userName);
                retrievedResult = table.Execute(retrieveOperation);

                // update or create the user rating entity
                UserRatingEntity userRatingEntity;

                // if exist - update
                if (retrievedResult.Result != null)
                {
                    userRatingEntity = ((UserRatingEntity)retrievedResult.Result);
                    userRatingEntity.Attempted++;

                    if (aa.IsCorrect)
                    {
                        userRatingEntity.Scored++;
                    }
                }
                else
                {
                    userRatingEntity = new UserRatingEntity()
                    {
                        Attempted = 1,
                        Scored = 0,
                        UserName = userName,
                        RowKey = userName,
                        PartitionKey = "user"
                    };
                }

                if (aa.IsCorrect)
                {
                    userRatingEntity.Scored++;
                }

                // Create the TableOperation object that inserts the customer entity.
                insertOperation = TableOperation.InsertOrReplace(userRatingEntity);
                // Execute the insert operation.
                table.ExecuteAsync(insertOperation);

            }
            else
            {
                // TODO: error
            }

            




        }

        public void OnGameEnd(TriviaGame game)
        {



        }
    }
}
