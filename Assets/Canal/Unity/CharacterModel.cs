using UnityEngine;

namespace Canal.Unity
{
    public abstract class CharacterCollisionState
    {
        protected CharacterCollisionState()
        {
            this.Reset();
        }

        public abstract void Reset();
    }

    public abstract class CharacterModel<TCollisionState> : MovingModel
        where TCollisionState : CharacterCollisionState
    {
        public abstract TCollisionState Collision { get; }
    }
}
