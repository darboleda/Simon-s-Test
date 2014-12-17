using UnityEngine;
using System.Collections;

using Canal.Engine.Physics;

namespace Canal.Unity.Platformer
{
    public class WallCollision : Behavior, IMovementLimiter<Vector2, WalkerModel>
    {
        public int SensorCount;
        public Vector2 Dimensions;
        public float Skin;
        public float Length;
        public float BottomOffset;

        public LayerMask SolidMask;

        #region ICollisionApplicator implementation
        public bool LimitMovement(ref Vector2 delta, ref WalkerModel model)
        {
            Vector2 finalDelta = delta;
            float bestXDelta = 0;

            float bestLeftDelta = float.PositiveInfinity;
            for (int i = 0, count = SensorCount; i < count; ++i)
            {
                float bestDelta = float.PositiveInfinity;
                float y = BottomOffset
                    + (i * ((Dimensions.y - BottomOffset) / (count - 1)));
                Vector3 rayOrigin = model.Transform.TransformModelPointToWorld(
                    new Vector2(Skin - Dimensions.x * 0.5f, y));
                Vector3 rayTarget = model.Transform.TransformModelPointToWorld(
                    new Vector2(-Dimensions.x * 0.5f - Length, y));
                Debug.DrawLine(rayOrigin, rayTarget, Color.red);

                foreach (RaycastHit hit in Physics.RaycastAll(rayOrigin, rayTarget - rayOrigin, (rayTarget - rayOrigin).magnitude))
                {
                    if (((1 << hit.collider.gameObject.layer) & SolidMask.value) == 0)
                    {
                        continue;
                    }

                    Vector2 hitPoint = model.Transform.TransformWorldPointToModel(hit.point) - finalDelta;
                    Vector2 hitDelta = new Vector2(-Dimensions.x * 0.5f, y) + model.Position - hitPoint;

                    if (hitDelta.x <= 0)
                    {
                        bestDelta = Mathf.Min(bestDelta, hitDelta.x);
                    }
                }

                if (!float.IsPositiveInfinity(bestDelta))
                {
                    bestLeftDelta = Mathf.Min(bestLeftDelta, bestDelta);
                    model.Collision.LeftCollision = true;
                }
            }

            float bestRightDelta = float.NegativeInfinity;
            for (int i = 0, count = SensorCount; i < count; ++i)
            {
                float bestDelta = float.NegativeInfinity;
                float y = BottomOffset
                    + (i * ((Dimensions.y - BottomOffset) / (count - 1)));
                Vector3 rayOrigin = model.Transform.TransformModelPointToWorld(
                    new Vector2(-Skin + Dimensions.x * 0.5f, y));
                Vector3 rayTarget = model.Transform.TransformModelPointToWorld(
                    new Vector2(Dimensions.x * 0.5f + Length, y));
                Debug.DrawLine(rayOrigin, rayTarget, Color.red);

                foreach (RaycastHit hit in Physics.RaycastAll(rayOrigin, rayTarget - rayOrigin, (rayTarget - rayOrigin).magnitude))
                {
                    if (((1 << hit.collider.gameObject.layer) & SolidMask.value) == 0)
                    {
                        continue;
                    }

                    Vector2 hitPoint = model.Transform.TransformWorldPointToModel(hit.point) - finalDelta;
                    Vector2 hitDelta = new Vector2(Dimensions.x * 0.5f, y) + model.Position - hitPoint;

                    if (hitDelta.x >= 0)
                    {
                        bestDelta = Mathf.Max(bestDelta, hitDelta.x);
                    }
                }

                if (!float.IsNegativeInfinity(bestDelta))
                {
                    bestRightDelta = Mathf.Max(bestRightDelta, bestDelta);
                    model.Collision.RightCollision = true;
                }
            }

            bestXDelta = (float.IsInfinity(bestRightDelta) && float.IsInfinity(bestLeftDelta) ? 0 : 
                Mathf.Abs(bestRightDelta) < Mathf.Abs(bestLeftDelta) ? bestRightDelta : bestLeftDelta);

            finalDelta.x -= bestXDelta;
            delta = finalDelta;

            return true;
        }

        #endregion
    }
}
