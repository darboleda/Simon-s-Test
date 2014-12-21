using UnityEngine;

namespace Canal.Unity.States
{
    public class GameStateBehavior : Behavior
    {
        private GameStateMachine internalStateMachine;
        private GameStateMachine StateMachine
        {
            get
            {
                internalStateMachine = internalStateMachine ?? GameObject.FindObjectOfType<GameStateMachine>();
                return internalStateMachine;
            }
        }

        public void RequestState(string stateId)
        {
            StateMachine.RequestState(stateId);
        }

        public T RequestFeature<T>(string featureId)
        {
            return StateMachine.RequestFeature<T>(featureId);
        }
    }
}
