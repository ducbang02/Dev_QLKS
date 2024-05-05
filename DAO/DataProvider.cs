using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAO
{
    public class DataProvider
    {
        SqlCommand command;
        SqlConnection getConnect;
        SqlDataAdapter adapter;
        DataTable data;
        private static DataProvider instance;

        private void getConnectData()
        {
            //connection
            getConnect = new SqlConnection("Data Source=localhost;Initial Catalog=QLKS2;Integrated Security=True");
            getConnect.Open();

        }
        //closeConnect SQL
        private void closeConnectData()
        {
            getConnect.Close();
            getConnect.Dispose();
        }
        //selectSQL
        public DataTable ExecuteQuery(string query, object [ ] parameter = null)
        {

            getConnectData();
            command = new SqlCommand(query, getConnect);
            AddParameter(query, parameter, command);
            adapter = new SqlDataAdapter(command);
            data = new DataTable();
            adapter.Fill(data);

            closeConnectData();
            return data;
        }
        //Thực hiện sql insert update delete
        //public int ExecuteNoneQuery(string query, object[] parameter = null)
        //{
        //    int dt = 0;
        //    try
        //    {
        //        getConnectData();
        //        command = new SqlCommand(query, getConnect);

        //        if (parameter != null)
        //        {
        //            AddParameter(query, parameter, command); // Thêm các tham số vào SqlCommand
        //        }

        //        dt = command.ExecuteNonQuery(); // Thực thi câu lệnh SQL
        //    }
        //    catch (SqlException ex)
        //    {
        //        Console.WriteLine($"Lỗi SQL: {ex.Message}");
        //    }
        //    finally
        //    {
        //        closeConnectData(); // Đảm bảo đóng kết nối
        //    }
        //    return dt;
        //}

        public int ExecuteNoneQuery(string query, object[] parameter = null)
        {
            int dt = 0;
            getConnectData();
            command = new SqlCommand(query, getConnect);
            AddParameter(query, parameter, command);
            dt = command.ExecuteNonQuery();
            closeConnectData();

            return dt;
        }

        public object ExecuteScalar(string query, object [ ] parameter = null)
        {
            object data = new object();
            getConnectData();
            command = new SqlCommand(query, getConnect);
            AddParameter(query, parameter, command);
            data = command.ExecuteScalar();
            closeConnectData();

            return data;
        }
        private void AddParameter(string query, object[] parameter, SqlCommand command)
        {
            if (parameter != null)
            {
                string[] listParameter = query.Split(' ');
                int i = 0;
                foreach (string item in listParameter)
                {
                    if (item.Contains("@"))
                    {
                        command.Parameters.AddWithValue(item, parameter[i]);
                        ++i;
                    }
                }
            }
            
        }
        /// <summary>
        /// Thêm các tham số vào đối tượng SqlCommand dựa trên câu truy vấn và mảng tham số.
        /// </summary>
        /// <param name="query">Câu truy vấn SQL.</param>
        /// <param name="parameter">Mảng các giá trị tham số.</param>
        /// <param name="command">Đối tượng SqlCommand để thêm tham số.</param>
        //private void AddParameter(string query, object[] parameter, SqlCommand command)
        //{
        //    if (parameter != null) // Kiểm tra nếu mảng tham số không phải null
        //    {
        //        // Tách câu truy vấn thành các từ để xác định tham số
        //        string[] listParameter = query.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);

        //        int i = 0; // Chỉ số cho mảng tham số
        //        foreach (string item in listParameter) // Duyệt qua từng từ trong câu truy vấn
        //        {
        //            if (item.StartsWith("@")) // Nếu từ chứa tham số (bắt đầu bằng '@')
        //            {
        //                if (i < parameter.Length && parameter[i] != null) // Đảm bảo chỉ số không vượt quá mảng và giá trị không null
        //                {
        //                    // Thêm tham số vào SqlCommand với tên tham số và giá trị tương ứng
        //                    command.Parameters.AddWithValue(item, parameter[i]);

        //                    // Tăng chỉ số để lấy tham số tiếp theo trong mảng
        //                    i++;
        //                }
        //                else
        //                {
        //                    // Nếu tham số bị thiếu hoặc giá trị là null
        //                    Console.WriteLine($"Thiếu hoặc tham số rỗng cho: {item}");
        //                    throw new ArgumentException($"Tham số {item} không hợp lệ hoặc rỗng."); // Ném ngoại lệ nếu cần
        //                }
        //            }
        //        }
        //    }
        //}

        public static DataProvider Instance
        {
            get { if ( instance == null ) instance = new DataProvider(); return instance; }
            private set => instance = value;
        }
        private DataProvider() { }
    }
}