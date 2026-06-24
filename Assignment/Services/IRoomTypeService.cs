using System.Collections.Generic;
using Assignment.BusinessObjects;

namespace Assignment.Services
{
    public interface IRoomTypeService
    {
        List<RoomType> GetAllRoomTypes();
    }
}
