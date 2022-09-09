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
        private readonly byte r_Row;
        private readonly byte r_Col;

        public byte Row
        {
            get { return r_Row; }
        }

        public byte Col
        {
            get { return r_Col; }
        }

        public ButtomIndexEvent(byte i_Row, byte i_Col)
        {
            r_Row = i_Row;
            r_Col = i_Col;
            Console.WriteLine(string.Format(k_ToStringFormt, i_Row, i_Col));
        }

        public static ButtomIndexEvent Parse(string i_ButtonIndexString)
        {
            byte row = byte.Parse(i_ButtonIndexString.Substring(0, 1));
            byte col = byte.Parse(i_ButtonIndexString.Substring(2, 1));

            return new ButtomIndexEvent(row, col);
        }

        public override string ToString()
        {
            return string.Format(k_ToStringFormt, sr_ABC[r_Col], r_Row);
        }
    }
}
