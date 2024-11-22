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
    public partial class flights : Form
    {
        void do_datagrid(List<int[]> pairs)
        {
            int r = 0;
            MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
            con.Open();
            MySqlCommand bdout = new MySqlCommand("select Номер,Дата,concat(A1.Название,'(',A1.КодIATA,')-',A2.Название,'(',A2.КодIATA,')') as М,concat(left(СменаНачало,5), '-', left(СменаКонец,5)) as П,КолвоКЭ,idРейса,Тип " +
                "from рейсы,расписание,аэропорты A1,аэропорты A2,вс where idВС=ВС and ДатаВылета = idРасписание and АэропортВылета = A1.idАэропорты AND АэропортПрилета = A2.idАэропорты order by Дата", con);
            MySqlDataReader bdreader = bdout.ExecuteReader();
            while (bdreader.Read())  //массив из ид рейсов
            {
                int kvas = 0, two = 0, st = 0;
                dataGridView1.Rows.Add();
                dataGridView1.Rows[r].Cells["number"].Value = bdreader[0].ToString();
                dataGridView1.Rows[r].Cells["date"].Value = bdreader[1].ToString();
                dataGridView1.Rows[r].Cells["where"].Value = bdreader[2].ToString();
                dataGridView1.Rows[r].Cells["time"].Value = bdreader[3].ToString();
                dataGridView1.Rows[r].Cells["type"].Value = bdreader[6].ToString();
                for (int i = 0; i < pairs.Count; i++)
                {
                    if(pairs.ElementAt(i)[0] ==Convert.ToInt32(bdreader[5]))
                    {
                        MySqlConnection con2 = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
                        con2.Open();
                        MySqlCommand bdout2 = new MySqlCommand("SELECT должность.Название,concat(Фамилия,' ',left(Имя,1),'. ',left(Отчество,1),'.') as `Фамилия И. О.` " +
                            "FROM рейсы, свидетельства, должность,сотрудники,вс WHERE idРейса = "+ r + " and ТипВС = ВС and ЧленЭкипажа = "
                            + pairs.ElementAt(i)[1] + " and idВС=ВС and idДолжность = Должность and ЧленЭкипажа = idСотрудники", con2);
                        MySqlDataReader bdreader2 = bdout2.ExecuteReader();
                            while (bdreader2.Read())  //строка 
                            {
                            dataGridView1.Rows[r].Cells["guys"].Value += bdreader2[1].ToString()+"\n";
                            dataGridView1.Rows[r].Cells["role"].Value += bdreader2[0].ToString() + "\n";
                            switch (bdreader2[0].ToString())
                            {
                                case "КВС":
                                    kvas++;
                                    break;
                                case "Второй пилот":
                                    two++;
                                    break;
                                case "Бортпроводник":
                                    st++;
                                    break;
                            }
                        }
                        bdreader2.Close();
                        con2.Close();
                    }
                }
                //тут нужна проверка что заполнен рейс
                if (kvas == 1 && two == 1 && st >= Convert.ToInt32(bdreader[4])) {
                    dataGridView1.Rows[r].DefaultCellStyle.BackColor = Color.LightGreen;
                }else
                dataGridView1.Rows[r].DefaultCellStyle.BackColor = DefaultBackColor;
                r++;
            }
            bdreader.Close();
            con.Close();
        }
        List<int[]> pairs = new List<int[]>();
       
        public flights(List<int[]> l)
        {
            pairs = l;
            InitializeComponent();
        }

        private void flights_Load(object sender, EventArgs e)
        {
            do_datagrid(pairs);
        }
    }
}
