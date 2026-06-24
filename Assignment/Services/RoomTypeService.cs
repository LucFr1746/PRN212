using System.Collections.Generic;
using Assignment.BusinessObjects;
using Assignment.Repositories;

namespace Assignment.Services
{
    public class RoomTypeService : IRoomTypeService
    {
        private readonly IRoomTypeRepository _roomTypeRepository;

        public RoomTypeService()
        {
            _roomTypeRepository = new RoomTypeRepository();
        }

        public List<RoomType> GetAllRoomTypes() => _roomTypeRepository.GetAllRoomTypes();
    }
}
