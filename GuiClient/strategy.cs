using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GuiClient
{
    public interface Istrategy
    {
        void Start( Model model);
    }
    
    public class Opened : Istrategy
    {
        public void Start( Model model)
        {
            Chain chain = new Chain();
            chain.userCreator.Handle(model, null);
            
        }
    }
    public class Banned : Istrategy
    {
        public void Start( Model model)
        {
            Window window = new BannedWindow();
            window.Show();

        }
    }

}
