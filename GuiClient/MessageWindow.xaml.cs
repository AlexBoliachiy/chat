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
using System.IO;
using System.Xml;
using System.Windows.Markup;

namespace GuiClient
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class MessangerWindow : Window
    {
        private TabItemFactory  tabItemFactory = new TabItemFactory();
        private Model model;
        public Model _model { get { return model; } set { model = value; } }
        public MessangerWindow()
        {
            InitializeComponent();

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public MessangerWindow(Model model)
        {
            InitializeComponent();
            this.model = model;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (model == null)
                return;
            Message[] NewMessage = model.GetNewMessage(); //
            foreach (Message msg in NewMessage)
            {
                AddMessage(msg.from, msg.from, msg.message, model.Search(msg.from)[0].Sex, msg.datetime);
            }
        }

        private void AddMessage(string DialogName, string from, string message, bool Sex, DateTime datetime)
        {
            
            foreach (TabItem x in Tab.Items)
            {
                if ((string)x.Header == DialogName)
                {
                    ScrollViewer scrollViewer = (ScrollViewer)x.Content;
                    StackPanel stackPanel = (StackPanel)(scrollViewer.Content);
                    TextBlock text = stackPanel.Children[0] as TextBlock;
                    text.Text += "\n";
                    text.Text += from + " " + datetime.ToString() + "\n";
                    text.Text += message;
                    return;
                }
            }
            Tab.Items.Add(tabItemFactory.CreateNewTab(DialogName, Sex));
            AddMessage(DialogName, from, message, Sex, datetime);

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            Actor[] actors = model.Search(searchText.Text);
            NameStack.Children.Clear();
            foreach (Actor x in actors)
            {

                Label label = new Label {Name=x.Login, Width = 100, Height = 40, Content = x.Login, ToolTip = (x.Sex ? "Male" : "Female") };
                label.MouseDoubleClick += (s, ev) => {
                    if (label.Name == model.Login)
                        return;
                    foreach (TabItem y in Tab.Items)
                    {
                        if (y == null)
                            continue;
                        if ((string)y.Header == x.Login)
                            return;
                    }
                        Tab.Items.Add(tabItemFactory.CreateNewTab(x.Login, x.Sex));

                };
                NameStack.Children.Add(label);
            }
        }

        private void MessageBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;
            if (Tab.SelectedItem == null)
                return;
            TabItem curremtItem = ((TabItem)Tab.SelectedItem);
            string to = (String)(((TabItem)Tab.SelectedItem).Header);
            model.SendMessage(MessageBox.Text, to);
            AddMessage(to, model.Login, MessageBox.Text, false, DateTime.Now); // Тут неважно какой пол, потому что вкладка уже создана
            MessageBox.Text = "";
        }

        private void exit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            this.model = null;
            window.Show();
        }

        public void SetModel(Model model)
        {
            this._model = model;
        }

        public void AddAdminInteface()
        {
            TabItem tabitem = new TabItem { Header = "AdminBar" };
            StackPanel stc = new StackPanel();
            TextBox box = new TextBox { Width = 80, Height = 20 };
            Button btn = new Button { Margin = new Thickness(100, 100, 100, 100), Width = 80, Height = 20, Content="Banhammer" };
            btn.Click += (o, e) =>
            {
                model.Ban(box.Text);
            };
            stc.Children.Add(btn);
            stc.Children.Add(box);
            tabitem.Content = stc;
            Tab.Items.Add(tabitem);
        }
        ~MessangerWindow()
        {
            model.Disconnect();
        }

    }
    public class TabItemFactory
    {
        private TabItem CreateNewTabBlue(string name)
        {
            TabItem tabItem = new TabItem { Header = name };
            ScrollViewer scrollViewer = new ScrollViewer { HorizontalAlignment = HorizontalAlignment.Right };
            StackPanel stackPanel = new StackPanel { Width = 600, Height = 1200, Background = Brushes.LightBlue, Orientation = Orientation.Horizontal };
            TextBlock textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.Wrap;

            stackPanel.Children.Add(textBlock);
            scrollViewer.Content = stackPanel;
            tabItem.Content = scrollViewer;

            return tabItem;
        }

        private TabItem CreateNewTabPink(string name)
        {
            TabItem tabItem = new TabItem { Header = name };
            ScrollViewer scrollViewer = new ScrollViewer { HorizontalAlignment = HorizontalAlignment.Right };
            StackPanel stackPanel = new StackPanel { Width = 600, Height = 1200, Background = Brushes.LightPink, Orientation = Orientation.Horizontal };
            TextBlock textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.Wrap;

            stackPanel.Children.Add(textBlock);
            scrollViewer.Content = stackPanel;
            tabItem.Content = scrollViewer;

            return tabItem;
        }
        public TabItem CreateNewTab(string name, bool sex)
        {
            return sex ? CreateNewTabBlue(name) : CreateNewTabPink(name);
        }

        



    }
}
