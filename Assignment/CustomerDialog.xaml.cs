using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Assignment.BusinessObjects;
using Assignment.Services;

namespace Assignment
{
    public partial class CustomerDialog : Window
    {
        private readonly ICustomerService _customerService;
        private readonly Customer? _existingCustomer;

        public CustomerDialog(Customer? customer = null)
        {
            InitializeComponent();
            _customerService = new CustomerService();
            _existingCustomer = customer;

            if (_existingCustomer != null)
            {
                // Edit Mode
                txtTitle.Text = "Edit Customer Details";
                LoadCustomerData(_existingCustomer);
                txtEmail.IsReadOnly = true;
                txtEmail.Background = System.Windows.Media.Brushes.LightGray;
            }
            else
            {
                // Add Mode
                txtTitle.Text = "Add New Customer";
                cbStatus.SelectedIndex = 0; // Default to Active
            }
        }

        private void LoadCustomerData(Customer customer)
        {
            txtFullName.Text = customer.CustomerFullName;
            txtPhone.Text = customer.Telephone;
            txtEmail.Text = customer.EmailAddress;
            txtPassword.Text = customer.Password;
            if (customer.CustomerBirthday.HasValue)
            {
                dpBirthday.SelectedDate = customer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue);
            }

            foreach (ComboBoxItem item in cbStatus.Items)
            {
                if (item.Tag.ToString() == customer.CustomerStatus?.ToString())
                {
                    cbStatus.SelectedItem = item;
                    break;
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateInput(out string errorMessage))
            {
                MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var customer = _existingCustomer ?? new Customer();
                customer.CustomerFullName = txtFullName.Text.Trim();
                customer.Telephone = txtPhone.Text.Trim();
                customer.EmailAddress = txtEmail.Text.Trim();
                customer.Password = txtPassword.Text.Trim();

                if (dpBirthday.SelectedDate.HasValue)
                {
                    customer.CustomerBirthday = DateOnly.FromDateTime(dpBirthday.SelectedDate.Value);
                }
                else
                {
                    customer.CustomerBirthday = null;
                }

                if (cbStatus.SelectedItem is ComboBoxItem selectedItem && byte.TryParse(selectedItem.Tag.ToString(), out byte statusValue))
                {
                    customer.CustomerStatus = statusValue;
                }

                if (_existingCustomer == null)
                {
                    _customerService.AddCustomer(customer);
                    MessageBox.Show("Customer added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _customerService.UpdateCustomer(customer);
                    MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInput(out string message)
        {
            message = string.Empty;

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                message = "Full Name cannot be empty.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text) || !Regex.IsMatch(txtPhone.Text.Trim(), @"^\d{9,12}$"))
            {
                message = "Telephone must be a valid number of 9 to 12 digits.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                message = "Please enter a valid email address.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                message = "Password cannot be empty.";
                return false;
            }

            return true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
