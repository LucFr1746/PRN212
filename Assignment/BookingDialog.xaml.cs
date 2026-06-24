using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Assignment.BusinessObjects;
using Assignment.Services;

namespace Assignment
{
    public partial class BookingDialog : Window
    {
        private readonly IBookingService _bookingService;
        private readonly ICustomerService _customerService;
        private readonly IRoomService _roomService;
        private readonly BookingReservation? _existingReservation;
        
        // Use ObservableCollection to auto-update DataGrid on adding/removing details
        private ObservableCollection<BookingDetail> _detailsCollection;

        public BookingDialog(BookingReservation? reservation = null)
        {
            InitializeComponent();
            _bookingService = new BookingService();
            _customerService = new CustomerService();
            _roomService = new RoomService();
            _existingReservation = reservation;
            _detailsCollection = new ObservableCollection<BookingDetail>();

            dgDetails.ItemsSource = _detailsCollection;

            LoadDropdowns();

            if (_existingReservation != null)
            {
                // Edit Mode
                LoadReservationData(_existingReservation);
            }
            else
            {
                // Add Mode
                dpBookingDate.SelectedDate = DateTime.Today;
                cbStatus.SelectedIndex = 0;
                RecalculateTotal();
            }
        }

        private void LoadDropdowns()
        {
            try
            {
                cbCustomer.ItemsSource = _customerService.GetAllCustomers();
                cbRoom.ItemsSource = _roomService.GetAllRooms().Where(r => r.RoomStatus == 1).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading options: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadReservationData(BookingReservation reservation)
        {
            cbCustomer.SelectedValue = reservation.CustomerId;
            cbStatus.SelectedIndex = reservation.BookingStatus == 1 ? 0 : 1;
            
            if (reservation.BookingDate.HasValue)
            {
                dpBookingDate.SelectedDate = reservation.BookingDate.Value.ToDateTime(TimeOnly.MinValue);
            }

            foreach (var detail in reservation.BookingDetails)
            {
                _detailsCollection.Add(detail);
            }

            RecalculateTotal();
        }

        private void CbRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbRoom.SelectedItem is RoomInformation selectedRoom)
            {
                txtActualPrice.Text = selectedRoom.RoomPricePerDay?.ToString("0.##") ?? "0";
            }
        }

        private void BtnAddDetail_Click(object sender, RoutedEventArgs e)
        {
            if (cbRoom.SelectedItem is not RoomInformation selectedRoom)
            {
                MessageBox.Show("Please select a room.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!dpStart.SelectedDate.HasValue || !dpEnd.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select both start date and end date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var start = DateOnly.FromDateTime(dpStart.SelectedDate.Value);
            var end = DateOnly.FromDateTime(dpEnd.SelectedDate.Value);

            if (start >= end)
            {
                MessageBox.Show("Start Date must be before End Date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(txtActualPrice.Text.Trim(), out decimal actualPrice) || actualPrice < 0)
            {
                MessageBox.Show("Please enter a valid price.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Check duplicate room in details
            if (_detailsCollection.Any(d => d.RoomId == selectedRoom.RoomId))
            {
                MessageBox.Show("This room is already added to this reservation.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Add detail to collection
            var detail = new BookingDetail
            {
                RoomId = selectedRoom.RoomId,
                Room = selectedRoom,
                StartDate = start,
                EndDate = end,
                ActualPrice = actualPrice
            };

            _detailsCollection.Add(detail);
            RecalculateTotal();

            // Clear inputs
            cbRoom.SelectedIndex = -1;
            txtActualPrice.Clear();
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;
        }

        private void BtnRemoveDetail_Click(object sender, RoutedEventArgs e)
        {
            if (dgDetails.SelectedItem is BookingDetail selectedDetail)
            {
                _detailsCollection.Remove(selectedDetail);
                RecalculateTotal();
            }
            else
            {
                MessageBox.Show("Please select a detail to remove.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RecalculateTotal()
        {
            decimal total = _detailsCollection.Sum(d => d.ActualPrice ?? 0);
            txtTotalPrice.Text = total.ToString("C0");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cbCustomer.SelectedValue == null)
            {
                MessageBox.Show("Please select a Customer.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!dpBookingDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select a Booking Date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_detailsCollection.Count == 0)
            {
                MessageBox.Show("Please add at least one Room to the reservation.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var reservation = _existingReservation ?? new BookingReservation();
                reservation.CustomerId = (int)cbCustomer.SelectedValue;
                reservation.BookingDate = DateOnly.FromDateTime(dpBookingDate.SelectedDate.Value);
                reservation.TotalPrice = _detailsCollection.Sum(d => d.ActualPrice ?? 0);

                if (cbStatus.SelectedItem is ComboBoxItem selectedItem && byte.TryParse(selectedItem.Tag.ToString(), out byte statusValue))
                {
                    reservation.BookingStatus = statusValue;
                }

                // Copy details
                reservation.BookingDetails.Clear();
                foreach (var detail in _detailsCollection)
                {
                    // Clear navigation property before saving to avoid duplicate inserts
                    var detailToSave = new BookingDetail
                    {
                        RoomId = detail.RoomId,
                        StartDate = detail.StartDate,
                        EndDate = detail.EndDate,
                        ActualPrice = detail.ActualPrice
                    };
                    reservation.BookingDetails.Add(detailToSave);
                }

                if (_existingReservation == null)
                {
                    _bookingService.AddReservation(reservation);
                    MessageBox.Show("Reservation added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _bookingService.UpdateReservation(reservation);
                    MessageBox.Show("Reservation updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Saving", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
