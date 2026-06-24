using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace Assignment.DataAccess
{
    public class RoomInformationDAO
    {
        private static RoomInformationDAO? instance;
        private static readonly object instanceLock = new object();

        private RoomInformationDAO() { }

        public static RoomInformationDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomInformationDAO();
                    }
                    return instance;
                }
            }
        }

        public List<RoomInformation> GetAll()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.RoomInformations
                    .Include(r => r.RoomType)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving rooms: {ex.Message}");
            }
        }

        public RoomInformation? GetById(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.RoomInformations
                    .Include(r => r.RoomType)
                    .FirstOrDefault(r => r.RoomId == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving room: {ex.Message}");
            }
        }

        public void Add(RoomInformation room)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                // Check if RoomNumber already exists
                if (context.RoomInformations.Any(r => r.RoomNumber == room.RoomNumber))
                {
                    throw new Exception("Room number already exists.");
                }
                context.RoomInformations.Add(room);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding room: {ex.Message}");
            }
        }

        public void Update(RoomInformation room)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                // Check if RoomNumber already exists (excluding current room)
                if (context.RoomInformations.Any(r => r.RoomNumber == room.RoomNumber && r.RoomId != room.RoomId))
                {
                    throw new Exception("Room number already exists on another room.");
                }
                context.Entry(room).State = EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating room: {ex.Message}");
            }
        }

        public void Delete(int id)
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                var room = context.RoomInformations.Find(id);
                if (room != null)
                {
                    // Check if room is associated with any renting transaction
                    bool hasRentingTransaction = context.BookingDetails.Any(bd => bd.RoomId == id);
                    if (hasRentingTransaction)
                    {
                        // Just change status to 0 (Inactive)
                        room.RoomStatus = 0;
                        context.Entry(room).State = EntityState.Modified;
                    }
                    else
                    {
                        // Hard delete
                        context.RoomInformations.Remove(room);
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting room: {ex.Message}");
            }
        }
    }
}
