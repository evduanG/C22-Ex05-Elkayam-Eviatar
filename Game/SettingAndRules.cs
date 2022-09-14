using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsUserInterface;

namespace Game
{
    public static class SettingAndRules
    {
        private const bool k_IsFixed = true;
        private const string k_ThrowFixedMsg = "The value is fixed";
        private const byte k_UpperBound = 6;
        private const byte k_LowerBound = 4;
        private const byte k_NumOfParticipants = 2;
        private const string k_ThrowDimensionsMsg = "The game dimensions have a fix size";

        private static readonly Image[] sr_Images =
        {
            ResourceImg.Img0_80x80,
            ResourceImg.Img1_80x80,
            ResourceImg.Img2_80x80,
            ResourceImg.Img3_80x80,
            ResourceImg.Img4_80x80,
            ResourceImg.Img5_80x80,
            ResourceImg.Img6_80x80,
            ResourceImg.Img7_80x80,
            ResourceImg.Img8_80x80,
            ResourceImg.Img9_80x80,
            ResourceImg.Img10_80x80,
            ResourceImg.Img11_80x80,
            ResourceImg.Img12_80x80,
            ResourceImg.Img13_80x80,
            ResourceImg.Img14_80x80,
            ResourceImg.Img15_80x80,
            ResourceImg.Img16_80x80,
            ResourceImg.Img17_80x80,
            ResourceImg.Img18_80x80,
            ResourceImg.Img19_80x80,
        };

        /******     time between turns      ******/
        public const int k_SleepBetweenTurns = 1000;

        /******     number of players       ******/
        private static readonly Rules sr_NumOfPlayers = new Rules("Number Of Players", k_NumOfParticipants, k_NumOfParticipants, k_IsFixed, k_ThrowFixedMsg);

        public static Rules NumOfPlayers
        {
            get { return sr_NumOfPlayers; }
        }

        /******     board dimensions        ******/
        private static readonly Rules sr_Rows = new Rules("Number of Rows", k_UpperBound, k_LowerBound, !k_IsFixed, k_ThrowDimensionsMsg);
        private static readonly Rules sr_Columns = new Rules("Number of Rows", k_UpperBound, k_LowerBound, !k_IsFixed, k_ThrowDimensionsMsg);

        public static Image GetImage(byte i_ImageIndex)
        {
            if (i_ImageIndex < 0 || i_ImageIndex > sr_Images.Length)
            {
                return null;
            }

            return sr_Images[i_ImageIndex];
        }

        public static List<BoardLocation> GetBoardLocations()
        {
            List<BoardLocation> ret = new List<BoardLocation>();

            for (byte i = sr_Rows.LowerBound; i <= sr_Rows.UpperBound; i++)
            {
                for (byte j = sr_Columns.LowerBound; j <= sr_Columns.UpperBound; j++)
                {
                    bool isEven = (i * j) % 2 == 0;
                    if (isEven)
                    {
                        ret.Add(new BoardLocation(i, j));
                    }
                }
            }

            return ret;
        }

        internal static byte[] GetRandomImagesIndexes(byte i_Size)
        {
            List<byte> indexes = new List<byte>();
            recImagesIndexes(ref indexes, i_Size);

            return indexes.ToArray();
        }

        private static void recImagesIndexes(ref List<byte> io_ListOfIndex, byte i_Index)
        {
            if (io_ListOfIndex == null)
            {
                throw new Exception("recImagesIndexes list is null");
            }

            switch(i_Index % 3)
            {
                case 0:
                    preOrderEnterCard(ref io_ListOfIndex, ref i_Index);
                    break;

                case 1:
                    inOrderEnterCard(ref io_ListOfIndex, ref i_Index);
                    break;

                case 2:
                    mixOrderEnterCard(ref io_ListOfIndex, ref i_Index);
                    break;

                default:
                    break;
            }
        }

        private static void mixOrderEnterCard(ref List<byte> io_ListOfIndex, ref byte io_Index)
        {
            byte j = io_Index;
            io_Index--;
            io_ListOfIndex.Add(io_Index);
            io_Index--;
            io_ListOfIndex.Add(j);
            recImagesIndexes(ref io_ListOfIndex, io_Index);
            io_Index++;
            io_ListOfIndex.Add(io_Index);
            io_ListOfIndex.Add(j);
        }

        private static void inOrderEnterCard(ref List<byte> io_ListOfIndex, ref byte io_Index)
        {
            io_ListOfIndex.Add(io_Index);
            io_Index--;
            recImagesIndexes(ref io_ListOfIndex, io_Index);
            io_Index++;
            io_ListOfIndex.Add(io_Index);
        }

        private static void preOrderEnterCard(ref List<byte> io_ListOfIndex, ref byte io_Index)
        {
            if (io_Index != 0)
            {
                io_ListOfIndex.Add(io_Index);
                io_ListOfIndex.Add(io_Index);
                io_Index--;
                recImagesIndexes(ref io_ListOfIndex, io_Index);
            }
        }

        public struct Rules
        {
            private readonly string r_Name;
            private readonly byte r_UpperBound;
            private readonly byte r_LowerBound;
            private readonly bool r_IsFixed;
            private readonly string r_ThrowMsg;

            public string Name
            {
                get
                {
                    return r_Name;
                }
            }

            public byte UpperBound
            {
                get
                {
                    return r_UpperBound;
                }
            }

            public byte LowerBound
            {
                get
                {
                    return r_LowerBound;
                }
            }

            public bool IsFixed
            {
                get
                {
                    return r_IsFixed;
                }
            }

            public string ThrowMsg
            {
                get
                {
                    return r_ThrowMsg;
                }
            }

            public Rules(string i_Name, byte i_UpperBound, byte i_LowerBound, bool i_IsFixed, string i_TorwStr)
            {
                r_Name = i_Name;
                r_UpperBound = i_UpperBound;
                r_LowerBound = i_LowerBound;
                r_IsFixed = i_IsFixed;
                r_ThrowMsg = i_TorwStr;
            }

            // check if value is within upper and lower bounds
            public static bool IsBetween(byte i_ValueChecked, byte i_UpperBound, byte i_LowerBound)
            {
                return i_ValueChecked <= i_UpperBound && i_ValueChecked >= i_LowerBound;
            }

            // authenticate value, throw exception if invalid
            public bool IsValid(byte i_ValueChecked)
            {
                if (IsFixed)
                {
                    throw new ArgumentException();
                }

                return IsBetween(i_ValueChecked, UpperBound, LowerBound);
            }

            public override string ToString()
            {
                return string.Format(" the {0} between {1} to {2} ", Name, LowerBound, UpperBound);
            }
        }
    }
}
