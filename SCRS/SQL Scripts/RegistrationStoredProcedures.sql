-- Get all registrations with student and course information
CREATE PROCEDURE [dbo].[GetAllRegistrations]
AS
BEGIN
    SELECT r.StudentID, r.CourseID, r.Grade, r.RegistrationDate, 
           s.FirstName, s.LastName, 
           c.CourseName
    FROM Registrations r
    INNER JOIN Students s ON r.StudentID = s.StudentID
    INNER JOIN Courses c ON r.CourseID = c.CourseID
    ORDER BY r.RegistrationDate DESC;
END
GO

-- Get a specific registration
CREATE PROCEDURE [dbo].[GetRegistration]
    @StudentID INT,
    @CourseID INT
AS
BEGIN
    SELECT r.StudentID, r.CourseID, r.Grade, r.RegistrationDate, 
           s.FirstName, s.LastName, 
           c.CourseName
    FROM Registrations r
    INNER JOIN Students s ON r.StudentID = s.StudentID
    INNER JOIN Courses c ON r.CourseID = c.CourseID
    WHERE r.StudentID = @StudentID AND r.CourseID = @CourseID;
END
GO

-- Enroll a student in a course
CREATE PROCEDURE [dbo].[EnrollStudentInCourse]
    @StudentID INT,
    @CourseID INT,
    @Grade NVARCHAR(2) = NULL
AS
BEGIN
    -- Check if student exists
    IF NOT EXISTS (SELECT 1 FROM Students WHERE StudentID = @StudentID)
    BEGIN
        RAISERROR('Student does not exist.', 16, 1);
        RETURN;
    END

    -- Check if course exists
    IF NOT EXISTS (SELECT 1 FROM Courses WHERE CourseID = @CourseID)
    BEGIN
        RAISERROR('Course does not exist.', 16, 1);
        RETURN;
    END

    -- Check if already enrolled
    IF EXISTS (SELECT 1 FROM Registrations WHERE StudentID = @StudentID AND CourseID = @CourseID)
    BEGIN
        RAISERROR('Student is already enrolled in this course.', 16, 1);
        RETURN;
    END

    -- Insert new registration
    INSERT INTO Registrations (StudentID, CourseID, RegistrationDate, Grade)
    VALUES (@StudentID, @CourseID, GETDATE(), @Grade);
END
GO

-- Update a student's grade in a course
CREATE PROCEDURE [dbo].[UpdateGrade]
    @StudentID INT,
    @CourseID INT,
    @Grade NVARCHAR(2)
AS
BEGIN
    -- Check if registration exists
    IF NOT EXISTS (SELECT 1 FROM Registrations WHERE StudentID = @StudentID AND CourseID = @CourseID)
    BEGIN
        RAISERROR('Registration does not exist.', 16, 1);
        RETURN;
    END

    -- Update the grade
    UPDATE Registrations
    SET Grade = @Grade
    WHERE StudentID = @StudentID AND CourseID = @CourseID;
END
GO

-- Remove a student from a course
CREATE PROCEDURE [dbo].[RemoveStudentFromCourse]
    @StudentID INT,
    @CourseID INT
AS
BEGIN
    -- Check if registration exists
    IF NOT EXISTS (SELECT 1 FROM Registrations WHERE StudentID = @StudentID AND CourseID = @CourseID)
    BEGIN
        RAISERROR('Registration does not exist.', 16, 1);
        RETURN;
    END

    -- Delete the registration
    DELETE FROM Registrations
    WHERE StudentID = @StudentID AND CourseID = @CourseID;
END
GO 