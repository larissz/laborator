using System;
using Azure.Data.Tables;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using Microsoft.WindowsAzure.Storage;
using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;

namespace Models
{
    public class StudentEntity : Microsoft.Azure.Cosmos.Table.TableEntity
    {
        public StudentEntity(string university, string cnp)
        {
            this.PartitionKey = university;
            this.RowKey = cnp;
        }

        public StudentEntity() { }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Year { get; set; }
        public string PhoneNumber { get; set; }
        public string Faculty { get; set; }

    }

}