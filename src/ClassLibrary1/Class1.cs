using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class Class1 : TableEntity
    {
        public Class1()
        {
        }

        public Class1(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }
    }
}
