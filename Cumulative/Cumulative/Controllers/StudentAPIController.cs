using Cumulative.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative.Controllers
{
    [Route("api/Student")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        // dependency injection of database context
        private readonly SchoolDbContext _context;

        public StudentAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Students in the system
        /// </summary>
        /// <example>
        /// GET api/Student/ListStudents
        /// Response: 
        /// [
        /// {"studentid": 1,"studentfname": "Sarah","studentlname": "Valdez","studentnumber": "N1678","enroldate": "2018-12-14"},
        /// {"studentId": 2,"studentFName": "Jennifer","studentLName": "Faulkner","studentNumber": "N1679","enrolDate": "2018-08-02"},
        /// ...........
        /// ]
        /// </example>
        /// <returns>
        /// A list of Students object
        /// </returns>


        [HttpGet]
        [Route(template: "ListStudents")]

        public List<Student> ListStudents()
        {
            // Create an empty list of student
            List<Student> Students = new List<Student>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "SELECT * FROM students";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        string StudentFName = ResultSet["studentfname"].ToString();
                        string StudentLName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        string EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]).ToString("yyyy-MM-dd");

                        //short form for setting all properties while creating the object
                        Student StudentsInfo = new Student()
                        {
                            StudentId = StudentId,
                            StudentFName = StudentFName,
                            StudentLName = StudentLName,
                            StudentNumber = StudentNumber,
                            EnrolDate = EnrolDate
                        };

                        // Add students object to the list
                        Students.Add(StudentsInfo);

                    }
                }
            }

            //Return the final list of students
            return Students;
        }



        /// <summary>
        /// Returns a student's details based on the provided student ID.
        /// </summary>
        /// <param name="id">The primary key of the students table</param>
        /// <example>
        /// GET api/Student/FindStudent/1 -> {"studentid": 1,"studentfname": "Sarah","studentlname": "Valdez","studentnumber": "N1678","enroldate": "2018-12-14"}
        /// </example>
        /// <returns>
        /// A student object containing the student's details such as studentid, studentfname, studentlname, studentnumber, and enroldate.
        /// If no student is found with the given ID, an empty student object is returned.
        /// </returns>

        [HttpGet]
        [Route(template: "FindStudent/{id}")]
        //Create empty Student
        public Student FindStudent(int id)
        {
            Student SelectedStudent = SelectedStudent = new Student(); ;


            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "SELECT * FROM students WHERE studentid=@id";

                // @id is replaced with a id
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {

                        //Access Column information by the DB column name as an index
                        int StudentId = Convert.ToInt32(ResultSet["studentid"]);
                        string StudentFName = ResultSet["studentfname"].ToString();
                        string StudentLName = ResultSet["studentlname"].ToString();
                        string StudentNumber = ResultSet["studentnumber"].ToString();
                        string EnrolDate = Convert.ToDateTime(ResultSet["enroldate"]).ToString("yyyy-MM-dd");

                        SelectedStudent.StudentId = StudentId;
                        SelectedStudent.StudentFName = StudentFName;
                        SelectedStudent.StudentLName = StudentLName;
                        SelectedStudent.StudentNumber = StudentNumber;
                        SelectedStudent.EnrolDate = EnrolDate;
                    }
                }
            }

            //Return selected student
            return SelectedStudent;
        }
    }
}
