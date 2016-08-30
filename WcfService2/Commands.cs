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
    public class Message : ICloneable
    {
        [DataMember]
        public string from { get; set; }
        [DataMember]
        public string to { get; set; }
        [DataMember]
        public string message { get; set; }
        [DataMember]
        public DateTime datetime { get; set; }
        public Message(string to, string from, string message)
        {
            this.to = to;
            this.from = from;
            this.message = message;
        }
        public Message(string to, string from, string message, DateTime datetime)
        {
            this.to = to;
            this.from = from;
            this.message = message;
            this.datetime = datetime;
        }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            Message objmsg = obj as Message;
            if (objmsg.from == this.from && objmsg.to == this.to && objmsg.message == this.message )
                return true;
            else
                return false;
        }
    }

    [DataContract]
    public class FriendRequest
    {
        [DataMember]
        public string from { get; set; }
        [DataMember]
        public string to { get; set; }
    }
    
}