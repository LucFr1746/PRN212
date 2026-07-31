using Microsoft.EntityFrameworkCore;
using Q2.Helpers;
using Q2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Linq;

namespace Q2
{
    public partial class MainWindow : Window
    {
        // TODO: Instantiate your service layers
        // private readonly EmployeeService _employeeService = new EmployeeService();
        // private readonly DepartmentService _departmentService = new DepartmentService();
        // private readonly SkillService _skillService = new SkillService();

        public MainWindow()
        {
            InitializeComponent();
            LoadInitData();
        }

        private void LoadInitData()
        {
            try
            {
                using (var context = new Prn21226sprB12Context())
                {
                    var categories = context.Categories.ToList();
                    var suppliers = context.Suppliers.ToList();
                    

                    cboFilterCategory.LoadWithDefault(categories, "CategoryName", "CategoryId", () => new Categories { CategoryId = 0, CategoryName = "All" });
                    cboFilterSupplier.LoadWithDefault(suppliers, "SupplierName", "SupplierId", () => new Suppliers { SupplierId = 0, SupplierName = "All" });


                    cboCategory.ItemsSource = categories;
                    cboCategory.DisplayMemberPath = "CategoryName";
                    cboCategory.SelectedValuePath = "CategoryId";
                    cboCategory.SelectedIndex = 0;

                    lstCheckedItems.ItemsSource = suppliers;
                }
                RefreshGrid();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Failed to load initial data: {ex.Message}");
            }
        }

        private void RefreshGrid()
        {
            using (var context = new Prn21226sprB12Context())
            {
                dgProduct.ItemsSource = context.Products
                    .Include(x => x.Category)
                    .ToList();
            }
        }

        // ==========================================
        // DYNAMIC FILTER IMPLEMENTATION
        // ==========================================
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int categoryId = Convert.ToInt32(cboFilterCategory.SelectedValue);
                int supplierId = Convert.ToInt32(cboFilterSupplier.SelectedValue);

                using (var context = new Prn21226sprB12Context())
                {
                    IQueryable<Products> query = context.Products
                        .Include(x => x.Category)
                        .Include(x => x.Supplier);

                    if (categoryId > 0)
                        query = query.Where(x => x.CategoryId == categoryId);

                    if (supplierId > 0)
                        query = query.Where(x => x.Supplier.Any(b => b.SupplierId == supplierId));

                    dgProduct.ItemsSource = query.ToList();
                }
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Filtering error: {ex.Message}");
            }
        }

        private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
        {
            // Reset dropdowns back to first index ("All")
            cboFilterCategory.SelectedIndex = 0;
            cboFilterSupplier.SelectedIndex = 0;
            RefreshGrid();
        }

        // ==========================================
        // VALIDATION & TRANSACTION INSERT
        // ==========================================
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            var name = txtName.Text.Trim();
            var price = txtPrice.Text.Trim();
            var stock = txtStock.Text.Trim();
            var categoryId = cboCategory.SelectedValue;

            // 2. Perform validations
            if (!ValidationHelpers.IsRequired(name))
            {
                WpfHelpers.ShowError("Name field is required.");
                return;
            }

            if (!ValidationHelpers.TryParseDecimal(price, out decimal priceValue) || priceValue <= 0)
            {
                WpfHelpers.ShowError("Price value must be a valid number greater than 0.");
                return;
            }

            if (!ValidationHelpers.TryParseInt(stock, out int stockValue) || stockValue <= 0)
            {
                WpfHelpers.ShowError("Stock value must be a valid number greater than 0.");
                return;
            }

            if (categoryId == null)
            {
                WpfHelpers.ShowError("Please select a valid category.");
                return;
            }

            // 3. Validate CheckedListBox
            var allPhu = lstCheckedItems.ItemsSource as List<Suppliers>;
            var selectedPhu = allPhu?.Where(x => x.IsChecked).ToList() ?? new List<Suppliers>();
            if (selectedPhu.Count == 0)
            {
                WpfHelpers.ShowError("Select at least one item."); return;
            }

                try
            {
                using (var context = new Prn21226sprB12Context())
                {
                    // 4. Tạo entity chính
                    var product = new Products
                    {
                        ProductName = name,
                        Price = priceValue,
                        Stock = stockValue,
                        CategoryId = Convert.ToInt32(categoryId)
                    };
                    context.Products.Add(product);
                    context.SaveChanges();

                    // 5. Tạo bản ghi bridge (many-to-many)
                    foreach (var phu in selectedPhu)
                    {
                        var dbSupplier = context.Suppliers.Find(phu.SupplierId);
                        if (dbSupplier != null)
                        {
                            product.Supplier.Add(dbSupplier);
                        }
                    }
                    context.SaveChanges();
                }

                WpfHelpers.ShowInfo("Added successfully!");
                RefreshGrid();
                ResetForm();
            }
            catch (Exception ex)
            {
                WpfHelpers.ShowError($"Database operation failed: {ex.Message}");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetForm();
        }

        private void ResetForm()
        {
            txtName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtStock.Text = string.Empty;
            cboCategory.SelectedIndex = 0;

            var allPhu = lstCheckedItems.ItemsSource as List<Suppliers>;
            if (allPhu != null) foreach (var x in allPhu) x.IsChecked = false;
            lstCheckedItems.Items.Refresh();
        }

        private void DgRecords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Optional: Map selected row to form TextBoxes for Updates
            // var selected = dgRecords.SelectedItem as Employee;
            // if (selected != null)
            // {
            //     txtName.Text = selected.FullName;
            //     txtNumericValue.Text = selected.Salary.ToString();
            //     cboCategory.SelectedValue = selected.DepartmentId;
            // }
        }
    }
}
