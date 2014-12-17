using UnityEngine;
using System.Collections;

using Canal.Engine.Physics;

namespace Canal.Unity.Platformer
{
    public class FloorCollision : Behavior, IMovementLimiter<Vector2, WalkerModel>
    {
        public int SensorCount;
        public Vector2 Dimensions;
        public float Offset;
        public float Skin;
        public float Length;

        public LayerMask SolidMask;
        public LayerMask PlatformMask;

        #region ICollisionApplicator implementation
        public bool LimitMovement(ref Vector2 delta, ref WalkerModel model)
        {
            Vector2 finalDelta = delta;

            // Check floor collisions
            float bestYDelta = float.PositiveInfinity;
            WalkerModel.CollisionState.FloorCollision bestCollision = WalkerModel.CollisionState.FloorCollision.None;

            float dropThreshold = (1 / model.MaxFallSpeed) *
                -(model.Velocity.y <= 0 ? model.Velocity.y - 0.01f : 0.0f);

            for (int i = 0, count = SensorCount; i < count; ++i)
            {
                float bestLowDelta = float.PositiveInfinity;
                WalkerModel.CollisionState.FloorCollision currentBestCollision = WalkerModel.CollisionState.FloorCollision.None;

                float x = (0.5f + i - (count * 0.5f)) 
                    * ((Dimensions.x - Offset * 2) / (count - 1));

                Vector3 rayOrigin = model.Transform.TransformModelPointToWorld(
                    new Vector2(x, Skin) + finalDelta);
                Vector3 rayTarget = model.Transform.TransformModelPointToWorld(
                    new Vector2(x, -Length) + finalDelta);

                Debug.DrawLine(rayOrigin, rayTarget);

                foreach (RaycastHit hit in Physics.RaycastAll(rayOrigin, rayTarget - rayOrigin, (rayTarget - rayOrigin).magnitude))
                {
                    if (((1 << hit.collider.gameObject.layer) & (SolidMask.value | PlatformMask.value)) == 0)
                    {
                        continue;
                    }

                    WalkerModel.CollisionState.FloorCollision currentCollision =
                        (((1 << hit.collider.gameObject.layer) & PlatformMask.value) == 0 ? WalkerModel.CollisionState.FloorCollision.Solid : WalkerModel.CollisionState.FloorCollision.Platform);

                    Vector2 hitPoint = model.Transform.TransformWorldPointToModel(hit.point) - finalDelta;
                    Vector2 hitDelta = new Vector2(x, 0) + model.Position - hitPoint;

                    if (hitDelta.y <= 0 || (model.Collision.ShouldClingToGround && delta.y <= 0))
                    {
                        if ((finalDelta.y - hitDelta.y) <= dropThreshold ||
                            (model.Collision.ShouldClingToGround && delta.y <= 0 && 
                                ((1 << hit.collider.gameObject.layer & (SolidMask.value)) != 0)))
                        {
                            if (i == 0 || i == count - 1)
                            {
                                Vector2 hitNormal = model.Transform.TransformWorldPointToModel(hit.point + hit.normal)
                                    - model.Transform.TransformWorldPointToModel(hit.point);
                                Vector2 horizontal = (i == 0 ? -1 : 1) * Vector2.right;
                                //Debug.DrawLine(hit.point, hit.normal * 2 + hit.point, Color.cyan, 0.5f);
                                float angle = Vector2.Angle(-horizontal, hitNormal);
                                if (angle <= 95 && angle >= 30)
                                {
                                    if (hitDelta.y < bestLowDelta)
                                    {
                                        bestLowDelta = hitDelta.y;
                                        currentBestCollision = currentCollision;
                                    }
                                    else if (hitDelta.y == bestLowDelta && currentBestCollision == WalkerModel.CollisionState.FloorCollision.Platform)
                                    {
                                        currentBestCollision = currentCollision;
                                    }
                                }
                            }
                            else
                            {
                                Vector2 hitNormal = model.Transform.TransformWorldPointToModel(hit.point + hit.normal)
                                    - model.Transform.TransformWorldPointToModel(hit.point);

                                if (Vector2.Angle(Vector2.up, hitNormal) == 0)
                                {
                                    if (hitDelta.y < bestLowDelta)
                                    {
                                        bestLowDelta = hitDelta.y;
                                        currentBestCollision = currentCollision;
                                    }
                                    else if (hitDelta.y == bestLowDelta && currentBestCollision == WalkerModel.CollisionState.FloorCollision.Platform)
                                    {
                                        currentBestCollision = currentCollision;
                                    }
                                }
                            }
                        }
                        else if (model.Collision.WasOnGround && delta.y <= 0)
                        {
                            if (i == 0 || i == count - 1)
                            {
                                Vector2 hitNormal = model.Transform.TransformWorldPointToModel(hit.point + hit.normal)
                                    - model.Transform.TransformWorldPointToModel(hit.point);
                                Vector2 horizontal = (i == 0 ? -1 : 1) * Vector2.right;
                                //Debug.DrawLine(hit.point, hit.normal * 2 + hit.point, Color.magenta, 0.5f);
                                if (Vector2.Angle(-horizontal, hitNormal) <= 95)
                                {
                                    Vector3 pointPoint = model.Transform.TransformModelPointToWorld(new Vector2(x, 0));
                                    Debug.DrawRay(pointPoint, hit.point - pointPoint, Color.yellow, 0.5f);

                                    Vector2 testFinal = finalDelta;
                                    testFinal.y -= hitDelta.y;

                                    if (testFinal.sqrMagnitude > 0 && Vector2.Angle(horizontal, testFinal) <= 60)
                                    {
                                        if (hitDelta.y < bestLowDelta)
                                        {
                                            bestLowDelta = hitDelta.y;
                                            currentBestCollision = currentCollision;
                                        }
                                        else if (hitDelta.y == bestLowDelta && currentBestCollision == WalkerModel.CollisionState.FloorCollision.Platform)
                                        {
                                            currentBestCollision = currentCollision;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.DrawLine(hit.point, hit.normal * 2 + hit.point, Color.green, 0.5f);
                        }
                    }
                }

                if (bestLowDelta < bestYDelta)
                {
                    bestYDelta = bestLowDelta;
                    bestCollision = currentBestCollision;
                }
                else if (bestLowDelta == bestYDelta && bestCollision == WalkerModel.CollisionState.FloorCollision.Platform)
                {
                    bestCollision = currentBestCollision;
                }
            }

            if (!float.IsPositiveInfinity(bestYDelta))
            {
                finalDelta.y -= bestYDelta;
                model.Collision.BottomCollision = bestCollision;
            }

            delta = finalDelta;
            return true;
        }
        #endregion
    }
}
