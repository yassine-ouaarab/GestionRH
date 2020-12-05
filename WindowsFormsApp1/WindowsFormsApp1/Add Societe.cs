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
    public partial class Add_Societe : Form
    {
        SqlConnection cn = new SqlConnection(@"Data Source=" + Globals.DS + ";Initial Catalog=" + Globals.IC + "; Integrated Security=true;");
        DataSet ds = new DataSet();
        SqlDataAdapter da;

       public  Form1 f1;


        public Add_Societe()
        {
            InitializeComponent();
             

            da = new SqlDataAdapter("select * from societe ", cn);
            da.Fill(ds, "societe");

        
        }

        private void Add_Societe_Load(object sender, EventArgs e)
        {

        }
       


        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == comboBox1.Items[0].ToString() || comboBox1.Text == comboBox1.Items[1].ToString())
            {
                // remplir DataRow

                DataRow dr;
                dr = ds.Tables["societe"].NewRow();
                dr[1] = textBox1.Text;
                dr[2] = comboBox1.Text;

                //ajout de la ligne au DataSet
                ds.Tables["societe"].Rows.Add(dr);

                //Migration vers la base de donne
                da = new SqlDataAdapter("select * from societe ", cn);
                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(da);
                da.Update(ds, "societe");

                MessageBox.Show("" + comboBox1.Text + " : " + textBox1.Text + " Ajoute avec succes");
                f1.refresh_societe_combobox();
            }
            else
            {
                MessageBox.Show("Veuillez choisir un type valide");
            }
        }
    }
}
