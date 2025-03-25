using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SCRS.Models;
using SCRS.DataAccess;

namespace SCRS.Controllers
{
    public class RegistrationController : Controller
    {
        private DatabaseHelper dbHelper = new DatabaseHelper();

        // GET: Registration
        public ActionResult Index()
        {
            var registrations = dbHelper.GetAllRegistrations();
            return View(registrations);
        }

        // GET: Registration/Create
        public ActionResult Create()
        {
            ViewBag.Students = new SelectList(dbHelper.GetAllStudents(), "StudentID", "FirstName");
            ViewBag.Courses = new SelectList(dbHelper.GetAllCourses(), "CourseID", "CourseName");
            return View();
        }

        // POST: Registration/Create
        [HttpPost]
        public ActionResult Create(Registration registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbHelper.EnrollStudentInCourse(registration.StudentID, registration.CourseID, registration.Grade);
                    return RedirectToAction("Index");
                }
                return View(registration);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error enrolling student: " + ex.Message);
                ViewBag.Students = new SelectList(dbHelper.GetAllStudents(), "StudentID", "FirstName");
                ViewBag.Courses = new SelectList(dbHelper.GetAllCourses(), "CourseID", "CourseName");
                return View(registration);
            }
        }

        // GET: Registration/Edit/5
        public ActionResult Edit(int studentId, int courseId)
        {
            var registration = dbHelper.GetRegistration(studentId, courseId);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registration/Edit
        [HttpPost]
        public ActionResult Edit(Registration registration)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    dbHelper.UpdateGrade(registration.StudentID, registration.CourseID, registration.Grade);
                    return RedirectToAction("Index");
                }
                return View(registration);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating grade: " + ex.Message);
                return View(registration);
            }
        }

        // GET: Registration/Delete
        public ActionResult Delete(int studentId, int courseId)
        {
            var registration = dbHelper.GetRegistration(studentId, courseId);
            if (registration == null)
            {
                return HttpNotFound();
            }
            return View(registration);
        }

        // POST: Registration/Delete
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int studentId, int courseId)
        {
            try
            {
                dbHelper.RemoveStudentFromCourse(studentId, courseId);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error removing student from course: " + ex.Message);
                return View(dbHelper.GetRegistration(studentId, courseId));
            }
        }
    }
} 