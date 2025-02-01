using Patterns;

namespace Game.State
{
    public class GameplayState : State<MiniGame>
    {
        public GameplayState(MiniGame context) : base(context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            _context.OnMinigamePlayEnter();


        }

        public override void Exit()
        {
            base.Exit();

            _context.OnMinigamePlayExit();
        }
    }

}
