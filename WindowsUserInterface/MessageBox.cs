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
        private const string k_FormtWinnerScoreMessage = "{0} won with {1} points!";
        private const string k_FormtAnotherGameMessage = "Do you want to play another game?";

        public event MessageBoxHandler Closed;

        private Label m_GameResultsMessage;
        private Label m_AnotherGameMessage;
        private Button m_ButtonYes;
        private Button m_ButtonNo;

        // =======================================================
        // constructor  and methods for the constructor
        // =======================================================
        public MessageBox()
        {
            initializeComponents();
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

        private void initializeLabels()
        {
            GameResultsMessage = new Label();
            GameResultsMessage.Text = k_FormtWinnerScoreMessage;
            GameResultsMessage.TextAlign = ContentAlignment.MiddleCenter;
            GameResultsMessage.BackColor = Color.LightGoldenrodYellow;
            GameResultsMessage.AutoSize = true;
            GameResultsMessage.Top = MainGameForm.k_Margin * 2;
            GameResultsMessage.Left = (ClientSize.Width / 2) - (GameResultsMessage.Width / 2);

            AnotherGameMessage = new Label();
            AnotherGameMessage.Text = k_FormtAnotherGameMessage;
            AnotherGameMessage.TextAlign = ContentAlignment.MiddleCenter;
            AnotherGameMessage.BackColor = Color.LightGoldenrodYellow;
            AnotherGameMessage.AutoSize = true;
            AnotherGameMessage.Top = GameResultsMessage.Bottom + MainGameForm.k_Margin;
            AnotherGameMessage.Left = GameResultsMessage.Left + (GameResultsMessage.Width / 2) - AnotherGameMessage.Width;

            Controls.Add(m_GameResultsMessage);
            Controls.Add(m_AnotherGameMessage);
        }

        private void initializeComponents()
        {
            initializeForm();
            initializeLabels();
            initializeButtons();
        }

        private void initializeButtons()
        {
            ButtonYes = new Button();
            ButtonYes.Text = "Yes";
            ButtonYes.Top = AnotherGameMessage.Bottom + (2 * MainGameForm.k_Margin);
            ButtonYes.Left = AnotherGameMessage.Left + (2 * MainGameForm.k_Margin);
            ButtonYes.Click += Yes_Click;

            ButtonNo = new Button();
            ButtonNo.Text = "No";
            ButtonNo.Top = ButtonYes.Top;
            ButtonNo.Left = ButtonYes.Right + (MainGameForm.k_Margin * 2);
            ButtonNo.Click += No_Click;

            Controls.Add(ButtonYes);
            Controls.Add(ButtonNo);
        }

        // =======================================================
        // Propertys
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