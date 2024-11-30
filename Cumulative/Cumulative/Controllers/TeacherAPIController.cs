using Cumulative.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace Cumulative.Controllers
{
    [Route("api/Teacher")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        // dependency injection of database context
        private readonly SchoolDbContext _context;

        public TeacherAPIController(SchoolDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of Teacher in the system
        /// </summary>
        /// <example>
        /// GET api/Teacher/ListTeachers  
        /// Response: 
        /// [
        /// {"teacherId": 1, "teacherFName": "Alexander","teacherLName": "Bennett","employeeNumber": "T378","hireDate": "2016-08-05T00:00:00","salary": 55.3},
        /// {"teacherId": 2,"teacherFName": "Caitlin","teacherLName": "Cummings","employeeNumber": "T381","hireDate": "2014-06-10T00:00:00","salary": 62.77},
        /// ..........
        /// ]
        /// </example>
        /// <returns>
        /// A list of Teachers object 
        /// </returns>


        [HttpGet]
        [Route(template: "ListTeachers")]

        public List<Teacher> ListTeachers()
        {
            // Create an empty list of Teacher
            List<Teacher> Teachers = new List<Teacher>();

            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "SELECT * FROM teachers";

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {
                        //Access Column information by the DB column name as an index
                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string TeacherFName = ResultSet["teacherfname"].ToString();
                        string TeacherLName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                        //short form for setting all properties while creating the object
                        Teacher TeachersInfo = new Teacher()
                        {
                            TeacherId = TeacherId,
                            TeacherFName = TeacherFName,
                            TeacherLName = TeacherLName,
                            EmployeeNumber = EmployeeNumber,
                            HireDate = HireDate,
                            Salary = Salary
                        };

                        // Add teacher object to the list
                        Teachers.Add(TeachersInfo);

                    }
                }
            }

            //Return the final list of teachers
            return Teachers;
        }



        /// <summary>
        /// Returns a teacher's details based on the provided teacher ID.
        /// </summary>
        /// <param name="id">The primary key of the teachers table</param>
        /// <example>
        /// GET api/Teacher/FindTeacher/3 -> {"teacherid": 3,"teacherfname": "Linda","teacherlname": "Chan","employeenumber": "T382","hiredate": "2015-08-22 00:00:00","salary": 60.22}
        /// </example>
        /// <returns>
        /// A Teacher object containing the teacher's details such as teacherid, first name, last name, employee number, hire date, and salary.
        /// If no teacher is found with the given ID, an empty Teacher object is returned.
        /// </returns>

        [HttpGet]
        [Route(template: "FindTeacher/{id}")]
        //Create empty Teacher
        public Teacher FindTeacher(int id)
        {
            Teacher SelectedTeacher = SelectedTeacher = new Teacher(); ;


            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();

                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();

                //SQL QUERY
                Command.CommandText = "SELECT * FROM teachers WHERE teacherid=@id";

                // @id is replaced with a id
                Command.Parameters.AddWithValue("@id", id);

                // Gather Result Set of Query into a variable
                using (MySqlDataReader ResultSet = Command.ExecuteReader())
                {
                    //Loop Through Each Row the Result Set
                    while (ResultSet.Read())
                    {

                        int TeacherId = Convert.ToInt32(ResultSet["teacherid"]);
                        string TeacherFName = ResultSet["teacherfname"].ToString();
                        string TeacherLName = ResultSet["teacherlname"].ToString();
                        string EmployeeNumber = ResultSet["employeenumber"].ToString();
                        DateTime HireDate = Convert.ToDateTime(ResultSet["hiredate"]);
                        decimal Salary = Convert.ToDecimal(ResultSet["salary"]);

                        SelectedTeacher.TeacherId = TeacherId;
                        SelectedTeacher.TeacherFName = TeacherFName;
                        SelectedTeacher.TeacherLName = TeacherLName;
                        SelectedTeacher.EmployeeNumber = EmployeeNumber;
                        SelectedTeacher.HireDate = HireDate;
                        SelectedTeacher.Salary = Salary;
                    }
                }
            }

            //Return selected teacher
            return SelectedTeacher;
        }

        /// <summary>
        /// Adds a new teacher to the database.
        /// </summary>
        /// <param name="TeacherData">An object containing the teacher's details.</param>
        /// <example>
        /// POST: api/Teacher/AddTeacher  
        /// Headers: Content-Type: application/json  
        /// Request Body:  
        /// { 
        ///     "teacherFName": "Maria",
        ///     "teacherLName": "Monte",
        ///     "employeeNumber": "T101",
        ///     "hireDate": "2024-11-28T00:00:00",
        ///     "salary": 40.5"
        /// }  
        /// Response: 409 (if there's a conflict, e.g., duplicate entry)  
        /// </example>
        /// <returns>
        /// The ID of the newly inserted teacher if successful, or 0 if the operation fails.
        /// </returns>

        [HttpPost(template: "AddTeacher")]
        public int AddTeacher([FromBody] Teacher TeacherData)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();
                Command.CommandText = "INSERT INTO teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@teacherfname, @teacherlname, @employeenumber, @hiredate, @salary)";
                Command.Parameters.AddWithValue("@teacherfname", TeacherData.TeacherFName);
                Command.Parameters.AddWithValue("@teacherlname", TeacherData.TeacherLName);
                Command.Parameters.AddWithValue("@employeenumber", TeacherData.EmployeeNumber);
                Command.Parameters.AddWithValue("@hiredate", TeacherData.HireDate);
                Command.Parameters.AddWithValue("@salary", TeacherData.Salary);

                Command.ExecuteNonQuery();

                return Convert.ToInt32(Command.LastInsertedId);

            }
            // if failure
            return 0;
        }

        /// <summary>
        /// Deletes a specific teacher record from the database. 
        /// This operation is performed using the primary key associated with the teacher record.
        /// </summary>
        /// <param name="TeacherId"> The unique ID for the teacher to be deleted from the database </param>
        /// <example>
        /// Example usage of this API endpoint:
        /// DELETE: api/TeacherData/DeleteTeacher/11 -> 1
        /// In this example, the teacher with the unique identifier (TeacherID) of 11 will be deleted 
        /// from the database if the record exists.
        /// </example>
        /// <returns>
        /// Returns the number of rows affected by the delete operation.
        /// </returns>


        [HttpDelete(template: "DeleteTeacher/{TeacherId}")]
        public int DeleteTeacher(int TeacherId)
        {
            // 'using' will close the connection after the code executes
            using (MySqlConnection Connection = _context.AccessDatabase())
            {
                Connection.Open();
                //Establish a new command (query) for our database
                MySqlCommand Command = Connection.CreateCommand();


                Command.CommandText = "DELETE FROM teachers WHERE teacherid=@id";
                Command.Parameters.AddWithValue("@id", TeacherId);

                return Command.ExecuteNonQuery();

            }
            // if failure
            return 0;
        }
    }
}
