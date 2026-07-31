# FPT UNIVERSITY | PRACTICAL EXAM | BASIC .NET PROGRAMMING

## INSTRUCTIONS

*   **Duration:** 85 minutes
*   **Total Points:** 10.0 points
*   **Number of Questions:** 2
*   **Tool:** Visual Studio 2022+
*   **Framework:** .NET 8.0
*   **DB Script:** Provided in Given Materials ([script6.sql](script6.sql))

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

You are required to create a **Console Application** that implements a course feedback management system. Your application must demonstrate the following .NET concepts:
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
Declare delegate `FeedbackHandler` with the following signature:
```csharp
public delegate void FeedbackHandler(string courseName, double rating);
```

### 2. INTERFACE
Create interface `IRatable` with the following members:
*   `double AverageRating { get; }` (read-only computed property)
*   `string GetGrade()`: returns a grade string based on `AverageRating` range below:

| AverageRating Range | Grade |
| :--- | :--- |
| `>= 4.5` | `"Outstanding"` |
| `>= 3.5` and `< 4.5` | `"Good"` |
| `>= 2.5` and `< 3.5` | `"Satisfactory"` |
| `< 2.5` | `"Poor"` |

### 3. CLASSES

#### **Abstract Class `Course`**
*   **Properties:**
    *   `string CourseId { get; set; }`
    *   `string CourseName { get; set; }`
    *   `List<double> Ratings { get; set; }`
*   **Constructor:**
    *   `Course(string courseId, string courseName)` – initializes properties, sets `Ratings` to an empty list.
*   **Methods:**
    *   `void AddRating(double rating)` – validates that `rating` is in range `[1, 5]`. Throws `ArgumentOutOfRangeException` with message `"Rating must be between 1 and 5"` if invalid; otherwise adds it to `Ratings`.
    *   `abstract string GetGrade()` – abstract method to be implemented by subclasses.

#### **Class `OnlineCourse` (extends `Course`, implements `IRatable`)**
*   **Property:**
    *   `string Platform { get; set; }`
*   **Constructor:**
    *   `OnlineCourse(string courseId, string courseName, string platform)` – calls base constructor and sets `Platform`.
*   **Implementation:**
    *   `double AverageRating { get; }` – returns the arithmetic mean of `Ratings`; returns `0` if `Ratings` is empty.
    *   `override string GetGrade()` – implements the grade logic using `AverageRating` and the grade table above.
    
    > [!NOTE]
    > Because `Course` already declares abstract `string GetGrade()` with the same signature as `IRatable.GetGrade()`, a single `public override string GetGrade()` satisfies both the abstract override and the interface contract.

#### **Class `FeedbackManager`**
*   **Private fields:**
    *   `_courses` of type `List<OnlineCourse>`
    *   `_onFeedbackReceived` of type `FeedbackHandler`
*   **Constructor:**
    *   `FeedbackManager(FeedbackHandler handler)` – initializes empty list and stores the handler.
*   **Methods:**
    *   `void AddCourse(OnlineCourse course)` – adds the course to the collection.
    *   `void AddRatingToCourse(string courseId, double rating)`:
        *   Finds the course with matching `CourseId`.
        *   If not found, throws `InvalidOperationException` with message `"Course not found: [courseId]"`.
        *   If found, calls `course.AddRating(rating)`, then invokes `_onFeedbackReceived` with `(course.CourseName, rating)`.
    *   `List<OnlineCourse> GetTopCourses(int n)` – uses LINQ to return the top `n` courses ordered by `AverageRating` descending.

### 4. TESTING (Main Method)
Write your own `Main` method that demonstrates all functionality:
1.  Create at least 3 `OnlineCourse` objects with different platforms.
2.  Add each course to a `FeedbackManager` instance (callback prints feedback to console).
3.  Add multiple ratings to courses (include a case that triggers `ArgumentOutOfRangeException` and handle it with `try-catch`).
4.  Call `GetTopCourses(2)` and print results.

#### **Example of expected output (for reference only):**
```text
[FEEDBACK]: C# Masterclass received rating: 4.5
[FEEDBACK]: Python Basics received rating: 3.8
[FEEDBACK]: Web Development received rating: 4.0
Error: Rating must be between 1 and 5

=== Top 2 Courses ===
1. C# Masterclass | Platform: Udemy | Average: 4.50 | Grade: Outstanding
2. Web Development | Platform: Coursera | Average: 4.00 | Grade: Good
```

> [!NOTE]
> The grader will replace your `Main` method with their own test. Make sure ALL classes, interfaces, and methods are public and work correctly.

---

## QUESTION 2 (6 points)

You are asked to build a **WPF Application** that allows listing, filtering, and adding book information stored in a bookstore database.

> [!IMPORTANT]
> *   **0 points** will be given if the database connection string is **NOT** read from `appsettings.json`.
> *   Use only the provided solution; do **NOT** add extra NuGet packages.
> *   Run the provided `.sql` script before coding to set up the database.

### DATABASE SCHEMA
The database contains 4 tables as described below:

#### **1. Authors**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `AuthorId` | `INT` | PRIMARY KEY, Identity |
| `AuthorName` | `NVARCHAR(100)` | NOT NULL |

#### **2. Books**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `BookId` | `INT` | PRIMARY KEY, Identity |
| `Title` | `NVARCHAR(150)` | NOT NULL |
| `Price` | `DECIMAL(18,2)` | NOT NULL |
| `PublishYear` | `INT` | NOT NULL |
| `AuthorId` | `INT` | Foreign Key (FK) -> `Authors(AuthorId)` |

#### **3. Genres**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `GenreId` | `INT` | PRIMARY KEY, Identity |
| `GenreName` | `NVARCHAR(100)` | NOT NULL |

#### **4. BookGenres**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `BookId` | `INT` | Foreign Key (FK) -> `Books(BookId)` |
| `GenreId` | `INT` | Foreign Key (FK) -> `Genres(GenreId)` |

*   **Composite Primary Key:** `(BookId, GenreId)`

---

### REQUIRED UI LAYOUT
The application window must contain **3 clearly separated areas**:

```text
+-------------------------------------------------------------------------------------------------+
| Book & Genre Management                                                                 - [] X  |
+-------------------------------------------------------------------------------------------------+
| FILTER AREA                                                                                     |
|   Author: [ All                       v ]   Genre: [ All                v ]   [Filter]  [Clear] |
|                                                                                                 |
| BOOK LIST                                                                                       |
|   +--------+----------------------------------+-----------+-------------+----------------------+ |
|   | BookId | Title                            | Price     | PublishYear | AuthorName           | |
|   +--------+----------------------------------+-----------+-------------+----------------------+ |
|   | 1      | Mat Biec                         | 85,000.00 | 2019        | Nguyen Nhat Anh      | |
|   | 3      | The Alchemist                    | 250,000   | 1988        | Paulo Coelho         | |
|   | ...    | ...                              | ...       | ...         | ...                  | |
|   +--------+----------------------------------+-----------+-------------+----------------------+ |
|                                                                                                 |
| ADD NEW BOOK                                                                                    |
|   Title: [                              ]  Price: [                ]  Publish Year: [          ]|
|   Author: [ Nguyen Nhat Anh             v ]                                                     |
|   Genres:                                                                                       |
|     +----------------------------+                                                              |
|     | [ ] Fiction             |#|                                                              |
|     | [ ] Non-Fiction         | |                                                              |
|     | [ ] Fantasy             | |                                                              |
|     | [ ] Self-Help           | |                                                              |
|     | [ ] Science             | |                                                              |
|     | [ ] Romance             | |                                                              |
|     +----------------------------+                                                              |
|                                                                          [Add Book] [Clear Form]|
+-------------------------------------------------------------------------------------------------+
```

---

### DETAILED REQUIREMENTS

#### **1. Load and Display Data**
*   Load the Book list from the `Books` table into the DataGrid, displaying columns: `BookId`, `Title`, `Price`, `PublishYear`, and `AuthorName` (joined from `Authors`).
*   Load the `Author` list into the Filter ComboBox (`Author`) and into the Add New Book ComboBox (`Author`).
*   Load the `Genre` list into the Filter ComboBox (`Genre`) and into the CheckedListBox in the Add area.
*   **Both Filter ComboBoxes (Author and Genre) MUST include an "All" option as the first item.**

#### **2. Filter Books**
*   **Filter by Author:** When a specific author is selected, display only books by that author. When `"All"` is selected, display all books.
*   **Filter by Genre:** When a specific genre is selected, display only books that belong to that genre (JOIN with `BookGenres`). When `"All"` is selected, display all books.
*   Both filters can be applied **simultaneously** (e.g., show only books by Haruki Murakami that also belong to the Fiction genre).

#### **3. Add New Book**
*   The user enters: `Title`, `Price` (numeric), `PublishYear` (numeric), selects an Author from ComboBox, and selects at least 1 Genre from the CheckedListBox.
*   **Click Add Book:**
    *   Validate that all fields are filled and at least 1 Genre is selected. Show an appropriate error message (`MessageBox`) if validation fails.
    *   Insert a new record into the `Books` table.
    *   Insert corresponding records into `BookGenres` for each selected genre.
    *   Reload the DataGrid and clear the form on success.
*   **Click Clear Form:**
    *   Clears all input controls in the Add area (all TextBoxes empty, ComboBox reset to default, all CheckedListBox items unchecked).
