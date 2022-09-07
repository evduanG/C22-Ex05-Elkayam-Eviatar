using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsUserInterface;

namespace MemoryCardGame
{
    public class Program
    {
        public static void Main()
        {
            // Application.EnableVisualStyles();
            SetUpNewGameForm form = SetUpNewGameForm.StartGameForm();
            form.SetListOfBordSizeOptions(4, 6, 4, 6);
            form.ShowDialog();
            form.GetSelectedDimensions(out byte o_Height, out byte o_Width);

            MainGameForm gameForm = new MainGameForm(o_Height, o_Width, form.FirstPlayerName, form.SecondPlayerName, form.FirstPlayerName);
            gameForm.ShowDialog();

            SetUpNewGameForm form1 = SetUpNewGameForm.RestartGameForm(form.FirstPlayerName, form.SecondPlayerName);
            form1.ShowDialog();
        }
    }
}