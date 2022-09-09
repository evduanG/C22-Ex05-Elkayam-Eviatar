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
        public const bool k_FlippedTheCard = true;
        private readonly byte r_TotalPLayers;
        private readonly Timer r_InbetweenTurnsTimer;
        private readonly Player[] r_AllPlayersInGame;
        private readonly List<ButtomIndexEvent> r_SelectedTileInTurn;
        private Screen.MainGameForm m_GameForm;
        private Screen.NumberOfPlayersBox m_NumberOfPlayersBox;
        private Player[] m_AllPlayersInGame;
        private List<BoardLocation> m_SelectedTileInTurn;
        private GameLogic m_GameLogic;
        private byte m_TurnCounter;


        // ===================================================================
        //  constructor and methods that the constructor uses
        // ===================================================================
        public GameEngine()
        {
            m_GameLogic = null;
            m_TurnCounter = 0;
            m_SelectedTileInTurn = new List<BoardLocation>();

            /******     number of players       ******/
            m_NumberOfPlayersBox = new NumberOfPlayersBox();
            m_NumberOfPlayersBox.ShowDialog();

            if (Setting.NumOfPlayers.IsFixed)
            {
                m_TotalPLayers = Setting.NumOfPlayers.UpperBound;
            }
            else
            {
                m_TotalPLayers = m_NumberOfPlayersBox.UserChoice;
            }

            m_AllPlayersInGame = new Player[m_TotalPLayers];

            /******     timer setup       ******/
            m_InbetweenTurnsTimer = new Timer();
            InbetweenTurnsTimer.Interval = Setting.k_SleepBetweenTurns;
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

            // TODO : make this func to be from input of setting
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

            foreach(Player player in m_AllPlayersInGame)
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

        // =======================================================
        // Game Progress
        // ======================================================
        private bool gameAction(object i_Sender, EventArgs i_ButtomIndexEvent)
        {
            bool ans = false;

            if (!IsInBetweenTurns)
            {
                Screen.MainGameForm mainGameForm = i_Sender as Screen.MainGameForm;
                ButtomIndexEvent buttomIndexEvent = i_ButtomIndexEvent as ButtomIndexEvent;
                char v = this.m_GameLogic.Flipped(buttomIndexEvent.Location, k_FlippedTheCard);
                m_GameForm.Flipped(buttomIndexEvent.Location, v);
                m_SelectedTileInTurn.Add(buttomIndexEvent.Location);
                ans = true;
            }

            return ans;
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
                    m_AllPlayersInGame[1] = new AIPlayer(1);
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
            bool isGameAction = gameAction(i_Sender, i_ButtomIndexEvent);

            if(isGameAction)
            {
                m_GameForm.AnyButtonClick -= AnyButtonClick_FirstClick;
                m_GameForm.AnyButtonClick += AnyButtonClick_SecondClick;
            }
        }

        protected virtual void AnyButtonClick_SecondClick(object i_Sender, EventArgs i_ButtomIndexEvent)
        {
            bool isGameAction = gameAction(i_Sender, i_ButtomIndexEvent);

            if(isGameAction)
            {
                InbetweenTurnsTimer.Start();
            }
        }

        protected virtual void InbetweenTurnsTimer_Tick(object sender, EventArgs e)
        {
            endOfTurn();
            InbetweenTurnsTimer.Stop();
        }
    }
}
