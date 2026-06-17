using System;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Services;

namespace WPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IProductService iProductService;
        private readonly ICategoryService iCategoryService;

        public MainWindow()
        {
            InitializeComponent();
            iProductService = new ProductService();
            iCategoryService = new CategoryService();
        }

        public void LoadCategoryList()
        {
            try
            {
                var catList = iCategoryService.GetCategories();
                cboCategory.ItemsSource = catList;
                cboCategory.DisplayMemberPath = "CategoryName";
                cboCategory.SelectedValuePath = "CategoryId";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error on load list of categories");
            }
        }

        public void LoadProductList()
        {
            try
            {
                var productList = iProductService.GetProducts();
                dgData.ItemsSource = productList;
            }
            catch (Exception)
            {
                // Commented out as in the slide screenshot
                // MessageBox.Show(ex.Message, "Error on load list of products");
            }
            finally
            {
                resetInput();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCategoryList();
            LoadProductList();
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Product product = new Product
                {
                    ProductName = txtProductName.Text,
                    UnitPrice = string.IsNullOrEmpty(txtPrice.Text) ? null : decimal.Parse(txtPrice.Text),
                    UnitsInStock = string.IsNullOrEmpty(txtUnitsInStock.Text) ? null : short.Parse(txtUnitsInStock.Text),
                    CategoryId = cboCategory.SelectedValue != null ? (int)cboCategory.SelectedValue : null
                };
                iProductService.SaveProduct(product);
                LoadProductList();
                MessageBox.Show("Create successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Create Product");
            }
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgData.SelectedItem is Product selectedProduct)
            {
                txtProductID.Text = selectedProduct.ProductId.ToString();
                txtProductName.Text = selectedProduct.ProductName;
                txtPrice.Text = selectedProduct.UnitPrice?.ToString("F4");
                txtUnitsInStock.Text = selectedProduct.UnitsInStock?.ToString();
                cboCategory.SelectedValue = selectedProduct.CategoryId;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtProductID.Text))
                {
                    MessageBox.Show("Please select a product!");
                    return;
                }
                Product product = new Product
                {
                    ProductId = int.Parse(txtProductID.Text),
                    ProductName = txtProductName.Text,
                    UnitPrice = string.IsNullOrEmpty(txtPrice.Text) ? null : decimal.Parse(txtPrice.Text),
                    UnitsInStock = string.IsNullOrEmpty(txtUnitsInStock.Text) ? null : short.Parse(txtUnitsInStock.Text),
                    CategoryId = cboCategory.SelectedValue != null ? (int)cboCategory.SelectedValue : null
                };
                iProductService.UpdateProduct(product);
                LoadProductList();
                MessageBox.Show("Update successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Update Product");
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtProductID.Text))
                {
                    MessageBox.Show("Please select a product!");
                    return;
                }

                if (dgData.SelectedItem is Product selectedProduct)
                {
                    iProductService.DeleteProduct(selectedProduct);
                    LoadProductList();
                    MessageBox.Show("Delete successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Delete Product");
            }
        }

        private void resetInput()
        {
            txtProductID.Text = "";
            txtProductName.Text = "";
            txtPrice.Text = "";
            txtUnitsInStock.Text = "";
            cboCategory.SelectedValue = null;
        }
    }
}