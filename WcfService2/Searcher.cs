using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Text;

namespace WcfService2
{
    public class Searcher
    {
        private string serverName = "localhost";
        private string userName = "root";
        private string password = "admin";
        private string port = "228";
        private UserFactory userFactory = new UserFactory();
        private AdminFactory adminFactory = new AdminFactory();

        public Searcher()
        {

        }

        public Actor[] Seacrh(string login)
        {
            string connStr = "server=" + serverName +
            ";user=" + userName +
            ";database=" + "mymessanger" +
            ";port=" + port +
            ";password=" + password + ";";
            List<Actor> Actors = new List<Actor>();
            using (MySqlConnection connection = new MySqlConnection(connStr))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("select login, email, information, gender,admin from PDN where login LIKE '"
                    + login + "%'", connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            bool IsAdmin = (bool)reader["admin"];
                            string FoundLogin = (string)reader["login"];
                            string email = (string)reader["email"];
                            string info = (string)reader["information"];
                            bool sex = (bool)reader["gender"];
                            Actors.Add((IsAdmin ? adminFactory.CreateNewActor(FoundLogin, email, info, sex)
                                : userFactory.CreateNewActor(FoundLogin, email, info, sex)));
                        }
                        
                            

                    }
                }
            }
            return Actors.ToArray();

        }
    }
}