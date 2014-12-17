using UnityEngine;
using System.Collections;

using Canal.Engine.Physics;
using Canal.Unity;

namespace Canal.Unity.Platformer
{
    public class MovingPhysics : Behavior
    {
        public MovingModel Model;

        public void FixedUpdate()
        {
            Vector2 acceleration = Model.Acceleration;
            Model.Velocity += acceleration * Time.deltaTime;
            Vector2 delta = Model.Velocity * Time.deltaTime;
            Model.Transform.Translate(delta);
        }
    }
}
