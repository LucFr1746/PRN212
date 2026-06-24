using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Assignment.BusinessObjects;
using Assignment.Services;

namespace Assignment
{
    public partial class MainWindow : Window
    {
        private readonly string _role;
        private readonly Customer? _currentCustomer;

        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly IBookingService _bookingService;

        public MainWindow(string role, Customer? customer)
        {
            InitializeComponent();
            _role = role;
            _currentCustomer = customer;

            _customerService = new CustomerService();
            _roomService = new RoomService();
            _bookingService = new BookingService();

            InitializeLayout();
        }

        private void InitializeLayout()
        {
            if (_role == "Admin")
            {
                txtWelcome.Text = "Welcome, Admin";
                adminTabControl.Visibility = Visibility.Visible;
                customerTabControl.Visibility = Visibility.Collapsed;

                // Load Admin grids
                LoadAllCustomers();
                LoadAllRooms();
                LoadAllBookings();
                
                // Initialize default date range for report (last 30 days)
                dpStart.SelectedDate = DateTime.Today.AddDays(-30);
                dpEnd.SelectedDate = DateTime.Today;
                GenerateReport();
            }
            else if (_role == "Customer" && _currentCustomer != null)
            {
                txtWelcome.Text = $"Welcome, {_currentCustomer.CustomerFullName}";
                adminTabControl.Visibility = Visibility.Collapsed;
                customerTabControl.Visibility = Visibility.Visible;

                // Load Customer Data
                LoadCustomerProfile();
                LoadCustomerBookingHistory();
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }

        #region ADMIN - CUSTOMER MANAGEMENT

        private void LoadAllCustomers()
        {
            try
            {
                dgCustomers.ItemsSource = _customerService.GetAllCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearchCustomer.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadAllCustomers();
                return;
            }

            try
            {
                var filtered = _customerService.GetAllCustomers()
                    .Where(c => (c.CustomerFullName != null && c.CustomerFullName.Contains(keyword, StringComparison.OrdinalIgnoreCase)) ||
                                c.EmailAddress.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                dgCustomers.ItemsSource = filtered;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClearCustomer_Click(object sender, RoutedEventArgs e)
        {
            txtSearchCustomer.Clear();
            LoadAllCustomers();
        }

        private void BtnAddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CustomerDialog();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                LoadAllCustomers();
            }
        }

        private void BtnEditCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selectedCustomer)
            {
                var dialog = new CustomerDialog(selectedCustomer);
                dialog.Owner = this;
                if (dialog.ShowDialog() == true)
                {
                    LoadAllCustomers();
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to edit.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDeleteCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem is Customer selectedCustomer)
            {
                var result = MessageBox.Show($"Are you sure you want to delete customer {selectedCustomer.CustomerFullName}?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _customerService.DeleteCustomer(selectedCustomer.CustomerId);
                        MessageBox.Show("Customer deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadAllCustomers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Delete failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region ADMIN - ROOM MANAGEMENT

        private void LoadAllRooms()
        {
            try
            {
                dgRooms.ItemsSource = _roomService.GetAllRooms();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading rooms: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSearchRoom_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearchRoom.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadAllRooms();
                return;
            }

            try
            {
                var filtered = _roomService.GetAllRooms()
                    .Where(r => r.RoomNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                dgRooms.ItemsSource = filtered;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClearRoom_Click(object sender, RoutedEventArgs e)
        {
            txtSearchRoom.Clear();
            LoadAllRooms();
        }

        private void BtnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new RoomDialog();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                LoadAllRooms();
            }
        }

        private void BtnEditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (dgRooms.SelectedItem is RoomInformation selectedRoom)
            {
                var dialog = new RoomDialog(selectedRoom);
                dialog.Owner = this;
                if (dialog.ShowDialog() == true)
                {
                    LoadAllRooms();
                }
            }
            else
            {
                MessageBox.Show("Please select a room to edit.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (dgRooms.SelectedItem is RoomInformation selectedRoom)
            {
                var result = MessageBox.Show($"Are you sure you want to delete Room {selectedRoom.RoomNumber}?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _roomService.DeleteRoom(selectedRoom.RoomId);
                        MessageBox.Show("Room deletion/status update completed successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadAllRooms();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Delete failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a room to delete.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region ADMIN - BOOKING RESERVATION MANAGEMENT

        private void LoadAllBookings()
        {
            try
            {
                dgBookings.ItemsSource = _bookingService.GetAllReservations();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookings: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSearchBooking_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearchBooking.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                LoadAllBookings();
                return;
            }

            try
            {
                var filtered = _bookingService.GetAllReservations()
                    .Where(b => b.Customer.EmailAddress.Contains(keyword, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                dgBookings.ItemsSource = filtered;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Search error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnClearBooking_Click(object sender, RoutedEventArgs e)
        {
            txtSearchBooking.Clear();
            LoadAllBookings();
        }

        private void BtnAddBooking_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new BookingDialog();
            dialog.Owner = this;
            if (dialog.ShowDialog() == true)
            {
                LoadAllBookings();
            }
        }

        private void BtnEditBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItem is BookingReservation selectedBooking)
            {
                // Retrieve the full reservation details (including nested details/rooms) from DB
                var fullReservation = _bookingService.GetReservationById(selectedBooking.BookingReservationId);
                if (fullReservation != null)
                {
                    var dialog = new BookingDialog(fullReservation);
                    dialog.Owner = this;
                    if (dialog.ShowDialog() == true)
                    {
                        LoadAllBookings();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation to edit.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItem is BookingReservation selectedBooking)
            {
                var result = MessageBox.Show($"Are you sure you want to delete reservation ID {selectedBooking.BookingReservationId}?", 
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _bookingService.DeleteReservation(selectedBooking.BookingReservationId);
                        MessageBox.Show("Reservation deleted successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadAllBookings();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Delete failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a reservation to delete.", "Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        #endregion

        #region ADMIN - STATISTICS REPORT

        private void BtnGenerateReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateReport();
        }

        private void GenerateReport()
        {
            if (!dpStart.SelectedDate.HasValue || !dpEnd.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select both start and end dates.", "Date Selection Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var start = DateOnly.FromDateTime(dpStart.SelectedDate.Value);
            var end = DateOnly.FromDateTime(dpEnd.SelectedDate.Value);

            if (start > end)
            {
                MessageBox.Show("Start Date cannot be after End Date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var details = _bookingService.GetReport(start, end);
                dgReport.ItemsSource = details;

                // Total calculations
                int totalBookings = details.Select(d => d.BookingReservationId).Distinct().Count();
                decimal totalRevenue = details.Sum(d => d.ActualPrice ?? 0);

                txtTotalBookings.Text = totalBookings.ToString();
                txtTotalRevenue.Text = totalRevenue.ToString("C0");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Report generation error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region CUSTOMER - PROFILE MANAGEMENT

        private void LoadCustomerProfile()
        {
            if (_currentCustomer == null)
            {
                return;
            }

            try
            {
                // Reload fresh copy from DB to make sure details are accurate
                var customer = _customerService.GetCustomerById(_currentCustomer.CustomerId);
                if (customer != null)
                {
                    txtCustName.Text = customer.CustomerFullName;
                    txtCustPhone.Text = customer.Telephone;
                    txtCustEmail.Text = customer.EmailAddress;
                    txtCustPassword.Text = customer.Password;
                    if (customer.CustomerBirthday.HasValue)
                    {
                        dpCustBirthday.SelectedDate = customer.CustomerBirthday.Value.ToDateTime(TimeOnly.MinValue);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading profile: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnUpdateProfile_Click(object sender, RoutedEventArgs e)
        {
            if (_currentCustomer == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCustName.Text))
            {
                MessageBox.Show("Full Name cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCustPhone.Text))
            {
                MessageBox.Show("Telephone cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtCustPassword.Text))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var customer = _customerService.GetCustomerById(_currentCustomer.CustomerId);
                if (customer != null)
                {
                    customer.CustomerFullName = txtCustName.Text.Trim();
                    customer.Telephone = txtCustPhone.Text.Trim();
                    customer.Password = txtCustPassword.Text.Trim();
                    if (dpCustBirthday.SelectedDate.HasValue)
                    {
                        customer.CustomerBirthday = DateOnly.FromDateTime(dpCustBirthday.SelectedDate.Value);
                    }
                    else
                    {
                        customer.CustomerBirthday = null;
                    }

                    _customerService.UpdateCustomer(customer);
                    MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadCustomerProfile();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Update failed: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region CUSTOMER - BOOKING HISTORY

        private void LoadCustomerBookingHistory()
        {
            if (_currentCustomer == null)
            {
                return;
            }

            try
            {
                var history = _bookingService.GetReservationsByCustomerId(_currentCustomer.CustomerId);
                dgCustHistory.ItemsSource = history;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading booking history: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region RELOAD BUTTON EVENT HANDLERS

        private void BtnReloadCustomer_Click(object sender, RoutedEventArgs e)
        {
            txtSearchCustomer.Clear();
            LoadAllCustomers();
        }

        private void BtnReloadRoom_Click(object sender, RoutedEventArgs e)
        {
            txtSearchRoom.Clear();
            LoadAllRooms();
        }

        private void BtnReloadBooking_Click(object sender, RoutedEventArgs e)
        {
            txtSearchBooking.Clear();
            LoadAllBookings();
        }

        private void BtnReloadProfile_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomerProfile();
        }

        private void BtnReloadCustHistory_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomerBookingHistory();
        }

        #endregion
    }
}