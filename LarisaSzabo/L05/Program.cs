using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using System;

namespace datc_tema5
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        private static CloudTable metricsTable;

        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            Task.Run(async () => { await Initialize(); })
               .GetAwaiter()
               .GetResult();
        }

        static async Task Initialize()
        {
             string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=tema4szabolarisa;AccountKey=FLvkVXesBW7yl4ACbgbZOffnh51pGPD5mOkR8ebuvJjQWvo1sI0cRf2vVOCDx025pwPBzhLcMLcCHaDTTw79Ew==;EndpointSuffix=core.windows.net";

            var account = CloudStorageAccount.Parse(storageConnectionString);
            tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("studenti");
            await studentsTable.CreateIfNotExistsAsync();

            metricsTable = tableClient.GetTableReference("metrics");
            await metricsTable.CreateIfNotExistsAsync();


            await GetAllStudents();
        }

         private static async Task GetAllStudents() 
        {
            int Count = 0;

            //Console.WriteLine("Universitate\tCNP\tNume\tEmail\tNumar de telefon\tAn");
            Console.WriteLine("PartitionKey\tRowKey\tCount");
            TableQuery<StudentEntity> query1 = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "UPT"));;
            TableQuery<StudentEntity> query2 = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "UVT"));;
            TableQuery<StudentEntity> query3 = new TableQuery<StudentEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query1, token);

                token = resultSegment.ContinuationToken;

                foreach (StudentEntity entity in resultSegment.Results)
                {
                    ++Count; 
                    Console.WriteLine("{0}\t{1}\t{2}", entity.PartitionKey, entity.RowKey, Count);      
                }

            }while (token != null);
        }
    }
}
