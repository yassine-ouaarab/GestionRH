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
    public partial class Form2 : Form
    {
        SqlConnection cn = new SqlConnection(@"Data Source=" + Globals.DS + ";Initial Catalog=" + Globals.IC + "; Integrated Security=true;");
        DataSet ds = new DataSet();
        SqlDataAdapter da;
        bool s=false, c=false, g=false;
        public Form2()
        {
            InitializeComponent();
            da = new SqlDataAdapter("select * from utilisateur ", cn);
            da.Fill(ds, "utilisateur");
            da = new SqlDataAdapter("select * from droits ", cn);
            da.Fill(ds, "droits");

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string id_utilisateur = "";
            int cpt = -1;
            for (int i = 0; i < ds.Tables["utilisateur"].Rows.Count; i++)
            {
                // MessageBox.Show(ds.Tables["utilisateur"].Rows[i][1].ToString());
                if (textBox1.Text == ds.Tables["utilisateur"].Rows[i][1].ToString())
                {
                    id_utilisateur = ds.Tables["utilisateur"].Rows[i][0].ToString();
                    cpt = i;
                    break;
                }
            }
            if (cpt == -1)
            {
                MessageBox.Show("utilisateur n'existe pas");
            }
            else
            {
                for (int i = 0; i < ds.Tables["utilisateur"].Rows.Count; i++)
                {
                    if (textBox1.Text == ds.Tables["utilisateur"].Rows[i][1].ToString())
                    {
                        if (textBox2.Text == ds.Tables["utilisateur"].Rows[i][2].ToString())
                        {
                            for (int j = 0; j < ds.Tables["droits"].Rows.Count; j++)
                            {
                                if (ds.Tables["droits"].Rows[j][0].ToString() == id_utilisateur)
                                {

                                    if (ds.Tables["droits"].Rows[j][1].ToString() == "True")
                                    {
                                        s = true;
                                    }
                                    if (ds.Tables["droits"].Rows[j][2].ToString() == "True")
                                    {
                                        c = true;
                                    }
                                    if (ds.Tables["droits"].Rows[j][3].ToString() == "True")
                                    {
                                        g = true;
                                    }
                                }
                            }

                            Form1 f1 = new Form1();
                            f1.s = s;
                            f1.c = c;
                            f1.g = g;

                            this.Hide();
                            f1.Show();

                        }
                        else
                        {
                            MessageBox.Show("pass incorrect");
                        }
                        break;
                    }
                }
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }


    }
}
