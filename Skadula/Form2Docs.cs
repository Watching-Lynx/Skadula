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

namespace Skadula   //ЗАДАЧИ:
{  
    public partial class Form2Docs : Form
    {
        public Form2Docs()
        {
            InitializeComponent();
        }

        private void Form2Docs_Load(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
            con.Open();
            MySqlCommand bdout = new MySqlCommand("select distinct idСотрудники as `ID сотрудника`, concat(Фамилия,' '," +
                    "left(Имя,1),'. ',left(Отчество,1),'.') as Фамилия, date_format(Окончание,'%Y-%m-%d') as Окончание, Название from расписание, документы, " +
                    "сотрудники, типдокумента where Сотрудник = idСотрудники and Тип = idТипДокумента", con);
            int r = 0;
            MySqlDataReader bdreader = bdout.ExecuteReader();
            while (bdreader.Read())  
            {
               docsdataGridView2.Rows.Add();
                docsdataGridView2.Rows[r].Cells["id"].Value = bdreader[0].ToString();
                docsdataGridView2.Rows[r].Cells["surname"].Value = bdreader[1].ToString();
                DateTime ddate2 = DateTime.Parse(bdreader[2].ToString());
                DateTime ddate =DateTime.Now.AddMonths(3);
                docsdataGridView2.Rows[r].Cells["date"].Value = bdreader[2].ToString();
                if(ddate>ddate2) docsdataGridView2.Rows[r].Cells["date"].Style = new DataGridViewCellStyle { ForeColor = Color.Red };
                docsdataGridView2.Rows[r].Cells["type"].Value = bdreader[3].ToString();
                r++;
            }
            bdreader.Close();
            con.Close();
        }

        private void docsdataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (e.ColumnIndex == 1 || e.ColumnIndex == 0))
            {
                person p = new person(Convert.ToInt32(docsdataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString()));
                p.ShowDialog();
            }
        }
    }
}
