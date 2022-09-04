using System.Windows.Forms;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace WindowsUserInterface
{
    public delegate void StartClickHandler(object sender, MouseEventArgs e);

    public class SetUpNewGameForm : Form
    {
        private const string k_TitleForm = "Memory Game - Settung";
        private const string k_TitleLabelFirstplayer = "First Plyer Name:";
        private const string k_TitleLabelSecondPlayer = "Second Plyer Name:";
        private const string k_TitleLabelBordSize = "Bord Size:";
        private const string k_TitleButtonAgainstAFriend = "Against a Friend";
        private const string k_TitleButtonStart = "Start!";
        private const string k_TitleDefultSecondPlayer = "-computer-";
        private const int k_Margin = 12;
        private const bool k_Show = true;
        private const bool k_Enable = true;
        private const bool k_FirstGame = true;

        public event StartClickHandler StartClick;

        public readonly bool r_IsFirstGame;
        private Label m_LabelFirstPlayer;
        private Label m_LabelSecondPlayer;
        private Label m_LabelBordSize;
        private TextBox m_TextBoxFirstPlayer;
        private TextBox m_TextBoxSecondPlayer;
        private ComboBox m_ComboBoxBordSize;
        private Button m_ButtonAgainstAFriend;
        private Button m_ButtonStart;
        private List<BordSizeOptions> m_ListBordSizeOptions;

        public string FirstPlayerName
        {
            get { return m_TextBoxFirstPlayer.Text; }
        }

        public string SecondPlayerName
        {
            get { return m_TextBoxSecondPlayer.Text; }
        }

        public bool IsSecondPlayerComputer
        {
            get
            {
                return m_ButtonAgainstAFriend.Enabled;
            }
        }

        public void GetSelectedDimensions(out byte o_Higt, out byte o_Width)
        {
            object bordSize = m_ComboBoxBordSize.SelectedItem;
            o_Higt = ((BordSizeOptions)bordSize).Higt;
            o_Width = ((BordSizeOptions)bordSize).Width;
        }

        private SetUpNewGameForm(bool i_IsFirstGame, string i_FirstplayerName, string i_SecondPlayerName)
        {
            r_IsFirstGame = i_IsFirstGame;
            Size = new Size(400, 400);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            this.Text = k_TitleForm;
            m_ListBordSizeOptions = new List<BordSizeOptions>();
            initiationForm(i_FirstplayerName, i_SecondPlayerName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void initiationForm(string i_FirstplayerName, string i_SecondPlayerName)
        {
            // Label-First-Plyer
            m_LabelFirstPlayer = new Label();
            m_LabelFirstPlayer.Text = k_TitleLabelFirstplayer;
            m_LabelFirstPlayer.Left = k_Margin;
            m_LabelFirstPlayer.Top = k_Margin;
            this.Controls.Add(m_LabelFirstPlayer);

            // TextBox-First-player
            m_TextBoxFirstPlayer = new TextBox();
            m_TextBoxFirstPlayer.Text = i_FirstplayerName;
            ElementsDesignerTool.DesignElements(m_TextBoxFirstPlayer, ePositionBy.NextToTheLeft,  m_LabelFirstPlayer, k_Margin);
            ElementsDesignerTool.DesignElements(m_TextBoxFirstPlayer, ePositionBy.HorizontalCentre, m_LabelFirstPlayer);
            this.Controls.Add(m_TextBoxFirstPlayer);

            // Label-Second-Plyer
            m_LabelSecondPlayer = new Label();
            m_LabelSecondPlayer.Text = i_SecondPlayerName;
            ElementsDesignerTool.DesignElements(m_LabelSecondPlayer, ePositionBy.Left, m_LabelFirstPlayer);
            ElementsDesignerTool.DesignElements(m_LabelSecondPlayer, ePositionBy.Under, m_LabelFirstPlayer, k_Margin);
            this.Controls.Add(m_LabelSecondPlayer);

            // TextBox-Second-Player
            m_TextBoxSecondPlayer = new TextBox();
            m_TextBoxSecondPlayer.Text = k_TitleDefultSecondPlayer;
            ElementsDesignerTool.DesignElements(m_TextBoxSecondPlayer, ePositionBy.NextToTheLeft, m_LabelSecondPlayer, k_Margin);
            ElementsDesignerTool.DesignElements(m_TextBoxSecondPlayer, ePositionBy.HorizontalCentre, m_LabelSecondPlayer, k_Margin);
            m_TextBoxSecondPlayer.Enabled = !k_Enable;
            this.Controls.Add(m_TextBoxSecondPlayer);

            // Button-Against-A-Friend
            m_ButtonAgainstAFriend = new Button();
            m_ButtonAgainstAFriend.Text = k_TitleButtonAgainstAFriend;
            ElementsDesignerTool.DesignElements(m_ButtonAgainstAFriend, ePositionBy.NextToTheLeft, m_TextBoxSecondPlayer, k_Margin);
            ElementsDesignerTool.DesignElements(m_ButtonAgainstAFriend, ePositionBy.HorizontalCentre, m_TextBoxSecondPlayer);
            m_ButtonAgainstAFriend.Click += ButtonAgainstAFriend_Click;
            m_ButtonAgainstAFriend.AutoSize = true;
            this.Controls.Add(m_ButtonAgainstAFriend);

            // Label-ComboBox-Bord-Size
            m_LabelBordSize = new Label();
            m_LabelBordSize.Text = k_TitleLabelBordSize;
            ElementsDesignerTool.DesignElements(m_LabelBordSize, ePositionBy.Left, m_LabelSecondPlayer);
            ElementsDesignerTool.DesignElements(m_LabelBordSize, ePositionBy.Under, m_LabelSecondPlayer, k_Margin);
            this.Controls.Add(m_LabelBordSize);

            // ComboBox-Bord-Size
            m_ComboBoxBordSize = new ComboBox();
            ElementsDesignerTool.DesignElements(m_ComboBoxBordSize, ePositionBy.Left, m_LabelBordSize);
            ElementsDesignerTool.DesignElements(m_ComboBoxBordSize, ePositionBy.Under, m_LabelBordSize, k_Margin);
            this.Controls.Add(m_ComboBoxBordSize);

            // Button-Start
            m_ButtonStart = new Button();
            m_ButtonStart.Text = k_TitleButtonStart;
            ElementsDesignerTool.DesignElements(m_ButtonStart, ePositionBy.Right, m_ButtonAgainstAFriend);
            ElementsDesignerTool.DesignElements(m_ButtonStart, ePositionBy.Bottom, m_ComboBoxBordSize);
            m_ButtonStart.Click += ButtonStart_Click;
            this.Controls.Add(m_ButtonStart);
            if(!r_IsFirstGame)
            {
                m_ButtonAgainstAFriend.Enabled = !k_Enable;
                m_TextBoxFirstPlayer.Enabled = !k_Enable;
            }
        }

        public static SetUpNewGameForm StartGameForm()
        {
            return new SetUpNewGameForm(k_FirstGame, string.Empty, k_TitleDefultSecondPlayer);
        }

        public static SetUpNewGameForm RestartGameForm(string i_FirstplayerName, string i_SecondPlayerName)
        {
            return new SetUpNewGameForm(!k_FirstGame, i_FirstplayerName, i_SecondPlayerName);
        }

        public void SetListOfBordSizeOptions(byte i_HigtMin, byte i_HigtMax, byte i_WidthMin, byte i_WidthMax)
        {
            for(byte higt = i_HigtMin; higt <= i_HigtMax; higt++)
            {
                for(byte widt = i_WidthMin; widt <= i_WidthMax; widt++)
                {
                    m_ListBordSizeOptions.Add(new BordSizeOptions(higt, widt));
                    m_ComboBoxBordSize.Items.Add(new BordSizeOptions(higt, widt));
                }
            }
        }

        protected void ButtonAgainstAFriend_Click(object i_Sender, EventArgs e)
        {
            if(r_IsFirstGame)
            {
                bool isTextBoxSecondPlayer = m_TextBoxSecondPlayer.Enabled;

                if (isTextBoxSecondPlayer)
                {
                    m_TextBoxSecondPlayer.Text = k_TitleDefultSecondPlayer;
                }
                else
                {
                    m_TextBoxSecondPlayer.Text = string.Empty;
                }

                m_TextBoxSecondPlayer.Enabled = !isTextBoxSecondPlayer;
            }
        }

        protected void ButtonStart_Click(object i_Sender, EventArgs e)
        {
            if(!isValidForm())
            {
                // eror masg
            }
            else
            {
                this.Close();
            }
        }

        private bool isValidForm()
        {
            bool isFirstPlayer = m_TextBoxFirstPlayer.Text != string.Empty;
            bool isSecondPlayer = m_TextBoxSecondPlayer.Text != string.Empty;
            bool isValid = m_ComboBoxBordSize.SelectedItem != null;

            return isFirstPlayer && isSecondPlayer && isValid;
        }

        private struct BordSizeOptions
        {
            private const string k_ToStringFormt = "{0}x{1}";
            private byte m_Higt;
            private byte m_Width;

            public BordSizeOptions(byte i_Higt, byte i_Width)
            {
                this.m_Higt = i_Higt;
                this.m_Width = i_Width;
            }

            public byte Higt
            {
                get { return m_Higt; }
            }

            public byte Width
            {
                get { return m_Width; }
            }

            public override string ToString()
            {
                return string.Format(k_ToStringFormt, m_Width, m_Higt);
            }
        }

        //private void InitializeComponent()
        //{
        //    this.SuspendLayout();
        //    // 
        //    // SetUpNewGameForm
        //    // 
        //    this.ClientSize = new System.Drawing.Size(282, 253);
        //    this.Name = "SetUpNewGameForm";
        //    this.ResumeLayout(false);

        //}
    }
}
