using System;
using System.Collections.Generic;
using System.Data;
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

        /// <summary>
        /// Checks whether a room has overlapping bookings in the database for the given date range.
        /// Uses the interval overlap condition: existingStart &lt; endDate AND existingEnd &gt; startDate.
        /// Optionally excludes a reservation (for edit scenarios to avoid self-conflict).
        /// </summary>
        public bool IsRoomOverlapping(int roomId, DateOnly startDate, DateOnly endDate, int? excludeReservationId = null)
        {
            using var context = new FuminiHotelManagementContext();
            return context.BookingDetails.Any(bd =>
                bd.RoomId == roomId &&
                (excludeReservationId == null || bd.BookingReservationId != excludeReservationId) &&
                bd.StartDate < endDate &&
                bd.EndDate > startDate
            );
        }

        public void Add(BookingReservation reservation)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                using var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);

                // Generate new ID
                if (reservation.BookingReservationId == 0)
                {
                    int maxId = context.BookingReservations.Max(br => (int?)br.BookingReservationId) ?? 0;
                    reservation.BookingReservationId = maxId + 1;
                }

                // Re-validate room availability inside the transaction to prevent race conditions
                foreach (var detail in reservation.BookingDetails)
                {
                    bool isOverlapping = context.BookingDetails.Any(bd =>
                        bd.RoomId == detail.RoomId &&
                        bd.StartDate < detail.EndDate &&
                        bd.EndDate > detail.StartDate
                    );
                    if (isOverlapping)
                    {
                        transaction.Rollback();
                        throw new Exception($"Room {detail.RoomId} is no longer available for the selected dates. Another booking was made concurrently.");
                    }
                    detail.BookingReservationId = reservation.BookingReservationId;
                }

                context.BookingReservations.Add(reservation);
                context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex) when (!ex.Message.Contains("no longer available"))
            {
                throw new Exception($"Error adding reservation: {ex.Message}");
            }
        }

        public void Update(BookingReservation reservation)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                using var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable);

                var existing = context.BookingReservations
                    .Include(br => br.BookingDetails)
                    .FirstOrDefault(br => br.BookingReservationId == reservation.BookingReservationId);

                if (existing == null)
                {
                    transaction.Rollback();
                    throw new Exception("Reservation not found.");
                }

                // Re-validate room availability inside the transaction, excluding self
                foreach (var detail in reservation.BookingDetails)
                {
                    bool isOverlapping = context.BookingDetails.Any(bd =>
                        bd.RoomId == detail.RoomId &&
                        bd.BookingReservationId != reservation.BookingReservationId &&
                        bd.StartDate < detail.EndDate &&
                        bd.EndDate > detail.StartDate
                    );
                    if (isOverlapping)
                    {
                        transaction.Rollback();
                        throw new Exception($"Room {detail.RoomId} is no longer available for the selected dates. Another booking was made concurrently.");
                    }
                }

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
                transaction.Commit();
            }
            catch (Exception ex) when (!ex.Message.Contains("no longer available") && !ex.Message.Contains("not found"))
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

