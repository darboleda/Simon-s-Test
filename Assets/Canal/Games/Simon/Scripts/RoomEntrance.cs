using UnityEngine;
using Canal.Unity;

public class RoomEntrance : Behavior
{
    [SerializeField]
    protected Vector2 position;

    public virtual Vector2 GetEntryPosition(Vector2 offsetFromExit)
    {
        return position + offsetFromExit;
    }
}
