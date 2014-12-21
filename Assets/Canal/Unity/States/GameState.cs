using System;
using Canal.Unity.Modes;

namespace Canal.Unity.States
{
    public class GameState : Behavior
    {
        public ModeConfiguration Configuration;

        public void Enter(GameStateMachine machine, GameState previousState)
        {
            this.OnEntered(previousState);
        }

        public void Exit(GameStateMachine machine, GameState nextState)
        {
            this.OnExited(nextState);
        }

        protected virtual void OnEntered(GameState previousState) { }
        protected virtual void OnExited(GameState nextState) { }
    }
}
