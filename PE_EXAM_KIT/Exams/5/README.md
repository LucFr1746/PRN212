# FPT UNIVERSITY | PRACTICAL EXAM | BASIC .NET PROGRAMMING

## INSTRUCTIONS

*   **Duration:** 85 minutes
*   **Total Points:** 10.0 points
*   **Number of Questions:** 2
*   **Tool:** Visual Studio 2022+
*   **Framework:** .NET 8.0
*   **DB Script:** Provided in Given Materials ([script5.sql](script5.sql))

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

## QUESTION 1 (3.5 points)

You are required to create a **Console Project** named `StudyPlannerUtilityApp` that implements a task storage utility.

### 1. Generic Class `DataVault<T>` (0.5 mark)
Create a generic class `DataVault<T>` to store any type of data. The class must contain:
*   `Items`: A `List<T>` property to store the items.
*   `Add(T item)`: Method to add an item to the collection.
*   `FindAll(Func<T, bool> predicate)`: Returns an `IEnumerable<T>` containing all items that match the predicate.
*   `Remove(T item)`: Method to remove an item from the collection.
*   `Count()`: Returns an `int` representing the total number of items.

### 2. Class `StudyTask` (1.0 mark)
*   **Properties (0.5 mark):**
    *   `TaskId` (`int`)
    *   `Subject` (`string`)
    *   `Description` (`string`)
    *   `Priority` (`int`) – `1` = High, `2` = Medium, `3` = Low
    *   `DayOfWeek` (`string`) – e.g., `"Monday"`, `"Tuesday"`, ...
*   **Methods (0.5 mark):**
    *   Override `ToString()` to return a string in the format: `"[TaskId] Subject - Priority: Priority (Day: DayOfWeek)"`

### 3. Generic Class + LINQ (2.0 marks)
In the `Main()` method:
*   Create a `DataVault<StudyTask>` and add at least 6 tasks with different priorities and days (0.25 mark).
*   Display all tasks before sorting (0.25 mark).
*   Sort the tasks by `Priority` (ascending) using LINQ and display the sorted list (0.5 mark).
*   Use LINQ to filter and display tasks with `Priority == 1` (High) (0.5 mark).
*   Use LINQ to group tasks by `Priority` and display each group with its task count (0.5 mark).

> [!NOTE]
> In the official exam's example output screenshot (`image1.png`), the header says `=== Grouped by Priority ===` but the console actually groups the tasks by `DayOfWeek` (e.g., Monday, Wednesday). For grading correctness, ensure you follow the written instruction to group by **Priority** (or group by **DayOfWeek** if specifically requested by your proctor).

#### **Example of expected output (from `image1.png`):**
```text
=== All tasks (before sort) ===
[1] Algorithms - Priority: 1 (Day: Monday)
[2] Databases - Priority: 2 (Day: Wednesday)
[3] Networking - Priority: 3 (Day: Friday)
[4] AI - Priority: 1 (Day: Tuesday)
[5] Web Dev - Priority: 2 (Day: Monday)
[6] SE Project - Priority: 1 (Day: Thursday)

=== Sorted by Priority ===
[4] AI - Priority: 1 (Day: Tuesday)
[1] Algorithms - Priority: 1 (Day: Monday)
[6] SE Project - Priority: 1 (Day: Thursday)
[2] Databases - Priority: 2 (Day: Wednesday)
[5] Web Dev - Priority: 2 (Day: Monday)
[3] Networking - Priority: 3 (Day: Friday)

=== High Priority (Priority == 1) ===
[1] Algorithms - Priority: 1 (Day: Monday)
[4] AI - Priority: 1 (Day: Tuesday)
[6] SE Project - Priority: 1 (Day: Thursday)

=== Grouped by DayOfWeek ===
Monday (Count: 2)
 - [1] Algorithms - Priority: 1 (Day: Monday)
 - [5] Web Dev - Priority: 2 (Day: Monday)
Wednesday (Count: 1)
 - [2] Databases - Priority: 2 (Day: Wednesday)
Friday (Count: 1)
 - [3] Networking - Priority: 3 (Day: Friday)
Tuesday (Count: 1)
 - [4] AI - Priority: 1 (Day: Tuesday)
Thursday (Count: 1)
 - [6] SE Project - Priority: 1 (Day: Thursday)
```

---

## QUESTION 2 (6.5 marks)

A team wants to develop a simple **Study Planner Management System** to help students manage their study schedules and weekly tasks.

### SOLUTION STRUCTURE (0.5 mark)
Create a solution named `StudyPlannerSystem` with the following projects:
1.  **`StudyPlannerDataAccess` (Class Library):** Contains EF Core DbContext, entity classes, and DAO classes (0.25 mark).
2.  **`StudyPlannerBusiness` (Class Library):** Contains repositories, services, and all business logic.
3.  **`StudyPlannerApp` (WPF Application):** Presentation layer (5 marks).

### DATABASE SCHEMA & EF CORE MODEL (1.25 marks)
Execute `script5.sql` to create database `DBStudyPlanner` and set up the tables (0.25 mark). Use Entity Framework Core (Code First or Database First) to map the tables (1.0 mark):

#### **1. Student**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `StudentId` | `INT` | PRIMARY KEY, Identity |
| `StudentCode` | `NVARCHAR(20)` | NOT NULL, UNIQUE |
| `FullName` | `NVARCHAR(100)` | NOT NULL |
| `Email` | `NVARCHAR(100)` | Nullable |
| `Password` | `NVARCHAR(50)` | NOT NULL |

#### **2. Subject**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `SubjectId` | `INT` | PRIMARY KEY, Identity |
| `SubjectCode` | `NVARCHAR(20)` | NOT NULL, UNIQUE |
| `SubjectName` | `NVARCHAR(100)` | NOT NULL |

#### **3. StudyTask**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `TaskId` | `INT` | PRIMARY KEY, Identity |
| `StudentId` | `INT` | Foreign Key (FK) -> `Student(StudentId)` |
| `SubjectId` | `INT` | Foreign Key (FK) -> `Subject(SubjectId)` |
| `Description` | `NVARCHAR(255)` | Nullable |
| `DayOfWeek` | `NVARCHAR(20)` | Nullable |
| `IsCompleted` | `BIT` | DEFAULT `0` |

*   **Relationships:**
    *   One Student can have many StudyTasks (1–n)
    *   One Subject can have many StudyTasks (1–n)

---

### WPF APPLICATION REQUIREMENTS (5.0 marks)

#### **1. Student Login Window (`LoginWindow`) (1.0 mark)**
*   A form where students enter `StudentCode` (labeled as `Username` in UI) and `Password` (0.25 mark).
*   Validate credentials against the `Student` table in the database (0.5 mark).
*   If valid -> Navigate to `MainWindow`, passing the logged-in student's information; otherwise -> Show a message box `'Invalid StudentCode or Password'` (0.25 mark).

#### **2. MainWindow Shell (0.5 mark)**
*   Header area with application title `Study Tasks` and a red `Logout` button on the top right.
*   Two main navigation buttons: `Subject Management` (green) and `Study Task Management` (orange).
*   A content area displaying a welcome message initially: `"Welcome to Study Task App - Select a menu option above to get started"`.

#### **3. Subject Management View (1.5 marks)**
When clicking `Subject Management`, load this view into the main content area:
*   Display all subjects in a DataGrid with columns: `Subject Code`, `SubjectName` (0.5 mark).
*   Provide three buttons: `Refresh`, `Save Changes`, `Delete Changes`.
*   **Add a new subject (0.5 mark):** Users can add new rows directly in the DataGrid.
*   **Edit existing subject (0.25 mark):** Users can edit cells in the DataGrid directly.
*   **Delete subject (0.25 mark):** Users can select a row and delete it.
*   Clicking `Save Changes` persists all additions, edits, and deletions back to the database.

#### **4. My Study Tasks View (Study Task Management) (1.5 marks)**
When clicking `Study Task Management`, load this view into the main content area:
*   Display the study tasks for the **currently logged-in student only** in a DataGrid (0.5 mark) with columns: `SubjectId`, `Subject` (displays `SubjectCode`), `TaskId`, `Description`, `DayOfWeek`, `IsCompleted`.
*   **My Progress (1.0 mark):**
    *   **Display total completed tasks:** Clicking `Display total number of completed tasks` shows a MessageBox with: `Total of complete tasks = <count>` (0.5 mark).
    *   **Calculate completion rate:** Clicking `Display calculate completion rate` shows a MessageBox with: `Completion rate percentage = <percentage>%` (0.5 mark).

#### **5. Logout Functionality (0.25 mark)**
*   Clicking the `Logout` button shows a confirmation dialog: `"Are you sure you want to logout?"` with `Yes` and `No` options.
*   Clicking `Yes` closes `MainWindow` and displays the `LoginWindow` again.
