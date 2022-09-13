using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsUserInterface
{
    public class ButtonIndexEvent : EventArgs
    {
        private readonly BoardLocation r_Location;

        public byte Row
        {
            get { return r_Location.Row; }
        }

        public byte Col
        {
            get { return r_Location.Col; }
        }

        public BoardLocation Location
        {
            get { return r_Location; }
        }

        public ButtonIndexEvent(byte i_Row, byte i_Col)
        {
            r_Location = new BoardLocation(i_Row, i_Col);
        }

        public ButtonIndexEvent(BoardLocation i_Location)
        {
            r_Location = i_Location;
        }

        public static ButtonIndexEvent Parse(string i_ButtonIndexString)
        {
            byte row = byte.Parse(i_ButtonIndexString.Substring(0, 1));
            byte col = byte.Parse(i_ButtonIndexString.Substring(1, 1));

            return new ButtonIndexEvent(row, col);
        }

        public override string ToString()
        {
            return r_Location.ToString();
        }
    }
}
