using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKSKotaPart2
{
    public partial class FormLogin : Form
    {
        koneksi con = new koneksi();
        DataTable dt = new DataTable();
        string userid, tipeuser, nama;
        public FormLogin()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Txt_Username.Text = "Admin";
            Txt_Password.Text = "Admin";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Txt_Username.Text = "Kasir";
            Txt_Password.Text = "Kasir";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Txt_Username.Text = "Gudang";
            Txt_Password.Text = "Gudang";
        }

        private void Btn_Login_Click(object sender, EventArgs e)
        {
            if ("".Equals(Txt_Username.Text) || "".Equals(Txt_Password.Text))
            {
                MessageBox.Show("Harap Isi Akun!");
            }
            else
            {
                dt.Clear();
                con.select("select * from tbl_user where username='" + Txt_Username.Text + "' and password='" + Txt_Password.Text + "'");
                con.adp.Fill(dt);

                if (dt.Rows.Count <= 0)
                {
                    MessageBox.Show("Akun Yang Anda Masukan Salah");
                }
                else
                {
                    foreach (DataRow dtr in dt.Rows)
                    {
                        userid = dtr[0].ToString();
                        tipeuser = dtr[1].ToString();
                        nama = dtr[2].ToString();
                    }
                    con.cud("insert into tbl_log(waktu,aktivitas,id_user) values('" + DateTime.Now.ToString() + "','Login','"+userid+"')");
                    if (tipeuser == "Admin")
                    {
                        admin.LogActivity la = new admin.LogActivity(userid, tipeuser, nama);
                        this.Hide();
                        la.ShowDialog();
                        this.Close();
                    }
                    else if (tipeuser == "Kasir")
                    {
                        kasir.KelolaTransaksi kt = new kasir.KelolaTransaksi(userid, tipeuser, nama);
                        this.Hide();
                        kt.ShowDialog();
                        this.Close();                    
                    }
                    else if (tipeuser == "Gudang")
                    {
                        gudang.KelolaBarang kb = new gudang.KelolaBarang(userid, tipeuser, nama);
                        this.Hide();
                        kb.ShowDialog();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Ada Sesuatu Yang Salah");
                    }
                }
            }
        }

        private void Btn_Reset_Click(object sender, EventArgs e)
        {
            Txt_Username.Clear();
            Txt_Password.Clear();
        }
    }
}
