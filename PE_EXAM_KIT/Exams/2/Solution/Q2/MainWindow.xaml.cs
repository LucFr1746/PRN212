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