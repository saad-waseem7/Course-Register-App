using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using SCRS.Models;
using System.Linq;

namespace SCRS.DataAccess
{
    public class DatabaseHelper
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["StudentCourseDB"].ConnectionString;

        #region Student Methods

        public List<Student> GetAllStudents()
        {
            List<Student> students = new List<Student>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetAllStudents", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Student student = new Student
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null
                    };

                    students.Add(student);
                }

                reader.Close();
            }

            return students;
        }

        public Student GetStudentByID(int studentID)
        {
            Student student = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetStudentByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentID", studentID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    student = new Student
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]),
                        Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : null
                    };
                }

                reader.Close();
            }

            return student;
        }

        public int AddStudent(Student student)
        {
            int studentID = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddStudent", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                command.Parameters.AddWithValue("@Address", (object)student.Address ?? DBNull.Value);

                connection.Open();
                studentID = Convert.ToInt32(command.ExecuteScalar());
            }

            return studentID;
        }

        public void UpdateStudent(Student student)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UpdateStudent", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentID", student.StudentID);
                command.Parameters.AddWithValue("@FirstName", student.FirstName);
                command.Parameters.AddWithValue("@LastName", student.LastName);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                command.Parameters.AddWithValue("@Address", (object)student.Address ?? DBNull.Value);

                connection.Open();
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error updating student: " + ex.Message);
                }
            }
        }

        public void DeleteStudent(int studentID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DeleteStudent", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentID", studentID);

                connection.Open();
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error deleting student: " + ex.Message);
                }
            }
        }

        #endregion

        #region Course Methods

        public List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetAllCourses", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Course course = new Course
                    {
                        CourseID = Convert.ToInt32(reader["CourseID"]),
                        CourseCode = reader["CourseCode"].ToString(),
                        CourseName = reader["CourseName"].ToString(),
                        Credits = Convert.ToInt32(reader["Credits"]),
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null
                    };

                    courses.Add(course);
                }

                reader.Close();
            }

            return courses;
        }

        public Course GetCourseByID(int courseID)
        {
            Course course = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetCourseByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CourseID", courseID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    course = new Course
                    {
                        CourseID = Convert.ToInt32(reader["CourseID"]),
                        CourseCode = reader["CourseCode"].ToString(),
                        CourseName = reader["CourseName"].ToString(),
                        Credits = Convert.ToInt32(reader["Credits"]),
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null
                    };
                }

                reader.Close();
            }

            return course;
        }

        public int AddCourse(Course course)
        {
            int courseID = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("AddCourse", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.Parameters.AddWithValue("@Credits", course.Credits);
                command.Parameters.AddWithValue("@Description", (object)course.Description ?? DBNull.Value);

                connection.Open();
                courseID = Convert.ToInt32(command.ExecuteScalar());
            }

            return courseID;
        }

        public void UpdateCourse(Course course)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UpdateCourse", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@CourseID", course.CourseID);
                command.Parameters.AddWithValue("@CourseCode", course.CourseCode);
                command.Parameters.AddWithValue("@CourseName", course.CourseName);
                command.Parameters.AddWithValue("@Credits", course.Credits);
                command.Parameters.AddWithValue("@Description", (object)course.Description ?? DBNull.Value);

                connection.Open();
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error updating course: " + ex.Message);
                }
            }
        }

        public void DeleteCourse(int courseID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("DeleteCourse", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CourseID", courseID);

                connection.Open();
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error deleting course: " + ex.Message);
                }
            }
        }

        #endregion

        #region Registration Methods

        public List<Registration> GetAllRegistrations()
        {
            List<Registration> registrations = new List<Registration>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetAllRegistrations", connection);
                command.CommandType = CommandType.StoredProcedure;

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Registration registration = new Registration
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        CourseID = Convert.ToInt32(reader["CourseID"]),
                        Grade = reader["Grade"] != DBNull.Value ? reader["Grade"].ToString() : null,
                        RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                        StudentName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                        CourseName = reader["CourseName"].ToString()
                    };

                    registrations.Add(registration);
                }

                reader.Close();
            }

            return registrations;
        }

        public Registration GetRegistration(int studentID, int courseID)
        {
            Registration registration = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("GetRegistration", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@StudentID", studentID);
                command.Parameters.AddWithValue("@CourseID", courseID);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    registration = new Registration
                    {
                        StudentID = Convert.ToInt32(reader["StudentID"]),
                        CourseID = Convert.ToInt32(reader["CourseID"]),
                        Grade = reader["Grade"] != DBNull.Value ? reader["Grade"].ToString() : null,
                        RegistrationDate = Convert.ToDateTime(reader["RegistrationDate"]),
                        StudentName = reader["FirstName"].ToString() + " " + reader["LastName"].ToString(),
                        CourseName = reader["CourseName"].ToString()
                    };
                }

                reader.Close();
            }

            return registration;
        }

        public void EnrollStudentInCourse(int studentID, int courseID, string grade = null)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("EnrollStudentInCourse", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentID", studentID);
                command.Parameters.AddWithValue("@CourseID", courseID);
                command.Parameters.AddWithValue("@Grade", (object)grade ?? DBNull.Value);

                connection.Open();
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error enrolling student in course: " + ex.Message);
                }
            }
        }

        public void UpdateGrade(int studentID, int courseID, string grade)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("UpdateGrade", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentID", studentID);
                command.Parameters.AddWithValue("@CourseID", courseID);
                command.Parameters.AddWithValue("@Grade", (object)grade ?? DBNull.Value);

                connection.Open();
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error updating grade: " + ex.Message);
                }
            }
        }

        public void RemoveStudentFromCourse(int studentID, int courseID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("RemoveStudentFromCourse", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@StudentID", studentID);
                command.Parameters.AddWithValue("@CourseID", courseID);

                connection.Open();
                
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Error removing student from course: " + ex.Message);
                }
            }
        }

        #endregion

        #region Legacy Methods (For Backwards Compatibility)

        // Legacy method that calls the new method
        public int RegisterStudentForCourse(int studentID, int courseID)
        {
            EnrollStudentInCourse(studentID, courseID);
            return 1; // Just return a placeholder value since we don't need the actual registration ID
        }

        // Legacy method that calls the new method
        public List<Course> GetCoursesForStudent(int studentID)
        {
            List<Course> courses = new List<Course>();
            var registrations = GetAllRegistrations().Where(r => r.StudentID == studentID);
            
            foreach (var reg in registrations)
            {
                Course course = GetCourseByID(reg.CourseID);
                if (course != null)
                {
                    courses.Add(course);
                }
            }
            
            return courses;
        }

        // Legacy method that calls the new method
        public List<Student> GetStudentsInCourse(int courseID)
        {
            List<Student> students = new List<Student>();
            var registrations = GetAllRegistrations().Where(r => r.CourseID == courseID);
            
            foreach (var reg in registrations)
            {
                Student student = GetStudentByID(reg.StudentID);
                if (student != null)
                {
                    students.Add(student);
                }
            }
            
            return students;
        }

        // Legacy method that calls the new method
        public void UpdateStudentGrade(int studentID, int courseID, string grade)
        {
            UpdateGrade(studentID, courseID, grade);
        }

        // Legacy method that calls the new method
        public void DropStudentFromCourse(int studentID, int courseID)
        {
            RemoveStudentFromCourse(studentID, courseID);
        }

        #endregion
    }
} 