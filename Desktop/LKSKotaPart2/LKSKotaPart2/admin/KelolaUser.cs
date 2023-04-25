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
    public partial class KelolaUser : Form
    {
        koneksi con = new koneksi();
        DataTable dt = new DataTable();
        string userid, tipeuser, nama;
        public KelolaUser(String userid, String tipeuser, String nama)
        {
            InitializeComponent();
            this.userid = userid;
            this.tipeuser = tipeuser;
            this.nama = nama;
            InitiateRefresh();
        }
        public void InitiateRefresh()
        {
            lblID.Enabled = false;
            Txt_ID.Enabled = false;
            lblID.Visible = false;
            Txt_ID.Visible = false;

            Txt_ID.Clear();
            cmbx_tipe.SelectedIndex = -1;
            Txt_Nama.Clear();
            Txt_Telpon.Clear();
            Txt_Username.Clear();
            Txt_Password.Clear();

            viewdg();
            AddCb();
            Btn_Edit.Enabled = false;
            Btn_Hapus.Enabled = false;
        }
        public void viewdg(){
            dt.Clear();
            dataGridView1.Refresh();
            dataGridView1.DataSource=dt;
            con.select("select * from tbl_user");
            con.adp.Fill(dt);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "ID User";
            dataGridView1.Columns[1].HeaderText = "Tipe User";
            dataGridView1.Columns[2].HeaderText = "Nama User";
            dataGridView1.Columns[3].HeaderText = "Alamat";
            dataGridView1.Columns[4].HeaderText = "Telepon";
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;

        }
        private void AddCb()
        {
            cmbx_tipe.Items.Clear();
            cmbx_tipe.Refresh();

            cmbx_tipe.Items.Add("Gudang");
            cmbx_tipe.Items.Add("Kasir");
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

        private void Btn_Tambah_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Apakah Anda Yakin?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogResult.Yes == result)
                {
                    if ("".Equals(Txt_Nama.Text) || "".Equals(cmbx_tipe.Text) || "".Equals(Txt_Telpon.Text) || "".Equals(Txt_Alamat.Text) || "".Equals(Txt_Username.Text) || "".Equals(Txt_Password.Text))
                    {
                        MessageBox.Show("Data Tidak Boleh Kosong");
                    }
                    else
                    {
                        con.cud("insert into tbl_user(tipe_user,nama,alamat,telpon,username,password)values('" + cmbx_tipe.Text + "','" + Txt_Nama.Text + "','" + Txt_Alamat.Text + "','" + Txt_Telpon.Text + "','" + Txt_Username.Text + "','" + Txt_Password.Text + "')");
                        MessageBox.Show("Data Berhasil Ditambah");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                InitiateRefresh();
            }
        }

        private void cmbx_tipe_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void Btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Apakah Anda Yakin?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogResult.Yes == result)
                {
                    if ("".Equals(Txt_Nama.Text) || "".Equals(cmbx_tipe.Text) || "".Equals(Txt_Telpon.Text) || "".Equals(Txt_Alamat.Text) || "".Equals(Txt_Username.Text) || "".Equals(Txt_Password.Text))
                    {
                        MessageBox.Show("Data Tidak Boleh Kosong");
                    }
                    else
                    {
                        con.cud("update tbl_user set tipe_user='" + cmbx_tipe.Text + "',nama='" + Txt_Nama.Text + "',alamat='" + Txt_Alamat.Text + "',telpon='" + Txt_Telpon.Text + "',username='" + Txt_Username.Text + "',password='" + Txt_Password.Text + "' where id_user='"+Txt_ID.Text+"' ");
                        MessageBox.Show("Data Berhasil Diedit");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                InitiateRefresh();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                Txt_ID.Text = row.Cells[0].Value.ToString();
                cmbx_tipe.Text = row.Cells[1].Value.ToString();
                Txt_Nama.Text = row.Cells[2].Value.ToString();
                Txt_Alamat.Text = row.Cells[3].Value.ToString();
                Txt_Telpon.Text = row.Cells[4].Value.ToString();
                Txt_Username.Text = row.Cells[5].Value.ToString();
                Txt_Password.Text = row.Cells[6].Value.ToString();

                Btn_Edit.Enabled = true;
                Btn_Hapus.Enabled = true;
                Btn_Tambah.Enabled = false;
            }
        }
        private void cari()
        {
            dt.Clear();
            con.select("select * from tbl_user where tipe_user like'%" + textBox5.Text + "%' or nama like'%" + textBox5.Text + "%' or alamat like'%" + textBox5.Text + "%' or telpon like'%" + textBox5.Text + "%' or username like'%" + textBox5.Text + "%' or tipe_user like'%" + textBox5.Text + "%' or password like'%" + textBox5.Text + "%'");
            con.adp.Fill(dt);
        }
        private void Btn_Hapus_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Apakah Anda Yakin?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogResult.Yes == result)
                {
                    if ("".Equals(Txt_Nama.Text) || "".Equals(cmbx_tipe.Text) || "".Equals(Txt_Telpon.Text) || "".Equals(Txt_Alamat.Text) || "".Equals(Txt_Username.Text) || "".Equals(Txt_Password.Text))
                    {
                        MessageBox.Show("Data Tidak Boleh Kosong");
                    }
                    else
                    {
                        con.cud("delete from tbl_user where id_user='" + Txt_ID.Text + "' ");
                        MessageBox.Show("Data Berhasil Dihapus");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                InitiateRefresh();
            }
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            cari();
        }
    }
}
