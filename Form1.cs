using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoCRUD
{
    public partial class Form1 : Form
    {
        public int ID;
        string connectStr = @"Data Source=HAUPC;Initial Catalog=DEMOCRUD;User ID=sa;Password=hau@@123";
        SqlConnection conn;
        SqlCommand cmd;
        DataTable table;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load( object sender , EventArgs e )
        {
            getStudent();
        }

        private void getStudent()
        {
            using ( conn = new SqlConnection(connectStr) )
            {
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select * from STUDENT";
                SqlDataReader dataReader = cmd.ExecuteReader();
                table = new DataTable();
                table.Load(dataReader);

                dgvDisplay.DataSource = table;
                dgvDisplay.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }

        }

        private void btnAdd_Click( object sender , EventArgs e )
        {
            if ( isValidData() )
            {
                using ( conn = new SqlConnection(connectStr) )
                {
                    conn.Open();
                    cmd = new SqlCommand("insert into STUDENT values(@SurName,@Name,@RollNumber,@Address,@phone)" , conn);
                    cmd.Parameters.AddWithValue("@SurName" , txtSurName.Text);
                    cmd.Parameters.AddWithValue("@Name" , txtName.Text);
                    cmd.Parameters.AddWithValue("@RollNumber" , txtRollNumber.Text);
                    cmd.Parameters.AddWithValue("@Address" , txtAddress.Text);
                    cmd.Parameters.AddWithValue("@phone" , txtPhone.Text);
                    cmd.ExecuteNonQuery();
                    getStudent();

                }
            }
            
        }

        private void dgvDisplay_CellClick( object sender , DataGridViewCellEventArgs e )
        {
            if ( e.RowIndex >= 0 )
            {
                ID = Convert.ToInt32(dgvDisplay.Rows[e.RowIndex].Cells[0].Value);
                txtSurName.Text = dgvDisplay.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtName.Text = dgvDisplay.Rows[e.RowIndex].Cells[2].Value.ToString();
                txtRollNumber.Text = dgvDisplay.Rows[e.RowIndex].Cells[3].Value.ToString();
                txtAddress.Text = dgvDisplay.Rows[e.RowIndex].Cells[4].Value.ToString();
                txtPhone.Text = dgvDisplay.Rows[e.RowIndex].Cells[5].Value.ToString();
            }
        }

        private void btnUpdate_Click( object sender , EventArgs e )
        {
            using ( conn = new SqlConnection(connectStr) )
            {
                conn.Open();
                cmd = new SqlCommand("update STUDENT set SurName = @SurName, Name = @Name, RollNumber =  @RollNumber, Address = @Address, Phone = @Phone where ID = @ID " , conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@SurName" , txtSurName.Text);
                cmd.Parameters.AddWithValue("@Name" , txtName.Text);
                cmd.Parameters.AddWithValue("@RollNumber" , txtRollNumber.Text);
                cmd.Parameters.AddWithValue("@Address" , txtAddress.Text);
                cmd.Parameters.AddWithValue("@Phone" , txtPhone.Text);
                cmd.Parameters.AddWithValue("@ID" , this.ID);
                cmd.ExecuteNonQuery();
                getStudent();
            }
        }

        private void btnDelete_Click( object sender , EventArgs e )
        {
            using ( conn = new SqlConnection(connectStr) )
            {
                conn.Open();
                cmd = new SqlCommand("Delete from STUDENT where ID = @id " , conn);
                cmd.Parameters.AddWithValue("@id" , this.ID);
                cmd.ExecuteNonQuery();
                getStudent();
                resetData();
            }
        }

        private void btnExit_Click( object sender , EventArgs e )
        {
            if ( MessageBox.Show("ban muốn thoát" , "thông báo" , MessageBoxButtons.YesNo) == DialogResult.Yes )
            {
                Application.Exit();
            }
        }

        private bool isValidData()
        {
            if ( txtSurName.Text == string.Empty || txtName.Text == string.Empty || txtAddress.Text == string.Empty || txtRollNumber.Text == string.Empty || txtPhone.Text==string.Empty )
            {
                MessageBox.Show("Vui lòng nhập đủ dữ liệu.");
                return false;
            }
            return true;
        }

        private void resetData()
        {
            txtSurName.Text = "";
            txtName.Text = "";
            txtPhone.Text = "";
            txtRollNumber.Text = "";
            txtAddress.Text = "";
        }
    }
}
