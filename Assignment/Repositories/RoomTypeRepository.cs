using System.Collections.Generic;
using Assignment.BusinessObjects;
using Assignment.DataAccess;

namespace Assignment.Repositories
{
    public class RoomTypeRepository : IRoomTypeRepository
    {
        public List<RoomType> GetAllRoomTypes() => RoomTypeDAO.Instance.GetAll();
    }
}
