# ĐỀ 6 — HƯỚNG DẪN LÀM BÀI TỪNG BƯỚC VỚI KIT

> **Thời gian ước tính:** 70–80 phút (dư ~5–15 phút kiểm tra)
> **Phân bổ:**  Setup 10 phút → Q1 Console 20 phút → Q2 WPF 40 phút → Kiểm tra 10 phút

---

## GIAI ĐOẠN 0: CHUẨN BỊ (10 phút)

### Bước 0.1 — Chạy SQL Script tạo Database

1. Mở **SQL Server Management Studio (SSMS)**.
2. Mở file `script6.sql` từ thư mục đề thi.
3. Nhấn **F5** để thực thi → tạo database `BookStoreDB`.
4. Kiểm tra trong Object Explorer: phải thấy 4 bảng `Authors`, `Books`, `Genres`, `BookGenres`.

### Bước 0.2 — Mở Solution đề thi

1. Mở file `.sln` đã được cung cấp trong Visual Studio 2022.
2. Xác nhận có 2 project: **Q1** (Console App) và **Q2** (WPF App).
3. Cả 2 đều phải target **.NET 8.0** (chuột phải project → Properties → Target Framework).

### Bước 0.3 — Scaffold Database vào Project Q2

1. Mở **Tools → NuGet Package Manager → Package Manager Console**.
2. Trong dropdown **Default project** ở góc trên Console, chọn **Q2** (project WPF).
3. Paste lệnh sau và nhấn **Enter**:

```powershell
Scaffold-DbContext "Server=.\SQLEXPRESS;Database=BookStoreDB;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force -NoPluralize
```

> [!WARNING]
> **Tên Server có thể khác trên máy thi!** Hướng dẫn này dùng `.\SQLEXPRESS`. Nếu máy thi dùng tên khác, mở **SSMS** → xem tên server ở cửa sổ Connect → thay vào chỗ `.\SQLEXPRESS`.

4. Đợi chạy xong → thấy thư mục `Models/` xuất hiện trong project Q2 với các file:
   - `BookStoreDbContext.cs` (hoặc tên tương tự)
   - `Author.cs`
   - `Book.cs`
   - `Genre.cs`
   - `BookGenre.cs`

> [!CAUTION]
> Nếu báo lỗi thiếu package, kiểm tra project Q2 đã cài đủ:
> - `Microsoft.EntityFrameworkCore.SqlServer`
> - `Microsoft.EntityFrameworkCore.Tools`
> - `Microsoft.EntityFrameworkCore.Design`
> - `Microsoft.Extensions.Configuration.Json`

### Bước 0.4 — Tạo appsettings.json

1. Chuột phải **project Q2** → **Add** → **New Item** → chọn **JSON File** → đặt tên `appsettings.json`.
2. Paste nội dung sau vào file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\SQLEXPRESS;Database=BookStoreDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

3. **QUAN TRỌNG:** Chuột phải file `appsettings.json` → **Properties** → đổi **Copy to Output Directory** thành **Copy if newer**.

> [!WARNING]
> Nếu quên bước 3 này, chương trình sẽ **không tìm thấy file cấu hình** khi chạy và bạn **mất toàn bộ điểm Q2**.

### Bước 0.5 — Copy 4 file Helper từ Kit vào Project Q2

Từ thư mục Kit, copy 4 file sau vào thư mục gốc (hoặc tạo folder `Helpers/`) của project Q2:

| File nguồn trong Kit | Copy vào Project Q2 |
|:---------------------|:--------------------|
| `Kits/Foundation/ConfigurationHelper.cs` | `Q2/Helpers/ConfigurationHelper.cs` |
| `Kits/Common/ComboBoxExtensions.cs` | `Q2/Helpers/ComboBoxExtensions.cs` |
| `Kits/Common/ValidationHelpers.cs` | `Q2/Helpers/ValidationHelpers.cs` |
| `Kits/Common/WpfHelpers.cs` | `Q2/Helpers/WpfHelpers.cs` |

**Sau khi copy, sửa namespace ở dòng đầu mỗi file** cho trùng với namespace project Q2:

```csharp
// Trước (trong Kit):
namespace PRN212.ExamKit.Foundation
namespace PRN212.ExamKit.Common

// Sau (sửa thành):
namespace Q2.Helpers
```

> [!TIP]
> Cách nhanh: Dùng **Ctrl+H** trong từng file, thay `PRN212.ExamKit.Foundation` và `PRN212.ExamKit.Common` thành `Q2.Helpers`.

### Bước 0.6 — Sửa DbContext dùng cấu hình động

1. Mở file `Models/BookStoreDbContext.cs` (hoặc tên tương tự được sinh ra).
2. Tìm hàm `OnConfiguring` → **xóa toàn bộ nội dung** bên trong hàm.
3. Thay bằng đoạn sau:

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
> Xóa dòng `#warning To protect potentially sensitive information...` nếu có — dòng này do scaffold tự tạo ra.

### Bước 0.7 — Tạo Partial Class cho Genre (IsChecked binding)

1. Trong thư mục `Models/` của project Q2, tạo file mới tên `PartialEntities.cs`.
2. Paste nội dung sau:

```csharp
using System.ComponentModel.DataAnnotations.Schema;

namespace Q2.Models  // Phải trùng namespace với các file Model sinh ra
{
    public partial class Genre
    {
        [NotMapped]
        public bool IsChecked { get; set; }
    }
}
```

> [!IMPORTANT]
> Namespace ở đây **phải trùng chính xác** với namespace của file `Genre.cs` được scaffold sinh ra. Mở file `Genre.cs` lên để kiểm tra trước khi viết.

---

## GIAI ĐOẠN 1: CÂU 1 — CONSOLE APP (20 phút)

### Bước 1.1 — Copy template vào project Q1

1. Mở file `Program.cs` trong **project Q1**.
2. **Xóa hết** nội dung cũ.
3. Mở file Kit: `Kits/Snippets/ConsoleOOPTemplate.cs`.
4. Copy **phần 3A (Student)** + **phần 4A (UndergraduateStudent)** + **phần 5A (ScoreManager)** vì đề 6 có cấu trúc tương tự dạng Student.

### Bước 1.2 — Đổi tên từ Student → Course

Dùng **Ctrl+H** (Find & Replace) trong file `Program.cs` của Q1, thực hiện **theo đúng thứ tự** sau:

| # | Tìm (Find) | Thay bằng (Replace) | Ghi chú |
|:--|:-----------|:---------------------|:--------|
| 1 | `ScoreUpdateHandler` | `FeedbackHandler` | Tên delegate |
| 2 | `IEvaluatable` | `IRatable` | Tên interface |
| 3 | `UndergraduateStudent` | `OnlineCourse` | Tên concrete class (phải đổi trước `Student`) |
| 4 | `Student` | `Course` | Tên abstract class |
| 5 | `ScoreManager` | `FeedbackManager` | Tên manager class |
| 6 | `StudentId` | `CourseId` | Property ID |
| 7 | `FullName` | `CourseName` | Property tên |
| 8 | `Scores` | `Ratings` | Property danh sách |
| 9 | `Major` | `Platform` | Property phụ của concrete class |
| 10 | `AddScore` | `AddRating` | Tên phương thức thêm |
| 11 | `AverageScore` | `AverageRating` | Property trung bình |
| 12 | `GetRank` | `GetGrade` | Tên phương thức xếp hạng |
| 13 | `AddScoreToStudent` | `AddRatingToCourse` | Tên phương thức manager |
| 14 | `GetTopStudents` | `GetTopCourses` | Tên phương thức LINQ |
| 15 | `_students` | `_courses` | Tên field private |
| 16 | `_onScoreUpdated` | `_onFeedbackReceived` | Tên field delegate |

> [!CAUTION]
> **Phải đổi `UndergraduateStudent` TRƯỚC `Student`!** Nếu đổi `Student` trước, từ `UndergraduateStudent` sẽ bị đổi thành `UndergraduateCourse` — sai hoàn toàn.

### Bước 1.3 — Sửa 3 điểm khác biệt logic

Sau khi Ctrl+H xong, cần sửa tay 3 chỗ:

#### 1.3a — Khoảng giá trị validation (trong class `Course`)

```csharp
// TÌM dòng này (đã đổi tên):
if (rating < 0 || rating > 10)
{
    throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 0 and 10");
}

// SỬA THÀNH:
if (rating < 1 || rating > 5)
{
    throw new ArgumentOutOfRangeException(nameof(rating), "Rating must be between 1 and 5");
}
```

#### 1.3b — Bảng xếp hạng (trong class `OnlineCourse`, hàm `GetGrade`)

```csharp
// TÌM phần này (đã đổi tên):
public override string GetGrade()
{
    var avg = AverageRating;
    if (avg >= 8.5) return "Excellent";
    if (avg >= 7.0) return "Good";
    if (avg >= 5.0) return "Average";
    return "Fail";
}

// SỬA THÀNH:
public override string GetGrade()
{
    var avg = AverageRating;
    if (avg >= 4.5) return "Outstanding";
    if (avg >= 3.5) return "Good";
    if (avg >= 2.5) return "Satisfactory";
    return "Poor";
}
```

#### 1.3c — Xóa phần không dùng

Xóa bỏ các khối code KHÔNG thuộc đề 6 (nếu copy nguyên file template):
- Xóa delegate `DiscountAppliedCallback`
- Xóa interface `IDiscountable`
- Xóa class `Product`, `ElectronicsProduct`, `DiscountManager`
- Xóa class `DataVault<T>`
- Xóa block Main variant B (Product)

### Bước 1.4 — Viết hàm Main

Bỏ comment block **7A** (đã được đổi tên), sửa dữ liệu test cho phù hợp:

```csharp
class Program
{
    static void Main(string[] args)
    {
        FeedbackHandler handler = (name, rating) =>
            Console.WriteLine($"[FEEDBACK]: {name} received rating: {rating}");

        var manager = new FeedbackManager(handler);

        var c1 = new OnlineCourse("C1", "C# Masterclass", "Udemy");
        var c2 = new OnlineCourse("C2", "Python Basics", "Coursera");
        var c3 = new OnlineCourse("C3", "Web Development", "Pluralsight");

        manager.AddCourse(c1);
        manager.AddCourse(c2);
        manager.AddCourse(c3);

        manager.AddRatingToCourse("C1", 4.5);
        manager.AddRatingToCourse("C1", 4.8);
        manager.AddRatingToCourse("C2", 3.8);
        manager.AddRatingToCourse("C2", 3.5);
        manager.AddRatingToCourse("C3", 4.0);
        manager.AddRatingToCourse("C3", 4.2);

        try
        {
            manager.AddRatingToCourse("C1", 6.0); // rating > 5 → lỗi
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        Console.WriteLine("\n=== Top 2 Courses ===");
        var top = manager.GetTopCourses(2);
        for (int i = 0; i < top.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {top[i].CourseName} | Platform: {top[i].Platform} | Average: {top[i].AverageRating:F2} | Grade: {top[i].GetGrade()}");
        }
    }
}
```

### Bước 1.5 — Build & Test Q1

1. Chuột phải project **Q1** → **Set as Startup Project**.
2. Nhấn **Ctrl+F5** (Start Without Debugging).
3. Kiểm tra output phải giống mẫu trong đề:

```text
[FEEDBACK]: C# Masterclass received rating: 4.5
[FEEDBACK]: C# Masterclass received rating: 4.8
[FEEDBACK]: Python Basics received rating: 3.8
[FEEDBACK]: Python Basics received rating: 3.5
[FEEDBACK]: Web Development received rating: 4.0
[FEEDBACK]: Web Development received rating: 4.2
Error: Rating must be between 1 and 5 (Parameter 'rating')

=== Top 2 Courses ===
1. C# Masterclass | Platform: Udemy | Average: 4.65 | Grade: Outstanding
2. Web Development | Platform: Pluralsight | Average: 4.10 | Grade: Good
```

✅ **Q1 hoàn tất.** Chuyển sang Q2.

---

## GIAI ĐOẠN 2: CÂU 2 — WPF APP (40 phút)

### PHẦN A: GIAO DIỆN XAML (10 phút)

#### Bước 2.1 — Copy template XAML vào MainWindow.xaml

1. Mở file `MainWindow.xaml` của project **Q2**.
2. **Xóa hết** nội dung cũ.
3. Mở file Kit: `Kits/Templates/CrudWindowTemplate.xaml`.
4. **Copy toàn bộ** nội dung → paste vào `MainWindow.xaml`.

#### Bước 2.2 — Sửa namespace Class (dòng 1)

```xml
<!-- TRƯỚC (trong Kit): -->
<Window x:Class="PRN212.ExamKit.Templates.CrudWindow"

<!-- SỬA THÀNH: -->
<Window x:Class="Q2.MainWindow"
```

Đồng thời sửa Title:
```xml
<!-- TRƯỚC: -->
Title="Resource Management"

<!-- SỬA THÀNH: -->
Title="Book & Genre Management"
```

#### Bước 2.3 — Sửa Zone 1 (Filter Area)

Tìm phần ZONE 1, sửa label và tên control:

```xml
<!-- TRƯỚC: -->
<TextBlock Text="Category:" VerticalAlignment="Center" Margin="0,0,5,0"/>
<ComboBox x:Name="cboFilterCategory" Width="150" Margin="0,0,15,0"/>

<TextBlock Text="Secondary:" VerticalAlignment="Center" Margin="0,0,5,0"/>
<ComboBox x:Name="cboFilterSecondary" Width="150" Margin="0,0,15,0"/>

<!-- SỬA THÀNH: -->
<TextBlock Text="Author:" VerticalAlignment="Center" Margin="0,0,5,0"/>
<ComboBox x:Name="cboFilterAuthor" Width="180" Margin="0,0,15,0"/>

<TextBlock Text="Genre:" VerticalAlignment="Center" Margin="0,0,5,0"/>
<ComboBox x:Name="cboFilterGenre" Width="150" Margin="0,0,15,0"/>
```

#### Bước 2.4 — Sửa Zone 2 (DataGrid)

Tìm phần ZONE 2, **thay toàn bộ** block `<DataGrid>`:

```xml
<!-- XÓA toàn bộ DataGrid cũ và THAY BẰNG: -->
<DataGrid x:Name="dgBooks" AutoGenerateColumns="False" IsReadOnly="True" SelectionMode="Single" AlternatingRowBackground="#F9FAFB" GridLinesVisibility="Horizontal">
    <DataGrid.Columns>
        <DataGridTextColumn Header="BookId" Binding="{Binding BookId}" Width="60"/>
        <DataGridTextColumn Header="Title" Binding="{Binding Title}" Width="*"/>
        <DataGridTextColumn Header="Price" Binding="{Binding Price, StringFormat={}{0:N2}}" Width="120"/>
        <DataGridTextColumn Header="Publish Year" Binding="{Binding PublishYear}" Width="100"/>
        <DataGridTextColumn Header="Author" Binding="{Binding Author.AuthorName}" Width="160"/>
    </DataGrid.Columns>
</DataGrid>
```

> [!CAUTION]
> **XÓA thuộc tính `SelectionChanged="DgRecords_SelectionChanged"`** khỏi thẻ `<DataGrid>` vì đề 6 không yêu cầu click-to-edit. Nếu để lại sẽ lỗi compile vì thiếu event handler.

Đổi GroupBox header:
```xml
<!-- TRƯỚC: -->
<GroupBox Header="Data Records"

<!-- SỬA THÀNH: -->
<GroupBox Header="Book List"
```

#### Bước 2.5 — Sửa Zone 3 (Form nhập liệu)

##### Sửa cột trái — TextBox và ComboBox:

```xml
<!-- SỬA label và tên control cho phù hợp: -->
<GroupBox Header="Add New Book" Grid.Row="2" Padding="10">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="2*"/>
            <ColumnDefinition Width="1*"/>
        </Grid.ColumnDefinitions>

        <!-- Left Column: Primary Fields -->
        <StackPanel Grid.Column="0" Margin="0,0,10,0">
            <Grid Margin="0,5">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="100"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>
                <TextBlock Text="Title:" VerticalAlignment="Center"/>
                <TextBox x:Name="txtTitle" Grid.Column="1" Padding="3"/>
            </Grid>

            <Grid Margin="0,5">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="100"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>
                <TextBlock Text="Price:" VerticalAlignment="Center"/>
                <TextBox x:Name="txtPrice" Grid.Column="1" Padding="3"/>
            </Grid>

            <!-- THÊM MỚI: TextBox cho PublishYear (đề có 3 trường nhập) -->
            <Grid Margin="0,5">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="100"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>
                <TextBlock Text="Publish Year:" VerticalAlignment="Center"/>
                <TextBox x:Name="txtPublishYear" Grid.Column="1" Padding="3"/>
            </Grid>

            <Grid Margin="0,5">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="100"/>
                    <ColumnDefinition Width="*"/>
                </Grid.ColumnDefinitions>
                <TextBlock Text="Author:" VerticalAlignment="Center"/>
                <ComboBox x:Name="cboAuthor" Grid.Column="1" Padding="3"/>
            </Grid>
        </StackPanel>
```

##### Sửa cột phải — CheckedListBox:

```xml
        <!-- Right Column: Checkbox List -->
        <Grid Grid.Column="1">
            <Grid.RowDefinitions>
                <RowDefinition Height="Auto"/>
                <RowDefinition Height="*"/>
            </Grid.RowDefinitions>
            <TextBlock Text="Genres:" FontWeight="Bold" Margin="0,0,0,5"/>
            <ListBox x:Name="lstGenres" Grid.Row="1" Height="120" SelectionMode="Multiple">
                <ListBox.ItemTemplate>
                    <DataTemplate>
                        <CheckBox Content="{Binding GenreName}" IsChecked="{Binding IsChecked, Mode=TwoWay}" Margin="2"/>
                    </DataTemplate>
                </ListBox.ItemTemplate>
            </ListBox>
        </Grid>
```

##### Sửa nút bấm:

```xml
        <!-- Lower Command Row -->
        <StackPanel Grid.ColumnSpan="2" Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,15,0,0">
            <Button Content="Add Book" Click="SaveButton_Click" Width="100" Margin="0,0,5,0" Padding="5" Background="#2563EB" Foreground="White" FontWeight="Bold"/>
            <Button Content="Clear Form" Click="ResetButton_Click" Width="100" Padding="5" Background="#9CA3AF" Foreground="White"/>
        </StackPanel>
    </Grid>
</GroupBox>
```

---

### PHẦN B: CODE-BEHIND C# (25 phút)

#### Bước 2.6 — Copy template code-behind vào MainWindow.xaml.cs

1. Mở file `MainWindow.xaml.cs` của project **Q2**.
2. **Xóa hết** nội dung cũ.
3. Paste code sau đây (đã được điều chỉnh hoàn chỉnh cho Đề 6):

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Q2.Models;
using Q2.Helpers;

namespace Q2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadInitData();
        }

        // ==========================================
        // LOAD INITIAL DATA
        // ==========================================
        private void LoadInitData()
        {
            try
            {
                using (var context = new BookStoreDbContext())
                {
                    var authors = context.Authors.ToList();
                    var genres = context.Genres.ToList();

                    // 1. Filter ComboBox "Author" — có "All"
                    cboFilterAuthor.LoadWithDefault(
                        authors,
                        "AuthorName",
                        "AuthorId",
                        () => new Author { AuthorId = 0, AuthorName = "All" }
                    );

                    // 2. Filter ComboBox "Genre" — có "All"
                    cboFilterGenre.LoadWithDefault(
                        genres,
                        "GenreName",
                        "GenreId",
                        () => new Genre { GenreId = 0, GenreName = "All" }
                    );

                    // 3. Form ComboBox "Author" — KHÔNG có "All"
                    cboAuthor.ItemsSource = authors;
                    cboAuthor.DisplayMemberPath = "AuthorName";
                    cboAuthor.SelectedValuePath = "AuthorId";
                    cboAuthor.SelectedIndex = 0;

                    // 4. CheckedListBox "Genres"
                    lstGenres.ItemsSource = genres;
                }

                // 5. Load DataGrid
                RefreshGrid();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to load initial data: {ex.Message}");
            }
        }

        private void RefreshGrid()
        {
            using (var context = new BookStoreDbContext())
            {
                dgBooks.ItemsSource = context.Books
                    .Include(b => b.Author)
                    .ToList();
            }
        }

        // ==========================================
        // FILTER
        // ==========================================
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedAuthorId = Convert.ToInt32(cboFilterAuthor.SelectedValue);
                int selectedGenreId = Convert.ToInt32(cboFilterGenre.SelectedValue);

                using (var context = new BookStoreDbContext())
                {
                    IQueryable<Book> query = context.Books
                        .Include(b => b.Author)
                        .Include(b => b.BookGenres);

                    // Lọc theo Author (0 = "All")
                    if (selectedAuthorId > 0)
                    {
                        query = query.Where(b => b.AuthorId == selectedAuthorId);
                    }

                    // Lọc theo Genre (0 = "All")
                    if (selectedGenreId > 0)
                    {
                        query = query.Where(b => b.BookGenres.Any(bg => bg.GenreId == selectedGenreId));
                    }

                    dgBooks.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Filtering error: {ex.Message}");
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            cboFilterAuthor.SelectedIndex = 0;
            cboFilterGenre.SelectedIndex = 0;
            RefreshGrid();
        }

        // ==========================================
        // ADD NEW BOOK
        // ==========================================
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Đọc dữ liệu từ form
            var title = txtTitle.Text.Trim();
            var priceText = txtPrice.Text.Trim();
            var yearText = txtPublishYear.Text.Trim();
            var authorId = cboAuthor.SelectedValue;

            // 2. Validate Title
            if (!ValidationHelpers.IsRequired(title))
            {
                WpfHelpers.ShowError("Title is required.");
                return;
            }

            // 3. Validate Price
            if (!ValidationHelpers.TryParseDecimal(priceText, out decimal price) || price <= 0)
            {
                WpfHelpers.ShowError("Price must be a valid number greater than 0.");
                return;
            }

            // 4. Validate Publish Year
            if (!ValidationHelpers.TryParseInt(yearText, out int publishYear) || publishYear < 1900 || publishYear > DateTime.Now.Year)
            {
                WpfHelpers.ShowError($"Publish Year must be between 1900 and {DateTime.Now.Year}.");
                return;
            }

            // 5. Validate Author
            if (authorId == null)
            {
                WpfHelpers.ShowError("Please select an author.");
                return;
            }

            // 6. Validate Genres (ít nhất 1 checkbox được tích)
            var allGenres = lstGenres.ItemsSource as List<Genre>;
            var selectedGenres = allGenres?.Where(g => g.IsChecked).ToList() ?? new List<Genre>();

            if (selectedGenres.Count == 0)
            {
                WpfHelpers.ShowError("Please select at least one genre.");
                return;
            }

            try
            {
                using (var context = new BookStoreDbContext())
                {
                    // 7. Tạo và lưu Book
                    var book = new Book
                    {
                        Title = title,
                        Price = price,
                        PublishYear = publishYear,
                        AuthorId = Convert.ToInt32(authorId)
                    };
                    context.Books.Add(book);
                    context.SaveChanges(); // Sinh BookId tự động

                    // 8. Tạo các bản ghi BookGenres (bảng trung gian)
                    foreach (var genre in selectedGenres)
                    {
                        var bookGenre = new BookGenre
                        {
                            BookId = book.BookId,
                            GenreId = genre.GenreId
                        };
                        context.BookGenres.Add(bookGenre);
                    }
                    context.SaveChanges();
                }

                WpfHelpers.ShowInfo("Book added successfully!");
                RefreshGrid();
                ResetForm();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Database operation failed: {ex.Message}");
            }
        }

        // ==========================================
        // RESET FORM
        // ==========================================
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtTitle.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtPublishYear.Text = string.Empty;
            cboAuthor.SelectedIndex = 0;

            // Bỏ tích tất cả checkbox
            var allGenres = lstGenres.ItemsSource as List<Genre>;
            if (allGenres != null)
            {
                foreach (var g in allGenres) g.IsChecked = false;
            }
            lstGenres.Items.Refresh();
        }
    }
}
```

> [!IMPORTANT]
> **Tên DbContext:** Trong code trên dùng `BookStoreDbContext`. Nếu scaffold sinh ra tên khác (ví dụ `BookStoreDBContext` hoặc `BookStoreDbContext`), hãy dùng **Ctrl+H** để đổi cho khớp. Mở file `Models/BookStore...Context.cs` để xem tên chính xác.

#### Bước 2.7 — Kiểm tra namespace và using

Đảm bảo các dòng `using` ở đầu file đúng với namespace thực tế trong project:

```csharp
using Q2.Models;   // Namespace chứa các entity (Book, Author, Genre, BookGenre, DbContext)
using Q2.Helpers;  // Namespace chứa các file Helper đã copy
```

> [!TIP]
> Nếu gặp lỗi đỏ ở `using`, chuột phải lên tên class bị lỗi → **Quick Actions** → **using ...** để VS tự thêm namespace đúng.

---

### PHẦN C: BUILD & TEST (5 phút)

#### Bước 2.8 — Build Solution

1. Nhấn **Ctrl+Shift+B** (Build Solution).
2. Nếu có lỗi, kiểm tra:
   - Tên DbContext có khớp không?
   - Namespace `using` đã đúng chưa?
   - File `appsettings.json` đã set **Copy if newer** chưa?
   - Partial class `Genre` namespace có khớp với `Models/Genre.cs` không?

#### Bước 2.9 — Chạy và test chức năng

1. Chuột phải project **Q2** → **Set as Startup Project**.
2. Nhấn **F5** (Debug) hoặc **Ctrl+F5** (Run without debug).

**Checklist test:**

| # | Chức năng | Kết quả mong đợi | ✓ |
|:--|:----------|:-----------------|:--|
| 1 | Cửa sổ mở lên | Hiển thị 3 vùng: Filter, DataGrid, Form | ☐ |
| 2 | DataGrid hiển thị sách | 10 sách với cột BookId, Title, Price, PublishYear, AuthorName | ☐ |
| 3 | ComboBox Author (filter) | Có "All" + 5 tác giả | ☐ |
| 4 | ComboBox Genre (filter) | Có "All" + 6 thể loại | ☐ |
| 5 | Lọc theo Author | Chọn "Haruki Murakami" → chỉ hiển thị 2 sách | ☐ |
| 6 | Lọc theo Genre | Chọn "Fantasy" → chỉ hiển thị 4 sách | ☐ |
| 7 | Lọc đồng thời | Chọn "J.K. Rowling" + "Fantasy" → 2 sách Harry Potter | ☐ |
| 8 | Clear filter | Bấm Clear → reset về All, hiển thị tất cả | ☐ |
| 9 | Add Book — validation trống | Để trống Title → bấm Add → hiện lỗi | ☐ |
| 10 | Add Book — validation genre | Không tích genre nào → hiện lỗi | ☐ |
| 11 | Add Book — thành công | Nhập đầy đủ + tích genre → Add → sách mới xuất hiện trong grid | ☐ |
| 12 | Clear Form | Bấm Clear Form → tất cả input trống, checkbox bỏ tích | ☐ |

---

## TÓM TẮT CÁC FILE ĐÃ TẠO/SỬA TRONG BÀI THI

```text
Solution/
├── Q1/                              (Console App - 4 điểm)
│   └── Program.cs                   ← Copy từ ConsoleOOPTemplate.cs + Ctrl+H đổi tên + sửa logic
│
└── Q2/                              (WPF App - 6 điểm)
    ├── Models/                      ← Tự sinh ra từ Scaffold-DbContext
    │   ├── Author.cs                (scaffold)
    │   ├── Book.cs                  (scaffold)
    │   ├── Genre.cs                 (scaffold)
    │   ├── BookGenre.cs             (scaffold)
    │   ├── BookStoreDbContext.cs     (scaffold → SỬA OnConfiguring)
    │   └── PartialEntities.cs       ← TẠO MỚI (partial Genre + IsChecked)
    │
    ├── Helpers/                     ← COPY từ Kit (sửa namespace)
    │   ├── ConfigurationHelper.cs
    │   ├── ComboBoxExtensions.cs
    │   ├── ValidationHelpers.cs
    │   └── WpfHelpers.cs
    │
    ├── MainWindow.xaml              ← COPY từ CrudWindowTemplate.xaml + sửa
    ├── MainWindow.xaml.cs           ← VIẾT MỚI (hoặc copy + sửa từ template)
    └── appsettings.json             ← TẠO MỚI (Copy if newer!)
```

> [!TIP]
> **Trước khi nộp bài:** Chuột phải Solution → **Clean Solution** để xóa `bin/obj`, giảm dung lượng file nộp.
