using UnityEngine;

using Canal.Unity;

public class InPlaceEntrance : RoomEntrance
{
    public override Vector2 GetEntryPosition(Vector2 offsetFromExit)
    {
        return this.position;
    }
}
