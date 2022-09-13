using System;

namespace WindowsUserInterface
{
    public struct BoardLocation
    {
        private const string k_ToStringFormtForSetUpBord = "{0} x {1}";
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

        public BoardLocation(byte i_Row, byte i_Col)
        {
            r_Row = i_Row;
            r_Col = i_Col;
        }

        public static BoardLocation Default()
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
            return string.Format(k_ToStringFormt, r_Col, r_Row);
        }

        public string GetStrForSetUpBord()
        {
            return string.Format(k_ToStringFormtForSetUpBord, Row, Col);
        }
    }
}
