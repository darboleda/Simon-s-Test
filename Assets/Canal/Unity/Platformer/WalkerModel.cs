using UnityEngine;

namespace Canal.Unity.Platformer
{
    public class WalkerModel : CharacterModel<WalkerModel.CollisionState>
    {
        public class CollisionState : CharacterCollisionState
        {
            public enum FloorCollision
            {
                None,
                Solid,
                Platform
            }

            public bool TopCollision, RightCollision, LeftCollision;
            public FloorCollision BottomCollision;

            public bool WasOnGround;
            public bool DisableGroundCling;
            public bool ShouldClingToGround { get { return WasOnGround && !DisableGroundCling; } }
            public bool OnGround { get { return BottomCollision != FloorCollision.None; } }

            public override void Reset()
            {
                TopCollision = RightCollision = LeftCollision = false;
                BottomCollision = FloorCollision.None;
            }
        }

        private CollisionState collision = new CollisionState();
        public override CollisionState Collision
        {
            get
            {
                return collision;
            }
        }

        public float FrictionMultiplier = 1;
        public float GravityMultiplier = 1;

        public float KineticFriction;
        public float WalkAcceleration;
        public float MaxWalkSpeed;
        public float JumpHeight;
        public float DoubleJumpHeight;
        public float Gravity;
        public float MaxFallSpeed;

        public bool FacingRight = true;
    }
}

