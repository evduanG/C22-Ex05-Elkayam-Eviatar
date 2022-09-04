using System.Text;
using WindowsUserInterface;
using Game;
using System;
using Setting = Game.SettingAndRules;
using System.Collections.Generic;

namespace MemoryCardGame
{
    public class GameEngine
    {
        public const bool k_flippedTheCard = true;

        private Player[] m_AllPlayersInGame;
        private GameLogic m_GameBoard;
        private byte m_TurnCounter;
        private bool m_IsPlaying;
        private byte m_TotalPLayers;
        private int k_SleepBetweenTurns = Setting.k_SleepBetweenTurns;

        // ===================================================================
        //  constructor  and methods that the constructor uses
        // ===================================================================
        public GameEngine()
        {
            m_GameBoard = null;
            m_IsPlaying = false;
            m_TurnCounter = 0;

            /******     number of players       ******/
            if (Setting.m_NumOfPlayers.v_IsFixed)
            {
                m_TotalPLayers = Setting.m_NumOfPlayers.m_UpperBound;
            }
            else
            {
                m_TotalPLayers = InputFromTheUserAccordingToTheRules(Setting.m_NumOfPlayers);
            }

            m_AllPlayersInGame = new Player[m_TotalPLayers];
        }

        public byte InputFromTheUserAccordingToTheRules(Setting.Rules i_rule)
        {
            string strMsg = string.Format("Please enter the {0}", i_rule.ToString());
            byte returnVal = 0;
            bool isInputValid = false;
            do
            {
                Screen.ShowMessage(strMsg);
                isInputValid = i_rule.IsValid(returnVal);
            } while (!isInputValid);

            return returnVal;
        }

        private bool isRunning()
        {
            return m_IsPlaying || m_GameBoard.HaveMoreMoves;
        }

        private int getPlayerIndex()
        {
            return m_TurnCounter % m_TotalPLayers;
        }

        // start the game
        public void Start()
        {
            m_IsPlaying = true;

            do
            {
                SetUpNewGameForm form = SetUpNewGameForm.StartGameForm();
                form.SetListOfBordSizeOptions(Setting.Columns.m_LowerBound, Setting.Columns.m_UpperBound, Setting.Rows.m_LowerBound, Setting.Rows.m_UpperBound);
                form.StartClick += ButtonStart_Click;
                form.ShowDialog();

                m_TurnCounter = 0;
                playTheGame();
                if (m_IsPlaying)
                {
                    Screen.ShowPrompt(ePromptType.AnotherGame);
                    m_IsPlaying = UserInput.GetBooleanAnswer();

                    if (m_IsPlaying)
                    {
                        // tell all the players that having a new game board
                        foreach (Player player in m_AllPlayersInGame)
                        {
                            player.RestartNewGame();
                        }
                    }
                }
            }
            while (m_IsPlaying);
        }

        private void playTheGame()
        {
            try
            {
                do
                {
                    Player currentlyPlayingPlayer = m_AllPlayersInGame[getPlayerIndex()];
                    List<string> playerChois = new List<string>();

                    for (int i = 0; i < Setting.NumOfChoiceInTurn.m_UpperBound; i++)
                    {
                        playerChois.Add(gameStage(currentlyPlayingPlayer));
                    }

                    // Show all players the board
                    showAllPlayersTheBoard();
                    bool isThePlyerHaveAnderTurn = m_GameBoard.DoThePlayersChoicesMatch(out byte o_scoreForTheTurn, playerChois.ToArray());

                    if (!isThePlyerHaveAnderTurn)
                    {
                        m_TurnCounter++;
                    }

                    currentlyPlayingPlayer.IncreaseScore(o_scoreForTheTurn);
                    Thread.Sleep(k_SleepBetweenTurns);

                }
                while (isRunning());
            }
            catch (Exception e)
            {
                m_IsPlaying = false;
                return;
            }
            showWhoWon();
        }

        private void showWhoWon()
        {
            byte highScore = 0;
            string winnerName = "";

            foreach (Player player in m_AllPlayersInGame)
            {
                if (player.Score > highScore)
                {
                    highScore = player.Score;
                    winnerName = player.Name;
                }
            }

            Screen.ShowPrompt(ePromptType.Winning, winnerName, highScore.ToString());
        }

        // player turn
        private string gameStage(Player i_currentlyPlayingPlayer)
        {
            bool userInputValid = true;
            string indexChoice;
            string mag = string.Format("{0} choose a tile", i_currentlyPlayingPlayer.Name);
            List<string> validSlotForChose = m_GameBoard.GetAllValidTilesForChoice();

            do
            {
                drawBoard();
                Screen.ShowMessage(mag);
                if (!userInputValid)
                {
                    Screen.ShowError(eErrorType.CardTaken);
                }
                indexChoice = i_currentlyPlayingPlayer.GetPlayerChoice(validSlotForChose, m_GameBoard.GetBoardToDraw());

                userInputValid = validSlotForChose.Contains(indexChoice.ToUpper());
            }
            while (!userInputValid);

            m_GameBoard.Flipped(indexChoice, k_flippedTheCard);

            return indexChoice;
        }

        // render the game board and show stats
        private void drawBoard()
        {
            Screen.ClearBoard();
            Screen.ShowBoard(m_GameBoard.GetBoardToDraw());
            Screen.ShowMessage(getPlayersScoreLine());
        }

        private void showAllPlayersTheBoard()
        {
            drawBoard();
            foreach (Player player in m_AllPlayersInGame)
            {
                player.ShowBoard(m_GameBoard.GetBoardToDraw());
            }
        }

        // show the current score
        private string getPlayersScoreLine()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Player player in m_AllPlayersInGame)
            {
                sb.Append(player.ToString());
            }

            return sb.ToString();
        }

        protected virtual void ButtonStart_Click(object i_Sender, EventArgs e)
        {
            SetUpNewGameForm setUpNewGameForm = i_Sender as SetUpNewGameForm;
            if (setUpNewGameForm != null)
            {
                m_AllPlayersInGame[0] = new Player(setUpNewGameForm.FirstPlayerName);

                if(setUpNewGameForm.IsSecondPlayerComputer)
                {
                    m_AllPlayersInGame[1] = new Player();
                }
                else
                {
                    m_AllPlayersInGame[1] = new Player(setUpNewGameForm.SecondPlayerName);
                }

                setUpNewGameForm.GetSelectedDimensions(out byte o_Higt, out byte o_Width);
                m_GameBoard = new GameLogic(o_Higt, o_Width);
            }
        }
    }
}
