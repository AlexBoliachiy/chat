using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;


namespace WcfService2
{
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IMessanger
    {
        [OperationContract]
        bool AddFriend(string Login, int id);

        [OperationContract]
        bool Ban(string Login);

        [OperationContract]
        int Authorization(string Login, string Password);

        [OperationContract]
        bool RegisterNewUser(string Login, string Password);

        [OperationContract]
        bool SendMessage(int id, Message msg);

        [OperationContract]
        List<Message> GetNewMessage(String login, int id);

        [OperationContract]
        bool AddEmail(int id, string login, string email);

        [OperationContract]
        bool AddInformation(int id, string login, string info);

        [OperationContract]
        bool AddGender(int id, string login, bool gender);

        [OperationContract]
        Actor[] Search(int id, string login);

        [OperationContract]
        User getUser();

        [OperationContract]
        Admin getAdmin();

        [OperationContract]
        void Disconnect();

    }
}