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
    public partial class FrmLogin : Form
    {
        public FrmLogin()
        {
            InitializeComponent();
        }
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
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-OC5036T\MSSQLSERVER1;Initial Catalog=DbFuelWarehouseStockTracking;Integrated Security=True");

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            Movements();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();

            if (TxtID.Text.Trim() != "")
            {

                SqlCommand command = new SqlCommand("select PersonelAuthority from TblPersonel where PersonelID=@p1 and PersonelPassword=@p2", connection);
                command.Parameters.AddWithValue("@p1", TxtID.Text);
                command.Parameters.AddWithValue("@p2", TxtPass.Text);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    string Authority = dr[0].ToString();
                    dr.Close();
                    Movements();

                    //// yetkiye göre ayarlama
                    if (Authority == "1")
                    {
                        // sadece satış ekranı


                        SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
                        command1.Parameters.AddWithValue("@p1", TxtID.Text);
                        command1.Parameters.AddWithValue("@p2", Movementsdate);
                        command1.Parameters.AddWithValue("@p3", Movementshour);
                        command1.Parameters.AddWithValue("@p4", "Giriş Yaptı");
                        command1.ExecuteNonQuery();
                        FrmlAuthority1 fr = new FrmlAuthority1();
                        fr.ID = TxtID.Text;
                        fr.Show();
                        this.Hide();
                        connection.Close();
                    }
                    else if (Authority == "2")
                    {
                        // stok giriş ve satış ekranı
                        

                        SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
                        command1.Parameters.AddWithValue("@p1", TxtID.Text);
                        command1.Parameters.AddWithValue("@p2", Movementsdate);
                        command1.Parameters.AddWithValue("@p3", Movementshour);
                        command1.Parameters.AddWithValue("@p4", "Giriş Yaptı");
                        command1.ExecuteNonQuery();
                        FrmlAuthority2 fr = new FrmlAuthority2();
                        fr.DpID = TxtID.Text;
                        fr.Show();
                        this.Hide();
                        connection.Close();
                    }
                    else if (Authority == "3")
                    {
                        //personel bütün ekranlar açık
                        

                        SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
                        command1.Parameters.AddWithValue("@p1", TxtID.Text);
                        command1.Parameters.AddWithValue("@p2", Movementsdate);
                        command1.Parameters.AddWithValue("@p3", Movementshour);
                        command1.Parameters.AddWithValue("@p4", "Giriş Yaptı");
                        command1.ExecuteNonQuery();

                        FrmlAuthority3 fr = new FrmlAuthority3();
                        fr.ID3 = TxtID.Text;
                        fr.Show();
                        this.Hide();
                        connection.Close();
                    }
                    else
                    {
                        MessageBox.Show("Giriş Yetkiniz Bulunmamaktadır");
                    }
                }
                else
                {
                    MessageBox.Show("ID ya da Şifre Yanlış");
                }

            }
            connection.Close();
        }
    }
}
