using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
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


namespace GuiClient
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        private Model model;
        public Model _model { get { return model; } set { model = value; } }
        public RegistrationWindow(Model model)
        {
            InitializeComponent();
            this.model = model;
        }

        

        private void Reg_Click(object sender, RoutedEventArgs e)
        {
            bool male = false ,female = false;
            
            try
            {
                male = (bool)Male.IsChecked;
                female = (bool)Female.IsChecked;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Select your gender please", "Error",MessageBoxButton.OK, MessageBoxImage.Error);
            }
            if (!male && !female)
                return;

            if (txtLogin.Text.Length > 3 && txtLogin.Text.Length < 64 && txtPassword.Password == txtPasswordR.Password && 
                                                                                          txtPassword.Password.Length > 4)
            {
                if (model.Registrate(txtLogin.Text, txtPassword.Password, txtEmail.Text, male) == false)
                    MessageBox.Show("Some error occured", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else 
                {
                    MessageBox.Show("You have been succesfull registrated!", "Success", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                    MainWindow window = new MainWindow();
                    this.Close();
                    window.Show();
                }
            }
            else
            {
                MessageBox.Show("Min lenght login - 4 character, password - 5", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void return_click(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = new MainWindow(model);
            window._model = this.model;
            this.Close();
            window.Show();
        }
    }
}
