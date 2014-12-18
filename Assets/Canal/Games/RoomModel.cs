using UnityEngine;
using System.Collections;

using Canal.Unity;

public class RoomModel : Behavior
{
    public CameraPositioner DefaultCameraPositioner;

    public RoomEntrance[] Entrances;
    public RoomExit[] Exits;

    public Vector2 DefaultSpawnPosition
    {
        get
        {
            return (Entrances.Length > 0 ? Entrances[0].GetEntryPosition(Vector2.zero) : Vector2.zero);
        }
    }
}
