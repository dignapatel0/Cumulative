using Cumulative.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative.Controllers
{
    [Route("api/Course")]
    [ApiController]
    public class CourseAPIController : ControllerBase
    {
        // dependency injection of database context
        private readonly SchoolDbContext _context;

        public CourseAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Courses in the system
        /// </summary>
        /// <example>
        /// GET api/Course/ListCourses
        /// Response:
        /// [
        /// {"courseid": 1,"coursecode": "http5101","teacherid": 1,"startdate": "2018-09-04","finishdate": "2018-12-14","coursename": "Web Application Development"},
        /// {"courseid": 2,"coursecode": "http5102","teacherid": 2,"startdate": "2018-09-04","finishdate": "2018-12-14","coursename": "Project Management"},
        /// ...........
        /// ]
        /// </example>
        /// <returns>
        /// A list of Courses object
        /// </returns>


        [HttpGet]
        [Route(template: "ListCourses")]

        public List<Course> ListCourses()
        {
            // Create an empty list of course
            List<Course> Courses = new List<Course>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "SELECT * FROM courses";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string StartDate = Convert.ToDateTime(ResultSet["startdate"]).ToString("yyyy-MM-dd");
                        string FinishDate = Convert.ToDateTime(ResultSet["finishdate"]).ToString("yyyy-MM-dd");
                        string CourseName = ResultSet["coursename"].ToString();

                        //short form for setting all properties while creating the object
                        Course CoursesInfo = new Course()
                        {
                            CourseId = CourseId,
                            CourseCode = CourseCode,
                            TeacherId = TeacherId,
                            StartDate = StartDate,
                            FinishDate = FinishDate,
                            CourseName = CourseName
                        };

                        // Add courses object to the list
                        Courses.Add(CoursesInfo);

                    }
                }
            }

            //Return the final list of courses
            return Courses;
        }



        /// <summary>
        /// Returns a course's details based on the provided course ID.
        /// </summary>
        /// <param name="id">The primary key of the courses table</param>
        /// <example>
        /// GET api/Course/FindCourse/1 -> {"courseid": 1,"coursecode": "http5101","teacherid": 1,"startdate": "2018-09-04","finishdate": "2018-12-14","coursename": "Web Application Development"}
        /// </example>
        /// <returns>
        /// A course object containing the course's details such as courseid, coursecode, teacherid, startdate, finishdate, and coursename.
        /// If no course is found with the given ID, an empty course object is returned.
        /// </returns>

        [HttpGet]
        [Route(template: "FindCourse/{id}")]
        //Create empty Course
        public Course FindCourse(int id)
        {
            Course SelectedCourse = SelectedCourse = new Course(); ;


            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "SELECT * FROM courses WHERE courseid=@id";

                // @id is replaced with a id
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {

                        //Access Column information by the DB column name as an index
                        int CourseId = Convert.ToInt32(ResultSet["courseid"]);
                        string CourseCode = ResultSet["coursecode"].ToString();
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string StartDate = Convert.ToDateTime(ResultSet["startdate"]).ToString("yyyy-MM-dd");
                        string FinishDate = Convert.ToDateTime(ResultSet["finishdate"]).ToString("yyyy-MM-dd");
                        string CourseName = ResultSet["coursename"].ToString();

                        SelectedCourse.CourseId = CourseId;
                        SelectedCourse.CourseCode = CourseCode;
                        SelectedCourse.TeacherId = TeacherId;
                        SelectedCourse.StartDate = StartDate;
                        SelectedCourse.FinishDate = FinishDate;
                        SelectedCourse.CourseName = CourseName;
                    }
                }
            }

            //Return selected course
            return SelectedCourse;
        }
    }
}
