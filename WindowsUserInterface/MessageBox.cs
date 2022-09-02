using System.Windows.Forms;
using System.Drawing;
using System;

namespace WindowsUserInterface
{
    internal class MessageBox : Form
    {
        private Label m_WinnerMessage;

        public Label WinnerMessage
        {
            get { return m_WinnerMessage; }
            set { m_WinnerMessage = value; }
        }

        public MessageBox()
        {
            initializeComponents();
        }

        private void initializeComponents()
        {
            initializeForm();
            initializeContorls();
        }

        private void initializeForm()
        {
           Text = "YOU WON A FREE IPHONE 12!";
           Size = new Size(360, 180);
           StartPosition = FormStartPosition.CenterParent;
           FormBorderStyle = FormBorderStyle.FixedDialog;
           MaximizeBox = false;
        }

        private void initializeContorls()
        {
            WinnerMessage = new Label();
            WinnerMessage.Text = "Congratulations!";
            WinnerMessage.TextAlign = ContentAlignment.MiddleCenter;
            WinnerMessage.Top = (ClientSize.Height / 2) - (WinnerMessage.Height / 2);
            WinnerMessage.Left = (ClientSize.Width / 2) - (WinnerMessage.Width / 2);
            Controls.Add(m_WinnerMessage);
        }
    }
}
