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

        //GET : StudentPage/New{id} -> A webpage that prompts the user to enter new Student information
        public IActionResult New(int id)
        {
            // direct us to the /Views/StudentPage/New.cshtml
            return View();
        }

        //POST: StudentPage/Create
        public IActionResult Create(Student NewStudent)
        {
            int StudentId = _api.AddStudent(NewStudent);

            // direct us to the /Views/StudentPage/Show/{Studentid}
            return RedirectToAction("Show", new { id = StudentId });
        }

        // GET: StudentPage/DeleteConfirm/{id} -> A webpage asking the user if they are sure they want to delete this Student
        public IActionResult DeleteConfirm(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);

            //directs us to Views/StudentPage/DeleteConfirm.cshtml
            return View(SelectedStudent);
        }

        // POST: StudentPage/Delete/{id} -> A webpage that lists the Student
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int StudentId = _api.DeleteStudent(id);

            //directs us to Views/StudentPage/List.cshtml
            return RedirectToAction("List");
        }

        // GET : StudentPage/Edit/{id}
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Student SelectedStudent = _api.FindStudent(id);
            return View(SelectedStudent);
        }

        // POST: StudentPage/Update/{id}
        [HttpPost]
        public IActionResult Update(int id, string StudentFName, string StudentLName, string StudentNumber, string EnrolDate)
        {
            Student UpdatedStudent = new Student();
            UpdatedStudent.StudentFName = StudentFName;
            UpdatedStudent.StudentLName = StudentLName;
            UpdatedStudent.StudentNumber = StudentNumber;
            UpdatedStudent.EnrolDate = EnrolDate;


            // not doing anything with the response
            _api.UpdateStudent(id, UpdatedStudent);


            // redirects to show Student
            return RedirectToAction("Show", new { id });
        }
    }
}
