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

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {

        SqlConnection cn = new SqlConnection(@"Data Source=" + Globals.DS + ";Initial Catalog=" + Globals.IC + "; Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter da;
        DataTable dt = new DataTable();
       


        public Form4()
        {
            InitializeComponent();

            cn.Open();
            da = new SqlDataAdapter("select U.id_utilisateur,username, saisie, consultation,gestion_cmpt,pass from utilisateur U,droits D where U.id_utilisateur=D.id_utilisateur ", cn);
            da.Fill(dt);

            


            dataGridView1.DataSource = dt;
            cn.Close();
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

       


        void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            checkBox1.Checked = false;
            checkBox2.Checked = false;
            checkBox3.Checked = false;
        }

        


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                checkBox1.Checked = ((bool)dataGridView1.SelectedRows[0].Cells[2].Value)? true:false;
                checkBox2.Checked = ((bool)dataGridView1.SelectedRows[0].Cells[3].Value) ? true : false;
                checkBox3.Checked = ((bool)dataGridView1.SelectedRows[0].Cells[4].Value) ? true : false;
                textBox2.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text) || !string.IsNullOrEmpty(textBox2.Text))
            {
                cn.Open();

                cmd = new SqlCommand("insert into utilisateur values ('" + textBox1.Text + "','" + textBox2.Text + "') ", cn);
                cmd.ExecuteNonQuery();

                int a = 0, b = 0, c = 0;
                if (checkBox1.Checked == true)
                {
                    a = 1;
                }
                if (checkBox2.Checked == true)
                {
                    b = 1;
                }
                if (checkBox3.Checked == true)
                {
                    c = 1;
                }
                cmd = new SqlCommand("insert into droits  values (scope_identity(),'" + a + "','" + b + "','" + c + "') ", cn);
                cmd.ExecuteNonQuery();

                MessageBox.Show("utilisateur Ajoute", "Ajouter", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dt = new DataTable();
                da = new SqlDataAdapter("select U.id_utilisateur,username, saisie, consultation,gestion_cmpt,pass from utilisateur U,droits D where U.id_utilisateur=D.id_utilisateur ", cn);
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                cn.Close();
                clear();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            cn.Open();
            string id = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value.ToString();
            cmd = new SqlCommand("delete from utilisateur where id_utilisateur='" + id + "'", cn);
            cmd.ExecuteNonQuery();

            MessageBox.Show("supprission avec succes", "Supprission", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dt = new DataTable();
            da = new SqlDataAdapter("select U.id_utilisateur,username, saisie, consultation,gestion_cmpt,pass from utilisateur U,droits D where U.id_utilisateur=D.id_utilisateur ", cn);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cn.Close();
            clear();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            cn.Open();

            //cmd = new SqlCommand("select id_utilisateur from  utilisateur where  username='" + comboBox1.Text + "'", cn);
            //dr = cmd.ExecuteReader();

            //int id = Convert.ToInt32(dataGridView1.SelectedColumns[0].Cells[0].Value.ToString());
            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[0].Value);
            //int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);

            cmd = new SqlCommand("update utilisateur set  username='" + textBox1.Text + "' , pass='" + textBox2.Text + "' where id_utilisateur='" + id + "'", cn);
            cmd.ExecuteNonQuery();

            int a = 0, b = 0, c = 0;
            if (checkBox1.Checked == true)
            {
                a = 1;
            }
            if (checkBox2.Checked == true)
            {
                b = 1;
            }
            if (checkBox3.Checked == true)
            {
                c = 1;
            }
            cmd = new SqlCommand("update droits set saisie='" + a + "' , consultation='" + b + "', gestion_cmpt= '" + c + "' where id_utilisateur='" + id + "'", cn);
            cmd.ExecuteNonQuery();

            MessageBox.Show("modification avec succes", "Modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            dt = new DataTable();
            da = new SqlDataAdapter("select U.id_utilisateur,username, saisie, consultation,gestion_cmpt,pass from utilisateur U,droits D where U.id_utilisateur=D.id_utilisateur ", cn);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            cn.Close();
            clear();
        }
    }
}
