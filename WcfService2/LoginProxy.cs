using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


namespace WcfService2
{
    public class LoginProxy : LoginControl
    {
        private LoginControl loginControl = new LoginControl();
        private MySqlConnection dbConn;
        private ServerState currentState;
        private Overflow overflow;
        private Enough enough;
        SHA1 encrypter = new SHA1CryptoServiceProvider();




        public bool Ban(string Login)
        {
            return Update("isbanned", Login, "1");
        }

        public override Dictionary<string, int> GetUsers()
        {
            return loginControl.GetUsers();
        }


        public LoginProxy()
        {
            overflow = new Overflow(loginControl);
            enough = new Enough(loginControl);
            currentState = enough;
            IniDB();
        }

        public override int Autorizathion(string login, string password)
        {
            if (loginControl.GetCountConnection > 100)
                currentState = overflow;
            else
                currentState = enough;

            if (IsBanned(login))
                return -2;
            if (Authentification(login, password) == true)
            {
                if (IsAdmin(login))
                    return 2;
                return currentState.Autorizathion(login, password);
            }
            else
                return -1;
        }

        private bool Authentification(string login, string password)
        {
            if (login == null || password == null || login == string.Empty || password == string.Empty)
                return false;



            var tmp = encrypter.ComputeHash(GetBytes(password));
            var tmp2 = GetHashedPasswordFromBD(login);
            return EqualBytes(encrypter.ComputeHash(GetBytes(password)), GetHashedPasswordFromBD(login));


        }
        public bool IsBanned(string login)
        {
            string query = "SELECT isbanned FROM PDN WHERE login='" + login + "'";
            MySqlCommand command = new MySqlCommand(query, dbConn);
            command.Connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            bool result = false;
            try
            {
                while (reader.Read())
                {
                    result = (bool)reader["isbanned"];
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: \r\n{0}", ex.ToString());
            }
            finally
            {
                command.Connection.Close();
                reader.Close();
            }
            return result;
        }

        public bool IsAdmin(string login)
        {
            string query = "SELECT admin FROM PDN WHERE login='" + login + "'";
            MySqlCommand command = new MySqlCommand(query, dbConn);
            command.Connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            bool result = false;
            try
            {
                while (reader.Read())
                {
                    result = (bool)reader["admin"];
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: \r\n{0}", ex.ToString());
            }
            finally
            {
                command.Connection.Close();
                reader.Close();
            }
            return result;
        }

        public override bool Registration(string login, string password)
        {
            if (login == null || login == String.Empty || password == null || password == string.Empty)
                return false;

            if (GetHashedPasswordFromBD(login) != null) // Same login already exist
                return false;

            bool success = false;
            try
            {

                using (MySqlCommand cmd = new MySqlCommand())
                {
                    cmd.Connection = dbConn;
                    cmd.CommandText = "INSERT INTO PDN (login, password) VALUES (?login, ?password);";
                    MySqlParameter loginpar = new MySqlParameter("?login", MySqlDbType.VarChar, 45);
                    MySqlParameter passwordpar = new MySqlParameter("?password", MySqlDbType.Blob, 20);

                    loginpar.Value = login;
                    passwordpar.Value = encrypter.ComputeHash(GetBytes(password));

                    cmd.Parameters.Add(loginpar);
                    cmd.Parameters.Add(passwordpar);
                    cmd.Connection.Open();
                    cmd.ExecuteNonQuery();
                    success = true;
                }
                Console.WriteLine(login + " new user. succesed save");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: \r\n{0}", ex.ToString());
            }

            finally
            {
                dbConn.Close();
            }

            return success;

        }

        public Byte[] GetHashedPasswordFromBD(string login)
        {
            if (login == null || login == String.Empty)
                return null;
            string query = "SELECT password FROM PDN WHERE login='" + login + "'";
            MySqlCommand command = new MySqlCommand(query, dbConn);
            command.Connection.Open();
            MySqlDataReader reader = command.ExecuteReader();
            Byte[] password = null;
            try
            {
                while (reader.Read())
                {
                    password = ObjectToByteArray((reader["password"]));
                }

            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: \r\n{0}", ex.ToString());
            }
            finally
            {
                command.Connection.Close();
                reader.Close();
            }
            //Данные из бд возвращаются с мусором. Приходится обрезать таким образом
            if (password != null)
            {
                Byte[] tmp = new Byte[20];
                for (int i = 0; i < 20; i++)
                {
                    tmp[i] = password[i + 27];
                }
                password = tmp;
            }
            return password;

        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
        static bool EqualBytes(Byte[] arr1, byte[] arr2)
        {
            if (arr1 == null || arr2 == null)
                return false;
            for (int i = 0; i < arr1.Length; i++)
                if (arr1[i] != arr2[i])
                    return false;
            return true;

        }

        public override void Disconect()
        {
            loginControl.Disconect();
        }
        public bool AddGender(int id, string login, bool gender)
        {
             return Update("gender", login, Convert.ToInt32(gender).ToString());
        }
        public bool AddEmail(int id, string login, string email)
        {
            return Update("email", login, email);
        }

        public bool AddInformation(int id, string login, string info)
        {
            bool update = false;
            try
            {
                if (id == users[login])
                    update = Update("information", login, info);
            }

            catch (KeyNotFoundException)
            {
                update = Update("information", login, info);
            }
            return update;
        }
        public bool Update(string FieldName, string login, string info)
        {
            bool success = false;
            try
            {
                
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    dbConn.Open();
                    cmd.Connection = dbConn;
                    cmd.CommandText = "UPDATE PDN SET " + FieldName + "='" + info + "'  WHERE login ='"+ login + "';";  
                    cmd.ExecuteNonQuery();
                    success = true;
                }
                Console.WriteLine(login + " new user. succesed save");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error: \r\n{0}", ex.ToString());
            }
            finally
            {
                dbConn.Close();
            }

            return success;
        }
        private void IniDB()
        {
            string serverName = "localhost";
            string userName = "root";
            string password = "admin";
            string port = "228";
            string connStr = "server=" + serverName +
                ";user=" + userName +
                ";database=" + "mymessanger" +
                ";port=" + port +
                ";password=" + password + ";";

            //Testing connection
            dbConn = new MySqlConnection(connStr);
            try
            {
                dbConn.Open();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Cannot connect to database");
                Console.WriteLine(ex.Number);
                Console.Read();
                Environment.Exit(ex.Number);
            }
            finally
            {
                dbConn.Close();
            }
        }
    }

    public abstract class ServerState
    {
        protected LoginControl loginManager;
        public ServerState(LoginControl loginManager)
        {
            this.loginManager = loginManager;
        }

        public abstract int Autorizathion(string login, string password);
    }


    public class Overflow : ServerState
    {
        public Overflow(LoginControl loginManager): base(loginManager)
        {

        }

        public override int Autorizathion(string login, string password)
        {
            return -1;
        }
    }

    public class Enough : ServerState
    {
        public Enough(LoginControl loginManager): base(loginManager)
        {

        }

        public override int Autorizathion(string login, string password)
        {
            return loginManager.Autorizathion(login, password);
        }
    }
}