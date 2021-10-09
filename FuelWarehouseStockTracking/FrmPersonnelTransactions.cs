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
    public partial class FrmPersonnelTransactions : Form
    {
        public FrmPersonnelTransactions()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-OC5036T\MSSQLSERVER1;Initial Catalog=DbFuelWarehouseStockTracking;Integrated Security=True");
        public string ID;
        string updateID;
        string Movementsdate, Movementshour;
        
        void clear()
        {
            TxtName.Clear();
            TxtSurName.Clear();
            MskTc.Clear();
            MskPhone.Clear();
            RchAdress.Clear();
            
        }
        void Authority()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * from TblAuthority where AuthorityID=1 or AuthorityID=2", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbAuthority.ValueMember = "AuthorityID";
            CmbAuthority.DisplayMember = "AuthorityName";
            CmbAuthority.DataSource = dt;
        }
        void personellist()
        {
            SqlDataAdapter da1 = new SqlDataAdapter("select PersonelID as 'ID', PersonelName as 'Ad',PersonelSurName as 'Soyad',PersonelTC as 'TC',PersonelPhone as 'Telefon Numarası', PersonelAdress as 'Adres',AuthorityName as 'Yetki' from TblPersonel INNER JOIN TblAuthority ON TblAuthority.AuthorityID=TblPersonel.PersonelAuthority where PersonelAuthority=1 or PersonelAuthority=2", connection);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;
        }
        void Movements()
        {
            DateTime hour = DateTime.Now;
            DateTime date = DateTime.Today;

            //gün
            Movementsdate = date.ToString("dd.MM.yyyy");
            //saat
            Movementshour = hour.ToString("HH:mm:ss");
            
        }
        void Movements1()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command.Parameters.AddWithValue("@p1", ID);
            command.Parameters.AddWithValue("@p2", Movementsdate);
            command.Parameters.AddWithValue("@p3", Movementshour);
            command.Parameters.AddWithValue("@p4", this.Text + " Giriş Yapıldı");
            command.ExecuteNonQuery();
            connection.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            Authority();
            personellist();
            Movements();
            Movements1();
            
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if(TxtName.Text.Trim()!=""&&TxtSurName.Text.Trim()!=""&&MskTc.Text.Trim()!="___________"&&MskPhone.Text.Trim()!= "(___) ___-____" && RchAdress.Text.Trim() != "")
            {
                DialogResult result = MessageBox.Show("Kayıt Yapıldıktan Sonra Ad, Soyad ve T.C. Kimlik Numarası Değiştirme Yetkiniz Yoktur. Girilen Bilgilerin Doğruluğundan Eminseniz Evet'e Tıklayınız.", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    Random random = new Random();
                    int ran = random.Next(100000, 1000000009);
                    connection.Open();
                    SqlCommand command1 = new SqlCommand("select * from TblPersonel where PersonelTC=@p1", connection);
                    command1.Parameters.AddWithValue("@p1", MskTc.Text);
                    SqlDataReader dr = command1.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Bu Kişi Kayıtlı");
                        dr.Close();
                    }
                    else
                    {
                        dr.Close();
                        SqlCommand command2 = new SqlCommand("select * from TblPersonel where PersonelPhone=@p1", connection);
                        command2.Parameters.AddWithValue("@p1", MskPhone.Text);
                        SqlDataReader dr1 = command2.ExecuteReader();
                        if (dr1.Read())
                        {
                            MessageBox.Show("Telefon Numarası Kayıtlı");
                            dr1.Close();
                        }
                        else
                        {
                            dr1.Close();

                            Movements();
                            SqlCommand command = new SqlCommand("insert into TblPersonel (PersonelName,PersonelSurName,PersonelTC,PersonelPhone,PersonelAdress,PersonelAuthority,PersonelPassword) values (@p1,@p2,@p3,@p4,@p5,@p6,@p7)", connection);
                            command.Parameters.AddWithValue("@p1", TxtName.Text);
                            command.Parameters.AddWithValue("@p2", TxtSurName.Text);
                            command.Parameters.AddWithValue("@p3", MskTc.Text);
                            command.Parameters.AddWithValue("@p4", MskPhone.Text);
                            command.Parameters.AddWithValue("@p5", RchAdress.Text);
                            command.Parameters.AddWithValue("@p6", CmbAuthority.SelectedValue.ToString());
                            command.Parameters.AddWithValue("@p7", ran);
                            command.ExecuteNonQuery();
                            MessageBox.Show("Personel Kaydı Yapıldı");
                            MessageBox.Show(TxtName.Text + " " + TxtSurName.Text + " Adlı Personelin Şifresi= " + ran + " Olarak Belirlendi. Lütfen Şifreyi Personele İletiniz ve Şifreyi Değiştirmesi Gerektiğini Belirtiniz");
                            personellist();

                            SqlCommand command3 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values(@p1,@p2,@p3,@p4)", connection);
                            command3.Parameters.AddWithValue("@p1", ID);
                            command3.Parameters.AddWithValue("@p2", Movementsdate);
                            command3.Parameters.AddWithValue("@p3", Movementshour);
                            command3.Parameters.AddWithValue("@p4", MskTc.Text + " T.C. Kimlik Numaralı Personelin Kayıt İşlemi Yapıldı");
                            command3.ExecuteNonQuery();

                        }
                    }
                    
                }
                
                

            }
            else
            {
                MessageBox.Show("Lütfen İstenen Bilgileri Eksiksiz Olarak Giriniz");
            }
            connection.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            updateID = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            TxtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            TxtSurName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            MskTc.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            MskPhone.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            RchAdress.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
            CmbAuthority.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
            LblID.Text= dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();


            TxtName.Enabled = false;
            TxtSurName.Enabled = false;
            MskTc.Enabled = false;
            BtnSave.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (updateID != "")
            {
                if(MskPhone.Text.Trim()!= "(___) ___-____" && RchAdress.Text.Trim() != "")
                {
                    Movements();
                    connection.Open();
                    SqlCommand command = new SqlCommand("update TblPersonel set PersonelPhone=@p1,PersonelAdress=@p2,PersonelAuthority=@p3 where PersonelID=@p4", connection);
                    command.Parameters.AddWithValue("@p4", updateID);
                    command.Parameters.AddWithValue("@p1", MskPhone.Text);
                    command.Parameters.AddWithValue("@p2", RchAdress.Text);
                    command.Parameters.AddWithValue("@p3", CmbAuthority.SelectedValue.ToString());
                    command.ExecuteNonQuery();
                    
                    MessageBox.Show("Güncelleme Yapıldı");
                    SqlCommand command3 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values(@p1,@p2,@p3,@p4)", connection);
                    command3.Parameters.AddWithValue("@p1", ID);
                    command3.Parameters.AddWithValue("@p2", Movementsdate);
                    command3.Parameters.AddWithValue("@p3", Movementshour);
                    command3.Parameters.AddWithValue("@p4", MskTc.Text + " T.C. Kimlik Numaralı Personelin Bilgi Güncelleme İşlemi Yapıldı");
                    command3.ExecuteNonQuery();
                    connection.Close();
                    personellist();

                }
                else
                {
                    MessageBox.Show("Lütfen Telefon Numarası ve Adres Bilgilerini Doldurunuz");
                }
            }
            else
            {
                MessageBox.Show("Lütfen Güncelleme Yapmak İstediğiniz Personeli Tablodan Seçiniz");
            }

            TxtName.Enabled = true;
            TxtSurName.Enabled = true;
            MskTc.Enabled = true;
            BtnSave.Enabled = true;
            clear();

        }
    }
}
