using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsUserInterface
{
    public delegate void AnyButtonHandler(object sender);

    public class MainGameForm : Form
    {
        // TODO: move public constants to another class maybe?
        public const int k_Margin = 10;
        public const int k_ButtonSize = 75;

        public event AnyButtonHandler AnyButtonHandler;

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

        private const string k_GameTitle = "Memory Game";
        private const string k_PlayerNameLabel = "{0}: {1} Pair(s)";
        private const string k_CurrentPlayerLabel = "Current Player: {0}";
        private const int k_StartingScore = 0;

        private Label m_CurrentPlayerName;
        private Label m_PlayerOne;
        private Label m_PlayerTwo;
        private Button[,] m_GameBoardButtons;
        private MessageBox m_GameOverDialog;

        // Ctor:
        public MainGameForm(byte i_BoardHeight, byte i_BoardWidth, string i_PlayerOneName, string i_PlayerTwoName, string i_CurrentPlayer)
        {
            initializeComponents(i_BoardHeight, i_BoardWidth, i_PlayerOneName, i_PlayerTwoName, i_CurrentPlayer);
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

        public MessageBox GameOverDialog
        {
            get { return m_GameOverDialog; }
            set { m_GameOverDialog = value; }
        }

        // Initializers:
        private void initializeComponents(byte i_BoardHeight, byte i_BoardWidth, string i_PlayerOneName, string i_PlayerTwoName, string i_CurrentPlayer)
        {
            initializeMainForm(i_BoardHeight, i_BoardWidth);
            initilizeGameBoardButtons(i_BoardHeight, i_BoardWidth);
            initializeLabels(i_PlayerOneName, i_PlayerTwoName, i_CurrentPlayer);
            initializeGameOverDialog();
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

        protected virtual void GameBoardTile_Click(object i_ClickedButton, EventArgs i_EventArgs)
        {
            Button clickedTile = i_ClickedButton as Button;
            clickedTile.BackColor = CurrentPlayerName.BackColor;
            clickedTile.Text = "A";
            if (isGameOver())
            {
                GameOverDialog.ShowDialog();
            }

            AnyButton_Click(this);
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
        private void initializeLabels(string i_PlayerOneName, string i_PlayerTwoName, string i_CurrentPlayer)
        {
            // setup labels:
            PlayerOne = new Label();
            PlayerOne.Text = getPlayerNameAndScore(i_PlayerOneName, k_StartingScore);
            PlayerOne.TextAlign = ContentAlignment.MiddleCenter;
            PlayerOne.BackColor = Color.LightSteelBlue;
            PlayerOne.Left = k_Margin;
            PlayerOne.AutoSize = true;
            PlayerOne.Top = ClientSize.Height - k_Margin - PlayerOne.Height;

            PlayerTwo = new Label
            {
                Text = getPlayerNameAndScore(i_PlayerTwoName, k_StartingScore),
                TextAlign = PlayerOne.TextAlign,
                BackColor = Color.PaleGreen,
                Left = PlayerOne.Left,
                AutoSize = true,
                Top = ClientSize.Height - ((PlayerOne.Height + k_Margin) * 2),
            };

            // setup current player
            CurrentPlayerName = new Label
            {
                TextAlign = PlayerOne.TextAlign,
                BackColor = PlayerOne.BackColor,
                Left = PlayerOne.Left,
                AutoSize = true,
                Top = ClientSize.Height - ((PlayerOne.Height + k_Margin) * 3),
            };

            SetCurrentPlayerName(i_CurrentPlayer);

            // add
            Controls.Add(PlayerOne);
            Controls.Add(PlayerTwo);
            Controls.Add(CurrentPlayerName);
        }

        private string getPlayerNameAndScore(string i_PlayerName, int i_playerScore)
        {
            return string.Format(k_PlayerNameLabel, i_PlayerName, i_playerScore);
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

        public void SetCurrentPlayerName(string i_CurrentPlayer)
        {
            CurrentPlayerName.Text = string.Format(k_CurrentPlayerLabel, i_CurrentPlayer);
        }

        public void UpdatePlayerScore(string i_PlayerName, int i_NewScore)
        {
            if(PlayerOne.Text.ToLower() == i_PlayerName.ToLower())
            {
                PlayerOne.Text = getPlayerNameAndScore(PlayerOne.Text, i_NewScore);
            }
            else
            {
                PlayerTwo.Text = getPlayerNameAndScore(PlayerTwo.Text, i_NewScore);
            }
        }

        public string GetCoordinates(Button i_ClickedButton)
        {
            string buttonCoordinates = string.Empty;

            for(int i = 0; i < GameBoardButtons.GetLength(0); i++)
            {
                for(int j = 0; j < GameBoardButtons.GetLength(i); j++)
                {
                    if (GameBoardButtons[i, j] == i_ClickedButton)
                    {
                        buttonCoordinates = string.Format("{0}{1}", i, j);
                    }
                }
            }

            return buttonCoordinates;
        }

        protected virtual void MainGameForm_Load(object sender)
        {
            // lissner 

        }

        protected virtual void AllButtem_Click(object sender)
        {
            // lissner 
            // 
        }

        protected virtual void AnyButton_Click(object sender)
        {
        }
    }
}
