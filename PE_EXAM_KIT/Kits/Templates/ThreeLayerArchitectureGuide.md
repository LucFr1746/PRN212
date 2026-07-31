# 3-Layer Solution Configuration Blueprint

If the exam requires a **3-Layer Architecture** (e.g. Exam 5: `DataAccess`, `Business`, `WpfApp`), follow this exact blueprint to set up references, packages, and folders in less than 5 minutes.

---

## 1. Project Reference Mapping
Add these assembly references to allow communication between layers:

```text
┌────────────────────────────────────────────────────────┐
│                   WpfApp (Presentation)                │
│  - References: Business                                │
│  - References: DataAccess                              │
└───────────────────────────┬────────────────────────────┘
                            │ (Calls Services & Models)
┌───────────────────────────▼────────────────────────────┐
│                   Business (Logic)                     │
│  - References: DataAccess                              │
└───────────────────────────┬────────────────────────────┘
                            │ (Calls DAOs & EF Context)
┌───────────────────────────▼────────────────────────────┐
│                 DataAccess (Data Storage)              │
│  - References: None                                    │
└────────────────────────────────────────────────────────┘
```
**How to configure in Visual Studio:**
1. Right-click the **Business** project -> **Add** -> **Project Reference...** -> Check **DataAccess** -> OK.
2. Right-click the **WpfApp** project -> **Add** -> **Project Reference...** -> Check **Business** and **DataAccess** -> OK.

---

## 2. NuGet Package Distribution
Ensure these NuGet packages are installed in the targeted projects (usually pre-installed, but verify if compilation errors occur):

| Project | Target Packages Required | Purpose |
| :--- | :--- | :--- |
| **DataAccess** | `Microsoft.EntityFrameworkCore.SqlServer`<br>`Microsoft.EntityFrameworkCore.Tools` | DB First Scaffolding, DbContext generation. |
| **Business** | None (or `Microsoft.EntityFrameworkCore` if using IQueryables). | Domain logic, interfaces. |
| **WpfApp** | `Microsoft.Extensions.Configuration.Json`<br>`Microsoft.EntityFrameworkCore.Design` | Configurations loading, UI components running. |

---

## 3. Directory Layout Setup
Copy your pre-built Foundation and Common utilities into these folders inside the solution:

```text
Solution/
├── Project.DataAccess/
│   ├── Models/                     # Place DB First Scaffold entities here
│   ├── DAOs/                       # Place GenericDAO.cs here and subclass it
│   └── PartialEntities.cs          # Place partial classes here (CheckedListBox hack)
│
├── Project.Business/
│   ├── Repositories/               # Place Repository interfaces and implementations
│   └── Services/                   # Place Services logic here
│
└── Project.WpfApp/
    ├── Helpers/                    # Place ConfigurationHelper.cs, WpfHelpers.cs, ComboBoxExtensions.cs
    ├── Views/                      # Place LoginWindow and UserControls/Windows
    ├── appsettings.json            # Configuration settings (Copy if newer!)
    └── App.xaml
```

---

## 4. Subclassing the Generic DAO
To quickly generate specific DAOs for each entity without writing repetitive code, inherit from `GenericDAO`:

```csharp
using DataAccess.Models;
using PRN212.ExamKit.Foundation;

namespace DataAccess.DAOs
{
    public class StudentDAO : GenericDAO<Student, DBStudyPlannerContext>
    {
        private static StudentDAO _instance;
        private static readonly object _lock = new object();

        private StudentDAO() { }

        public static StudentDAO Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new StudentDAO();
                }
            }
        }
        
        // Add any custom entity-specific queries here if needed
    }
}
```
