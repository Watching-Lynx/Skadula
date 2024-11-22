using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Skadula
{
    class bd
    {                  // создание класса для работы с бд

        public bd()     //конструктор, чтобы работать с бд
        {                            //нужно будет сначала авторизоваться
        }



        public void createCon()
        {
            MySqlConnection con = new MySqlConnection("server=localhost;user=root;database=courses;port=3306");  //создание объекта соединения
            con.Open();           //открыть соединение
            MySqlCommand bdout = new MySqlCommand("SELECT `Фамилия_П` FROM `преподаватели` where `ID_Преподавателя` = 4", con);  //создать объект для отправки и приема запроса
            string order = bdout.ExecuteScalar().ToString();  //перевести в строку
            Console.WriteLine(order);
            con.Close();        //закрыть соединение
        }

    }

}
