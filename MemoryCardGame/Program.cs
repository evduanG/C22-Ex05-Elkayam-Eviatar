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
            GameEngine game = new GameEngine();
            game.DisplaySetUpForm();
        }
    }
}