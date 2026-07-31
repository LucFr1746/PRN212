# Bảng Tra Cứu Copy-Paste Nhanh Cho Đề 2

Tài liệu này chứa các đoạn code ăn liền được định cấu hình riêng cho **Đề thi 2**, giúp bạn copy-paste và chạy chương trình trong vòng vài phút.

---

## 1. Lệnh Scaffold Database First (Dành Cho Đề 2)
Mở Terminal tại thư mục của dự án WPF `Q2` và chạy:
```bash
dotnet restore
dotnet ef dbcontext scaffold "Server=.\\SQLEXPRESS;Database=PRN212_26SprB1_2;User Id=sa;Password=123123;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -f --no-pluralize
```

---

## 2. File appsettings.json Cấu Hình Kết Nối (Đề 2)
Tạo file `appsettings.json` tại thư mục gốc dự án WPF:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=PRN212_26SprB1_2;User Id=sa;Password=123123;TrustServerCertificate=True;"
  }
}
```
*(Đừng quên chọn Properties -> **Copy to Output Directory** = **Copy if newer**).*

---

## 3. Khai Báo Biến Phụ Tích Chọn (Đề 2)
Tạo file `Models/PartialEntities.cs` trong dự án WPF:
```csharp
using System.ComponentModel.DataAnnotations.Schema;

namespace Q2.Models
{
    public partial class Suppliers
    {
        [NotMapped]
        public bool IsChecked { get; set; }
    }
}
```

---

## 4. Giao Diện CheckedListBox (Đề 2)
Thêm ListBox này vào phần XAML của `MainWindow.xaml`:
```xml
<ListBox x:Name="lstSuppliers" SelectionMode="Multiple" Height="110">
    <ListBox.ItemTemplate>
        <DataTemplate>
            <CheckBox Content="{Binding SupplierName}" IsChecked="{Binding IsChecked, Mode=TwoWay}" Margin="2"/>
        </DataTemplate>
    </ListBox.ItemTemplate>
</ListBox>
```

---

## 5. Tải Dữ Liệu Bộ Lọc Cbo Có Chữ "All" (Đề 2)
Dán code này vào phần khởi tạo `LoadData()` trong `MainWindow.xaml.cs`:
```csharp
using Q2.Helpers;

// Tải Categories
cboFilterCategory.LoadWithDefault(
    context.Categories.ToList(), 
    "CategoryName", 
    "CategoryId", 
    () => new Categories { CategoryId = 0, CategoryName = "All" }
);

// Tải Suppliers
cboFilterSupplier.LoadWithDefault(
    context.Suppliers.ToList(), 
    "SupplierName", 
    "SupplierId", 
    () => new Suppliers { SupplierId = 0, SupplierName = "All" }
);
```

---

## 6. Đoạn Mã Lọc Đồng Thời Đề 2 (Multi-Filter)
Dán vào nút sự kiện Filter:
```csharp
int selectedCategoryVal = Convert.ToInt32(cboFilterCategory.SelectedValue);
int selectedSupplierVal = Convert.ToInt32(cboFilterSupplier.SelectedValue);

using (var context = new Prn21226sprB12Context())
{
    IQueryable<Products> query = context.Products
        .Include(p => p.Category)
        .Include(p => p.Supplier);

    if (selectedCategoryVal > 0)
    {
        query = query.Where(p => p.CategoryId == selectedCategoryVal);
    }

    if (selectedSupplierVal > 0)
    {
        query = query.Where(p => p.Supplier.Any(s => s.SupplierId == selectedSupplierVal));
    }

    dgProducts.ItemsSource = query.ToList();
}
```

---

## 7. Giao Dịch Lưu Sản Phẩm Nhiều Bảng (Transaction Insert)
Dán vào nút sự kiện Add Product:
```csharp
// 1. Kiểm tra
var productName = txtProductName.Text.Trim();
if (cboCategory.SelectedValue == null) return;

// 2. Lọc danh sách nhà cung cấp tích chọn
var allSuppliers = lstSuppliers.ItemsSource as List<Suppliers>;
var selectedSuppliers = allSuppliers?.Where(s => s.IsChecked).ToList() ?? new List<Suppliers>();

// 3. Thực hiện lưu
using (var context = new Prn21226sprB12Context())
{
    var product = new Products
    {
        ProductName = productName,
        Price = Convert.ToDecimal(txtPrice.Text.Trim()),
        Stock = Convert.ToInt32(txtStock.Text.Trim()),
        CategoryId = Convert.ToInt32(cboCategory.SelectedValue)
    };

    foreach (var s in selectedSuppliers)
    {
        context.Entry(s).State = EntityState.Unchanged; // Tránh tạo nhà cung cấp mới
        product.Supplier.Add(s); // EF Core tự động tạo bản ghi trong bảng liên kết ProductSuppliers
    }

    context.Products.Add(product);
    context.SaveChanges();
}
```
