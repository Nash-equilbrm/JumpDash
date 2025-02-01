using Patterns;

namespace Game.State
{
    public class GameInitState : State<MiniGame>
    {
        public GameInitState(MiniGame context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _context.OnMinigameInitEnter();
        }

        public override void Exit()
        {
            base.Exit();

            _context.OnMinigameInitExit();
        }


      
    }
}

