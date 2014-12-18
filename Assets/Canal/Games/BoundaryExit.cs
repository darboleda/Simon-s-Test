using UnityEngine;
using System.Collections;

using Canal.Unity;

public class BoundaryExit : RoomExit {
    public enum ExitDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public ExitDirection Direction;
    public Vector2 Position;
    public float BoundaryMin;
    public float BoundaryMax;

    public override bool CheckExit(PositionModel model, out Vector2 offset)
    {
        offset = model.Position - Position;;
        switch (Direction)
        {
            case ExitDirection.Up:
                if (model.Position.y > Position.y
                    && model.Position.x > BoundaryMin
                    && model.Position.x < BoundaryMax)
                {

                    return true;
                };
                break;

            case ExitDirection.Down:
                if (model.Position.y < Position.y
                    && model.Position.x > BoundaryMin
                    && model.Position.x < BoundaryMax)
                {

                    return true;
                };
                break;

            case ExitDirection.Right:
                if (model.Position.x > Position.x
                    && model.Position.y > BoundaryMin
                    && model.Position.y < BoundaryMax)
                {
                    return true;
                };
                break;

            case ExitDirection.Left:
                if (model.Position.x < Position.x
                    && model.Position.y > BoundaryMin
                    && model.Position.y < BoundaryMax)
                {
                    return true;
                };
                break;
        }
        offset = Vector2.zero;
        return false;
    }
}
