using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsUserInterface;
using Screen = WindowsUserInterface;

namespace Game
{
    public class GameLogic
    {
        private const bool k_FaceUp = true;
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
        public GameLogic(byte i_Height, byte i_Width)
        {
            this.r_NumOfRows = i_Width;
            this.r_NumOfCols = i_Height;
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
        /// <exception cref="ArgumentNullException"></exception>
        private byte[] shuffleCard(ref byte[] io_CharArrToShuffle)
        {
            int len = io_CharArrToShuffle.Length;

            if (len == 0)
            {
                throw new ArgumentNullException(nameof(io_CharArrToShuffle));
            }

            for (int s = 0; s < io_CharArrToShuffle.Length - 1; s++)
            {
                int indexOfnewValueFor_s = generateAnotherNum(s, len);
                (io_CharArrToShuffle[indexOfnewValueFor_s], io_CharArrToShuffle[s]) = (io_CharArrToShuffle[s], io_CharArrToShuffle[indexOfnewValueFor_s]);
            }

            return io_CharArrToShuffle;
        }

        private int generateAnotherNum(int i_LowerLimit, int i_UpperLimit)
        {
            Random random = new Random();
            return random.Next(i_LowerLimit, i_UpperLimit);
        }

        // ===================================================================
        // Properties and Indexers
        // ===================================================================
        public int Length
        {
            get
            {
                return Rows * Columns;
            }
        }

        public bool HaveMoreMoves
        {
            get
            {
                return Length - FlippedCardsCounter > 0;
            }
        }

        private Card this[string i_IndexFormt]
        {
            get
            {
                configIndexFormat(i_IndexFormt, out int io_rowIndex, out int io_colIndex);
                return this[(byte)io_rowIndex, (byte)io_colIndex];
            }

            set
            {
                configIndexFormat(i_IndexFormt, out int io_rowIndex, out int io_colIndex);
                this[(byte)io_rowIndex, (byte)io_colIndex] = value;
            }
        }

        private Card this[BoardLocation i_IndexFormt]
        {
            get
            {
                return this[i_IndexFormt.Row, i_IndexFormt.Col];
            }

            set
            {
                this[i_IndexFormt.Row, i_IndexFormt.Col] = value;
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
                r_GameBoard[i_Rows, i_Columns] = value;
            }
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
        public bool DoThePlayersChoicesMatch(out byte o_ScoreForTheTurn, params BoardLocation[] i_ArgsChosenInTurn)
        {
            o_ScoreForTheTurn = 0;
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
                o_ScoreForTheTurn = 1;
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

        private void configIndexFormat(string i_IndexFormt, out int o_RowIndex, out int o_ColIndex)
        {
            o_ColIndex = 0;
            bool isUpper = false;
            char charToFindTheIndex = char.ToUpper(i_IndexFormt[0]);
            string charToReplaceTheIndex = i_IndexFormt.Substring(1);
            bool isSuccessTryParse = int.TryParse(charToReplaceTheIndex, out o_RowIndex);
            o_RowIndex--;

            // check if col exists
            for (int i = 0; i < Columns; i++)
            {
                if (charToFindTheIndex == sr_ABC[i])
                {
                    o_ColIndex = i;
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
i_IndexFormt,
charToReplaceTheIndex,
charToFindTheIndex,
isSuccessTryParse,
isUpper,
r_GameBoard[o_RowIndex, o_ColIndex]));
            }

            bool isInvalueRow = o_RowIndex < 0 || o_RowIndex >= Rows;
            bool isInvalueCol = o_ColIndex < 0 || o_ColIndex >= Columns;
            if (isInvalueRow || isInvalueCol)
            {
                throw new IndexOutOfRangeException("Index out of range in configIndexFormat");
            }
        }
    }
}
