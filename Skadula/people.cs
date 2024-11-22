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
    public partial class people : Form
    {
        private DataTable DocsTable()
        {
            DataTable people = new DataTable();
            using (MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306"))
            {
                using (MySqlCommand bdout = new MySqlCommand
                    ("SELECT ЧленЭкипажа as ID,concat(Фамилия,' ',left(Имя,1),'. ',left(Отчество,1),'.') as `Фамилия И.О.`, Тип,Название FROM свидетельства,сотрудники,вс,должность WHERE ЧленЭкипажа=idСотрудники and ТипВС=idВС and Должность=idДолжность order by ЧленЭкипажа", con))
                {
                    con.Open();
                    MySqlDataReader bdreader = bdout.ExecuteReader();
                    people.Load(bdreader);
                }
            }
            return people;
        }

        public people()
        {
            InitializeComponent();
        }

        private void people_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = DocsTable();
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (e.ColumnIndex == 1 || e.ColumnIndex == 0))
            {
               person p = new person(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString()));
               p.ShowDialog();
            }
        }
    }
}
