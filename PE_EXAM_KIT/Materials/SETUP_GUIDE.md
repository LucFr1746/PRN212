# HƯỚNG DẪN SETUP TRƯỚC KHI THI (Offline Preparation)

> **Mục đích:** Tải hết thư viện NuGet, build sẵn solution để khi vào phòng thi (không có mạng) chỉ cần mở và code.

> [!IMPORTANT]
> **Tên SQL Server Instance:** Hướng dẫn này dùng `.\SQLEXPRESS` (SQL Server Express). Nếu máy thi dùng instance khác, mở **SSMS** → xem tên server ở cửa sổ **Connect to Server** → thay vào tất cả chỗ có `.\SQLEXPRESS`.

---

## Tình trạng Solution từ nhà trường

File `PE_PRN212_GivenSolution - Testing` chứa 2 project:

| Project | Loại | Target | NuGet đã có |
|:--------|:-----|:-------|:------------|
| **Q1** | Console App (.NET 8.0) | `net8.0` | Không cần NuGet |
| **Q2** | WPF App (.NET 8.0) | `net8.0-windows` | `EntityFrameworkCore.Design` 8.0.18, `EntityFrameworkCore.SqlServer` 8.0.18, `Extensions.Configuration.Json` 9.0.7 |

### ⚠️ Vấn đề: Thiếu `Microsoft.EntityFrameworkCore.Tools`

Solution gốc từ trường **thiếu** package `Microsoft.EntityFrameworkCore.Tools`. Package này **bắt buộc** để chạy lệnh `Scaffold-DbContext` trong Package Manager Console (PMC) khi thi.

> [!CAUTION]
> Nếu không có package này, khi vào thi bạn sẽ **KHÔNG** scaffold được database → mất thời gian tìm cách sửa mà lại không có mạng để cài.

---

## Hướng dẫn Setup (Làm tại nhà, CÓ MẠNG)

### Bước 1 — Mở Solution trong Visual Studio 2022

1. Mở file `PE_PRN212_GivenSolution.sln` bằng **Visual Studio 2022**.
2. Đợi VS tải xong solution.

### Bước 2 — Thêm package EF Core Tools (thiếu từ trường)

1. Mở **Tools** → **NuGet Package Manager** → **Package Manager Console**.
2. Trong dropdown **Default project**, chọn **Q2**.
3. Chạy lệnh:

```powershell
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 8.0.18
```

4. Đợi cài xong → kiểm tra file `Q2.csproj` phải có dòng:

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.18">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

> [!TIP]
> Hoặc chuột phải **Q2** → **Manage NuGet Packages** → tìm `Microsoft.EntityFrameworkCore.Tools` → Install version **8.0.18**.

### Bước 3 — Restore toàn bộ NuGet Packages

Chuột phải **Solution** trong Solution Explorer → **Restore NuGet Packages**.

Hoặc mở **Terminal** (View → Terminal) và chạy:

```powershell
dotnet restore
```

Đợi cho tới khi output hiển thị: `Restore completed` hoặc `Build succeeded`.

### Bước 4 — Build Solution

Nhấn **Ctrl+Shift+B** (Build Solution).

Kết quả mong đợi:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

> [!WARNING]
> Nếu build **thất bại**, kiểm tra:
> - Target framework = `.NET 8.0` (có cài .NET 8.0 SDK chưa?)
> - NuGet đã restore hết chưa?

### Bước 5 — Test chạy thử

1. Chuột phải **Q1** → **Set as Startup Project** → **Ctrl+F5** → phải thấy in `Hello, World!`.
2. Chuột phải **Q2** → **Set as Startup Project** → **Ctrl+F5** → phải thấy cửa sổ WPF trắng mở lên.

### Bước 6 — Test lệnh Scaffold hoạt động (QUAN TRỌNG)

Bước này đảm bảo lệnh scaffold hoạt động trước khi vào thi. Tạo 1 database test nhanh:

1. Mở **SSMS** → chạy:
```sql
CREATE DATABASE TestScaffoldDB;
GO
USE TestScaffoldDB;
GO
CREATE TABLE TestTable (Id INT PRIMARY KEY IDENTITY, Name NVARCHAR(50));
GO
```

2. Quay lại VS → **Package Manager Console** → chọn project **Q2** → chạy:
```powershell
Scaffold-DbContext "Server=.\SQLEXPRESS;Database=TestScaffoldDB;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir TestModels -Force -NoPluralize
```

3. Nếu thấy thư mục `TestModels/` xuất hiện trong project Q2 với file `TestTable.cs` + Context → **Scaffold hoạt động**.

4. **Test sửa DbContext (quan trọng):** Mở file `TestModels/TestScaffoldDbContext.cs` (hoặc tên tương tự) → tìm hàm `OnConfiguring` → thay nội dung bằng:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=TestScaffoldDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}
```

> [!TIP]
> Bước này chỉ để test scaffold hoạt động. Khi thi thật, bạn sẽ dùng `ConfigurationHelper` + `appsettings.json` thay vì hardcode connection string (xem SOLUTION_GUIDE).

5. **Build lại** solution (Ctrl+Shift+B) — phải **0 errors**.

6. **Dọn dẹp:** Xóa thư mục `TestModels/` trong project Q2 và drop database test trong SSMS:
```sql
DROP DATABASE TestScaffoldDB;
```

### Bước 7 — Đảm bảo NuGet cache offline

Mở **Terminal** trong VS (**View → Terminal**) hoặc **PowerShell** bất kỳ, chạy 4 lệnh sau:

```powershell
ls "$env:USERPROFILE\.nuget\packages\microsoft.entityframeworkcore.sqlserver\8.0.18"
ls "$env:USERPROFILE\.nuget\packages\microsoft.entityframeworkcore.tools\8.0.18"
ls "$env:USERPROFILE\.nuget\packages\microsoft.entityframeworkcore.design\8.0.18"
ls "$env:USERPROFILE\.nuget\packages\microsoft.extensions.configuration.json\9.0.7"
```

**Kết quả mong đợi:** Mỗi lệnh đều hiển thị danh sách file/thư mục (không báo lỗi "not found").

Nếu cả 4 đều có → packages đã được cache cục bộ trong thư mục `C:\Users\<TênBạn>\.nuget\packages\`. Khi thi offline, `dotnet restore` sẽ sử dụng cache này mà không cần internet.

---

## Checklist trước khi vào phòng thi

| # | Kiểm tra | ✓ |
|:--|:---------|:--|
| 1 | Visual Studio 2022 đã cài, target .NET 8.0 | ☐ |
| 2 | SQL Server + SSMS đã cài và chạy được | ☐ |
| 3 | Solution `PE_PRN212_GivenSolution` build thành công (0 errors) | ☐ |
| 4 | Q1 chạy được (in Hello World) | ☐ |
| 5 | Q2 chạy được (hiển thị cửa sổ WPF) | ☐ |
| 6 | `Scaffold-DbContext` hoạt động trong PMC | ☐ |
| 7 | 4 NuGet packages đã cache trong `~/.nuget/packages/` | ☐ |
| 8 | Copy thư mục **Kits** vào USB/máy thi (để copy-paste templates) | ☐ |

---

## Danh sách NuGet Packages đầy đủ cho Q2

| Package | Version | Mục đích |
|:--------|:--------|:---------|
| `Microsoft.EntityFrameworkCore.SqlServer` | 8.0.18 | Kết nối SQL Server |
| `Microsoft.EntityFrameworkCore.Tools` | 8.0.18 | Chạy `Scaffold-DbContext` trong PMC |
| `Microsoft.EntityFrameworkCore.Design` | 8.0.18 | Design-time EF Core (scaffold code gen) |
| `Microsoft.Extensions.Configuration.Json` | 9.0.7 | Đọc `appsettings.json` |

> [!IMPORTANT]
> **Package `Tools` là package mà solution gốc từ trường THIẾU.** Nếu bạn dùng máy mới hoặc copy solution từ nguồn khác, nhớ kiểm tra lại và cài nếu thiếu.
