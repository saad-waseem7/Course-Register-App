-- Student Course Registration System Database Setup

-- Create the database
CREATE DATABASE StudentCourseDB;
GO

-- Use the database
USE StudentCourseDB;
GO

-- Create Students table
CREATE TABLE Students (
    StudentID INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    DateOfBirth DATE NOT NULL,
    Address NVARCHAR(200)
);
GO

-- Create Courses table
CREATE TABLE Courses (
    CourseID INT PRIMARY KEY IDENTITY(1,1),
    CourseCode NVARCHAR(10) UNIQUE NOT NULL,
    CourseName NVARCHAR(100) NOT NULL,
    Credits INT NOT NULL,
    Description NVARCHAR(500)
);
GO

-- Create Registrations table to establish many-to-many relationship
CREATE TABLE Registrations (
    RegistrationID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT NOT NULL,
    CourseID INT NOT NULL,
    RegistrationDate DATETIME DEFAULT GETDATE(),
    Grade NVARCHAR(2),
    CONSTRAINT FK_Registration_Student FOREIGN KEY (StudentID) REFERENCES Students(StudentID),
    CONSTRAINT FK_Registration_Course FOREIGN KEY (CourseID) REFERENCES Courses(CourseID),
    CONSTRAINT UQ_StudentCourse UNIQUE (StudentID, CourseID)
);
GO

-- Insert sample data
-- Sample Students
INSERT INTO Students (FirstName, LastName, Email, DateOfBirth, Address)
VALUES 
('John', 'Doe', 'john.doe@example.com', '2000-01-15', '123 Main St'),
('Jane', 'Smith', 'jane.smith@example.com', '2001-05-20', '456 Oak Ave'),
('Michael', 'Johnson', 'michael.j@example.com', '1999-11-10', '789 Pine Blvd');
GO

-- Sample Courses
INSERT INTO Courses (CourseCode, CourseName, Credits, Description)
VALUES 
('CS101', 'Introduction to Programming', 3, 'Basic programming concepts using C#'),
('CS201', 'Data Structures', 4, 'Advanced data structures and algorithms'),
('MATH101', 'Calculus I', 3, 'Introduction to differential calculus');
GO

-- Sample Registrations
INSERT INTO Registrations (StudentID, CourseID)
VALUES 
(1, 1),
(1, 2),
(2, 1),
(3, 3);
GO

-- Create stored procedures for common operations

-- Get all students
CREATE PROCEDURE GetAllStudents
AS
BEGIN
    SELECT * FROM Students ORDER BY LastName, FirstName;
END
GO

-- Get all courses
CREATE PROCEDURE GetAllCourses
AS
BEGIN
    SELECT * FROM Courses ORDER BY CourseCode;
END
GO

-- Get student by ID
CREATE PROCEDURE GetStudentByID
    @StudentID INT
AS
BEGIN
    SELECT * FROM Students WHERE StudentID = @StudentID;
END
GO

-- Get course by ID
CREATE PROCEDURE GetCourseByID
    @CourseID INT
AS
BEGIN
    SELECT * FROM Courses WHERE CourseID = @CourseID;
END
GO

-- Add new student
CREATE PROCEDURE AddStudent
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @DateOfBirth DATE,
    @Address NVARCHAR(200) = NULL
AS
BEGIN
    INSERT INTO Students (FirstName, LastName, Email, DateOfBirth, Address)
    VALUES (@FirstName, @LastName, @Email, @DateOfBirth, @Address);
    
    SELECT SCOPE_IDENTITY() AS StudentID;
END
GO

-- Add new course
CREATE PROCEDURE AddCourse
    @CourseCode NVARCHAR(10),
    @CourseName NVARCHAR(100),
    @Credits INT,
    @Description NVARCHAR(500) = NULL
AS
BEGIN
    INSERT INTO Courses (CourseCode, CourseName, Credits, Description)
    VALUES (@CourseCode, @CourseName, @Credits, @Description);
    
    SELECT SCOPE_IDENTITY() AS CourseID;
END
GO

-- Register student for course
CREATE PROCEDURE RegisterStudentForCourse
    @StudentID INT,
    @CourseID INT
AS
BEGIN
    BEGIN TRY
        INSERT INTO Registrations (StudentID, CourseID)
        VALUES (@StudentID, @CourseID);
        
        SELECT SCOPE_IDENTITY() AS RegistrationID;
    END TRY
    BEGIN CATCH
        -- Check if it's a duplicate registration
        IF ERROR_NUMBER() = 2627 -- Violation of unique constraint
        BEGIN
            RAISERROR('Student is already registered for this course.', 16, 1);
        END
        ELSE
        BEGIN
            -- Re-throw the original error
            DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
            RAISERROR(@ErrorMessage, 16, 1);
        END
    END CATCH
END
GO

-- Get courses for a student
CREATE PROCEDURE GetCoursesForStudent
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
CREATE PROCEDURE GetStudentsInCourse
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
CREATE PROCEDURE UpdateStudentGrade
    @StudentID INT,
    @CourseID INT,
    @Grade NVARCHAR(2)
AS
BEGIN
    UPDATE Registrations
    SET Grade = @Grade
    WHERE StudentID = @StudentID AND CourseID = @CourseID;
    
    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Registration not found for this student and course.', 16, 1);
    END
END
GO

-- Drop student from course
CREATE PROCEDURE DropStudentFromCourse
    @StudentID INT,
    @CourseID INT
AS
BEGIN
    DELETE FROM Registrations
    WHERE StudentID = @StudentID AND CourseID = @CourseID;
    
    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Registration not found for this student and course.', 16, 1);
    END
END
GO

-- Update student information
CREATE PROCEDURE UpdateStudent
    @StudentID INT,
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @DateOfBirth DATE,
    @Address NVARCHAR(200) = NULL
AS
BEGIN
    UPDATE Students
    SET FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        DateOfBirth = @DateOfBirth,
        Address = @Address
    WHERE StudentID = @StudentID;
    
    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Student not found.', 16, 1);
    END
END
GO

-- Delete student
CREATE PROCEDURE DeleteStudent
    @StudentID INT
AS
BEGIN
    BEGIN TRY
        -- First delete all registrations for this student
        DELETE FROM Registrations
        WHERE StudentID = @StudentID;
        
        -- Then delete the student
        DELETE FROM Students
        WHERE StudentID = @StudentID;
        
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Student not found.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

-- Update course information
CREATE PROCEDURE UpdateCourse
    @CourseID INT,
    @CourseCode NVARCHAR(10),
    @CourseName NVARCHAR(100),
    @Credits INT,
    @Description NVARCHAR(500) = NULL
AS
BEGIN
    UPDATE Courses
    SET CourseCode = @CourseCode,
        CourseName = @CourseName,
        Credits = @Credits,
        Description = @Description
    WHERE CourseID = @CourseID;
    
    IF @@ROWCOUNT = 0
    BEGIN
        RAISERROR('Course not found.', 16, 1);
    END
END
GO

-- Delete course
CREATE PROCEDURE DeleteCourse
    @CourseID INT
AS
BEGIN
    BEGIN TRY
        -- First delete all registrations for this course
        DELETE FROM Registrations
        WHERE CourseID = @CourseID;
        
        -- Then delete the course
        DELETE FROM Courses
        WHERE CourseID = @CourseID;
        
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR('Course not found.', 16, 1);
        END
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Database setup completed successfully!'; 