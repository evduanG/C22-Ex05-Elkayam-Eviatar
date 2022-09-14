using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace WindowsUserInterface
{
    public delegate void AnyPictureBoxHandler(object i_Sender, BoardLocationEventArgs i_EventArgs);

    public class MainGameForm : Form
    {
        public const int k_Margin = 10;
        public const int k_PictureBoxSize = 90;
        private const int k_WindowHeightModifier = 16;
        private const int k_WindowWidthModifier = 3;
        private const bool k_Enabled = true;
        private const string k_GameTitle = "Memory game";
        private const string k_CurrentPlayerLabel = "Current Player : {0}";
        private readonly byte r_NumOfRows;
        private readonly byte r_NumOfCols;

        public event AnyPictureBoxHandler AnyPictureBoxClick;

        private readonly Label[] r_Players;
        private Label m_CurrentPlayerName;
        private PictureBox[,] m_GameBoardCards;

        // =======================================================
        // constructor  and methods for the constructor
        // =======================================================
        public MainGameForm(byte i_BoardHeight, byte i_BoardWidth, byte i_NumOfPlayers)
        {
            r_NumOfCols = i_BoardHeight;
            r_NumOfRows = i_BoardWidth;
            r_Players = new Label[i_NumOfPlayers];
            initializeComponents();
        }

        public void RestartGame()
        {
            foreach(PictureBox pb in m_GameBoardCards)
            {
                pb.Image = null;
                pb.BackColor = DefaultColor;
                pb.Enabled = k_Enabled;
            }
        }

        // =======================================================
        // Properties and Indexers
        // =======================================================
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

        public PictureBox[,] GameBoardCards
        {
            get { return m_GameBoardCards; }
        }

        public Label CurrentPlayerName
        {
            get { return m_CurrentPlayerName; }
            set { m_CurrentPlayerName = value; }
        }

        public Color DefaultColor
        {
            get
            {
                return Color.LightGray;
            }
        }

        public Label[] Players
        {
            get { return r_Players; }
        }

        public PictureBox this[byte i_Row, byte i_Col]
        {
            get
            {
                return m_GameBoardCards[i_Row, i_Col];
            }

            set
            {
                m_GameBoardCards[i_Row, i_Col] = value;
            }
        }

        public PictureBox this[BoardLocation i_BoardLocation]
        {
            get
            {
                return this[i_BoardLocation.Row, i_BoardLocation.Col];
            }

            set
            {
                this[i_BoardLocation.Row, i_BoardLocation.Col] = value;
            }
        }

        // Initializers:
        private void initializeComponents()
        {
            initializeMainForm();
            initilizeGameBoardCards();
            initializeLabels();
            ElementsDesignerTool.FitTheSizeOfForm(this, k_Margin);
        }

        private void initilizeGameBoardCards()
        {
            m_GameBoardCards = new PictureBox[Rows, Columns];
            createPicBox();
            positionPicBoxsOnGrid();
        }

        private void initializeMainForm()
        {
            Text = k_GameTitle;
            Size = getWindowSize();
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
        }

        private void positionPicBoxsOnGrid()
        {
            // setup the rest
            int upOneSlot = 0;

            for (int i = 0; i < Rows; i++)
            {
                bool isTopLeft = i == 0;

                Control controlToPlace;
                if (isTopLeft)
                {
                    GameBoardCards[0, 0].Top = k_Margin;
                    GameBoardCards[0, 0].Left = k_Margin;
                    Controls.Add(GameBoardCards[0, 0]);
                }
                else
                {
                    upOneSlot = i - 1;
                    controlToPlace = GameBoardCards[i, 0];
                    ElementsDesignerTool.DesignElements(controlToPlace, ePositionBy.Left, GameBoardCards[upOneSlot, 0]);
                    ElementsDesignerTool.DesignElements(controlToPlace, ePositionBy.Under, GameBoardCards[upOneSlot, 0], k_Margin);
                    Controls.Add(GameBoardCards[i, 0]);
                }

                for (int j = 1; j < Columns; j++)
                {
                    controlToPlace = GameBoardCards[i, j];
                    ePositionBy topPositionBy = i == 0 ? ePositionBy.Top : ePositionBy.Under;
                    ElementsDesignerTool.DesignElements(controlToPlace, ePositionBy.NextToTheLeft, GameBoardCards[i, j - 1], k_Margin);
                    ElementsDesignerTool.DesignElements(controlToPlace, topPositionBy, GameBoardCards[upOneSlot, j], k_Margin);
                    Controls.Add(GameBoardCards[i, j]);
                }
            }
        }

        private void initializeLabels()
        {
            // setup current player
            CurrentPlayerName = new Label();
            PictureBox lastPicBox = GameBoardCards[GameBoardCards.GetLength(0) - 1, 0];
            ElementsDesignerTool.DesignElements(CurrentPlayerName, ePositionBy.Under, lastPicBox, k_Margin);
            ElementsDesignerTool.DesignElements(CurrentPlayerName, ePositionBy.Left, lastPicBox);
            CurrentPlayerName.AutoSize = true;
            Controls.Add(CurrentPlayerName);

            Label temp = CurrentPlayerName;

            for(int i = 0; i < r_Players.Length; i++)
            {
                r_Players[i] = new Label();
                ElementsDesignerTool.DesignElements(r_Players[i], ePositionBy.Under, temp, k_Margin);
                ElementsDesignerTool.DesignElements(r_Players[i], ePositionBy.Left, temp);
                r_Players[i].AutoSize = true;
                temp = r_Players[i];
                Controls.Add(r_Players[i]);
            }
        }

        private void createPicBox()
        {
            // create PicBoxes
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GameBoardCards[i, j] = new PictureBox()
                    {
                        Size = new Size(k_PictureBoxSize, k_PictureBoxSize),
                        BackColor = DefaultColor,
                    };
                    GameBoardCards[i, j].Click += GameBoardPictureBox_Click;
                }
            }
        }

        private Size getWindowSize()
        {
            int buttonSizeAndMargin = k_PictureBoxSize + k_Margin;
            int formHeight = (buttonSizeAndMargin * Rows) + (k_WindowHeightModifier * k_Margin);
            int formWidth = (buttonSizeAndMargin * Columns) + (k_WindowWidthModifier * k_Margin);

            return new Size(formWidth, formHeight);
        }

        // =======================================================
        // Methods for updating during the game
        // =======================================================
        public void SetPlayerNamesAndScore(string i_PlayerNameAndScore, byte i_ID)
        {
            r_Players[i_ID].Text = i_PlayerNameAndScore;
        }

        public void FlippCardToFaceUp(BoardLocation i_PicBoxLocationInGrid, Image i_CardImage)
        {
            this[i_PicBoxLocationInGrid].Image = i_CardImage;
            this[i_PicBoxLocationInGrid].SizeMode = PictureBoxSizeMode.CenterImage;
            this[i_PicBoxLocationInGrid].Enabled = false;
        }

        public void ColorPair(List<BoardLocation> i_PlayerChoice, Color i_PlayerColor)
        {
            foreach (BoardLocation choice in i_PlayerChoice)
            {
                GameBoardCards[choice.Row, choice.Col].BackColor = i_PlayerColor;
            }
        }

        public void FlippCardsToFaceDown(List<BoardLocation> i_PlayerChoice)
        {
            foreach (BoardLocation choice in i_PlayerChoice)
            {
                PictureBox picBoxClick = GameBoardCards[choice.Row, choice.Col];
                picBoxClick.Enabled = k_Enabled;
                picBoxClick.BackColor = DefaultColor;
                picBoxClick.Image = null;
            }
        }

        public void SetCurrentPlayer(string i_PlayerName, Color i_PlayerColor)
        {
            CurrentPlayerName.Text = string.Format(k_CurrentPlayerLabel, i_PlayerName);
            CurrentPlayerName.BackColor = i_PlayerColor;
        }

        public void SetPlayer(string i_PlayerName, Color i_PlayerColor, byte i_PlayerID)
        {
            setColor(i_PlayerColor, i_PlayerID);
            SetPlayerNamesAndScore(i_PlayerName, i_PlayerID);
        }

        public bool GetCoordinates(PictureBox i_ClickedPictureBox, out byte o_Row, out byte o_Col)
        {
            bool pictureBoxExist = false;
            o_Row = 0;
            o_Col = 0;

            for (byte row = 0; row < GameBoardCards.GetLength(0); row++)
            {
                for(byte col = 0; col < GameBoardCards.GetLength(1); col++)
                {
                    if (GameBoardCards[row, col] == i_ClickedPictureBox)
                    {
                        pictureBoxExist = true;
                        o_Row = row;
                        o_Col = col;
                        break;
                    }
                }
            }

            return pictureBoxExist;
        }

        private void setColor(Color i_Color, byte i_PlayerID)
        {
            r_Players[i_PlayerID].BackColor = i_Color;
        }

        // =======================================================
        // Delegates and Events methods
        // =======================================================
        protected virtual void GameBoardPictureBox_Click(object i_ClickedButton, EventArgs i_EventArgs)
        {
            PictureBox clickedTile = i_ClickedButton as PictureBox;
            bool isButtomExists = GetCoordinates(clickedTile, out byte o_Row, out byte o_Col);

            if (!isButtomExists)
            {
                throw new FormatException("the button does not exists");
            }

            OnAnyPictureBoxClick(clickedTile, new BoardLocationEventArgs(o_Row, o_Col));
        }

        protected virtual void OnAnyPictureBoxClick(object i_Sender, EventArgs i_EventArgs)
        {
            if (AnyPictureBoxClick != null)
            {
                AnyPictureBoxClick.Invoke(this, (BoardLocationEventArgs)i_EventArgs);
            }
        }
    }
}
