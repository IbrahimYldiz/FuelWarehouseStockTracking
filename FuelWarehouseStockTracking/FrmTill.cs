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
    public partial class FrmTill : Form
    {
        public string TillID;
        string Movementsdate, Movementshour;
        public FrmTill()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-OC5036T\MSSQLSERVER1;Initial Catalog=DbFuelWarehouseStockTracking;Integrated Security=True");

        void Movements()
        {
            DateTime hour = DateTime.Now;
            DateTime date = DateTime.Today;

            //gün
            Movementsdate = date.ToString("dd.MM.yyyy");
            //saat
            Movementshour = hour.ToString("HH:mm:ss");
            LblDate.Text = Movementsdate;


        }
        void prototal()
        {
            Movements();
            connection.Open();
            SqlCommand command = new SqlCommand("select SUM(ProductPrice) from TblTill where Date=@p1", connection);
            command.Parameters.AddWithValue("@p1", LblDate.Text);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {

                LblMarkSell.Text = dr[0].ToString();

            }

            daytotal();
            dr.Close();
            connection.Close();
        }
        decimal f, m, t;
        void daytotal()
        {
            if (LblMarkSell.Text != "Null" && LblFuelSell.Text != "Null")
            {
                f = Convert.ToDecimal(LblFuelSell.Text);
                m = Convert.ToDecimal(LblMarkSell.Text);
                t = m + f;
                LblDayTotal.Text = t.ToString();
            }

        }
        void list()
        {

            SqlDataAdapter da = new SqlDataAdapter("select ProdutName as 'Ürün Adı',ProductPrice as 'Ürün Fiyatı',(PersonelName+' '+PersonelSurName) as 'Personel Ad Soyad',FuelName as 'Akaryakıt Adı',TillFuelPrice as 'Akaryakıt Fiyatı',Date as 'Tarih' from TblTill INNER JOIN TblFuel on TblFuel.FuelID=TblTill.TillFuel INNER JOIN TblStock on TblStock.ProductID=TblTill.TillProduct INNER JOIN TblPersonel ON TblPersonel.PersonelID=TblTill.Personel where Date=@p1", connection);
            da.SelectCommand.Parameters.AddWithValue("@p1", LblDate.Text);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            prototal();
            daytotal();

        }


        private void button1_Click(object sender, EventArgs e)
        {
            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", TillID);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", LblDate.Text + " Tarihindeki Kasa Dökümü Görüntülendi");
            command1.ExecuteNonQuery();
            connection.Close();
            list();
            prototal();
            fueltotal();


        }
        void fueltotal()
        {
            Movements();
            connection.Open();
            SqlCommand command = new SqlCommand("select SUM(TillFuelPrice) from TblTill where Date=@p1", connection);
            command.Parameters.AddWithValue("@p1", LblDate.Text);
            SqlDataReader dr = command.ExecuteReader();

            while (dr.Read())
            {

                LblFuelSell.Text = dr[0].ToString();


            }
            daytotal();

            dr.Close();
            connection.Close();
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            LblDate.Text = dateTimePicker1.Value.ToString("dd.MM.yyyy");



        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlDataAdapter da = new SqlDataAdapter("select ProdutName as 'Ürün Adı',ProductPrice as 'Ürün Fiyatı',(PersonelName+' '+PersonelSurName) as 'Personel Ad Soyad',FuelName as 'Akaryakıt Adı',TillFuelPrice as 'Akaryakıt Fiyatı',Date as 'Tarih' from TblTill INNER JOIN TblFuel on TblFuel.FuelID=TblTill.TillFuel INNER JOIN TblStock on TblStock.ProductID=TblTill.TillProduct INNER JOIN TblPersonel ON TblPersonel.PersonelID=TblTill.Personel where Date=@p1", connection);
            da.SelectCommand.Parameters.AddWithValue("@p1", Movementsdate);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            prototal();
            fueltotal();

            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", TillID);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", LblDate.Text + " Tarihindeki Kasa Dökümü Görüntülendi");
            command1.ExecuteNonQuery();
            connection.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FrmTill_Load(object sender, EventArgs e)
        {
            Movements();

            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", TillID);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", this.Text + " Ekranına Giriş Yapıldı");
            command1.ExecuteNonQuery();
            connection.Close();

            list();
            fueltotal(); prototal();

        }
    }
}
