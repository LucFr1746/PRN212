# FPT UNIVERSITY | PRACTICAL EXAM | BASIC .NET PROGRAMMING

## INSTRUCTIONS

*   **Duration:** 85 minutes
*   **Total Points:** 10.0 points
*   **Number of Questions:** 2
*   **Tool:** Visual Studio 2022+
*   **Framework:** .NET 8.0
*   **DB Script:** Provided in Given Materials ([script2.sql](script2.sql))

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
*   `void ApplyDiscount(double percent)` – reduces `Price` by the given percentage

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
