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
            MiniGame,
        }




        [Header("Hierachy")]
        public GameObject Managers;
        public GameObject UI;
        public GameObject Settings;
        public GameObject World;



        #region Configs
        [Header("Config files")]
        public TextAsset gameConfigFile;
        [SerializeField] private GameConfig _gameConfig;
        public GameConfig GameConfig { get => _gameConfig; internal set => _gameConfig = value; }

        public TextAsset gameDataFile;
        [SerializeField] private GameData _gameData;
        public GameData GameData { get => _gameData; internal set => _gameData = value; }

        public int CurrentGame { get; private set; } = 2;
        #endregion


        #region States
        private LoadGameDataState _loadGameState;
        private MainMenuState _mainMenuState;
        private MiniGameState _miniGameState;
        #endregion

        private StateMachine<GameManager> _stateMachine = new();



        private void Start()
        {
            _loadGameState = new(this);
            _mainMenuState = new(this);
            _miniGameState = new(this);

            _stateMachine.Initialize(_loadGameState);
        }

        internal void ChangeState(GameState state)
        {
            switch (state) 
            {
                case GameState.LoadGame:
                    {
                        _stateMachine.ChangeState(_loadGameState);
                        break;
                    }
                case GameState.MainMenu:
                    {
                        _stateMachine.ChangeState(_mainMenuState);
                        break;
                    }
                case GameState.MiniGame:
                    {
                        _stateMachine.ChangeState(_miniGameState);
                        break;
                    }
            }
        }
    }
}
