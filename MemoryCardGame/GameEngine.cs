using System.Text;
using System;
using System.Collections.Generic;
using System.Threading;
using Game;
using Setting = Game.SettingAndRules;
using Screen = WindowsUserInterface;
using WindowsUserInterface;

namespace MemoryCardGame
{
    public class GameEngine
    {
        public const bool k_Running = true;
        public const bool k_FlippedTheCard = true;
        private Screen.MainGameForm m_GameForm;
        private Player[] m_AllPlayersInGame;
        private List<string> m_PlayerChois = new List<string>();
        private GameLogic m_GameBoard;
        private byte m_TurnCounter;

        private byte m_TotalPLayers;
        private int m_SleepBetweenTurns = Setting.k_SleepBetweenTurns; // TODO : renam thiis to be Ticker ..

        private Player CurrentPlayer
        {
            get
            {
                int indx = m_TurnCounter % m_AllPlayersInGame.Length;
                return m_AllPlayersInGame[indx];
            }
        }

        // ===================================================================
        //  constructor  and methods that the constructor uses
        // ===================================================================
        public GameEngine()
        {
            m_GameBoard = null;

            // r_IsPlaying = false;
            m_TurnCounter = 0;
            m_PlayerChois = new List<string>();

            /******     number of players       ******/
            if (Setting.NumOfPlayers.v_IsFixed)
            {
                m_TotalPLayers = Setting.NumOfPlayers.r_UpperBound;
            }
            else
            {
                m_TotalPLayers = InputFromTheUserAccordingToTheRules(Setting.NumOfPlayers);
            }

            m_AllPlayersInGame = new Player[m_TotalPLayers];
        }

        public byte InputFromTheUserAccordingToTheRules(Setting.Rules i_Rule)
        {
            // TODO: make this func

            // string strMsg = string.Format("Please enter the {0}", i_Rule.ToString());
            // byte returnVal = 0;
            // bool isInputValid = false;
            // do
            // {
            //    // Screen.ShowMessage(strMsg);
            //    isInputValid = i_Rule.IsValid(returnVal);
            // } while (!isInputValid);

            // return returnVal;
            return 2;
        }

        // start the game
        private void startNewGame(byte i_Higt, byte i_Width)
        {
            m_GameBoard = new GameLogic(i_Higt, i_Higt);
            m_GameForm = new Screen.MainGameForm(i_Higt, i_Width, m_AllPlayersInGame[0].Name, m_AllPlayersInGame[0].Name);
            m_GameBoard.ApplyAllTheButtons(m_GameForm);
            m_GameForm.AynButtonClick += FirstCoche_Occur;
            m_GameForm.ShowDialog();
        }

        // render the game board and show stats
        private void drawBoard()
        {
            /*
            screen.clearboard();
            screen.showboard(m_gameboard.getboardtodraw());
            screen.showmessage(getplayersscoreline());
            */
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
            Screen.SetUpNewGameForm setUpNewGameForm = i_Sender as Screen.SetUpNewGameForm;

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
            }

            setUpNewGameForm.GetSelectedDimensions(out byte o_Higt, out byte o_Width);
            startNewGame(o_Higt, o_Width);
        }

        protected virtual void MessageBox_Occur(object i_Sender, EventArgs e)
        {
            Screen.MessageBox messageBox = i_Sender as Screen.MessageBox;
        }

        protected virtual void FirstChoice_Occur(object i_Sender)
        {
            Screen.MainGameForm mainGameForm = i_Sender as Screen.MainGameForm;
            ButtomIndexEvent buttomIndexEvent = e as ButtomIndexEvent;

            // m_GameBoard[x,y].flipe
            // set form to the img
            // add
            m_PlayerChois.Add(buttomIndexEvent.ToString());
            m_GameForm.AynButtonClick -= FirstCoche_Occur;
            m_GameForm.AynButtonClick += ScendCoche_Occur;
        }

        protected virtual void SecondChoice_Occur(object i_Sender)
        {
            Screen.MainGameForm mainGameForm = i_Sender as Screen.MainGameForm;
            ButtomIndexEvent buttomIndexEvent = e as ButtomIndexEvent;

            // m_GameBoard[x,y].flipe
            // set form to the img
            // add
            m_PlayerChois.Add(buttomIndexEvent.ToString());
            endOfTurn();
        }

        private void endOfTurn()
        {
            bool isThePlyerHaveAnderTurn = m_GameBoard.DoThePlayersChoicesMatch(out byte o_ScoreForTheTurn, m_PlayerChois.ToArray());

            CurrentPlayer.IncreaseScore(o_ScoreForTheTurn);
            if (!isThePlyerHaveAnderTurn)
            {
                m_TurnCounter++;
            }
            else
            {
                m_GameForm.ColorAndEnablePair(m_PlayerChois, CurrentPlayer.Color);
            }

            if(m_GameBoard.HaveMoreMoves)
            {
                m_PlayerChois.Clear();
                m_GameForm.AynButtonClick += FirstCoche_Occur;
                m_GameForm.AynButtonClick -= ScendCoche_Occur;
            }
        }

        public void DisplaySetUpForm()
        {
            Screen.SetUpNewGameForm form = Screen.SetUpNewGameForm.StartGameForm();
            form.SetListOfBordSizeOptions(4, 6, 4, 6);
            form.StartClick += ButtonStart_Click;
            form.ShowDialog();

            if (form.ShowDialog() == DialogResult.OK)
            {
                form.RestartGameForm();
            }

            // asq for more game ?
        }
    }
}

/*
private void start(byte i_Higt, byte i_Width)
{
    m_GameBoard = new GameLogic(i_Higt, i_Higt);
    m_IsPlaying = true;
    do
    {
        Screen.SetUpNewGameForm setUpForm = Screen.SetUpNewGameForm.StartGameForm();
        setUpForm.SetListOfBordSizeOptions(Setting.Columns.r_LowerBound, Setting.Columns.r_UpperBound, Setting.Rows.r_LowerBound, Setting.Rows.r_UpperBound);
        setUpForm.StartClick += ButtonStart_Click;
        setUpForm.ShowDialog();

        m_TurnCounter = 0;
        playTheGame();
        if (m_IsPlaying)
        {
            Screen.MessageBox messageBox = new Screen.MessageBox();
            messageBox.m_MessageBox += MessageBox_Occur;
            messageBox.ShowDialog();

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


    private void playTheGame()
    {
        m_GameForm = new Screen.MainGameForm(m_GameBoard.Rows, m_GameBoard.Columns,
            m_AllPlayersInGame[0].Name, m_AllPlayersInGame[1].Name);
        try
        {
            do
            {
                m_PlayerChois.Clear();
                // settheForm();
                // TODO : set the name of the currnt pleayr:
                // TODO : set the score of the players

                m_GameForm.AynButtonClick += FirstCoche_Occur;
                Player currentlyPlayingPlayer = m_AllPlayersInGame[getPlayerIndex()]; // chang in the form the name

                for (int i = 0; i < Setting.s_NumOfChoiceInTurn.r_UpperBound; i++)
                {
                }

                // Show all players the board
                showAllPlayersTheBoard();
                bool isThePlyerHaveAnderTurn = m_GameBoard.DoThePlayersChoicesMatch(out byte o_scoreForTheTurn, m_PlayerChois.ToArray());

                if (!isThePlyerHaveAnderTurn)
                {
                    m_TurnCounter++;
                }

                currentlyPlayingPlayer.IncreaseScore(o_scoreForTheTurn);
                Thread.Sleep(m_SleepBetweenTurns);
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

        // Screen.ShowPrompt(ePromptType.Winning, winnerName, highScore.ToString());
    }

    player turn
private string gameStage(Player i_currentlyPlayingPlayer)
    {
        bool userInputValid = true;
        string indexChoice = string.Empty;
        string mag = string.Format("{0} choose a tile", i_currentlyPlayingPlayer.Name);
        List<string> validSlotForChose = m_GameBoard.GetAllValidTilesForChoice();

        do
        {
            drawBoard();
            // Screen.ShowMessage(mag);
            if (!userInputValid)
            {
                // Screen.ShowError(eErrorType.CardTaken);
            }

            //indexChoice = i_currentlyPlayingPlayer.GetPlayerChoice(validSlotForChose, m_GameBoard.GetBoardToDraw());

            // userInputValid = validSlotForChose.Contains(indexChoice.ToUpper());
        }
        while (!userInputValid);

        m_GameBoard.Flipped(indexChoice, k_FlippedTheCard);

        return indexChoice;
    }
*/