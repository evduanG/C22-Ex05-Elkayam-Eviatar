using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsUserInterface
{
    internal class Tester
    {
        public static void Main()
        {
            // Application.EnableVisualStyles();
            MainGameForm gameForm = new MainGameForm(4, 4, "Elkayam", "Eviatar");
            gameForm.ShowDialog();
        }
    }
}
