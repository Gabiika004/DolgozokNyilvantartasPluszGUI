using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DolgozokNyilvantartasPluszGUI.Classes
{
    public class Dolgozo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Position { get; set; }


        public Dolgozo(int id, string name, int salary, string position)
        {
            Id = id;
            Name = name;
            Salary = salary;
            Position = position;
        }

        public Dolgozo() { }
    }
}
