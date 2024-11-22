using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Skadula
{
    //--------------------------------ЗАДАЧИ: 
    //больше условий для соблюдения нормативов(не менее 42ч еженедельного отдыха)
//-формирование файла для другого типа расписания


    public partial class Form1 : Form
    {
        selected_cell thisCell = new selected_cell();
        List<int[]> pairs = new List<int[]>();
        void study(DataGridView dataGridView1,string cell, int i, int num)
        {
            char symbol = '"';
            MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
            con.Open();
            int id = 0;
            MySqlCommand bdout = new MySqlCommand("SELECT Обучение-> '$." +
                symbol + "Id документа" + symbol + "[" + num + "]' FROM расписание where Дата = '" + cell + "'", con);
            MySqlDataReader bdreader = bdout.ExecuteReader();
            while (bdreader.Read())
            { id = Convert.ToInt32(bdreader[0]); }
            bdreader.Close();
            bdout = new MySqlCommand("SELECT Название FROM типдокумента where idТипДокумента="+id, con);
            MySqlDataReader bdreader2 = bdout.ExecuteReader();
            while (bdreader2.Read())
            {
                dataGridView1.Rows[i].Cells[cell].Value = bdreader2[0].ToString();
            }
            bdreader2.Close();
            con.Close();
        }

        void flightpeople(int e, int t)
        {
                dataGridView3.Rows.Clear();
                int r = 0;
                int kvas = 0, two = 0, st = 0;
                MySqlConnection con3 = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
                con3.Open();
                for (int i = 0; i < pairs.Count; i++)
                {
                    if (pairs.ElementAt(i)[0] == t)
                    {
                        MySqlCommand bdout2 = new MySqlCommand("SELECT concat(Фамилия,' ',left(Имя,1),'. ',left(Отчество,1),'.') as ф, Название, " +
        " КолвоКЭ FROM свидетельства, сотрудники, должность, рейсы,вс where ЧленЭкипажа = idСотрудники and Должность = idДолжность and idСотрудники =" + pairs.ElementAt(i)[1] +
        " and idРейса =" + t + " and ВС = ТипВС and idВС=ВС", con3);
                        MySqlDataReader bdreader3 = bdout2.ExecuteReader();
                        while (bdreader3.Read())  //строка 
                        {
                            dataGridView3.Rows.Add();
                            dataGridView3.Rows[r].Cells["sur"].Value = bdreader3[0].ToString();
                            dataGridView3.Rows[r].Cells["role"].Value = bdreader3[1].ToString();
                            switch (bdreader3[1].ToString())
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
                            r++;
                        }
                        if (kvas == 1 && two == 1 && st >= Convert.ToInt32(bdreader3[2]))
                        {
                            dataGridView3.RowsDefaultCellStyle.BackColor = Color.LightGreen;
                            dataGridView2.Rows[e].DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            dataGridView3.RowsDefaultCellStyle.BackColor = DefaultBackColor;
                            dataGridView2.Rows[e].DefaultCellStyle.BackColor = DefaultBackColor;
                        }
                        bdreader3.Close();
                    }
                }
                con3.Close();
        }

        int[] flight(DataGridView dataGridView1, string cell, int i, int num)
        {
            char symbol = '"';
            MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
            con.Open();
            int id = 0;
            MySqlCommand bdout = new MySqlCommand("SELECT ЭкипажРейса-> '$." +
                symbol + "Id рейса" + symbol + "[" + num + "]' FROM расписание where Дата = '" + cell + "'", con);
            MySqlDataReader bdreader = bdout.ExecuteReader();
            while (bdreader.Read())
            { id = Convert.ToInt32(bdreader[0]); }
            bdreader.Close();
            bdout = new MySqlCommand("select Номер, ABS(TIMEDIFF(СменаНачало, СменаКонец)) as Смена, ABS(TIMEDIFF(ВремяВылета, ВремяПрилета)) as Полетное, " +
                " concat(left(ВремяВылета,5),'-',left(ВремяПрилета,5)) as Время, concat(A1.КодIATA, '-',A2.КодIATA) as Маршрут from рейсы, аэропорты A1, аэропорты A2 " +
                " where АэропортВылета=A1.idАэропорты and АэропортПрилета= A2.idАэропорты and  idРейса =" + id, con);
            MySqlDataReader bdreader2 = bdout.ExecuteReader();
            int[] norma = { 0, 0 };
            while (bdreader2.Read())
            {
                dataGridView1.Rows[i].Cells[cell].Value = bdreader2[0].ToString() + " \n" + bdreader2[3].ToString() + " \n" + bdreader2[4].ToString();
                norma[0] += Convert.ToInt32(bdreader2[1]);
                norma[1] += Convert.ToInt32(bdreader2[2]);
            }
            bdreader2.Close();
            int[] mass = { id, i+1 };
            pairs.Add(mass);
            con.Close();
            return norma;
        }

        void do_file(DataGridView dataBase)
        {
            string line = "------------------------+";
            FileStream fs = new FileStream("Расписание.txt", FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fs);
            int colline = 0;
            for (int c = 1; c < dataBase.Columns.Count; c++)
            {
                string r = dataBase.Columns[c].HeaderText.ToString();
              r = r.PadRight(24);
                streamWriter.Write(r + "|");
                colline++;
            }
            for (int j = 0; j < dataBase.Rows.Count; j++)
                {
                 streamWriter.WriteLine();
                for (int l = 0; l < colline; l++) { streamWriter.Write(line); }
                streamWriter.WriteLine();
                for (int i = 1; i < dataBase.Rows[j].Cells.Count; i++)
                    {
                    if(dataBase.Rows[j].Cells[i].Value != null)
                    {
                        string w = dataBase.Rows[j].Cells[i].Value.ToString();
                        w = w.PadRight(24);
                        streamWriter.Write(w + "|");
                    }
                    else
                    {
                        string w = "";
                        w = w.PadRight(24);
                        streamWriter.Write(w +"|");
                    }
                }  
            }
            streamWriter.Close();
                fs.Close();
            MessageBox.Show("Файл создан");
        }

        void do_datagrid(string where = "",string where2="",bool eng=false)
        {
            ToolTip warningTip = new ToolTip();
            char s = '"';
            bd me = new bd();
            int[] norma = new int[2];
            pairs.Clear();
            string warning = "";
            List<string> date_order = me.listOrder("SELECT date_format(`Дата`,'%Y-%m-%d') FROM `расписание`");
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();
            if (where != "") where = " and Название="+s+ where + s;
            if (where2 != "") where2 = " and Тип=" + s + where2 + s;
            string english = "";
            if(eng == true)english= " and (Уровень >=4 or Уровень is null)";
            DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn();
            id.HeaderText = "ID";
            id.Name = "id";
            dataGridView1.Columns.Add(id);
            DataGridViewTextBoxColumn surname = new DataGridViewTextBoxColumn();
            surname.HeaderText = "Фамилия И. О.";
            surname.Name = "surname";
            dataGridView1.Columns.Add(surname);
            MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
            con.Open();           //открыть соединение
            MySqlCommand bdout = new MySqlCommand("SELECT distinct idСотрудники,concat(Фамилия,' ',left(Имя,1),'. ',left(Отчество,1),'.') as фио,НалетМесяц, НалетНеделя FROM сотрудники,свидетельства,должность,вс " +
                "where ЧленЭкипажа = idСотрудники and idДолжность = Должность and idВС=ТипВС " + where+ where2+ english+" order by idСотрудники", con);
            MySqlDataReader bdreader = bdout.ExecuteReader();
            int r = 0;
            while (bdreader.Read())
            {
                dataGridView1.Rows.Add();
                norma[0] = Convert.ToInt32(bdreader[2].ToString());
                norma[1] = Convert.ToInt32(bdreader[3].ToString());
                dataGridView1.Rows[r].Cells["id"].Value = Convert.ToInt32(bdreader[0]);
                dataGridView1.Rows[r].Cells["surname"].Value = bdreader[1].ToString();
                for (int w = 0; w < date_order.Count; w++)
                {
                    if (r == 0)
                    {
                        DataGridViewTextBoxColumn dati = new DataGridViewTextBoxColumn();
                        dati.HeaderText = date_order[w];
                        dati.Name = date_order[w];
                        dataGridView1.Columns.Add(dati);
                    }
                    DateTime week = DateTime.Parse(date_order[w]);
                    DateTime m = DateTime.Parse(date_order[w]);
                    if (Convert.ToInt32(week.DayOfWeek) == 0) norma[1] = 0;
                    if (m.Month == DateTime.DaysInMonth(m.Year, m.Month)) norma[0] = 0;
                    List<string> busy_order = me.today_busy(date_order[w]);  //лист из 4х элементов на каждый вид занятости, 
                                                                             //каждый элемент это строка с номерами ид людей
                    for (int x = 0; x < busy_order.Count; x++)  //просмотр списка людей конкретной занятости
                    {
                        if (busy_order[x] != "")
                        {
                            //string[] nums = busy_order[x].Split(new char[] { ',' }); //разделить цифры ид членов экипажа
                            int[] nums = busy_order[x].Split(',').Select(n => Int32.Parse(n)).ToArray();
                            if (nums.Contains(Convert.ToInt32(bdreader[0])))
                            {
                                for (int v = 0; v < nums.Length; v++)
                                {
                                    if (nums[v] == Convert.ToInt32(bdreader[0]))  //сравнение номера человека с номером в массиве занятости
                                    {
                                        switch (x)
                                        {
                                            case 0:
                                                dataGridView1.Rows[r].Cells[date_order[w]].Style.BackColor = Color.Yellow; //на рейс
                                                int[] dlit = flight(dataGridView1, date_order[w], r, v);
                                                norma[0] += dlit[0]; norma[1] += dlit[1];
                                                if (norma[1] > 360000)
                                                {
                                                    dataGridView1.Rows[r].Cells["surname"].Style.BackColor = Color.Red;
                                                    dataGridView1.Rows[r].Cells["id"].Style.BackColor = Color.Red;
                                                    label1.Visible = true;
                                                    warning += bdreader[1] + ": Превышение недельной нагрузки (" + date_order[w] + ")\n";
                                                }
                                                if (norma[0] > 800000)   //проверка норм
                                                {
                                                    dataGridView1.Rows[r].Cells["surname"].Style.BackColor = Color.Red;
                                                    dataGridView1.Rows[r].Cells["id"].Style.BackColor = Color.Red;
                                                    warning += bdreader[1] + ": Превышение месячной нагрузки (" + date_order[w] + ")\n";
                                                    label1.Visible = true;
                                                }
                                                goto next_day;
                                            case 1:
                                                study(dataGridView1, date_order[w], r, v);
                                                dataGridView1.Rows[r].Cells[date_order[w]].Style.BackColor = Color.LightCyan; //на учебу
                                                goto next_day;
                                            case 2:
                                                dataGridView1.Rows[r].Cells[date_order[w]].Style.BackColor = Color.IndianRed;  //влэк
                                                dataGridView1.Rows[r].Cells[date_order[w]].Value = "ВЛЭК";
                                                goto next_day;
                                            case 3:
                                                dataGridView1.Rows[r].Cells[date_order[w]].Style.BackColor = Color.YellowGreen; //выходной
                                                goto next_day;
                                        }
                                    }
                                }
                            }
                    }
                }
                next_day:
                    if (warning != "")
                    {
                        warningTip.BackColor = Color.MistyRose;
                        warningTip.SetToolTip(this.label1, warning);
                    }
                    else label1.Visible = false;
                }
                r++;
            }
            bdreader.Close();
            con.Close();
            dataGridView2.Rows.Clear();
            int r2 = 0;
            MySqlConnection con2 = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
            con2.Open();
            bdout = new MySqlCommand("select Номер,Дата,concat(A1.КодIATA,'-',A2.КодIATA) as М,concat(left(ВремяВылета,5), '-', left(ВремяПрилета,5)) as П,КолвоКЭ,idРейса,Тип " +
                "from рейсы,расписание,аэропорты A1,аэропорты A2,вс where idВС=ВС and ДатаВылета = idРасписание and АэропортВылета = A1.idАэропорты AND АэропортПрилета = A2.idАэропорты order by Дата", con2);
            MySqlDataReader bdreader2 = bdout.ExecuteReader();
            while (bdreader2.Read())  //массив из ид рейсов
            {
                dataGridView2.Rows.Add();
                dataGridView2.Rows[r2].Cells["id"].Value = Convert.ToInt32(bdreader2[5]);
                dataGridView2.Rows[r2].Cells["info"].Value = bdreader2[1].ToString() + " \n" + bdreader2[0].ToString() + " \n" + bdreader2[2].ToString() + " \n" + bdreader2[3].ToString() + " \n" + bdreader2[6].ToString();
                flightpeople(r2, Convert.ToInt32(bdreader2[5]));
                r2++;
            }
            bdreader2.Close();
            con2.Close();
        }

        class selected_cell    //собирает инфу с той ячейки из которой было вызвано контекстное меню
        {
           public string date;
            public int person_id;
            public string busy_type;
            public void delete()  //будет делть запрос удаления
            {
                char s = '"';
                string order = "select " + busy_type + " ->> '$." + s + "Члены экипажа" + s + "' from расписание where Дата = '" + date + "'";
                MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
                con.Open();           //открыть соединение
                MySqlCommand bdout = new MySqlCommand(order, con);  
                MySqlDataReader bdreader = bdout.ExecuteReader();
                string str = "";
                while (bdreader.Read())
                {
                    string[] split = bdreader[0].ToString().Split(new char[] { ']', '[' });
                    str = split[1];
                }
                bdreader.Close();
                string[] nums = str.Split(new char[] { ',' });
                int pos=0;
                for (int t = 0; t < nums.Length; t++)
                {
                    if (Convert.ToInt32(nums[t]) == person_id) pos = t;
                }
                order = "select json_keys(" + busy_type + ") from расписание where Дата = '" + date + "'";
                bdout = new MySqlCommand(order, con);
                MySqlDataReader bdreader2 = bdout.ExecuteReader();
                while (bdreader2.Read())
                {
                    string[] split = bdreader2[0].ToString().Split(new char[] { ']', '[' });
                    str = split[1];
                }
                bdreader2.Close();
                string[] nums2 = str.Split(new char[] { ',' });
                for(int t=0;t< nums2.Length; t++)
                {
                    order = "UPDATE расписание SET "+ busy_type + "= JSON_REMOVE(" + busy_type + ", '$."+
                        nums2[t]+"["+pos+"]') where Дата = '" + date + "'";
                    bdout = new MySqlCommand(order, con);
                    bdout.ExecuteNonQuery();
                }
                con.Close();        //закрыть соединение
            }
            public void addWithoutF(string what="")
            {
                char s = '"';
                string ord1 = "update расписание set "+ busy_type+" = JSON_ARRAY_APPEND(" + busy_type +", '$." +
                    s + "Члены экипажа" + s + "', " + person_id + ") where Дата = '" + date + "'";
                MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306"); //добавление ид человека в ЭкипажРейса
                con.Open();
                MySqlCommand bdout = new MySqlCommand(ord1, con);
                bdout.ExecuteNonQuery();
                if (what!= "")
                {
                    bdout = new MySqlCommand("update расписание set " + busy_type + " = JSON_ARRAY_APPEND(" + busy_type + ", '$." +
                    s +"Id документа"+s+ "',(select idТипДокумента from типдокумента where Название="+s+what + s + ")) where Дата = '" + date + "'", con);
                    bdout.ExecuteNonQuery();
                }


                con.Close();
            }
        }

        class bd
        {                  
            public bd(){ }
            public string[] smallOrder(string mysql)   //пока не используется, нужен будет возможно для однострочных запросов
            {
                MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");  //создание объекта соединения
                con.Open();           //открыть соединение
                MySqlCommand bdout = new MySqlCommand(mysql, con);  //создать объект для отправки и приема запроса
                MySqlDataReader bdreader = bdout.ExecuteReader();
                string[] ord = new string[3];
                while (bdreader.Read())
                {
                    ord[0] = bdreader[0].ToString(); ord[1] = bdreader[1].ToString(); ord[2] = bdreader[2].ToString();
                }
                bdreader.Close();
                con.Close();        //закрыть соединение
                return ord;
            }
            public List<string> listOrder(string mysql)
            {
                MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
                con.Open();           //открыть соединение
                MySqlCommand bdout = new MySqlCommand(mysql, con);  //создать объект для отправки и приема запроса
                MySqlDataReader bdreader = bdout.ExecuteReader();
                List<string> ord = new List<string>();
                while (bdreader.Read())
                {
                    ord.Add(bdreader[0].ToString());
                }  
                bdreader.Close();
                con.Close();        //закрыть соединение
                return ord;
            } 
            public List<string> today_busy(string date)
            {
                char symbol = '"';
                MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");  //создание объекта соединения
                con.Open();           //открыть соединение
                string[] todo = { "ЭкипажРейса", "Обучение","ВЛЭК","Выходной" };
                List<string> ord = new List<string>();
                for (int y = 0; y< 4; y++)
                {
                    MySqlCommand bdout = new MySqlCommand("SELECT " + todo[y] + " -> '$."+symbol+ "Члены экипажа" + symbol+" 'FROM `расписание` where Дата = '" + date + "'", con);  //создать объект для отправки и приема запроса
                    MySqlDataReader bdreader = bdout.ExecuteReader();
                    while (bdreader.Read())
                    {
                        string[] split = bdreader[0].ToString().Split(new char[] { ']','[' });
                        if(split.Length > 1)ord.Add(split[1]);
                    }
                    bdreader.Close();
                }
                con.Close();        //закрыть соединение
                return ord;
            }
        }

        public Form1()
        {
            InitializeComponent();  
        }

        void Form1_Load(object sender, EventArgs e)
        {
            do_datagrid();
            MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=mydb;port=3306");
            con.Open();
            MySqlCommand bdout = new MySqlCommand("SELECT Тип FROM вс", con);
            MySqlDataReader bdreader = bdout.ExecuteReader();
            while (bdreader.Read())
            {
                comboBox2.Items.Add(bdreader[0]);
            }
            con.Close();
        }

        public void dataGridView1_MouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.ColumnIndex > 1 && e.RowIndex >= 0)
            {
                switch (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor.Name)
                {
                    case "Yellow": thisCell.busy_type = "ЭкипажРейса";
                        break;
                    case "LightCyan":  thisCell.busy_type = "Обучение";
                        break;
                    case "IndianRed": thisCell.busy_type = "ВЛЭК";
                        break;
                    case "YellowGreen":thisCell.busy_type = "Выходной";
                        break;
                }
                thisCell.date = dataGridView1.Columns[e.ColumnIndex].HeaderText.ToString();
                thisCell.person_id =Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value);
                contextMenuStrip1.Show(Cursor.Position);
            }
            if (e.Button == MouseButtons.Left && (e.ColumnIndex == 1 || e.ColumnIndex == 0) && e.RowIndex>=0)
            {
                person p = new person(Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value));
                p.ShowDialog();
            }
        }

        private void документыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2Docs docs = new Form2Docs();
            docs.Show();
        }

        private void toolStripTextBox1_DoubleClick(object sender, EventArgs e)  //нажатие Отменить для ячейки
        {
            thisCell.delete();
            contextMenuStrip1.Close();
            do_datagrid();
            dataGridView3.Rows.Clear();
        }

        private void рейсыToolStripMenuItem_Click(object sender, EventArgs e)  //создание формы со списком рейсов
        {
            flights f = new flights(pairs);
            f.ShowDialog();
        }

        private void рейсToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addFlight addf = new addFlight(thisCell.busy_type, thisCell.date, thisCell.person_id);
            addf.Show();
            addf.FormClosed += new FormClosedEventHandler(OnFormClosed);
        }

        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
           do_datagrid();
            dataGridView3.Rows.Clear();
       }


        private void вЛЭКToolStripMenuItem_Click(object sender, EventArgs e)
        {
            thisCell.busy_type = "ВЛЭК";
            thisCell.addWithoutF();
            do_datagrid();
        }

        private void выходнойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            thisCell.busy_type = "Выходной";
            thisCell.addWithoutF();
            do_datagrid();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            do_file(dataGridView1);
        }

        private void авиационныйПерсоналToolStripMenuItem_Click(object sender, EventArgs e)
        {
            people p = new people();
            p.ShowDialog();
        }

        private void обучениеToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            thisCell.busy_type = "Обучение";
        }

        private void cRMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            thisCell.addWithoutF("CRM");
            do_datagrid();
        }

        private void аСПСушаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            thisCell.addWithoutF("АСП(Суша)");
            do_datagrid();
        }

        private void аСПВодаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            thisCell.addWithoutF("АСП(Вода)");
            do_datagrid();
        }

        private void профессиональныйАнглийскийЯзыкToolStripMenuItem_Click(object sender, EventArgs e)
        {
            thisCell.addWithoutF("Английский язык");
            do_datagrid();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            do_datagrid(comboBox1.Text,comboBox2.Text,checkBox1.Checked);   
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox2.Text = "";
            checkBox1.Checked = false;
            do_datagrid();
        }

        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
                flightpeople(e.RowIndex,Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value));
        }

        private void помощьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            help h = new help();
            h.ShowDialog();
        }
    }
}
