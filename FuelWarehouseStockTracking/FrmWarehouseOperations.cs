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
    public partial class FrmWarehouseOperations : Form
    {
        public string DpID1;
        string Movementsdate, Movementshour;
        public FrmWarehouseOperations()
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

        }
        void Productcmblist()
        {

            SqlDataAdapter da1 = new SqlDataAdapter("select * from TblStock", connection);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            CmbUpdateProductName.DisplayMember = "ProdutName";

            CmbUpdateProductName.ValueMember = "ProductID";

            CmbUpdateProductName.DataSource = dt1;



        }
        void Fuelcmblist()
        {
            SqlDataAdapter da = new SqlDataAdapter("select FuelID,FuelName from TblFuel", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbUpdateFuelName.DisplayMember = "FuelName";
            CmbUpdateFuelName.ValueMember = "FuelID";

            CmbUpdateFuelName.DataSource = dt;
        }
        void FuelList()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("select FuelID as 'Yakıt Kodu',FuelName as 'Yakıt Adı',FuelSellPrice as 'Yakıt Ücreti',FuelStock as 'Stok' from TblFuel", connection);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView2.DataSource = dt3;
        }
        void productlist()
        {

            SqlDataAdapter da2 = new SqlDataAdapter("select ProductID as 'Ürün Kodu',ProdutName as 'Adı',Price as 'Fiyat',Stock as 'Stok' from TblStock ", connection);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
        }
        private void BtnProductSave_Click(object sender, EventArgs e)
        {
            if (TxtAddProductName.Text.Trim() != "" && TxtAddProductPrice.Text.Trim() != "" && TxtAddProductStock.Text.Trim() != "")
            {
                if (decimal.Parse(TxtAddProductPrice.Text.Trim()) > 0 && int.Parse(TxtAddProductStock.Text.Trim()) >= 0)
                {
                    connection.Open();
                    SqlCommand command2 = new SqlCommand("select * from TblStock where ProdutName=@p1", connection);
                    command2.Parameters.AddWithValue("@p1", TxtAddProductName.Text);
                    SqlDataReader dr = command2.ExecuteReader();
                    if (dr.Read())
                    {
                        MessageBox.Show("Ürün Daha Önce Kayıt Edilmiş. Lütfen Ürün Bilgilerini Güncelleyiniz");
                        dr.Close();
                    }

                    else
                    {
                        dr.Close();
                        Movements();


                        SqlCommand command = new SqlCommand("insert into TblStock (ProdutName,Price,Stock) values (@p1,@p2,@p3)", connection);
                        command.Parameters.AddWithValue("@p1", TxtAddProductName.Text);
                        command.Parameters.AddWithValue("@p2", TxtAddProductPrice.Text);
                        command.Parameters.AddWithValue("@p3", TxtAddProductStock.Text);
                        command.ExecuteNonQuery();




                        SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
                        command1.Parameters.AddWithValue("@p1", DpID1);
                        command1.Parameters.AddWithValue("@p2", Movementsdate);
                        command1.Parameters.AddWithValue("@p3", Movementshour);
                        command1.Parameters.AddWithValue("@p4", "Yeni Ürün Kaydı Yapıldı Ürün Adı= " + TxtAddProductName.Text);
                        command1.ExecuteNonQuery();





                        MessageBox.Show("Yeni Ürün Kaydı Yapıldı");


                    }


                    connection.Close();
                    Productcmblist();
                    productlist();
                }
                else
                {
                    MessageBox.Show("Lütfen Girilen Bilgileri Kontrol Ediniz");
                }
            }
            else
            {
                MessageBox.Show("Alanlar Boş Geçilemez");
            }
        }

        private void BtnProductUpdate_Click(object sender, EventArgs e)
        {
            if (TxtUpdateProductPrice.Text.Trim() != "" && TxtUpdateProductStocks.Text.Trim() != "")
            {
                if (Convert.ToInt32(TxtUpdateProductStocks.Text.Trim()) >= 0 && Convert.ToDecimal(TxtUpdateProductPrice.Text) > 0)
                {
                    if (LblProductCode.Text != "0")
                    {
                        Movements();
                        connection.Open();
                        SqlCommand command = new SqlCommand("Update TblStock set ProdutName=@p1,Price=@p2,Stock=Stock+@p3 where ProductID=@p4", connection);
                        command.Parameters.AddWithValue("@p4", LblProductCode.Text);
                        command.Parameters.AddWithValue("@p1", CmbUpdateProductName.Text);
                        command.Parameters.AddWithValue("@p2", Convert.ToDecimal(TxtUpdateProductPrice.Text));
                        command.Parameters.AddWithValue("@p3", TxtUpdateProductStocks.Text);
                        command.ExecuteNonQuery();


                        SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken,MovementsProduct) values (@p1,@p2,@p3,@p4,@p5)", connection);
                        command1.Parameters.AddWithValue("@p1", DpID1);
                        command1.Parameters.AddWithValue("@p2", Movementsdate);
                        command1.Parameters.AddWithValue("@p3", Movementshour);
                        command1.Parameters.AddWithValue("@p4", "Ürün Güncellendi. Ürün Adı= " + CmbUpdateProductName.Text);
                        command1.Parameters.AddWithValue("@p5", LblProductCode.Text);
                        command1.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Ürün Güncellendi");
                        productlist();
                        Productcmblist();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen Güncellemek İstediğiniz Ürünü Seçiniz");
                    }

                }
                else
                {
                    MessageBox.Show("Lütfen Girilen Bilgileri Kontrol Ediniz");
                }
            }
            else
            {
                MessageBox.Show("Lütfen Bütün Alanları Doldurunuz");
            }
        }

        private void BtnFuelSave_Click(object sender, EventArgs e)
        {
            if (TxtAddFuelName.Text.Trim() != "" && TxtAddFuelPrice.Text.Trim() != "" && TxtAddFuelStock.Text.Trim() != "")
            {
                connection.Open();
                SqlCommand command = new SqlCommand("select * from TblFuel where FuelName=@p1", connection);
                command.Parameters.AddWithValue("@p1", TxtAddFuelName.Text);
                SqlDataReader dr = command.ExecuteReader();
                if (dr.Read())
                {
                    MessageBox.Show("Akaryakıt Tipi Daha Öncesinde Kayıt Edilmiş");
                }
                else
                {
                    dr.Close();
                    Movements();
                    if (Convert.ToDecimal(TxtAddFuelPrice.Text) > 0 && Convert.ToDecimal(TxtAddFuelStock.Text) > 0)
                    {
                        SqlCommand command2 = new SqlCommand("insert into TblFuel (FuelName,FuelPrice,FuelStock) values (@p1,@p2,@p3)", connection);
                        command2.Parameters.AddWithValue("@p1", TxtAddFuelName.Text);
                        command2.Parameters.AddWithValue("@p2", TxtAddFuelPrice.Text);
                        command2.Parameters.AddWithValue("@p3", TxtAddFuelStock.Text);
                        command2.ExecuteNonQuery();
                        MessageBox.Show("Yakıt Kaydı Yapıldı");

                        SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
                        command1.Parameters.AddWithValue("@p1", DpID1);
                        command1.Parameters.AddWithValue("@p2", Movementsdate);
                        command1.Parameters.AddWithValue("@p3", Movementshour);
                        command1.Parameters.AddWithValue("@p4", "Yeni Yakıt Kaydı Yapıldı Ürün Adı= " + TxtAddFuelName.Text);
                        command1.ExecuteNonQuery();
                    }
                    else
                    {
                        MessageBox.Show("Lütfen Girilen Bilgileri Kontrol Ediniz");
                    }

                }
                dr.Close();
                connection.Close();
                Fuelcmblist();
                FuelList();
            }
        }

        private void CmbUpdateProductName_TextChanged(object sender, EventArgs e)
        {
            string productid;
            connection.Open();
            SqlCommand command = new SqlCommand("select * from TblStock where ProdutName like '%" + CmbUpdateProductName.Text + "%'", connection);


            SqlDataReader dr = command.ExecuteReader();
            if (dr.Read())
            {

                productid = dr[0].ToString();
                dr.Close();
                SqlDataAdapter da = new SqlDataAdapter("select ProductID as 'Ürün Kodu',ProdutName as 'Adı',Price as 'Fiyat',Stock as 'Stok' from TblStock where ProductID=@p1", connection);
                da.SelectCommand.Parameters.AddWithValue("@p1", productid);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageBox.Show("Kayıt Bulunamadı Lütfen Tablodan Ürünü Seçiniz");
            }
            dr.Close();
            connection.Close();
        }

        private void BtnFuelUpdate_Click(object sender, EventArgs e)
        {
            if (TxtUpdateFuelPrice.Text.Trim() != "" && TxtUpdateFuelPrice.Text.Trim() != "")
            {
                if (Convert.ToInt32(TxtUpdateFuelPrice.Text.Trim()) >= 0 && Convert.ToDecimal(TxtUpdateProductStocks.Text) > 0)
                {
                    if (LblProductCode.Text != "0")
                    {
                        Movements();
                        connection.Open();
                        SqlCommand command4 = new SqlCommand("Select FuelStockMax from where FuelID=@p1", connection);
                        command4.Parameters.AddWithValue("@p1", LblFuelCode);
                        SqlDataReader dr = command4.ExecuteReader();
                        decimal a = Convert.ToDecimal(TxtUpdateFuelStock.Text);
                        Math.Round(a);

                        decimal max = Convert.ToDecimal(dr[0].ToString());
                        dr.Close();
                        Math.Round(max);
                        if (max >= a)
                        {

                            SqlCommand command = new SqlCommand("Update TblStock set FuelName=@p1,FuelPrice=@p2,FuelStock=FuelStock+@p3 where FuelID=@p4", connection);
                            command.Parameters.AddWithValue("@p4", LblFuelCode.Text);
                            command.Parameters.AddWithValue("@p1", CmbUpdateProductName.Text);
                            command.Parameters.AddWithValue("@p2", TxtUpdateFuelPrice.Text);
                            command.Parameters.AddWithValue("@p3", TxtUpdateFuelStock.Text);
                            command.ExecuteNonQuery();


                            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken,MovementsFuel) values (@p1,@p2,@p3,@p4,@p5)", connection);
                            command1.Parameters.AddWithValue("@p1", DpID1);
                            command1.Parameters.AddWithValue("@p2", Movementsdate);
                            command1.Parameters.AddWithValue("@p3", Movementshour);
                            command1.Parameters.AddWithValue("@p4", "Ürün Güncellendi. Ürün Adı= " + CmbUpdateFuelName.Text);
                            command1.Parameters.AddWithValue("@p5", LblFuelCode.Text);
                            command1.ExecuteNonQuery();

                            MessageBox.Show("Ürün Güncellendi");
                        }
                        else
                        {
                            MessageBox.Show("Eklenen Yakıt Miktarı Depo Hacminden Fazla ");
                        }
                        connection.Close();


                    }
                    else
                    {
                        MessageBox.Show("Lütfen Güncellemek İstediğiniz Ürünü Seçiniz");
                    }

                }
                else
                {
                    MessageBox.Show("Lütfen Girilen Bilgileri Kontrol Ediniz");
                }
            }
            else
            {
                MessageBox.Show("Lütfen Bütün Alanları Doldurunuz");
            }
            Fuelcmblist();
            FuelList();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e1)
        {
            if (e1.RowIndex != -1)
            {

            }
        }
        int x;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            


            LblProductCode.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            CmbUpdateProductName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            TxtUpdateProductPrice.Text = dataGridView1.Rows[0].Cells[2].Value.ToString();
            TxtUpdateProductStocks.Text = dataGridView1.Rows[0].Cells[3].Value.ToString();
        }


        private void CmbUpdateProductName_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {


            LblFuelCode.Text = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
            CmbUpdateFuelName.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
            TxtUpdateFuelPrice.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
            TxtUpdateFuelStock.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
        }

        private void FrmWarehouseOperations_Load(object sender, EventArgs e)
        {


            Movements();
            connection.Open();
            SqlCommand command1 = new SqlCommand("insert into TblMovements (MovementsPersonel,Date,Hour,ActionTaken) values (@p1,@p2,@p3,@p4)", connection);
            command1.Parameters.AddWithValue("@p1", DpID1);
            command1.Parameters.AddWithValue("@p2", Movementsdate);
            command1.Parameters.AddWithValue("@p3", Movementshour);
            command1.Parameters.AddWithValue("@p4", this.Text + " Ekranına Giriş Yapıldı");
            command1.ExecuteNonQuery();
            connection.Close();

            Fuelcmblist();
            Productcmblist();
            productlist();
            FuelList();
        }
    }
}
