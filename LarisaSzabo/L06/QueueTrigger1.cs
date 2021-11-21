using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Models;
using Microsoft.Json;

namespace Students
{
    public class QueueTrigger1
    {
        [FunctionName("QueueTrigger1")]
        [return: Table("studenti")]
        public static StudentEntity Run([QueueTrigger("students-queue", Connection = "tema4szabolarisa_STORAGE")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            var student = JsonConvert.DeserializeObject<StudentEntity>(myQueueItem);

            return student;
        }
    }
}
