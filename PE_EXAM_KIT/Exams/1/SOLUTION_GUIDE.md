# Hướng Dẫn Giải Đề 1 (Từng Bước Từ A-Z)

Tài liệu này hướng dẫn chi tiết các bước để giải quyết trọn vẹn Đề 1 sử dụng các công cụ và cấu trúc của **PRN212 Exam Kit**.

---

## Bước 1: Chuẩn Bị Cơ Sở Dữ Liệu & Scaffolding (10 Phút)

### 1. Khởi tạo Cơ sở dữ liệu trong SQL Server
1. Mở SSMS.
2. Mở file [script1.sql](file:///d:/Coding Space/FPT/PRN212/PE_EXAM_KIT/Exams/1/script1.sql) và chạy lệnh (`F5`). Cơ sở dữ liệu `PRN212_26SprB1_1` sẽ được tạo ra với các bảng: `Departments`, `Employees`, `Skills`, và bảng liên kết `EmployeeSkills`.

### 2. Tạo file appsettings.json trong WPF Project
Tạo file `appsettings.json` tại thư mục gốc của dự án WPF `Q2`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=PRN212_26SprB1_1;User Id=sa;Password=123123;TrustServerCertificate=True;"
  }
}
```
> [!IMPORTANT]
> Nhấp chuột phải vào `appsettings.json` -> chọn **Properties** -> thiết lập **Copy to Output Directory** là **Copy if newer**.

### 3. Thực hiện Scaffold Database First
Mở một cửa sổ Terminal tại thư mục `Exams\1\Solution\Q2` (chứa file `Q2.csproj`) và chạy lệnh khôi phục gói NuGet trước:
```bash
dotnet restore
```
Sau đó chạy lệnh Scaffold Database First bằng tài khoản `sa` của bạn:
```bash
dotnet ef dbcontext scaffold "Server=.\\SQLEXPRESS;Database=PRN212_26SprB1_1;User Id=sa;Password=123123;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -f --no-pluralize
```

### 4. Tích hợp DbContext với Configuration Connection
Mở file `Models/Prn21226sprB11Context.cs` vừa sinh ra, thay thế thân hàm `OnConfiguring` để đọc động connection string:
```csharp
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (!optionsBuilder.IsConfigured)
    {
        try
        {
            optionsBuilder.UseSqlServer(Q2.Helpers.ConfigurationHelper.GetConnectionString("DefaultConnection"));
        }
        catch (System.Exception)
        {
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=PRN212_26SprB1_1;User Id=sa;Password=123123;TrustServerCertificate=True");
        }
    }
}
```

---

## Bước 2: Giải Quyết Câu 1 (Console App - OOP & Collections) (20 Phút)

Viết mã nguồn hoàn chỉnh cho file `Program.cs` của dự án Console `Q1` để xử lý tính điểm và quản lý học sinh:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace Q1
{
    // 1. DELEGATE DECLARATION
    public delegate void ScoreUpdateHandler(string studentName, double newScore);

    // 2. INTERFACE DECLARATION
    public interface IEvaluatable
    {
        double AverageScore { get; }
        string GetRank();
    }

    // 3. ABSTRACT CLASS STUDENT
    public abstract class Student
    {
        public string StudentId { get; set; }
        public string FullName { get; set; }
        public List<double> Scores { get; set; }

        protected Student(string studentId, string fullName)
        {
            StudentId = studentId;
            FullName = fullName;
            Scores = new List<double>();
        }

        public void AddScore(double score)
        {
            if (score < 0 || score > 10)
            {
                throw new ArgumentOutOfRangeException(nameof(score), "Score must be between 0 and 10");
            }
            Scores.Add(score);
        }

        public abstract string GetRank();
    }

    // 4. CONCRETE CLASS UNDERGRADUATESTUDENT
    public class UndergraduateStudent : Student, IEvaluatable
    {
        public string Major { get; set; }

        public UndergraduateStudent(string studentId, string fullName, string major)
            : base(studentId, fullName)
        {
            Major = major;
        }

        public double AverageScore
        {
            get
            {
                if (Scores.Count == 0) return 0;
                return Scores.Average();
            }
        }

        public override string GetRank()
        {
            double avg = AverageScore;
            if (avg >= 8.5) return "Excellent";
            if (avg >= 7.0) return "Good";
            if (avg >= 5.0) return "Average";
            return "Fail";
        }
    }

    // 5. SCORE MANAGER CLASS
    public class ScoreManager
    {
        private readonly List<UndergraduateStudent> _students;
        private readonly ScoreUpdateHandler _onScoreUpdated;

        public ScoreManager(ScoreUpdateHandler handler)
        {
            _students = new List<UndergraduateStudent>();
            _onScoreUpdated = handler;
        }

        public void AddStudent(UndergraduateStudent student)
        {
            _students.Add(student);
        }

        public void AddScoreToStudent(string studentId, double score)
        {
            var student = _students.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null)
            {
                throw new InvalidOperationException($"Student not found: {studentId}");
            }

            student.AddScore(score); // Throws ArgumentOutOfRangeException if out of range
            _onScoreUpdated?.Invoke(student.FullName, score); // Trigger event callback
        }

        public List<UndergraduateStudent> GetTopStudents(int n)
        {
            return _students
                .OrderByDescending(s => s.AverageScore)
                .Take(n)
                .ToList();
        }
    }

    // 6. MAIN PROGRAM TESTING
    class Program
    {
        static void Main(string[] args)
        {
            // Define Callback Handler
            ScoreUpdateHandler handler = (name, score) =>
                Console.WriteLine($"[SCORE UPDATE]: {name} received score: {score:F1}");

            ScoreManager manager = new ScoreManager(handler);

            // 1. Create students
            var s1 = new UndergraduateStudent("S1", "Nguyen Van A", "IT");
            var s2 = new UndergraduateStudent("S2", "Tran Thi B", "BA");
            var s3 = new UndergraduateStudent("S3", "Le Van C", "GD");

            manager.AddStudent(s1);
            manager.AddStudent(s2);
            manager.AddStudent(s3);

            // 2. Add scores with exception testing
            try
            {
                manager.AddScoreToStudent("S1", 8.5);
                manager.AddScoreToStudent("S2", 7.0);
                manager.AddScoreToStudent("S3", 6.0);

                // This triggers ArgumentOutOfRangeException
                manager.AddScoreToStudent("S1", 11.5); 
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // Clean exception message
                string message = ex.Message.Split('\n')[0];
                if (message.Contains("Score must be between 0 and 10"))
                {
                    Console.WriteLine("Error: Score must be between 0 and 10");
                }
                else
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            // 3. Print Top Students
            Console.WriteLine("\n=== Top 2 Students ===");
            var top = manager.GetTopStudents(2);
            int rank = 1;
            foreach (var student in top)
            {
                Console.WriteLine($"{rank++}. {student.FullName,-12} | Major: {student.Major} | Average: {student.AverageScore:F2} | Rank: {student.GetRank()}");
            }
        }
    }
}
```

---

## Bước 3: Tích Hợp Thư Viện Kit & Binding Hack (5 Phút)

### 1. Sao chép công cụ dùng chung vào thư mục Helpers
Tạo thư mục `Helpers` trong dự án WPF `Q2` và tạo các file sau từ Kit:
*   `Kits/Foundation/ConfigurationHelper.cs`
*   `Kits/Common/ComboBoxExtensions.cs`
*   `Kits/Common/ValidationHelpers.cs`
*   `Kits/Common/WpfHelpers.cs`

### 2. Thiết lập Partial Entity Binding Hack cho checked list
Tạo file `PartialEntities.cs` trong thư mục `Models` của `Q2` để thêm biến phụ tích chọn cho class `Skills`:

```csharp
using System.ComponentModel.DataAnnotations.Schema;

namespace Q2.Models
{
    public partial class Skills
    {
        [NotMapped]
        public bool IsChecked { get; set; }
    }
}
```

---

## Bước 4: Thiết Kế Giao Diện MainWindow.xaml (15 Phút)

Thay thế nội dung file `MainWindow.xaml` của `Q2` bằng XAML phân chia 3 khu vực lọc, xem và nhập liệu:

```xml
<Window x:Class="Q2.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Employee and Skills Management" Height="650" Width="850" WindowStartupLocation="CenterScreen">
    <Grid Margin="10">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <!-- ZONE 1: FILTER PANEL -->
        <GroupBox Header="FILTER AREA" Grid.Row="0" Margin="0,0,0,10" Padding="10">
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="Department:" VerticalAlignment="Center" Margin="0,0,5,0"/>
                <ComboBox x:Name="cboFilterDept" Width="150" Margin="0,0,20,0"/>

                <TextBlock Text="Skill:" VerticalAlignment="Center" Margin="0,0,5,0"/>
                <ComboBox x:Name="cboFilterSkill" Width="150" Margin="0,0,20,0"/>

                <Button Content="Filter" Click="FilterButton_Click" Width="80" Margin="0,0,5,0" Background="#10B981" Foreground="White" FontWeight="Bold"/>
                <Button Content="Clear" Click="ClearFilterButton_Click" Width="80" Background="#6B7280" Foreground="White"/>
            </StackPanel>
        </GroupBox>

        <!-- ZONE 2: EMPLOYEE LIST DATA GRID -->
        <GroupBox Header="EMPLOYEE LIST" Grid.Row="1" Margin="0,0,0,10">
            <DataGrid x:Name="dgEmployees" AutoGenerateColumns="False" IsReadOnly="True" AlternatingRowBackground="#F9FAFB" GridLinesVisibility="Horizontal">
                <DataGrid.Columns>
                    <DataGridTextColumn Header="Employee ID" Binding="{Binding EmployeeId}" Width="100"/>
                    <DataGridTextColumn Header="Full Name" Binding="{Binding FullName}" Width="1.5*"/>
                    <DataGridTextColumn Header="Email" Binding="{Binding Email}" Width="2*"/>
                    <DataGridTextColumn Header="Salary" Binding="{Binding Salary, StringFormat={}{0:N2}}" Width="1.2*"/>
                    <DataGridTextColumn Header="Hire Date" Binding="{Binding HireDate, StringFormat={}{0:yyyy-MM-dd}}" Width="1.2*"/>
                    <DataGridTextColumn Header="Department" Binding="{Binding Department.DepartmentName}" Width="1.5*"/>
                </DataGrid.Columns>
            </DataGrid>
        </GroupBox>

        <!-- ZONE 3: ADD NEW EMPLOYEE FORM -->
        <GroupBox Header="ADD NEW EMPLOYEE" Grid.Row="2" Padding="10">
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="2*"/>
                    <ColumnDefinition Width="1*"/>
                </Grid.ColumnDefinitions>

                <!-- Left Column: Inputs -->
                <StackPanel Grid.Column="0" Margin="0,0,15,0">
                    <Grid Margin="0,5">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="120"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        <TextBlock Text="Full Name:" VerticalAlignment="Center"/>
                        <TextBox x:Name="txtFullName" Grid.Column="1" Padding="4"/>
                    </Grid>

                    <Grid Margin="0,5">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="120"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        <TextBlock Text="Email:" VerticalAlignment="Center"/>
                        <TextBox x:Name="txtEmail" Grid.Column="1" Padding="4"/>
                    </Grid>

                    <Grid Margin="0,5">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="120"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        <TextBlock Text="Salary:" VerticalAlignment="Center"/>
                        <TextBox x:Name="txtSalary" Grid.Column="1" Padding="4"/>
                    </Grid>

                    <Grid Margin="0,5">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="120"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        <TextBlock Text="Department:" VerticalAlignment="Center"/>
                        <ComboBox x:Name="cboDept" Grid.Column="1" Padding="4"/>
                    </Grid>
                </StackPanel>

                <!-- Right Column: Skills Checklist -->
                <Grid Grid.Column="1">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                    </Grid.RowDefinitions>
                    <TextBlock Text="Skills selection (select at least 1):" FontWeight="Bold" Margin="0,0,0,5"/>
                    <ListBox x:Name="lstSkills" Grid.Row="1" Height="110" BorderThickness="1" BorderBrush="#D1D5DB" SelectionMode="Multiple">
                        <ListBox.ItemTemplate>
                            <DataTemplate>
                                <CheckBox Content="{Binding SkillName}" IsChecked="{Binding IsChecked, Mode=TwoWay}" Margin="2"/>
                            </DataTemplate>
                        </ListBox.ItemTemplate>
                    </ListBox>
                </Grid>

                <!-- Buttons -->
                <StackPanel Grid.ColumnSpan="2" Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,15,0,0">
                    <Button Content="Add Employee" Click="AddEmployeeButton_Click" Width="130" Margin="0,0,5,0" Padding="6" Background="#2563EB" Foreground="White" FontWeight="Bold" BorderThickness="0"/>
                    <Button Content="Clear Form" Click="ClearFormButton_Click" Width="100" Padding="6" Background="#9CA3AF" Foreground="White" BorderThickness="0"/>
                </StackPanel>
            </Grid>
        </GroupBox>
    </Grid>
</Window>
```

---

## Bước 5: Viết Code Xử Lý MainWindow.xaml.cs (25 Phút)

Sao chép toàn bộ logic xử lý nghiệp vụ của `MainWindow.xaml.cs` sau đây:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
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
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var context = new Prn21226sprB11Context())
                {
                    var departments = context.Departments.ToList();
                    var skills = context.Skills.ToList();

                    // 1. Tải bộ lọc ComboBox kèm chữ "All" dùng hàm mở rộng ComboBoxExtensions
                    cboFilterDept.LoadWithDefault(
                        departments, 
                        "DepartmentName", 
                        "DepartmentId", 
                        () => new Departments { DepartmentId = 0, DepartmentName = "All" }
                    );

                    cboFilterSkill.LoadWithDefault(
                        skills, 
                        "SkillName", 
                        "SkillId", 
                        () => new Skills { SkillId = 0, SkillName = "All" }
                    );

                    // 2. Tải ComboBox trong form nhập
                    cboDept.ItemsSource = departments;
                    cboDept.DisplayMemberPath = "DepartmentName";
                    cboDept.SelectedValuePath = "DepartmentId";
                    cboDept.SelectedIndex = 0;

                    // 3. Tải danh sách checkbox kỹ năng
                    lstSkills.ItemsSource = skills;
                }

                RefreshGrid();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to initialize data: {ex.Message}");
            }
        }

        private void RefreshGrid()
        {
            try
            {
                using (var context = new Prn21226sprB11Context())
                {
                    // LƯU Ý: Phải sử dụng Eager Loading .Include(e => e.Department) để hiển thị tên phòng ban
                    dgEmployees.ItemsSource = context.Employees
                        .Include(e => e.Department)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to load employees: {ex.Message}");
            }
        }

        // ==========================================
        // DỰNG TRUY VẤN LỌC ĐỒNG THỜI
        // ==========================================
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedDeptId = Convert.ToInt32(cboFilterDept.SelectedValue);
                int selectedSkillId = Convert.ToInt32(cboFilterSkill.SelectedValue);

                using (var context = new Prn21226sprB11Context())
                {
                    // Đề bài tự động ánh xạ quan hệ nhiều-nhiều trực tiếp thông qua thuộc tính Skill
                    IQueryable<Employees> query = context.Employees
                        .Include(e => e.Department)
                        .Include(e => e.Skill);

                    // A. Lọc theo Phòng ban
                    if (selectedDeptId > 0)
                    {
                        query = query.Where(e => e.DepartmentId == selectedDeptId);
                    }

                    // B. Lọc theo Kỹ năng
                    if (selectedSkillId > 0)
                    {
                        query = query.Where(e => e.Skill.Any(s => s.SkillId == selectedSkillId));
                    }

                    dgEmployees.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to filter: {ex.Message}");
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            cboFilterDept.SelectedIndex = 0;
            cboFilterSkill.SelectedIndex = 0;
            RefreshGrid();
        }

        // ==========================================
        // THÊM MỚI NHÂN VIÊN & LIÊN KẾT NHIỀU - NHIỀU
        // ==========================================
        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string salaryText = txtSalary.Text.Trim();
            object deptIdVal = cboDept.SelectedValue;

            // 1. Thực hiện các validation bắt buộc
            if (!ValidationHelpers.IsRequired(fullName))
            {
                WpfHelpers.ShowError("Full name is required.");
                return;
            }

            if (!ValidationHelpers.IsRequired(email) || !ValidationHelpers.IsValidEmail(email))
            {
                WpfHelpers.ShowError("A valid email address is required.");
                return;
            }

            if (!ValidationHelpers.TryParseDecimal(salaryText, out decimal salary) || salary <= 0)
            {
                WpfHelpers.ShowError("Salary must be a positive numeric value.");
                return;
            }

            if (deptIdVal == null)
            {
                WpfHelpers.ShowError("Please select a department.");
                return;
            }

            // 2. Lấy ra các Kỹ năng được tích chọn
            var allSkills = lstSkills.ItemsSource as List<Skills>;
            var selectedSkills = allSkills?.Where(s => s.IsChecked).ToList() ?? new List<Skills>();

            if (selectedSkills.Count == 0)
            {
                WpfHelpers.ShowError("Please select at least 1 skill.");
                return;
            }

            try
            {
                using (var context = new Prn21226sprB11Context())
                {
                    var employee = new Employees
                    {
                        FullName = fullName,
                        Email = email,
                        Salary = salary,
                        DepartmentId = Convert.ToInt32(deptIdVal),
                        HireDate = DateOnly.FromDateTime(DateTime.Today) // Gán HireDate là ngày hiện tại
                    };

                    // Vì EF Core đã tối ưu hóa quan hệ nhiều-nhiều trực tiếp:
                    // Thêm trực tiếp các thực thể Skill đã tồn tại vào danh sách Skill của Employee.
                    // Sử dụng EntityState.Unchanged để báo cho EF Core biết các Skill này đã có sẵn trong DB, tránh sinh mới.
                    foreach (var s in selectedSkills)
                    {
                        context.Entry(s).State = EntityState.Unchanged; 
                        employee.Skill.Add(s);
                    }

                    context.Employees.Add(employee);
                    context.SaveChanges(); // Lưu đồng thời Employee và các bản ghi liên kết trong DB
                }

                WpfHelpers.ShowInfo("Employee and skills added successfully!");
                RefreshGrid();
                ResetForm();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Database insert failed: {ex.Message}");
            }
        }

        private void ClearFormButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtFullName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtSalary.Text = string.Empty;
            
            if (cboDept.Items.Count > 0)
            {
                cboDept.SelectedIndex = 0;
            }

            var allSkills = lstSkills.ItemsSource as List<Skills>;
            if (allSkills != null)
            {
                foreach (var s in allSkills) s.IsChecked = false;
            }
            lstSkills.Items.Refresh(); // Yêu cầu vẽ lại giao diện checkbox
        }
    }
}
```
