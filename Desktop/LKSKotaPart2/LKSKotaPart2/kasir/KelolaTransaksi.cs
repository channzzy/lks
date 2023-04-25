using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKSKotaPart2.kasir
{
    public partial class KelolaTransaksi : Form
    {
        koneksi con = new koneksi();
        string userid, tipeuser, nama;
        int totalakhir, kembalian = 0;
        string idtransaksi;
        public KelolaTransaksi(String userid, String tipeuser, String nama)
        {
            InitializeComponent();
            this.userid = userid;
            this.tipeuser = tipeuser;
            this.nama = nama;

            InitiateRefresh();
        }
        private void InitiateRefresh()
        {
            Btn_Tambah.Enabled = false;
            Btn_Save.Enabled = false;
            Btn_Bayar.Enabled = false;
            button1.Enabled = false;

            Txt_Harga.Enabled = false;
            Txt_Total.Enabled = false;
            cmbx_menu.SelectedIndex = -1;

            loadCB();
        }
        private void loadCB()
        {
            cmbx_menu.DisplayMember = "Text";
            cmbx_menu.ValueMember = "Value";

            DataTable cb = new DataTable();
            con.select("select id_barang,kode_barang,nama_barang,harga_satuan from tbl_barang");
            con.adp.Fill(cb);

            foreach (DataRow dtr in cb.Rows)
            {
                cmbx_menu.Items.Add(new
                {
                    Value = dtr[0].ToString(),
                    Text = dtr[1].ToString() + "-" + dtr[2].ToString(),
                    Kode = dtr[0].ToString(),
                    Nama = dtr[2].ToString(),
                    Harga = dtr[3].ToString()
                });
            }
        }
        private void KelolaTransaksi_Load(object sender, EventArgs e)
        {
            

        }

        private void cmbx_menu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbx_menu.SelectedIndex != -1)
            {
                Txt_Harga.Text = (cmbx_menu.SelectedItem as dynamic).Harga;
                Btn_Tambah.Enabled = true;
            }
            else
            {
                Txt_Harga.Text = "";
            }
            hitungtotal();
        }
        private void hitungtotal()
        {
            int harga = 0;
            if (cmbx_menu.SelectedIndex != -1)
            {
                harga = int.Parse((cmbx_menu.SelectedItem as dynamic).Harga);
            }
            int qty = 0;
            if (Txt_Qty.Text != "")
            {
                qty = int.Parse(Txt_Qty.Text);
            }
            int total = harga * qty;
            Txt_Total.Text = total.ToString();
            
        }

        private void Txt_Qty_TextChanged(object sender, EventArgs e)
        {
            hitungtotal();
        }

        private void Btn_Tambah_Click(object sender, EventArgs e)
        {
            var barang = (cmbx_menu.SelectedItem as dynamic);
            int newID = (dataGridView1.Rows.Count + 1);
            string idtrx = "TR" + newID.ToString().PadLeft(3, '0');

            dataGridView1.Rows.Add(idtrx, barang.Value, barang.Kode, barang.Nama, barang.Harga, Txt_Qty.Text, Txt_Total.Text);

            totalakhir += int.Parse(Txt_Total.Text);
            lbltotal.Text = totalakhir.ToString();
            Btn_Bayar.Enabled = true;
        }

        private void Btn_Bayar_Click(object sender, EventArgs e)
        {
            int bayar = int.Parse(Txt_Bayar.Text);
            int total = int.Parse(lbltotal.Text);

            if (bayar < total)
            {
                MessageBox.Show("Uang Anda Kurang");
            }
            else
            {
                kembalian = bayar - total;
                lblkembalian.Text = kembalian.ToString();
                Btn_Save.Enabled = true;
                button1.Enabled = true;
            }
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            con.cud("insert into tbl_transaksi(no_transaksi,tgl_transaksi,total_bayar,id_user)values('" + DateTime.Now.ToString("yyyyMMddHHmmss") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + lbltotal.Text + "','"+userid+"')");

            DataTable dt = new DataTable();
            con.select("select max(id_transaksi) from tbl_transaksi");
            con.adp.Fill(dt);

            foreach (DataRow dtr in dt.Rows)
            {
                idtransaksi = dtr[0].ToString();
            }
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                con.cud("insert into tbl_transaksidetail (id_transaksi,id_barang,qty,harga_satuan,subtotal) values('" + idtransaksi + "','" + row.Cells[1].Value.ToString() + "','" + row.Cells[5].Value.ToString() + "','" + row.Cells[4].Value.ToString() + "','" + row.Cells[6].Value.ToString() + "')");
                MessageBox.Show("Data Berhasil Ditambah");
            }
        }

        private void Btn_Logout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Apakah Anda Yakin?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (DialogResult.Yes == result)
            {
                FormLogin fl = new FormLogin();
                this.Hide();
                con.cud("update tbl_log set waktu='" + DateTime.Now.ToString() + "',aktivitas='Logout'");
                MessageBox.Show("Anda Berhasil Logout");
                fl.ShowDialog();
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                e.Graphics.DrawString("Laporan Transaksi",new Font("Arial",19,FontStyle.Regular), Brushes.Red, new Point(185,10));
                e.Graphics.DrawString("Pada: "+DateTime.Now.ToString("dddd-MM-yyyy"),new Font("Arial",19,FontStyle.Regular), Brushes.Red, new Point(0,40));
                e.Graphics.DrawString("ID Transaksi:" + totalakhir.ToString(), new Font("Arial",14,FontStyle.Regular), Brushes.Black, new Point(50,60));
            }
        }
    }
}
