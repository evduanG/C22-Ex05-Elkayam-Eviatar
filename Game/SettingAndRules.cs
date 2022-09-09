using System;

namespace Game
{
    public static class SettingAndRules
    {
        public const int k_SleepBetweenTurns = 1500;
        public const char k_ExitTheGame = 'Q';
        private const int k_NumOfChoiceInPlayerTurn = 2;
        private const bool k_IsFixed = true;
        private const byte k_UpperBound = 6;
        private const byte k_LowerBound = 4;
        private const byte k_NumOfParticipants = 2;
        private const string k_ThrowFixedMsg = "The value is fixed";
        private const string k_ThrowDimensionsMsg = "The game dimensions heva a fix size";
        public static Rules Rows = new Rules("Num of Rows", k_UpperBound, k_LowerBound, !k_IsFixed, k_ThrowDimensionsMsg);
        public static Rules Columns = new Rules("Num of Rows", k_UpperBound, k_LowerBound, !k_IsFixed, k_ThrowDimensionsMsg);
        public static readonly Rules NumOfPlayers = new Rules("num Of Players", k_NumOfParticipants, k_NumOfParticipants, k_IsFixed, k_ThrowFixedMsg);
        public static Rules NumOfChoiceInTurn = new Rules("Num Of Choice In player Turn", k_NumOfChoiceInPlayerTurn, k_NumOfChoiceInPlayerTurn, k_IsFixed, k_ThrowFixedMsg);

        // TODO: 2.0 :
        /// make a arr of link to pic  or comfig how to get the resdpons link
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
