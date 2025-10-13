using System;
using System.Collections.Generic;

namespace UniversityManagement
{
    // Абстрактный класс — базовый для всех людей
    abstract class Person
    {
        private string name;
        private int age;
        private string contactInfo;

        public string Name => name;
        public int Age => age;
        public string ContactInfo => contactInfo;

        protected Person(string name, int age, string contactInfo)
        {
            this.name = name;
            this.age = age;
            this.contactInfo = contactInfo;
        }

        public abstract void ShowInfo();
    }

    class Student : Person
    {
        private List<Course> courses = new List<Course>();

        public Student(string name, int age, string contact) : base(name, age, contact) { }

        public void EnrollCourse(Course course)
        {
            courses.Add(course);
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Студент: {Name}, Возраст: {Age}, Контакт: {ContactInfo}");
            Console.WriteLine("Курсы:");
            if (courses.Count == 0)
                Console.WriteLine("  Нет записей на курсы.");
            else
                foreach (var c in courses)
                    Console.WriteLine($"  - {c.Title}");
        }
    }

    class Teacher : Person
    {
        public Teacher(string name, int age, string contact) : base(name, age, contact) { }

        public override void ShowInfo()
        {
            Console.WriteLine($"Преподаватель: {Name}, Возраст: {Age}, Контакт: {ContactInfo}");
        }
    }

    class Course
    {
        public string Title;
        public Teacher Instructor;
        private List<Student> students = new List<Student>();

        public Course(string title, Teacher instructor)
        {
            Title = title;
            Instructor = instructor;
        }

        public void AddStudent(Student student)
        {
            students.Add(student);
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Курс: {Title}, Преподаватель: {Instructor.Name}");
            Console.WriteLine("Студенты:");
            if (students.Count == 0)
                Console.WriteLine("  Пока никто не записан.");
            else
                foreach (var s in students)
                    Console.WriteLine($"  - {s.Name}");
        }
    }

    class University
    {
        private List<Student> students = new List<Student>();
        private List<Teacher> teachers = new List<Teacher>();
        private List<Course> courses = new List<Course>();

        public void AddStudent()
        {
            Console.Write("Имя студента: ");
            string name = Console.ReadLine();
            Console.Write("Возраст: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Контакт: ");
            string contact = Console.ReadLine();
            students.Add(new Student(name, age, contact));
        }

        public void AddTeacher()
        {
            Console.Write("Имя преподавателя: ");
            string name = Console.ReadLine();
            Console.Write("Возраст: ");
            int age = int.Parse(Console.ReadLine());
            Console.Write("Контакт: ");
            string contact = Console.ReadLine();
            teachers.Add(new Teacher(name, age, contact));
        }

        public void AddCourse()
        {
            if (teachers.Count == 0)
            {
                Console.WriteLine("Нет преподавателей для назначения курса.");
                return;
            }

            Console.Write("Название курса: ");
            string title = Console.ReadLine();

            Console.WriteLine("Выберите преподавателя:");
            for (int i = 0; i < teachers.Count; i++)
                Console.WriteLine($"{i + 1}. {teachers[i].Name}");

            int choice = int.Parse(Console.ReadLine()) - 1;
            courses.Add(new Course(title, teachers[choice]));
        }

        public void EnrollStudentInCourse()
        {
            if (students.Count == 0 || courses.Count == 0)
            {
                Console.WriteLine("Недостаточно данных (нужно добавить студентов и курсы).");
                return;
            }

            Console.WriteLine("Выберите студента:");
            for (int i = 0; i < students.Count; i++)
                Console.WriteLine($"{i + 1}. {students[i].Name}");
            int sIndex = int.Parse(Console.ReadLine()) - 1;

            Console.WriteLine("Выберите курс:");
            for (int i = 0; i < courses.Count; i++)
                Console.WriteLine($"{i + 1}. {courses[i].Title}");
            int cIndex = int.Parse(Console.ReadLine()) - 1;

            students[sIndex].EnrollCourse(courses[cIndex]);
            courses[cIndex].AddStudent(students[sIndex]);
        }

        public void ShowAllStudents()
        {
            foreach (var s in students)
                s.ShowInfo();
        }

        public void ShowAllTeachers()
        {
            foreach (var t in teachers)
                t.ShowInfo();
        }

        public void ShowAllCourses()
        {
            foreach (var c in courses)
                c.ShowInfo();
        }
    }

    class Program
    {
        static void Main()
        {
            University university = new University();

            while (true)
            {
                Console.WriteLine("\n-- Меню управления университетом --");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Добавить преподавателя");
                Console.WriteLine("3. Создать курс");
                Console.WriteLine("4. Записать студента на курс");
                Console.WriteLine("5. Показать всех студентов");
                Console.WriteLine("6. Показать всех преподавателей");
                Console.WriteLine("7. Показать все курсы");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": university.AddStudent(); break;
                    case "2": university.AddTeacher(); break;
                    case "3": university.AddCourse(); break;
                    case "4": university.EnrollStudentInCourse(); break;
                    case "5": university.ShowAllStudents(); break;
                    case "6": university.ShowAllTeachers(); break;
                    case "7": university.ShowAllCourses(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный выбор."); break;
                }
            }
        }
    }
}
