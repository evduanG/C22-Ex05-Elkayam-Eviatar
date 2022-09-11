using System;
using System.Collections.Generic;
using System.Linq;
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

        public void ShowBoard(string[,] i_GameBoard)
        {
            byte row = 0;
            byte col = 0;

            foreach (string link in i_GameBoard)
            {
                if (col < i_GameBoard.Rank)
                {
                    col = 0;
                    row++;
                }

                if (link != string.Empty)
                {
                    MemorySlot memoryToCheck = new MemorySlot(link, row, col);

                    if (!r_Memory.Contains(memoryToCheck))
                    {
                        r_Memory.Add(memoryToCheck);
                    }
                }

                col++;
            }
        }

        public ButtonIndexEvent GetPlayerChoice(List<BoardLocation> i_ValidSlotTOChase, string[,] i_boardToDraw)
        {
            BoardLocation ans = BoardLocation.Defult();
            bool isFind = false;

            if (r_Memory.Count != 0)
            {
                if (i_ValidSlotTOChase.Count % 2 == 0)
                {
                    isFind = getAIFirstPlayerChoice(i_ValidSlotTOChase, ref ans);
                }
                else
                {
                    isFind = getAISecondPlayerChoice(i_ValidSlotTOChase, i_boardToDraw, ref ans);
                }
            }

            if (!isFind)
            {
                ans = getRandomChoice(i_ValidSlotTOChase);
            }

            return new ButtonIndexEvent(ans);
        }

        private bool getAIFirstPlayerChoice(List<BoardLocation> i_ValidSlotTOChase, ref BoardLocation io_Location)
        {
            r_Memory.Sort();
            // i_ValidSlotTOChase.Sort();
            bool isItFound = false;
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
                    if (!i_ValidSlotTOChase.Contains(valueSecond.BoardLocation))
                    {
                        r_Memory.Remove(valueFirst);
                        isItFound = false;
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            io_Location = isItFound ? valueFirst.BoardLocation : BoardLocation.Defult();

            return isItFound;
        }

        private bool getAISecondPlayerChoice(List<BoardLocation> i_validSlotToChase, string[,] i_boardToDraw, ref BoardLocation io_Location)
        {
            r_Memory.Sort();
            MemorySlot returnedIndex = default;
            bool isItFound = false;

            foreach (string value in i_boardToDraw)
            {
                foreach (MemorySlot mem in r_Memory)
                {
                    if (mem.Value == value)
                    {
                        returnedIndex = mem;
                    }
                }

                if (((object)returnedIndex) != null)
                {
                    isItFound = i_validSlotToChase.Contains(returnedIndex.BoardLocation);

                    // in case of possible move
                    if (isItFound)
                    {
                        break;
                    }
                }
            }

            io_Location = isItFound ? (BoardLocation)returnedIndex.BoardLocation : BoardLocation.Defult();

            return isItFound;
        }

        private BoardLocation getRandomChoice(List<BoardLocation> i_validSlotTOChase)
        {
            int randomTile = sr_Random.Next(i_validSlotTOChase.Count);

            return i_validSlotTOChase[randomTile];
        }

        internal struct MemorySlot
        {
            private string m_Value;
            private BoardLocation m_BoardLocation;

            public string Value
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
            public MemorySlot(string i_Value, byte i_Row, byte i_col)
            {
                m_Value = i_Value;
                m_BoardLocation = new BoardLocation(i_Row, i_col);
            }

            public MemorySlot(string i_Value, BoardLocation i_BoardLocation)
            {
                m_Value = i_Value;
                m_BoardLocation = i_BoardLocation;
            }

            // implementing CompareTo for sort()
            //public int CompareTo(object obj)
            //{

            //    //int ans;

            //    //if (this > (MemorySlot?)obj)
            //    //{
            //    //    ans = 1;
            //    //}
            //    //else
            //    //{
            //    //    ans = -1;
            //    //}

            //    //return ans;
            //}

            public static bool operator ==(MemorySlot i_Other1, MemorySlot i_Other2)
            {
                return i_Other1.Value == i_Other2.Value;
            }

            public static bool operator !=(MemorySlot i_Other1, MemorySlot i_Other2)
            {
                return !(i_Other1 == i_Other2);
            }

            //public static bool operator >(MemorySlot i_Other1, MemorySlot i_Other2)
            //{
            //    return i_Other1.Value > i_Other2.Value;
            //}

            //public static bool operator <(MemorySlot i_Other1, MemorySlot i_Other2)
            //{
            //    return !(i_Other1 > i_Other2);
            //}

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
