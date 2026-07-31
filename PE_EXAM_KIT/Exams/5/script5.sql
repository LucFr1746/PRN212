-- ==========================================
-- Create database
-- ==========================================
CREATE DATABASE DBStudyPlanner;
GO
USE DBStudyPlanner;
GO

-- ==========================================
-- Create tables
-- ==========================================

-- 1. Student table
CREATE TABLE Student (
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    StudentCode NVARCHAR(20) NOT NULL UNIQUE,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100),
    Password NVARCHAR(50) NOT NULL
);

-- 2. Subject table
CREATE TABLE Subject (
    SubjectId INT IDENTITY(1,1) PRIMARY KEY,
    SubjectCode NVARCHAR(20) NOT NULL UNIQUE,
    SubjectName NVARCHAR(100) NOT NULL
);

-- 3. StudyTask table
CREATE TABLE StudyTask (
    TaskId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL  FOREIGN KEY (StudentId)
        REFERENCES Student(StudentId),
    SubjectId INT NOT NULL FOREIGN KEY (SubjectId)
        REFERENCES Subject(SubjectId),
    Description NVARCHAR(255),
    DayOfWeek NVARCHAR(20),
    IsCompleted BIT DEFAULT 0,

);

-- ==========================================
-- Insert sample data
-- ==========================================

-- Students
INSERT INTO Student (StudentCode, FullName, Email, Password)
VALUES 
('ST001', N'Nguyen Van An', 'an@fpt.edu.vn', '123'),
('ST002', N'Tran Thi Binh', 'binh@fpt.edu.vn', '123'),
('ST003', N'Le Hoang Nam', 'nam@fpt.edu.vn', '123');

-- Subjects
INSERT INTO Subject (SubjectCode, SubjectName)
VALUES
('PRN212', N'.NET Programming'),
('DBI202', N'Database Systems'),
('WED201', N'Web Development');

-- StudyTasks
INSERT INTO StudyTask (StudentId, SubjectId, Description, DayOfWeek, IsCompleted)
VALUES
(1, 1, N'Finish Lab 3 exercises', N'Monday', 0),
(1, 2, N'Prepare SQL normalization notes', N'Tuesday', 1),
(2, 3, N'Complete HTML & CSS assignment', N'Wednesday', 0),
(2, 1, N'Write CRUD sample in C#', N'Friday', 1),
(3, 2, N'Practice joins and subqueries', N'Thursday', 0),
(3, 3, N'Build responsive layout demo', N'Saturday', 0);

-- ==========================================
-- Check data
-- ==========================================
SELECT * FROM Student;
SELECT * FROM Subject;
SELECT * FROM StudyTask;


