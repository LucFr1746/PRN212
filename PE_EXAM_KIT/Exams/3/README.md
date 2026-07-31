# FPT UNIVERSITY | PRACTICAL EXAM | BASIC .NET PROGRAMMING

## INSTRUCTIONS

*   **Duration:** 85 minutes
*   **Total Points:** 10.0 points
*   **Number of Questions:** 2
*   **Tool:** Visual Studio 2022+
*   **Framework:** .NET 8.0
*   **DB Script:** Provided in Given Materials ([script3.sql](script3.sql))

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

You are asked to build a **WPF Application** that allows listing, filtering, and adding product information stored in an online store database.

> [!IMPORTANT]
> *   **0 points** will be given if the database connection string is **NOT** read from `appsettings.json`.
> *   Use only the provided solution; do **NOT** add extra NuGet packages.
> *   Run the provided `.sql` script before coding to set up the database.

### DATABASE SCHEMA
The database contains 4 tables as described below:

#### **1. Categories**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `CategoryId` | `INT` | PRIMARY KEY, Identity |
| `CategoryName` | `NVARCHAR(100)` | NOT NULL |

#### **2. Products**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `ProductId` | `INT` | PRIMARY KEY, Identity |
| `ProductName` | `NVARCHAR(100)` | NOT NULL |
| `Price` | `DECIMAL(18,2)` | NOT NULL |
| `Stock` | `INT` | NOT NULL |
| `CategoryId` | `INT` | Foreign Key (FK) -> `Categories(CategoryId)` |

#### **3. Suppliers**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `SupplierId` | `INT` | PRIMARY KEY, Identity |
| `SupplierName` | `NVARCHAR(100)` | NOT NULL |
| `ContactEmail` | `NVARCHAR(150)` | NOT NULL |

#### **4. ProductSuppliers**
| Column | Data Type | Notes |
| :--- | :--- | :--- |
| `ProductId` | `INT` | Foreign Key (FK) -> `Products(ProductId)` |
| `SupplierId` | `INT` | Foreign Key (FK) -> `Suppliers(SupplierId)` |

*   **Composite Primary Key:** `(ProductId, SupplierId)`

---

### REQUIRED UI LAYOUT
The application window must contain **3 clearly separated areas**:

```text
+-------------------------------------------------------------------------------------------------+
| MainWindow                                                                              - [] X  |
+-------------------------------------------------------------------------------------------------+
| FILTER AREA                                                                                     |
|   Category: [ All                     v ]   Supplier: [ All              v ]   [Filter]  [Clear]|
|                                                                                                 |
| PRODUCT LIST                                                                                    |
|   +------------+---------------------+---------------+---------------+-------------------------+ |
|   | ID         | Product Name        | Price         | Stock         | Category                | |
|   +------------+---------------------+---------------+---------------+-------------------------+ |
|   | 1          | Laptop Pro          | 999.99        | 50            | Computers               | |
|   | 2          | Wireless Mouse      | 29.99         | 200           | Accessories             | |
|   | ...        | ...                 | ...           | ...           | ...                     | |
|   +------------+---------------------+---------------+---------------+-------------------------+ |
|                                                                                                 |
| ADD NEW PRODUCT                                                                                 |
|   Product Name: [                     ]  Price: [                 ]  Stock: [                 ] |
|   Category: [ Accessories             v ]                                                       |
|   Suppliers:                                                                                    |
|     +----------------------------+                                                              |
|     | [ ] DigiSource          |#|                                                              |
|     | [ ] FastShip            | |                                                              |
|     | [ ] GlobalDist          | |                                                              |
|     | [ ] MegaWholesale       | |                                                              |
|     | [ ] NovaTrade           | |                                                              |
|     +----------------------------+                                                              |
|                                                                        [Add Product] [Clear Form] |
+-------------------------------------------------------------------------------------------------+
```

---

### DETAILED REQUIREMENTS

#### **1. Load and Display Data**
*   Load the Product list from the `Products` table into the DataGrid, displaying columns: `ProductId`, `ProductName`, `Price`, `Stock`, and `CategoryName` (joined from `Categories`).
*   Load the `Category` list into the Filter ComboBox (`Category`) and into the Add New Product ComboBox (`Category`).
*   Load the `Supplier` list into the Filter ComboBox (`Supplier`) and into the CheckedListBox in the Add area.
*   **Both Filter ComboBoxes (Category and Supplier) MUST include an "All" option as the first item.**

#### **2. Filter Products**
*   **Filter by Category:** When a specific category is selected, display only products in that category. When `"All"` is selected, display all products.
*   **Filter by Supplier:** When a specific supplier is selected, display only products from that supplier (JOIN with `ProductSuppliers`). When `"All"` is selected, display all products.
*   Both filters can be applied **simultaneously** (e.g., show only Computer products that are also supplied by TechCorp).

#### **3. Add New Product**
*   The user enters: `ProductName`, `Price` (numeric), `Stock` (numeric), selects a Category from ComboBox, and selects at least 1 Supplier from the CheckedListBox.
*   **Click Add Product:**
    *   Validate that all fields are filled and at least 1 Supplier is selected. Show an appropriate error message (`MessageBox`) if validation fails.
    *   Insert a new record into the `Products` table.
    *   Insert corresponding records into `ProductSuppliers` for each selected supplier.
    *   Reload the DataGrid and clear the form on success.
*   **Click Clear Form:**
    *   Clears all input controls in the Add area (all TextBoxes empty, ComboBox reset to default, all CheckedListBox items unchecked).
