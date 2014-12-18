using UnityEngine;

using Canal.Unity;

public abstract class RoomExit : Behavior
{
    public string TargetRoomId;
    public int TargetRoomEntranceIndex;

    public abstract bool CheckExit(PositionModel model, out Vector2 offset);
}
