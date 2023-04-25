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
    public partial class LogActivity : Form
    {
        koneksi con = new koneksi();
        DataTable dt = new DataTable();
        string userid, tipeuser, nama;
        public LogActivity(String userid,String tipeuser,String nama)
        {
            InitializeComponent();
            this.userid = userid;
            this.tipeuser = tipeuser;
            this.nama = nama;
            dtpicker1.Value = DateTime.Now;
            viewdg();
        }

        public void viewdg()
        {
            dt.Clear();
            dataGridView1.Refresh();
            dataGridView1.DataSource = dt;
            fillData();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "ID Log";
            dataGridView1.Columns[1].HeaderText = "Username";
            dataGridView1.Columns[2].HeaderText = "Waktu";
            dataGridView1.Columns[3].HeaderText = "Aktivitas";
        }
        public void fillData()
        {
            dt.Clear();
            con.select("select tl.id_log,tu.nama,tl.waktu,tl.aktivitas from tbl_log as tl inner join tbl_user as tu on(tl.id_user=tu.id_user) where cast(waktu as date)='" + dtpicker1.Value.ToString("yyyy-MM-dd") + "'");
            con.adp.Fill(dt);
        }

        private void LogActivity_Load(object sender, EventArgs e)
        {

        }

        private void Btn_Filter_Click(object sender, EventArgs e)
        {
            fillData();
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

        private void Btn_KelolaUser_Click(object sender, EventArgs e)
        {
            admin.KelolaUser ku = new KelolaUser(userid, tipeuser, nama);
            this.Hide();
            ku.ShowDialog();
            this.Close();
        }

        private void Btn_KelolaLaporan_Click(object sender, EventArgs e)
        {
            admin.KelolaLaporan kl = new KelolaLaporan(userid, tipeuser, nama);
            this.Hide();
            kl.ShowDialog();
            this.Close();
        }
    }
}
