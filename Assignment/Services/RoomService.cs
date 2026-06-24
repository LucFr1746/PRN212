using System.Collections.Generic;
using Assignment.BusinessObjects;
using Assignment.Repositories;

namespace Assignment.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService()
        {
            _roomRepository = new RoomRepository();
        }

        public List<RoomInformation> GetAllRooms() => _roomRepository.GetAllRooms();

        public RoomInformation? GetRoomById(int id) => _roomRepository.GetRoomById(id);

        public void AddRoom(RoomInformation room) => _roomRepository.AddRoom(room);

        public void UpdateRoom(RoomInformation room) => _roomRepository.UpdateRoom(room);

        public void DeleteRoom(int id) => _roomRepository.DeleteRoom(id);
    }
}
