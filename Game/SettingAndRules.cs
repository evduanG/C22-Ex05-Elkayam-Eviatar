using System;
using System.Net;
using System.IO;
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

        private static readonly string[] sr_Link =
        {
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\25-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\30-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\48-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\80-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\247-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\310-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\353-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\376-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\416-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\427-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\476-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\485-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\488-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\493-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\615-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\723-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\765-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\880-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\936-80x80.jpg",
            @"C:\Users\eviatar\source\repos\C22-Ex05-Elkayam-Eviatar\Resources\1015-80x80.jpg",
        };

        internal static byte[] GetRandomImagesIndexes(byte i_Size)
        {
            //Random rand = new Random();
            //List<byte> imageIndex = new List<byte>(i_Size);
            //for(byte i = 0; i < i_Size; i++)
            //{
            //    imageIndex.Add(i);
            //    imageIndex.Add(i);
            //}
            List<byte> indexes = new List<byte>();
            recImagesIndexes(ref indexes, i_Size);
            return indexes.ToArray();
        }

        private static void recImagesIndexes(ref List<byte> io_ListOfIndex, byte i)
        {
            if(io_ListOfIndex == null)
            {
                throw new Exception("recImagesIndexes list id null null");
            }
            switch(i % 3)
            {
                case 0:
                    if (i != 0)
                    {
                        io_ListOfIndex.Add(i);
                        io_ListOfIndex.Add(i);
                        i--;
                        recImagesIndexes(ref io_ListOfIndex, i);
                    }

                    break;
                case 1:
                    io_ListOfIndex.Add(i);
                    i--;
                    recImagesIndexes(ref io_ListOfIndex, i);
                    i++;
                    io_ListOfIndex.Add(i);
                    break;
                case 2:

                    byte j = i;
                    i--;
                    io_ListOfIndex.Add(i);
                    i--;
                    io_ListOfIndex.Add(j);
                    recImagesIndexes(ref io_ListOfIndex, i);
                    i++;
                    io_ListOfIndex.Add(i);
                    io_ListOfIndex.Add(j);
                    break;
                default:
                    break;
            }
        }

        private static readonly string[] sr_LinkO =
{
            @"https://i.picsum.photos/id/25/80/80.jpg?hmac=pzK6FjNKpAJWffMtnj_ko5UwkYllroQ23YINkLBzDZ0",
            @"https://i.picsum.photos/id/30/80/80.jpg?hmac=Zgep64WdlaMQdQbL5Hima-hzvojJJP7zN4IGM2gxutU",
            @"https://i.picsum.photos/id/48/80/80.jpg?hmac=WybqawWvwaF11Nj88_mZkxqivgz9yznCh-VmOSmsP7I",
            @"https://i.picsum.photos/id/80/80/80.jpg?hmac=KCN7yqJV-4KbonMgzkSkLRI2V7Pr_KhALlnouzoIsKY",
            @"https://i.picsum.photos/id/247/80/80.jpg?hmac=PSjSs_obpzxKCgk2oOMnEyDe_UgjruRHVJPxZf-N9Yc",
            @"https://i.picsum.photos/id/310/80/80.jpg?hmac=uZ8ldB4CmrLxzguUOLjbqllENRi2VklZsc4gGmdnfoU",
            @"https://i.picsum.photos/id/353/80/80.jpg?hmac=BWQ7KuaJ0teqLghZOlAl6TkN2k6f94WOvRGcRKOM--4",
            @"https://i.picsum.photos/id/376/80/80.jpg?hmac=Mxg9CeVBTpoBw0O_TZGG7Fvd4LlvzFF99yGaIKwBDQY",
            @"https://i.picsum.photos/id/416/80/80.jpg?hmac=khvRDqKe_LGx-Enq4za0M3EndIkG_GKaaYVhK5phpY4",
            @"https://i.picsum.photos/id/427/80/80.jpg?hmac=a2SISMkkFvvpaeI650fZrO5RiWMmabCLVua77WQzEnc",
            @"https://i.picsum.photos/id/476/80/80.jpg?hmac=ssImL_jvuT76YYjzqn8AMjg_zuyuP2UpAc2_oMYYgNo",
            @"https://i.picsum.photos/id/493/80/80.jpg?hmac=N7c5JkrVmTCiiGNeqzhZaMK1xKdwf5UzWTMA_V4GaOs",
            @"https://i.picsum.photos/id/488/80/80.jpg?hmac=thGJivubFS_NxpiNPNq10OyHWT7YtQXPswfv8zYv8eA",
            @"https://i.picsum.photos/id/485/80/80.jpg?hmac=WbVLZyNv434G9mOrtncUKLc-jpG6EN_HR_988Sku0MI",
            @"https://i.picsum.photos/id/615/80/80.jpg?hmac=ikStaZgsnTu9LKUpRx7AE5sYBYoOFFuJ-PHHdwZv3Gk",
            @"https://i.picsum.photos/id/723/80/80.jpg?hmac=eJLWsM3F-i5LGUTlbzNEmda4A7fjv5fb67yAOZF4LEs",
            @"https://i.picsum.photos/id/765/80/80.jpg?hmac=NJx9yZvvs25CZ-fb8JSl1qUnIoCUzjxI6DL9Vrb32Bo",
            @"https://i.picsum.photos/id/880/80/80.jpg?hmac=680-5_pQ5l25TOXAYzuLA3nhycuYB910Qc4DJ-KfCMY",
            @"https://i.picsum.photos/id/936/80/80.jpg?hmac=sZw54wNmri4TL29yfZJT3NsIVfCVyQ3GkwP2TC8V1JU",
            @"https://i.picsum.photos/id/1015/80/80.jpg?hmac=kN_c4gtIcr9tt2Od4uqye5EXzpPIZxGRzi_RifG8foU",
};

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

        public static string[] Link
        {
            get { return sr_Link; }
        }

        /******     number of players       ******/
        public const int k_SleepBetweenTurns = 200;

        /******     number of players       ******/
        public static readonly Rules sr_NumOfPlayers = new Rules("Number Of Players", k_NumOfParticipants, k_NumOfParticipants, k_IsFixed, k_ThrowFixedMsg);

        /****** number of Choice In players Turn ******/
        private const int k_NumOfChoiceInPlayerTurn = 2;
        private static readonly Rules sr_NumOfChoiceInTurn = new Rules("Number Of Choice In player Turn", k_NumOfChoiceInPlayerTurn, k_NumOfChoiceInPlayerTurn, k_IsFixed, k_ThrowFixedMsg);

        private static readonly Rules sr_Rows = new Rules("Number of Rows", k_UpperBound, k_LowerBound, !k_IsFixed, k_ThrowDimensionsMsg);
        private static readonly Rules sr_Columns = new Rules("Number of Rows", k_UpperBound, k_LowerBound, !k_IsFixed, k_ThrowDimensionsMsg);

        // TODO: add randomness to GetImage
        public static Image GetImage(byte i_ImageIndex)
        {
            if(i_ImageIndex < 0 || i_ImageIndex > sr_Images.Length)
            {
                return null;
            }

            return sr_Images[i_ImageIndex];
        }

        public static List<BoardLocation> GetBoardLocations()
        {
            List<BoardLocation> ret = new List<BoardLocation>();

            for(byte i = sr_Rows.LowerBound; i <= sr_Rows.UpperBound; i++)
            {
                for (byte j = sr_Columns.LowerBound; j <= sr_Columns.UpperBound; j++)
                {
                    bool isEven = (i * j) % 2 == 0;
                    if(isEven)
                    {
                        ret.Add(new BoardLocation(i, j));
                    }
                }
            }

            return ret;
        }

        /// make a arr of link to pix  or configure how to get the respond link
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
            public static bool IsBetween(byte i_valueChecked, byte i_upperBound, byte i_lowerBound)
            {
                return i_valueChecked <= i_upperBound && i_valueChecked >= i_lowerBound;
            }

            // authenticate value, throw exception if invalid
            public bool IsValid(byte i_valueChecked)
            {
                if (IsFixed)
                {
                    throw new ArgumentException();
                }

                return IsBetween(i_valueChecked, UpperBound, LowerBound);
            }

            public override string ToString()
            {
                return string.Format(" the {0} between {1} to {2} ", Name, LowerBound, UpperBound);
            }
        }
    }
}
