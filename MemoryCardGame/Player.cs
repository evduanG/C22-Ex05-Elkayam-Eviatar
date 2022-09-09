using System.Collections.Generic;
using System.Drawing;

namespace MemoryCardGame
{
    internal class Player
    {
        private static readonly Color[] sr_Colors = { System.Drawing.Color.LightBlue, System.Drawing.Color.PaleGreen, System.Drawing.Color.LightPink };

        private byte m_Score;
        private string m_Name;
        private byte m_ID;

        // properties:
        public byte Score { get => m_Score; set => m_Score = value; }

        public string Name { get => m_Name; set => m_Name = value; }

        public byte ID { get => m_ID; set => m_ID = value; }

        public bool IsHuman
        {
            get
            {
                return this is AIPlayer;
            }
        }

        public Color Color
        {
            get
            {
                return sr_Colors[m_ID];
            }
        }

        public Player(string i_name, byte i_ID)
        {
            m_Score = 0;
            m_Name = i_name;
            m_ID = i_ID;
        }

        public void IncreaseScore(byte i_ScoreInTheTurn)
        {
            this.m_Score += i_ScoreInTheTurn;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} ", m_Name, m_Score);
        }

        public virtual string GetPlayerChoice(List<string> i_validSlotTOChase, char[,] i_BoardToDraw)
        {
            string returnChosice = string.Empty;
            // TODO : this is an empty func not a good way to do it
            return returnChosice;
        }

        public virtual void ShowBoard(char[,] i_GameBoard)
        {
            // TODO : this is an empty func not a good way to do it
        }

        public virtual void RestartNewGame()
        {
            // TODO : this is an empty func not a good way to do it
        }
    }
}
