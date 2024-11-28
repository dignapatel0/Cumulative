namespace Cumulative.Models
{
    public class Course
    {
        //Represents Course

        //Course Id
        public int CourseId { get; set; }

        //Course Code
        public string CourseCode { get; set; }

        //Teacher Id
        public int TeacherId { get; set; }

        //Start Date
        public string StartDate { get; set; }

        //Finish Date
        public string FinishDate { get; set; }

        //Course Name
        public string CourseName { get; set; }
    }
}
