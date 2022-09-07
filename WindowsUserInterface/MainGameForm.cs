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
            m_Players = new Label[i_numOfPlayers];
            initializeComponents(i_BoardHeight, i_BoardWidth, i_CurrentPlayer);
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

        // Initializers:
        private void initializeComponents(byte i_BoardHeight, byte i_BoardWidth, string i_CurrentPlayer)
        {
            initializeMainForm(i_BoardHeight, i_BoardWidth);
            initilizeGameBoardButtons(i_BoardHeight, i_BoardWidth);
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
        private void initializeMainForm(byte i_BoardHeight, byte i_BoardWidth)
        {
            Text = k_GameTitle;
            Size = getWindowSize(i_BoardHeight, i_BoardWidth);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
        }

        private Size getWindowSize(byte i_BoardHeight, byte i_BoardWidth)
        {
            int formHeight = ((k_ButtonSize + k_Margin) * i_BoardHeight) + (16 * k_Margin);
            int formWidth = ((k_ButtonSize + k_Margin) * i_BoardWidth) + (3 * k_Margin);

            return new Size(formWidth, formHeight);
        }

        // Game Board:
        private void initilizeGameBoardButtons(byte i_BoardHeight, byte i_BoardWidth)
        {
            m_GameBoardButtons = new Button[i_BoardHeight, i_BoardWidth];
            createButtons(i_BoardHeight, i_BoardWidth);
            positionButtonsOnGrid(i_BoardHeight, i_BoardWidth);
        }

        private void createButtons(byte i_BoardHeight, byte i_BoardWidth)
        {
            // create buttons
            for (int i = 0; i < i_BoardHeight; i++)
            {
                for (int j = 0; j < i_BoardWidth; j++)
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

        private void positionButtonsOnGrid(byte i_BoardHeight, byte i_BoardWidth)
        {
            // setup top-left button
            GameBoardButtons[0, 0].Top = k_Margin;
            GameBoardButtons[0, 0].Left = k_Margin;
            Controls.Add(GameBoardButtons[0, 0]);

            // setup the rest
            for (int i = 0; i < i_BoardHeight; i++)
            {
                for (int j = 0; j < i_BoardWidth; j++)
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

                            // ElementsDesignerTool.DesignElements(ePositionBy.Top, GameBoardButtons[i, j - 1], GameBoardButtons[i, j], k_Margin);
                            // ElementsDesignerTool.DesignElements(ePositionBy.Left, GameBoardButtons[i - 1, j], GameBoardButtons[i, j], k_Margin);
                        }
                    }

                    Controls.Add(GameBoardButtons[i, j]);
                }
            }
        }

        public void ColorAndEnablePair(List<string> m_PlayerChoice, Color color)
        {
            foreach (string choice in m_PlayerChoice)
            {
                byte col = (byte)sr_ABC.IndexOf(choice[0]);
                byte row = byte.Parse(choice.Substring(2, 1));
                GameBoardButtons[col, row].BackColor = color;
            }
        }

        protected virtual void GameBoardTile_Click(object i_ClickedButton, EventArgs i_EventArgs)
        {
            Button clickedTile = i_ClickedButton as Button;
            if (isGameOver())
            {
                GameOverDialog.ShowDialog();
            }

            AnyButton_Click(clickedTile, ButtomIndexEvent.Parse(GetCoordinates(clickedTile)));
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


        public string GetCoordinates(Button i_ClickedButton)
        {
            string buttonCoordinates = string.Empty;

            for(int i = 0; i < GameBoardButtons.GetLength(0); i++)
            {
                for(int j = 0; j < GameBoardButtons.GetLength(1); j++)
                {
                    if (GameBoardButtons[i, j] == i_ClickedButton)
                    {
                        buttonCoordinates = string.Format("{0} {1}", j, i);
                    }
                }
            }

            return buttonCoordinates;
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
