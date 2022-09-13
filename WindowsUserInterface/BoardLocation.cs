using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsUserInterface
{
    public struct BoardLocation
    {
        private const string k_ToStringFormtForSetUpBord = "{0} x {1}";
        private const string k_ToStringFormt = "{0} {1}";
        private static readonly char[] sr_ABC =
        {
            'A',
            'B',
            'C',
            'D',
            'E',
            'F',
            'G',
            'H',
            'I',
            'J',
            'K',
            'L',
            'M',
            'N',
            'O',
            'P',
            'Q',
            'R',
            'S',
            'T',
            'U',
            'V',
            'W',
            'X',
            'Y',
            'Z',
        };

        private byte m_Row;
        private byte m_Col;

        public byte Row
        {
            get { return m_Row; }
        }

        public byte Col
        {
            get { return m_Col; }
        }

        public BoardLocation(byte i_Row, byte i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public static BoardLocation Defult()
        {
            return new BoardLocation(byte.MaxValue, byte.MaxValue);
        }

        public static bool TryParse(string i_BoardLocString, out BoardLocation o_BoardLocation)
        {
            bool isRow = byte.TryParse(i_BoardLocString.Substring(0, 1), out byte o_Row);
            bool isCol = byte.TryParse(i_BoardLocString.Substring(1, 1), out byte o_Col);

            if(isCol && isRow)
            {
                o_BoardLocation = new BoardLocation(o_Row, o_Col);
            }
            else
            {
                o_BoardLocation = new BoardLocation(0, 0);
                Console.WriteLine("Try parse failed " + i_BoardLocString);
            }

            return isRow && isCol;
        }

        public override string ToString()
        {
            return string.Format(k_ToStringFormt, m_Col, m_Row);
        }

        public string GetStrForSetUpBord()
        {
            return string.Format(k_ToStringFormtForSetUpBord, Row, Col);
        }
    }
}
