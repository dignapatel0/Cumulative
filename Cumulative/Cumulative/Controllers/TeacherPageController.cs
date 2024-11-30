using Cumulative.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative.Controllers
{
    public class TeacherPageController : Controller
    {
        private readonly TeacherAPIController _api;
        //dependency
        public TeacherPageController(TeacherAPIController api)
        {
            _api = api;
        }

        // GET: TeacherPage/List -> A webpage that shows all teachers in the database
        [HttpGet]
        public IActionResult List()
        {

            List<Teacher> Teachers = _api.ListTeachers();
             
            // direct us to the /Views/TeacherPage/List.cshtml
            return View(Teachers);
        }

        //GET : TeacherPage/Show/{id} -> A webpage that shows a particular teacher in the database matching the given id
        [HttpGet]
        public IActionResult Show(int id) 
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);

            // direct us to the /Views/TeacherPage/Show.cshtml
            return View(SelectedTeacher);
        }

        //GET : TeacherPage/New{id} -> A webpage that prompts the user to enter new teacher information
        public IActionResult New(int id)
        {
            // direct us to the /Views/TeacherPage/New.cshtml
            return View();
        }

        //POST: TeacherPage/Create
        public IActionResult Create(Teacher NewTeacher)
        {
            int TeacherId = _api.AddTeacher(NewTeacher);

            // direct us to the /Views/TeacherPage/Show/{Teacherid}
            return RedirectToAction("Show", new { id = TeacherId });
        }

        // GET: TeacherPage/DeleteConfirm/{id} -> A webpage asking the user if they are sure they want to delete this teacher
        public IActionResult DeleteConfirm(int id)
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);

            //directs us to Views/TeacherPage/DeleteConfirm.cshtml
            return View(SelectedTeacher);
        }

        // POST: TeacherPage/Delete/{id} -> A webpage that lists the teacher
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int TeacherId = _api.DeleteTeacher(id);
            
            //directs us to Views/TeacherPage/List.cshtml
            return RedirectToAction("List");
        }
    }
}
