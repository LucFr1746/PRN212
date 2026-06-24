using System;
using System.Windows;
using System.Windows.Controls;
using Assignment.BusinessObjects;
using Assignment.Services;

namespace Assignment
{
    public partial class RoomDialog : Window
    {
        private readonly IRoomService _roomService;
        private readonly IRoomTypeService _roomTypeService;
        private readonly RoomInformation? _existingRoom;

        public RoomDialog(RoomInformation? room = null)
        {
            InitializeComponent();
            _roomService = new RoomService();
            _roomTypeService = new RoomTypeService();
            _existingRoom = room;

            LoadRoomTypes();

            if (_existingRoom != null)
            {
                // Edit Mode
                txtTitle.Text = "Edit Room Details";
                LoadRoomData(_existingRoom);
            }
            else
            {
                // Add Mode
                txtTitle.Text = "Add New Room";
                cbStatus.SelectedIndex = 0; // Default to Active
            }
        }

        private void LoadRoomTypes()
        {
            try
            {
                var roomTypes = _roomTypeService.GetAllRoomTypes();
                cbRoomType.ItemsSource = roomTypes;
                if (roomTypes.Count > 0)
                {
                    cbRoomType.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load room types: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadRoomData(RoomInformation room)
        {
            txtRoomNumber.Text = room.RoomNumber;
            txtCapacity.Text = room.RoomMaxCapacity?.ToString();
            txtPrice.Text = room.RoomPricePerDay?.ToString("0.##");
            txtDescription.Text = room.RoomDetailDescription;
            cbRoomType.SelectedValue = room.RoomTypeId;

            foreach (ComboBoxItem item in cbStatus.Items)
            {
                if (item.Tag.ToString() == room.RoomStatus?.ToString())
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
                var room = _existingRoom ?? new RoomInformation();
                room.RoomNumber = txtRoomNumber.Text.Trim();
                room.RoomTypeId = (int)cbRoomType.SelectedValue;
                room.RoomMaxCapacity = int.Parse(txtCapacity.Text.Trim());
                room.RoomPricePerDay = decimal.Parse(txtPrice.Text.Trim());
                room.RoomDetailDescription = txtDescription.Text.Trim();

                if (cbStatus.SelectedItem is ComboBoxItem selectedItem && byte.TryParse(selectedItem.Tag.ToString(), out byte statusValue))
                {
                    room.RoomStatus = statusValue;
                }

                if (_existingRoom == null)
                {
                    _roomService.AddRoom(room);
                    MessageBox.Show("Room added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _roomService.UpdateRoom(room);
                    MessageBox.Show("Room updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
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

            if (string.IsNullOrWhiteSpace(txtRoomNumber.Text))
            {
                message = "Room Number cannot be empty.";
                return false;
            }

            if (cbRoomType.SelectedValue == null)
            {
                message = "Please select a Room Type.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtCapacity.Text) || !int.TryParse(txtCapacity.Text.Trim(), out int capacity) || capacity <= 0)
            {
                message = "Max Capacity must be a positive integer.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPrice.Text) || !decimal.TryParse(txtPrice.Text.Trim(), out decimal price) || price < 0)
            {
                message = "Price Per Day must be a non-negative decimal value.";
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
