using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.Control;

namespace WindowsUserInterface
{
    internal class ElementsDesignerTool
    {
        private const int k_NoMargin = 0;
        private const bool k_Show = true;

        public static void DesignElements(Control i_ControlToSetPosition, ePositionBy i_PositionBy, Control i_ControlCompareTo)
        {
            DesignElements(i_ControlToSetPosition, i_PositionBy, i_ControlCompareTo, k_NoMargin);
        }

        public static void DesignElements(Control i_ControlToSetPosition, ePositionBy i_PositionBy, Control i_ControlCompareTo, int i_Margin)
        {
            switch (i_PositionBy)
            {
                case ePositionBy.Left:
                    setControlToTheLeft(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.Right:
                    setControlToTheRight(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.Top:
                    setControlToTheTop(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.Bottom:
                    setControlToTheBottom(i_ControlCompareTo, i_ControlToSetPosition);
                    break;
                case ePositionBy.Under:
                    setControlToTheUnder(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.NextToTheLeft:
                    setControlNextToTheLeft(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.VerticalCentre:
                    setControlToThVerticalCentre(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.HorizontalCentre:
                    setControlToTheHorizontalCentre(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
            }
        }

        private static void setControlNextToTheLeft(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Left = i_ControlCompareTo.Right + i_Margin;
        }

        private static void setControlToTheUnder(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Bottom + i_Margin;
        }

        internal static void DesignElementsInMidOfForm(Form i_Form, Control i_ControlToSetPosition)
        {
            int midOfForm = (int)i_Form.ClientSize.Width / 2;
            int midOfControl = (int)i_ControlToSetPosition.Width / 2;
            int position = midOfForm - midOfControl;
            i_ControlToSetPosition.Left = position;
        }

        internal static void DesignElementsInMidOfForm(Form i_Form, Control i_ControlToSetPosition, int i_Margin)
        {
            DesignElementsInMidOfForm(i_Form, i_ControlToSetPosition);
            if (i_Margin < 0)
            {
                i_Margin -= i_ControlToSetPosition.Width;
            }

            Console.WriteLine(string.Format("DesignElementsInMidOfForm : form = {0}", i_Form.Width));
            Console.WriteLine(string.Format("DesignElementsInMidOfForm : control = {0}", i_ControlToSetPosition.Left));
            Console.WriteLine(string.Format("DesignElementsInMidOfForm : i_Margin = {0}", i_Margin));
            i_ControlToSetPosition.Left += i_Margin;
        }

        private static void setControlToTheHorizontalCentre(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Top + (i_ControlCompareTo.Height / 2) - (i_ControlToSetPosition.Height / 2) + i_Margin;
        }

        private static void setControlToThVerticalCentre(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            int midOfCompareTo = i_ControlCompareTo.Width / 2;
            int midOfSetPosition = i_ControlToSetPosition.Width / 2;
            i_ControlToSetPosition.Left = i_ControlCompareTo.Left + midOfCompareTo - midOfSetPosition + i_Margin;
        }

        private static void setControlToTheBottom(Control i_ControlCompareTo, Control i_ControlToSetPosition)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Bottom - i_ControlToSetPosition.Height;
        }

        private static void setControlToTheTop(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Top + i_Margin;
        }

        private static void setControlToTheRight(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Left = i_ControlCompareTo.Right + i_Margin - i_ControlToSetPosition.Width;
        }

        private static void setControlToTheLeft(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Left = i_ControlCompareTo.Left + i_Margin;
        }

        private static void displayInTaskbar(Form i_FormToHid, bool i_IsShowingInToolBar)
        {
            i_FormToHid.ShowInTaskbar = i_IsShowingInToolBar;
        }

        public static void ShowInTaskbar(Form i_FormToHide)
        {
            displayInTaskbar(i_FormToHide, k_Show);
        }

        public static void HideInTaskbar(Form i_FormToHide)
        {
            displayInTaskbar(i_FormToHide, !k_Show);
        }

        public static void FitTheSizeOfForm(Form i_FormToFit, int i_Margin)
        {
            if (i_FormToFit == null)
            {
                throw new ArgumentNullException("no form to fit the size");
            }

            int width = 0;
            int height = 0;
            ControlCollection controls = i_FormToFit.Controls;

            foreach (Control control in controls)
            {
                width = Math.Max(width, control.Location.X + control.Width);
                height = Math.Max(height, control.Location.Y + control.Height);
            }

            i_FormToFit.ClientSize = new Size(width + i_Margin, height + i_Margin);
        }
    }
}
