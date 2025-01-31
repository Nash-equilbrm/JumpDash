using Commons;
using Game.UI;
using Patterns;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace Game.State
{
    public class GameEndState : State<GameManager>
    {
        public GameEndState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            UIManager.Instance.ShowPopup<ContinuePopup>(forceShowData: true);
            _context.Register(EventID.OnBackToMainMenuBtnClicked, OnBackToMainMenuClicked);
        }

        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.OnBackToMainMenuBtnClicked, OnBackToMainMenuClicked);
        }

        private void OnBackToMainMenuClicked(object obj)
        {
            _context.Player.SetActive(false);
            _context.GameData.bestScore = (_context.GameData.bestScore < _context.CurrentScore) ? _context.CurrentScore : _context.GameData.bestScore;
            _context.CurrentScore = 0;
            ObjectPooling.Instance.GetPool(Strings.Blocks).RecycleAll();
            Common.UnloadSceneAsync(_context, _context.GameData.currentLevel, () =>
            {
                _context.ChangeState(GameManager.GameState.MainMenu);
            });
        }
    }
}

