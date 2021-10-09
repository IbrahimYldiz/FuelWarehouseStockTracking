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
    public partial class FrmMovements : Form
    {
        public FrmMovements()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-OC5036T\MSSQLSERVER1;Initial Catalog=DbFuelWarehouseStockTracking;Integrated Security=True");
        public string MID;
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
        void list()
        {
            Movements();
            SqlDataAdapter da = new SqlDataAdapter("select MovementsID as 'İşlem Numarası',TblStock.ProdutName as ' Ürün Adı' ,FuelName as 'Yakıt Adı' ,(PersonelName+' '+PersonelSurName) as 'Personel Ad Soyad',Date as 'Tarih',Hour as 'Saat',ActionTaken as 'Yapılan İşlem' from TblMovements INNER JOIN TblStock on TblStock.ProductID=TblMovements.MovementsProduct INNER JOIN TblFuel on TblFuel.FuelID=TblMovements.MovementsFuel INNer JOIN TblPersonel on TblPersonel.PersonelID=TblMovements.MovementsPersonel where TblMovements.Date=@p1", connection);
            da.SelectCommand.Parameters.AddWithValue("@p1", Movementsdate);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
           Movementsdate= dateTimePicker1.Value.ToString("dd.MM.yyyy");
            list();

            Movements();
            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", MID);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", Movementsdate + " Tarihi Kayıtlarına bakıldı");
            command1.ExecuteNonQuery();
            connection.Close();
        }

        private void FrmMovements_Load(object sender, EventArgs e)
        {
            
            list();
            Movements();
            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", MID);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", this.Text + " Ekranına Giriş Yapıldı");
            command1.ExecuteNonQuery();
            connection.Close();
        }
    }
}
