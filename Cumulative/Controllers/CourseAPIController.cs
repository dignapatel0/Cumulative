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

        /// <summary>
        /// Adds a new course to the database.
        /// </summary>
        /// <param name="CourseData">An object containing the Course's details.</param>
        /// <example>
        /// POST: api/Course/AddCourse
        /// Headers: Content-Type: application/json  
        /// Request Body:  
        /// {
        ///	    "courseCode":"Http5101",
        ///	    "teacherid":"1",
        ///	    "startdate":"2018-09-04",
        ///	    "finishdate":"2018-09-04"
        ///	    "coursename":"Web application"
        ///	 }
        /// Response: 409 (if there's a conflict, e.g., duplicate entry)  
        /// </example>
        /// <returns>
        /// The ID of the newly inserted course if successful, or 0 if the operation fails.
        /// </returns>

        [HttpPost(template: "AddCourse")]
        public int AddCourse([FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "INSERT INTO courses (coursecode, teacherid, startdate, finishdate, coursename) values (@coursecode,@teacherid,@startdate,@finishdate,@coursename)";
                Command.Parameters.AddWithValue("@coursecode", CourseData.CourseCode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.TeacherId);
                Command.Parameters.AddWithValue("@startdate", CourseData.StartDate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.FinishDate);
                Command.Parameters.AddWithValue("@coursename", CourseData.CourseName);


                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }

        /// <summary>
        /// Deletes a specific Course record from the database. 
        /// This operation is performed using the primary key associated with the Course record.
        /// </summary>
        /// <param name="CourseId"> The unique ID for the Course to be deleted from the database </param>
        /// <example>
        /// Example usage of this API endpoint:
        /// DELETE: api/CourseData/DeleteCourse/11 -> 1
        /// In this example, the Course with the unique identifier (CourseID) of 11 will be deleted 
        /// from the database if the record exists.
        /// </example>
        /// <returns>
        /// Returns the number of rows affected by the delete operation.
        /// </returns>


        [HttpDelete(template: "DeleteCourse/{CourseId}")]
        public int DeleteCourse(int CourseId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "DELETE FROM courses WHERE courseid=@id";
                Command.Parameters.AddWithValue("@id", CourseId);

                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }

        /// <summary>
        /// Updates a Course in the database. Data is Course object, request query contains ID
        /// </summary>
        /// <param name="CourseData">Course Object</param>
        /// <param name="CourseId">The Course ID primary key</param>
        /// <example>
        /// PUT: api/Courses/UpdateCourse/11
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// { 
        ///     "CourseCode":"http5121", 
        ///     "TeacherId": 1",
        ///     "CourseStartDate":2024-09-03,
        ///     "CourseFinishDate":2024-12-13,
        ///     "CourseName":"Front End"
        /// } -> 
        /// {
        ///     "CourseId":11,
        ///     "CourseCode":"http5121", 
        ///     "TeacherId": 1",
        ///     "CourseStartDate":2024-09-03,
        ///     "CourseFinishDate":2024-12-13,
        ///     "CourseName":"Front End" }
        /// </example>
        /// <returns>
        /// The updated Course object
        /// </returns>
        [HttpPut(template: "UpdateCourse/{CourseId}")]
        public Course UpdateCourse(int CourseId, [FromBody] Course CourseData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query
                Command.CommandText = "update courses set coursecode=@coursecode, startdate = @startdate, finishdate = @finishdate, coursename = @coursename, teacherid = @teacherid where courseid=@id";
                Command.Parameters.AddWithValue("@coursecode", CourseData.CourseCode);
                Command.Parameters.AddWithValue("@teacherid", CourseData.TeacherId);
                Command.Parameters.AddWithValue("@startdate", CourseData.StartDate);
                Command.Parameters.AddWithValue("@finishdate", CourseData.FinishDate);
                Command.Parameters.AddWithValue("@coursename", CourseData.CourseName);

                Command.Parameters.AddWithValue("@id", CourseId);

                Command.ExecuteNonQuery();
            }

            return FindCourse(CourseId);
        }


    }
}
