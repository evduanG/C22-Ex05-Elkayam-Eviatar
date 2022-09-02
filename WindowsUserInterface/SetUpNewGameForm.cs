using System.Windows.Forms;
using System.Drawing;

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

        private Label m_LabelFirstPlayer;
        private Label m_LabelSecondPlayer;
        private Label m_LabelBordSize;
        private TextBox m_TextBoxFirstPlayer;
        private TextBox m_TextBoxSecondPlayer;
        private ScrollBar m_ScrollBordSize;
        private Button m_ButtonAgainstAFriend;
        private Button m_ButtonStart;

        private SetUpNewGameForm()
        {
            initiationForm();
        }

        private void initiationForm()
        {
            // Label-First-Plyer
            m_LabelFirstPlayer = new Label();
            m_LabelFirstPlayer.Text = k_TitleLabelFirstplayer;
            m_LabelFirstPlayer.Left = k_Margin;
            m_LabelFirstPlayer.Top = k_Margin;

            // TextBox-First-player
            m_TextBoxFirstPlayer = new TextBox();
            ElementsDesignerTool.DesignElements(ePositionBy.Left, m_LabelFirstPlayer, m_TextBoxFirstPlayer, k_Margin);
            ElementsDesignerTool.DesignElements(ePositionBy.HorizontalCentre, m_LabelFirstPlayer, m_TextBoxFirstPlayer);

            // Label-Second-Plyer
            m_LabelSecondPlayer = new Label();
            m_LabelSecondPlayer.Text = k_TitleLabelSecondPlayer;
            ElementsDesignerTool.DesignElements(ePositionBy.Left, m_LabelFirstPlayer, m_LabelSecondPlayer);
            ElementsDesignerTool.DesignElements(ePositionBy.Top, m_LabelFirstPlayer, m_LabelSecondPlayer, k_Margin);

            // TextBox-Second-Player
            m_TextBoxSecondPlayer = new TextBox();
            m_TextBoxSecondPlayer.Text = k_TitleDefultSecondPlayer;
            ElementsDesignerTool.DesignElements(ePositionBy.Left, m_LabelFirstPlayer, m_TextBoxSecondPlayer, k_Margin);
            ElementsDesignerTool.DesignElements(ePositionBy.HorizontalCentre, m_LabelSecondPlayer, m_TextBoxSecondPlayer, k_Margin);

            // Button-Against-A-Friend
            m_ButtonAgainstAFriend = new Button();
            m_ButtonAgainstAFriend.Text = k_TitleButtonAgainstAFriend;
            ElementsDesignerTool.DesignElements(ePositionBy.Left, m_TextBoxSecondPlayer, m_ButtonAgainstAFriend, k_Margin);
            ElementsDesignerTool.DesignElements(ePositionBy.VerticalCentre, m_TextBoxSecondPlayer, m_ButtonAgainstAFriend);

            // Scroll-Bord-Size
            // m_ScrollBordSize = new ScrollBar();
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
    }
}
