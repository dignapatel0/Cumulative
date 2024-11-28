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

        //GET : TeacherPage/Show/{id} -> A webpage that shows a particular teachers in the database matching the given id
        [HttpGet]
        public IActionResult Show(int id) 
        {
            Teacher SelectedTeacher = _api.FindTeacher(id);

            // direct us to the /Views/TeacherPage/Show.cshtml
            return View(SelectedTeacher);
        }
    }
}
