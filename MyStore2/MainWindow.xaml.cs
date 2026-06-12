using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using MyStore2.Data;
using MyStore2.Models;

namespace MyStore2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyStoreContext? _context;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await InitializeDatabaseAsync();
        }

        private async Task InitializeDatabaseAsync()
        {
            SetDbStatus("Connecting to database...", System.Windows.Media.Brushes.Orange);
            
            try
            {
                _context = new MyStoreContext();
                
                // Perform EF Core database initialization & seeding in a background task
                await Task.Run(() =>
                {
                    DbInitializer.Initialize(_context);
                });
                
                SetDbStatus("Connected (sa)", System.Windows.Media.Brushes.Green);
                
                // Populate interface lists
                LoadCategories();
                LoadProducts();
            }
            catch (Exception ex)
            {
                SetDbStatus("Connection Error!", System.Windows.Media.Brushes.Red);
                MessageBox.Show(
                    $"Unable to establish a database connection or execute seeding.\n\n" +
                    $"Error Details: {ex.Message}\n\n" +
                    $"Please ensure SQL Server is running locally, your authentication mode allows SA login, " +
                    $"and credentials (username: sa, password: 123123) match.",
                    "Database Connection Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error
                );
            }
        }

        private void SetDbStatus(string statusText, System.Windows.Media.Brush color)
        {
            Dispatcher.Invoke(() =>
            {
                lblDbStatus.Text = statusText;
                elDbStatus.Fill = color;
            });
        }

        // ==========================================
        // CATEGORY SECTION METHODS
        // ==========================================

        private void LoadCategories()
        {
            try
            {
                if (_context == null) return;
                
                var categories = _context.Categories.ToList();
                
                // Update ListBox
                lstCategories.ItemsSource = categories;
                
                // Update Product editor ComboBox
                cmbProductCategory.ItemsSource = categories;
                
                // Update Category Filter ComboBox (Insert "All Categories" as default option)
                var filterCategories = new List<Category>
                {
                    new Category { CategoryId = 0, CategoryName = "-- All Categories --" }
                };
                filterCategories.AddRange(categories);
                cmbCategoryFilter.ItemsSource = filterCategories;
                
                // Prevent triggering unnecessary filter queries while updating items
                cmbCategoryFilter.SelectionChanged -= cmbCategoryFilter_SelectionChanged;
                cmbCategoryFilter.SelectedIndex = 0;
                cmbCategoryFilter.SelectionChanged += cmbCategoryFilter_SelectionChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void lstCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstCategories.SelectedItem is Category selectedCategory)
            {
                txtCategoryId.Text = selectedCategory.CategoryId.ToString();
                txtCategoryName.Text = selectedCategory.CategoryName;
                txtCategoryDescription.Text = selectedCategory.Description;
            }
            else
            {
                ClearCategoryFields();
            }
        }

        private void ClearCategoryFields()
        {
            txtCategoryId.Text = "[Auto Generated]";
            txtCategoryName.Text = string.Empty;
            txtCategoryDescription.Text = string.Empty;
            lstCategories.SelectedItem = null;
        }

        private void btnCategoryAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_context == null) return;
                
                string name = txtCategoryName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Category Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Check duplicate category name
                if (_context.Categories.Any(c => c.CategoryName.ToLower() == name.ToLower()))
                {
                    MessageBox.Show($"Category '{name}' already exists.", "Duplicate Category Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var category = new Category
                {
                    CategoryName = name,
                    Description = txtCategoryDescription.Text.Trim()
                };

                _context.Categories.Add(category);
                _context.SaveChanges();

                MessageBox.Show("Category successfully created.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
                ClearCategoryFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCategoryUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_context == null) return;
                if (lstCategories.SelectedItem is not Category selectedCategory)
                {
                    MessageBox.Show("Please select a category from the list to update.", "No Category Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string name = txtCategoryName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Category Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Check duplicate name for other categories
                if (_context.Categories.Any(c => c.CategoryId != selectedCategory.CategoryId && c.CategoryName.ToLower() == name.ToLower()))
                {
                    MessageBox.Show($"Another category with the name '{name}' already exists.", "Duplicate Category Name", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedCategory.CategoryName = name;
                selectedCategory.Description = txtCategoryDescription.Text.Trim();

                _context.Categories.Update(selectedCategory);
                _context.SaveChanges();

                MessageBox.Show("Category updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // Track currently selected Category index to restore if needed
                int index = lstCategories.SelectedIndex;
                LoadCategories();
                ClearCategoryFields();
                LoadProducts(); // Refresh Grid to update matching category details
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCategoryDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_context == null) return;
                if (lstCategories.SelectedItem is not Category selectedCategory)
                {
                    MessageBox.Show("Please select a category from the list to delete.", "No Category Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var choice = MessageBox.Show(
                    $"Are you sure you want to delete the category '{selectedCategory.CategoryName}'?\n\n" +
                    $"WARNING: Deleting this category will delete all associated products!",
                    "Confirm Cascade Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                if (choice == MessageBoxResult.Yes)
                {
                    _context.Categories.Remove(selectedCategory);
                    _context.SaveChanges();

                    MessageBox.Show("Category and its products deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCategories();
                    ClearCategoryFields();
                    LoadProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCategoryClear_Click(object sender, RoutedEventArgs e)
        {
            ClearCategoryFields();
        }

        // ==========================================
        // PRODUCT SECTION METHODS
        // ==========================================

        private void LoadProducts()
        {
            try
            {
                if (_context == null) return;

                IQueryable<Product> query = _context.Products.Include(p => p.Category);

                // Category Filter
                if (cmbCategoryFilter.SelectedValue is int categoryId && categoryId > 0)
                {
                    query = query.Where(p => p.CategoryId == categoryId);
                }

                // Search Query Filter
                string searchText = txtProductSearch.Text.Trim();
                if (!string.IsNullOrEmpty(searchText))
                {
                    query = query.Where(p => p.ProductName.Contains(searchText));
                }

                var products = query.ToList();
                dgProducts.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgProducts.SelectedItem is Product selectedProduct)
            {
                txtProductId.Text = selectedProduct.ProductId.ToString();
                txtProductName.Text = selectedProduct.ProductName;
                txtProductPrice.Text = selectedProduct.Price.ToString("F2");
                txtProductQuantity.Text = selectedProduct.Quantity.ToString();
                cmbProductCategory.SelectedValue = selectedProduct.CategoryId;
            }
            else
            {
                ClearProductFields();
            }
        }

        private void ClearProductFields()
        {
            txtProductId.Text = "[Auto Generated]";
            txtProductName.Text = string.Empty;
            txtProductPrice.Text = string.Empty;
            txtProductQuantity.Text = string.Empty;
            cmbProductCategory.SelectedIndex = -1;
            dgProducts.SelectedItem = null;
        }

        private void btnProductAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_context == null) return;

                string name = txtProductName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Product Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(txtProductPrice.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Price must be a valid non-negative number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtProductQuantity.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Quantity must be a valid non-negative integer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbProductCategory.SelectedValue is not int categoryId)
                {
                    MessageBox.Show("Please assign a valid Category for this product.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var product = new Product
                {
                    ProductName = name,
                    Price = price,
                    Quantity = quantity,
                    CategoryId = categoryId
                };

                _context.Products.Add(product);
                _context.SaveChanges();

                MessageBox.Show("Product added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadProducts();
                ClearProductFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnProductUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_context == null) return;
                if (dgProducts.SelectedItem is not Product selectedProduct)
                {
                    MessageBox.Show("Please select a product from the grid to update.", "No Product Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string name = txtProductName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Product Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!decimal.TryParse(txtProductPrice.Text, out decimal price) || price < 0)
                {
                    MessageBox.Show("Price must be a valid non-negative number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(txtProductQuantity.Text, out int quantity) || quantity < 0)
                {
                    MessageBox.Show("Quantity must be a valid non-negative integer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (cmbProductCategory.SelectedValue is not int categoryId)
                {
                    MessageBox.Show("Please select an associated Category.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                selectedProduct.ProductName = name;
                selectedProduct.Price = price;
                selectedProduct.Quantity = quantity;
                selectedProduct.CategoryId = categoryId;

                _context.Products.Update(selectedProduct);
                _context.SaveChanges();

                MessageBox.Show("Product details updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadProducts();
                ClearProductFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnProductDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_context == null) return;
                if (dgProducts.SelectedItem is not Product selectedProduct)
                {
                    MessageBox.Show("Please select a product from the grid to delete.", "No Product Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var choice = MessageBox.Show($"Delete product '{selectedProduct.ProductName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (choice == MessageBoxResult.Yes)
                {
                    _context.Products.Remove(selectedProduct);
                    _context.SaveChanges();

                    MessageBox.Show("Product deleted.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadProducts();
                    ClearProductFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnProductClear_Click(object sender, RoutedEventArgs e)
        {
            ClearProductFields();
        }

        // ==========================================
        // FILTER & SEARCH HANDLERS
        // ==========================================

        private void txtProductSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadProducts();
        }

        private void cmbCategoryFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadProducts();
        }

        private void btnResetFilters_Click(object sender, RoutedEventArgs e)
        {
            txtProductSearch.Text = string.Empty;
            cmbCategoryFilter.SelectionChanged -= cmbCategoryFilter_SelectionChanged;
            cmbCategoryFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectionChanged += cmbCategoryFilter_SelectionChanged;
            LoadProducts();
        }
    }
}