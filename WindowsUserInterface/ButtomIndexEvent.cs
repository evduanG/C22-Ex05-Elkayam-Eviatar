using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsUserInterface
{
    public class ButtomIndexEvent : EventArgs
    {
        public static readonly char[] sr_ABC =
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

        private const string k_ToStringFormt = "{0} {1}";
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

        public ButtomIndexEvent(byte i_Row, byte i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }

        public override string ToString()
        {
            return string.Format(k_ToStringFormt, sr_ABC[m_Col], m_Row);
        }
    }
}
