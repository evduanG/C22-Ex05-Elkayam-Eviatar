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

            if (Setting.NumOfPlayers.sv_IsFixed)
            {
                m_TotalPLayers = Setting.NumOfPlayers.sr_UpperBound;
            }
            else
            {
                m_TotalPLayers = m_NumberOfPlayersBox.UserChoice;
            }

            m_AllPlayersInGame = new Player[m_TotalPLayers];

            // timer setup
            m_InbetweenTurnsTimer = new Timer();
            InbetweenTurnsTimer.Interval = m_SleepBetweenTurns;
            InbetweenTurnsTimer.Tick += InbetweenTurnsTimer_Tick;
        }

        // =======================================================
        // Propertys
        // =======================================================
        private Player CurrentPlayer
        {
            get
            {
                int indx = TurnCounter % m_AllPlayersInGame.Length;
                return m_AllPlayersInGame[indx];
            }
        }

        public Timer InbetweenTurnsTimer
        {
            get { return m_InbetweenTurnsTimer; }
        }

        public bool IsInBetweenTurns
        {
            get { return m_InbetweenTurnsTimer.Enabled; }
        }

        public byte TurnCounter
        {
            get
            {
                return m_TurnCounter;
            }

            set
            {
                bool isOverflow = m_TurnCounter >= byte.MaxValue - value;

                if (isOverflow)
                {
                   value = 0;
                }

                m_TurnCounter = value;
            }
        }

        // ===================================================================
        //   start methods
        // ===================================================================
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

        private void startNewGame(byte i_Higt, byte i_Width)
        {
            m_GameLogic = new GameLogic(i_Higt, i_Width);
            m_GameForm = new Screen.MainGameForm(i_Higt, i_Width, 2, CurrentPlayer.Name);

            // m_GameBoard.ApplyAllTheButtons(m_GameForm);
            m_GameForm.AnyButtonClick += AnyButtonClick_FirstClick;


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

        private void endOfTurn() // TODO: find a good name
        {
            bool isThePlyerHaveAnderTurn = m_GameLogic.DoThePlayersChoicesMatch(out byte o_ScoreForTheTurn, m_SelectedTileInTurn.ToArray());

            CurrentPlayer.IncreaseScore(o_ScoreForTheTurn);
            m_GameForm.SetPlayerNamesAndScore(CurrentPlayer.ToString(), CurrentPlayer.ID);

            if (!isThePlyerHaveAnderTurn)
            {
                TurnCounter++;
                m_GameForm.FlippCardsToFaceDown(m_SelectedTileInTurn);
                m_GameForm.SetCurrentPlayer(CurrentPlayer.Name, CurrentPlayer.Color);
            }
            else
            {
                m_GameForm.ColorPair(m_SelectedTileInTurn, CurrentPlayer.Color);
            }

            m_SelectedTileInTurn.Clear();

            if(m_GameLogic.HaveMoreMoves)
            {
                m_GameForm.AnyButtonClick += AnyButtonClick_FirstClick;
                m_GameForm.AnyButtonClick -= AnyButtonClick_SecondClick;
            }

            // System.Threading.Thread.Sleep(1000);
        }

        // =======================================================
        // Delegates and Events methods
        // =======================================================
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
            setUpNewGameForm.HideInTaskbar();
            startNewGame(o_Higt, o_Width);
        }

        protected virtual void MessageBox_Occur(object i_Sender, EventArgs i_ButtomIndexEvent)
        {
            Screen.MessageBox messageBox = i_Sender as Screen.MessageBox;
        }

        protected virtual void AnyButtonClick_FirstClick(object i_Sender, EventArgs i_ButtomIndexEvent)
        {
            if (!IsInBetweenTurns)
            {
                Screen.MainGameForm mainGameForm = i_Sender as Screen.MainGameForm;
                ButtomIndexEvent buttomIndexEvent = i_ButtomIndexEvent as ButtomIndexEvent;
                char v = this.m_GameLogic.Flipped(buttomIndexEvent, true);
                m_GameForm.Flipped(buttomIndexEvent, v);
                m_SelectedTileInTurn.Add(buttomIndexEvent);
                m_GameForm.AnyButtonClick -= AnyButtonClick_FirstClick;
                m_GameForm.AnyButtonClick += AnyButtonClick_SecondClick;
            }
        }

        protected virtual void AnyButtonClick_SecondClick(object i_Sender, EventArgs e)
        {
            if (!IsInBetweenTurns)
            {
                Screen.MainGameForm mainGameForm = i_Sender as Screen.MainGameForm;
                ButtomIndexEvent buttomIndexEvent = e as ButtomIndexEvent;
                char v = this.m_GameLogic.Flipped(buttomIndexEvent, true);
                m_GameForm.Flipped(buttomIndexEvent, v);
                m_SelectedTileInTurn.Add(buttomIndexEvent);
                InbetweenTurnsTimer.Start();
                // endOfTurn();
            }
        }

        protected virtual void InbetweenTurnsTimer_Tick(object sender, EventArgs e)
        {
            endOfTurn();
            InbetweenTurnsTimer.Stop();
        }
    }
}