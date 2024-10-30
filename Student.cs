using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegister_GOhman
{
    internal class Student
    {
        public int StudentID { get; set; } 
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? City { get; set; }

        public override string ToString()
        {
            return string.Format("ID: {0,-10} Name: {1,-10} {2,-20}  City: {3, -40}", StudentID, FirstName, LastName, City);
        }
    }
}
