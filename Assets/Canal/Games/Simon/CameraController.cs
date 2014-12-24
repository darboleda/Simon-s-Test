using UnityEngine;
using System.Collections;

using Canal.Unity;

public class CameraController : Behavior
{
	public Camera Camera;	
    public Transform Target;
    public CameraPositioner CurrentPositioner;
	
    public void LateUpdate()
	{
        UpdateCamera();
	}

    public void UpdateCamera()
    {
        Camera.transform.position = CurrentPositioner.GetCameraWorldPosition(Camera, Target);
        Camera.transform.eulerAngles = CurrentPositioner.GetCameraWorldEulerAngles(Camera, Target);
    }
}
