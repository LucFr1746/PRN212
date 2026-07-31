# Đề 6 — Hướng Dẫn Áp Dụng Kit Nhanh (Copy-Paste Recipe)

Đề 6 thuộc **Dạng A+X** (Q1: OOP Console + Q2: WPF Filter/Add/CheckedListBox), hoàn toàn tương thích với bộ Kit.

---

## Câu 1: Console App — Course Feedback Management

### Bản đồ đổi tên từ Kit template:

| Kit Template (ConsoleOOPTemplate.cs) | Đổi thành Đề 6 |
|:-------------------------------------|:----------------|
| `ScoreUpdateHandler` | `FeedbackHandler` |
| `IEvaluatable` | `IRatable` |
| `Student` (abstract) | `Course` (abstract) |
| `UndergraduateStudent` | `OnlineCourse` |
| `ScoreManager` | `FeedbackManager` |
| `StudentId` | `CourseId` |
| `FullName` | `CourseName` |
| `Scores` | `Ratings` |
| `Major` | `Platform` |
| `AddScore` | `AddRating` |
| `AverageScore` | `AverageRating` |
| `GetRank()` | `GetGrade()` |
| `AddScoreToStudent` | `AddRatingToCourse` |
| `GetTopStudents` | `GetTopCourses` |

### Điểm khác biệt cần sửa:
1. **Khoảng giá trị validation:** `[0, 10]` → `[1, 5]`
2. **Thông báo lỗi:** `"Score must be between 0 and 10"` → `"Rating must be between 1 and 5"`
3. **Bảng xếp hạng (GetGrade):**

| AverageRating | Grade |
|:---|:---|
| `>= 4.5` | `"Outstanding"` |
| `>= 3.5` | `"Good"` |
| `>= 2.5` | `"Satisfactory"` |
| `< 2.5` | `"Poor"` |

---

## Câu 2: WPF App — Book & Genre Management

### Bản đồ Ctrl+H:

| Kit Template | Đổi thành Đề 6 | Ý nghĩa |
|:-------------|:----------------|:--------|
| `Employees` | `Books` | Thực thể chính |
| `Departments` | `Authors` | Thực thể danh mục (1-N) |
| `Skills` | `Genres` | Thực thể tích chọn (N-N) |
| `EmployeeSkills` | `BookGenres` | Bảng trung gian |
| `EmployeeId` | `BookId` | Khóa chính |
| `FullName` | `Title` | Tên hiển thị |
| `Salary` | `Price` | Giá trị số thực |
| `Email` | *(không cần)* | Bỏ TextBox Email |
| `HireDate` | *(không cần)* | Bỏ cột HireDate |
| `DepartmentId` | `AuthorId` | Khóa ngoại danh mục |
| `DepartmentName` | `AuthorName` | Tên hiển thị danh mục |
| `SkillId` | `GenreId` | Khóa phụ |
| `SkillName` | `GenreName` | Tên hiển thị phụ |
| `cboFilterDept` | `cboFilterAuthor` | ComboBox lọc tác giả |
| `cboFilterSkill` | `cboFilterGenre` | ComboBox lọc thể loại |
| `cboDept` | `cboAuthor` | ComboBox chọn tác giả (form) |
| `lstSkills` | `lstGenres` | ListBox chứa Checkbox |
| `dgEmployees` | `dgBooks` | DataGrid chính |
| `txtFullName` | `txtTitle` | TextBox nhập tên sách |
| `txtSalary` | `txtPrice` | TextBox nhập giá |

### Cột DataGrid:
```xml
<DataGrid.Columns>
    <DataGridTextColumn Header="BookId" Binding="{Binding BookId}" Width="60"/>
    <DataGridTextColumn Header="Title" Binding="{Binding Title}" Width="*"/>
    <DataGridTextColumn Header="Price" Binding="{Binding Price, StringFormat={}{0:N2}}" Width="120"/>
    <DataGridTextColumn Header="Publish Year" Binding="{Binding PublishYear}" Width="100"/>
    <DataGridTextColumn Header="Author" Binding="{Binding Author.AuthorName}" Width="150"/>
</DataGrid.Columns>
```

### TextBox bổ sung cho `PublishYear`:
Thêm 1 Grid block trong Zone 3 (dưới Price):
```xml
<Grid Margin="0,5">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="100"/>
        <ColumnDefinition Width="*"/>
    </Grid.ColumnDefinitions>
    <TextBlock Text="Publish Year:" VerticalAlignment="Center"/>
    <TextBox x:Name="txtPublishYear" Grid.Column="1" Padding="3"/>
</Grid>
```

### Validation bổ sung trong SaveButton_Click:
```csharp
string yearText = txtPublishYear.Text.Trim();
if (!ValidationHelpers.TryParseInt(yearText, out int publishYear) || publishYear < 1900 || publishYear > DateTime.Now.Year)
{
    WpfHelpers.ShowError("Publish Year must be a valid year.");
    return;
}
```
