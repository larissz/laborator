using System.Collections.Generic;
using Models;

namespace Repositories
{
    public static class StudentRepo 
    {
        public static List<Student> Students = new List<Student>() {
            new Student() { Id = 1, Nume = "Popescu", Prenume = "Vladimir", Facultate = "AC", AnStudiu = 4, NrMatricol = "LO612445"},
            new Student() { Id = 2, Nume = "Stoica", Prenume = "Emilian", Facultate = "AC", AnStudiu = 1, NrMatricol = "LO615215"}
        };

    }

}
