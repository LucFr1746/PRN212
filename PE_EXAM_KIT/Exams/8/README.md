# FPT UNIVERSITY | PRACTICAL EXAM | BASIC .NET PROGRAMMING (PRN212)

## INSTRUCTIONS

*   **Duration:** 85 minutes
*   **Total Points:** 10.0 points
*   **Number of Questions:** 2
*   **Tool:** Visual Studio 2022+
*   **Framework:** .NET 8.0
*   **DB Script:** Provided in Given Materials ([script8.sql](script8.sql))

### Guidelines:
*   **Allowed:** Personal computer, notebook, textbook.
*   **NOT Allowed:** Any network communication or data sharing with other students.
*   **Use the given Solution file provided.** Do NOT create a new solution.
*   **Do NOT install additional NuGet packages.**
*   **Execute the provided .sql script** (`script8.sql`) to set up the database before starting Question 2.
*   All projects must target **.NET 8.0** and use **Visual Studio 2022+**.
*   Violating ANY of the above will invalidate your submission.
*   **On completion:** submit the entire solution folder. You may delete `[bin]` and `[obj]` folders to reduce size.

---

## QUESTION 1 (4 points)

You are required to create a **Console Application** that implements a Smart Home Device Management System. Your application must demonstrate the following .NET concepts:
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
Declare delegate `DeviceAlertCallback` with the following signature:
```csharp
public delegate void DeviceAlertCallback(string deviceName, string alertMessage);
```

### 2. INTERFACE
Create interface `IPowerConsumable` with the following members:
*   `double CalculateMonthlyPowerCost(double kwhRate)`: Calculates estimated monthly electricity cost based on power consumption rate.
*   `string GetDeviceSummary()`: Returns a formatted summary of device specifications.

### 3. CLASSES

#### A. Abstract Class: `SmartDevice` (implements `IPowerConsumable`)
Properties:
*   `int DeviceId`: `{ get; set; }`
*   `string DeviceName`: `{ get; set; }`
*   `double PowerWatts`: `{ get; set; }`

Constructor:
*   `SmartDevice(int deviceId, string deviceName, double powerWatts)`

Virtual / Abstract Methods:
*   `abstract double CalculateMonthlyPowerCost(double kwhRate)`: Abstract method from `IPowerConsumable`.
*   `virtual string GetDeviceSummary()`: Returns `$"[ID: {DeviceId}] {DeviceName} - Power: {PowerWatts:F1}W"`.

#### B. Subclass: `SmartAirConditioner` (inherits `SmartDevice`)
Additional Properties:
*   `int CoolingBTU`: `{ get; set; }`
*   `bool HasInverter`: `{ get; set; }`

Constructor:
*   `SmartAirConditioner(int deviceId, string deviceName, double powerWatts, int coolingBTU, bool hasInverter)`: Calls base constructor.

Method Overrides:
*   `CalculateMonthlyPowerCost(double kwhRate)`:
    *   Estimated running hours: 8 hours/day * 30 days = 240 hours.
    *   Base kWh: `(PowerWatts * 240) / 1000.0`.
    *   If `HasInverter` is true, reduce total kWh by 30% (multiply kWh by 0.70).
    *   Formula: `Final_kWh * kwhRate`.
    *   Throw `ArgumentException` if `kwhRate <= 0`.
*   `GetDeviceSummary()`: Overrides base method, appending `$" | Cooling: {CoolingBTU} BTU | Inverter: {(HasInverter ? "Yes" : "No")}"`.

#### C. Manager Class: `SmartHomeManager`
Fields:
*   `List<SmartDevice> _devices`
*   `DeviceAlertCallback _onDeviceAlert`

Constructor:
*   `SmartHomeManager(DeviceAlertCallback callback)`: Initializes `_devices` list and stores callback.

Methods:
*   `void AddDevice(SmartDevice device)`: Adds device to list.
*   `void TriggerOverheatAlert(int deviceId, string reason)`:
    *   Finds device by `deviceId`. If not found, throws `KeyNotFoundException($"Device ID {deviceId} not found.")`.
    *   Triggers callback `_onDeviceAlert?.Invoke(device.DeviceName, reason)`.
*   `List<SmartDevice> GetHighPowerDevices(int topN)`:
    *   Uses LINQ `OrderByDescending` by `PowerWatts` and `Take(topN)`.
    *   If `topN <= 0`, throws `ArgumentOutOfRangeException`.

---

## QUESTION 2 (6 points)

You are required to create a **WPF Application** using **Entity Framework Core Database First** connected to database `PRN212_26Spr_Exam8`.

### Database Schema:
- **`Categories`**: `CategoryId` (PK, Identity), `CategoryName`, `Description`
- **`Courses`**: `CourseId` (PK, Identity), `CourseCode`, `Title`, `DurationHours`, `TuitionFee`, `IsActive`, `CategoryId` (FK)
- **`Skills`**: `SkillId` (PK, Identity), `SkillName`
- **`CourseSkills`**: `CourseId` (FK), `SkillId` (FK) - N-N Direct Relationship

---

### REQUIRED WPF FEATURES:

#### 1. Data Viewing & Formatting
*   Display all courses in a `DataGrid` when application opens.
*   Show `CourseCode`, `Title`, `DurationHours`, `TuitionFee`, `IsActive`, `Category` (CategoryName), and `Skills` (Comma-separated string of attached skill names).

#### 2. Filtering by Category
*   Populate a `ComboBox` with categories on startup (including an "All Categories" option).
*   Selecting a category filters the DataGrid immediately.

#### 3. Selection & Form Population
*   Clicking a course row in DataGrid populates form fields (`TextBox` for CourseCode, Title, DurationHours, TuitionFee; `CheckBox` for IsActive; `ComboBox` for Category; ListBox with CheckBoxes for Skills).

#### 4. Add / Edit / Delete Course (CRUD)
*   **Add**: Validates input -> Creates new `Course` -> Attaches selected `Skills` -> Saves to DB -> Refreshes DataGrid.
*   **Edit**: Validates input -> Updates selected `Course` details and `Skills` -> Saves to DB -> Refreshes DataGrid.
*   **Delete**: Asks confirmation via `MessageBox` -> Deletes course -> Refreshes DataGrid.

#### 5. Validation Rules
*   `CourseCode` and `Title` must not be empty.
*   `DurationHours` must be an integer > 0.
*   `TuitionFee` must be a decimal > 0.
*   `Category` must be selected.
