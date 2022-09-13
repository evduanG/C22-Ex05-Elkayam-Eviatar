using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsUserInterface;
using Screen = WindowsUserInterface;

namespace Game
{
    public class GameLogic
    {
        private const bool k_FaceUp = true;
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

        private readonly byte r_NumOfCols;
        private readonly byte r_NumOfRows;
        private readonly Card[,] r_GameBoard;
        private byte m_FlippedCardsCounter;

        /******     Dimensions   ******/

        public byte Rows
        {
            get
            {
                return r_NumOfRows;
            }
        }

        public byte Columns
        {
            get
            {
                return r_NumOfCols;
            }
        }

        public byte FlippedCardsCounter
        {
            get
            {
                return m_FlippedCardsCounter;
            }

            set
            {
                m_FlippedCardsCounter = value;
            }
        }

        /// constructor
        public GameLogic(byte i_height, byte i_width)
        {
            /******     Dimensions   ******/
            this.r_NumOfRows = i_width;
            this.r_NumOfCols = i_height;
            this.m_FlippedCardsCounter = 0;

            byte size = (byte)(Rows * Columns);
            byte numOfPairs = (byte)(size >> 1);
            byte[] randomImagesIndexes = SettingAndRules.GetRandomImagesIndexes(numOfPairs);

            shuffleCard(ref randomImagesIndexes);
            r_GameBoard = new Card[Rows, Columns];
            byte indexInChars = 0;

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    r_GameBoard[i, j] = new Card(randomImagesIndexes[indexInChars++], !k_FaceUp);
                }
            }
        }

        // ===================================================================
        // methods that the constructor uses
        // ===================================================================
        // return a letter by index
        private char getCharForSlat(byte i_index)
        {
            return sr_ABC[i_index >> 1];
        }

        public void ApplyAllTheButtons(Screen.MainGameForm i_GameForm)
        {
            for(byte i = 0; i < Rows; i++)
            {
                for(byte j = 0; j < Columns; j++)
                {
                    i_GameForm[i, j].Click += r_GameBoard[i, j].Card_Clicked;
                }
            }
        }

        /// function to Shuffle array the char array before creation
        /// need to change to any array
        /// <exception cref="ArgumentNullException"></exception>
        private byte[] shuffleCard(ref byte[] i_CharArrToShuffle)
        {
            int len = i_CharArrToShuffle.Length;
            if (len == 0)
            {
                throw new ArgumentNullException(nameof(i_CharArrToShuffle));
            }

            for (int s = 0; s < i_CharArrToShuffle.Length - 1; s++)
            {
                int indexOfnewValueFor_s = generateAnotherNum(s, len); // note the range

                // swap procedure: note, char h to store initial i_CharArrToShuffle[s] i_Value
                (i_CharArrToShuffle[indexOfnewValueFor_s], i_CharArrToShuffle[s]) = (i_CharArrToShuffle[s], i_CharArrToShuffle[indexOfnewValueFor_s]);
            }

            return i_CharArrToShuffle;
        }

        /// Let unknown GenerateAnotherNum be a random number
        private int generateAnotherNum(int from, int to)
        {
            Random random = new Random();
            return random.Next(from, to);
        }

        // ===================================================================
        // Properties
        // ===================================================================

        /// Length is the total number of slots in the array
        public int Length
        {
            get
            {
                return Rows * Columns;
            }
        }

        // return true if there are move available moves
        public bool HaveMoreMoves
        {
            get
            {
                return Length - FlippedCardsCounter > 0;
            }
        }

        /// indexer:
        private Card this[string i_indexFormt]
        {
            get
            {
                configIndexFormat(i_indexFormt, out int io_rowIndex, out int io_colIndex);
                return this[(byte)io_rowIndex, (byte)io_colIndex];
            }

            set
            {
                configIndexFormat(i_indexFormt, out int io_rowIndex, out int io_colIndex);
                this[(byte)io_rowIndex, (byte)io_colIndex] = value;
            }
        }

        private Card this[BoardLocation i_indexFormt]
        {
            get
            {
                return this[i_indexFormt.Row, i_indexFormt.Col];
            }

            set
            {
                this[i_indexFormt.Row, i_indexFormt.Col] = value;
            }
        }

        private Card this[byte i_Rows, byte i_Columns]
        {
            get
            {
                try
                {
                    return r_GameBoard[i_Rows, i_Columns];
                }
                catch (Exception e)
                {
                    throw new Exception(string.Format("i_Rows = {0}, Rows = {1}, i_Columns= {2}, Columns= {3}", i_Rows, Rows, i_Columns, Columns), e);
                }
            }

            set
            {
                // isValidLocation(i_Rows, i_Columns);
                r_GameBoard[i_Rows, i_Columns] = value;
            }
        }

        // return true if locations is still hidden
        private bool isValidLocation(byte i_Rows, byte i_Columns)
        {
            bool isValidRow = SettingAndRules.Rules.IsBetween(i_Rows, Rows, 0);
            bool isValidCol = SettingAndRules.Rules.IsBetween(i_Columns, Columns, 0);

            if (!isValidRow || !isValidCol)
            {
                throw new IndexOutOfRangeException("Index out of range in configIndexFormat");
            }

            return isValidRow && isValidCol;
        }

        // flip a card
        public Image Flipped(BoardLocation i_Index, bool i_Value)
        {
            Card c = this[i_Index];
            c.Flipped = i_Value;
            this[i_Index] = c;
            Console.WriteLine("index: " + i_Index);
            return SettingAndRules.GetImage(this[i_Index].ImageIndex);
        }

        // return true if the player got another turn
        public bool DoThePlayersChoicesMatch(out byte io_scoreForTheTurn, params BoardLocation[] i_ArgsChosenInTurn)
        {
            io_scoreForTheTurn = 0;
            bool isPair = true;

            if (i_ArgsChosenInTurn.Length != 2)
            {
                throw new Exception("The number of cards does not match the format: " + i_ArgsChosenInTurn.Length.ToString());
            }

            Card firstCard = this[i_ArgsChosenInTurn[0]];

            foreach (BoardLocation index in i_ArgsChosenInTurn)
            {
                isPair = firstCard == this[index];

                if (!isPair)
                {
                    break;
                }
            }

            if (!isPair)
            {
                foreach (BoardLocation index in i_ArgsChosenInTurn)
                {
                    Flipped(index, !k_FaceUp);
                }
            }
            else
            {
                FlippedCardsCounter += 2;
                io_scoreForTheTurn = 1;
            }

            return isPair;
        }

        // ===================================================================
        // methods that use to draw the board
        // ===================================================================
        // return board to draw
        public string[,] GetBoardToDraw()
        {
            string[,] boardToDraw = new string[Rows, Columns];
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    string str = r_GameBoard[i, j].Flipped ? r_GameBoard[i, j].ImageIndex.ToString() : string.Empty;
                    boardToDraw[i, j] = str;
                }
            }

            return boardToDraw;
        }

        // ===================================================================
        // methods that are used to select a new tile
        // ===================================================================
        // return list of available tiles to choose from
        public List<BoardLocation> GetAllValidTilesForChoice()
        {
            List<BoardLocation> validSlots = new List<BoardLocation>();
            for (byte i = 0; i < Rows; i++)
            {
                for (byte j = 0; j < Columns; j++)
                {
                    bool isCardFlip = r_GameBoard[i, j].Flipped;

                    if (!isCardFlip)
                    {
                        validSlots.Add(new BoardLocation(i, j));
                    }
                }
            }

            return validSlots;
        }

        private void configIndexFormat(string i_indexFormt, out int io_rowIndex, out int io_colIndex)
        {
            io_colIndex = 0;
            bool isUpper = false;
            char charToFindTheIndex = char.ToUpper(i_indexFormt[0]);
            string charToReplaceTheIndex = i_indexFormt.Substring(1);
            bool isSuccessTryParse = int.TryParse(charToReplaceTheIndex, out io_rowIndex);
            io_rowIndex--;

            // check if col exists
            for (int i = 0; i < Columns; i++)
            {
                if (charToFindTheIndex == sr_ABC[i])
                {
                    io_colIndex = i;
                    isUpper = true;
                    break;
                }
            }

            if (!isSuccessTryParse || !isUpper)
            {
                throw new IndexOutOfRangeException(string.Format(
@"Index out of range in configIndexFormat => 
the string (index): {0}
 subStrOfNum :{1} 
charToFindTheIndex : {2} 
isSuccessTryParse : {3} 
isUpper : {4}
m_GameBoard[io_rowIndex, io_colIndex] : {5}",
i_indexFormt,
charToReplaceTheIndex,
charToFindTheIndex,
isSuccessTryParse,
isUpper,
r_GameBoard[io_rowIndex, io_colIndex]));
            }

            bool isInvalueRow = io_rowIndex < 0 || io_rowIndex >= Rows;
            bool isInvalueCol = io_colIndex < 0 || io_colIndex >= Columns;
            if (isInvalueRow || isInvalueCol)
            {
                throw new IndexOutOfRangeException("Index out of range in configIndexFormat");
            }
        }

        // represents a game card
        private struct Card
        {
            private byte m_ImageIndex;
            private bool m_Flipped;

            /// constructor
            public Card(byte i_Value, bool i_Flipped)
            {
                m_ImageIndex = i_Value;
                m_Flipped = i_Flipped;
            }

            public Card(byte value)
                : this()
            {
                m_ImageIndex = value;
                m_Flipped = false;
            }

            // Properties
            public byte ImageIndex
            {
                get
                {
                    byte retunValue = byte.MaxValue;
                    if (Flipped)
                    {
                        retunValue = m_ImageIndex;
                    }

                    return retunValue;
                }

                set
                {
                    m_ImageIndex = value;
                }
            }

            public bool Flipped
            {
                get
                {
                    return m_Flipped;
                }

                set
                {
                    m_Flipped = value;
                }
            }

            public static bool operator ==(Card i_card1, Card i_card2)
            {
                return i_card1.Equals(i_card2);
            }

            public static bool operator !=(Card i_card1, Card i_card2)
            {
                return !i_card1.Equals(i_card2);
            }

            public override bool Equals(object i_comperTo)
            {
                return this.ImageIndex == ((Card)i_comperTo).ImageIndex;
            }

            public override int GetHashCode()
            {
                int hashCode = 1148891178;
                hashCode = (hashCode * -1521134295) + m_ImageIndex.GetHashCode();
                hashCode = (hashCode * -1521134295) + m_Flipped.GetHashCode();
                hashCode = (hashCode * -1521134295) + ImageIndex.GetHashCode();
                hashCode = (hashCode * -1521134295) + Flipped.GetHashCode();
                return hashCode;
            }

            internal void Card_Clicked(object i_Sender, EventArgs i_EventArgs)
            {
                if (i_Sender is Button clickedButton)
                {
                    clickedButton.Enabled = false;
                    Flipped = true;
                    // clickedButton.Text = ImageIndex;
                }
            }
        }
    }
}
