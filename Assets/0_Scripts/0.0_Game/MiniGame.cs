using Game.State;
using Patterns;
using System;
using UnityEngine;

namespace Game
{
    public abstract class MiniGame : MonoBehaviour
    {
        public enum GameState
        {
            GameInit,
            Gameplay,
            GameEnd
        }

        [Header("Prefabs")]
        public GameObject playerPrefab;
        public GameObject Player { get; internal set; }

        

        private StateMachine<MiniGame> _stateMachine = new();
        private GameInitState _gameInitState;
        private GameplayState _gameplayState;
        private GameEndState _gameEndState;

        [field: SerializeField] public int CurrentScore { get; set; } = 0;


        void Start()
        {
            _gameInitState = new(this);
            _gameplayState = new(this);
            _gameEndState = new(this);

            _stateMachine.Initialize(_gameInitState);
        }


        internal void ChangeState(GameState state)
        {
            switch (state)
            {
                case GameState.GameInit:
                    {
                        _stateMachine.ChangeState(_gameInitState);
                        break;
                    }
                case GameState.Gameplay:
                    {
                        _stateMachine.ChangeState(_gameplayState);
                        break;
                    }
                case GameState.GameEnd:
                    {
                        _stateMachine.ChangeState(_gameEndState);
                        break;
                    }
            }
        }

        internal virtual void OnMinigameInitEnter()
        {
        }

        internal virtual void OnMinigameInitExit()
        {
        }

        internal virtual void OnMinigamePlayEnter()
        {
        }

        internal virtual void OnMinigamePlayExit()
        {
        }

        internal virtual void OnMinigameEndEnter()
        {
        }

        internal virtual void OnMinigameEndExit()
        {
        }
    }
}

