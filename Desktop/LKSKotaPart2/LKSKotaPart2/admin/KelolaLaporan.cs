using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKSKotaPart2.admin
{
    public partial class KelolaLaporan : Form
    {
        koneksi con = new koneksi();
        DataTable dt = new DataTable();
        string userid, tipeuser, nama;
        public KelolaLaporan(String userid, String tipeuser, String nama)
        {
            InitializeComponent();
            this.userid = userid;
            this.tipeuser = tipeuser;
            this.nama = nama;
            DateTime date = DateTime.Now;
            dtpicker1.Value = new DateTime(date.Year, date.Month, 1);
            dtpicker2.Value = date;
            viewdg();
        }
        public void viewdg()
        {
            dt.Clear();
            dataGridView1.Refresh();
            dataGridView1.DataSource = dt;
            FillData();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "ID Transaksi";
            dataGridView1.Columns[1].HeaderText = "Tanggal Transaksi";
            dataGridView1.Columns[2].HeaderText = "Total Pembayaran";
            dataGridView1.Columns[3].HeaderText = "Nama Kasir";
        }
        public void FillData()
        {
            dt.Clear();
            con.select("select tt.id_transaksi,tt.tgl_transaksi,tt.total_bayar,tu.username from tbl_transaksi as tt inner join tbl_user as tu on (tt.id_user = tu.id_user) where tgl_transaksi between '"+dtpicker1.Value.ToString("yyyy-MM-dd")+"' and '"+dtpicker2.Value.ToString("yyyy-MM-dd")+"'");
            con.adp.Fill(dt);
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

        private void Btn_Filter_Click(object sender, EventArgs e)
        {
            FillData();
        }

        private void Btn_Chart_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt.Clear(); 
            con.select("select convert(varchar(10), tgl_transaksi, 111) as tanggal, sum(total_bayar) total from tbl_transaksi where tgl_transaksi between '" + dtpicker1.Value.ToString("yyyy-MM-dd") + "' and '" + dtpicker2.Value.ToString("yyyy-MM-dd") + "' group by tgl_transaksi");
            con.adp.Fill(dt);

            chart1.Series.Clear();
            chart1.Titles.Clear();

            chart1.Series.Add("Omset");

            foreach (DataRow dtr in dt.Rows)
            {
                chart1.Series["Omset"].Points.AddXY(dtr[0].ToString(), int.Parse(dtr[1].ToString()));
            }
        }
    }
}
