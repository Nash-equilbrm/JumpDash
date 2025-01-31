using Commons;
using Patterns;
using UnityEngine;

namespace Game.State
{
    public class InitState : State<GameManager>
    {
        public InitState(GameManager context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            InstantitateGameObjects();
            _context.Broadcast(EventID.InitGamePlay);

            _context.ChangeState(GameManager.GameState.Gameplay);
        }

        public override void Exit()
        {
            base.Exit();
        }


        private void InstantitateGameObjects()
        {
            if(_context.Player == null)
            {
                _context.Player = Object.Instantiate(_context.playerPrefab, _context.World.transform);
            }
            else
            {
                _context.Player.SetActive(true);
            }
            ObjectPooling.Instance.GetPool(Strings.Blocks).Prepare(10);
        }
    }
}

