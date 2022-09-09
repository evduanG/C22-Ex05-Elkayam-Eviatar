using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsUserInterface
{
    public class ButtomIndexEvent : EventArgs
    {
        private BoardLocation m_Location;

        public byte Row
        {
            get { return m_Location.Row; }
        }

        public byte Col
        {
            get { return m_Location.Col; }
        }

        public BoardLocation Location
        {
            get { return m_Location; }
        }

        public ButtomIndexEvent(byte i_Row, byte i_Col)
        {
            m_Location = new BoardLocation(i_Row, i_Col);
        }

        public ButtomIndexEvent(BoardLocation i_Location)
        {
            m_Location = i_Location;
        }

        public static ButtomIndexEvent Parse(string i_ButtonIndexString)
        {
            byte row = byte.Parse(i_ButtonIndexString.Substring(0, 1));
            byte col = byte.Parse(i_ButtonIndexString.Substring(2, 1));

            return new ButtomIndexEvent(row, col);
        }

        public override string ToString()
        {
            return m_Location.ToString();
        }
    }
}
