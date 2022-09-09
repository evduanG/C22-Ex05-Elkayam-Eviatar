using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace WindowsUserInterface
{
    public delegate void AnyButtonHandler(object sender, ButtomIndexEvent e);

    public class MainGameForm : Form
    {
        private static readonly List<char> sr_ABC = new List<char>()
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

        public const int k_Margin = 10;
        public const int k_ButtonSize = 75;
        private const int k_WindowHeightModifier = 16;
        private const int k_WindowWidthModifier = 3;
        private const bool k_Enabled = true;
        private const string k_GameTitle = "Memory Game";
        private const string k_PlayerNameLabel = "{0}: {1} Pair(s)";
        private const string k_CurrentPlayerLabel = "Current Player: {0}";
        private readonly byte r_NumOfRows;
        private readonly byte r_NumOfCols;

        public event AnyButtonHandler AnyButtonClick;

        private readonly Label[] r_Players;
        private Label m_CurrentPlayerName;
        private Button[,] m_GameBoardButtons;

        // =======================================================
        // constructor  and methods for the constructor
        // =======================================================
        public MainGameForm(byte i_BoardHeight, byte i_BoardWidth, byte i_numOfPlayers)
        {
            r_NumOfCols = i_BoardHeight;
            r_NumOfRows = i_BoardWidth;
            r_Players = new Label[i_numOfPlayers];
            initializeComponents();
        }

        // Initializers:
        private void initializeComponents()
        {
            initializeMainForm();
            initilizeGameBoardButtons();
            initializeLabels();
            ElementsDesignerTool.FitTheSizeOfForm(this, k_Margin);
        }

        private void initilizeGameBoardButtons()
        {
            m_GameBoardButtons = new Button[Rows, Columns];
            createButtons();
            positionButtonsOnGrid();
        }

        private void initializeMainForm()
        {
            Text = k_GameTitle;
            Size = getWindowSize();
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
        }

        private void positionButtonsOnGrid()
        {
            // setup the rest
            int upOneSlot = 0;

            for (int i = 0; i < Rows; i++)
            {
                bool isTopLeft = i == 0;

                Control controlToPlace;
                if (isTopLeft)
                {
                    GameBoardButtons[0, 0].Top = k_Margin;
                    GameBoardButtons[0, 0].Left = k_Margin;
                    Controls.Add(GameBoardButtons[0, 0]);
                }
                else
                {
                    upOneSlot = i - 1;
                    controlToPlace = GameBoardButtons[i, 0];
                    ElementsDesignerTool.DesignElements(controlToPlace, ePositionBy.Left, GameBoardButtons[upOneSlot, 0]);
                    ElementsDesignerTool.DesignElements(controlToPlace, ePositionBy.Under, GameBoardButtons[upOneSlot, 0], k_Margin);
                    Controls.Add(GameBoardButtons[i, 0]);
                }

                for (int j = 1; j < Columns; j++)
                {
                    controlToPlace = GameBoardButtons[i, j];
                    ePositionBy topPositionBy = i == 0 ? ePositionBy.Top : ePositionBy.Under;
                    ElementsDesignerTool.DesignElements(controlToPlace, ePositionBy.NextToTheLeft, GameBoardButtons[i, j - 1], k_Margin);
                    ElementsDesignerTool.DesignElements(controlToPlace, topPositionBy, GameBoardButtons[upOneSlot, j], k_Margin);
                    Controls.Add(GameBoardButtons[i, j]);
                }
            }
        }

        private void initializeLabels()
        {
            // setup current player
            CurrentPlayerName = new Label();
            Button lastButton = GameBoardButtons[GameBoardButtons.GetLength(0) - 1, 0];
            ElementsDesignerTool.DesignElements(CurrentPlayerName, ePositionBy.Under, lastButton, k_Margin);
            ElementsDesignerTool.DesignElements(CurrentPlayerName, ePositionBy.Left, lastButton);
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

        private void createButtons()
        {
            // create buttons
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    GameBoardButtons[i, j] = new Button
                    {
                        Size = new Size(k_ButtonSize, k_ButtonSize),
                        BackColor = DefulltColor,
                    };
                    GameBoardButtons[i, j].Click += GameBoardTile_Click;
                }
            }
        }

        // =======================================================
        // Propertys
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

        public Button[,] GameBoardButtons
        {
            get { return m_GameBoardButtons; }
        }

        public Label CurrentPlayerName
        {
            get { return m_CurrentPlayerName; }
            set { m_CurrentPlayerName = value; }
        }

        public Color DefulltColor
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

        // Indexer:
        public Button this[byte i_Row, byte i_Col]
        {
            get
            {
                return m_GameBoardButtons[i_Row, i_Col];
            }

            set
            {
                m_GameBoardButtons[i_Row, i_Col] = value;
            }
        }

        public Button this[BoardLocation indexEvent]
        {
            get
            {
                return this[indexEvent.Row, indexEvent.Col];
            }

            set
            {
                this[indexEvent.Row, indexEvent.Col] = value;
            }
        }

        private Size getWindowSize()
        {
            int buttonSizeAndMargin = k_ButtonSize + k_Margin;
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

        public void Flipped(BoardLocation buttomIndexEvent, char v)
        {
            this[buttomIndexEvent].Text = v.ToString();
            this[buttomIndexEvent].Enabled = false;
        }

        public void ColorPair(List<BoardLocation> m_PlayerChoice, Color color)
        {
            foreach (BoardLocation choice in m_PlayerChoice)
            {
                GameBoardButtons[choice.Row, choice.Col].BackColor = color;
            }
        }

        public void FlippCardsToFaceDown(List<BoardLocation> m_PlayerChoice)
        {
            foreach (BoardLocation choice in m_PlayerChoice)
            {
                Button buttonClick = GameBoardButtons[choice.Row, choice.Col];

                buttonClick.Enabled = k_Enabled;
                buttonClick.BackColor = DefulltColor;
                buttonClick.Text = string.Empty;
            }
        }

        public void SetCurrentPlayer(string i_PlayerName, Color i_PlayerColor)
        {
            CurrentPlayerName.Text = string.Format(k_CurrentPlayerLabel, i_PlayerName);
            CurrentPlayerName.BackColor = i_PlayerColor;
        }

        public void SetPlayer(string i_PlayerName, Color i_PlayerColor, byte i_ID)
        {
            setColor(i_PlayerColor, i_ID);
            SetPlayerNamesAndScore(i_PlayerName, i_ID);
        }

        public bool GetCoordinates(Button i_ClickedButton, out byte o_Row, out byte o_Col)
        {
            bool buttonExist = false;
            o_Row = 0;
            o_Col = 0;

            for (byte row = 0; row < GameBoardButtons.GetLength(0); row++)
            {
                for(byte col = 0; col < GameBoardButtons.GetLength(1); col++)
                {
                    if (GameBoardButtons[row, col] == i_ClickedButton)
                    {
                        buttonExist = true;
                        o_Row = row;
                        o_Col = col;
                        break;
                    }
                }
            }

            return buttonExist;
        }

        private void setColor(Color i_Color, byte i_ID)
        {
            r_Players[i_ID].BackColor = i_Color;
        }

        // =======================================================
        // Delegates and Events methods
        // =======================================================
        protected virtual void MainGameForm_Load(object sender, EventArgs i_EventArgs)
        {
            // TODO: wht is this ?
        }

        private void anyButtemInvoker(EventArgs i_EventArgs)
        {
            if(AnyButtonClick != null)
            {
                AnyButtonClick.Invoke(this, (ButtomIndexEvent)i_EventArgs);
            }
        }

        protected virtual void GameBoardTile_Click(object i_ClickedButton, EventArgs i_EventArgs)
        {
            Button clickedTile = i_ClickedButton as Button;
            bool isButtomExists;
            isButtomExists = GetCoordinates(clickedTile, out byte o_Row, out byte o_Col);

            if(!isButtomExists)
            {
                throw new FormatException("the button does not exists");
            }

            OnAnyButtonClick(clickedTile, new ButtomIndexEvent(o_Row, o_Col));
        }

        protected virtual void AllButtem_Click(object sender, EventArgs i_EventArgs)
        {
        }

        protected virtual void OnAnyButtonClick(object sender, EventArgs i_EventArgs)
        {
            anyButtemInvoker(i_EventArgs);
        }
    }
}
