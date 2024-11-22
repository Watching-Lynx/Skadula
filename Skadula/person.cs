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
    public partial class person : Form
    {
        int id;
        public person(int p)
        {
            id = p;
            InitializeComponent();
        }

        private void person_Load(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
            con.Open();           //открыть соединение
            MySqlCommand bdout = new MySqlCommand("SELECT concat(Фамилия, ' ', Имя, ' ', Отчество),ОбщийНалет,ЭлПочта,Телефон,ДР,Номер,Уровень FROM сотрудники,свидетельства where idСотрудники="+id+ " and ЧленЭкипажа=idСотрудники", con);  
            MySqlDataReader bdreader = bdout.ExecuteReader();
            while (bdreader.Read())
            {
                label1.Text = bdreader[0].ToString();
                labelnalet.Text = bdreader[1].ToString();
                labelmail.Text = bdreader[2].ToString();
                labelphone.Text = bdreader[3].ToString();
                labeldate.Text = bdreader[4].ToString();
                labelnum.Text = bdreader[5].ToString();
                labellvl.Text = bdreader[6].ToString();
            }
            bdreader.Close();
            bdout = new MySqlCommand("SELECT Название,Тип FROM свидетельства,должность,вс where ЧленЭкипажа =" + id + " and Должность=idДолжность and idВС=ТипВС", con);
            MySqlDataReader bdreader2 = bdout.ExecuteReader();
            while (bdreader2.Read())
            {
                int i = dataGridView1.Rows.Add();
                dataGridView1.Rows[i].Cells["role"].Value=bdreader2[0].ToString();
                dataGridView1.Rows[i].Cells["vs"].Value = bdreader2[1].ToString();
            }
            bdreader2.Close();
            bdout = new MySqlCommand("SELECT Название,Окончание FROM документы,типдокумента where Тип=idТипДокумента and Сотрудник=" + id, con);
            MySqlDataReader bdreader3 = bdout.ExecuteReader();
            while (bdreader3.Read())
            {
                int i = dataGridView2.Rows.Add();
                dataGridView2.Rows[i].Cells["doc"].Value = bdreader3[0].ToString();
                dataGridView2.Rows[i].Cells["date"].Value = bdreader3[1].ToString();
            }
            bdreader3.Close();
            con.Close();
        }
    }
}
