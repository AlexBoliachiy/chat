using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Security.Cryptography;
using MySql.Data.MySqlClient;

namespace WcfService2
{
    public class MessangerService : IMessanger
    {
        private LoginProxy loginManager = new LoginProxy();
        private MessageManager messageManager = new MessageManager();
        private Searcher searcher = new Searcher();

        public void Disconnect()
        {
            loginManager.Disconect();
        }

        public bool AddFriend(string name, int ID)
        {
            return false;
        }

        public bool Ban(string Login)
        {
            return loginManager.Ban(Login);
        }


        public User getUser()
        {
            return null; 
        }

        public Admin getAdmin()
        {
            return null; 
        }

        public int Authorization(string login, string password)
        {
            return loginManager.Autorizathion(login, password);
        }

        public bool RegisterNewUser(string login, string password)
        {

            return loginManager.Registration(login, password);
        }

        public bool SendMessage(int id, Message msg)
        {

            return messageManager.SendMessage(msg);
        }

        public List<Message> GetNewMessage(String login, int id)
        {
            
            return messageManager.GetNewMessage(login);
        }

        public bool AddGender(int id, string login, bool gender)
        {
            return loginManager.AddGender(id, login, gender);
        }

        public bool AddInformation(int id, string login, string info)
        {
            return loginManager.AddInformation(id, login, info);
        }

        public bool AddEmail(int id, string login, string email)
        {
            return loginManager.AddEmail(id, login, email);
        }

        public Actor[] Search(int id, string login)
        {
            return searcher.Seacrh(login);
        }


    }
}
