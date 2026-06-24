using System.Collections.Generic;
using Assignment.BusinessObjects;

namespace Assignment.Services
{
    public interface IRoomService
    {
        List<RoomInformation> GetAllRooms();
        RoomInformation? GetRoomById(int id);
        void AddRoom(RoomInformation room);
        void UpdateRoom(RoomInformation room);
        void DeleteRoom(int id);
    }
}
