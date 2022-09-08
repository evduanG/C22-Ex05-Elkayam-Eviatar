using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Game;
using WindowsUserInterface;
using Screen = WindowsUserInterface;
using Setting = Game.SettingAndRules;

namespace MemoryCardGame
{
    public class GameEngine
    {
        public const bool k_Running = true;
        public const bool k_FlippedTheCard = true;
        private Screen.MainGameForm m_GameForm;
        private Screen.NumberOfPlayersBox m_NumberOfPlayersBox;
        private Player[] m_AllPlayersInGame;
        private List<ButtomIndexEvent> m_SelectedTileInTurn;
        private GameLogic m_GameLogic;
        private byte m_TurnCounter;

        private byte m_TotalPLayers;
        private Timer m_InbetweenTurnsTimer;
        private int m_SleepBetweenTurns = Setting.k_SleepBetweenTurns;

        private Player CurrentPlayer
        {
            get
            {
                int indx = m_TurnCounter % m_AllPlayersInGame.Length;
                return m_AllPlayersInGame[indx];
            }
        }

        public Timer InbetweenTurnsTimer
        {
            get { return m_InbetweenTurnsTimer; }
        }

        // ===================================================================
        //  constructor and methods that the constructor uses
        // ===================================================================
        public GameEngine()
        {
            m_GameLogic = null;

            // r_IsPlaying = false;
            m_TurnCounter = 0;
            m_SelectedTileInTurn = new List<ButtomIndexEvent>();

            /******     number of players       ******/
            m_NumberOfPlayersBox = new NumberOfPlayersBox();
            m_NumberOfPlayersBox.ShowDialog();

            if (Setting.NumOfPlayers.v_IsFixed)
            {
                m_TotalPLayers = Setting.NumOfPlayers.r_UpperBound;
            }
            else
            {
                m_TotalPLayers = m_NumberOfPlayersBox.UserChoice;
            }

            m_AllPlayersInGame = new Player[m_TotalPLayers];

            // timer setup
            m_InbetweenTurnsTimer = new Timer();
            InbetweenTurnsTimer.Interval = m_SleepBetweenTurns;
            InbetweenTurnsTimer.Tick += inbetweenTurnsTimer_Tick;
        }

        // start the game
        private void startNewGame(byte i_Higt, byte i_Width)
        {
            m_GameLogic = new GameLogic(i_Higt, i_Width);
            m_GameForm = new Screen.MainGameForm(i_Higt, i_Width, 2, CurrentPlayer.Name);

            m_GameForm.AnyButtonHandler += FirstChoice_Occur;

            foreach(Player player in m_AllPlayersInGame)
            {
                m_GameForm.SetPlayer(player.ToString(), player.Color, player.ID);
            }

            m_GameForm.SetCurrentPlayer(CurrentPlayer.Name, CurrentPlayer.Color);
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
            foreach (Player player in m_AllPlayersInGame)
            {
                player.ShowBoard(m_GameLogic.GetBoardToDraw());
                // TODO : ShowBoard  m_SelectedTileInTurn and list of val ==> forgot what this means
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

        protected virtual void ButtonStart_Click(object i_Sender, EventArgs i_ButtomIndexEvent)
        {
            Screen.SetUpNewGameForm setUpNewGameForm = i_Sender as Screen.SetUpNewGameForm;

            if (setUpNewGameForm != null)
            {
                m_AllPlayersInGame[0] = new Player(setUpNewGameForm.FirstPlayerName, 0);

                if(setUpNewGameForm.IsSecondPlayerComputer)
                {
                    m_AllPlayersInGame[1] = new Player(1);
                }
                else
                {
                    m_AllPlayersInGame[1] = new Player(setUpNewGameForm.SecondPlayerName, 1);
                }
            }

            setUpNewGameForm.GetSelectedDimensions(out byte o_Higt, out byte o_Width);
            startNewGame(o_Higt, o_Width);
        }

        protected virtual void MessageBox_Occur(object i_Sender, EventArgs i_ButtomIndexEvent)
        {
            Screen.MessageBox messageBox = i_Sender as Screen.MessageBox;
        }

        protected virtual void FirstChoice_Occur(object i_Sender, EventArgs i_ButtomIndexEvent)
        {
            Screen.MainGameForm mainGameForm = i_Sender as Screen.MainGameForm;
            ButtomIndexEvent buttomIndexEvent = i_ButtomIndexEvent as ButtomIndexEvent;
            char v = this.m_GameLogic.Flipped(buttomIndexEvent, true);
            m_GameForm.Flipped(buttomIndexEvent, v);
                        // m_GameBoard[x,y].flipe
                        // set form to the img
                        // add
            m_SelectedTileInTurn.Add(buttomIndexEvent);
            m_GameForm.AnyButtonHandler -= FirstChoice_Occur;
            m_GameForm.AnyButtonHandler += SecondChoice_Occur;
        }

        protected virtual void SecondChoice_Occur(object i_Sender, EventArgs e)
        {
            Screen.MainGameForm mainGameForm = i_Sender as Screen.MainGameForm;
            ButtomIndexEvent buttomIndexEvent = e as ButtomIndexEvent;
            char v = this.m_GameLogic.Flipped(buttomIndexEvent, true);
            m_GameForm.Flipped(buttomIndexEvent, v);
                    // m_GameBoard[x,y].flipe
                    // set form to the img
                    // add
            m_SelectedTileInTurn.Add(buttomIndexEvent);
            InbetweenTurnsTimer.Start();
            // endOfTurn();
        }

        private void endOfTurn()
        {
            bool isThePlyerHaveAnderTurn = m_GameLogic.DoThePlayersChoicesMatch(out byte o_ScoreForTheTurn, m_SelectedTileInTurn.ToArray());

            CurrentPlayer.IncreaseScore(o_ScoreForTheTurn);
            m_GameForm.SetPlayerNamesAndScore(CurrentPlayer.ToString(), CurrentPlayer.ID);

            if (!isThePlyerHaveAnderTurn)
            {
                m_TurnCounter++;
                m_GameForm.FlippCardsToFaceDown(m_SelectedTileInTurn);
                m_GameForm.SetCurrentPlayer(CurrentPlayer.Name, CurrentPlayer.Color);
            }
            else
            {
                m_GameForm.ColorPair(m_SelectedTileInTurn, CurrentPlayer.Color);
            }

            if(m_GameLogic.HaveMoreMoves)
            {
                m_SelectedTileInTurn.Clear();
                m_GameForm.AnyButtonHandler += FirstChoice_Occur;
                m_GameForm.AnyButtonHandler -= SecondChoice_Occur;
            }

            // System.Threading.Thread.Sleep(1000);
        }

        private void inbetweenTurnsTimer_Tick(object sender, EventArgs e)
        {
            endOfTurn();
            InbetweenTurnsTimer.Stop();
        }

        public void DisplaySetUpForm()
        {
            Screen.SetUpNewGameForm form = Screen.SetUpNewGameForm.StartGameForm();
            form.SetListOfBordSizeOptions(4, 6, 4, 6);
            form.StartClick += ButtonStart_Click;
            form.ShowDialog();

            if (form.ShowDialog() == DialogResult.Yes)
            {
                form.RestartGameForm();
            }

            // asq for more game ?
        }

        public void DisplayNumberOfPlayersForm()
        {
            m_NumberOfPlayersBox = new NumberOfPlayersBox();
        }
    }
}