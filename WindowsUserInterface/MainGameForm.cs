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
        private const int k_WindowHeightModifier = 16;
        private const int k_WindowWidthModifier = 3;
        private const bool k_Enabled = true;
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

        public Color DefulltColor
        {
            get
            {
                return Color.LightGray;
            }
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
        public void SetPlayerNamesAndScore(string i_PlayerNameAndScore, byte i_ID)
        {
            m_Players[i_ID].Text = i_PlayerNameAndScore;
        }

        private void setColor(Color i_Color, byte i_ID)
        {
            m_Players[i_ID].BackColor = i_Color;
        }

        // Game Board:
        public void Flipped(ButtomIndexEvent buttomIndexEvent, char v)
        {
            this[buttomIndexEvent].Text = v.ToString();
            this[buttomIndexEvent].Enabled = false;
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

                buttonClick.Enabled = k_Enabled;
                buttonClick.BackColor = DefulltColor;
                buttonClick.Text = string.Empty;
            }
        }

        private void initializeComponents(string i_CurrentPlayer)
        {
            initializeMainForm();
            initilizeGameBoardButtons();
            initializeLabels(i_CurrentPlayer);
            ElementsDesignerTool.FitTheSizeOfForm(this, k_Margin);
            initializeGameOverDialog();
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
                        BackColor = DefulltColor,
                    };
                    GameBoardButtons[i, j].Click += GameBoardTile_Click;
                }
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
            int buttonSizeAndMargin = k_ButtonSize + k_Margin;
            int formHeight = (buttonSizeAndMargin * Rows) + (k_WindowHeightModifier * k_Margin);
            int formWidth = (buttonSizeAndMargin * Columns) + (k_WindowWidthModifier * k_Margin);

            return new Size(formWidth, formHeight);
        }

        private void anyButtemInvoker(EventArgs i_EventArgs)
        {
            if(AnyButtonHandler != null)
            {
                AnyButtonHandler.Invoke(this, (ButtomIndexEvent)i_EventArgs);
            }
        }

        // Names and Score:
        private void initializeLabels(string i_CurrentPlayer)
        {
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
                ElementsDesignerTool.DesignElements(m_Players[i], ePositionBy.Under, temp, k_Margin);
                ElementsDesignerTool.DesignElements(m_Players[i], ePositionBy.Left, temp);
                m_Players[i].AutoSize = true;
                temp = m_Players[i];
                Controls.Add(m_Players[i]);
            }
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
                // TODO: this ??;
               // rematch
            }
        }

        // Game Over Dialog:
        private void initializeGameOverDialog()
        {
            m_GameOverDialog = new MessageBox();
            GameOverDialog.FormClosed += gameOverDialog_FormClosed;
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

        protected virtual void MainGameForm_Load(object sender, EventArgs i_EventArgs)
        {
            // TODO: wht is this ?
        }

        protected virtual void GameBoardTile_Click(object i_ClickedButton, EventArgs i_EventArgs)
        {
            Button clickedTile = i_ClickedButton as Button;
            bool isButtomExists;

            if (isGameOver())
            {
                GameOverDialog.ShowDialog();
            }

            isButtomExists = GetCoordinates(clickedTile, out byte o_Row, out byte o_Col);

            if(!isButtomExists)
            {
                throw new FormatException("the button does not exists");
            }

            AnyButton_Click(clickedTile, new ButtomIndexEvent(o_Row, o_Col));
        }

        protected virtual void AllButtem_Click(object sender, EventArgs i_EventArgs)
        {
        }

        protected virtual void AnyButton_Click(object sender, EventArgs i_EventArgs)
        {
            anyButtemInvoker(i_EventArgs);
        }

    }
}
