using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Game;
using WindowsUserInterface;
using Screen = WindowsUserInterface;
using Setting = Game.SettingAndRules;

namespace MemoryCardGame
{
    public delegate void AIPlayerHandler();

    public delegate void EndOfGameHandler();

    public class GameEngine
    {
        public const bool k_FlippedTheCard = true;
        public const byte k_NumOfPlayers = 2;
        private const int k_NumOfCardsToFlip = 2;
        private readonly byte r_TotalPLayers;
        private readonly Timer r_InbetweenTurnsTimer;
        private readonly Timer r_AiTimer;
        private readonly Player[] r_AllPlayersInGame;
        private readonly List<BoardLocation> r_SelectedTileInTurn;
        private Screen.MainGameForm m_GameForm;
        private GameLogic m_GameLogic;
        private byte m_TurnCounter;

        public event AIPlayerHandler AIPlaying;

        public event EndOfGameHandler GameEnding;

        // ===================================================================
        //  constructor and methods that the constructor uses
        // ===================================================================
        public GameEngine()
        {
            m_GameLogic = null;
            m_TurnCounter = 0;
            r_SelectedTileInTurn = new List<BoardLocation>();
            AIPlaying += AIPlaying_Move;
            GameEnding += GameEngine_GameEnding;

            /******     number of players       ******/
            r_TotalPLayers = Setting.NumOfPlayers.UpperBound;
            r_AllPlayersInGame = new Player[r_TotalPLayers];

            /******     timer setup       ******/
            r_InbetweenTurnsTimer = new Timer();
            r_AiTimer = new Timer();
            InbetweenTurnsTimer.Interval = Setting.k_SleepBetweenTurns;
            r_AiTimer.Interval = Setting.k_SleepBetweenTurns;
            InbetweenTurnsTimer.Tick += InbetweenTurnsTimer_Tick;
            r_AiTimer.Tick += AITimer_Tick;
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

        public bool IsClickale
        {
            get { return r_InbetweenTurnsTimer.Enabled || !CurrentPlayer.IsHuman; }
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
            Screen.SetUpNewGameForm form = Screen.SetUpNewGameForm.StartGameForm(Setting.GetBoardLocations());
            form.StartClick += ButtonStart_Click;
            form.ShowDialog();
        }

        private void startNewGame(byte i_Higt, byte i_Width)
        {
            m_GameLogic = new GameLogic(i_Higt, i_Width);
            m_GameForm = new Screen.MainGameForm(i_Higt, i_Width, k_NumOfPlayers);
            m_GameForm.AnyPictureBoxClick += AnyPictureBoxClick_FirstClick;

            foreach(Player player in r_AllPlayersInGame)
            {
                m_GameForm.SetPlayer(player.ToString(), player.Color, player.ID);
            }

            r_SelectedTileInTurn.Clear();
            m_GameForm.SetCurrentPlayer(CurrentPlayer.Name, CurrentPlayer.Color);
            m_GameForm.ShowDialog();
        }

        private void restartNewGame(byte i_Rows, byte i_Columns)
        {
            m_GameLogic = new GameLogic(i_Rows, i_Columns);
            m_GameForm.RestartGame();

            foreach (Player player in r_AllPlayersInGame)
            {
                m_GameForm.SetPlayer(player.ToString(), player.Color, player.ID);
            }

            r_SelectedTileInTurn.Clear();
            m_GameForm.SetCurrentPlayer(CurrentPlayer.Name, CurrentPlayer.Color);
            switchAnyButtonClick();
        }

        private void showAllPlayersTheBoard()
        {
            foreach (Player player in r_AllPlayersInGame)
            {
                if (!player.IsHuman)
                {
                    (player as AIPlayer).ShowBoard(m_GameLogic.GetBoardToDraw());
                }
            }
        }

        // =======================================================
        // Game Progress
        // ======================================================
        private bool doGameAction(EventArgs i_BoardLocationEventArgs)
        {
            bool ans = false;

            if (!IsClickale)
            {
                BoardLocationEventArgs boardLocationEventArgs = i_BoardLocationEventArgs as BoardLocationEventArgs;
                Image imageOfCard = this.m_GameLogic.Flipped(boardLocationEventArgs.Location, k_FlippedTheCard);
                m_GameForm.FlippCardToFaceUp(boardLocationEventArgs.Location, imageOfCard);
                r_SelectedTileInTurn.Add(boardLocationEventArgs.Location);
                ans = true;
            }

            return ans;
        }

        private void endOfTurn()
        {
            InbetweenTurnsTimer.Stop();
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

            showAllPlayersTheBoard();
            r_SelectedTileInTurn.Clear();

            if(m_GameLogic.HaveMoreMoves)
            {
                switchAnyButtonClick();
            }
            else
            {
                GameEnding.Invoke();
            }
        }

        private bool showWinner()
        {
            Player winner = CurrentPlayer;
            bool isTie = false;
            DialogResult dialogResult;

            foreach (Player playingPlayer in r_AllPlayersInGame)
            {
                if(winner == CurrentPlayer)
                {
                    continue;
                }

                if(winner.Score < playingPlayer.Score)
                {
                    winner = playingPlayer;
                }
                else
                {
                    if (winner.Score == playingPlayer.Score)
                    {
                        isTie = true;
                    }
                }
            }

            if (isTie)
            {
                dialogResult = Screen.MessageBox.MessageBoxTie(winner.Score.ToString()).ShowDialog();
            }
            else
            {
                dialogResult = Screen.MessageBox.MessageBoxWinner(winner.Name, winner.Score).ShowDialog();
            }

            return dialogResult == DialogResult.Yes;
        }

        private void switchAnyButtonClick()
        {
            m_GameForm.AnyPictureBoxClick -= AnyPictureBoxClick_SecondClick;

            if (CurrentPlayer.IsHuman)
            {
                m_GameForm.AnyPictureBoxClick += AnyPictureBoxClick_FirstClick;
            }
            else
            {
                r_AiTimer.Start();
            }
        }

        // =======================================================
        // Delegates and Events methods
        // =======================================================
        protected virtual void ButtonStart_Click(object i_Sender, EventArgs i_BoardLocationEventArgs)
        {
            Screen.SetUpNewGameForm setUpNewGameForm = i_Sender as Screen.SetUpNewGameForm;

            if (setUpNewGameForm != null)
            {
                r_AllPlayersInGame[0] = new Player(setUpNewGameForm.FirstPlayerName, 0);

                if(setUpNewGameForm.IsSecondPlayerComputer)
                {
                    r_AllPlayersInGame[1] = new AIPlayer(1);
                }
                else
                {
                    r_AllPlayersInGame[1] = new Player(setUpNewGameForm.SecondPlayerName, 1);
                }
            }

            setUpNewGameForm.GetSelectedDimensions(out byte o_Height, out byte o_Width);
            setUpNewGameForm.HideInTaskbar();
            startNewGame(o_Height, o_Width);
        }

        protected virtual void AnyPictureBoxClick_FirstClick(object i_Sender, EventArgs i_BoardLocationEventArgs)
        {
            bool isGameAction = doGameAction(i_BoardLocationEventArgs);

            if(isGameAction)
            {
                m_GameForm.AnyPictureBoxClick -= AnyPictureBoxClick_FirstClick;
                m_GameForm.AnyPictureBoxClick += AnyPictureBoxClick_SecondClick;
            }
        }

        protected virtual void AnyPictureBoxClick_SecondClick(object i_Sender, EventArgs i_BoardLocationEventArgs)
        {
            bool isGameAction = doGameAction(i_BoardLocationEventArgs);

            if(isGameAction)
            {
                InbetweenTurnsTimer.Start();
            }
        }

        protected virtual void InbetweenTurnsTimer_Tick(object i_Sender, EventArgs i_EventArgs)
        {
            InbetweenTurnsTimer.Stop();
            endOfTurn();
        }

        protected virtual void AITimer_Tick(object i_Sender, EventArgs i_EventArgs)
        {
            r_AiTimer.Stop();
            AIPlaying.Invoke();
        }

        protected virtual void AIPlaying_Move()
        {
            InbetweenTurnsTimer.Stop();

            for (int i = 0; i < k_NumOfCardsToFlip; i++)
            {
                BoardLocationEventArgs choiceOfAI = (CurrentPlayer as AIPlayer).GetPlayerChoice(m_GameLogic.GetAllValidTilesForChoice(), m_GameLogic.GetBoardToDraw());
                Image boardSlotValue = this.m_GameLogic.Flipped(choiceOfAI.Location, k_FlippedTheCard);
                m_GameForm.FlippCardToFaceUp(choiceOfAI.Location, boardSlotValue);
                r_SelectedTileInTurn.Add(choiceOfAI.Location);
            }

            InbetweenTurnsTimer.Start();
        }

        protected virtual void GameEngine_GameEnding()
        {
            bool wantRematch = showWinner();
            if (wantRematch)
            {
                foreach(Player player in r_AllPlayersInGame)
                {
                    if (!player.IsHuman)
                    {
                        (player as AIPlayer).RestartNewGame();
                    }
                }

                restartNewGame(m_GameLogic.Rows, m_GameLogic.Columns);
            }
            else
            {
                m_GameForm.Close();
            }
        }
    }
}
