using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsUserInterface
{
    internal class MainGameForm : Form
    {
        private const int k_Margin = 10;
        private const int k_ButtonSize = 50;
        private const string k_GameTitle = "Memory Game";
        private const string k_PlayerNameLabel = "{0}: {1} Pair(s)";
        private const string k_CurrentPlayerLabel = "Current Player: {0}";

        private Label m_CurrentPlayerName;
        private Label m_PlayerOne;
        private Label m_PlayerTwo;
        private Button[,] m_GameBoardButtons;

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

        public Label PlayerOne
        {
            get { return m_PlayerOne; }
            set { m_PlayerOne = value; }
        }

        public Label PlayerTwo
        {
            get { return m_PlayerTwo; }
            set { m_PlayerTwo = value; }
        }

        // ctor:
        public MainGameForm(byte i_BoardHeight, byte i_BoardWidth, string i_PlayerOneName, string i_PlayerTwoName)
        {
            initializeComponents(i_BoardHeight, i_BoardWidth, i_PlayerOneName, i_PlayerTwoName);
        }

        private void initializeComponents(byte i_BoardHeight, byte i_BoardWidth, string i_PlayerOneName, string i_PlayerTwoName)
        {
            initializeMainForm(i_BoardHeight, i_BoardWidth);
            initilizeGameBoardButtons(i_BoardHeight, i_BoardWidth);
            initializeLabels(i_PlayerOneName, i_PlayerTwoName);
        }

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

        private void initilizeGameBoardButtons(byte i_BoardHeight, byte i_BoardWidth)
        {
            m_GameBoardButtons = new Button[i_BoardHeight, i_BoardWidth];

            for(int i = 0; i < i_BoardHeight; i++)
            {
                for(int j = 0; j < i_BoardWidth; j++)
                {
                    GameBoardButtons[i, j] = new Button
                    {
                        Size = new Size(k_ButtonSize, k_ButtonSize),
                        BackColor = Color.LightGray,
                    };
                    GameBoardButtons[i, j].Click += gameBoardTile_Click;
                }
            }

            // setup top-left button
            GameBoardButtons[0, 0].Top = k_Margin;
            GameBoardButtons[0, 0].Left = k_Margin;
            this.Controls.Add(GameBoardButtons[0, 0]);

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

                    this.Controls.Add(GameBoardButtons[i, j]);
                }
            }
        }

        private void gameBoardTile_Click(object sender, EventArgs e)
        {
            Button clickedTile = sender as Button;
            clickedTile.BackColor = CurrentPlayerName.BackColor;
        }

        private void initializeLabels(string i_PlayerOneName, string i_PlayerTwoName)
        {
            // setup labels:
            PlayerOne = new Label()
            {
                Text = string.Format(k_PlayerNameLabel, i_PlayerOneName, 0),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightSteelBlue,
                Left = k_Margin,
                AutoSize = true,
            };
            PlayerOne.Top = ClientSize.Height - k_Margin - PlayerOne.Height;

            PlayerTwo = new Label()
            {
                Text = string.Format(k_PlayerNameLabel, i_PlayerTwoName, 0),
                TextAlign = PlayerOne.TextAlign,
                BackColor = Color.PaleGreen,
                Left = PlayerOne.Left,
                AutoSize = true,
                Top = ClientSize.Height - ((PlayerOne.Height + k_Margin) * 2),
            };

            // setup current player
            CurrentPlayerName = new Label()
            {
                TextAlign = PlayerOne.TextAlign,
                BackColor = PlayerOne.BackColor,
                Left = PlayerOne.Left,
                AutoSize = true,
                Top = ClientSize.Height - ((PlayerOne.Height + k_Margin) * 3),
            };

            SetCurrentPlayerName(i_PlayerOneName);

            // add
            this.Controls.Add(PlayerOne);
            this.Controls.Add(PlayerTwo);
            this.Controls.Add(CurrentPlayerName);
        }

        public void SetCurrentPlayerName(string i_CurrentPlayer)
        {
            CurrentPlayerName.Text = string.Format(k_CurrentPlayerLabel, i_CurrentPlayer);
        }
    }
}
