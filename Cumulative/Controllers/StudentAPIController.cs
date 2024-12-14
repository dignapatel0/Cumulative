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

        /// <summary>
        /// Adds a new Student to the database.
        /// </summary>
        /// <param name="StudentData">An object containing the Student's details.</param>
        /// <example>
        /// POST: api/Student/AddStudent
        /// Headers: Content-Type: application/json  
        /// Request Body:  
        /// { 
        ///     "studentFName": "Maria",
        ///     "studentLName": "Monte",
        ///     "studentNumber": "T101",
        ///     "enrolDate": "2024-11-28T00:00:00",
        /// }  
        /// Response: 409 (if there's a conflict, e.g., duplicate entry)  
        /// </example>
        /// <returns>
        /// The ID of the newly inserted Student if successful, or 0 if the operation fails.
        /// </returns>

        [HttpPost(template: "AddStudent")]
        public int AddStudent([FromBody] Student StudentData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                Command.CommandText = "INSERT INTO students (studentfname, studentlname, studentnumber, enroldate) values (@studentfname,@studentlname ,@studentnumber, @enroldate)";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrolDate);


                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }

        /// <summary>
        /// Deletes a specific Student record from the database. 
        /// This operation is performed using the primary key associated with the Student record.
        /// </summary>
        /// <param name="StudentId"> The unique ID for the Student to be deleted from the database </param>
        /// <example>
        /// Example usage of this API endpoint:
        /// DELETE: api/StudentData/StudentTeacher/20 -> 1
        /// In this example, the Student with the unique identifier (StudentID) of 20 will be deleted 
        /// from the database if the record exists.
        /// </example>
        /// <returns>
        /// Returns the number of rows affected by the delete operation.
        /// </returns>


        [HttpDelete(template: "DeleteStudent/{StudentId}")]
        public int DeleteStudent(int StudentId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "DELETE FROM students WHERE studentid=@id";
                Command.Parameters.AddWithValue("@id", StudentId);

                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }

        /// <summary>
        /// Updates a Student in the database. Data is Student object, request query contains ID
        /// </summary>
        /// <param name="StudentData">Student Object</param>
        /// <param name="StudentId">The Student ID primary key</param>
        /// <example>
        /// PUT: api/Student/UpdateStudent/11
        /// Headers: Content-Type: application/json
        /// Request Body:
        /// {
        ///	    "StudentFname": "Ayush",
        ///     "StudentLname": "Patel",
        ///     "StudentNumber": "S12345",
        ///     "EnrolDate": "2024-09-01T00:00:00"
        /// } -> 
        /// {
        ///     "teacherId":11,
        ///     "StudentFname": "Ayush",
        ///     "StudentLname": "Patel",
        ///     "StudentNumber": "S12345",
        ///     "EnrolDate": "2024-09-01T00:00:00"
        /// }
        /// </example>
        /// <returns>
        /// The updated Student object
        /// </returns>
        [HttpPut(template: "UpdateStudent/{StudentId}")]
        public Student UpdateStudent(int StudentId, [FromBody] Student StudentData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                // parameterize query
                Command.CommandText = "update students set studentfname=@studentfname, studentlname=@studentlname, studentnumber = @studentnumber, enroldate = @enroldate where studentid=@id";
                Command.Parameters.AddWithValue("@studentfname", StudentData.StudentFName);
                Command.Parameters.AddWithValue("@studentlname", StudentData.StudentLName);
                Command.Parameters.AddWithValue("@studentnumber", StudentData.StudentNumber);
                Command.Parameters.AddWithValue("@enroldate", StudentData.EnrolDate);

                Command.Parameters.AddWithValue("@id", StudentId);

                Command.ExecuteNonQuery();
            }

            return FindStudent(StudentId);
        }


    }
}
