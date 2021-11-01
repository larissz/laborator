using System;
using Azure.Data.Tables;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace L04
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            Task.Run(async () => {await Initialize(); })
              .GetAwaiter()
              .GetResult();
        }

        static async Task Initialize() 
        {
          string storageConnectionString = "DefaultEndpointsProtocol=https;"
            + "AccountName=tema4szabolarisa"
            + ";AccountKey=FLvkVXesBW7yl4ACbgbZOffnh51pGPD5mOkR8ebuvJjQWvo1sI0cRf2vVOCDx025pwPBzhLcMLcCHaDTTw79Ew=="
            + ";EndpointSuffix=core.windows.net";

          var account = CloudStorageAccount.Parse(storageConnectionString);
          tableClient = account.CreateCloudTableClient();

          studentsTable = tableClient.GetTableReference("studenti");

          await studentsTable.CreateIfNotExistsAsync();

          await AddNewStudent();
          await EditStudent();
          await GetAllStudents();
          await DeleteStudent();
        }

        private static async Task AddNewStudent()
        {
            var student = new StudentEntity("UPT", "1980307072296");
            student.FirstName = "George";
            student.LastName = "Popescu";
            student.Email = "george.popescu@gmail.com";
            student.Year = 4;
            student.PhoneNumber = "0758372291";
            student.Faculty = "AC";

            var insertOperation = TableOperation.Insert(student);

            await studentsTable.ExecuteAsync(insertOperation);
        }

        private static async Task GetAllStudents() 
        {
            Console.WriteLine("Universitate\tCNP\tNume\tEmail\tNumar de telefon\tAn");
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (StudentEntity entity in resultSegment.Results)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", entity.PartitionKey, entity.RowKey, entity.FirstName, entity.LastName, entity.Email, entity.Year, entity.PhoneNumber, entity.Faculty);
                }

            }while (token != null);
        }

        private static async Task EditStudent()
        {
            var student = new StudentEntity("UPT", "1980307072296");
            student.FirstName = "George";
            student.Year = 3;
            student.ETag = "*";

            var editOperation = TableOperation.Merge(student);
            await studentsTable.ExecuteAsync(editOperation);
        }

        private static async Task DeleteStudent()
        {
            var student = new StudentEntity("UPT", "1980307072296");
            //student.FirstName = "George";
            //student.Year = 3;
            //student.ETag = "*";

            var deleteOperation = TableOperation.Delete(student);
            await studentsTable.ExecuteAsync(deleteOperation);
        }
    }

}
