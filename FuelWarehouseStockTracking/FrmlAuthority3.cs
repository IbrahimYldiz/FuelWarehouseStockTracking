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
    public partial class FrmlAuthority3 : Form
    {
        public FrmlAuthority3()
        {
            InitializeComponent();
        }
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-OC5036T\MSSQLSERVER1;Initial Catalog=DbFuelWarehouseStockTracking;Integrated Security=True");
        public string ID3;
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

        private void button1_Click(object sender, EventArgs e)
        {
            FrmWarehouseOperations fr = new FrmWarehouseOperations();
            fr.DpID1 = ID3;
            fr.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmProductSell fr = new FrmProductSell();
            fr.PRID = ID3;
            fr.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FrmTill fr = new FrmTill();
            fr.TillID = ID3;
            fr.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmMovements fr = new FrmMovements();
            fr.MID = ID3;
            fr.Show();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FrmChangePassword fr = new FrmChangePassword();
            fr.PSID = ID3;
            fr.Show();
        }

        private void FrmlAuthority3_Load(object sender, EventArgs e)
        {
            Movements();
            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", ID3);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", this.Text+ " Ekranına Giriş Yapıldı");
            command1.ExecuteNonQuery();
            connection.Close();
        }
    }
}
