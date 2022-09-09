using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsUserInterface
{
    public class NumberOfPlayersBox : Form
    {
        private const string k_NumOfPlayersPrompt = "How many players do you want?";
        private const string k_NumOfPlayersTitle = "Set up number of players";
        private const string k_ButtonOkText = "Confirm";

        private const byte k_MaxNumOfPlayers = 5;
        private Label m_LabelNumberOfPlayers;
        private NumericUpDown m_NumberOfPlayers;
        private Button m_ButtonOK;

        // ===================================================================
        //  Properties & Constructor
        // ===================================================================
        public NumberOfPlayersBox()
        {
            initilizeComponents();
        }

        public Label LabelNumberOfPlayers
        {
            get { return m_LabelNumberOfPlayers; }
            set { m_LabelNumberOfPlayers = value; }
        }

        public NumericUpDown NumberOfPlayers
        {
            get { return m_NumberOfPlayers; }
            set { m_NumberOfPlayers = value; }
        }

        public Button ButtonOK
        {
            get { return m_ButtonOK; }
            set { m_ButtonOK = value; }
        }

        public byte UserChoice
        {
            get { return (byte)NumberOfPlayers.Value; }
        }

        // ===================================================================
        //  Initializations
        // ===================================================================
        private void initilizeComponents()
        {
            initializeForm();
            initializeLabel();
            initillizeNumericUpDown();
            initializeButton();
            ElementsDesignerTool.FitTheSizeOfForm(this, MainGameForm.k_Margin);
        }

        private void initializeForm()
        {
            ShowInTaskbar = false;
            ClientSize = new Size(400, 400);
            Text = k_NumOfPlayersTitle;
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            AcceptButton = m_ButtonOK;
        }

        private void initializeLabel()
        {
            LabelNumberOfPlayers = new Label();
            LabelNumberOfPlayers.Text = k_NumOfPlayersPrompt;
            LabelNumberOfPlayers.Left = MainGameForm.k_Margin;
            LabelNumberOfPlayers.Top = MainGameForm.k_Margin;

            Controls.Add(LabelNumberOfPlayers);
        }

        private void initillizeNumericUpDown()
        {
            NumberOfPlayers = new NumericUpDown();
            NumberOfPlayers.Left = LabelNumberOfPlayers.Left + MainGameForm.k_Margin;
            NumberOfPlayers.Maximum = k_MaxNumOfPlayers;
            NumberOfPlayers.Minimum = 2;
            ElementsDesignerTool.DesignElements(NumberOfPlayers, ePositionBy.HorizontalCentre, LabelNumberOfPlayers);
            ElementsDesignerTool.DesignElements(NumberOfPlayers, ePositionBy.NextToTheLeft, LabelNumberOfPlayers);

            Controls.Add(NumberOfPlayers);
        }

        private void initializeButton()
        {
            ButtonOK = new Button();
            ElementsDesignerTool.DesignElements(ButtonOK, ePositionBy.Under, NumberOfPlayers, MainGameForm.k_Margin);
            ElementsDesignerTool.DesignElements(ButtonOK, ePositionBy.Left, NumberOfPlayers, MainGameForm.k_Margin);
            ButtonOK.Text = k_ButtonOkText;
            ButtonOK.Click += ButtonOK_Click;

            Controls.Add(ButtonOK);
        }

        protected virtual void ButtonOK_Click(object i_ButtonOK, EventArgs i_EventArgs)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
