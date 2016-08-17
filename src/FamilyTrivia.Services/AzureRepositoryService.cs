using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FamilyTrivia.Contracts;
using FamilyTrivia.Contracts.Models;
//using Microsoft.Azure; // Namespace for CloudConfigurationManager 
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types

namespace FamilyTrivia.Services
{
    public class AzureRepositoryService : IGamesRepositoryService
    {
        public AzureRepositoryService()
        {
            string connection = Environment.GetEnvironmentVariable("StorageConnectionString");

            // Parse the connection string and return a reference to the storage account.
            //CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            //CloudConfigurationManager.GetSetting("StorageConnectionString"));
        }
        public Guid AddUpdate(TriviaGame game)
        {
            throw new NotImplementedException();
        }

        public TriviaGame GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<TriviaGame> GetByOwner(string owner)
        {
            throw new NotImplementedException();
        }
    }
}
