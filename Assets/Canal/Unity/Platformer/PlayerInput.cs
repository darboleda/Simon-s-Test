using UnityEngine;

using Canal.Unity;

namespace Canal.Unity.Platformer
{
    public class PlayerInput : Behavior
    {
        public WalkerModel Model;
        private bool startedJump = false;

        public void Update()
        {
            Model.Collision.DisableGroundCling = false;
            float movement = Input.GetAxisRaw("Horizontal");
            Model.Acceleration.x = movement * (Model.WalkAcceleration + Model.KineticFriction);

            if (Model.Collision.OnGround && Input.GetAxisRaw("Vertical") == -1)
            {
                Model.Acceleration.x = 0;
                Model.Velocity.x = 0;
            }

            if (Input.GetButtonDown("Jump"))
            {
                if (Model.Collision.OnGround)
                {
                    if (Input.GetAxisRaw("Vertical") == -1)
                    {
                        if (Model.Collision.BottomCollision == WalkerModel.CollisionState.FloorCollision.Platform)
                        {
                            Model.Velocity.x = 0;
                            Model.Velocity.y = -Mathf.Sqrt(Model.Gravity) * 0.5f;
                            Model.Transform.Translate(Vector2.up * Model.Velocity.y * 0.1f);
                        }
                    }
                    else
                    {
                        Model.Velocity.y = Mathf.Sqrt(Model.JumpHeight * 2 * Model.Gravity);
                    }
                }
                else
                {
                    Model.Velocity.y = Mathf.Sqrt(Model.DoubleJumpHeight * 2 * Model.Gravity);
                }
                startedJump = true;
            }

            if ((!Input.GetButton("Jump") && startedJump) && Model.Velocity.y > 0)
            {
                Model.Velocity.y *= 0.3f;
                startedJump = false;
            }
        }
    }
}

