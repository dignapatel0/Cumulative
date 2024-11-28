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
                        double Salary = Convert.ToDouble(ResultSet["salary"]);

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
                        double Salary = Convert.ToDouble(ResultSet["salary"]);

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

    }
}
