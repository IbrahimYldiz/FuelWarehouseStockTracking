using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FuelWarehouseStockTracking
{
    public partial class FrmChangePassword : Form
    {
        public string PSID;
        string Movementsdate, Movementshour;
        void Movements()
        {
            DateTime hour = DateTime.Now;
            DateTime date = DateTime.Today;

            //gün
            Movementsdate = date.ToString("dd.MM.yyyy");
            //saat
            Movementshour = hour.ToString("HH:mm:ss");

        }
        public FrmChangePassword()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-OC5036T\MSSQLSERVER1;Initial Catalog=DbFuelWarehouseStockTracking;Integrated Security=True");

        private void button1_Click(object sender, EventArgs e)
        {
            Movements();
            connection.Open();
            if (TxtPass.Text.Trim()!="")
            {
                if (TxtNewPass.Text.Length>7 &&TxtNewPassRety.Text.Length>7)
                {
                    SqlCommand command = new SqlCommand("select * from TblPersonel where PersonelID=@p1 and PersonelPassword=@p2", connection);
                    command.Parameters.AddWithValue("@p1", PSID);
                    command.Parameters.AddWithValue("@p2", TxtPass.Text);
                    SqlDataReader dr = command.ExecuteReader();
                    if (dr.Read())
                    {
                        dr.Close();
                        if (TxtNewPass.Text == TxtNewPassRety.Text)
                        {
                            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
                            command1.Parameters.AddWithValue("@p1", PSID);
                            command1.Parameters.AddWithValue("@p2", Movementsdate);
                            command1.Parameters.AddWithValue("@p3", Movementshour);
                            command1.Parameters.AddWithValue("@p4", "Şifre değiştirildi");
                            command1.ExecuteNonQuery();

                            SqlCommand command2 = new SqlCommand("update TblPersonel set PersonelPassword=@p1 where PersonelID=@p2", connection);
                            command2.Parameters.AddWithValue("@p1", TxtNewPassRety.Text);
                            command2.Parameters.AddWithValue("@p2", PSID);
                            command2.ExecuteNonQuery();
                            MessageBox.Show("Şifre Değiştirildi");
                        }
                        else
                        {
                            MessageBox.Show("Yeni Şifre ve Yeni Şifre Tekrar Girişlerindeki Değerler Aynı Değil");
                        }


                    }
                    else
                    {
                        MessageBox.Show("Geçerli Şifre Hatalı");
                    }
                }
                else
                {
                    MessageBox.Show("Yeni Şifre En Az 8 Karekter Olmalıdır");
                }
            }
            else
            {
                MessageBox.Show("Lütfen Geçerli Şifreyi Giriniz");
            }
            connection.Close();

        }

        private void FrmChangePassword_Load(object sender, EventArgs e)
        {
            Movements();
            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", PSID);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", this.Text + " Ekranına Giriş Yapıldı");
            command1.ExecuteNonQuery();
            connection.Close();

        }
    }
}
