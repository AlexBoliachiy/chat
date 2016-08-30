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
   [DataContract]
   [KnownType(typeof(User))]
   [KnownType(typeof(Admin))]
    abstract public class Actor
    {
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public bool Sex { get; set; }
        [DataMember]
        public string info { get; set; }

        public Actor(string login, string email, string info, bool sex)
        {
            this.Login = login;
            this.Email = email;
            this.info = info;
            this.Sex = sex;
        }
    }

    [DataContract]
    public class User : Actor
    {
        [DataMember]
        public bool IsAdmin = false;
        public User(string login, string email, string info, bool sex) : base(login, email, info, sex)
        {

        }
    }

    [DataContract]
    public class Admin : Actor
    {
        [DataMember]
        public bool IsAdmin = true;
        public Admin(string login, string email, string info, bool sex) : base(login, email, info, sex)
        {

        }
    }


       

    abstract public class Factory
    {
        abstract public Actor CreateNewActor(string login, string email, string info, bool sex);
    }

    public class UserFactory
    {
        public Actor CreateNewActor(string login, string email, string info, bool sex)
        {
            return new User(login, email, info, sex);
        }
    }

    /// <summary>
    /// Wcf не поддерживает override методов, так что фабрика вынуждена создавать почти однотипные классы.
    /// </summary>
    public class AdminFactory
    {
        public Actor CreateNewActor(string login, string email, string info, bool sex)
        {
            return new Admin(login, email, info, sex);
        }
    }



}