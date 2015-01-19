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
        if (CurrentPositioner == null || Target == null) return;

        Camera.transform.position = CurrentPositioner.GetCameraWorldPosition(Camera, Target);
        Camera.transform.eulerAngles = CurrentPositioner.GetCameraWorldEulerAngles(Camera, Target);
    }
}
