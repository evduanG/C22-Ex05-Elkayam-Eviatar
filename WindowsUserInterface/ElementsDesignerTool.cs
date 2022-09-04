using System;
using System.Windows.Forms;

namespace WindowsUserInterface
{
    internal class ElementsDesignerTool
    {
        private const int k_NoMargin = 0;

        public static void DesignElements(Control i_ControlCompareTo, ePositionBy i_PositionBy, Control i_ControlToSetPosition)
        {
            DesignElements(i_ControlCompareTo, i_PositionBy, i_ControlToSetPosition, k_NoMargin);
        }

        public static void DesignElements(Control i_ControlCompareTo, ePositionBy i_PositionBy, Control i_ControlToSetPosition, int i_Margin)
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
                    setControlToTheBottom(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.Under:
                    setControlToTheUnder(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.VerticalCentre:
                    setControlToThVerticalCentre(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.HorizontalCentre:
                    setControlToTheHorizontalCentre(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
            }
        }

        private static void setControlToTheUnder(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Bottom + i_Margin;
        }

        private static void setControlToTheHorizontalCentre(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Top + (i_ControlCompareTo.Height / 2) - (i_ControlToSetPosition.Height / 2) + i_Margin;
        }

        private static void setControlToThVerticalCentre(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Left = i_ControlCompareTo.Left + (i_ControlCompareTo.Width / 2) - (i_ControlToSetPosition.Width / 2) + i_Margin;
        }

        private static void setControlToTheBottom(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Bottom + i_ControlToSetPosition.Height;
        }

        private static void setControlToTheTop(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Top + i_Margin;
        }

        private static void setControlToTheRight(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Left = i_ControlCompareTo.Right + i_Margin;
        }

        private static void setControlToTheLeft(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Left = i_ControlCompareTo.Left + i_Margin;
        }

        public static int ConfigClientSizeWidth(Control i_First, Control i_Last, int i_Margin)
        {
            return 0;
        }

        public static int ConfigClientSizeHeight(Control i_First, Control i_Last, int i_Margin)
        {
            return 0;
        }
    }
}
