# FPT UNIVERSITY | PRACTICAL EXAM | BASIC .NET PROGRAMMING

## INSTRUCTIONS

*   **Duration:** 85 minutes
*   **Total Points:** 10.0 points
*   **Number of Questions:** 2
*   **Tool:** Visual Studio 2022+
*   **Framework:** .NET 8.0
*   **DB Script:** Provided in Given Materials ([script1.sql](../../script1.sql))

### Guidelines:
*   **Allowed:** Personal computer, notebook, textbook.
*   **NOT Allowed:** Any network communication or data sharing with other students.
*   **Use the given Solution file provided.** Do NOT create a new solution.
*   **Do NOT install additional NuGet packages.**
*   **Execute the provided .sql script** to set up the database before starting Question 2.
*   All projects must target **.NET 8.0** and use **Visual Studio 2022+**.
*   Violating ANY of the above will invalidate your submission.
*   **On completion:** submit the entire solution folder. You may delete `[bin]` and `[obj]` folders to reduce size.

---

## QUESTION 1 (4 points)

You are required to create a **Console Application** that implements a student grade management system. Your application must demonstrate the following .NET concepts:
*   **Inheritance** (Abstract class and subclass)
*   **Interface implementation**
*   **LINQ queries on collections**
*   **Exception Handling** (try-catch, custom exception throwing)
*   **Delegate and callback function**

> [!IMPORTANT]
> *   All code must be inside namespace `Q1`.
> *   Use correct C# naming conventions (`PascalCase` for types, `camelCase` for variables).
> *   All classes, interfaces, and delegates must be `public`.
> *   Implement ALL members exactly as specified (name, return type, parameters).

### 1. DELEGATE
Declare delegate `ScoreUpdateHandler` with the following signature:
```csharp
public delegate void ScoreUpdateHandler(string studentName, double newScore);
```

### 2. INTERFACE
Create interface `IEvaluatable` with the following members:
*   `double AverageScore { get; }` (read-only computed property)
*   `string GetRank()`: returns a grade rank string based on `AverageScore` range below:

| AverageScore Range | Rank |
| :--- | :--- |
| `>= 8.5` | `"Excellent"` |
| `>= 7.0` and `< 8.5` | `"Good"` |
| `>= 5.0` and `< 7.0` | `"Average"` |
| `< 5.0` | `"Fail"` |

### 3. CLASSES

#### **Abstract Class `Student`**
*   **Properties:**
    *   `string StudentId { get; set; }`
    *   `string FullName { get; set; }`
    *   `List<double> Scores { get; set; }`
*   **Constructor:**
    *   `Student(string studentId, string fullName)` – initializes properties, sets `Scores` to an empty list.
*   **Methods:**
    *   `void AddScore(double score)` – validates that `score` is in range `[0, 10]`. Throws `ArgumentOutOfRangeException` with message `"Score must be between 0 and 10"` if invalid; otherwise adds it to `Scores`.
    *   `abstract string GetRank()` – abstract method to be implemented by subclasses.

#### **Class `UndergraduateStudent` (extends `Student`, implements `IEvaluatable`)**
*   **Property:**
    *   `string Major { get; set; }`
*   **Constructor:**
    *   `UndergraduateStudent(string studentId, string fullName, string major)` – calls base constructor and sets `Major`.
*   **Implementation:**
    *   `double AverageScore { get; }` – returns the arithmetic mean of `Scores`; returns `0` if `Scores` is empty.
    *   `override string GetRank()` – implements the rank logic using `AverageScore` and the rank table above.
    
    > [!NOTE]
    > Because `Student` already declares abstract `string GetRank()` with the same signature as `IEvaluatable.GetRank()`, a single `public override string GetRank()` satisfies both the abstract override and the interface contract.

#### **Class `ScoreManager`**
*   **Private fields:**
    *   `_students` of type `List<UndergraduateStudent>`
    *   `_onScoreUpdated` of type `ScoreUpdateHandler`
*   **Constructor:**
    *   `ScoreManager(ScoreUpdateHandler handler)` – initializes empty list and stores the handler.
*   **Methods:**
    *   `void AddStudent(UndergraduateStudent student)` – adds the student to the collection.
    *   `void AddScoreToStudent(string studentId, double score)`:
        *   Finds the student with matching `StudentId`.
        *   If not found, throws `InvalidOperationException` with message `"Student not found: [studentId]"`.
        *   If found, calls `student.AddScore(score)`, then invokes `_onScoreUpdated` with `(student.FullName, score)`.
    *   `List<UndergraduateStudent> GetTopStudents(int n)` – uses LINQ to return the top `n` students ordered by `AverageScore` descending.

### 4. TESTING (Main Method)
Write your own `Main` method that demonstrates all functionality:
1.  Create at least 3 `UndergraduateStudent` objects with different majors.
2.  Add each student to a `ScoreManager` instance (callback prints score updates to console).
3.  Add multiple scores to students (include a case that triggers `ArgumentOutOfRangeException` and handle it with `try-catch`).
4.  Call `GetTopStudents(2)` and print results.

#### **Example of expected output (for reference only):**
```text
[SCORE UPDATE]: Nguyen Van A received score: 8.5
[SCORE UPDATE]: Tran Thi B received score: 7.0
[SCORE UPDATE]: Le Van C received score: 6.0
Error: Score must be between 0 and 10

=== Top 2 Students ===
1. Nguyen Van A | Major: IT | Average: 8.50 | Rank: Excellent
2. Tran Thi B   | Major: BA | Average: 7.00 | Rank: Good
```

> [!NOTE]
> The grader will replace your `Main` method with their own test. Make sure ALL classes, interfaces, and methods are public and work correctly.

---

## QUESTION 2 (6 points)

You are asked to build a **WPF Application** that allows listing, filtering, and adding employee information stored in a company database.

> [!IMPORTANT]
> *   **0 points** will be given if the database connection string is **NOT** read from `appsettings.json`.
> *   Use only the provided solution; do **NOT** add extra NuGet packages.
> *   Run the provided `.sql` script before coding to set up the database.

### DATABASE SCHEMA
The database contains 4 tables as described below:

#### **1. Departments**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `DepartmentId` | `INT` | PRIMARY KEY, Identity |
| `DepartmentName` | `NVARCHAR(100)` | NOT NULL |

#### **2. Employees**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `EmployeeId` | `INT` | PRIMARY KEY, Identity |
| `FullName` | `NVARCHAR(100)` | NOT NULL |
| `Email` | `NVARCHAR(150)` | NOT NULL |
| `Salary` | `DECIMAL(18,2)` | NOT NULL |
| `HireDate` | `DATE` | NOT NULL |
| `DepartmentId` | `INT` | Foreign Key (FK) -> `Departments(DepartmentId)` |

#### **3. Skills**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `SkillId` | `INT` | PRIMARY KEY, Identity |
| `SkillName` | `NVARCHAR(100)` | NOT NULL |

#### **4. EmployeeSkills**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `EmployeeId` | `INT` | Foreign Key (FK) -> `Employees(EmployeeId)` |
| `SkillId` | `INT` | Foreign Key (FK) -> `Skills(SkillId)` |

*   **Composite Primary Key:** `(EmployeeId, SkillId)`

---

### REQUIRED UI LAYOUT
The application window must contain **3 clearly separated areas**:

```text
+-------------------------------------------------------------------------------------------------+
| Employee and Skills Management                                                          - [] X  |
+-------------------------------------------------------------------------------------------------+
| FILTER AREA                                                                                     |
|   Department: [ All                   v ]   Skill: [ All                 v ]   [Filter]  [Clear]|
|                                                                                                 |
| EMPLOYEE LIST                                                                                   |
|   +------------+---------------+---------------+---------------+------------+-----------------+ |
|   | EmployeeId | FullName      | Email         | Salary        | HireDate   | DepartmentName  | |
|   +------------+---------------+---------------+---------------+------------+-----------------+ |
|   | 1          | Nguyen Van A  | a@fpt.edu.vn  | 15,000,000.00 | 2023-01-15 | Engineering     | |
|   | 2          | Tran Thi B    | b@fpt.edu.vn  | 12,000,000.00 | 2022-08-01 | Marketing       | |
|   | ...        | ...           | ...           | ...           | ...        | ...             | |
|   +------------+---------------+---------------+---------------+------------+-----------------+ |
|                                                                                                 |
| ADD NEW EMPLOYEE                                                                                |
|   Full Name: [                        ]  Email: [                 ]  Salary: [                ] |
|   Department: [ Customer Support      v ]                                                       |
|   Skills:                                                                                       |
|     +----------------------------+                                                              |
|     | [ ] Azure               |#|                                                              |
|     | [ ] C#                  | |                                                              |
|     | [ ] Communication       | |                                                              |
|     | [ ] Excel               | |                                                              |
|     | [ ] Project Management  | |                                                              |
|     +----------------------------+                                                              |
|                                                                     [Add Employee] [Clear Form] |
+-------------------------------------------------------------------------------------------------+
```

---

### DETAILED REQUIREMENTS

#### **1. Load and Display Data**
*   Load the Employee list from the `Employees` table into the DataGrid, displaying columns: `EmployeeId`, `FullName`, `Email`, `Salary`, `HireDate`, and `DepartmentName` (joined from `Departments`).
*   Load the `Department` list into the Filter ComboBox (`Department`) and into the Add New Employee ComboBox (`Department`).
*   Load the `Skill` list into the Filter ComboBox (`Skill`) and into the CheckedListBox in the Add area.
*   **Both Filter ComboBoxes (Department and Skill) MUST include an "All" option as the first item.**

#### **2. Filter Employees**
*   **Filter by Department:** When a specific department is selected, display only employees in that department. When `"All"` is selected, display all employees.
*   **Filter by Skill:** When a specific skill is selected, display only employees who have that skill (JOIN with `EmployeeSkills`). When `"All"` is selected, display all employees.
*   Both filters can be applied **simultaneously** (e.g., show only Engineering employees who also have C# skill).

#### **3. Add New Employee**
*   The user enters: `FullName`, `Email`, `Salary` (numeric), selects a Department from ComboBox, and selects at least 1 Skill from the CheckedListBox.
*   `HireDate` is **NOT** required in the form, set it to `DateTime.Today` when inserting into the database.
*   **Click Add Employee:**
    *   Validate that all fields are filled and at least 1 Skill is selected. Show an appropriate error message (`MessageBox`) if validation fails.
    *   Insert a new record into the `Employees` table.
    *   Insert corresponding records into `EmployeeSkills` for each selected skill.
    *   Reload the DataGrid and clear the form on success.
*   **Click Clear Form:**
    *   Clears all input controls in the Add area (all TextBoxes empty, ComboBox reset to default, all CheckedListBox items unchecked).
