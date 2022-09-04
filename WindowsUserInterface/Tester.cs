using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsUserInterface
{
    internal class Tester
    {

        public static void Main()
        {
            MainGameForm gameForm = new MainGameForm(6, 6, "elka", "avi");
            gameForm.ShowDialog();

            SetUpNewGameForm form = SetUpNewGameForm.StartGameForm();
            form.ShowDialog();

        }

        public void ButtonStart_Click(object i_Sender, EventArgs e)
        {
            Console.WriteLine("Start");

        }
    }
}
