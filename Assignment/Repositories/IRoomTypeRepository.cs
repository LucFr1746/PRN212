using System.Collections.Generic;
using Assignment.BusinessObjects;

namespace Assignment.Repositories
{
    public interface IRoomTypeRepository
    {
        List<RoomType> GetAllRoomTypes();
    }
}
