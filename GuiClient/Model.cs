using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GuiClient.ServiceReference1;
using System.ServiceModel.Description;
using System.ServiceModel;

namespace GuiClient
{
    public class Model
    {

        private MessangerClient client;
        private  int id = -1;
        public int ID { get { return id; } private set { id = value; } }
        private  string login;
        public string Login { get { return login; } private set { login = value; } }

        public Model()
        {
            BasicHttpBinding basicbinding = new BasicHttpBinding();
            EndpointAddress endpointAdress = new EndpointAddress("http://localhost:8000/GettingStarted/CalculatorService/");
            client = new MessangerClient(basicbinding, endpointAdress);
        }

        public void Disconnect()
        {
            client.Disconnect();
        }

        public Istrategy Authorization(string login, string password)
        { 
            // Интересно, это можно считать за магические числа следовательно плохой стиль кодирования?

            id = client.Authorization(login, password);
            if (id == -1)
            {
                MessageBox.Show("Incorrect login or/and password!", "MyMessanger", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            else if (id == 1 || id == 2)
            {
                this.login = login;
                return new Opened();
            }

            else
                return new Banned();
            

        }

        public bool Registrate(string login, string password, string Email, bool Sex)
        {
            bool success = client.RegisterNewUser(login, password);
            if (success == false)
                return false;

            if (Email != String.Empty)
                client.AddEmail(id, login, Email);
            success = client.AddGender(id, login, Sex);
            return success;
            
        }

        public Actor[] Search(string login)
        {
           return client.Search(id, login);
            
        }

        public Message[] GetNewMessage()
        {
            return client.GetNewMessage(login, id);
        }

        public bool SendMessage(string text, string to)
        {
            Message msg = new Message();
            msg.from = this.Login;
            msg.to = to;
            msg.message = text;
            return client.SendMessage(id, msg);
        }

        public bool Ban(string Login)
        {
            if (Login == this.login)
            {
                MessageBox.Show("Why u wanna ban yourself?");
                return false;
            }
            else
            {
                return client.Ban(Login);
            }

               
        }
    }

}

