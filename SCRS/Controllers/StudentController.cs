using System;
using System.Web.Mvc;
using SCRS.DataAccess;
using SCRS.Models;
using System.Linq;
using System.Collections.Generic;

namespace SCRS.Controllers
{
    public class StudentController : Controller
    {
        private readonly DatabaseHelper db = new DatabaseHelper();

        // GET: Student
        public ActionResult Index()
        {
            var students = db.GetAllStudents();
            return View(students);
        }

        // GET: Student/Details/5
        public ActionResult Details(int id)
        {
            var student = db.GetStudentByID(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Student/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.AddStudent(student);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error adding student: " + ex.Message);
                }
            }

            return View(student);
        }

        // GET: Student/Courses/5
        public ActionResult Courses(int id)
        {
            var student = db.GetStudentByID(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            ViewBag.Student = student;
            var courses = db.GetCoursesForStudent(id);
            return View(courses);
        }

        // GET: Student/Register
        public ActionResult Register(int id)
        {
            var student = db.GetStudentByID(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            ViewBag.Student = student;
            ViewBag.Courses = db.GetAllCourses();
            return View();
        }

        // POST: Student/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(int studentId, int courseId)
        {
            try
            {
                db.RegisterStudentForCourse(studentId, courseId);
                return RedirectToAction("Courses", new { id = studentId });
            }
            catch (Exception ex)
            {
                var student = db.GetStudentByID(studentId);
                ViewBag.Student = student;
                ViewBag.Courses = db.GetAllCourses();
                ViewBag.ErrorMessage = ex.Message;
                return View();
            }
        }

        // POST: Student/Drop
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Drop(int studentId, int courseId)
        {
            try
            {
                db.DropStudentFromCourse(studentId, courseId);
                return RedirectToAction("Courses", new { id = studentId });
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return RedirectToAction("Courses", new { id = studentId });
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {
            var student = db.GetStudentByID(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.UpdateStudent(student);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating student: " + ex.Message);
                }
            }
            return View(student);
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            var student = db.GetStudentByID(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                db.DeleteStudent(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Get the student again for the view
                var student = db.GetStudentByID(id);
                ViewBag.ErrorMessage = ex.Message;
                return View(student);
            }
        }
    }
} 