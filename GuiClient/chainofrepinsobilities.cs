using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuiClient
{
    public abstract class Handler
    {
        protected Handler successor;
        public void SetSuccessor(Handler successor)
        {
            this.successor = successor;
        }

        public abstract void Handle(Model model, MessangerWindow window);

    }

    public class UserCreator : Handler
    {
        public override void Handle(Model model, MessangerWindow window)
        {
            window = new MessangerWindow(model);
            successor.Handle(model, window);
        }
    }

    public class AdminCreator : Handler
    {
        public override void Handle(Model model, MessangerWindow window)
        {
            if (model.ID == 2)
            {
                window.AddAdminInteface();
            }
            successor.Handle(model, window);
        }
    }

    public class Show : Handler
    {
        public override void Handle(Model model, MessangerWindow window)
        {
            window.Show();
        }
    }

    public class Chain
    {
        private Show show;
        public UserCreator userCreator; // Вот отсюда доступ
        private AdminCreator adminCreator;

        public Chain()
        {
            show = new Show();
            userCreator = new UserCreator();
            adminCreator = new AdminCreator();

            userCreator.SetSuccessor(adminCreator);
            adminCreator.SetSuccessor(show);
        }
    }
}
