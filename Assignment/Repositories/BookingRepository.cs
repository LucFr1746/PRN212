using System;
using System.Collections.Generic;
using Assignment.BusinessObjects;
using Assignment.DataAccess;

namespace Assignment.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        public List<BookingReservation> GetAllReservations() => BookingReservationDAO.Instance.GetAll();

        public BookingReservation? GetReservationById(int id) => BookingReservationDAO.Instance.GetById(id);

        public List<BookingReservation> GetReservationsByCustomerId(int customerId) => BookingReservationDAO.Instance.GetByCustomerId(customerId);

        public void AddReservation(BookingReservation reservation) => BookingReservationDAO.Instance.Add(reservation);

        public void UpdateReservation(BookingReservation reservation) => BookingReservationDAO.Instance.Update(reservation);

        public void DeleteReservation(int id) => BookingReservationDAO.Instance.Delete(id);

        public List<BookingDetail> GetReport(DateOnly startDate, DateOnly endDate) => BookingReservationDAO.Instance.GetReportDetails(startDate, endDate);

        public bool IsRoomOverlapping(int roomId, DateOnly startDate, DateOnly endDate, int? excludeReservationId = null) => BookingReservationDAO.Instance.IsRoomOverlapping(roomId, startDate, endDate, excludeReservationId);
    }
}
