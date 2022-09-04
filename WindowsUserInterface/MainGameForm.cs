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
            Text = k_GameTitle;
            Size = new Size(i_BoardWidth * 80, i_BoardHeight * 80);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            // MaximizeBox = false;
            // MinimizeBox = false;

            initilizeGameBoardButtons(i_BoardHeight, i_BoardWidth);
            initializeLabels(i_PlayerOneName, i_PlayerTwoName);
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
                        BackColor = Color.Gray,
                    };
                }
            }

            // setup top-left button
            GameBoardButtons[0, 0].Top = k_Margin;
            GameBoardButtons[0, 0].Left = k_Margin;

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
                            GameBoardButtons[i, j].Left = GameBoardButtons[i - 1, j - 1].Top;

                            // ElementsDesignerTool.DesignElements(ePositionBy.Top, GameBoardButtons[i, j - 1], GameBoardButtons[i, j], k_Margin);
                            // ElementsDesignerTool.DesignElements(ePositionBy.Left, GameBoardButtons[i - 1, j], GameBoardButtons[i, j], k_Margin);
                        }
                    }

                    this.Controls.Add(GameBoardButtons[i, j]);
                }
            }
        }

        private void initializeLabels(string i_PlayerOneName, string i_PlayerTwoName)
        {
            // setup labels:
            PlayerOne = new Label()
            {
                Text = i_PlayerOneName,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.SeaGreen,
                Left = k_Margin,
            };
            PlayerOne.Top = ClientSize.Height - k_Margin - PlayerOne.Height;

            PlayerTwo = new Label()
            {
                Text = i_PlayerTwoName,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.PaleVioletRed,
                Left = PlayerOne.Left,
                Top = ClientSize.Height - (PlayerOne.Height * 2) - k_Margin,
            };

            // add
            this.Controls.Add(PlayerOne);
            this.Controls.Add(PlayerTwo);

            // setup current player
            CurrentPlayerName = new Label()
            {
                BackColor = Color.LightSkyBlue,
                Left = PlayerOne.Left,
                Top = ClientSize.Height - k_Margin - ((PlayerOne.Height + k_Margin) * 3),
            };

            SetCurrentPlayerName(i_PlayerOneName);
        }

        public void SetCurrentPlayerName(string i_PlayerOneName)
        {
            CurrentPlayerName.Text = i_PlayerOneName;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // MainGameForm
            this.ClientSize = new System.Drawing.Size(282, 253);
            this.Name = "MainGameForm";
            this.Load += new System.EventHandler(this.MainGameForm_Load);
            this.ResumeLayout(false);

        }

        private void MainGameForm_Load(object sender, EventArgs e)
        {

        }
    }
}
