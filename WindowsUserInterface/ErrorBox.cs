using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsUserInterface
{
    internal class ErrorBox : Form
    {
        private const int k_MessageBoxHeight = 180;
        private const int k_MessageBoxWidth = 300;
        private const string k_Error = "Error: {0}. Please try again!";

        private Label m_ErrorMessage;
        private Button m_ButtonOk;

        // ctor:
        public ErrorBox(string i_ErrorType)
        {
            initializeComponents(i_ErrorType);
        }

        // properties:
        public Label ErrorLabel
        {
            get { return m_ErrorMessage; }
            set { m_ErrorMessage = value; }
        }

        public Button ButtonOk
        {
            get { return m_ButtonOk; }
            set { m_ButtonOk = value; }
        }

        private void initializeComponents(string i_ErrorType)
        {
            initializeForm();
            initializeLabel(i_ErrorType);
            initializeButton();
        }

        private void initializeForm()
        {
            Text = "Error";
            Size = new Size(k_MessageBoxWidth, k_MessageBoxHeight);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
        }

        private void initializeLabel(string i_ErrorType)
        {
            ErrorLabel = new Label();
            ErrorLabel.Text = string.Format(k_Error, i_ErrorType);
            ErrorLabel.TextAlign = ContentAlignment.MiddleCenter;
            ErrorLabel.AutoSize = true;
            ErrorLabel.Top = MainGameForm.k_Margin * 3;
            ErrorLabel.Left = (ClientSize.Width / 2) - ErrorLabel.Width;

            Controls.Add(m_ErrorMessage);
        }

        private void initializeButton()
        {
            ButtonOk = new Button();
            ButtonOk.Text = "Okay";
            ButtonOk.Top = ErrorLabel.Bottom + (MainGameForm.k_Margin * 2);
            ElementsDesignerTool.DesignElements(ePositionBy.VerticalCentre, ErrorLabel, ButtonOk, MainGameForm.k_Margin);
            ButtonOk.Click += buttonOk_Click;

            Controls.Add(ButtonOk);
        }

        private void buttonOk_Click(object i_ButtonClicked, EventArgs i_EventArgs)
        {
            // ((Button)i_ButtonClicked).DialogResult = DialogResult.OK;
            Close();
        }
    }
}
