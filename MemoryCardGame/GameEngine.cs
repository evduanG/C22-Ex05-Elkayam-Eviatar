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
        private readonly byte r_TotalPLayers;
        private readonly Timer r_InbetweenTurnsTimer;
        private readonly int r_SleepBetweenTurns = Setting.k_SleepBetweenTurns;
        private readonly Player[] r_AllPlayersInGame;
        private readonly List<ButtomIndexEvent> r_SelectedTileInTurn;

        private Screen.MainGameForm m_GameForm;
        private GameLogic m_GameLogic;
        private byte m_TurnCounter;

        // ===================================================================
        //  constructor and methods that the constructor uses
        // ===================================================================
        public GameEngine()
        {
            m_GameLogic = null;

            // r_IsPlaying = false;
            m_TurnCounter = 0;
            r_SelectedTileInTurn = new List<ButtomIndexEvent>();
            r_TotalPLayers = Setting.sr_NumOfPlayers.r_UpperBound;
            r_AllPlayersInGame = new Player[r_TotalPLayers];

            // timer setup
            r_InbetweenTurnsTimer = new Timer();
            InbetweenTurnsTimer.Interval = r_SleepBetweenTurns;
            InbetweenTurnsTimer.Tick += InbetweenTurnsTimer_Tick;
        }

        // =======================================================
        // Properties
        // =======================================================
        private Player CurrentPlayer
        {
            get
            {
                int indx = TurnCounter % r_AllPlayersInGame.Length;
                return r_AllPlayersInGame[indx];
            }
        }

        public Timer InbetweenTurnsTimer
        {
            get { return r_InbetweenTurnsTimer; }
        }

        public bool IsInBetweenTurns
        {
            get { return r_InbetweenTurnsTimer.Enabled; }
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

        private void startNewGame(byte i_Higt, byte i_Width)
        {
            m_GameLogic = new GameLogic(i_Higt, i_Width);
            m_GameForm = new Screen.MainGameForm(i_Higt, i_Width, 2);

            // m_GameBoard.ApplyAllTheButtons(m_GameForm);
            m_GameForm.AnyButtonClick += AnyButtonClick_FirstClick;

            foreach(Player player in r_AllPlayersInGame)
            {
                m_GameForm.SetPlayer(player.ToString(), player.Color, player.ID);
            }

            m_GameForm.SetCurrentPlayer(CurrentPlayer.Name, CurrentPlayer.Color);
            m_GameForm.ShowDialog();
        }

        private void showAllPlayersTheBoard()
        {
            foreach (Player player in r_AllPlayersInGame)
            {
                player.ShowBoard(m_GameLogic.GetBoardToDraw());

                // TODO : ShowBoard  m_SelectedTileInTurn and list of val ==> forgot what this means
            }
        }

        private void endOfTurn() // TODO: find a good name
        {
            bool isThePlyerHaveAnderTurn = m_GameLogic.DoThePlayersChoicesMatch(out byte o_ScoreForTheTurn, r_SelectedTileInTurn.ToArray());

            CurrentPlayer.IncreaseScore(o_ScoreForTheTurn);
            m_GameForm.SetPlayerNamesAndScore(CurrentPlayer.ToString(), CurrentPlayer.ID);

            if (!isThePlyerHaveAnderTurn)
            {
                TurnCounter++;
                m_GameForm.FlippCardsToFaceDown(r_SelectedTileInTurn);
                m_GameForm.SetCurrentPlayer(CurrentPlayer.Name, CurrentPlayer.Color);
            }
            else
            {
                m_GameForm.ColorPair(r_SelectedTileInTurn, CurrentPlayer.Color);
            }

            r_SelectedTileInTurn.Clear();

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
                r_AllPlayersInGame[0] = new Player(setUpNewGameForm.FirstPlayerName, 0);

                if(setUpNewGameForm.IsSecondPlayerComputer)
                {
                    r_AllPlayersInGame[1] = new Player(1);
                }
                else
                {
                    r_AllPlayersInGame[1] = new Player(setUpNewGameForm.SecondPlayerName, 1);
                }
            }

            setUpNewGameForm.GetSelectedDimensions(out byte o_Higt, out byte o_Width);
            setUpNewGameForm.HideInTaskbar();
            startNewGame(o_Higt, o_Width);
        }

        protected virtual void AnyButtonClick_FirstClick(object i_Sender, EventArgs i_ButtomIndexEvent)
        {
            if (!IsInBetweenTurns)
            {
                Screen.MainGameForm mainGameForm = i_Sender as Screen.MainGameForm;
                ButtomIndexEvent buttomIndexEvent = i_ButtomIndexEvent as ButtomIndexEvent;
                char v = this.m_GameLogic.Flipped(buttomIndexEvent, true);
                m_GameForm.Flipped(buttomIndexEvent, v);
                r_SelectedTileInTurn.Add(buttomIndexEvent);
                m_GameForm.AnyButtonClick -= AnyButtonClick_FirstClick;
                m_GameForm.AnyButtonClick += AnyButtonClick_SecondClick;
            }
        }

        protected virtual void AnyButtonClick_SecondClick(object i_Sender, EventArgs e)
        {
            if (!IsInBetweenTurns)
            {
                ButtomIndexEvent buttomIndexEvent = e as ButtomIndexEvent;
                char v = this.m_GameLogic.Flipped(buttomIndexEvent, true);
                m_GameForm.Flipped(buttomIndexEvent, v);
                r_SelectedTileInTurn.Add(buttomIndexEvent);
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
