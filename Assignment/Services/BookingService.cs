using System;
using System.Collections.Generic;
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
    }
}
