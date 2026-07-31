# FPT UNIVERSITY | PRACTICAL EXAM | BASIC .NET PROGRAMMING

## INSTRUCTIONS

*   **Duration:** 85 minutes
*   **Total Points:** 10.0 points
*   **Number of Questions:** 2
*   **Tool:** Visual Studio 2022+
*   **Framework:** .NET 8.0
*   **DB Script:** Provided in Given Materials ([script4.sql](script4.sql))

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

You are required to create a **Console Application** that implements a product discount management system. Your application must demonstrate the following .NET concepts:
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
Declare delegate `DiscountAppliedCallback` with the following signature:
```csharp
public delegate void DiscountAppliedCallback(string productName, double discountPercent);
```

### 2. INTERFACE
Create interface `IDiscountable` with the following members:
*   `double Price { get; set; }` – the current price of the product
*   `void ApplyDiscount(double percent)` – reduces Price by the given percentage

### 3. CLASSES

#### **Abstract Class `Product`**
*   **Properties:**
    *   `int ProductId { get; set; }`
    *   `string Name { get; set; }`
    *   `double Price { get; set; }`
*   **Constructor:**
    *   `Product(int productId, string name, double price)` – initializes properties. Validates `price > 0`; throws `ArgumentOutOfRangeException` with message `"Price must be greater than 0"` if invalid.
*   **Methods:**
    *   `void ApplyDiscount(double percent)` – validates `0 < percent <= 100`; throws `ArgumentOutOfRangeException` with message `"Discount percent must be between 0 and 100"` if invalid; otherwise: `Price = Price * (1 - percent / 100)`.
    *   `abstract string GetCategory()` – abstract method to be implemented by subclasses.

#### **Class `ElectronicsProduct` (extends `Product`, implements `IDiscountable`)**
*   **Properties:**
    *   `string Brand { get; set; }`
    *   `int WarrantyMonths { get; set; }`
*   **Constructor:**
    *   `ElectronicsProduct(int productId, string name, double price, string brand, int warrantyMonths)` – calls base constructor and sets `Brand`, `WarrantyMonths`.
*   **Implementation:**
    *   `override string GetCategory()` – returns `"Electronics"`.
    
    > [!NOTE]
    > Because `Product` already declares `double Price { get; set; }` with the same signature as `IDiscountable.Price`, and `ApplyDiscount()` is implemented in `Product`, the class inherits both implementations automatically and fully satisfies `IDiscountable`.

#### **Class `DiscountManager`**
*   **Private fields:**
    *   `_products` of type `List<ElectronicsProduct>`
    *   `_onDiscountApplied` of type `DiscountAppliedCallback`
*   **Constructor:**
    *   `DiscountManager(DiscountAppliedCallback callback)` – initializes empty list and stores the callback.
*   **Methods:**
    *   `void AddProduct(ElectronicsProduct product)` – adds the product to the collection.
    *   `void ApplyDiscountToProduct(int productId, double percent)`:
        *   Finds the product with matching `ProductId`.
        *   If not found, throws `InvalidOperationException` with message `"Product not found: {productId}"`.
        *   If found, calls `product.ApplyDiscount(percent)`, then invokes `_onDiscountApplied` with `(product.Name, percent)`.
    *   `List<ElectronicsProduct> GetProductsUnder(double maxPrice)` – uses LINQ to return products where `Price < maxPrice`, ordered by `Price` ascending.

### 4. TESTING (Main Method)
Write your own `Main` method that demonstrates all functionality:
1.  Create at least 3 `ElectronicsProduct` objects with different brands.
2.  Create a `DiscountManager` with a callback that prints to console.
3.  Add all products to the manager.
4.  Apply discounts (include a case that triggers `ArgumentOutOfRangeException` and handle it with `try-catch`).
5.  Call `GetProductsUnder(500.0)` and print results.

#### **Example of expected output (for reference only):**
```text
[DISCOUNT]: Laptop Pro received 10.0% discount
[DISCOUNT]: Wireless Headphones received 15.0% discount
[DISCOUNT]: Smart Watch received 20.0% discount
Error: Discount percent must be between 0 and 100

=== Products Under 500.0 ===
1. Wireless Headphones | Brand: Sony   | Price: 127.50 | Category: Electronics
2. Smart Watch         | Brand: Garmin | Price: 239.99 | Category: Electronics
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
|   +------------+---------------------+-------------------+---------------+------------+--------+ |
|   | ID         | FullName            | Email             | Salary        | HireDate   | Depart | |
|   +------------+---------------------+-------------------+---------------+------------+--------+ |
|   | 1          | Nguyen Van A        | a@fpt.edu.vn      | 15,000,000.00 | 2023-01-15 | Engine | |
|   | 2          | Tran Thi B          | b@fpt.edu.vn      | 12,000,000.00 | 2022-08-01 | Market | |
|   | ...        | ...                 | ...               | ...           | ...        | ...    | |
|   +------------+---------------------+-------------------+---------------+------------+--------+ |
|                                                                                                 |
| ADD NEW EMPLOYEE                                                                                |
|   Full Name: [                        ]  Email: [                 ]  Salary: [                ] |
|   Department: [ Customer Support      v ]                                                       |
|   Skills:                                                                                       |
|     +----------------------------+                                                              |
|     | [ ] Azure                  |#|                                                              |
|     | [ ] C#                     | |                                                              |
|     | [ ] Communication          | |                                                              |
|     | [ ] Excel                  | |                                                              |
|     | [ ] Project Management     | |                                                              |
|     +----------------------------+                                                              |
|                                                                        [Add Employee] [Clear Form]|
+-------------------------------------------------------------------------------------------------+
```

---

### DETAILED REQUIREMENTS

#### **1. Load and Display Data**
*   Load the Employee list from the `Employees` table into the DataGrid, displaying columns: `EmployeeId`, `FullName`, `Email`, `Salary`, `HireDate`, and `DepartmentName` (joined from `Departments`).
*   Load the `Department` list into the Filter ComboBox (`Department`) and into the Add New Employee ComboBox (`Department`). Both must include `"All"` as the first item in the Filter ComboBox only.
*   Load the `Skill` list into the Filter ComboBox (`Skill`) and into the CheckedListBox in the Add area.
*   **Both Filter ComboBoxes (Department and Skill) MUST include an "All" option as the first item.**

#### **2. Filter Employees**
*   **Filter by Department:** When a specific department is selected, display only employees in that department. When `"All"` is selected, display all employees.
*   **Filter by Skill:** When a specific skill is selected, display only employees who have that skill (JOIN with `EmployeeSkills`). When `"All"` is selected, display all employees.
*   Both filters can be applied **simultaneously** (e.g., show only Engineering employees who also have C# skill).

#### **3. Add New Employee**
*   The user enters: `FullName`, `Email`, `Salary` (numeric), selects a Department from ComboBox, and selects at least 1 Skill from the CheckedListBox. `HireDate` is NOT required in the form, set it to `DateTime.Today` when inserting into the database.
*   **Click Add Employee:**
    *   Validate that all fields are filled and at least 1 Skill is selected. Show an appropriate error message (`MessageBox`) if validation fails.
    *   Insert a new record into the `Employees` table.
    *   Insert corresponding records into `EmployeeSkills` for each selected skill.
    *   Reload the DataGrid and clear the form on success.
*   **Click Clear Form:**
    *   Clears all input controls in the Add area (all TextBoxes empty, ComboBox reset to default, all CheckedListBox items unchecked).
