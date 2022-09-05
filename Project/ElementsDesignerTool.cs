using System;
using System.Windows.Forms;

namespace WindowsUserInterface
{
    internal class ElementsDesignerTool
    {
        private const int k_NoMargin = 0;

        public static void DesignElements(ePositionBy i_PositionBy, Control i_ControlComprTo, Control i_ControlToSetPosition)
        {
            DesignElements(i_PositionBy, i_ControlComprTo, i_ControlToSetPosition, k_NoMargin);
        }

        public static void DesignElements(ePositionBy i_PositionBy, Control i_ControlComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            switch(i_PositionBy)
            {
                case ePositionBy.Left:
                    setControlToTheLeft(i_ControlComprTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.Right:
                    setControlToThRight(i_ControlComprTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.Top:
                    setControlToThTop(i_ControlComprTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.Bottom:
                    setControlToThBottom(i_ControlComprTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.VerticalCentre:
                    setControlToThVerticalCentre(i_ControlComprTo, i_ControlToSetPosition, i_Margin);
                    break;
                case ePositionBy.HorizontalCentre:
                    setControlToThHorizontalCentre(i_ControlComprTo, i_ControlToSetPosition, i_Margin);
                    break;
            }
        }

        private static void setControlToThHorizontalCentre(Control i_ControlComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Left = i_ControlComprTo.Left + i_Margin;
        }

        private static void setControlToThVerticalCentre(Control i_ControlComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Left = i_ControlComprTo.Left + i_Margin + i_ControlComprTo.Width;
        }

        private static void setControlToThBottom(Control i_ControlComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            i_ControlToSetPosition.Top = i_ControlComprTo.Top + i_Margin;
        }

        private static void setControlToThTop(Control i_ControlComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            throw new NotImplementedException();
        }

        private static void setControlToThRight(Control i_ControlComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            throw new NotImplementedException();
        }

        private static void setControlToTheLeft(Control i_ComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            throw new NotImplementedException();
        }

        public static int ConfigClientSizeWidth(Control i_First, Control i_Last, int i_Margin)
        {
            int clientSizeWidth = i_Margin * 2;

            return clientSizeWidth;
        }

        public static int ConfigClientSizeHeight(Control i_First, Control i_Last, int i_Margin)
        {
            int clientSizeeHeight = i_Margin * 2;

            return clientSizeeHeight;
        }
    }
}
