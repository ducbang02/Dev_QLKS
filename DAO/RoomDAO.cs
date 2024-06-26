﻿using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAO
{
    public class RoomDAO
    {
        private static RoomDAO instance;
        #region Method

        public DataTable LoadFullRoom()
        {
            return DataProvider.Instance.ExecuteQuery("USP_LoadFullRoom");
        }
        public bool InsertRoom(Room roomNow)
        {
            if (roomNow.IdStatusRoom == 0) // Nếu chưa được thiết lập
            {
                roomNow.IdStatusRoom = 1;
            }
            return InsertRoom(roomNow.Name, roomNow.IdRoomType, roomNow.IdStatusRoom);
        }
        public bool InsertRoom(string roomName, string idRoomType, int idStatusRoom)
        {
            idStatusRoom = 1;
            string query = "USP_InsertRoom @nameRoom , @idRoomType , @idStatusRoom";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { roomName, idRoomType, idStatusRoom }) > 0;
        }
        public bool UpdateCustomer(Room roomNow)
        {
            string query = "USP_UpdateRoom  @id , @nameRoom , @idRoomType , @idStatusRoom";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { roomNow.Id, roomNow.Name, roomNow.IdRoomType, roomNow.IdStatusRoom }) > 0;
        }
        public DataTable Search(string text, int id)
        {
            string query = "USP_SearchRoom @string , @id";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { text, id });
        }
        public List<Room> LoadEmptyRoom(string idRoomType)
        {
            List<Room> rooms = new List<Room>();
            string query = "USP_LoadEmptyRoom @idRoomType";
            DataTable data = DataProvider.Instance.ExecuteQuery(query,new object[] { idRoomType });
            foreach (DataRow item in data.Rows)
            {
                Room room = new Room(item);
                rooms.Add(room);
            }
            return rooms;
        }
        public List<Room> LoadListFullRoom()
        {
            string query = "USP_LoadListFullRoom @getToday";
            List<Room> rooms = new List<Room>();
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] {DateTime.Now.Date });
            foreach (DataRow item in data.Rows)
            {
                Room room = new Room(item);
                rooms.Add(room);
            }
            return rooms;
        }
        public int GetPeoples(string idBill)
        {
            string query = "USP_GetPeoples @idBill";
            return ( int )DataProvider.Instance.ExecuteScalar(query, new object [ ] { idBill }) + 1;
        }
        public string GetIdRoomFromReceiveRoom(string idReceiveRoom)
        {
            string query = "USP_GetIDRoomFromReceiveRoom @idReceiveRoom";
            return (string)DataProvider.Instance.ExecuteScalar(query, new object[] { idReceiveRoom });
        }
        public bool UpdateStatusRoom(string idRoom)
        {
            string query = "USP_UpdateStatusRoom @idRoom";
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { idRoom }) > 0;
        }
        #endregion

        public static RoomDAO Instance { get {if(instance==null) instance=new RoomDAO();return instance; }
            private set => instance = value; }
        private RoomDAO() { }
    }
}
