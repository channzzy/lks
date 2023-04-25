using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKSKotaPart2.gudang
{
    public partial class KelolaBarang : Form
    {
        koneksi con = new koneksi();
        DataTable dt = new DataTable();
        string userid, tipeuser, nama;
        public KelolaBarang(String userid, String tipeuser, String nama)
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
            cmbx_satuan.SelectedIndex = -1;
            Txt_Nama.Clear();
            Txt_Kode.Clear();
            Txt_Jumlah.Clear();
            Txt_Harga.Clear();

            dateTimePicker1.Value = DateTime.Now;
            viewdg();
            AddCb();
            Btn_Edit.Enabled = false;
            Btn_Hapus.Enabled = false;
        }
        public void viewdg()
        {
            dt.Clear();
            dataGridView1.Refresh();
            dataGridView1.DataSource=dt;
            con.select("select * from tbl_barang");
            con.adp.Fill(dt);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns[0].HeaderText = "ID Barang";
            dataGridView1.Columns[1].HeaderText = "Kode Barang";
            dataGridView1.Columns[2].HeaderText = "Nama Barang";
            dataGridView1.Columns[3].HeaderText = "Expired Date";
            dataGridView1.Columns[4].HeaderText = "Jumlah";
            dataGridView1.Columns[5].HeaderText = "Satuan";
            dataGridView1.Columns[6].HeaderText = "Harga Satuan";
        }
        private void AddCb()
        {
            cmbx_satuan.Items.Clear();
            cmbx_satuan.Refresh();

            cmbx_satuan.Items.Add("Botol");
            cmbx_satuan.Items.Add("Picies");
            cmbx_satuan.Items.Add("Box");
        }

        private void Btn_Tambah_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Apakah Anda Yakin?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogResult.Yes == result)
                {
                    if ("".Equals(Txt_Nama.Text) || "".Equals(cmbx_satuan.Text) || "".Equals(Txt_Kode.Text) || "".Equals(Txt_Jumlah.Text) || "".Equals(Txt_Harga.Text))
                    {
                        MessageBox.Show("Data Tidak Boleh Kosong");
                    }
                    else
                    {
                        con.cud("insert into tbl_barang(kode_barang,nama_barang,expired_date,jumlah_barang,satuan,harga_satuan)values('" + Txt_Kode.Text + "','" + Txt_Nama.Text + "','" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "','" + Txt_Jumlah.Text + "','" + cmbx_satuan.Text + "','" + Txt_Harga.Text + "')");
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

        private void Txt_Jumlah_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                errorProvider1.SetError(Txt_Jumlah, "Hanya Boleh Angka");
            }
        }

        private void Txt_Harga_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                errorProvider1.SetError(Txt_Harga, "Hanya Boleh Angka");
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

        private void Btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Apakah Anda Yakin?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogResult.Yes == result)
                {
                    if ("".Equals(Txt_Nama.Text) || "".Equals(cmbx_satuan.Text) || "".Equals(Txt_Kode.Text) || "".Equals(Txt_Jumlah.Text) || "".Equals(Txt_Harga.Text))
                    {
                        MessageBox.Show("Data Tidak Boleh Kosong");
                    }
                    else
                    {
                        con.cud("update tbl_barang set kode_barang='" + Txt_Kode.Text + "',nama_barang='" + Txt_Nama.Text + "',expired_date='" + dateTimePicker1.Value.ToString("yyyy-MM-dd") + "',jumlah_barang='" + Txt_Jumlah.Text + "',satuan='" + cmbx_satuan.Text + "',harga_satuan='" + Txt_Harga.Text + "' where id_barang='" + Txt_ID.Text + "' ");
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
        private void cari()
        {
            dt.Clear();
            con.select("select * from tbl_barang where kode_barang like'%" + Txt_Search.Text + "%' or nama_barang like'%" + Txt_Search.Text + "%' or expired_date like'%" + Txt_Search.Text + "%' or jumlah_barang like'%" + Txt_Search.Text + "%' or satuan like'%" + Txt_Search.Text + "%' or harga_satuan like'%" + Txt_Search.Text + "%'");
            con.adp.Fill(dt);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                Txt_ID.Text = row.Cells[0].Value.ToString();
                Txt_Kode.Text = row.Cells[1].Value.ToString();
                Txt_Nama.Text = row.Cells[2].Value.ToString();
                dateTimePicker1.Value = Convert.ToDateTime(row.Cells[3].Value.ToString());
                Txt_Jumlah.Text = row.Cells[4].Value.ToString();
                cmbx_satuan.Text = row.Cells[5].Value.ToString();
                Txt_Harga.Text = row.Cells[6].Value.ToString();

                Btn_Edit.Enabled = true;
                Btn_Hapus.Enabled = true;
                Btn_Tambah.Enabled = false;

            }
        }

        private void Btn_Hapus_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Apakah Anda Yakin?", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (DialogResult.Yes == result)
                {
                    if ("".Equals(Txt_Nama.Text) || "".Equals(cmbx_satuan.Text) || "".Equals(Txt_Kode.Text) || "".Equals(Txt_Jumlah.Text) || "".Equals(Txt_Harga.Text))
                    {
                        MessageBox.Show("Data Tidak Boleh Kosong");
                    }
                    else
                    {
                        con.cud("delete from tbl_barang where id_barang='" + Txt_ID.Text + "' ");
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

        private void Txt_Search_TextChanged(object sender, EventArgs e)
        {
            cari();
        }
    }
}
