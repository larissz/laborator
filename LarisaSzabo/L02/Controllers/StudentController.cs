using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Models;
using Repositories;
using System.Linq;

namespace L02.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        public StudentController()
        {}       

        [HttpGet]
        public IEnumerable<Student> Get()
        {
           return StudentRepo.Students;
        }

        [HttpGet("{id}")]
        public Student Get(int id)
        {
            return StudentRepo.Students.FirstOrDefault(s => s.Id == id);
        }

        [HttpPost]
        public void Post([FromBody] Student s)
        {
           
        }

        [HttpPut]
        public void Put([FromBody] Student s)
        {

        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            StudentRepo.Students.Remove(id);

        }

    }
}
