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

        //GET : CoursePage/New{id} -> A webpage that prompts the user to enter new Course information
        public IActionResult New(int id)
        {
            // direct us to the /Views/CoursePage/New.cshtml
            return View();
        }

        //POST: CoursePage/Create
        public IActionResult Create(Course NewCourse)
        {
            int CourseId = _api.AddCourse(NewCourse);

            // direct us to the /Views/CoursePage/Show/{Courseid}
            return RedirectToAction("Show", new { id = CourseId });
        }

        // GET: CoursePage/DeleteConfirm/{id} -> A webpage asking the user if they are sure they want to delete this Course
        public IActionResult DeleteConfirm(int id)
        {
            Course SelectedCourse = _api.FindCourse(id);

            //directs us to Views/CoursePage/DeleteConfirm.cshtml
            return View(SelectedCourse);
        }

        // POST: CoursePage/Delete/{id} -> A webpage that lists the Course
        [HttpPost]
        public IActionResult Delete(int id)
        {
            int CourseId = _api.DeleteCourse(id);

            //directs us to Views/CoursePage/List.cshtml
            return RedirectToAction("List");
        }
    }
}
