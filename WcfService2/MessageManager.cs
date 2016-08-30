using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace WcfService2
{
    public class MessageManager
    {
        
        private List<Message> MessageStorage = new List<Message>();
        private string connStr;
        string serverName = "localhost";
        string userName = "root";
        string password = "admin";
        string port = "228";
        public MessageManager()
        {
            
            connStr = "server=" + serverName +
                ";user=" + userName +
                ";database=" + "mymessanger" +
                ";port=" + port +
                ";password=" + password + ";";
        }

        public bool SendMessage(Message msg)
        {
            bool success = false;
            this.MessageStorage.Add(msg);
            using (MySqlConnection connection = new MySqlConnection(connStr))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.CommandText = "INSERT INTO MSG (toer, fromer, message) VALUES (?fromwho, ?towho, ?message);";
                    MySqlParameter topar = new MySqlParameter("?fromwho", MySqlDbType.VarChar, 64);
                    MySqlParameter frompar = new MySqlParameter("?towho", MySqlDbType.VarChar, 64);
                    MySqlParameter messagepar = new MySqlParameter("?message", MySqlDbType.VarChar, 512);

                    topar.Value = msg.to;
                    frompar.Value = msg.from;
                    messagepar.Value = msg.message;

                    cmd.Parameters.Add(topar);
                    cmd.Parameters.Add(frompar);
                    cmd.Parameters.Add(messagepar);
                    cmd.Connection = connection;
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    success = true;
                }
            }
            return success;
        }


        public List<Message> GetOldMessage(string login)
        {
            List<Message> msg = new List<Message>();
            foreach (Message cashmessage in MessageStorage.ToArray())
            {
                if (cashmessage.to == login)
                {
                    msg.Add(cashmessage.Clone() as Message);
                    MessageStorage.Remove(cashmessage);
                }
            }
            using (MySqlConnection connection = new MySqlConnection(connStr))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM MSG WHERE toer='" + login +"' OR fromer='" + login + "' ORDER by timeadded;", connection))
                {

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string from = reader["fromer"] as string;
                            string to = reader["toer"] as string;
                            string message = reader["message"] as string;
                            DateTime datetime = (DateTime)reader["timeadded"];
                            msg.Add(new Message(to, from, message));

                        }
                    }
                }
                


            }
            return msg.Distinct().ToList();
        }

        public List<Message> GetNewMessage(string login)
        {
            List<Message> msg = new List<Message>();
            foreach (Message cashmessage in MessageStorage.ToArray())
            {
                if (cashmessage.to == login)
                {
                    msg.Add(cashmessage.Clone() as Message);
                    MessageStorage.Remove(cashmessage);
                }
            }
            using (MySqlConnection connection = new MySqlConnection(connStr))
            {
                connection.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM MSG WHERE toer='" + login + "' AND readed = '0';", connection))
                {
                    
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string from = reader["fromer"] as string;
                            string to = reader["toer"] as string;
                            string message = reader["message"] as string;
                            DateTime datetime = (DateTime)reader["timeadded"];
                            msg.Add(new Message(to, from, message, datetime));

                        }



                    }
                        cmd.CommandText = "UPDATE MSG SET readed = '1' WHERE toer='" + login + "' AND readed='0';";
                        cmd.ExecuteNonQuery();
                }
            }
            return msg.Distinct().ToList();
        }

    }
}