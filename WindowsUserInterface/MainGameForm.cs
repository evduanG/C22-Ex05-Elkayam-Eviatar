using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace WindowsUserInterface
{
    public delegate void AnyButtonHandler(object sender, ButtomIndexEvent e);

    public class MainGameForm : Form
    {
        public const int k_Margin = 10;
        public const int k_ButtonSize = 75;
        private const string k_GameTitle = "Memory Game";
        private const string k_PlayerNameLabel = "{0}: {1} Pair(s)";
        private const string k_CurrentPlayerLabel = "Current Player: {0}";
        private const int k_StartingScore = 0;
        private readonly byte r_NumOfRows;
        private readonly byte r_NumOfCols;

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

        public event AnyButtonHandler AnyButtonHandler;

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

        private Label m_CurrentPlayerName;
        private Label[] m_Players;
        // private Label m_PlayerOne;
        // private Label m_PlayerTwo;
        private Button[,] m_GameBoardButtons;
        private MessageBox m_GameOverDialog;

        // Ctor:
        public MainGameForm(byte i_BoardHeight, byte i_BoardWidth, byte i_numOfPlayers, string i_CurrentPlayer)
        {
            r_NumOfCols = i_BoardHeight;
            r_NumOfRows = i_BoardWidth;
            m_Players = new Label[i_numOfPlayers];
            initializeComponents(i_CurrentPlayer);
        }

        // Properties:
        public Button[,] GameBoardButtons
        {
            get { return m_GameBoardButtons; }
        }

        public Label CurrentPlayerName
        {
            get { return m_CurrentPlayerName; }
            set { m_CurrentPlayerName = value; }
        }

        public Label[] Players
        {
            get { return m_Players; }
        }

        public MessageBox GameOverDialog
        {
            get { return m_GameOverDialog; }
            set { m_GameOverDialog = value; }
        }

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

        public Button this[ButtomIndexEvent indexEvent]
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

        // Initializers:
        private void initializeComponents(string i_CurrentPlayer)
        {
            initializeMainForm();
            initilizeGameBoardButtons();
            initializeLabels(i_CurrentPlayer);
            initializeGameOverDialog();
        }

        public void SetPlayerNamesAndScore(string i_PlayerNameAndScore, byte i_ID)
        {
            m_Players[i_ID].Text = i_PlayerNameAndScore;
        }

        private void setColor(Color i_Color, byte i_ID)
        {
            m_Players[i_ID].BackColor = i_Color;
        }

        // Main Form:
        private void initializeMainForm()
        {
            Text = k_GameTitle;
            Size = getWindowSize();
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
        }

        private Size getWindowSize()
        {
            // TODO: 16 and 3 to const
            int buttonSizeAndMargin = k_ButtonSize + k_Margin;
            int formHeight = (buttonSizeAndMargin * Rows) + (16 * k_Margin); // what is 16 ??
            int formWidth = (buttonSizeAndMargin * Columns) + (3 * k_Margin); // what is 3 ??

            return new Size(formWidth, formHeight);
        }

        // Game Board:
        private void initilizeGameBoardButtons()
        {
            m_GameBoardButtons = new Button[Rows, Columns];
            createButtons();
            positionButtonsOnGrid();
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
                        BackColor = Color.LightGray,
                    };
                    GameBoardButtons[i, j].Click += GameBoardTile_Click;
                }
            }
        }

        public void Flipped(ButtomIndexEvent buttomIndexEvent, char v)
        {
            this[buttomIndexEvent].Text = v.ToString();
            this[buttomIndexEvent].Enabled = false;
        }

        private void positionButtonsOnGrid()
        {
            // setup top-left button
            GameBoardButtons[0, 0].Top = k_Margin;
            GameBoardButtons[0, 0].Left = k_Margin;
            Controls.Add(GameBoardButtons[0, 0]);

            // TODO : use the ElementsDesignerTool 
            // setup the rest
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    // top-left button
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    // left column
                    if (j == 0)
                    {
                        GameBoardButtons[i, j].Left = k_Margin;
                        GameBoardButtons[i, j].Top = k_Margin + ((k_ButtonSize + k_Margin) * i);
                    }

                    // right block
                    else
                    {
                        // top row
                        if (i == 0)
                        {
                            GameBoardButtons[i, j].Top = k_Margin;
                            GameBoardButtons[i, j].Left = k_Margin + ((k_ButtonSize + k_Margin) * j);
                        }

                        // inner right block
                        else
                        {
                            GameBoardButtons[i, j].Top = GameBoardButtons[i, j - 1].Top;
                            GameBoardButtons[i, j].Left = GameBoardButtons[i - 1, j].Left;

                            // ElementsDesignerTool.DesignElements(ePositionBy.Top, GameBoardButtons[row, col - 1], GameBoardButtons[row, col], k_Margin);
                            // ElementsDesignerTool.DesignElements(ePositionBy.Left, GameBoardButtons[row - 1, col], GameBoardButtons[row, col], k_Margin);
                        }
                    }

                    Controls.Add(GameBoardButtons[i, j]);
                }
            }
        }

        public void ColorPair(List<ButtomIndexEvent> m_PlayerChoice, Color color)
        {
            foreach (ButtomIndexEvent choice in m_PlayerChoice)
            {
                GameBoardButtons[choice.Row, choice.Col].BackColor = color;
            }
        }

        public void FlippCardsToFaceDown(List<ButtomIndexEvent> m_PlayerChoice)
        {
            foreach (ButtomIndexEvent choice in m_PlayerChoice)
            {
                Button buttonClick = GameBoardButtons[choice.Row, choice.Col];

                // TODO : make the Enabled to be const
                // TODO : make the Color to be statir redonly 
                buttonClick.Enabled = true;
                buttonClick.BackColor = Color.LightGray;
                buttonClick.Text = " ";
            }
        }

        protected virtual void GameBoardTile_Click(object i_ClickedButton, EventArgs i_EventArgs)
        {
            Button clickedTile = i_ClickedButton as Button;

            if (isGameOver())
            {
                GameOverDialog.ShowDialog();
            }

            bool isButtomExists = GetCoordinates(clickedTile, out byte o_Row, out byte o_Col);
            if(!isButtomExists)
            {
                throw new FormatException("the Buttom is not  Exists");
            }

            AnyButton_Click(clickedTile, new ButtomIndexEvent(o_Row, o_Col));
        }

        private bool isGameOver()
        {
            bool isComplete = true;

            foreach (Button button in GameBoardButtons)
            {
                if (button.BackColor != CurrentPlayerName.BackColor)
                {
                    isComplete = false;
                }
            }

            return isComplete;
        }

        // Names and Score:
        private void initializeLabels(string i_CurrentPlayer)
        {
            //// setup labels:
            //PlayerOne = new Label();
            //PlayerOne.Text = getPlayerNameAndScore(i_PlayerOneName, k_StartingScore);
            //PlayerOne.TextAlign = ContentAlignment.MiddleCenter;
            //PlayerOne.BackColor = Color.LightSteelBlue;
            //PlayerOne.Left = k_Margin;
            //PlayerOne.AutoSize = true;
            //PlayerOne.Top = ClientSize.Height - k_Margin - PlayerOne.Height;

            //PlayerTwo = new Label
            //{
            //    Text = getPlayerNameAndScore(i_PlayerTwoName, k_StartingScore),
            //    TextAlign = PlayerOne.TextAlign,
            //    BackColor = Color.PaleGreen,
            //    Left = PlayerOne.Left,
            //    AutoSize = true,
            //    Top = ClientSize.Height - ((PlayerOne.Height + k_Margin) * 2),
            //};

            // setup current player
            CurrentPlayerName = new Label();
            Button lastButton = GameBoardButtons[GameBoardButtons.GetLength(0) - 1, 0];
            ElementsDesignerTool.DesignElements(CurrentPlayerName, ePositionBy.Under, lastButton, k_Margin);
            ElementsDesignerTool.DesignElements(CurrentPlayerName, ePositionBy.Left, lastButton);
            CurrentPlayerName.AutoSize = true;
            Controls.Add(CurrentPlayerName);

            Label temp = CurrentPlayerName;

            for(int i = 0; i < m_Players.Length; i++)
            {
                m_Players[i] = new Label();
                ElementsDesignerTool.DesignElements(m_Players[i], ePositionBy.Under, temp);
                ElementsDesignerTool.DesignElements(m_Players[i], ePositionBy.Left, temp);
                m_Players[i].AutoSize = true;
                temp = m_Players[i];
                Controls.Add(m_Players[i]);
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

        // Game Over Dialog:
        private void initializeGameOverDialog()
        {
            m_GameOverDialog = new MessageBox();
            GameOverDialog.FormClosed += gameOverDialog_FormClosed;
        }

        private void gameOverDialog_FormClosed(object i_ClosedForm, FormClosedEventArgs i_EventArgs)
        {
            DialogResult userChoice = ((MessageBox)i_ClosedForm).DialogResult;
            if(userChoice == DialogResult.No)
            {
                DialogResult = DialogResult.Cancel;
            }
            else
            {
               // rematch
            }
        }

        public bool GetCoordinates(Button i_ClickedButton, out byte o_Row, out byte o_Col)
        {
            bool buttonExit = false;
            o_Row = 0;
            o_Col = 0;

            for (byte row = 0; row < GameBoardButtons.GetLength(0); row++)
            {
                for(byte col = 0; col < GameBoardButtons.GetLength(1); col++)
                {
                    if (GameBoardButtons[row, col] == i_ClickedButton)
                    {
                        buttonExit = true;
                        o_Row = row;
                        o_Col = col;
                        break;
                    }
                }
            }

            return buttonExit;
        }

        protected virtual void MainGameForm_Load(object sender, EventArgs i_EventArgs)
        {
        }

        protected virtual void AllButtem_Click(object sender, EventArgs i_EventArgs)
        {
        }

        protected virtual void AnyButton_Click(object sender, EventArgs i_EventArgs)
        {
            anyButtemInvoker(i_EventArgs);
        }

        private void anyButtemInvoker(EventArgs i_EventArgs)
        {
            if(AnyButtonHandler != null)
            {
                AnyButtonHandler.Invoke(this, (ButtomIndexEvent)i_EventArgs);
            }
        }
    }
}
