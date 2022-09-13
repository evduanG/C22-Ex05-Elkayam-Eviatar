using System.Windows.Forms;
using System.Drawing;
using System;

namespace WindowsUserInterface
{
    public delegate void MessageBoxHandler(object sender, MouseEventArgs e);

    public class MessageBox : Form
    {
        private const int k_MessageBoxHeight = 180;
        private const int k_MessageBoxWidth = 360;
        private const string k_FormtWinnerScoreMessage = "{0} won with {1} points!";
        private const string k_FormtTieScore = "It's a tie with {0} points!";
        private const string k_FormtAnotherGameMessage = "Do you want to play another game?";

        // public event MessageBoxHandler Closed;
        private Label m_GameResultsMessage;
        private Label m_AnotherGameMessage;
        private Button m_ButtonYes;
        private Button m_ButtonNo;

        // =======================================================
        // constructor  and methods for the constructor
        // =======================================================
        private MessageBox(string i_StrToShow)
        {
            initializeComponents(i_StrToShow);
            ElementsDesignerTool.FitTheSizeOfForm(this, MainGameForm.k_Margin);
        }

        public static MessageBox MessageBoxTie(string i_TieScore)
        {
            return new MessageBox(string.Format(k_FormtTieScore, i_TieScore));
        }

        public static MessageBox MessageBoxWinner(string i_WinnerName, byte i_Score)
        {
            return new MessageBox(string.Format(k_FormtWinnerScoreMessage, i_WinnerName, i_Score));
        }

        private void initializeForm()
        {
            Text = "GAME OVER";
            Size = new Size(k_MessageBoxWidth, k_MessageBoxHeight);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            ShowInTaskbar = false;
            AcceptButton = ButtonYes;
        }

        private void initializeLabels(string i_StrToShow)
        {
            GameResultsMessage = new Label
            {
                Text = i_StrToShow,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightGoldenrodYellow,
                AutoSize = true,
                Top = MainGameForm.k_Margin * 2,
            };
            GameResultsMessage.Left = (ClientSize.Width / 2) - (GameResultsMessage.Width / 2);

            AnotherGameMessage = new Label
            {
                Text = k_FormtAnotherGameMessage,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightGoldenrodYellow,
                AutoSize = true,
                Top = GameResultsMessage.Bottom + MainGameForm.k_Margin,
            };
            AnotherGameMessage.Left = GameResultsMessage.Left + (GameResultsMessage.Width / 2) - AnotherGameMessage.Width;

            // ElementsDesignerTool(AnotherGameMessage, GameResultsMessage);
            Controls.Add(m_GameResultsMessage);
            Controls.Add(m_AnotherGameMessage);
        }

        private void initializeComponents(string i_StrToShow)
        {
            initializeForm();
            initializeLabels(i_StrToShow);
            initializeButtons();
        }

        private void initializeButtons()
        {
            ButtonYes = new Button
            {
                Text = "Yes",
                Top = AnotherGameMessage.Bottom + (2 * MainGameForm.k_Margin),
                Left = AnotherGameMessage.Left + (2 * MainGameForm.k_Margin),
            };
            ButtonYes.Click += Yes_Click;

            ButtonNo = new Button
            {
                Text = "No",
                Top = ButtonYes.Top,
                Left = ButtonYes.Right + (MainGameForm.k_Margin * 2),
            };
            ButtonNo.Click += No_Click;

            Controls.Add(ButtonYes);
            Controls.Add(ButtonNo);
        }

        // =======================================================
        // Properties
        // =======================================================
        public Label GameResultsMessage
        {
            get { return m_GameResultsMessage; }
            set { m_GameResultsMessage = value; }
        }

        public Label AnotherGameMessage
        {
            get { return m_AnotherGameMessage; }
            set { m_AnotherGameMessage = value; }
        }

        public Button ButtonYes
        {
            get { return m_ButtonYes; }
            set { m_ButtonYes = value; }
        }

        public Button ButtonNo
        {
            get { return m_ButtonNo; }
            set { m_ButtonNo = value; }
        }

        // =======================================================
        // Delegates and Events methods
        // =======================================================
        protected virtual void No_Click(object i_ButtonClicked, EventArgs i_EventArgs)
        {
            DialogResult = DialogResult.No;
        }

        protected virtual void Yes_Click(object i_ButtonClicked, EventArgs i_EventArgs)
        {
            DialogResult = DialogResult.Yes;
        }
    }
}