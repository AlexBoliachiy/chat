using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;

namespace WcfService2
{
    public abstract class AbsLoginControl
    {
        protected Dictionary<string, int> users = new Dictionary<string, int>();
        protected static int autID = 0;
        public abstract int Autorizathion(string login, string password);
        public abstract bool Registration(string login, string password);
        public int GetCountConnection { get { return autID; } }
    }


    public class LoginControl : AbsLoginControl
    {

        public virtual Dictionary<string, int> GetUsers()
        {
            return users;
        }

        public override int Autorizathion(string login, string password)
        {
            users.Add(login, ++autID);
            return autID;
        }

        public override bool Registration(string login, string password)
        {
            throw new NotImplementedException();
        }
        public virtual void Disconect()
        {
            autID--;
        }

        

        


    }
}