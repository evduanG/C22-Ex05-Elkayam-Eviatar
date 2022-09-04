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
        private const int k_LabelWidth = 200;
        private const string k_WinnerScoreMessage = "{0} won with {1} points!";
        private const string k_AnotherGameMessage = "Do you want to play another game?";

        public event MessageBoxHandler m_MessageBox; // TODO: rename

        private Label m_GameResultsMessage;
        private Label m_AnotherGameMessage;
        private Button m_Yes;
        private Button m_No;

        // Ctor:
        public MessageBox()
        {
            initializeComponents();
        }

        // Properties:
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

        public Button Yes
        {
            get { return m_Yes; }
            set { m_Yes = value; }
        }

        public Button No
        {
            get { return m_No; }
            set { m_No = value; }
        }

        private void initializeComponents()
        {
            initializeForm();
            initializeLabels();
            initializeButtons();
        }

        private void initializeButtons()
        {
            Yes = new Button();
            Yes.Text = "Yes";
            Yes.Top = AnotherGameMessage.Bottom + (2 * MainGameForm.k_Margin);
            Yes.Left = AnotherGameMessage.Left + (2 * MainGameForm.k_Margin);
            Yes.Click += yes_Click;

            No = new Button();
            No.Text = "No";
            No.Top = Yes.Top;
            No.Left = Yes.Right + (MainGameForm.k_Margin * 2);
            No.Click += no_Click;

            Controls.Add(Yes);
            Controls.Add(No);
        }

        private void no_Click(object i_ButtonClicked, EventArgs i_EventArgs)
        {
            DialogResult = DialogResult.No;
        }

        private void yes_Click(object i_ButtonClicked, EventArgs i_EventArgs)
        {
            DialogResult = DialogResult.Yes;
        }

        private void initializeForm()
        {
           Text = "GAME OVER";
           Size = new Size(k_MessageBoxWidth, k_MessageBoxHeight);
           StartPosition = FormStartPosition.CenterParent;
           FormBorderStyle = FormBorderStyle.FixedDialog;
           MaximizeBox = false;
        }

        private void initializeLabels()
        {
            GameResultsMessage = new Label();
            GameResultsMessage.Text = k_WinnerScoreMessage;
            GameResultsMessage.TextAlign = ContentAlignment.MiddleCenter;
            GameResultsMessage.Width = k_LabelWidth;
            GameResultsMessage.Top = MainGameForm.k_Margin * 2;
            GameResultsMessage.Left = (ClientSize.Width / 2) - (GameResultsMessage.Width / 2);

            AnotherGameMessage = new Label();
            AnotherGameMessage.Text = k_AnotherGameMessage;
            AnotherGameMessage.TextAlign = ContentAlignment.MiddleCenter;
            AnotherGameMessage.Width = k_LabelWidth;
            AnotherGameMessage.Top = GameResultsMessage.Bottom + MainGameForm.k_Margin;
            AnotherGameMessage.Left = GameResultsMessage.Left;

            Controls.Add(m_GameResultsMessage);
            Controls.Add(m_AnotherGameMessage);
        }
    }
}
