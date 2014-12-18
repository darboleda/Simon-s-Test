using UnityEngine;
using System.Collections;

using Canal.Unity;

public class RoomModel : Behavior
{
    public CameraPositioner DefaultCameraPositioner;

    public Vector2[] EntrancePositions;

    public RoomExit[] Exits;

    public Vector2 DefaultSpawnPosition
    {
        get
        {
            return (EntrancePositions.Length > 0 ? EntrancePositions[0] : Vector2.zero);
        }
    }
}
