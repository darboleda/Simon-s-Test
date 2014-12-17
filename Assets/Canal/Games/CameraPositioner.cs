using UnityEngine;
using System.Collections;

using Canal.Unity;

public abstract class CameraPositioner : Behavior {

    public abstract Vector3 GetCameraWorldPosition(Camera camera);
    public abstract Vector3 GetCameraWorldEulerAngles(Camera camera);
}
