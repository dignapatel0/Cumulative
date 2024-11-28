using Cumulative.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative.Controllers
{
    public class StudentPageController : Controller
    {
        private readonly StudentAPIController _api;
        //dependency
        public StudentPageController(StudentAPIController api)
        {
            _api = api;
        }

        // GET: StudentPage/List -> A webpage that shows all student in the database
        [HttpGet]
        public IActionResult List()
        {

            List<Student> Student = _api.ListStudents();

            // direct us to the /Views/StudentPage/List.cshtml
            return View(Student);
        }

        //GET : StudentPage/Show/{id} -> A webpage that shows a particular Student in the database matching the given id
        [HttpGet]
        public IActionResult Show(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);

            // direct us to the /Views/StudentPage/Show.cshtml
            return View(SelectedStudent);
        }
    }
}
