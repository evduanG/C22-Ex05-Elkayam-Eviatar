using System;
using System.Collections.Generic;
using System.Linq;
using Game;
using WindowsUserInterface;

namespace MemoryCardGame
{
    public delegate void AIGameMoveHandler(BoardLocationEventArgs i_BoardLocationEventArgs);

    internal class AIPlayer : Player
    {
        public const string k_NamePc = "PC";
        private static readonly Random sr_Random = new Random();
        private readonly List<MemorySlot> r_Memory;

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

        public BoardLocationEventArgs GetPlayerChoice(List<BoardLocation> i_ValidSlotTOChase, string[,] i_BoardToDraw)
        {
            BoardLocation ans = BoardLocation.Default();
            bool isFind = false;
            bool isMemoryEmpty = r_Memory.Count == 0;

            if (!isMemoryEmpty)
            {
                bool isFirstMove = i_ValidSlotTOChase.Count % 2 == 0;

                if (isFirstMove)
                {
                    isFind = getAIFirstPlayerChoice(i_ValidSlotTOChase, ref ans);
                }
                else
                {
                    isFind = getAISecondPlayerChoice(i_ValidSlotTOChase, i_BoardToDraw, ref ans);
                }
            }

            if (!isFind)
            {
                ans = getRandomChoice(i_ValidSlotTOChase);
            }

            return new BoardLocationEventArgs(ans);
        }

        private bool getAIFirstPlayerChoice(List<BoardLocation> i_ValidSlotToChase, ref BoardLocation io_Location)
        {
            r_Memory.Sort();
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
                    if (!i_ValidSlotToChase.Contains(valueSecond.BoardLocation))
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

            io_Location = isItFound ? valueFirst.BoardLocation : BoardLocation.Default();

            return isItFound;
        }

        private bool getAISecondPlayerChoice(List<BoardLocation> i_ValidSlotToChase, string[,] i_BoardToDraw, ref BoardLocation io_Location)
        {
            r_Memory.Sort();
            MemorySlot returnedIndex = default;
            bool isItFound = false;

            foreach (string value in i_BoardToDraw)
            {
                foreach (MemorySlot mem in r_Memory)
                {
                    if (mem.Value == value)
                    {
                        returnedIndex = mem;
                    }
                }

                bool isReturnedIndexEmpty = ((object)returnedIndex) == null;

                if (!isReturnedIndexEmpty)
                {
                    isItFound = i_ValidSlotToChase.Contains(returnedIndex.BoardLocation);

                    if (isItFound)
                    {
                        break;
                    }
                }
            }

            io_Location = isItFound ? (BoardLocation)returnedIndex.BoardLocation : BoardLocation.Default();

            return isItFound;
        }

        private BoardLocation getRandomChoice(List<BoardLocation> i_ValidSlotTOChase)
        {
            int randomTile = sr_Random.Next(i_ValidSlotTOChase.Count);

            return i_ValidSlotTOChase[randomTile];
        }

        internal struct MemorySlot : IComparable
        {
            private readonly BoardLocation r_BoardLocation;
            private string m_Value;

            public string Value
            {
                get { return m_Value; }
                set { m_Value = value; }
            }

            public string StrLocation
            {
                get { return r_BoardLocation.ToString(); }
            }

            public BoardLocation BoardLocation
            {
                get { return r_BoardLocation; }
            }

            /// ===============================================
            // Constructors
            /// ===============================================
            public MemorySlot(string i_Value, byte i_Row, byte i_Col)
            {
                m_Value = i_Value;
                r_BoardLocation = new BoardLocation(i_Row, i_Col);
            }

            public MemorySlot(string i_Value, BoardLocation i_BoardLocation)
            {
                m_Value = i_Value;
                r_BoardLocation = i_BoardLocation;
            }

            public static bool operator ==(MemorySlot i_Other1, MemorySlot i_Other2)
            {
                return i_Other1.Value == i_Other2.Value;
            }

            public static bool operator !=(MemorySlot i_Other1, MemorySlot i_Other2)
            {
                return !(i_Other1 == i_Other2);
            }

            public override bool Equals(object i_Obj)
            {
                return this == (MemorySlot?)i_Obj;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override string ToString()
            {
                return base.ToString();
            }

            public int CompareTo(object i_Obj)
            {
                return string.Compare(Value, ((MemorySlot)i_Obj).Value);
            }
        }
    }
}
