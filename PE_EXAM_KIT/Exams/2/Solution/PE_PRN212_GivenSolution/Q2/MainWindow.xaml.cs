using Q2.Helpers;
using Q2.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Q2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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

                    // Filter ComboBox có "All"
                    cboFilterCategory.LoadWithDefault(categories, "CategoryName", "CategoryId",
                        () => new Categories { CategoryId = 0, CategoryName = "All" });
                    cboFilterSupplier.LoadWithDefault(suppliers, "SupplierName", "SupplierId",
                        () => new Suppliers { SupplierId = 0, SupplierName = "All" });

                    // Form ComboBox (không có "All")
                    cboCategory.ItemsSource = categories;
                    cboCategory.DisplayMemberPath = "CategoryName";
                    cboCategory.SelectedValuePath = "CategoryId";
                    cboCategory.SelectedIndex = 0;

                    // CheckedListBox
                    lstSuppliers.ItemsSource = suppliers;
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
                dgProducts.ItemsSource = context.Products
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

                    dgProducts.ItemsSource = query.ToList();
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
            // 1. Extract inputs
            var productName = txtProductName.Text.Trim();
            var price = txtPrice.Text.Trim();
            var stock = txtStock.Text.Trim();
            var categoryId = cboCategory.SelectedValue;

            // 2. Validate
            if (!ValidationHelpers.IsRequired(productName)) { WpfHelpers.ShowError("Tên is required."); return; }
            if (!ValidationHelpers.TryParseDecimal(price, out decimal priceValue) || priceValue <= 0)
            { WpfHelpers.ShowError("Invalid number."); return; }
            if (!ValidationHelpers.TryParseInt(stock, out int stockValue) || stockValue < 0)
            { WpfHelpers.ShowError("Invalid stock number."); return; }
            if (categoryId == null) { WpfHelpers.ShowError("Select a category."); return; }

            // 3. Validate CheckedListBox (Bỏ qua bước này nếu đề chỉ có 1-N, không có CheckedListBox)
            var allPhu = lstSuppliers.ItemsSource as List<Suppliers>;
            var selectedPhu = allPhu?.Where(x => x.IsChecked).ToList() ?? new List<Suppliers>();
            if (selectedPhu.Count == 0) { WpfHelpers.ShowError("Select at least one item."); return; }

            try
            {
                using (var context = new Prn21226sprB12Context())
                {
                    var entity = new Products
                    {
                        ProductName = productName,
                        Price = priceValue,
                        Stock = stockValue,
                        CategoryId = Convert.ToInt32(categoryId)
                    };

                    // =========================================================
                    // DẠNG A: Quan hệ N-N Trực tiếp (KHÔNG CÓ class Bridge ghép như Đề 3)
                    // =========================================================
                    foreach (var phu in selectedPhu)
                    {
                        var dbPhu = context.Suppliers.Find(phu.SupplierId);
                        if (dbPhu != null)
                        {
                            entity.Supplier.Add(dbPhu); // EF Core tự chèn N-N vào SQL
                        }
                    }
                    context.Products.Add(entity);
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
            txtProductName.Text = string.Empty;
            txtPrice.Text = string.Empty;
            txtStock.Text = string.Empty;
            cboCategory.SelectedIndex = 0;

            var allPhu = lstSuppliers.ItemsSource as List<Suppliers>;
            if (allPhu != null) foreach (var x in allPhu) x.IsChecked = false;
            lstSuppliers.Items.Refresh();
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