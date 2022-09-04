using Game;

namespace MemoryCardGame
{
    internal struct AIPlayer
    {
        private static readonly Random sr_Random = new Random();
        private readonly List<MemorySlot> m_Memory = new List<MemorySlot>();

        public AIPlayer(List<MemorySlot> memory)
        {
            Memory = memory;
        }

        public AIPlayer() : this(new List<MemorySlot>()) { }

        private List<MemorySlot> Memory
        {
            get
            {
                return m_Memory;
            }
        }

        public void ResetMemory()
        {
            Memory.Clear();
        }

        // $G$ CSS-013 (-3) Bad parameter name (should be in the form of i_PascalCase).
        public void ShowBoard(char[,] i_gameBoard)
        {
            byte row = 0;
            byte col = 0;

            foreach (char ch in i_gameBoard)
            {
                if (col < i_gameBoard.Rank)
                {
                    col = 0;
                    row++;
                }

                if (ch != ' ')
                {
                    MemorySlot memoryToCheck = new MemorySlot(ch, string.Format("{0}{1}", GameLogic.sr_ABC[col], row));

                    if (!m_Memory.Contains(memoryToCheck))
                    {
                        m_Memory.Add(memoryToCheck);
                    }
                }

                col++;

            }
        }

        // $G$ CSS-013 (-3) Bad parameter name (should be in the form of i_PascalCase).
        internal string GetAIPlayerChoice(List<string> i_validSlotTOChase,
            char[,] i_boardToDraw)
        {
            string? ans = null;

            if (i_validSlotTOChase.Count % 2 == 0)
            {
                ans = getAIFirstPlayerChoice(i_validSlotTOChase);
            }
            else
            {
                ans = getAISecondPlayerChoice(i_validSlotTOChase, i_boardToDraw);
            }

            // when AI don't know what to do
            if (ans == null)
            {
                ans = getRandomChoice(i_validSlotTOChase);
            }

            return ans;
        }

        // $G$ CSS-013 (-3) Bad parameter name (should be in the form of i_PascalCase).
        private string? getAIFirstPlayerChoice(List<string> i_validSlotTOChase)
        {
            m_Memory.Sort();
            i_validSlotTOChase.Sort();
            MemorySlot valueFirst = m_Memory.First();
            MemorySlot valueSecond;
            bool isItFound = false;

            foreach (MemorySlot str in m_Memory)
            {
                if (str.Index == valueFirst.Index)
                {
                    continue;
                }

                valueSecond = valueFirst;
                valueFirst = str;

                if (valueSecond.Value == valueFirst.Value)
                {
                    if (!i_validSlotTOChase.Contains(valueSecond.Index))
                    {
                        m_Memory.Remove(valueFirst);
                        isItFound = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }
            }

            return (isItFound) ? valueFirst.Index : null;
        }

        private string? getAISecondPlayerChoice(List<string> i_validSlotToChase,
            char[,] i_boardToDraw)
        {
            m_Memory.Sort();
            string? returnedIndex = "";

            foreach (char value in i_boardToDraw)
            {
                foreach (MemorySlot mem in m_Memory)
                {
                    char memVal = mem.Value;
                    if (memVal == value)
                    {
                        returnedIndex = mem.Index;
                    }
                }

                bool isItFound = i_validSlotToChase.Contains(returnedIndex);

                // in case of possible move
                if (isItFound)
                {
                    break;
                }
                // otherwise choose null
                else
                {
                    returnedIndex = null;
                }
            }

            return returnedIndex;
        }

        private string getRandomChoice(List<string> i_validSlotTOChase)
        {
            int randomTile = sr_Random.Next(i_validSlotTOChase.Count);

            return i_validSlotTOChase[randomTile];
        }

        internal struct MemorySlot : IComparable
        {
            char m_Value;
            string m_Index;

            //===============================================
            // Properties
            //===============================================
            public char Value
            {
                get { return m_Value; }
                set { m_Value = value; }
            }

            public string Index
            {
                get { return m_Index; }
                set { m_Index = value; }
            }

            //===============================================
            // Constructor
            //===============================================
            public MemorySlot()
            {
                m_Value = ' ';
                m_Index = "";
            }


            //===============================================
            // Methods
            //===============================================
            public MemorySlot(char i_value, string i_index)
            {
                m_Index = i_index;
                m_Value = i_value;
            }

            // implementing CompareTo for sort()
            public int CompareTo(object? obj)
            {
                int ans;

                if (this > (MemorySlot?)obj)
                {
                    ans = 1;
                }
                else
                {
                    ans = -1;
                }

                return ans;
            }

            public static bool operator ==(MemorySlot i_Other1, MemorySlot i_Other2)
            {
                return i_Other1.Value == i_Other2.Value;
            }

            public static bool operator !=(MemorySlot i_Other1, MemorySlot i_Other2)
            {
                return !(i_Other1 == i_Other2);
            }

            public static bool operator >(MemorySlot i_Other1, MemorySlot i_Other2)
            {
                return i_Other1.Value > i_Other2.Value;
            }

            public static bool operator <(MemorySlot i_Other1, MemorySlot i_Other2)
            {
                return !(i_Other1 > i_Other2);
            }

            public override bool Equals(object? obj)
            {
                return this == (MemorySlot?)obj;
            }

        }
    }

}
