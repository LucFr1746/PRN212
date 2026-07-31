CREATE DATABASE PRN212_26SprB1_1;
GO

USE PRN212_26SprB1_1;
GO

-- ============================================
-- 1. TABLES
-- ============================================

CREATE TABLE Departments
(
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Employees
(
    EmployeeId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    HireDate DATE NOT NULL,
    DepartmentId INT NOT NULL,
    CONSTRAINT FK_Employees_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);
GO

CREATE TABLE Skills
(
    SkillId INT IDENTITY(1,1) PRIMARY KEY,
    SkillName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE EmployeeSkills
(
    EmployeeId INT NOT NULL,
    SkillId INT NOT NULL,
    CONSTRAINT PK_EmployeeSkills PRIMARY KEY (EmployeeId, SkillId),
    CONSTRAINT FK_EmployeeSkills_Employees
        FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId),
    CONSTRAINT FK_EmployeeSkills_Skills
        FOREIGN KEY (SkillId) REFERENCES Skills(SkillId)
);
GO

-- ============================================
-- 2. DEMO DATA
-- ============================================

-- Departments: 7 records
INSERT INTO Departments (DepartmentName)
VALUES
    (N'Engineering'),
    (N'Marketing'),
    (N'Human Resources'),
    (N'Finance'),
    (N'Sales'),
    (N'Operations'),
    (N'Customer Support');
GO

-- Skills: 7 records
INSERT INTO Skills (SkillName)
VALUES
    (N'C#'),
    (N'SQL'),
    (N'Python'),
    (N'Azure'),
    (N'Communication'),
    (N'Excel'),
    (N'Project Management');
GO

-- Employees: 8 records
INSERT INTO Employees (FullName, Email, Salary, HireDate, DepartmentId)
VALUES
    (N'Nguyen Van A', N'a@fpt.edu.vn', 15000000.00, '2023-01-15', 1),
    (N'Tran Thi B',   N'b@fpt.edu.vn', 12000000.00, '2022-08-01', 2),
    (N'Le Van C',     N'c@fpt.edu.vn', 18000000.00, '2021-03-20', 1),
    (N'Pham Thi D',   N'd@fpt.edu.vn', 11000000.00, '2024-02-10', 3),
    (N'Hoang Van E',  N'e@fpt.edu.vn', 17000000.00, '2020-11-05', 4),
    (N'Do Thi F',     N'f@fpt.edu.vn', 13000000.00, '2023-07-12', 5),
    (N'Bui Van G',    N'g@fpt.edu.vn', 14000000.00, '2022-05-18', 6),
    (N'Vu Thi H',     N'h@fpt.edu.vn', 12500000.00, '2024-01-22', 7);
GO

-- EmployeeSkills: 18 records
INSERT INTO EmployeeSkills (EmployeeId, SkillId)
VALUES
    (1, 1), -- Nguyen Van A - C#
    (1, 2), -- SQL
    (1, 4), -- Azure

    (2, 5), -- Tran Thi B - Communication
    (2, 7), -- Project Management

    (3, 1), -- Le Van C - C#
    (3, 2), -- SQL
    (3, 3), -- Python
    (3, 4), -- Azure

    (4, 5), -- Pham Thi D - Communication
    (4, 7), -- Project Management

    (5, 2), -- Hoang Van E - SQL
    (5, 6), -- Excel

    (6, 5), -- Do Thi F - Communication
    (6, 7), -- Project Management

    (7, 3), -- Bui Van G - Python
    (7, 6), -- Excel

    (8, 5); -- Vu Thi H - Communication
GO

-- ============================================
-- 3. QUICK CHECK
-- ============================================

SELECT * FROM Departments;
SELECT * FROM Skills;
SELECT * FROM Employees;
SELECT * FROM EmployeeSkills;

-- View joined employee list similar to the DataGrid requirement
SELECT
    e.EmployeeId,
    e.FullName,
    e.Email,
    e.Salary,
    e.HireDate,
    d.DepartmentName
FROM Employees e
INNER JOIN Departments d
    ON e.DepartmentId = d.DepartmentId
ORDER BY e.EmployeeId;
GO