using System;
using System.Web.Mvc;
using SCRS.DataAccess;
using SCRS.Models;
using System.Linq;
using System.Collections.Generic;

namespace SCRS.Controllers
{
    public class CourseController : Controller
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        // GET: Course
        public ActionResult Index()
        {
            var courses = db.GetAllCourses();
            return View(courses);
        }

        // GET: Course/Details/5
        public ActionResult Details(int id)
        {
            var course = db.GetCourseByID(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // GET: Course/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.AddCourse(course);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error adding course: " + ex.Message);
                }
            }

            return View(course);
        }

        // GET: Course/Students/5
        public ActionResult Students(int id)
        {
            var course = db.GetCourseByID(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.Course = course;
            var students = db.GetStudentsInCourse(id);
            return View(students);
        }

        // GET: Course/UpdateGrade
        public ActionResult UpdateGrade(int courseId, int studentId)
        {
            var course = db.GetCourseByID(courseId);
            var student = db.GetStudentByID(studentId);
            
            if (course == null || student == null)
            {
                return HttpNotFound();
            }

            ViewBag.Course = course;
            ViewBag.Student = student;
            
            return View();
        }

        // POST: Course/UpdateGrade
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateGrade(int courseId, int studentId, string grade)
        {
            try
            {
                db.UpdateStudentGrade(studentId, courseId, grade);
                return RedirectToAction("Students", new { id = courseId });
            }
            catch (Exception ex)
            {
                var course = db.GetCourseByID(courseId);
                var student = db.GetStudentByID(studentId);
                
                ViewBag.Course = course;
                ViewBag.Student = student;
                ViewBag.ErrorMessage = ex.Message;
                
                return View();
            }
        }

        // GET: Course/Edit/5
        public ActionResult Edit(int id)
        {
            var course = db.GetCourseByID(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.UpdateCourse(course);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating course: " + ex.Message);
                }
            }
            return View(course);
        }

        // GET: Course/Delete/5
        public ActionResult Delete(int id)
        {
            var course = db.GetCourseByID(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        // POST: Course/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                db.DeleteCourse(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Get the course again for the view
                var course = db.GetCourseByID(id);
                ViewBag.ErrorMessage = ex.Message;
                return View(course);
            }
        }
    }
} 