using UnityEngine;

using Canal.Unity;
using Canal.Unity.Platformer;

public class InPlaceExit : RoomExit {
    public Vector2 Position;
    public Vector2 Threshold;

	public override bool CheckExit(PositionModel model, out Vector2 offset)
    {
        Vector2 test = model.Position;
        offset = Vector2.zero;

        WalkerModel walker = model as WalkerModel;
        if (walker.Collision.OnGround
            && GetEnterRoomInputDown()
            && Between(test.x, Position.x - Threshold.x, Position.x + Threshold.x)
            && Between (test.y, Position.y - Threshold.y, Position.y + Threshold.y))
        {
            return true;
        }
        return false;
    }

    private bool Between(float value, float min, float max)
    {
        return (value > min && value < max);
    }
        
    private bool previousRoomInput;
    private bool roomInput;
    private bool GetEnterRoomInputDown()
    {
        return !previousRoomInput && roomInput;
    }

    public void Update()
    {
        previousRoomInput = roomInput;
        roomInput = Input.GetAxisRaw("Vertical") > 0.5f;
    }
}
