# Database First Scaffolding & Configuration Snippets

These snippets provide the exact commands and code configurations required to execute Entity Framework Core Database First scaffolding and dynamically read connection strings.

---

## 1. Package Manager Console (PMC) Scaffold Command
Run this inside Package Manager Console in Visual Studio. Ensure your default project in the PMC dropdown points to the project where you want your database entities to reside (e.g. `DataAccess` or `WpfApp`).

```powershell
Scaffold-DbContext "Server=.\SQLEXPRESS;Database=DB_NAME;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force -NoPluralize
```
*Replace `DB_NAME` with the exact name of the database created by your SQL script (e.g., `DBStudyPlanner` or `CompanyDB`).*

> [!WARNING]
> **Tên Server phụ thuộc vào máy thi:**
> - Nếu dùng **SQL Server Express**: thay `(local)` bằng `.\SQLEXPRESS` hoặc `(localdb)\MSSQLLocalDB`
> - Nếu dùng **SQL Server Developer/Standard**: dùng `(local)` hoặc `.` hoặc `localhost`
> - **Cách kiểm tra nhanh:** mở SSMS, xem tên server ở cửa sổ Connect → đó là giá trị đúng.

---

## 2. dotnet CLI Scaffold Command (Terminal)
If you prefer running scaffolding from the PowerShell terminal, use this command:

```bash
dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=DB_NAME;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -f --no-pluralize
```

---

## 3. Dynamic connection string configuration (DbContext override)
After scaffolding, EF Core writes a hardcoded connection string warning in the generated DbContext's `OnConfiguring` method. If you leave this, you will receive 0 points for database configuration.

Replace the generated `OnConfiguring` method with this:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        try
        {
            // Read connection string from appsettings.json using ConfigurationHelper
            string connectionString = PRN212.ExamKit.Foundation.ConfigurationHelper.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
        }
        catch (System.Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load connection string from config: {ex.Message}");
            
            // Fallback connection string in case appsettings.json is missing in testing environment
            optionsBuilder.UseSqlServer("Server=.\SQLEXPRESS;Database=DB_NAME;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}
```

---

## 4. appsettings.json Template
Create a file named `appsettings.json` in the root of your WPF application and insert this connection block:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\SQLEXPRESS;Database=DB_NAME;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```
> [!IMPORTANT]
> Right-click on `appsettings.json` in Visual Studio Explorer, go to **Properties**, and set **Copy to Output Directory** to **Copy if newer** or **Copy always**.
