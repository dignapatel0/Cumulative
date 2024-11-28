using Cumulative.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cumulative.Controllers
{
    public class CoursePageController : Controller
    {
        private readonly CourseAPIController _api;
        //dependency
        public CoursePageController(CourseAPIController api)
        {
            _api = api;
        }

        // GET: CoursePage/List -> A webpage that shows all courses in the database
        [HttpGet]
        public IActionResult List()
        {

            List<Course> Courses = _api.ListCourses();

            // direct us to the /Views/CoursePage/List.cshtml
            return View(Courses);
        }

        //GET : CoursePage/Show/{id} -> A webpage that shows a particular courses in the database matching the given id
        [HttpGet]
        public IActionResult Show(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);

            // direct us to the /Views/CoursePage/Show.cshtml
            return View(SelectedCourse);
        }
    }
}
