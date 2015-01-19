using UnityEngine;
using System.Collections;

using Canal.Unity;
using Canal.Unity.Platformer;
using Canal.Unity.Framework;

public class RoomLevel : Level
{
    public RoomModel Room;
    public CameraController PrimaryCamera;

    public void SetPlayerCharacter(PositionModel character, int entranceIndex, Vector2 offset)
    {
        character.Position = Room.Entrances[entranceIndex].GetEntryPosition(offset);
        character.Transform.Translate(Vector2.zero);

        CameraController cameraController = PrimaryCamera;
        cameraController.Target = character.transform;
        cameraController.CurrentPositioner = Room.DefaultCameraPositioner;
        character.transform.parent = Room.transform;

        cameraController.UpdateCamera();
    }

    public override void OnLoaded()
    {
        base.OnLoaded();
    }
}
