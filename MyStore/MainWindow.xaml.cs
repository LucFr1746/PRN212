using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MyStore.Entities;

namespace MyStore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                using (var context = new MyStoreContext())
                {
                    dgCategories.ItemsSource = context.Categories.ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load categories: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            txtCategoryID.Text = string.Empty;
            txtCategoryName.Text = string.Empty;
            dgCategories.SelectedItem = null;
        }

        private void dgCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCategories.SelectedItem is Category selectedCategory)
            {
                txtCategoryID.Text = selectedCategory.CategoryId.ToString();
                txtCategoryName.Text = selectedCategory.CategoryName;
            }
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = txtCategoryName.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Category Name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new MyStoreContext())
                {
                    var newCategory = new Category
                    {
                        CategoryName = categoryName
                    };

                    if (context.Categories.Contains(newCategory)) {
                        MessageBox.Show("Category already exists.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    context.Categories.Add(newCategory);
                    context.SaveChanges();
                }

                MessageBox.Show("Category inserted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadCategories();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to insert category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCategoryID.Text))
            {
                MessageBox.Show("Please select a category to update.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int categoryId = int.Parse(txtCategoryID.Text);
            string categoryName = txtCategoryName.Text.Trim();

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Category Name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = new MyStoreContext())
                {
                    var category = context.Categories.Find(categoryId);
                    if (category != null)
                    {
                        category.CategoryName = categoryName;
                        context.SaveChanges();
                        MessageBox.Show("Category updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Category not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                LoadCategories();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCategoryID.Text))
            {
                MessageBox.Show("Please select a category to delete.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int categoryId = int.Parse(txtCategoryID.Text);

            var result = MessageBox.Show("Are you sure you want to delete this category?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            try
            {
                using (var context = new MyStoreContext())
                {
                    var category = context.Categories.Find(categoryId);
                    if (category != null)
                    {
                        context.Categories.Remove(category);
                        context.SaveChanges();
                        MessageBox.Show("Category deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Category not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                LoadCategories();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete category: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}