using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

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
        //public bool InsertRoom(Room roomNow)
        //{
        //    return InsertRoom(roomNow);
        //}
        public bool InsertRoom(Room roomNow)
        {
            string query = "INSERT INTO dbo.Room(ID,Name, IDRoomType, IDStatusRoom) VALUES (@id, @nameRoom, @idRoomType, @idStatusRoom)";
            //MessageBox.Show($"SQL Query: {query}");
            MessageBox.Show($"SQL Query: {roomNow.Id}, {roomNow.Name }, {roomNow.IdRoomType }, {roomNow.IdStatusRoom}");
            try
            {
                // Thực thi câu lệnh SQL và kiểm tra số lượng hàng bị ảnh hưởng
                int rowsAffected = DataProvider.Instance.ExecuteNoneQuery(query, new object[] { roomNow.Id, roomNow.Name, roomNow.IdRoomType, roomNow.IdStatusRoom });

                if (rowsAffected > 0)
                {
                    // Nếu có hàng bị ảnh hưởng, thông báo thành công
                    MessageBox.Show("Câu lệnh đã chạy thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Nếu không, báo rằng không có thay đổi
                    MessageBox.Show("Không có thay đổi nào được thực hiện.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SqlException sqlEx)
            {
                // Xử lý các lỗi liên quan đến SQL
                MessageBox.Show($"Lỗi SQL xảy ra: {sqlEx.Message}\nMã lỗi: {sqlEx.Number}", "Lỗi SQL", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            catch (ArgumentException argEx)
            {
                // Xử lý các lỗi liên quan đến tham số không hợp lệ
                MessageBox.Show("Lỗi tham số không hợp lệ: " + argEx.Message, "Lỗi tham số", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return DataProvider.Instance.ExecuteNoneQuery(query, new object[] { roomNow.Id, roomNow.Name, roomNow.IdRoomType, roomNow.IdStatusRoom }) > 0;
                
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
