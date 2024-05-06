using DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HotelManager
{
    public partial class fReceiveRoomDetails : Form
    {
        string idReceiveRoom;
        public fReceiveRoomDetails(string _idReceiRoom)
        {
            InitializeComponent();
            idReceiveRoom = _idReceiRoom;
            ShowReceiveRoom(_idReceiRoom);
            ShowCustomers(_idReceiRoom);
        }
        public void ShowReceiveRoom(string idReceiveRoom)
        {
            DataRow data = ReceiveRoomDAO.Instance.ShowReceiveRoom(idReceiveRoom).Rows[0];
            txbIDReceiveRoom.Text = (data["Mã nhận phòng"]).ToString();
            txbRoomName.Text = data["Tên phòng"].ToString();
            txbDateCheckIn.Text = ((DateTime)data["Ngày nhận"]).ToString().Split(' ')[0];
            txbDateCheckOut.Text= ((DateTime)data["Ngày trả"]).ToString().Split(' ')[0];
        }
        public void ShowCustomers(string idReceiveRoom)
        {
            dataGridView.DataSource = ReceiveRoomDAO.Instance.ShowCusomers(idReceiveRoom);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnClose__Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            //Kiểm tra số lượng khách hàng hiện tại trong DataGridView
            int currentCustomerCount = dataGridView.Rows.Count;
            DataTable result = DAO.ParameterDAO.Instance.GetMaxPersonByRoomType(txbRoomName.Text);

            int maxPerson = (int)result.Rows[0]["LimitPerson"]; // Số lượng tối đa
            if (currentCustomerCount >= maxPerson)
            {
                MessageBox.Show("Phòng đã đạt số lượng người tối đa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Không cho phép thêm người nữa
            }

            fAddCustomerInfo f = new fAddCustomerInfo();
            this.Hide();
            f.ShowDialog();
            this.Show();
            if (fAddCustomerInfo.ListIdCustomer.Count > 0)
                foreach (var item in fAddCustomerInfo.ListIdCustomer)
                {
                    ReceiveRoomDetailsDAO.Instance.InsertReceiveRoomDetails(idReceiveRoom, item);
                }
            ShowCustomers(idReceiveRoom);
        }

        //private void bunifuThinButton21_Click(object sender, EventArgs e)
        //{
        //    // Kiểm tra số lượng khách hàng hiện tại trong DataGridView
        //    int currentCustomerCount = dataGridView.Rows.Count;

        //    // Lấy số lượng tối đa khách hàng theo loại phòng

        //    //string roomTypeId = GetRoomTypeIdFromReceiveRoomId(idReceiveRoom); // Bạn cần một phương thức để lấy loại phòng từ ID nhận phòng
        //    DataTable result = DataProvider.Instance.ExecuteQuery("USP_GetMaxPersonByRoomType @idRoomType", new object[] { roomTypeId });

        //    if (result.Rows.Count == 0)
        //    {
        //        MessageBox.Show("Không thể xác định số lượng khách hàng tối đa cho loại phòng này.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return;
        //    }

        //    int maxPerson = (int)result.Rows[0]["Value"]; // Số lượng tối đa

        //    // Kiểm tra nếu số lượng hiện tại đã đạt hoặc vượt quá giới hạn
        //    if (currentCustomerCount >= maxPerson)
        //    {
        //        MessageBox.Show("Phòng đã đạt số lượng khách hàng tối đa.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }

        //    // Nếu chưa đạt giới hạn, cho phép thêm khách hàng mới
        //    fAddCustomerInfo f = new fAddCustomerInfo();
        //    this.Hide();
        //    f.ShowDialog();
        //    this.Show();

        //    // Kiểm tra danh sách khách hàng mới được thêm từ fAddCustomerInfo
        //    if (fAddCustomerInfo.ListIdCustomer.Count > 0)
        //    {
        //        foreach (var item in fAddCustomerInfo.ListIdCustomer)
        //        {
        //            ReceiveRoomDetailsDAO.Instance.InsertReceiveRoomDetails(idReceiveRoom, item);
        //        }
        //    }

        //    // Hiển thị lại danh sách khách hàng trong phòng
        //    ShowCustomers(idReceiveRoom);
        //}


        private void bunifuThinButton22_Click(object sender, EventArgs e)
        {
            string idCard =dataGridView.SelectedRows[0].Cells[1].Value.ToString();
            string idCustomer = CustomerDAO.Instance.GetInfoByIdCard(idCard).Id;
            if (idCustomer != CustomerDAO.Instance.GetIDCustomerFromBookRoom(idReceiveRoom))
            {
                ReceiveRoomDetailsDAO.Instance.DeleteReceiveRoomDetails(idReceiveRoom, idCustomer);
                MessageBox.Show("Xóa khách hàng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowCustomers(idReceiveRoom);
            }
            else
                MessageBox.Show("Không thể xóa!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void bunifuThinButton23_Click(object sender, EventArgs e)
        {
            string idCard = dataGridView.SelectedRows[0].Cells[1].Value.ToString();
            string idCustomer = CustomerDAO.Instance.GetInfoByIdCard(idCard).Id;
            fUpdateCustomerInfo f = new fUpdateCustomerInfo(idCard);
            this.Hide();
            f.ShowDialog();
            this.Show();
            ShowCustomers(idReceiveRoom);
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            fChangeRoom f = new fChangeRoom(RoomDAO.Instance.GetIdRoomFromReceiveRoom(idReceiveRoom),idReceiveRoom);
            this.Hide();
            f.ShowDialog();
            this.Show();
            ShowReceiveRoom(idReceiveRoom);
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
