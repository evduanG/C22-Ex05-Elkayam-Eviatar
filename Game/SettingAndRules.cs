using System;

namespace Game
{
    public static class SettingAndRules
    {
        private const bool v_IsFixed = true;
        private const string k_ThrowFixedMsg = "The value is fixed";
        public const char k_ExitTheGame = 'Q';

        private const byte k_UpperBound = 6;
        private const byte k_LowerBound = 4;
        private const string k_ThrowDimensionsMsg = "The game dimensions heva a fix size";

        public static Rules Rows = new Rules("Num of Rows", k_UpperBound, k_LowerBound, !v_IsFixed, k_ThrowDimensionsMsg);
        public static Rules Columns = new Rules("Num of Rows", k_UpperBound, k_LowerBound, !v_IsFixed, k_ThrowDimensionsMsg);

        // or add to user input
        /******     number of players       ******/
        private const byte k_NumOfParticipants = 2;
        public static Rules NumOfPlayers = new Rules("num Of Players", k_NumOfParticipants, k_NumOfParticipants, v_IsFixed, k_ThrowFixedMsg);

        /******     input format       ******/
        private const string k_InputFormatMsg = "The input format of the game is Capital letter between A - F , and a number between 1-6";
        private static Rules[] s_InputFormat = { new Rules("input Format Letter",(byte)'A', (byte)'F', !v_IsFixed, k_InputFormatMsg),
            new Rules("input Format number", (byte)'1', (byte)'6', !v_IsFixed, k_InputFormatMsg),
        };

        /******     number of players       ******/
        public const int k_SleepBetweenTurns = 1500;

        /****** number of Choice In players Turn ******/
        private const int k_NumOfChoiceInPlayerTurn = 2;
        public static Rules s_NumOfChoiceInTurn = new Rules("Num Of Choice In player Turn",
            k_NumOfChoiceInPlayerTurn, k_NumOfChoiceInPlayerTurn, v_IsFixed, k_ThrowFixedMsg);

        // TODO: 2.0 : 
        /// make a arr of link to pic  or comfig how to get the resdpons link 
        /// 
        /// 
        public struct Rules
        {
            public readonly string r_Name;
            public readonly byte r_UpperBound;
            public readonly byte r_LowerBound;
            public readonly bool v_IsFixed;
            private readonly string r_TrowMsg;

            public Rules(string i_Name, byte i_UpperBound, byte i_LowerBound, bool i_IsFixed, string i_TorwStr)
            {
                r_Name = i_Name;
                r_UpperBound = i_UpperBound;
                r_LowerBound = i_LowerBound;
                v_IsFixed = i_IsFixed;
                r_TrowMsg = i_TorwStr;
            }

            // authenticate value, throw exception if invalid
            public bool IsValid(byte i_valueChecked)
            {
                if (v_IsFixed)
                {
                    throw new ArgumentException();
                }

                return IsBetween(i_valueChecked, r_UpperBound, r_LowerBound);
            }

            // check if value is within upper and lower bounds
            public static bool IsBetween(byte i_valueChecked, byte i_upperBound, byte i_lowerBound)
            {
                return i_valueChecked <= i_upperBound && i_valueChecked >= i_lowerBound;
            }

            public override string ToString()
            {
                return string.Format(" the {0} between {1} to {2} ", r_Name, r_LowerBound, r_UpperBound);
            }
        }
    }
}
