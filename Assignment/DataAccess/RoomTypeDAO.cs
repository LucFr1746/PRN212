using System;
using System.Collections.Generic;
using System.Linq;
using Assignment.BusinessObjects;

namespace Assignment.DataAccess
{
    public class RoomTypeDAO
    {
        private static RoomTypeDAO? instance;
        private static readonly object instanceLock = new object();

        private RoomTypeDAO() { }

        public static RoomTypeDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomTypeDAO();
                    }
                    return instance;
                }
            }
        }

        public List<RoomType> GetAll()
        {
            try
            {
                using var context = new FuminiHotelManagementContext();
                return context.RoomTypes.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving room types: {ex.Message}");
            }
        }
    }
}
