using Patterns;
using UI;
using Commons;
using UnityEngine.SceneManagement;

namespace Game.State
{
    public class MainMenuState : State<GameManager>
    {
        public MainMenuState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            UIManager.Instance.ShowScreen<MainMenuScreen>(forceShowData: true);
            _context.Register(EventID.OnPlayBtnClicked, OnPlayBtnClicked);
        }

        public override void Exit()
        {
            base.Exit();

            _context.Unregister(EventID.OnPlayBtnClicked, OnPlayBtnClicked);
        }

        private void OnPlayBtnClicked(object obj)
        {
            if (obj is not int gameIndex) return;
            _context.CurrentGame = gameIndex;
            _context.ChangeState(GameManager.GameState.MiniGame);
        }

    }
}
