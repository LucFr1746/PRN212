# HƯỚNG DẪN SETUP & QUY TRÌNH LÀM BÀI THI PRN212

> **Mục đích:** Tải hết thư viện NuGet, build sẵn solution, và có quy trình chuẩn để vào thi làm bài nhanh nhất.

> [!IMPORTANT]
> **Tên SQL Server Instance:** Hướng dẫn này dùng `.\SQLEXPRESS` (SQL Server Express). Nếu máy thi dùng instance khác, mở **SSMS** → xem tên server ở cửa sổ **Connect to Server** → thay vào tất cả chỗ có `.\SQLEXPRESS`.

---

# PHẦN 1: SETUP TRƯỚC KHI THI (Làm ở nhà, CÓ MẠNG)

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

1. Mở **SSMS** (kết nối tới `.\SQLEXPRESS`) → chạy:
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

4. **Test sửa DbContext:** Mở file `TestModels/TestScaffoldDbContext.cs` → tìm hàm `OnConfiguring` → thay nội dung bằng:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=TestScaffoldDB;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}
```

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

---
---

# PHẦN 2: QUY TRÌNH LÀM BÀI TRONG PHÒNG THI (Không có mạng)

> **Tổng thời gian:** 85 phút → Phân bổ: Setup 10p → Q1 Console 20p → Q2 WPF 45p → Kiểm tra 10p

## Bước A — Nhận đề & Setup nền (10 phút)

### A1. Đọc đề, xác định dạng

| Nếu đề có... | Dạng | Kit cần dùng |
|:-------------|:-----|:-------------|
| Q1: Delegate + Interface + Abstract class + Manager | **Dạng 1-4** | `ConsoleOOPTemplate.cs` |
| Q1: Generic class `DataVault<T>` | **Dạng 5** | `ConsoleOOPTemplate.cs` (phần 6) |
| Q2: Filter ComboBox + Add form + CheckedListBox | **Dạng 1-4** | `CrudWindowTemplate.xaml/.cs` |
| Q2: Login + Navigation shell + Inline grid CRUD | **Dạng 5** | `LoginWindowTemplate` + `InlineGridCrudTemplate` |

### A2. Chạy SQL Script

1. Mở **SSMS** → kết nối tới `.\SQLEXPRESS`.
2. Mở file `.sql` đề thi → nhấn **F5** thực thi.
3. Xác nhận database và bảng đã tạo thành công.

### A3. Scaffold Database vào Project Q2

1. Mở **Package Manager Console** (Tools → NuGet Package Manager → PMC).
2. Dropdown **Default project** → chọn **Q2**.
3. Chạy lệnh (thay `TÊN_DATABASE` bằng tên DB thực tế):

```powershell
Scaffold-DbContext "Server=.\SQLEXPRESS;Database=TÊN_DATABASE;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force -NoPluralize
```

4. Kiểm tra thư mục `Models/` xuất hiện với đầy đủ file entity + DbContext.

### A4. Tạo appsettings.json

1. Chuột phải **Q2** → **Add** → **New Item** → **JSON File** → tên `appsettings.json`.
2. Paste (thay `TÊN_DATABASE`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\SQLEXPRESS;Database=TÊN_DATABASE;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. **Chuột phải `appsettings.json`** → **Properties** → **Copy to Output Directory** = **Copy if newer**.

> [!CAUTION]
> Quên bước 3 = **mất toàn bộ điểm Q2** vì chương trình không đọc được connection string.

### A5. Copy 4 file Helper từ Kit

| File nguồn trong Kit | Copy vào Project Q2 |
|:---------------------|:--------------------|
| `Kits/Foundation/ConfigurationHelper.cs` | `Q2/Helpers/ConfigurationHelper.cs` |
| `Kits/Common/ComboBoxExtensions.cs` | `Q2/Helpers/ComboBoxExtensions.cs` |
| `Kits/Common/ValidationHelpers.cs` | `Q2/Helpers/ValidationHelpers.cs` |
| `Kits/Common/WpfHelpers.cs` | `Q2/Helpers/WpfHelpers.cs` |

Sau khi copy, **Ctrl+H** trong mỗi file đổi namespace:
- `PRN212.ExamKit.Foundation` → `Q2.Helpers`
- `PRN212.ExamKit.Common` → `Q2.Helpers`

### A6. Sửa DbContext — đọc connection string từ appsettings.json

Mở file `Models/...Context.cs` → tìm hàm `OnConfiguring` → **xóa hết** nội dung → thay bằng:

```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        optionsBuilder.UseSqlServer(Q2.Helpers.ConfigurationHelper.GetConnectionString("DefaultConnection"));
    }
}
```

> [!WARNING]
> Xóa dòng `#warning To protect potentially sensitive information...` nếu có.

### A7. Tạo Partial Class cho entity nhiều-nhiều (CheckedListBox)

Nhìn đề xem entity nào dùng CheckedListBox (bảng phụ many-to-many, ví dụ: `Skill`, `Genre`, `Supplier`...).

Tạo file `Models/PartialEntities.cs`:

```csharp
using System.ComponentModel.DataAnnotations.Schema;

namespace Q2.Models  // Phải trùng namespace với file entity được scaffold
{
    public partial class TÊN_ENTITY_PHỤ  // Ví dụ: Skill, Genre, Supplier...
    {
        [NotMapped]
        public bool IsChecked { get; set; }
    }
}
```

### A8. Build kiểm tra

Nhấn **Ctrl+Shift+B** → phải **0 errors** trước khi bắt đầu code.

---

## Bước B — Câu 1: Console App (20 phút)

### B1. Xác định dạng Q1 và copy template

Mở `Kits/Snippets/ConsoleOOPTemplate.cs` → xác định phần nào cần dùng:

| Đề yêu cầu | Copy phần nào |
|:-----------|:--------------|
| Student + Score + Manager | Phần 3A + 4A + 5A + 7A |
| Product + Discount + Manager | Phần 3B + 4B + 5B + 7B |
| DataVault\<T\> generic | Phần 6 |

### B2. Ctrl+H đổi tên

Dựa vào đề thi, lập bảng đổi tên. **Luôn đổi tên concrete class TRƯỚC abstract class.**

Ví dụ nếu đề dùng dạng Student → Course:

| Tìm | Thay | Thứ tự |
|:----|:-----|:-------|
| `UndergraduateStudent` | `TênConcreteClass` | **1 (trước)** |
| `Student` | `TênAbstractClass` | **2 (sau)** |
| `ScoreManager` | `TênManager` | 3 |
| `StudentId` → `FullName` → `Scores`... | Tên property theo đề | 4+ |

### B3. Sửa logic khác biệt

- **Khoảng validation** (ví dụ `[0, 10]` → `[1, 5]`)
- **Bảng xếp hạng** (ví dụ `GetRank()` → `GetGrade()` với ngưỡng khác)
- **Thông báo lỗi** (thay message cho khớp đề)

### B4. Viết Main, build & test

Uncomment block Main tương ứng → sửa dữ liệu test → **Ctrl+F5** chạy thử.

---

## Bước C — Câu 2: WPF App (45 phút)

### C1. Copy XAML template → MainWindow.xaml

1. Mở `Kits/Templates/CrudWindowTemplate.xaml` → **copy toàn bộ** → paste vào `MainWindow.xaml`.
2. **Sửa dòng đầu:**
```xml
<Window x:Class="Q2.MainWindow"
        ...
        Title="Tên cửa sổ theo đề" Height="650" Width="850" ...>
```

### C2. Sửa Zone 1 (Filter) — đổi tên ComboBox

```xml
<!-- Đổi label + tên control cho đúng đề -->
<TextBlock Text="TênDanhMục:" .../>         <!-- Ví dụ: Author, Department -->
<ComboBox x:Name="cboFilterDanhMuc" .../>

<TextBlock Text="TênPhụ:" .../>              <!-- Ví dụ: Genre, Skill -->
<ComboBox x:Name="cboFilterPhu" .../>
```

### C3. Sửa Zone 2 (DataGrid) — đổi cột binding

```xml
<DataGrid x:Name="dgTênEntity" AutoGenerateColumns="False" IsReadOnly="True" ...>
    <DataGrid.Columns>
        <DataGridTextColumn Header="ID" Binding="{Binding TênId}" Width="60"/>
        <DataGridTextColumn Header="Tên" Binding="{Binding TênCột}" Width="*"/>
        <DataGridTextColumn Header="Số" Binding="{Binding CộtSố, StringFormat={}{0:N2}}" Width="120"/>
        <DataGridTextColumn Header="Danh mục" Binding="{Binding NavigationProperty.TênHiểnThị}" Width="150"/>
    </DataGrid.Columns>
</DataGrid>
```

> [!WARNING]
> **XÓA** thuộc tính `SelectionChanged="DgRecords_SelectionChanged"` nếu đề không yêu cầu click-to-edit. Nếu để lại sẽ lỗi compile.

### C4. Sửa Zone 3 (Form nhập) — đổi tên TextBox + ComboBox + ListBox

- Đổi `txtName` → `txtTênTrường` (theo đề)
- Đổi `txtNumericValue` → `txtTênSố`
- Thêm TextBox nếu đề cần nhiều hơn 2 trường (copy 1 block Grid + đổi tên)
- Đổi `cboCategory` → `cboTênDanhMục`
- Đổi `lstCheckedItems` → `lstTênEntityPhụ`
- Sửa `Content="{Binding TênHiểnThị}"` trong CheckBox template

### C5. Code-behind — Copy template + bỏ comment + đổi tên

1. Mở `Kits/Templates/CrudWindowTemplate.xaml.cs` → copy toàn bộ vào `MainWindow.xaml.cs`.
2. **Sửa namespace:**
```csharp
// Đổi:
namespace PRN212.ExamKit.Templates → namespace Q2
// Đổi class:
public partial class CrudWindow → public partial class MainWindow
// Đổi using:
using PRN212.ExamKit.Common → using Q2.Helpers
// Thêm:
using Q2.Models;
using Microsoft.EntityFrameworkCore;
```

3. **Bỏ comment từng block và đổi tên entity.** Quy trình cho mỗi hàm:

#### `LoadInitData()` — Bỏ comment + đổi tên:

```csharp
private void LoadInitData()
{
    try
    {
        using (var context = new TÊN_CONTEXT())
        {
            var danhMucs = context.TênBảngDanhMục.ToList();        // Ví dụ: Authors
            var entityPhus = context.TênBảngPhụ.ToList();          // Ví dụ: Genres

            // Filter ComboBox có "All"
            cboFilterDanhMuc.LoadWithDefault(danhMucs, "TênHiểnThị", "TênId",
                () => new TênEntity { TênId = 0, TênHiểnThị = "All" });
            cboFilterPhu.LoadWithDefault(entityPhus, "TênHiểnThị", "TênId",
                () => new TênEntity { TênId = 0, TênHiểnThị = "All" });

            // Form ComboBox (không có "All")
            cboTênDanhMục.ItemsSource = danhMucs;
            cboTênDanhMục.DisplayMemberPath = "TênHiểnThị";
            cboTênDanhMục.SelectedValuePath = "TênId";
            cboTênDanhMục.SelectedIndex = 0;

            // CheckedListBox
            lstTênEntityPhụ.ItemsSource = entityPhus;
        }
        RefreshGrid();
    }
    catch (Exception ex) { WpfHelpers.ShowError($"Failed to load: {ex.Message}"); }
}
```

#### `RefreshGrid()`:

```csharp
private void RefreshGrid()
{
    using (var context = new TÊN_CONTEXT())
    {
        dgTênEntity.ItemsSource = context.TênBảngChính
            .Include(x => x.NavigationDanhMục)   // Ví dụ: .Include(b => b.Author)
            .ToList();
    }
}
```

#### `FilterButton_Click()`:

```csharp
private void FilterButton_Click(object sender, RoutedEventArgs e)
{
    try
    {
        int danhMucId = Convert.ToInt32(cboFilterDanhMuc.SelectedValue);
        int phuId = Convert.ToInt32(cboFilterPhu.SelectedValue);

        using (var context = new TÊN_CONTEXT())
        {
            IQueryable<TênEntityChính> query = context.TênBảngChính
                .Include(x => x.NavigationDanhMục)
                .Include(x => x.TênBảngBridge);

            if (danhMucId > 0)
                query = query.Where(x => x.DanhMucId == danhMucId);

            if (phuId > 0)
                query = query.Where(x => x.TênBảngBridge.Any(b => b.PhuId == phuId));

            dgTênEntity.ItemsSource = query.ToList();
        }
    }
    catch (Exception ex) { WpfHelpers.ShowError($"Filter error: {ex.Message}"); }
}
```

#### `SaveButton_Click()`:

```csharp
private void SaveButton_Click(object sender, RoutedEventArgs e)
{
    // 1. Đọc input
    var ten = txtTênTrường.Text.Trim();
    var soText = txtTênSố.Text.Trim();
    var danhMucId = cboTênDanhMục.SelectedValue;

    // 2. Validate
    if (!ValidationHelpers.IsRequired(ten)) { WpfHelpers.ShowError("Tên is required."); return; }
    if (!ValidationHelpers.TryParseDecimal(soText, out decimal soValue) || soValue <= 0)
    { WpfHelpers.ShowError("Invalid number."); return; }
    if (danhMucId == null) { WpfHelpers.ShowError("Select a category."); return; }

    // 3. Validate CheckedListBox
    var allPhu = lstTênEntityPhụ.ItemsSource as List<TênEntityPhụ>;
    var selectedPhu = allPhu?.Where(x => x.IsChecked).ToList() ?? new List<TênEntityPhụ>();
    if (selectedPhu.Count == 0) { WpfHelpers.ShowError("Select at least one item."); return; }

    try
    {
        using (var context = new TÊN_CONTEXT())
        {
            // 4. Tạo entity chính
            var entity = new TênEntityChính
            {
                TênCột = ten,
                CộtSố = soValue,
                DanhMucId = Convert.ToInt32(danhMucId)
            };
            context.TênBảngChính.Add(entity);
            context.SaveChanges();  // Sinh ID tự động

            // 5. Tạo bản ghi bridge (many-to-many)
            foreach (var phu in selectedPhu)
            {
                context.TênBảngBridge.Add(new TênBridge
                {
                    EntityChínhId = entity.Id,
                    PhuId = phu.PhuId
                });
            }
            context.SaveChanges();
        }

        WpfHelpers.ShowInfo("Added successfully!");
        RefreshGrid();
        ResetForm();
    }
    catch (Exception ex) { WpfHelpers.ShowError($"Error: {ex.Message}"); }
}
```

#### `ResetForm()`:

```csharp
private void ResetForm()
{
    txtTênTrường.Text = string.Empty;
    txtTênSố.Text = string.Empty;
    cboTênDanhMục.SelectedIndex = 0;

    var allPhu = lstTênEntityPhụ.ItemsSource as List<TênEntityPhụ>;
    if (allPhu != null) foreach (var x in allPhu) x.IsChecked = false;
    lstTênEntityPhụ.Items.Refresh();
}
```

### C6. Build & Test

Nhấn **Ctrl+Shift+B** → sửa lỗi nếu có → **Ctrl+F5** chạy thử → test từng chức năng.

---

## Bước D — Trước khi nộp bài (5 phút)

1. **Xóa package `Tools`** khỏi `Q2.csproj` (không cần cho runtime, tránh bị trừ điểm cài thêm package).
2. **Build lại** (Ctrl+Shift+B) → phải 0 errors.
3. Chuột phải **Solution** → **Clean Solution** → xóa `bin/obj` giảm dung lượng.
4. Nộp bài.

---

## Bảng Tra Cứu & Quy Tắc Áp Dụng Cho Mọi Đề Thi (Universal Mapping Guide)

### 1. Mô Hình 3 Bảng Chuẩn Của Mọi Đề PRN212 WPF

Mọi đề thi WPF PRN212 đều xoay quanh **3 Bảng chính** trong CSDL:

```text
[Bảng Danh Mục (1-N)]  ───< 1-N >───  [Bảng Chính (Main)]  ───< N-N >───  [Bảng Phụ (Supplier/Skill)]
(Categories / Departments / Authors)   (Products / Employees / Books)    (Suppliers / Skills / Genres)
```

> [!TIP]
> **Quy Tắc Nhận Diện Giao Diện Nhanh:**
> - **Bảng Chính (Main):** Bảng xuất hiện ở DataGrid (`Products`, `Books`, `Employees`...).
> - **Bảng Danh Mục (1-N):** Danh mục đổ vào ComboBox Dropdown (`Categories`, `Authors`, `Departments`...).
> - **Bảng Phụ (N-N):** Danh sách tích chọn CheckBox (`Suppliers`, `Genres`, `Skills`...).

1. **Bảng Chính (Main Entity):** Đối tượng chính cần quản lý (Product, Employee, Book, Car...) ➔ Hiển thị trên DataGrid & Form nhập.
2. **Bảng Danh Mục (1-N Lookup Entity):** Danh mục liên kết 1-N (Category, Department, Author, Brand...) ➔ Hiển thị ở ComboBox lọc & ComboBox form.
3. **Bảng Phụ (N-N Bridge/Detail Entity):** Đối tượng liên kết N-N (Supplier, Skill, Genre, Feature...) ➔ Hiển thị ở ComboBox lọc & CheckedListBox form.

---

### 2. Quy Tắc Quyết Định Cách Code Quan Hệ N-N (Cực Kỳ Quan Trọng)

Trước khi viết hàm `SaveButton_Click`, mở thư mục `Models/` kiểm tra:

- **Dạng A — Có Class Trung Gian (Ví dụ: `EmployeeSkill.cs`):**
  - **Dấu hiệu:** Mở `Models/` thấy có file class tên ghép kiểu `EmployeeSkill.cs` hoặc `BookGenre.cs`.
  - **Cách code:**
    ```csharp
    context.EmployeeSkills.Add(new EmployeeSkill { EmployeeId = entity.Id, SkillId = phu.SkillId });
    ```
- **Dạng B — Quan Hệ N-N Trực Tiếp (Ví dụ Đề 3 - Product & Supplier):**
  - **Dấu hiệu:** Mở `Models/` **KHÔNG CÓ** class `ProductSupplier.cs`. EF Core tự tạo `public virtual ICollection<Suppliers> Supplier { get; set; }` trong `Products.cs`.
  - **Cách code:**
    ```csharp
    entity.Supplier.Add(dbSupplier); // EF Core tự chèn vào bảng trung gian bên SQL
    ```

---

### 3. Bảng Tra Cứu Tên Placeholder ➔ Tên Thực Tế Theo Đề Thi

| Placeholder (Trong Guide) | Vai Trò (Role) | Đề 3 (Product) | Đề Book Store | Đề Employee |
| :--- | :--- | :--- | :--- | :--- |
| `TÊN_CONTEXT` | Class DbContext | `Prn21226sprB12Context` | `BookStoreDbContext` | `CompanyDbContext` |
| `TênBảngChính` | DbSet Bảng chính | `context.Products` | `context.Books` | `context.Employees` |
| `TênBảngDanhMục` | DbSet Bảng 1-N | `context.Categories` | `context.Authors` | `context.Departments` |
| `TênBảngPhụ` | DbSet Bảng N-N | `context.Suppliers` | `context.Genres` | `context.Skills` |
| `TênEntityChính` | Class Entity chính | `Products` | `Books` | `Employees` |
| `TênEntityPhụ` | Class Entity phụ | `Suppliers` | `Genres` | `Skills` |
| `NavigationDanhMục` | Navigation Prop (1-N) | `x.Category` | `x.Author` | `x.Department` |
| `NavigationPhụ` | Navigation Prop (N-N) | `x.Supplier` | `x.BookGenres` (hoặc `Genre`) | `x.EmployeeSkills` (hoặc `Skill`) |
| `TênHiểnThị` (Danh mục) | Tên hiển thị ComboBox 1-N | `CategoryName` | `AuthorName` | `DepartmentName` |
| `TênHiểnThị` (Phụ) | Tên hiển thị CheckedListBox | `SupplierName` | `GenreName` | `SkillName` |
| `DanhMucId` | Khóa chính Bảng 1-N | `CategoryId` | `AuthorId` | `DepartmentId` |
| `PhuId` | Khóa chính Bảng N-N | `SupplierId` | `GenreId` | `SkillId` |
| `txtTênTrường` | TextBox nhập tên | `txtName` (`ProductName`) | `txtTitle` (`Title`) | `txtFullName` (`FullName`) |
| `txtTênSố` | TextBox nhập số | `txtPrice`, `txtStock` | `txtPrice` | `txtSalary` |
| `cboFilterDanhMuc` | ComboBox lọc 1-N | `cboFilterCategory` | `cboFilterAuthor` | `cboFilterDept` |
| `cboFilterPhu` | ComboBox lọc N-N | `cboFilterSupplier` | `cboFilterGenre` | `cboFilterSkill` |
| `cboTênDanhMục` | ComboBox Form nhập 1-N | `cboCategory` | `cboAuthor` | `cboDept` |
| `lstTênEntityPhụ` | ListBox Form nhập N-N | `lstCheckedItems` | `lstGenres` | `lstSkills` |
| `dgTênEntity` | DataGrid hiển thị | `dgProduct` | `dgBooks` | `dgEmployees` |

