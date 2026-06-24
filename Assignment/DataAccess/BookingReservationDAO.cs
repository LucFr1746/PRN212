using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Assignment.DataAccess
{
    public class BookingReservationDAO
    {
        private static BookingReservationDAO? instance;
        private static readonly object instanceLock = new object();

        private BookingReservationDAO() { }

        public static BookingReservationDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingReservationDAO();
                    }
                    return instance;
                }
            }
        }

        public List<BookingReservation> GetAll()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingReservations
                    .Include(br => br.Customer)
                    .Include(br => br.BookingDetails)
                        .ThenInclude(bd => bd.Room)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving reservations: {ex.Message}");
            }
        }

        public BookingReservation? GetById(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingReservations
                    .Include(br => br.Customer)
                    .Include(br => br.BookingDetails)
                        .ThenInclude(bd => bd.Room)
                    .FirstOrDefault(br => br.BookingReservationId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving reservation: {ex.Message}");
            }
        }

        public List<BookingReservation> GetByCustomerId(int customerId)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingReservations
                    .Include(br => br.Customer)
                    .Include(br => br.BookingDetails)
                        .ThenInclude(bd => bd.Room)
                    .Where(br => br.CustomerId == customerId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving reservations for customer: {ex.Message}");
            }
        }

        public void Add(BookingReservation reservation)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                
                // Set the default reservation ID if it is 0 or check if it already exists
                // Note: database does not have identity on BookingReservationID (it was set to int NOT NULL, ValueGeneratedNever)
                // So we can generate a new ID dynamically: Max(ID) + 1
                if (reservation.BookingReservationId == 0)
                {
                    int maxId = context.BookingReservations.Max(br => (int?)br.BookingReservationId) ?? 0;
                    reservation.BookingReservationId = maxId + 1;
                }

                foreach (var detail in reservation.BookingDetails)
                {
                    detail.BookingReservationId = reservation.BookingReservationId;
                }

                context.BookingReservations.Add(reservation);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding reservation: {ex.Message}");
            }
        }

        public void Update(BookingReservation reservation)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                
                // Load existing reservation from DB to update details properly
                var existing = context.BookingReservations
                    .Include(br => br.BookingDetails)
                    .FirstOrDefault(br => br.BookingReservationId == reservation.BookingReservationId);

                if (existing != null)
                {
                    // Update main properties
                    existing.BookingDate = reservation.BookingDate;
                    existing.TotalPrice = reservation.TotalPrice;
                    existing.CustomerId = reservation.CustomerId;
                    existing.BookingStatus = reservation.BookingStatus;

                    // Remove existing details
                    context.BookingDetails.RemoveRange(existing.BookingDetails);

                    // Add new details
                    foreach (var detail in reservation.BookingDetails)
                    {
                        detail.BookingReservationId = reservation.BookingReservationId;
                        context.BookingDetails.Add(detail);
                    }

                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Reservation not found.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating reservation: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var reservation = context.BookingReservations.Find(id);
                if (reservation != null)
                {
                    context.BookingReservations.Remove(reservation);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting reservation: {ex.Message}");
            }
        }

        public List<BookingDetail> GetReportDetails(DateOnly startDate, DateOnly endDate)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.BookingDetails
                    .Include(bd => bd.BookingReservation)
                        .ThenInclude(br => br.Customer)
                    .Include(bd => bd.Room)
                    .Where(bd => bd.StartDate >= startDate && bd.EndDate <= endDate)
                    .OrderByDescending(bd => bd.StartDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving report: {ex.Message}");
            }
        }
    }
}
