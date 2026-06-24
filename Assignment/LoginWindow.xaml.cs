using System;
using System.IO;
using System.Windows;
using Assignment.BusinessObjects;
using Assignment.Services;
using Microsoft.Extensions.Configuration;

namespace Assignment
{
    public partial class LoginWindow : Window
    {
        private readonly ICustomerService _customerService;

        public LoginWindow()
        {
            InitializeComponent();
            _customerService = new CustomerService();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both email and password.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                // 1. Check against Admin account in appsettings.json
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .Build();

                string adminEmail = configuration["AdminAccount:Email"] ?? "admin@FUMiniHotelSystem.com";
                string adminPassword = configuration["AdminAccount:Password"] ?? "@@abc123@@";

                if (email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) && password == adminPassword)
                {
                    // Open MainWindow as Admin
                    var mainWindow = new MainWindow("Admin", null);
                    mainWindow.Show();
                    this.Close();
                    return;
                }

                // 2. Check against Database Customers
                var customer = _customerService.Login(email, password);
                if (customer != null)
                {
                    // Open MainWindow as Customer
                    var mainWindow = new MainWindow("Customer", customer);
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid email, password, or the account is inactive.", "Login Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during login: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
