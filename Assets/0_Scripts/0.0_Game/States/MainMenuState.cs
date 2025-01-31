using Patterns;
using UI;
using Commons;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;

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
            Common.LoadSceneAsync(_context,
                sceneIndex: _context.GameData.currentLevel,
                loadSceneMode: LoadSceneMode.Additive,
                () =>
                {
                    _context.ChangeState(GameManager.GameState.InitGameplay);
                });
        }

    }
}
