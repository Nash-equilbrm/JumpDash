using Commons;
using DG.Tweening;
using Patterns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.State
{
    public class LoadGameDataState : State<GameManager>
    {
        public LoadGameDataState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;
            DOTween.Init();

            GetConfigAndData();
        }

        public override void Exit()
        {
            base.Exit();
        }

        private void GetConfigAndData()
        {
            _context.GameConfig = Common.ReadJson<GameConfig>(_context.gameConfigFile);
            _context.GameData = Common.ReadJson<GameData>(_context.gameDataFile);
            _context.ChangeState(GameManager.GameState.MainMenu);
        }
    }
}

