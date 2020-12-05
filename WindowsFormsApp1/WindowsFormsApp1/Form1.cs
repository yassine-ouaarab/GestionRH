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


    public partial class Form1 : Form
    {
        
        SqlConnection cn = new SqlConnection(@"Data Source=" + Globals.DS + ";Initial Catalog=" + Globals.IC + "; Integrated Security=true;");
        DataSet ds;
        SqlDataAdapter da;
        
   

       public  bool s = false, c = false, g = false;


        public Form1()
        {
            InitializeComponent();

            try
            {
                refresh_dataSet();

                refresh_societe_combobox();
                refresh_banque_combobox();
                refresh_first_grid();
                refresh_second_grid();
                refresh_third_grid();
                refresh_fourth_grid();

                dataGridView7.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                refresh_soldes();

                button10.Hide();
                button3.Hide();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

            
            
        }
        public void refresh_soldes()
        {

            try{
            double sd = 0f, sc = 0f;
             
             for (int i = 0; i < ds.Tables["part2"].Rows.Count; i++)
             {
                 for (int j = 0; j < ds.Tables["operation"].Rows.Count; j++)
                 {
                     if(ds.Tables["part2"].Rows[i][0].ToString()==ds.Tables["operation"].Rows[j][0].ToString())
                     {
                         string id_soc = ds.Tables["operation"].Rows[j][10].ToString();

                         for (int z = 0; z < ds.Tables["societe"].Rows.Count; z++)
                         {
                             if (id_soc == ds.Tables["societe"].Rows[z][0].ToString())
                             {
                                 string type_soc = ds.Tables["societe"].Rows[z][2].ToString();
                                 if(type_soc.ToLower()=="client")
                                 {
                                     sd+= Double.Parse( ds.Tables["part2"].Rows[i][9].ToString());
                                 }
                                 else if (type_soc.ToLower() == "fournisseur")
                                 {
                                     sc += Double.Parse(ds.Tables["part2"].Rows[i][9].ToString());
                                 }
                             }
                         }
                     }
                 }


             }
             label16.Text = sd.ToString();
             label17.Text = sc.ToString();
             label18.Text = (sd - sc).ToString();
        }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
       
        public void emptyfirstform()
        {
            try
            {
                textBox7.Text = string.Empty;
                // textBox3.Text = string.Empty;
                comboBox2.Text = string.Empty;
                textBox5.Text = string.Empty;
                textBox8.Text = string.Empty;

                numericUpDown1.Value = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
        }
        public void Migrer()
        {
            try
            {
                da = new SqlDataAdapter("select * from facture ", cn);
                SqlCommandBuilder objCommandBuilder = new SqlCommandBuilder(da);
                da.Update(ds, "facture");

                da = new SqlDataAdapter("select * from operation ", cn);
                objCommandBuilder = new SqlCommandBuilder(da);
                da.Update(ds, "operation");

                da = new SqlDataAdapter("select * from effet ", cn);
                objCommandBuilder = new SqlCommandBuilder(da);
                da.Update(ds, "effet");

                da = new SqlDataAdapter("select * from banque ", cn);
                objCommandBuilder = new SqlCommandBuilder(da);
                da.Update(ds, "banque");

                da = new SqlDataAdapter("select * from societe ", cn);
                objCommandBuilder = new SqlCommandBuilder(da);
                da.Update(ds, "societe");


            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                dataGridView2.Columns.Add("c1", "Date Echeance");
                dataGridView2.Columns.Add("c2", "Mode Regle");
                dataGridView2.Columns.Add("c3", "N° Regle");
                dataGridView2.Columns.Add("c4", "Banque");
                dataGridView2.Columns.Add("c5", "Nature");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        public void refresh_first_grid()
        {
            try
            {

                // dataGridView4.AutoGenerateColumns = false;
                dataGridView4.DataSource = ds.Tables["part1"];

                dataGridView4.Show();

                dataGridView4.Columns[0].Visible = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            

        }

        public void refresh_second_grid()
        {

            try
            {
                // dataGridView4.AutoGenerateColumns = false;
                dataGridView7.DataSource = ds.Tables["part2"];




                dataGridView7.Show();
                // MessageBox.Show(ds.Tables["effet"].Rows[0][2].ToString()); // = "encaissement";

                dataGridView7.Columns[0].Visible = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void refresh_third_grid()
        {
            try
            {

                // dataGridView4.AutoGenerateColumns = false;
                dataGridView2.DataSource = ds.Tables["part3"];




                dataGridView2.Show();
                // MessageBox.Show(ds.Tables["effet"].Rows[0][2].ToString()); // = "encaissement";

                dataGridView2.Columns[0].Visible = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void refresh_fourth_grid()
        {
            try
            {

                // dataGridView4.AutoGenerateColumns = false;
                dataGridView3.DataSource = ds.Tables["part4"];
                // MessageBox.Show(ds.Tables["effet"].Rows[0][2].ToString()); // = "encaissement";

                dataGridView3.Columns[0].Visible = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        public void refresh_part4()
        {
            try
            {
                if (ds.Tables.Contains("part4"))
                {
                    ds.Tables["part4"].Clear();
                }
                da = new SqlDataAdapter("select O.id_operation , O.date_encaissement As Date_Encaissement,O.date_remise As Date_Remise, O.num_remise As N_Remise, O.mode_regl As Mode_Regl, O.num_regl As Num_Regl, S.nom As Nom_Client, O.date_ech As Date_D_echeance ,O.montant As Mt_TCC,F.mt_tva As MT_TVA,F.num_facture As Num_Facture, O.nature As Observ, B.nom_bq As Banque, Null As Cumul From operation O Left join facture F On O.num_facture=F.num_facture, societe S, banque B where O.id_societe=S.id_societe and O.id_banque=B.id_banque Order by O.date_encaissement ", cn);
                da.Fill(ds, "part4");
                float cumul = 0f;
                for (int i = 0; i < ds.Tables["part4"].Rows.Count; i++)
                {
                    cumul += float.Parse(ds.Tables["part4"].Rows[i]["Mt_TCC"].ToString());
                    ds.Tables["part4"].Rows[i]["Cumul"] = cumul;

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public void refresh_societe_combobox()
        {

            try
            {
                comboBox1.Items.Clear();
                refresh_dataSet();
                for (int i = 0; i < ds.Tables["societe"].Rows.Count; i++)
                {
                    comboBox1.Items.Add(ds.Tables["societe"].Rows[i][1].ToString());

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void refresh_banque_combobox()
        {
            try
            {
                comboBox4.Items.Clear();
                comboBox5.Items.Clear();
                refresh_dataSet();
                for (int i = 0; i < ds.Tables["banque"].Rows.Count; i++)
                {
                    comboBox4.Items.Add(ds.Tables["banque"].Rows[i][1].ToString());
                    comboBox5.Items.Add(ds.Tables["banque"].Rows[i][1].ToString());

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void refresh_dataSet()
        {

            try
            {
                ds = new DataSet();
                da = new SqlDataAdapter("select * from operation ", cn);
                da.Fill(ds, "operation");
                da = new SqlDataAdapter("select * from societe ", cn);
                da.Fill(ds, "societe");
                da = new SqlDataAdapter("select O.id_operation,O.date_ech, O.mode_regl , O.banque, O.num_regl, S.nom, O.nature,O.montant from operation O,societe S where O.id_societe=S.id_societe", cn);
                da.Fill(ds, "part1");
                da = new SqlDataAdapter("select O.id_operation, O.date_ech,O.date_remise, O.mode_regl , O.banque, O.num_regl, S.nom  , O.num_remise, O.nature,O.montant , B.nom_bq As BQ_remise from  operation O Left join banque B On O.id_banque=B.id_banque , societe S  where O.id_societe=S.id_societe ", cn);
                da.Fill(ds, "part2");
                da = new SqlDataAdapter("select O.id_operation, O.date_ech,O.date_remise, O.mode_regl , O.banque, O.num_regl, S.nom,O.num_remise, O.nature,O.montant , E.type_eff, B.nom_bq As BQ_Remise, ((O.montant*(DATEDIFF(DAY, O.date_remise, O.date_ech))*E.taux)/360) As Interet, ((O.montant*(DATEDIFF(DAY, O.date_remise, O.date_ech))*E.taux)/360)*1.1 As Interet_TCC from operation O Left Join Banque B On B.id_banque=O.id_banque,societe S , effet E   where O.id_societe=S.id_societe AND O.id_operation=E.id_operation AND mode_regl='EFF' AND E.type_eff='escompte'", cn);
                da.Fill(ds, "part3");
                refresh_part4();
                da = new SqlDataAdapter("select * from effet", cn);
                da.Fill(ds, "effet");
                da = new SqlDataAdapter("select * from banque", cn);
                da.Fill(ds, "banque");
                da = new SqlDataAdapter("select * from facture", cn);
                da.Fill(ds, "facture");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if (!g)
                {
                    button15.Hide();

                }
                if (!s)
                {
                    panel4.Enabled = false;
                    pictureBox11.Enabled = false;
                    panel1.Enabled = false;
                    panel5.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Add_Societe ads = new Add_Societe();
                ads.Show();
                ads.f1 = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        private void dataGridView4_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView7_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dataGridView7.SelectedRows.Count == 1)
                {
                    if (string.IsNullOrWhiteSpace(dataGridView7.SelectedRows[0].Cells[2].Value.ToString()))
                    {
                        textBox1.Text = "";
                    }
                    else
                    {
                        DateTime dt = (DateTime)dataGridView7.SelectedRows[0].Cells[2].Value;
                        textBox1.Text = dt.ToShortDateString();
                    }

                    textBox2.Text = dataGridView7.SelectedRows[0].Cells[7].Value.ToString();
                }

                if (dataGridView7.SelectedRows[0].Cells[3].Value.ToString() == "EFF")
                {

                    string op_id = dataGridView7.SelectedRows[0].Cells[0].Value.ToString();
                    int j = -1;
                    for (int i = 0; i < ds.Tables["effet"].Rows.Count; i++)
                    {

                        if (op_id == ds.Tables["effet"].Rows[i][1].ToString())
                        {
                            j = i;
                            break;
                        }
                    }
                    if (j == -1)
                    {

                    }
                    else
                    {
                        if (ds.Tables["effet"].Rows[j][2].ToString() == "encaissement")
                        {
                            button10.Show();
                            button3.Hide();
                            label20.Text = "";
                        }
                        else
                        {
                            button10.Hide();
                            button3.Show();
                            label20.Text = "Escompte Chez " + dataGridView7.SelectedRows[0].Cells[10].Value.ToString() + " avec un taux de " + ds.Tables["effet"].Rows[j][3].ToString();

                        }
                    }

                }
                else
                {
                    button10.Hide();
                    button3.Hide();
                    label20.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





       

        

        private void dataGridView4_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView7_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                Gest_BQ gb = new Gest_BQ();
                gb.Show();
                gb.f1 = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure!","Message",MessageBoxButtons.YesNo,MessageBoxIcon.Exclamation);
            if (result == DialogResult.Yes)
            { 
            try
            {
                if (dataGridView7.SelectedRows.Count == 1)
                {
                    if (string.IsNullOrWhiteSpace(dataGridView7.SelectedRows[0].Cells[2].Value.ToString())
                    || string.IsNullOrWhiteSpace(dataGridView7.SelectedRows[0].Cells[5].Value.ToString()) || string.IsNullOrWhiteSpace(dataGridView7.SelectedRows[0].Cells[10].Value.ToString()))
                    {
                        MessageBox.Show("Veuillez remplir les donnees de la remise avant d'effectuer cette operation!");
                    }
                    else
                    {
                        if (dataGridView7.SelectedRows[0].Cells[3].Value.ToString() == "EFF")
                        {
                            string op_id = dataGridView7.SelectedRows[0].Cells[0].Value.ToString();
                            int j = -1;
                            for (int i = 0; i < ds.Tables["effet"].Rows.Count; i++)
                            {

                                if (op_id == ds.Tables["effet"].Rows[i][1].ToString())
                                {
                                    j = i;
                                    break;
                                }
                            }
                            if (j == -1)
                            {

                            }
                            else
                            {
                                if (ds.Tables["effet"].Rows[j][2].ToString() == "encaissement")
                                {



                                    int z = -1;
                                    for (int i = 0; i < ds.Tables["banque"].Rows.Count; i++)
                                    {

                                        if (dataGridView7.SelectedRows[0].Cells[10].Value.ToString() == ds.Tables["banque"].Rows[i][1].ToString())
                                        {
                                            z = i;
                                            break;
                                        }
                                    }
                                    if (z == -1)
                                    {
                                        MessageBox.Show("Cette banque n'existe pas");
                                    }
                                    else
                                    {
                                        ds.Tables["effet"].Rows[j][2] = "escompte";
                                        ds.Tables["effet"].Rows[j][3] = ds.Tables["banque"].Rows[z][2];

                                    }

                                    Migrer();
                                    refresh_dataSet();
                                    refresh_second_grid();
                                    refresh_third_grid();
                                    refresh_fourth_grid();
                                }
                            }
                                MessageBox.Show("changer par escompte avec success","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                            }
                        else
                        {
                            MessageBox.Show("L'operation selectionnee n'est pas un effet");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {


                string id_op = "";

                if (dataGridView7.SelectedRows.Count == 1 && !string.IsNullOrWhiteSpace(dataGridView7.SelectedRows[0].Cells[0].Value.ToString()))
                {
                    id_op = dataGridView7.SelectedRows[0].Cells[0].Value.ToString();
                    tabControl1.SelectTab("tabpage1");
                    int z = -1;
                    for (int i = 0; i < dataGridView2.Rows.Count; i++)
                    {

                        if (dataGridView2.Rows[i].Cells[0].Value.ToString() == id_op)
                        {
                            z = i;
                            break;
                        }
                    }
                    if (z == -1)
                    {

                    }
                    else
                    {
                        dataGridView2.ClearSelection();
                        dataGridView2.Rows[z].Selected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
           
        }

        private void dataGridView3_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView3_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            
   
        }

        private void tabPage6_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView3_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

          

            
        }

        private void dataGridView3_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int row_index = dataGridView3.CurrentRow.Index, cell_index = dataGridView3.CurrentCell.ColumnIndex;
                string id_op = dataGridView3.Rows[dataGridView3.CurrentRow.Index].Cells[0].Value.ToString();

                int z = -1;
                for (int i = 0; i < ds.Tables["operation"].Rows.Count; i++)
                {

                    if (ds.Tables["operation"].Rows[i][0].ToString() == id_op)
                    {
                        z = i;
                        break;
                    }
                }
                if (z == -1)
                {
                    MessageBox.Show("operation not found");
                }
                else
                {
                    if (dataGridView3.CurrentCell.ColumnIndex == 1)
                    {
                        ds.Tables["operation"].Rows[z][3] = dataGridView3.CurrentCell.Value;
                        Migrer();
                        refresh_dataSet();
                        refresh_fourth_grid();

                    }
                    else if (dataGridView3.CurrentCell.ColumnIndex == 9)
                    {
                        string f = dataGridView3.CurrentRow.Cells[10].Value.ToString();
                        if (string.IsNullOrWhiteSpace(f))
                        {
                            MessageBox.Show("Veuillez dabord entrez un numero de facture!");
                        }
                        else
                        {
                            int cpt = -1;
                            for (int i = 0; i < ds.Tables["facture"].Rows.Count; i++)
                            {

                                if (ds.Tables["facture"].Rows[i][0].ToString() == f)
                                {
                                    cpt = i;
                                    break;
                                }
                            }
                            if (cpt == -1)
                            {
                                //ce num de facture n existe pas!
                                //  MessageBox.Show("ce num de facture n'existe pas!");

                                //creez la facture
                                DataRow dr;
                                dr = ds.Tables["facture"].NewRow();
                                dr[0] = Int32.Parse(f);
                                dr[1] = dataGridView3.CurrentCell.Value;

                                //ajout de la ligne au DataSet
                                ds.Tables["facture"].Rows.Add(dr);

                                //ajouter la facture a l'operation
                                ds.Tables["operation"].Rows[z][8] = Int32.Parse(f);


                                //Migration vers la base de donne
                                Migrer();

                                refresh_dataSet();

                                refresh_fourth_grid();


                            }
                            else
                            {
                                //ce num de facture existe
                                // MessageBox.Show("ce num de facture existe ");

                                //modifier le mt tva de cette facture
                                ds.Tables["facture"].Rows[cpt][1] = dataGridView3.CurrentCell.Value;

                                //ajouter la facture a l'operation
                                ds.Tables["operation"].Rows[z][8] = Int32.Parse(f);



                                //Migration vers la base de donne
                                Migrer();


                                refresh_fourth_grid();

                                //retour a la meme cellule
                                // dataGridView2.ClearSelection();
                                //dataGridView2.Rows[row_index].Cells[cell_index].Selected = true;

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    



        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }
        

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                int cpt = -1;
                for (int i = 0; i < ds.Tables["societe"].Rows.Count; i++)
                {

                    if (comboBox1.Text == ds.Tables["societe"].Rows[i][1].ToString())
                    {
                        cpt = i;
                        break;
                    }
                }

                if (cpt == -1)
                {
                    MessageBox.Show("cette societe n existe pas");

                }
                else
                {// remplir DataRow

                    if (string.IsNullOrWhiteSpace(textBox7.Text) || string.IsNullOrWhiteSpace(textBox8.Text) || string.IsNullOrWhiteSpace(comboBox2.Text))
                    {
                        MessageBox.Show("Veuillez remplir tout les champs obligatoires!");
                    }
                    else
                    {
                        int id_societe = Int32.Parse(ds.Tables["societe"].Rows[cpt][0].ToString());

                        DataRow dr;
                        dr = ds.Tables["operation"].NewRow();
                        dr[1] = dateTimePicker2.Text;
                        dr[2] = DBNull.Value;
                        dr[3] = DBNull.Value;

                        if (string.IsNullOrWhiteSpace(textBox5.Text))
                        {
                            //nature empty
                            // dr[5] = "";
                        }
                        else
                        {
                            dr[5] = textBox5.Text;
                        }
                        dr[6] = textBox8.Text;
                        dr[7] = comboBox2.Text;//textBox3.Text;
                        dr[8] = DBNull.Value;
                        dr[9] = DBNull.Value;
                        dr[10] = id_societe;
                        // utilisateur dr[11]

                        dr[12] = textBox7.Text;

                        dr[13] = DBNull.Value;

                        dr[14] = numericUpDown1.Value;

                        //ajout de la ligne au DataSet
                        ds.Tables["operation"].Rows.Add(dr);



                        //Migration vers la base de donne
                        Migrer();


                        emptyfirstform();
                        refresh_dataSet();
                        refresh_second_grid();
                        refresh_soldes();
                    }

                }


                //  SqlDataAdapter da = new SqlDataAdapter("select * from operation O,societe S where O.id_societe=S.id_societe", cn);
                //  DataTable dt = new DataTable();
                //da.Fill(dt);
                // dataGridView1.AutoGenerateColumns = false;
                //dataGridView1.DataSource = ds.Tables["utilisateur"];
                //dataGridView1.Show();

                refresh_dataSet();
                refresh_first_grid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            try
            {
                emptyfirstform();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {

            try
            {
                string id_op = "";
                if (dataGridView4.SelectedRows.Count == 1)
                {
                    id_op = dataGridView4.SelectedRows[0].Cells[0].Value.ToString();




                    for (int i = 0; i < ds.Tables["operation"].Rows.Count; i++)
                    {
                        if (id_op == ds.Tables["operation"].Rows[i][0].ToString())
                        {
                            ds.Tables["operation"].Rows[i].Delete();
                        }
                    }

                    Migrer();
                    refresh_dataSet();
                    refresh_first_grid();
                    refresh_second_grid();
                    refresh_third_grid();
                    refresh_fourth_grid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            try
            {
                string cmd = "select O.id_operation, O.date_ech,O.date_remise, O.mode_regl , O.banque, O.num_regl, S.nom  , O.num_remise, O.nature,O.montant , B.nom_bq As BQ_remise ";
                if (!string.IsNullOrWhiteSpace(comboBox3.Text))
                {

                    if (comboBox3.Text == "EFF")
                    {

                        cmd = cmd + ", E.type_eff  from  operation O Left join banque B On O.id_banque=B.id_banque , societe S , effet E  where E.id_operation=O.id_operation And O.id_societe=S.id_societe AND mode_regl='" + comboBox3.Text + "'";

                    }
                    else
                    {
                        cmd = cmd + "from  operation O Left join banque B On O.id_banque=B.id_banque , societe S  where O.id_societe=S.id_societe AND mode_regl='" + comboBox3.Text + "'";

                    }


                }
                if (string.IsNullOrWhiteSpace(comboBox3.Text))
                {
                    cmd = "select O.id_operation, O.date_ech,O.date_remise, O.mode_regl , O.banque, O.num_regl, S.nom  , O.num_remise, O.nature,O.montant , B.nom_bq As BQ_remise from  operation O Left join banque B On O.id_banque=B.id_banque , societe S  where O.id_societe=S.id_societe ";
                }
                if (!string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    cmd = cmd + " AND B.nom_bq = '" + textBox3.Text + "'";
                }
                if (radioButton3.Checked == true)
                {
                    cmd = cmd + " AND O.date_ech >= '" + dateTimePicker7.Text + "' AND O.date_ech <= '" + dateTimePicker1.Text + "'";
                }
                if (radioButton4.Checked == true)
                {
                    cmd = cmd + " AND O.date_remise >= '" + dateTimePicker7.Text + "' AND O.date_remise <= '" + dateTimePicker1.Text + "'";
                }
                if (checkBox1.Checked == true)
                {
                    cmd = cmd + " AND O.num_remise is NULL";
                }
                if (!string.IsNullOrWhiteSpace(comboBox6.Text))
                {
                    if (comboBox6.Text == "Client")
                    {
                        cmd = cmd + " AND S.genre='Client'";
                    }
                    else if (comboBox6.Text == "Fournisseur")
                    {
                        cmd = cmd + " AND S.genre='Fournisseur'";
                    }
                    else
                    {
                        comboBox6.Text = "";
                    }
                }
                if (!string.IsNullOrWhiteSpace(textBox4.Text))
                {

                    cmd = cmd + " AND O.num_regl='" + textBox4.Text + "'";

                }
                if (numericUpDown2.Value > 0)
                {
                    int montant_int = (int)numericUpDown2.Value;
                    string decimal_value = (numericUpDown2.Value - montant_int).ToString();
                    string decimal_string = "" + montant_int + ".";
                    for (int i = 2; i < decimal_value.Length; i++)
                    {
                        decimal_string += decimal_value[i];
                    }

                    cmd = cmd + " AND O.montant=" + decimal_string;

                }


                da = new SqlDataAdapter(cmd, cn);
                ds.Tables.Remove(ds.Tables["part2"]);
                da.Fill(ds, "part2");
                refresh_second_grid();

                refresh_soldes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            try
            {
                refresh_dataSet();
                refresh_second_grid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            /*try
            {
                Form3 f3 = new Form3();
                f3.da = da;
                f3.dt = ds.Tables["part2"];
                f3.fill_report_table();
                f3.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }*/
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(comboBox5.Text))
                {
                    ds.Tables.Remove(ds.Tables["part4"]);

                    da = new SqlDataAdapter("select O.id_operation , O.date_encaissement As Date_Encaissement,O.date_remise As Date_Remise, O.num_remise As N_Remise, O.mode_regl As Mode_Regl, O.num_regl As Num_Regl, S.nom As Nom_Client, O.date_ech As Date_D_echeance ,O.montant As Mt_TCC,F.mt_tva As MT_TVA,F.num_facture As Num_Facture, O.nature As Observ, B.nom_bq As Banque, Null As Cumul From operation O Left join facture F On O.num_facture=F.num_facture, societe S, banque B where O.id_societe=S.id_societe and O.id_banque=B.id_banque  and B.nom_bq='" + comboBox5.Text + "' Order by date_encaissement", cn);

                    da.Fill(ds, "part4");

                    float cumul = 0f;
                    for (int i = 0; i < ds.Tables["part4"].Rows.Count; i++)
                    {
                        cumul += float.Parse(ds.Tables["part4"].Rows[i]["Mt_TCC"].ToString());
                        ds.Tables["part4"].Rows[i]["Cumul"] = cumul;

                    }

                    refresh_fourth_grid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            try
            {
                refresh_part4();
                refresh_fourth_grid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text)
                    || string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(comboBox4.Text))
                {
                    MessageBox.Show("Veuillez remplir les champs!");
                }
                else
                {
                    int cpt = -1;
                    for (int i = 0; i < ds.Tables["operation"].Rows.Count; i++)
                    {

                        if (dataGridView7.SelectedRows[0].Cells[0].Value.ToString() == ds.Tables["operation"].Rows[i][0].ToString())
                        {
                            cpt = i;
                            break;
                        }
                    }

                    if (cpt == -1)
                    {
                        MessageBox.Show("Operation ne peut pas etre realisee");

                    }
                    else
                    {
                        int j = -1;
                        for (int i = 0; i < ds.Tables["banque"].Rows.Count; i++)
                        {

                            if (comboBox4.Text.ToString() == ds.Tables["banque"].Rows[i][1].ToString())
                            {
                                j = i;
                                break;
                            }
                        }
                        if (j == -1)
                        {
                            MessageBox.Show("Cette banque n'existe pas");
                        }
                        else
                        {
                            ds.Tables["operation"].Rows[cpt][2] = textBox1.Text;
                            ds.Tables["operation"].Rows[cpt][9] = textBox2.Text;
                            string id_bq = ds.Tables["banque"].Rows[j][0].ToString();
                            ds.Tables["operation"].Rows[cpt][13] = id_bq;
                        }


                        Migrer();
                        refresh_dataSet();
                        refresh_first_grid();
                        refresh_second_grid();
                        refresh_fourth_grid();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Hide();
            f2.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            this.Hide();
            f2.Show();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView2.SelectedRows.Count == 1)
                {
                    string row_id = dataGridView2.SelectedRows[0].Index.ToString();
                    string op_id = dataGridView2.Rows[Int32.Parse(row_id)].Cells[0].Value.ToString();

                    int cpt = -1;
                    for (int i = 0; i < ds.Tables["effet"].Rows.Count; i++)
                    {
                        if (ds.Tables["effet"].Rows[i][1].ToString() == op_id)
                        {
                            cpt = i;
                            break;
                        }
                    }
                    if (cpt == -1)
                    {
                    }
                    else
                    {
                        ds.Tables["effet"].Rows[cpt][2] = "encaissement";
                        ds.Tables["effet"].Rows[cpt][3] = DBNull.Value;
                        Migrer();
                        refresh_dataSet();
                        refresh_first_grid();
                        refresh_second_grid();
                        refresh_third_grid();
                        refresh_fourth_grid();
                        MessageBox.Show("Escompte Annule","Message",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

       
        private void button15_Click(object sender, EventArgs e)
        {
            Form4 f4 = new Form4();
            f4.Show();
        }
    }

    public class Globals
        {
            public static string DS = "ASUS-DHC34T\\SQLEXPRESS";
            public static string IC = "GDIRAGRI";
        }
    }
