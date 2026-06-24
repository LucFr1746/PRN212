using System.Collections.Generic;
using Assignment.BusinessObjects;
using Assignment.DataAccess;

namespace Assignment.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        public List<RoomInformation> GetAllRooms() => RoomInformationDAO.Instance.GetAll();

        public RoomInformation? GetRoomById(int id) => RoomInformationDAO.Instance.GetById(id);

        public void AddRoom(RoomInformation room) => RoomInformationDAO.Instance.Add(room);

        public void UpdateRoom(RoomInformation room) => RoomInformationDAO.Instance.Update(room);

        public void DeleteRoom(int id) => RoomInformationDAO.Instance.Delete(id);
    }
}
