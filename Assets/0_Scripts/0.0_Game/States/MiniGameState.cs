using Commons;
using Patterns;
using System;
using UnityEngine.SceneManagement;


namespace Game.State
{
    public class MiniGameState : State<GameManager>
    {
        public MiniGameState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _context.Register(EventID.ExitMiniGame, OnExitMiniGame);

            Common.LoadSceneAsync(_context,
                sceneIndex: _context.CurrentGame,
                loadSceneMode: LoadSceneMode.Additive,
                null);
        }

        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.ExitMiniGame, OnExitMiniGame);
        }

        private void OnExitMiniGame(object obj)
        {
            _context.ChangeState(GameManager.GameState.MainMenu);
            _context.Broadcast(EventID.UnloadMiniGame);
        }
    }
}

