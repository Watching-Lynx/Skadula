using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Skadula
{
    public partial class addFlight : Form
    {
        static string date1;
        static int person_id1;
        static string busy_type1;
        selectedCell cell = new selectedCell(busy_type1, date1, person_id1);
        class selectedCell
        {
            public string date;
            public int person_id;
            public string busy_type;

            public selectedCell(string busy_, string date_, int id_)
            {
                    date = date_;
                person_id = id_;
                busy_type = busy_;
        }
            public void youllwork(Label label, DataGridView dataGridView)
            {
                string ord1 = "SELECT Фамилия, Имя, Отчество FROM сотрудники where idСотрудники ="+person_id;
                MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
                con.Open();          
                MySqlCommand bdout = new MySqlCommand(ord1, con);
                MySqlDataReader bdreader = bdout.ExecuteReader();
                string fio = "";
                while (bdreader.Read())
                {
                    fio =bdreader[0].ToString() + " " + bdreader[1].ToString() + " " + bdreader[2].ToString();
                }
                bdreader.Close();
                label.Text = fio;
                string ord2 = "SELECT рейсы.Номер, Тип, должность.Название,Международный,Уровень FROM рейсы, расписание, свидетельства, вс, должность " +
                    "where Дата = '"+date+"' and ЧленЭкипажа = "+person_id+" and ДатаВылета = idРасписание " +
                    "and ВС = idВС and ТипВС = idВС and idДолжность = Должность";
               bdout = new MySqlCommand(ord2, con);
                MySqlDataReader bdreader2 = bdout.ExecuteReader();
                while (bdreader2.Read())
                {
                    if (bdreader2[4].ToString() != "3" && Convert.ToInt32(bdreader2[3]) != 1)
                    {
                        int y = dataGridView.Rows.Add();
                        dataGridView.Rows[y].Cells["flightNum"].Value = bdreader2[0].ToString();
                        dataGridView.Rows[y].Cells["type"].Value = bdreader2[1].ToString();
                        dataGridView.Rows[y].Cells["flightDolj"].Value = bdreader2[2].ToString();
                    }
                }
                bdreader2.Close();
                con.Close();
            }
            public void sendWorker(string b)
            {
                char s = '"';
                int c = 0;
                string ord1 = "update расписание set ЭкипажРейса = JSON_ARRAY_APPEND(ЭкипажРейса, '$." +
                    s + "Члены экипажа" + s + "', " + person_id + ") where Дата = '" + date + "'";
                MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306"); //добавление ид человека в ЭкипажРейса
                con.Open();
                MySqlCommand bdout = new MySqlCommand(ord1, con);
                bdout.ExecuteNonQuery();
                string ord2 = "select idРейса from рейсы,расписание where Дата = '" + date + "' and idРасписание = ДатаВылета and Номер = " + s + b + s;
                bdout = new MySqlCommand(ord2, con);
                MySqlDataReader bdreader = bdout.ExecuteReader();
                while (bdreader.Read())
                {
                    c = Convert.ToInt32(bdreader[0]);
                }
                bdreader.Close();
                string ord3 = "update расписание set ЭкипажРейса = JSON_ARRAY_APPEND(ЭкипажРейса, '$." + s + "Id рейса" + s + "'," + c + ") where Дата ='" + date+"'";
               bdout = new MySqlCommand(ord3, con);
                bdout.ExecuteNonQuery();
                con.Close();
            }
        }

        public addFlight(string busy_type, string date, int person_id)
        {
            busy_type1 = busy_type;
            date1 = date;
            person_id1 = person_id;
            InitializeComponent();
        }

        private void addFlight_Load(object sender, EventArgs e)
        {
            selectedCell cell = new selectedCell(busy_type1, date1, person_id1);
            this.Text = cell.date;
            cell.youllwork(label2, dataGridView1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectedCell cell = new selectedCell(busy_type1, date1, person_id1);
            for (int r = 0; r < dataGridView1.Rows.Count; r++)
            {
                if (Convert.ToBoolean(dataGridView1[0, r].Value) == true)
                {
                    cell.sendWorker(Convert.ToString(dataGridView1[1,r].Value));
                    break;
                }
            }
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            for (int r = 0; r < dataGridView1.Rows.Count; r++)
            {
                dataGridView1[0, r].Value = false;
            }
        }
    }
}
