using System;
using System.Windows.Forms;

namespace WindowsUserInterface
{
    internal class ElementsDesignerTool
    {
        public static void DesignElements(ePositionBy i_PositionBy, Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            switch(i_PositionBy)
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
                case ePositionBy.VerticalCentre:
                    setControlToThVerticalCentre(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.HorizontalCentre:
                    setControlToTheHorizontalCentre(i_ControlCompareTo, i_ControlToSetPosition, i_Margin);
                    break;
            }
        }

        private static void setControlToTheHorizontalCentre(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Top + (i_ControlCompareTo.Height / 2) - (i_ControlToSetPosition.Height / 2);
        }

        private static void setControlToThVerticalCentre(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Left = i_ControlCompareTo.Left + (i_ControlCompareTo.Width / 2) - (i_ControlToSetPosition.Width / 2);
        }

        private static void setControlToTheBottom(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlCompareTo.Bottom + i_ControlToSetPosition.Height;
        }

        private static void setControlToTheTop(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            throw new NotImplementedException();
        }

        private static void setControlToTheRight(Control i_ControlCompareTo, Control i_ControlToSetPosition, int i_Margin)
        {
            throw new NotImplementedException();
        }

        private static void setControlToTheLeft(Control i_ComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            throw new NotImplementedException();
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
