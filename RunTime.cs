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

        public RunTime()
        {
        }

        public void Start()
        {
            while (true)
            {
                WriteMenu();
                MenuChoice(Console.ReadLine()!);

            }
        }


        private void MenuChoice(string input)
        {
            switch (input)
            {
                case "1":
                    Console.Clear();

                    Console.WriteLine("Add Student");
                    Console.Write("First name: ");
                    string firstName = Console.ReadLine()!;
                    Console.Write("Last name: ");
                    string lastName = Console.ReadLine()!;
                    Console.Write("City: ");
                    string city = Console.ReadLine()!;

                    currStudent = CreateStudent(firstName, lastName, city);
                    AddStudentToDatabase(currStudent);
                    Console.WriteLine("Student added to database");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();

                    break;
                case "2":
                    Console.Clear();

                    Console.WriteLine("Modify Student");
                    Console.Write("Give student ID to modify: ");
                    int studID = int.Parse(Console.ReadLine()!);

                    currStudent = GetStudent(studID);

                    if (currStudent != null)
                    {
                        Console.WriteLine("What to modify?");
                        Console.WriteLine("1: First name\r\n" +
                            "2: Last name\r\n" +
                            "3: City\r\n");

                        if (ModifyStudent(Console.ReadLine()!, ref currStudent))
                        {
                            dbCtx.SaveChanges();
                            Console.WriteLine("Student successfully modified!");
                            Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Modification failed. Press any key to continue");
                            Console.ReadKey();
                        }
                        
                    }
                    else
                    {
                        Console.WriteLine("Invalid student ID. Press any key to continue");
                        Console.ReadKey();
                    }

                    Console.Clear();


                    break;
                case "3":
                    Console.WriteLine("List all Students");

                    if (dbCtx.Students.Count() == 0)
                    {
                        return;
                    }

                    foreach (var student in dbCtx.Students)
                    {
                        Console.WriteLine(student.ToString());
                    }

                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();

                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    break;
            }
        }

        public bool ModifyStudent(string strIn, ref Student student)
        {
            bool ok = true;
            switch (strIn)
            {
                case "1":
                    Console.Write("Enter new First Name:");
                    student.FirstName = Console.ReadLine();
                    break;
                case "2":
                    Console.Write("Enter new Last Name:");
                    student.LastName = Console.ReadLine();
                    break;
                case "3":
                    Console.Write("Enter new City:");
                    student.City = Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    ok = false;
                    break;
            }
            return ok;
        }
        public void WriteMenu()
        {
            Console.WriteLine("MENU: \r\n"
                + "1: Register Student"
                + "\r\n2: Modify Student"
                + "\r\n3: List All Students");
            Console.WriteLine();
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

        public void AddStudentToDatabase(Student studIn)
        {
            if (studIn != null)
            {
                dbCtx.Add(studIn);
                SaveChangesToDb();
            }
        }
        private void SaveChangesToDb()
        {
            dbCtx.SaveChanges();
        }

        public Student GetStudent(int studentID)
        {
            var student = dbCtx.Students.Where(s => s.StudentID == studentID).FirstOrDefault();

            if (student != null)
                return student;


            return null!;
        }
    }
}
