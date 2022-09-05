
using System.Collections.Generic;

namespace MemoryCardGame
{
    internal class Player
    {
        private readonly bool r_IsHuman;
        private byte m_Score;
        private string m_Name;

        // properties:
        public byte Score { get => m_Score; set => m_Score = value; }

        public string Name { get => m_Name; set => m_Name = value; }

        private AIPlayer? m_aiPlayer;

        public Player()
        {
            m_Name = "PC";
            m_Score = 0;
            r_IsHuman = false;
            m_aiPlayer = AIPlayer.CreateNew();
        }

        public Player(string i_name)
        {
            m_Score = 0;
            r_IsHuman = true;
            m_Name = i_name;
            m_aiPlayer = null;
        }

        public void IncreaseScore(byte i_ScoreInTheTurn)
        {
            this.m_Score += i_ScoreInTheTurn;
        }

        public bool IsHuman
        {
            get { return r_IsHuman; }
        }

        public string GetPlayerChoice(List<string> i_validSlotTOChase, char[,] i_BoardToDraw)
        {
            string returnChosice = string.Empty;

            if (IsHuman)
            {
                // returnChosice = UserInput.GetPlayerGameMove();
            }
            else
            {
                returnChosice = m_aiPlayer?.GetAIPlayerChoice(i_validSlotTOChase, i_BoardToDraw);
            }

            return returnChosice;
        }

        public void ShowBoard(char[,] i_GameBoard)
        {
            if (!IsHuman)
            {
                m_aiPlayer?.ShowBoard(i_GameBoard);
            }
        }

        public void RestartNewGame()
        {
            if (!IsHuman)
            {
                m_aiPlayer?.ResetMemory();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} ", m_Name, m_Score);
        }
    }
}
