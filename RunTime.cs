using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentRegister_GOhman
{
    internal class RunTime
    {
        Student? currStudent;
        private StudentDbContext dbCtx = new StudentDbContext();

        public void Start()
        {
            while (true)
            {
                WriteMenu();
                MenuChoice(Console.ReadKey()!);
            }
        }

        private void MenuChoice(ConsoleKeyInfo input) //Switch-case Menu for choosing operations
        {
            ConsoleKey key = input.Key;
            switch (key)
            {
                case ConsoleKey.D1: //Add student
                    Console.Clear();

                    if (!RequestAddStudent())
                    {
                        return;
                    }
                    Console.WriteLine("Student added to database");
                    PressAnyKeyToContinueLogic();
                    break;

                case ConsoleKey.D2: //Modify student
                    Console.Clear();

                    if (!RequestModifyStudent())
                    {
                        return;
                    }
                    dbCtx.SaveChanges(); //Save changes
                    Console.WriteLine("Student successfully modified!");
                    PressAnyKeyToContinueLogic();
                    break;

                case ConsoleKey.D3: //List students
                    Console.Clear();
                    ListAllStudents();
                    PressAnyKeyToContinueLogic();
                    break;

                default:
                    Console.WriteLine("Invalid choice");
                    PressAnyKeyToContinueLogic();
                    break;
            }
        }

        private bool RequestAddStudent() //Creates student and add student to database. Return bool true/false depending on result.
        {
            Console.WriteLine("Add Student");
            Console.Write("First name: ");
            string firstName = Console.ReadLine()!;
            Console.Write("Last name: ");
            string lastName = Console.ReadLine()!;
            Console.Write("City: ");
            string city = Console.ReadLine()!;

            currStudent = CreateStudent(firstName, lastName, city);
            return AddStudentToDatabase(currStudent);
        }

        private bool RequestModifyStudent() //Call ModifyStudent and return bool true/false depending on result.
        {
            Console.WriteLine("Modify Student");
            ListAllStudents();
            Console.WriteLine();
            Console.Write("Give student ID to modify: ");
            int studID;

            if (!int.TryParse(Console.ReadLine()!, out studID) || GetStudent(studID) == null)
            {
                Console.WriteLine("Invalid student ID");
                PressAnyKeyToContinueLogic();
                return false;
            }

            currStudent = GetStudent(studID);

            Console.WriteLine("\r\nWhat to modify?");
            Console.WriteLine("1: First name\r\n" +
                "2: Last name\r\n" +
                "3: City\r\n");
            return (ModifyStudent(Console.ReadKey()!, currStudent));
        }

        public bool ModifyStudent(ConsoleKeyInfo keyIn, Student student) //Modifies student by given condition.
        {
            bool ok = true;
            ConsoleKey key = keyIn.Key;
            Console.WriteLine();
            switch (key)
            {
                case ConsoleKey.D1:
                    Console.Write("Enter new First Name: ");
                    student.FirstName = Console.ReadLine();
                    break;
                case ConsoleKey.D2:
                    Console.Write("Enter new Last Name: ");
                    student.LastName = Console.ReadLine();
                    break;
                case ConsoleKey.D3:
                    Console.Write("Enter new City: ");
                    student.City = Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    PressAnyKeyToContinueLogic();
                    ok = false;
                    break;
            }
            return ok;
        }

        private Student CreateStudent(string firstName, string lastName, string city)
        {
            Student student = new Student()
            {
                FirstName = firstName,
                LastName = lastName,
                City = city
            };
            return student;
        }

        public bool AddStudentToDatabase(Student studIn) //Add student to database, if not null. 
        {
            if (studIn != null)
            {
                dbCtx.Add(studIn); //Add student to database
                dbCtx.SaveChanges(); //Save changes
                return true;
            }
            return false;
        }

        public Student GetStudent(int studentID) //Get student by ID
        {
            var student = dbCtx.Students.Where<Student>(s => s.StudentID == studentID).FirstOrDefault();

            if (student != null)
            {
                return student;
            }

            return null!;
        }

        private void ListAllStudents()
        {
            if (dbCtx.Students.Count() == 0)
            {
                Console.WriteLine("No students in database");
                return;
            }

            foreach (var student in dbCtx.Students)
            {
                Console.WriteLine(student.ToString());
            }
        }

        public void WriteMenu()
        {
            Console.WriteLine("MENU: "
                + "\r\n1: Register Student"
                + "\r\n2: Modify Student"
                + "\r\n3: List All Students");
            Console.WriteLine();
        }

        private void PressAnyKeyToContinueLogic() //Logic for continuing and cleaning console. Used to clean upp the code.
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            Console.Clear();
        }
    }
}
