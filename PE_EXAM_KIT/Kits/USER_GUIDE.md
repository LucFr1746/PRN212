# PRN212 Exam Kit: Hướng Dẫn Sử Dụng & Áp Dụng Cho Đề Thi

Tài liệu này hướng dẫn cách nhanh chóng thiết lập **PRN212 Exam Kit** và áp dụng các module dùng chung, template và các đoạn code mẫu (snippets) để giải quyết các dạng đề thi thực hành trong vòng 85 phút.

---

## Phần 1: Thiết Lập Nhanh Giải Pháp (10 Phút Đầu)

Thực hiện đúng trình tự sau ngay khi bắt đầu giờ thi:

### 1. Khởi Tạo Cơ Sở Dữ Liệu
1. Mở SQL Server Management Studio (SSMS).
2. Mở file script `.sql` được cung cấp trong thư mục đề thi và nhấn `F5` để tạo cơ sở dữ liệu.
3. Quan sát kỹ cấu trúc các bảng và khóa chính/khóa ngoại để hiểu các thực thể nghiệp vụ (ví dụ: `Department`, `Employee`, `Skill`, `EmployeeSkills`).

### 2. Scaffold Database First
Mở **Package Manager Console** trong Visual Studio. Chọn dự án chứa database layer của bạn (thường là dự án Class Library `DataAccess` hoặc dự án `WpfApp` nếu là single-project) và chạy lệnh:
```powershell
Scaffold-DbContext "Server=.;Database=DB_NAME;Trusted_Connection=True;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutDir Models -Force -NoPluralize
```
*Lưu ý: Thay thế `DB_NAME` bằng tên cơ sở dữ liệu thực tế vừa tạo.*

### 3. Cấu Hình appsettings.json & Kết Nối DbContext
1. Tạo một file tên là `appsettings.json` tại thư mục gốc của dự án WPF App và thiết lập chuỗi kết nối:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.;Database=DB_NAME;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```
2. **Nhấp chuột phải vào file `appsettings.json`** -> **Properties** -> Chọn **Copy to Output Directory** là **Copy if newer**.
3. Mở file `Models/DBContext.cs` đã được sinh ra sau khi scaffold. Tìm hàm `OnConfiguring` và thay thế toàn bộ nội dung hàm bằng đoạn mã sau:
   ```csharp
   protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
   {
       if (!optionsBuilder.IsConfigured)
       {
           optionsBuilder.UseSqlServer(PRN212.ExamKit.Foundation.ConfigurationHelper.GetConnectionString("DefaultConnection"));
       }
   }
   ```

### 4. Import Các Công Cụ Dùng Chung
Sao chép các file sau từ thư mục Kit vào dự án của bạn (sửa lại namespace ở dòng đầu của mỗi file cho trùng khớp với namespace dự án của bạn):
*   `Foundation/ConfigurationHelper.cs`
*   `Foundation/GenericDAO.cs`
*   `Common/ComboBoxExtensions.cs`
*   `Common/ValidationHelpers.cs`
*   `Common/WpfHelpers.cs`

---

## Phần 2: Áp Dụng Cho Câu 1 (Console App - OOP & Collections)

Câu 1 thường chiếm từ 3.5 đến 4 điểm và cực kỳ chuẩn hóa. Hãy sử dụng mẫu [ConsoleOOPTemplate.cs](file:///d:/Coding Space/FPT/PRN212/PE_EXAM_KIT/Kits/Snippets/ConsoleOOPTemplate.cs) làm khung mã nguồn cơ bản.

### Dạng đề A: Quản Lý Điểm Số / Giảm Giá Sản Phẩm (Đề 1-4)
1.  **Khai báo Delegate:** Sao chép khai báo delegate tương ứng từ template.
2.  **Lớp Trừu Tượng (Abstract Class):** Kế thừa cấu trúc lớp từ template. Sử dụng `ArgumentOutOfRangeException` trong kiểm tra điều kiện đầu vào của điểm hoặc giá trị phần trăm.
3.  **Lớp Con Thực Tế (Concrete Class) & Giao Diện (Interface):** Áp dụng triển khai interface. Tính điểm trung bình sử dụng LINQ: `Scores.Average()`.
4.  **Lớp Quản Lý (Manager Class):** Thiết lập danh sách và gọi delegate callback:
    ```csharp
    public void ApplyAction(int id, double value)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);
        if (item == null) throw new InvalidOperationException("Item not found");
        
        item.Process(value); // Throws ArgumentOutOfRangeException if invalid
        _callback?.Invoke(item.Name, value); // Trigger delegate callback
    }
    ```

### Dạng đề B: Kho Lưu Trữ Dữ Liệu Generic (Đề 5)
Nếu đề bài yêu cầu tạo lớp lưu trữ generic `DataVault<T>`, sao chép toàn bộ lớp `DataVault<T>` từ file `ConsoleOOPTemplate.cs`. Lớp này đã có sẵn:
*   `Add(T item)`
*   `Remove(T item)`
*   `FindAll(Func<T, bool> predicate)` -> Sử dụng: `Items.Where(predicate)`
*   `Count()` -> Sử dụng: `Items.Count`

---

## Phần 3: Áp Dụng Cho Câu 2 (WPF App - CRUD & Filtering)

Lựa chọn mẫu phù hợp dựa trên yêu cầu kiến trúc của đề bài:

### 1. Giao Diện Nhập Dữ Liệu Có Danh Sách Checkbox (Đề 1-4)
Áp dụng cho các đề bài hiển thị DataGrid danh sách chính, bảng lọc bằng ComboBox ở trên và form thêm mới ở dưới có CheckedListBox chọn nhiều:

1.  **Thiết kế Giao diện (UI Layout):** Sao chép grid layout từ file [CrudWindowTemplate.xaml](file:///d:/Coding Space/FPT/PRN212/PE_EXAM_KIT/Kits/Templates/CrudWindowTemplate.xaml) vào file `MainWindow.xaml` của bạn.
2.  **Đưa Dữ Liệu Vào ComboBox Lọc Với Lựa Chọn "All":**
    Gọi hàm mở rộng `LoadWithDefault` tại code-behind khi khởi tạo cửa sổ:
    ```csharp
    cboFilterCategory.LoadWithDefault(
        _categoryService.GetAll(), 
        "CategoryName", 
        "CategoryId", 
        () => new Category { CategoryId = 0, CategoryName = "All" }
    );
    ```
3.  **Liên Kết CheckedListBox Bằng Mẹo Partial Class:**
    *   Tạo file partial class trùng tên thực thể phụ (ví dụ: `Skill` hoặc `Supplier`) và khai báo thuộc tính `IsChecked` (xem chi tiết ở [CheckedListBoxGuide.md](file:///d:/Coding Space/FPT/PRN212/PE_EXAM_KIT/Kits/Common/CheckedListBoxGuide.md)).
    *   Bắt đầu bind danh sách này vào ListBox trong WPF.
4.  **Thực Hiện Lọc Đồng Thời Nhiều ComboBox:**
    Xử lý truy vấn kết hợp có điều kiện trong sự kiện Click của nút Filter sử dụng [LinqQueries.md](file:///d:/Coding Space/FPT/PRN212/PE_EXAM_KIT/Kits/Snippets/LinqQueries.md):
    ```csharp
    int catId = Convert.ToInt32(cboFilterCategory.SelectedValue);
    int supId = Convert.ToInt32(cboFilterSupplier.SelectedValue);
    dgRecords.ItemsSource = _employeeService.Filter(catId, supId);
    ```
5.  **Thêm Mới Bản Ghi Liên Kết Nhiều Bảng (Transaction Insert):**
    Khi nhấn nút thêm, kiểm tra dữ liệu bằng `ValidationHelpers`, duyệt qua các checkbox được tích chọn trong ListBox, lưu bản ghi cha để lấy ID tự sinh, sau đó lưu các bản ghi liên kết vào bảng trung gian (xem mã mẫu tại [CrudWindowTemplate.xaml.cs](file:///d:/Coding Space/FPT/PRN212/PE_EXAM_KIT/Kits/Templates/CrudWindowTemplate.xaml.cs)).

---

### 2. Quản Lý Đăng Nhập & Chỉnh Sửa Trực Tiếp Trên DataGrid (Đề 5)
Áp dụng cho đề bài yêu cầu màn hình đăng nhập, thanh điều hướng và DataGrid sửa đổi trực tiếp:

1.  **Điều Hướng Đăng Nhập:**
    Sử dụng [LoginWindowTemplate.xaml.cs](file:///d:/Coding Space/FPT/PRN212/PE_EXAM_KIT/Kits/Templates/LoginWindowTemplate.xaml.cs) để kiểm tra tài khoản mật khẩu và chuyển hướng:
    ```csharp
    var user = _studentService.Login(txtUsername.Text, txtPassword.Password);
    if (user != null) {
        MainWindow main = new MainWindow(user); // Transfer user session to main shell
        WpfHelpers.NavigateTo(this, main);
    }
    ```
2.  **Bố Cục MainWindow:**
    Khai báo một `ContentControl` trong layout của `MainWindow`, và thực hiện hoán đổi các UserControl khi nhấn nút menu:
    ```csharp
    private void SubjectMenu_Click(object sender, RoutedEventArgs e)
    {
        mainContentArea.Content = new SubjectManagementView();
    }
    ```
3.  **Thao Tác Thêm/Sửa/Xóa Trực Tiếp Trên Grid và Lưu Hàng Loạt:**
    Thiết lập DataGrid với thuộc tính `CanUserAddRows="True"` và `CanUserDeleteRows="True"`.
    Áp dụng theo hướng dẫn trong file [InlineGridCrudTemplate.md](file:///d:/Coding Space/FPT/PRN212/PE_EXAM_KIT/Kits/Snippets/InlineGridCrudTemplate.md):
    ```csharp
    // Bắt đầu theo dõi và bind trực tiếp vào danh sách local của EF Tracker
    _context.Subjects.Load();
    dgSubjects.ItemsSource = _context.Subjects.Local.ToObservableCollection();

    // Lưu toàn bộ chỉnh sửa, thêm mới, xóa dòng chỉ bằng một lệnh duy nhất
    private void SaveChanges_Click(object sender, RoutedEventArgs e) {
        _context.SaveChanges();
    }
    ```
