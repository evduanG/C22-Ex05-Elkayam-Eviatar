using Game;
using System;
using System.Collections.Generic;

namespace MemoryCardGame
{
    internal struct AIPlayer
    {
        private static readonly Random sr_Random = new Random();
        private readonly List<MemorySlot> r_Memory;

        public AIPlayer(List<MemorySlot> memory)
        {
            r_Memory = memory;
        }

        public static AIPlayer CreateNew()
        {
            return new AIPlayer(new List<MemorySlot>());
        }

        private List<MemorySlot> Memory
        {
            get
            {
                return r_Memory;
            }
        }

        public void ResetMemory()
        {
            Memory.Clear();
        }

        public void ShowBoard(char[,] i_GameBoard)
        {
            byte row = 0;
            byte col = 0;

            foreach (char ch in i_GameBoard)
            {
                if (col < i_GameBoard.Rank)
                {
                    col = 0;
                    row++;
                }

                if (ch != ' ')
                {
                    MemorySlot memoryToCheck = new MemorySlot(ch, string.Format("{0}{1}", GameLogic.sr_ABC[col], row));

                    if (!r_Memory.Contains(memoryToCheck))
                    {
                        r_Memory.Add(memoryToCheck);
                    }
                }

                col++;
            }
        }

        internal string GetAIPlayerChoice(List<string> i_ValidSlotTOChase,
            char[,] i_boardToDraw)
        {
            string ans = string.Empty;

            if (i_ValidSlotTOChase.Count % 2 == 0)
            {
                ans = getAIFirstPlayerChoice(i_ValidSlotTOChase);
            }
            else
            {
                ans = getAISecondPlayerChoice(i_ValidSlotTOChase, i_boardToDraw);
            }

            // when AI don't know what to do
            if (ans == null)
            {
                ans = getRandomChoice(i_ValidSlotTOChase);
            }

            return ans;
        }

        private string getAIFirstPlayerChoice(List<string> i_ValidSlotTOChase)
        {
            r_Memory.Sort();
            i_ValidSlotTOChase.Sort();
            MemorySlot valueFirst = r_Memory[0];
            MemorySlot valueSecond;
            bool isItFound = false;

            foreach (MemorySlot str in r_Memory)
            {
                if (str.Index == valueFirst.Index)
                {
                    continue;
                }

                valueSecond = valueFirst;
                valueFirst = str;

                if (valueSecond.Value == valueFirst.Value)
                {
                    if (!i_ValidSlotTOChase.Contains(valueSecond.Index))
                    {
                        r_Memory.Remove(valueFirst);
                        isItFound = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            return isItFound ? valueFirst.Index : null;
        }

        private string getAISecondPlayerChoice(
            List<string> i_validSlotToChase,
            char[,] i_boardToDraw)
        {
            r_Memory.Sort();
            string returnedIndex = "";

            foreach (char value in i_boardToDraw)
            {
                foreach (MemorySlot mem in r_Memory)
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
                else
                {
                    // otherwise choose null
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
            private char m_Value;
            private string m_Index;

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

            /// ===============================================
            // Constructor
            /// ===============================================
            public MemorySlot(char i_value, string i_index)
            {
                m_Index = i_index;
                m_Value = i_value;
            }

            // implementing CompareTo for sort()
            public int CompareTo(object obj)
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

            public override bool Equals(object obj)
            {
                return this == (MemorySlot?)obj;
            }
        }
    }

}
