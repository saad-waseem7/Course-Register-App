-- This script adds the legacy stored procedures that were renamed in our recent refactoring

-- Get courses for a student
CREATE PROCEDURE [dbo].[GetCoursesForStudent]
    @StudentID INT
AS
BEGIN
    SELECT c.CourseID, c.CourseCode, c.CourseName, c.Credits, c.Description, 
           r.RegistrationDate, r.Grade
    FROM Courses c
    INNER JOIN Registrations r ON c.CourseID = r.CourseID
    WHERE r.StudentID = @StudentID
    ORDER BY c.CourseCode;
END
GO

-- Get students in a course
CREATE PROCEDURE [dbo].[GetStudentsInCourse]
    @CourseID INT
AS
BEGIN
    SELECT s.StudentID, s.FirstName, s.LastName, s.Email, 
           r.RegistrationDate, r.Grade
    FROM Students s
    INNER JOIN Registrations r ON s.StudentID = r.StudentID
    WHERE r.CourseID = @CourseID
    ORDER BY s.LastName, s.FirstName;
END
GO

-- Update student grade
CREATE PROCEDURE [dbo].[UpdateStudentGrade]
    @StudentID INT,
    @CourseID INT,
    @Grade NVARCHAR(2)
AS
BEGIN
    -- Calls the new renamed procedure
    EXEC [dbo].[UpdateGrade] @StudentID, @CourseID, @Grade
END
GO

-- Register student for course
CREATE PROCEDURE [dbo].[RegisterStudentForCourse]
    @StudentID INT,
    @CourseID INT
AS
BEGIN
    -- Calls the new renamed procedure
    EXEC [dbo].[EnrollStudentInCourse] @StudentID, @CourseID, NULL
END
GO

-- Drop student from course
CREATE PROCEDURE [dbo].[DropStudentFromCourse]
    @StudentID INT,
    @CourseID INT
AS
BEGIN
    -- Calls the new renamed procedure
    EXEC [dbo].[RemoveStudentFromCourse] @StudentID, @CourseID
END
GO 