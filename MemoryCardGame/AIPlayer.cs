using System;
using System.Collections.Generic;
using Game;
using WindowsUserInterface;

namespace MemoryCardGame
{
    public delegate void AIGameMoveHandler(ButtonIndexEvent i_ButtonIndexEvent);

    internal class AIPlayer : Player
    {
        public const string k_NamePc = "PC";
        private static readonly Random sr_Random = new Random();
        private readonly List<MemorySlot> r_Memory;

        // TODO : enable the form when the AI playing
        // public AIPlayer(List<MemorySlot> memory)
        // {
        //    base()
        //    r_Memory = memory;
        // }
        public AIPlayer(byte i_ID)
            : base(k_NamePc, i_ID)
        {
            r_Memory = new List<MemorySlot>();
        }

        private List<MemorySlot> Memory
        {
            get
            {
                return r_Memory;
            }
        }

        public void RestartNewGame()
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
                    MemorySlot memoryToCheck = new MemorySlot(ch, row, col);

                    if (!r_Memory.Contains(memoryToCheck))
                    {
                        r_Memory.Add(memoryToCheck);
                    }
                }

                col++;
            }
        }

        public ButtonIndexEvent GetPlayerChoice(
            List<string> i_ValidSlotTOChase,
            char[,] i_boardToDraw)
        {
            BoardLocation? ans = null;
            if (r_Memory.Count != 0)
            {
                if (i_ValidSlotTOChase.Count % 2 == 0)
                {
                    ans = getAIFirstPlayerChoice(i_ValidSlotTOChase);
                }
                else
                {
                    ans = getAISecondPlayerChoice(i_ValidSlotTOChase, i_boardToDraw);
                }
            }

            bool heveSmartChoice = ans != null;
            BoardLocation returnLocation;

            if (!heveSmartChoice)
            {
                returnLocation = getRandomChoice(i_ValidSlotTOChase);
            }
            else
            {
                returnLocation = (BoardLocation)ans;
            }

            return new ButtonIndexEvent(returnLocation);
        }

        private BoardLocation? getAIFirstPlayerChoice(List<string> i_ValidSlotTOChase)
        {
            r_Memory.Sort();
            i_ValidSlotTOChase.Sort();
            MemorySlot valueFirst = r_Memory[0];
            MemorySlot valueSecond;

            foreach (MemorySlot memorySlot in r_Memory)
            {
                if (memorySlot.StrLocation == valueFirst.StrLocation)
                {
                    continue;
                }

                valueSecond = valueFirst;
                valueFirst = memorySlot;

                if (valueSecond.Value == valueFirst.Value)
                {
                    if (!i_ValidSlotTOChase.Contains(valueSecond.StrLocation))
                    {
                        r_Memory.Remove(valueFirst);
                        bool isItFound = true;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            BoardLocation? returnValue = valueFirst.BoardLocation;

            return (BoardLocation)returnValue;
        }

        private BoardLocation? getAISecondPlayerChoice(
            List<string> i_validSlotToChase,
            char[,] i_boardToDraw)
        {
            r_Memory.Sort();
            BoardLocation? returnedIndex = null;

            foreach (char value in i_boardToDraw)
            {
                foreach (MemorySlot mem in r_Memory)
                {
                    char memVal = mem.Value;
                    if (memVal == value)
                    {
                        returnedIndex = mem.BoardLocation;
                    }
                }

                bool isItFound = i_validSlotToChase.Contains(returnedIndex.ToString());

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

        private BoardLocation getRandomChoice(List<string> i_validSlotTOChase)
        {
            int randomTile = sr_Random.Next(i_validSlotTOChase.Count);
            _ = BoardLocation.TryParse(i_validSlotTOChase[randomTile], out BoardLocation o_BoardLocation);

            return o_BoardLocation;
        }

        internal struct MemorySlot : IComparable
        {
            private char m_Value;
            private BoardLocation m_BoardLocation;

            public char Value
            {
                get { return m_Value; }
                set { m_Value = value; }
            }

            public string StrLocation
            {
                get { return m_BoardLocation.ToString(); }
            }

            public BoardLocation BoardLocation
            {
                get { return m_BoardLocation; }
            }

            /// ===============================================
            // Constructors
            /// ===============================================
            public MemorySlot(char i_Value, byte i_Row, byte i_col)
            {
                m_Value = i_Value;
                m_BoardLocation = new BoardLocation(i_Row, i_col);
            }

            public MemorySlot(char i_Value, BoardLocation i_BoardLocation)
            {
                m_Value = i_Value;
                m_BoardLocation = i_BoardLocation;
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

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return base.ToString();
            }
        }
    }
}
