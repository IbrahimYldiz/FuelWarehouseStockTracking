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
    public partial class FrmFuelSell : Form
    {
        public FrmFuelSell()
        {
            InitializeComponent();
        }
        public string FuelID;
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
        void logsave()
        {
            Movements();
            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", FuelID);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", this.Text + " Ekranına Giriş Yapıldı");
            command1.ExecuteNonQuery();
            connection.Close();
        }
        void cmblist()
        {
            SqlDataAdapter da = new SqlDataAdapter("select FuelID,FuelName from TblFuel where FuelStock>0", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbFuelName.DataSource = dt;
            CmbFuelName.DisplayMember = "FuelName";
            CmbFuelName.ValueMember = "FuelID";
        }
        void stock()
        {

            PgbVMaxKursunsuz.Minimum = 0;
            PgbVMaxDiesel.Minimum = 0;
            PgbVProDiesel.Minimum = 0;
            PgbPOGaz.Minimum = 0;
            connection.Open();
            SqlCommand command = new SqlCommand("select FuelStock,FuelStockMax from TblFuel where FuelID=3", connection);
            SqlDataReader dr = command.ExecuteReader();
            int t;
            decimal de;
            
            
            while (dr.Read())
            {
                de = Convert.ToDecimal(dr[0].ToString());
                Math.Round(de);
                t = Convert.ToInt32(de);
                PgbVMaxKursunsuz.Maximum = int.Parse(dr[1].ToString());
                PgbVMaxKursunsuz.Value = t;
                
            }

            dr.Close();
            SqlCommand command1 = new SqlCommand("select FuelStock,FuelStockMax from TblFuel where FuelID=4", connection);
            SqlDataReader dr1 = command1.ExecuteReader();
            int t1;
            decimal de1;


            while (dr1.Read())
            {
                de1 = Convert.ToDecimal(dr1[0].ToString());
                Math.Round(de1);
                t1 = Convert.ToInt32(de1);
                PgbVMaxDiesel.Maximum = int.Parse(dr1[1].ToString());
                PgbVMaxDiesel.Value = t1;

            }
            dr1.Close();
            SqlCommand command2 = new SqlCommand("select FuelStock,FuelStockMax from TblFuel where FuelID=5", connection);
            SqlDataReader dr2 = command2.ExecuteReader();
            int t2;
            decimal de2;


            while (dr2.Read())
            {
                de2 = Convert.ToDecimal(dr2[0].ToString());
                Math.Round(de2);
                t2 = Convert.ToInt32(de2);
                PgbVProDiesel.Maximum = int.Parse(dr2[1].ToString());
                PgbVProDiesel.Value = t2;

            }
            dr2.Close();
            SqlCommand command3 = new SqlCommand("select FuelStock,FuelStockMax from TblFuel where FuelID=6", connection);
            SqlDataReader dr4 = command3.ExecuteReader();
            int t4;
            decimal de4;


            while (dr4.Read())
            {
                de4 = Convert.ToDecimal(dr4[0].ToString());
                Math.Round(de4);
                t4 = Convert.ToInt32(de4);
                PgbPOGaz.Maximum = int.Parse(dr4[1].ToString());
                PgbPOGaz.Value = t4;

            }
            dr4.Close();

            connection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Girilen Bilgiler Doğru Olduğunu Onaylıyor musunuz?", "Satış Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                if(TxtPlaka.Text.Trim()!=""&&TxtPlaka.Text.Trim().Length>=8 && 13 >= TxtPlaka.Text.Trim().Length&& TxtPrice.Text.Trim()!=""&& TxtStok.Text.Trim() != "")
                {
                    if (Convert.ToDecimal(TxtStok.Text.Trim()) > 0 && Convert.ToDecimal(TxtPrice.Text.Trim()) > 0)
                    {
                        Movements();
                        connection.Open();
                        SqlCommand command = new SqlCommand("update TblFuel set FuelStock=FuelStock-1 where FuelID=@p1", connection);
                        command.Parameters.AddWithValue("@p1", CmbFuelName.SelectedValue.ToString());
                        command.ExecuteNonQuery();

                        SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken,MovementsFuel) values (@p1,@p2,@p3,@p4,@p5)", connection);
                        command1.Parameters.AddWithValue("@p1", FuelID);
                        command1.Parameters.AddWithValue("@p2", Movementsdate);
                        command1.Parameters.AddWithValue("@p3", Movementshour);
                        command1.Parameters.AddWithValue("@p4", "Ürün Satışı Yapıldı");
                        command1.Parameters.AddWithValue("@p5", CmbFuelName.SelectedValue.ToString());
                        command1.ExecuteNonQuery();
                        SqlCommand command2 = new SqlCommand("insert into TblTill (TillFuel,TillFuelPrice,Personel,Date,Hour,TillFuelCar) values (@p1,@p2,@p3,@p4,@p5,@p6)", connection);
                        command2.Parameters.AddWithValue("@p1", CmbFuelName.SelectedValue.ToString());
                        command2.Parameters.AddWithValue("@p2", Convert.ToDecimal(TxtPrice.Text));
                        command2.Parameters.AddWithValue("@p3", FuelID);
                        command2.Parameters.AddWithValue("@p4", Movementsdate);
                        command2.Parameters.AddWithValue("@p5", Movementshour);
                        command2.Parameters.AddWithValue("@p6", TxtPlaka.Text);
                        command2.ExecuteNonQuery();
                        MessageBox.Show("Satış Gerçekleştirildi");
                        
                        connection.Close();
                        stock();
                    }
                    else
                    {

                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }

        private void TxtPrice_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void TxtStok_TextChanged(object sender, EventArgs e)
        {
            decimal sf,alm, totalprice;
            connection.Open();
            SqlCommand command = new SqlCommand("select FuelSellPrice from TblFuel where FuelID=@p1",connection);
            command.Parameters.AddWithValue("@p1", CmbFuelName.SelectedValue.ToString());
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                sf = Convert.ToDecimal(dr[0].ToString());
                alm = Convert.ToDecimal(TxtStok.Text);
                totalprice = sf * alm;
                TxtPrice.Text = totalprice.ToString();
            }
            connection.Close();
            
        }

        private void FrmFuelSell_Load(object sender, EventArgs e)
        {
            Movements();
            logsave();
            cmblist();

            stock();
        }
    }
}
