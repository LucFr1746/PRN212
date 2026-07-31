CREATE DATABASE PRN212_26Spr_Exam8;
GO

USE PRN212_26Spr_Exam8;
GO

-- ============================================
-- 1. TABLES
-- ============================================

CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255) NULL
);
GO

CREATE TABLE Courses
(
    CourseId INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode NVARCHAR(50) NOT NULL,
    Title NVARCHAR(150) NOT NULL,
    DurationHours INT NOT NULL,
    TuitionFee DECIMAL(18,2) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CategoryId INT NOT NULL,
    CONSTRAINT FK_Courses_Categories
        FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);
GO

CREATE TABLE Skills
(
    SkillId INT IDENTITY(1,1) PRIMARY KEY,
    SkillName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE CourseSkills
(
    CourseId INT NOT NULL,
    SkillId INT NOT NULL,
    CONSTRAINT PK_CourseSkills PRIMARY KEY (CourseId, SkillId),
    CONSTRAINT FK_CourseSkills_Courses
        FOREIGN KEY (CourseId) REFERENCES Courses(CourseId) ON DELETE CASCADE,
    CONSTRAINT FK_CourseSkills_Skills
        FOREIGN KEY (SkillId) REFERENCES Skills(SkillId) ON DELETE CASCADE
);
GO

-- ============================================
-- 2. DEMO DATA
-- ============================================

-- Categories: 5 records
INSERT INTO Categories (CategoryName, Description)
VALUES
    (N'Web Development', N'Frontend & Backend web development technologies'),
    (N'Mobile Development', N'Cross-platform and native mobile apps'),
    (N'Data Science & AI', N'Machine Learning, Deep Learning, Big Data'),
    (N'Cloud & DevOps', N'Docker, Kubernetes, AWS & Azure Infrastructure'),
    (N'Software Engineering', N'System Design, OOP, Clean Code & Testing');
GO

-- Courses: 10 records
INSERT INTO Courses (CourseCode, Title, DurationHours, TuitionFee, IsActive, CategoryId)
VALUES
    (N'PRN212', N'Basic Cross-Platform Application Programming with .NET', 60, 250.00, 1, 1),
    (N'PRN231', N'Building Web APIs with ASP.NET Core & Entity Framework', 60, 300.00, 1, 1),
    (N'PRN221', N'Advanced Web Development with Razor Pages & SignalR', 45, 220.00, 1, 1),
    (N'PRM392', N'Android & iOS Mobile Development with Flutter', 60, 280.00, 1, 2),
    (N'PRM391', N'Native Mobile App Development with Swift & Kotlin', 60, 320.00, 0, 2),
    (N'DAT301', N'Data Analytics & Visualization with Python & PowerBI', 40, 200.00, 1, 3),
    (N'AIL302', N'Practical Machine Learning & Neural Networks', 75, 450.00, 1, 3),
    (N'DOP401', N'DevOps Pipeline Automation with Docker & GitHub Actions', 50, 350.00, 1, 4),
    (N'SWE201', N'Object-Oriented Programming & Clean Architecture', 60, 270.00, 1, 5),
    (N'SWE302', N'Software Testing & Automated QA Frameworks', 40, 190.00, 1, 5);
GO

-- Skills: 6 records
INSERT INTO Skills (SkillName)
VALUES
    (N'C# & .NET 8'),
    (N'SQL Server & EF Core'),
    (N'RESTful API Design'),
    (N'Docker & CI/CD'),
    (N'Python & Pandas'),
    (N'Git & Version Control');
GO

-- CourseSkills (Bridge table records)
INSERT INTO CourseSkills (CourseId, SkillId)
VALUES
    (1, 1), (1, 2), (1, 6),
    (2, 1), (2, 2), (2, 3), (2, 6),
    (3, 1), (3, 3),
    (4, 3), (4, 6),
    (5, 3),
    (6, 5), (6, 6),
    (7, 5),
    (8, 4), (8, 6),
    (9, 1), (9, 6),
    (10, 1), (10, 6);
GO
