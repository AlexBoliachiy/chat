using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace GuiClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Istrategy strategy { get; set; }
        private Model model;
        public Model _model { get { return model; } set { model = value; } }
        public MainWindow()
        {
            InitializeComponent();
            model = new Model();
        }
        public MainWindow(Model model)
        {
            InitializeComponent();
            this.model = model;
        }
        

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow regWindow = new RegistrationWindow(model);
            regWindow._model = this.model;
            this.Close();
            regWindow.Show();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            strategy =  model.Authorization(txtLogin.Text, txtPassword.Password);
            if (strategy != null)
                strategy.Start(model);
            else return;
            this.Close();
        }
    }
}
