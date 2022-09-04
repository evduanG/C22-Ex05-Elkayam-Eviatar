using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryCardGame
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            GameEngine game = new GameEngine();
            game.Start();
        }
    }
}
