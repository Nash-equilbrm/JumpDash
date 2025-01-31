using Game.UI;
using Patterns;
using System;
using UI;

namespace Game.State
{
    public class GameplayState : State<GameManager>
    {
        public GameplayState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _context.Register(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
            _context.Register(EventID.HitBlock, OnHitBlock);
            UIManager.Instance.ShowScreen<GameplayScreen>(forceShowData: true);
        }

        public override void Exit()
        {
            base.Exit();
            _context.Unregister(EventID.PlayerFinishMovement, OnPlayerFinishMovement);
            _context.Unregister(EventID.HitBlock, OnHitBlock);
        }

        private void OnHitBlock(object obj)
        {
            _context.ChangeState(GameManager.GameState.EndGameplay);
        }

        private void OnPlayerFinishMovement(object obj)
        {
            _context.CurrentScore++;
            _context.Broadcast(EventID.GainPoint, _context.CurrentScore);

        }
    }

}
