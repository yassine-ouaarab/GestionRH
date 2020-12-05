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
    public partial class Gest_BQ : Form
    {
        SqlConnection cn = new SqlConnection(@"Data Source=" + Globals.DS + ";Initial Catalog=" + Globals.IC + "; Integrated Security=true;");
        DataSet ds = new DataSet();
        SqlDataAdapter da;

        public Form1 f1;
        public Gest_BQ()
        {
            InitializeComponent();

            refresh_grid();

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            get_ready_to_create();
        }

        void refresh_grid()
        {
            da = new SqlDataAdapter("select * from banque ", cn);
            ds = new DataSet();
            da.Fill(ds, "banque");
            dataGridView1.DataSource = ds.Tables["banque"];
        }

        private void Gest_BQ_Load(object sender, EventArgs e)
        {

        }



       

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            
        }

        private void Gest_BQ_Click(object sender, EventArgs e)
        {
            get_ready_to_create();
        }

        void get_ready_to_create()
        {
            dataGridView1.ClearSelection();
            textBox1.Enabled = true;
            pictureBox1.Text = "Creer";
            textBox1.Text = "";
            numericUpDown1.Value = (decimal)0.0f;
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                textBox1.Enabled = false;
                textBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                numericUpDown1.Value =(decimal) dataGridView1.SelectedRows[0].Cells[2].Value;
                pictureBox1.Text = "Modifier";
            }
            else
            {
                get_ready_to_create();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Text == "Creer")
            {
                if (!string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    // remplir DataRow

                    DataRow dr;
                    dr = ds.Tables["banque"].NewRow();
                    dr[1] = textBox1.Text;
                    dr[2] = numericUpDown1.Value;

                    //ajout de la ligne au DataSet
                    ds.Tables["banque"].Rows.Add(dr);

                    //Migration vers la base de donne
                    da = new SqlDataAdapter("select * from banque ", cn);
                    SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(da);
                    da.Update(ds, "banque");

                    MessageBox.Show("Banque: " + textBox1.Text + " Ajoute avec succes");
                    f1.refresh_banque_combobox();
                    refresh_grid();
                }
                else
                {
                    MessageBox.Show("Veuillez saisir un texte valide");
                }
            }
            else
            {


                for (int i = 0; i < ds.Tables["banque"].Rows.Count; i++)
                {
                    if (dataGridView1.SelectedRows[0].Cells[0].Value.ToString() == ds.Tables["banque"].Rows[i][0].ToString())
                    {
                        ds.Tables["banque"].Rows[i][2] = numericUpDown1.Value;

                        //Migration vers la base de donne
                        da = new SqlDataAdapter("select * from banque ", cn);
                        SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(da);
                        da.Update(ds, "banque");


                        f1.refresh_banque_combobox();
                        refresh_grid();
                        break;
                    }
                }



            }
        }
    }
}
