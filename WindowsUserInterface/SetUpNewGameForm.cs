using System.Windows.Forms;
using System.Drawing;
using System;

namespace WindowsUserInterface
{
    internal class SetUpNewGameForm : Form
    {
        private const string k_TitleForm = "Memory Game - Settung";
        private const string k_TitleLabelFirstplayer = "First Plyer Name:";
        private const string k_TitleLabelSecondPlayer = "Second Plyer Name:";
        private const string k_TitleLabelBordSize = "Bord Size:";
        private const string k_TitleButtonAgainstAFriend = "Against a Friend";
        private const string k_TitleButtonStart = "Start!";
        private const string k_TitleDefultSecondPlayer = "-computer-";
        private const int k_Margin = 12;
        private const bool v_Show = true;
        private const bool v_Enable = true;

        private Label m_LabelSecondPlayer;
        private Label m_LabelFirstPlayer;
        private Label m_LabelBordSize;
        private TextBox m_TextBoxFirstPlayer;
        private TextBox m_TextBoxSecondPlayer;
        private ScrollBar m_ScrollBordSize;
        private Button m_ButtonAgainstAFriend;
        private Button m_ButtonStart;

        private SetUpNewGameForm()
        {
            Size = new Size(400, 400);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            initiationForm();
        }

        private void initiationForm()
        {
            // Label-First-Plyer
            m_LabelFirstPlayer = new Label();
            m_LabelFirstPlayer.Text = k_TitleLabelFirstplayer;
            m_LabelFirstPlayer.Left = k_Margin;
            m_LabelFirstPlayer.Top = k_Margin;
            this.Controls.Add(m_LabelFirstPlayer);

            // TextBox-First-player
            m_TextBoxFirstPlayer = new TextBox();
            ElementsDesignerTool.DesignElements(m_LabelFirstPlayer, ePositionBy.Right, m_TextBoxFirstPlayer, k_Margin);
            ElementsDesignerTool.DesignElements(m_LabelFirstPlayer, ePositionBy.HorizontalCentre, m_TextBoxFirstPlayer);
            this.Controls.Add(m_TextBoxFirstPlayer);

            // Label-Second-Plyer
            m_LabelSecondPlayer = new Label();
            m_LabelSecondPlayer.Text = k_TitleLabelSecondPlayer;
            ElementsDesignerTool.DesignElements(m_LabelFirstPlayer, ePositionBy.Left, m_LabelSecondPlayer);
            ElementsDesignerTool.DesignElements(m_LabelFirstPlayer, ePositionBy.Under, m_LabelSecondPlayer, k_Margin);
            this.Controls.Add(m_LabelSecondPlayer);

            // TextBox-Second-Player
            m_TextBoxSecondPlayer = new TextBox();
            m_TextBoxSecondPlayer.Text = k_TitleDefultSecondPlayer;
            ElementsDesignerTool.DesignElements(m_LabelSecondPlayer, ePositionBy.Right, m_TextBoxSecondPlayer, k_Margin);
            ElementsDesignerTool.DesignElements(m_LabelSecondPlayer, ePositionBy.HorizontalCentre, m_TextBoxSecondPlayer, k_Margin);
            m_TextBoxSecondPlayer.Enabled = !v_Enable;
            this.Controls.Add(m_TextBoxSecondPlayer);

            // Button-Against-A-Friend
            m_ButtonAgainstAFriend = new Button();
            m_ButtonAgainstAFriend.Text = k_TitleButtonAgainstAFriend;
            ElementsDesignerTool.DesignElements(m_TextBoxSecondPlayer, ePositionBy.Right, m_ButtonAgainstAFriend, k_Margin);
            ElementsDesignerTool.DesignElements(m_TextBoxSecondPlayer, ePositionBy.HorizontalCentre, m_ButtonAgainstAFriend);
            m_ButtonAgainstAFriend.Click += ButtonAgainstAFriend_Click;
            this.Controls.Add(m_ButtonAgainstAFriend);

            // Label-Scroll-Bord-Size
            m_LabelBordSize = new Label();
            m_LabelBordSize.Text = k_TitleLabelBordSize;
            ElementsDesignerTool.DesignElements(m_LabelBordSize, ePositionBy.Left, m_LabelSecondPlayer);
            ElementsDesignerTool.DesignElements(m_LabelBordSize, ePositionBy.Under, m_LabelSecondPlayer, k_Margin);
            this.Controls.Add(m_LabelBordSize);

            // Scroll-Bord-Size
            m_ScrollBordSize = new VScrollBar();
            ElementsDesignerTool.DesignElements(m_ScrollBordSize, ePositionBy.Left, m_LabelSecondPlayer);
            ElementsDesignerTool.DesignElements(m_ScrollBordSize, ePositionBy.Under, m_LabelSecondPlayer, k_Margin);
            this.Controls.Add(m_ScrollBordSize);

            //Button-Start
            m_ButtonStart = new Button();
            m_ButtonStart.Text = k_TitleButtonStart;
            ElementsDesignerTool.DesignElements(m_ButtonStart, ePositionBy.Right, m_ButtonAgainstAFriend);
            ElementsDesignerTool.DesignElements(m_ButtonStart, ePositionBy.Bottom, m_ScrollBordSize);
            this.Controls.Add(m_ButtonStart);
        }

        public static SetUpNewGameForm StartGameForm()
        {
            return new SetUpNewGameForm();
        }

        public static SetUpNewGameForm RestartGameForm()
        {
            SetUpNewGameForm restartGameForm = new SetUpNewGameForm();
            restartGameForm.m_LabelFirstPlayer.Visible = !v_Show;
            restartGameForm.m_LabelSecondPlayer.Visible = !v_Show;
            restartGameForm.m_TextBoxSecondPlayer.Visible = !v_Show;
            restartGameForm.m_TextBoxFirstPlayer.Visible = !v_Show;
            restartGameForm.m_ButtonAgainstAFriend.Visible = !v_Show;

            return restartGameForm;
        }

        private void ButtonAgainstAFriend_Click(object i_Sender, EventArgs e)
        {
            bool isTextBoxSecondPlayer = m_TextBoxSecondPlayer.Enabled;
            if (isTextBoxSecondPlayer)
            {
                m_TextBoxSecondPlayer.Text = k_TitleDefultSecondPlayer;
            }
            m_TextBoxSecondPlayer.Enabled = !isTextBoxSecondPlayer;
        }
    }
}
