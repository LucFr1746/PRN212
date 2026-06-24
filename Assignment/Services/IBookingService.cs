using System;
using System.Collections.Generic;
using Assignment.BusinessObjects;

namespace Assignment.Services
{
    public interface IBookingService
    {
        List<BookingReservation> GetAllReservations();
        BookingReservation? GetReservationById(int id);
        List<BookingReservation> GetReservationsByCustomerId(int customerId);
        void AddReservation(BookingReservation reservation);
        void UpdateReservation(BookingReservation reservation);
        void DeleteReservation(int id);
        List<BookingDetail> GetReport(DateOnly startDate, DateOnly endDate);

        /// <summary>
        /// Validates a booking detail before adding it to a reservation.
        /// Checks date range validity, past dates, local duplicate/overlap, and database overlap.
        /// Throws an Exception with a descriptive message if validation fails.
        /// </summary>
        void ValidateBookingDetail(BookingDetail detail, IEnumerable<BookingDetail> currentDetails, int? excludeReservationId = null);

        /// <summary>
        /// Checks whether a room has overlapping bookings in the database for the given date range.
        /// </summary>
        bool IsRoomOverlapping(int roomId, DateOnly startDate, DateOnly endDate, int? excludeReservationId = null);
    }
}
