using System;
using System.Collections.Generic;
using Assignment.BusinessObjects;

namespace Assignment.Repositories
{
    public interface IBookingRepository
    {
        List<BookingReservation> GetAllReservations();
        BookingReservation? GetReservationById(int id);
        List<BookingReservation> GetReservationsByCustomerId(int customerId);
        void AddReservation(BookingReservation reservation);
        void UpdateReservation(BookingReservation reservation);
        void DeleteReservation(int id);
        List<BookingDetail> GetReport(DateOnly startDate, DateOnly endDate);
    }
}
