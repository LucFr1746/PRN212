# FPT UNIVERSITY | PRACTICAL EXAM | BASIC .NET PROGRAMMING (PRN212)

## INSTRUCTIONS

*   **Duration:** 85 minutes
*   **Total Points:** 10.0 points
*   **Number of Questions:** 2
*   **Tool:** Visual Studio 2022+
*   **Framework:** .NET 8.0
*   **DB Script:** Provided in Given Materials ([script7.sql](script7.sql))

### Guidelines:
*   **Allowed:** Personal computer, notebook, textbook.
*   **NOT Allowed:** Any network communication or data sharing with other students.
*   **Use the given Solution file provided.** Do NOT create a new solution.
*   **Do NOT install additional NuGet packages.**
*   **Execute the provided .sql script** (`script7.sql`) to set up the database before starting Question 2.
*   All projects must target **.NET 8.0** and use **Visual Studio 2022+**.
*   Violating ANY of the above will invalidate your submission.
*   **On completion:** submit the entire solution folder. You may delete `[bin]` and `[obj]` folders to reduce size.

---

## QUESTION 1 (4 points)

You are required to create a **Console Application** that implements a luxury vehicle rental fleet management system. Your application must demonstrate the following .NET concepts:
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
Declare delegate `BookingStatusChangedCallback` with the following signature:
```csharp
public delegate void BookingStatusChangedCallback(string customerName, double totalAmount);
```

### 2. INTERFACE
Create interface `IBookable` with the following members:
*   `double CalculateTotalRentalPrice(int rentalDays)`: Calculates and returns the total rental fee for `rentalDays`.
*   `string GetVehicleSummary()`: Returns a formatted string summarizing vehicle details.

### 3. CLASSES

#### A. Abstract Class: `RentalVehicle` (implements `IBookable`)
Properties:
*   `int VehicleId`: `{ get; set; }`
*   `string ModelName`: `{ get; set; }`
*   `double DailyRate`: `{ get; set; }`

Constructor:
*   `RentalVehicle(int vehicleId, string modelName, double dailyRate)`

Virtual / Abstract Methods:
*   `abstract double CalculateTotalRentalPrice(int rentalDays)`: Abstract method from `IBookable`.
*   `virtual string GetVehicleSummary()`: Returns `$"[ID: {VehicleId}] {ModelName} - Daily Rate: ${DailyRate:F2}"`.

#### B. Subclass: `LuxuryCar` (inherits `RentalVehicle`)
Additional Properties:
*   `double ChauffeurDailyFee`: `{ get; set; }`
*   `bool IncludesInsurance`: `{ get; set; }`

Constructor:
*   `LuxuryCar(int vehicleId, string modelName, double dailyRate, double chauffeurDailyFee, bool includesInsurance)`: Calls base constructor.

Method Overrides:
*   `CalculateTotalRentalPrice(int rentalDays)`:
    *   Formula: `(DailyRate + ChauffeurDailyFee) * rentalDays`.
    *   If `IncludesInsurance` is true, add a flat fee of `$50.00` total to the final price.
    *   Throw `ArgumentException` if `rentalDays <= 0`.
*   `GetVehicleSummary()`: Overrides base method, appending `$" | Chauffeur Fee: ${ChauffeurDailyFee:F2}/day | Insurance: {(IncludesInsurance ? "Yes" : "No")}"`.

#### C. Manager Class: `FleetManager`
Fields:
*   `List<RentalVehicle> _vehicles`
*   `BookingStatusChangedCallback _onBookingCompleted`

Constructor:
*   `FleetManager(BookingStatusChangedCallback callback)`: Initializes `_vehicles` list and stores the callback.

Methods:
*   `void AddVehicle(RentalVehicle vehicle)`: Adds vehicle to list.
*   `void ProcessBooking(int vehicleId, string customerName, int rentalDays)`:
    *   Finds vehicle by `vehicleId`. If not found, throws `KeyNotFoundException($"Vehicle ID {vehicleId} not found.")`.
    *   Calculates total price using `CalculateTotalRentalPrice(rentalDays)`.
    *   Triggers callback `_onBookingCompleted?.Invoke(customerName, totalPrice)`.
*   `List<RentalVehicle> GetTopExpensiveVehicles(int n)`:
    *   Uses LINQ `OrderByDescending` by `DailyRate` and `Take(n)`.
    *   If `n <= 0`, throws `ArgumentOutOfRangeException`.

---

## QUESTION 2 (6 points)

You are required to create a **WPF Application** using **Entity Framework Core Database First** connected to database `PRN212_26Spr_Exam7`.

### Database Schema:
- **`RoomTypes`**: `RoomTypeId` (PK, Identity), `TypeName`, `Description`
- **`Rooms`**: `RoomId` (PK, Identity), `RoomNumber`, `Capacity`, `PricePerNight`, `IsActive`, `RoomTypeId` (FK)
- **`Amenities`**: `AmenityId` (PK, Identity), `AmenityName`
- **`RoomAmenities`**: `RoomId` (FK), `AmenityId` (FK) - N-N Direct Relationship

---

### REQUIRED WPF FEATURES:

#### 1. Data Viewing & Formatting
*   Display all rooms in a `DataGrid` when application opens.
*   Show `RoomNumber`, `Capacity`, `PricePerNight`, `IsActive`, `RoomType` (TypeName), and `Amenities` (Comma-separated string of attached amenity names).

#### 2. Filtering by RoomType
*   Populate a `ComboBox` with room types on startup (including an "All Room Types" option).
*   Selecting a room type filters the DataGrid immediately.

#### 3. Selection & Form Population
*   Clicking a room row in DataGrid populates form fields (`TextBox` for RoomNumber, Capacity, PricePerNight; `CheckBox` for IsActive; `ComboBox` for RoomType; ListBox with CheckBoxes for Amenities).

#### 4. Add / Edit / Delete Room (CRUD)
*   **Add**: Validates input -> Creates new `Room` -> Attaches selected `Amenities` -> Saves to DB -> Refreshes DataGrid.
*   **Edit**: Validates input -> Updates selected `Room` details and `Amenities` -> Saves to DB -> Refreshes DataGrid.
*   **Delete**: Asks confirmation via `MessageBox` -> Deletes room -> Refreshes DataGrid.

#### 5. Validation Rules
*   `RoomNumber` must not be empty.
*   `Capacity` must be an integer > 0.
*   `PricePerNight` must be a decimal > 0.
*   `RoomType` must be selected.
