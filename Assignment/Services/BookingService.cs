using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.BusinessObjects;
using Assignment.Repositories;

namespace Assignment.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingService()
        {
            _bookingRepository = new BookingRepository();
        }

        public List<BookingReservation> GetAllReservations() => _bookingRepository.GetAllReservations();

        public BookingReservation? GetReservationById(int id) => _bookingRepository.GetReservationById(id);

        public List<BookingReservation> GetReservationsByCustomerId(int customerId) => _bookingRepository.GetReservationsByCustomerId(customerId);

        public void AddReservation(BookingReservation reservation) => _bookingRepository.AddReservation(reservation);

        public void UpdateReservation(BookingReservation reservation) => _bookingRepository.UpdateReservation(reservation);

        public void DeleteReservation(int id) => _bookingRepository.DeleteReservation(id);

        public List<BookingDetail> GetReport(DateOnly startDate, DateOnly endDate) => _bookingRepository.GetReport(startDate, endDate);

        public bool IsRoomOverlapping(int roomId, DateOnly startDate, DateOnly endDate, int? excludeReservationId = null)
            => _bookingRepository.IsRoomOverlapping(roomId, startDate, endDate, excludeReservationId);

        public void ValidateBookingDetail(BookingDetail detail, IEnumerable<BookingDetail> currentDetails, int? excludeReservationId = null)
        {
            // 1. Date range: StartDate must be strictly before EndDate (minimum 1-day stay)
            if (detail.StartDate >= detail.EndDate)
            {
                throw new Exception("Start Date must be before End Date (minimum 1-day stay).");
            }

            // 2. No past dates: StartDate must be today or later
            if (detail.StartDate < DateOnly.FromDateTime(DateTime.Today))
            {
                throw new Exception("Start Date cannot be in the past.");
            }

            // 3. Local duplicate check: same room with overlapping dates within the current dialog
            bool localOverlap = currentDetails.Any(d =>
                d.RoomId == detail.RoomId &&
                d.StartDate < detail.EndDate &&
                d.EndDate > detail.StartDate
            );
            if (localOverlap)
            {
                throw new Exception("This room already has an overlapping date range in the current reservation.");
            }

            // 4. Database overlap check: verify against persisted bookings
            if (IsRoomOverlapping(detail.RoomId, detail.StartDate, detail.EndDate, excludeReservationId))
            {
                throw new Exception("This room is already booked for the selected dates in another reservation.");
            }
        }
    }
}

