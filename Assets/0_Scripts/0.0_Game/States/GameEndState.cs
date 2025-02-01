using Patterns;


namespace Game.State
{
    public class GameEndState : State<MiniGame>
    {
        public GameEndState(MiniGame context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _context.OnMinigameEndEnter();
           
        }

        public override void Exit()
        {
            base.Exit();

            _context.OnMinigameEndExit();
        }


    }
}

