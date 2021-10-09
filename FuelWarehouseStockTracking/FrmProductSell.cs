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
    public partial class FrmProductSell : Form
    {
        public FrmProductSell()
        {
            InitializeComponent();
        }
        public string PRID;
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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        void list()
        {
            SqlDataAdapter da = new SqlDataAdapter("select ProductID as 'Ürün Kodu',ProdutName as 'Ürün Adı',Price as 'Ürün Fiyatı' from TblStock where Stock>0", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }
        void personelinfo()
        {
            connection.Open();
            SqlCommand command = new SqlCommand("select (PersonelName+ ' ' +PersonelSurName) as 'Name' from TblPersonel where  PersonelID=@p1", connection);
            command.Parameters.AddWithValue("@p1", PRID);
            SqlDataReader dr1 = command.ExecuteReader();
            while (dr1.Read())
            {
                LblID.Text = PRID;
                LblName.Text = dr1[0].ToString();
            }
            connection.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                TxtProductCode.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                TxtPrice.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();

            }

        }

        private void TxtSearc_TextChanged(object sender, EventArgs e)
        {
            SqlDataAdapter da1 = new SqlDataAdapter("select ProductID as 'Ürün Kodu',ProdutName as 'Ürün Adı',Price as 'Ürün Fiyatı' from TblStock where Stock>0 and ProdutName like '%" + TxtSearc.Text + "%'", connection);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            dataGridView1.DataSource = dt1;
        }

        private void TxtProductCode_TextChanged(object sender, EventArgs e)
        {
            SqlDataAdapter da2 = new SqlDataAdapter("select ProductID as 'Ürün Kodu',ProdutName as 'Ürün Adı',Price as 'Ürün Fiyatı' from TblStock where Stock>0 and ProductID=@p1", connection);
            da2.SelectCommand.Parameters.AddWithValue("@p1", TxtProductCode.Text);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
        }

        private void BtnSell_Click(object sender, EventArgs e)
        {
            Movements();
            connection.Open();
            if (TxtProductCode.Text.Trim() != "" && TxtPrice.Text.Trim() != "")
            {
                SqlCommand command3 = new SqlCommand("select * from TblStock where ProductID=@p1 and Price=@p2 and Stock>0", connection);
                command3.Parameters.AddWithValue("@p1", TxtProductCode.Text);
                command3.Parameters.AddWithValue("@p2", Convert.ToDecimal(TxtPrice.Text));
                SqlDataReader dr = command3.ExecuteReader();
                if (dr.Read())
                {
                    dr.Close();
                    SqlCommand command = new SqlCommand("update TblStock set Stock=Stock-1 where ProductID=@p1", connection);
                    command.Parameters.AddWithValue("@p1", TxtProductCode.Text);
                    command.ExecuteNonQuery();

                    SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken,MovementsProduct) values (@p1,@p2,@p3,@p4,@p5)", connection);
                    command1.Parameters.AddWithValue("@p1", PRID);
                    command1.Parameters.AddWithValue("@p2", Movementsdate);
                    command1.Parameters.AddWithValue("@p3", Movementshour);
                    command1.Parameters.AddWithValue("@p4", "Ürün Satışı Yapıldı");
                    command1.Parameters.AddWithValue("@p5", TxtProductCode.Text);
                    command1.ExecuteNonQuery();
                    SqlCommand command2 = new SqlCommand("insert into TblTill (TillProduct,ProductPrice,Personel,Date,Hour) values (@p1,@p2,@p3,@p4,@p5)", connection);
                    command2.Parameters.AddWithValue("@p1", TxtProductCode.Text);
                    command2.Parameters.AddWithValue("@p2", Convert.ToDecimal(TxtPrice.Text));
                    command2.Parameters.AddWithValue("@p3", PRID);
                    command2.Parameters.AddWithValue("@p4", Movementsdate);
                    command2.Parameters.AddWithValue("@p5", Movementshour);
                    command2.ExecuteNonQuery();
                    MessageBox.Show("Satış Gerçekleştirildi");
                }
                else
                {
                    MessageBox.Show("Ürün Fiyat Yanlış Lütfen Satmak İstediğiniz Ürünü Tablodan Seçiniz");
                }
                

            }
            else
            {
                MessageBox.Show("Lütfen Satılmak İstenen Ürünü Seçiniz");
            }
            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FrmFuelSell fr = new FrmFuelSell();
            fr.FuelID = PRID;
            fr.Show();
        }

        private void FrmProductSell_Load(object sender, EventArgs e)
        {
            Movements();
            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", PRID);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", this.Text + " Ekranına Giriş Yapıldı");
            command1.ExecuteNonQuery();
            connection.Close();
            list();
            personelinfo();

        }
    }
}
