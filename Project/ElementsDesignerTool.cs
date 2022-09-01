using System;
using System.Windows.Forms;

namespace WindowsUserInterface
{
    internal class ElementsDesignerTool
    {
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
            throw new NotImplementedException();
        }

        private static void setControlToThVerticalCentre(Control i_ControlComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            throw new NotImplementedException();
        }

        private static void setControlToThBottom(Control i_ControlComprTo, Control i_ControlToSetPosition, int i_Margin)
        {
            throw new NotImplementedException();
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
            return 0;
        }

        public static int ConfigClientSizeHeight(Control i_First, Control i_Last, int i_Margin)
        {
            return 0;
        }
    }
}
