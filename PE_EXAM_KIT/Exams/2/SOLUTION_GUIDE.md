# Hướng Dẫn Giải Đề 2 (Từng Bước Từ A-Z)

Tài liệu này hướng dẫn chi tiết các bước để giải quyết trọn vẹn Đề 2 sử dụng các công cụ và cấu trúc của **PRN212 Exam Kit**.

---

## Bước 1: Chuẩn Bị Cơ Sở Dữ Liệu & Scaffolding (10 Phút)

### 1. Khởi tạo Cơ sở dữ liệu trong SQL Server
1. Mở SSMS.
2. Mở file [script2.sql](file:///d:/Coding Space/FPT/PRN212/PE_EXAM_KIT/Exams/2/script2.sql) và chạy lệnh (`F5`). Cơ sở dữ liệu `PRN212_26SprB1_2` sẽ được tạo ra với các bảng: `Categories`, `Products`, `Suppliers`, và bảng liên kết `ProductSuppliers`.

### 2. Tạo file appsettings.json trong WPF Project
Tạo file `appsettings.json` tại thư mục gốc của dự án WPF `Q2`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.\\SQLEXPRESS;Database=PRN212_26SprB1_2;User Id=sa;Password=123123;TrustServerCertificate=True;"
  }
}
```
> [!IMPORTANT]
> Nhấp chuột phải vào `appsettings.json` -> chọn **Properties** -> thiết lập **Copy to Output Directory** là **Copy if newer**.

### 3. Thực hiện Scaffold Database First
Mở cửa sổ Terminal tại thư mục `Exams\2\Solution\Q2` (hoặc dự án WPF của đề thi) và chạy:
```bash
dotnet restore
dotnet ef dbcontext scaffold "Server=.\\SQLEXPRESS;Database=PRN212_26SprB1_2;User Id=sa;Password=123123;TrustServerCertificate=True" Microsoft.EntityFrameworkCore.SqlServer -o Models -f --no-pluralize
```

### 4. Tích hợp DbContext với Configuration Connection
Mở file `Models/Prn21226sprB12Context.cs` vừa sinh ra, thay thế thân hàm `OnConfiguring` để đọc động connection string:
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
            optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=PRN212_26SprB1_2;User Id=sa;Password=123123;TrustServerCertificate=True");
        }
    }
}
```

---

## Bước 2: Giải Quyết Câu 1 (Console App - OOP & Collections) (20 Phút)

Viết mã nguồn hoàn chỉnh cho file `Program.cs` của dự án Console `Q1` để xử lý tính giảm giá sản phẩm điện tử:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace Q1
{
    // 1. DELEGATE DECLARATION
    public delegate void DiscountAppliedCallback(string productName, double discountPercent);

    // 2. INTERFACE DECLARATION
    public interface IDiscountable
    {
        double Price { get; set; }
        void ApplyDiscount(double percent);
    }

    // 3. ABSTRACT CLASS PRODUCT
    public abstract class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        protected Product(int productId, string name, double price)
        {
            if (price <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(price), "Price must be greater than 0");
            }
            ProductId = productId;
            Name = name;
            Price = price;
        }

        public void ApplyDiscount(double percent)
        {
            if (percent <= 0 || percent > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(percent), "Discount percent must be between 0 and 100");
            }
            Price = Price * (1 - percent / 100);
        }

        public abstract string GetCategory();
    }

    // 4. CONCRETE CLASS ELECTRONICSPRODUCT
    public class ElectronicsProduct : Product, IDiscountable
    {
        public string Brand { get; set; }
        public int WarrantyMonths { get; set; }

        public ElectronicsProduct(int productId, string name, double price, string brand, int warrantyMonths)
            : base(productId, name, price)
        {
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }

        public override string GetCategory()
        {
            return "Electronics";
        }
    }

    // 5. DISCOUNT MANAGER CLASS
    public class DiscountManager
    {
        private readonly List<ElectronicsProduct> _products;
        private readonly DiscountAppliedCallback _onDiscountApplied;

        public DiscountManager(DiscountAppliedCallback callback)
        {
            _products = new List<ElectronicsProduct>();
            _onDiscountApplied = callback;
        }

        public void AddProduct(ElectronicsProduct product)
        {
            _products.Add(product);
        }

        public void ApplyDiscountToProduct(int productId, double percent)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product not found: {productId}");
            }

            product.ApplyDiscount(percent); // Throws exception if invalid percent
            _onDiscountApplied?.Invoke(product.Name, percent); // Trigger callback
        }

        public List<ElectronicsProduct> GetProductsUnder(double maxPrice)
        {
            return _products
                .Where(p => p.Price < maxPrice)
                .OrderBy(p => p.Price)
                .ToList();
        }
    }

    // 6. MAIN PROGRAM TESTING
    class Program
    {
        static void Main(string[] args)
        {
            // Define Callback
            DiscountAppliedCallback callback = (name, percent) =>
                Console.WriteLine($"[DISCOUNT]: {name} received {percent:F1}% discount");

            DiscountManager manager = new DiscountManager(callback);

            // 1. Create electronics products
            var p1 = new ElectronicsProduct(1, "Laptop Pro", 1111.11, "Dell", 12);
            var p2 = new ElectronicsProduct(2, "Wireless Headphones", 150.00, "Sony", 6);
            var p3 = new ElectronicsProduct(3, "Smart Watch", 299.99, "Garmin", 24);

            manager.AddProduct(p1);
            manager.AddProduct(p2);
            manager.AddProduct(p3);

            // 2. Apply discount with exception handling
            try
            {
                manager.ApplyDiscountToProduct(1, 10.0);
                manager.ApplyDiscountToProduct(2, 15.0);
                manager.ApplyDiscountToProduct(3, 20.0);

                // This triggers ArgumentOutOfRangeException
                manager.ApplyDiscountToProduct(2, 150.0);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                string message = ex.Message.Split('\n')[0];
                if (message.Contains("Discount percent must be between 0 and 100"))
                {
                    Console.WriteLine("Error: Discount percent must be between 0 and 100");
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

            // 3. Print Products Under 500.0
            Console.WriteLine("\n=== Products Under 500.0 ===");
            var under = manager.GetProductsUnder(500.0);
            int rank = 1;
            foreach (var product in under)
            {
                Console.WriteLine($"{rank++}. {product.Name,-20} | Brand: {product.Brand,-5} | Price: {product.Price:F2} | Category: {product.GetCategory()}");
            }
        }
    }
}
```

---

## Bước 3: Tích Hợp Thư Viện Kit & Binding Hack (5 Phút)

### 1. Sao chép công cụ dùng chung vào thư mục Helpers
Tạo thư mục `Helpers` trong dự án WPF `Q2` và sao chép các file sau từ Kit:
*   `Kits/Foundation/ConfigurationHelper.cs`
*   `Kits/Common/ComboBoxExtensions.cs`
*   `Kits/Common/ValidationHelpers.cs`
*   `Kits/Common/WpfHelpers.cs`

### 2. Thiết lập Partial Entity Binding Hack cho checked list
Tạo file `PartialEntities.cs` trong thư mục `Models` của `Q2` để thêm trường tích chọn cho thực thể `Suppliers`:

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

## Bước 4: Thiết Kế Giao Diện MainWindow.xaml (15 Phút)

Cấu hình giao diện `MainWindow.xaml` của `Q2` với bố cục chia 3 vùng chuẩn hóa:

```xml
<Window x:Class="Q2.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="MainWindow" Height="650" Width="850" WindowStartupLocation="CenterScreen">
    <Grid Margin="10">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <!-- ZONE 1: FILTER AREA -->
        <GroupBox Header="FILTER AREA" Grid.Row="0" Margin="0,0,0,10" Padding="10">
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="Category:" VerticalAlignment="Center" Margin="0,0,5,0"/>
                <ComboBox x:Name="cboFilterCategory" Width="150" Margin="0,0,20,0"/>

                <TextBlock Text="Supplier:" VerticalAlignment="Center" Margin="0,0,5,0"/>
                <ComboBox x:Name="cboFilterSupplier" Width="150" Margin="0,0,20,0"/>

                <Button Content="Filter" Click="FilterButton_Click" Width="80" Margin="0,0,5,0" Background="#10B981" Foreground="White" FontWeight="Bold"/>
                <Button Content="Clear" Click="ClearFilterButton_Click" Width="80" Background="#6B7280" Foreground="White"/>
            </StackPanel>
        </GroupBox>

        <!-- ZONE 2: PRODUCT LIST DATA GRID -->
        <GroupBox Header="PRODUCT LIST" Grid.Row="1" Margin="0,0,0,10">
            <DataGrid x:Name="dgProducts" AutoGenerateColumns="False" IsReadOnly="True" AlternatingRowBackground="#F9FAFB" GridLinesVisibility="Horizontal">
                <DataGrid.Columns>
                    <DataGridTextColumn Header="ID" Binding="{Binding ProductId}" Width="100"/>
                    <DataGridTextColumn Header="Product Name" Binding="{Binding ProductName}" Width="2*"/>
                    <DataGridTextColumn Header="Price" Binding="{Binding Price, StringFormat={}{0:N2}}" Width="1.2*"/>
                    <DataGridTextColumn Header="Stock" Binding="{Binding Stock}" Width="1.2*"/>
                    <DataGridTextColumn Header="Category" Binding="{Binding Category.CategoryName}" Width="1.5*"/>
                </DataGrid.Columns>
            </DataGrid>
        </GroupBox>

        <!-- ZONE 3: ADD NEW PRODUCT FORM -->
        <GroupBox Header="ADD NEW PRODUCT" Grid.Row="2" Padding="10">
            <Grid>
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="2*"/>
                    <ColumnDefinition Width="1*"/>
                </Grid.ColumnDefinitions>

                <!-- Left Column: Primary Fields -->
                <StackPanel Grid.Column="0" Margin="0,0,15,0">
                    <Grid Margin="0,5">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="120"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        <TextBlock Text="Product Name:" VerticalAlignment="Center"/>
                        <TextBox x:Name="txtProductName" Grid.Column="1" Padding="4"/>
                    </Grid>

                    <Grid Margin="0,5">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="120"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        <TextBlock Text="Price:" VerticalAlignment="Center"/>
                        <TextBox x:Name="txtPrice" Grid.Column="1" Padding="4"/>
                    </Grid>

                    <Grid Margin="0,5">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="120"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        <TextBlock Text="Stock:" VerticalAlignment="Center"/>
                        <TextBox x:Name="txtStock" Grid.Column="1" Padding="4"/>
                    </Grid>

                    <Grid Margin="0,5">
                        <Grid.ColumnDefinitions>
                            <ColumnDefinition Width="120"/>
                            <ColumnDefinition Width="*"/>
                        </Grid.ColumnDefinitions>
                        <TextBlock Text="Category:" VerticalAlignment="Center"/>
                        <ComboBox x:Name="cboCategory" Grid.Column="1" Padding="4"/>
                    </Grid>
                </StackPanel>

                <!-- Right Column: Suppliers Checklist -->
                <Grid Grid.Column="1">
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="*"/>
                    </Grid.RowDefinitions>
                    <TextBlock Text="Suppliers selection (select at least 1):" FontWeight="Bold" Margin="0,0,0,5"/>
                    <ListBox x:Name="lstSuppliers" Grid.Row="1" Height="110" BorderThickness="1" BorderBrush="#D1D5DB" SelectionMode="Multiple">
                        <ListBox.ItemTemplate>
                            <DataTemplate>
                                <CheckBox Content="{Binding SupplierName}" IsChecked="{Binding IsChecked, Mode=TwoWay}" Margin="2"/>
                            </DataTemplate>
                        </ListBox.ItemTemplate>
                    </ListBox>
                </Grid>

                <!-- Command Row -->
                <StackPanel Grid.ColumnSpan="2" Orientation="Horizontal" HorizontalAlignment="Right" Margin="0,15,0,0">
                    <Button Content="Add Product" Click="AddProductButton_Click" Width="130" Margin="0,0,5,0" Padding="6" Background="#2563EB" Foreground="White" FontWeight="Bold" BorderThickness="0"/>
                    <Button Content="Clear Form" Click="ClearFormButton_Click" Width="100" Padding="6" Background="#9CA3AF" Foreground="White" BorderThickness="0"/>
                </StackPanel>
            </Grid>
        </GroupBox>
    </Grid>
</Window>
```

---

## Bước 5: Viết Code Xử Lý MainWindow.xaml.cs (25 Phút)

Sao chép toàn bộ logic xử lý nghiệp vụ cho file `MainWindow.xaml.cs` của `Q2`:

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
                using (var context = new Prn21226sprB12Context())
                {
                    // 1. Tải danh mục Categories và nhà cung cấp Suppliers
                    var categories = context.Categories.ToList();
                    var suppliers = context.Suppliers.ToList();

                    // 2. Load bộ lọc cbo có chèn chữ "All" bằng ComboBoxExtensions
                    cboFilterCategory.LoadWithDefault(
                        categories, 
                        "CategoryName", 
                        "CategoryId", 
                        () => new Categories { CategoryId = 0, CategoryName = "All" }
                    );

                    cboFilterSupplier.LoadWithDefault(
                        suppliers, 
                        "SupplierName", 
                        "SupplierId", 
                        () => new Suppliers { SupplierId = 0, SupplierName = "All" }
                    );

                    // 3. Load ComboBox trong form thêm mới
                    cboCategory.ItemsSource = categories;
                    cboCategory.DisplayMemberPath = "CategoryName";
                    cboCategory.SelectedValuePath = "CategoryId";
                    cboCategory.SelectedIndex = 0;

                    // 4. Load Checklist nhà cung cấp
                    lstSuppliers.ItemsSource = suppliers;
                }

                RefreshGrid();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to load data: {ex.Message}");
            }
        }

        private void RefreshGrid()
        {
            try
            {
                using (var context = new Prn21226sprB12Context())
                {
                    // Tải danh sách Product kèm quan hệ liên kết Category
                    dgProducts.ItemsSource = context.Products
                        .Include(p => p.Category)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to refresh grid: {ex.Message}");
            }
        }

        // ==========================================
        // LỌC ĐỒNG THỜI CATEGORY & SUPPLIER
        // ==========================================
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int selectedCategoryVal = Convert.ToInt32(cboFilterCategory.SelectedValue);
                int selectedSupplierVal = Convert.ToInt32(cboFilterSupplier.SelectedValue);

                using (var context = new Prn21226sprB12Context())
                {
                    IQueryable<Products> query = context.Products
                        .Include(p => p.Category)
                        .Include(p => p.Supplier);

                    // A. Lọc theo Category
                    if (selectedCategoryVal > 0)
                    {
                        query = query.Where(p => p.CategoryId == selectedCategoryVal);
                    }

                    // B. Lọc theo Supplier
                    if (selectedSupplierVal > 0)
                    {
                        query = query.Where(p => p.Supplier.Any(s => s.SupplierId == selectedSupplierVal));
                    }

                    dgProducts.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Filter failed: {ex.Message}");
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            cboFilterCategory.SelectedIndex = 0;
            cboFilterSupplier.SelectedIndex = 0;
            RefreshGrid();
        }

        // ==========================================
        // THÊM MỚI PRODUCT KÈM LIÊN KẾT NHIỀU - NHIỀU
        // ==========================================
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            string productName = txtProductName.Text.Trim();
            string priceText = txtPrice.Text.Trim();
            string stockText = txtStock.Text.Trim();
            object categoryVal = cboCategory.SelectedValue;

            // 1. Kiểm tra dữ liệu hợp lệ
            if (!ValidationHelpers.IsRequired(productName))
            {
                WpfHelpers.ShowError("Product name is required.");
                return;
            }

            if (!ValidationHelpers.TryParseDecimal(priceText, out decimal price) || price <= 0)
            {
                WpfHelpers.ShowError("Price must be a valid positive number.");
                return;
            }

            if (!ValidationHelpers.TryParseInt(stockText, out int stock) || stock < 0)
            {
                WpfHelpers.ShowError("Stock must be a non-negative integer.");
                return;
            }

            if (categoryVal == null)
            {
                WpfHelpers.ShowError("Please select a category.");
                return;
            }

            // 2. Kiểm tra checklist
            var allSuppliers = lstSuppliers.ItemsSource as List<Suppliers>;
            var selectedSuppliers = allSuppliers?.Where(s => s.IsChecked).ToList() ?? new List<Suppliers>();

            if (selectedSuppliers.Count == 0)
            {
                WpfHelpers.ShowError("Please select at least 1 supplier.");
                return;
            }

            try
            {
                using (var context = new Prn21226sprB12Context())
                {
                    // 3. Khởi tạo đối tượng Product mới
                    var product = new Products
                    {
                        ProductName = productName,
                        Price = price,
                        Stock = stock,
                        CategoryId = Convert.ToInt32(categoryVal)
                    };

                    // 4. Attach các nhà cung cấp được chọn và gán quan hệ liên kết
                    foreach (var s in selectedSuppliers)
                    {
                        context.Entry(s).State = EntityState.Unchanged; // Báo cho EF các Suppliers này đã có sẵn trong DB
                        product.Supplier.Add(s);
                    }

                    context.Products.Add(product);
                    context.SaveChanges(); // Lưu đồng thời sản phẩm và các bản ghi bảng trung gian ProductSuppliers
                }

                WpfHelpers.ShowInfo("Product and suppliers added successfully!");
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
            txtProductName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtStock.Text = string.Empty;
            
            if (cboCategory.Items.Count > 0)
            {
                cboCategory.SelectedIndex = 0;
            }

            var allSuppliers = lstSuppliers.ItemsSource as List<Suppliers>;
            if (allSuppliers != null)
            {
                foreach (var s in allSuppliers) s.IsChecked = false;
            }
            lstSuppliers.Items.Refresh(); // Vẽ lại giao diện checkbox
        }
    }
}
```
