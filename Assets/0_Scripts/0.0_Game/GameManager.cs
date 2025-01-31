using Game.State;
using Patterns;
using System;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public enum GameState
        {
            LoadGame,
            MainMenu,
            InitGameplay,
            Gameplay,
            EndGameplay
        }

        [Header("Prefabs")]
        public GameObject playerPrefab;


        [Header("Hierachy")]
        public GameObject Managers;
        public GameObject UI;
        public GameObject Settings;
        public GameObject World;

        public GameObject Player { get; internal set; }

        #region Configs
        [Header("Config files")]
        public TextAsset levelDesignConfigFile;
        private LevelDesignConfig _levelDesignConfig;
        public LevelDesignConfig LevelDesignConfig { get => _levelDesignConfig; internal set => _levelDesignConfig = value; }

        public TextAsset gameDataFile;
        [SerializeField] private GameData _gameData;
        public GameData GameData { get => _gameData; internal set => _gameData = value; }
        #endregion


        #region States
        private LoadGameDataState _loadGameState;
        private MainMenuState _mainMenuState;
        private InitState _initGameplayState;
        private GameplayState _gameplayState;
        private GameEndState _gameEndState; 
        #endregion

        private StateMachine<GameManager> _stateMachine = new();


        [field: SerializeField] public int CurrentScore { get; set; } = 0;

        private void Start()
        {
            _loadGameState = new(this);
            _mainMenuState = new(this);
            _initGameplayState = new(this);
            _gameplayState = new(this);
            _gameEndState = new(this);

            _stateMachine.Initialize(_loadGameState);
        }

        internal void ChangeState(GameState state)
        {
            switch (state) 
            {
                case GameState.MainMenu:
                    {
                        _stateMachine.ChangeState(_mainMenuState);
                        break;
                    }
                case GameState.InitGameplay:
                    {
                        _stateMachine.ChangeState(_initGameplayState);
                        break;
                    }
                case GameState.Gameplay:
                    {
                        _stateMachine.ChangeState(_gameplayState);
                        break;
                    }
                case GameState.EndGameplay:
                    {
                        _stateMachine.ChangeState(_gameEndState);
                        break;
                    }
                case GameState.LoadGame:
                    {
                        _stateMachine.ChangeState(_loadGameState);
                        break;
                    }
            }
        }
    }
}
