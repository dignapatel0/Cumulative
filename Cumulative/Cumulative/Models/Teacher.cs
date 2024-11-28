namespace Cumulative.Models
{
    public class Teacher
    {
        //Represents Teacher

        //Teacher id
        public int TeacherId { get; set; }

        //Teacher first name
        public string TeacherFName { get; set; }

        //Teacher last name
        public string TeacherLName { get; set; }

        // Teacher employee number
        public string EmployeeNumber { get; set; }

        // Teacher hiredate
        public DateTime HireDate { get; set; }

        //Teacher salary
        public double Salary { get; set; }
    }
}