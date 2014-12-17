using UnityEngine;
using System.Collections;

using Canal.Engine.Physics;
using Canal.Unity;

namespace Canal.Unity.Platformer
{
    public class WalkerPhysics : Behavior
    {
        public WalkerModel Model;

        [InterfaceBehaviorAttribute(typeof(IMovementLimiter<Vector2, WalkerModel>))]
        public Behavior[] Collisions;

        public void FixedUpdate()
        {
            // Apply acceleration
            Vector2 acceleration = Model.Acceleration;
            if (Model.Velocity.x != 0 && Model.Collision.OnGround)
            {
                if (Mathf.Sign(Model.Acceleration.x) != Mathf.Sign(Model.Velocity.x))
                {
                    acceleration.x = 0;
                }
                else
                {
                    acceleration.x *= Model.FrictionMultiplier;
                }
            }
            Model.Velocity += acceleration * Time.deltaTime;

            if (!Model.Collision.OnGround)
            {
                Model.Velocity += Model.Gravity * Model.GravityMultiplier * -Vector2.up * Time.deltaTime;
            }

            if (Model.Velocity.y < -Model.MaxFallSpeed)
            {
                Model.Velocity.y = -Model.MaxFallSpeed;
            }

            if (Mathf.Abs(Model.Velocity.x) < 0.01f)
            {
                Model.Velocity.x = 0;
            }
            else
            {
                if (Model.Collision.OnGround)
                {
                    float frictionMultiplier = Mathf.Sign(Model.Velocity.x) * Model.KineticFriction * Model.FrictionMultiplier * Time.deltaTime;
                    if (Mathf.Abs(frictionMultiplier) >= Mathf.Abs(Model.Velocity.x))
                    {
                        Model.Velocity.x = 0;
                    }
                    else
                    {
                        Model.Velocity.x -= frictionMultiplier;
                    }
                }
                Model.Velocity.x = Mathf.Clamp(Model.Velocity.x, -Model.MaxWalkSpeed, Model.MaxWalkSpeed);
            }

            // Apply Velocity
            Vector2 delta = Model.Velocity * Time.deltaTime;
            Model.Collision.WasOnGround = Model.Collision.OnGround;
            Model.Collision.Reset();
            for (int i = 0, count = Collisions.Length; i < count; ++i)
            {
                ((IMovementLimiter<Vector2, WalkerModel>)Collisions[i]).LimitMovement(ref delta, ref Model);
            }

            if (Model.Collision.OnGround)
            {
                Model.Velocity.y = 0;
            }

            if ((Model.Collision.LeftCollision && Model.Velocity.x < 0)
                || (Model.Collision.RightCollision && Model.Velocity.x > 0))
            {
                Model.Velocity.x = 0;
            }

            Model.Transform.Translate(delta);
        }
    }
}