using UnityEngine;
using System.Collections;

using Canal.Unity;

public class CameraController : Behavior
{
	public new Camera camera;	
    public CameraPositioner currentPositioner;
	
    public void LateUpdate()
	{
        camera.transform.position = currentPositioner.GetCameraWorldPosition(camera);
        camera.transform.eulerAngles = currentPositioner.GetCameraWorldEulerAngles(camera);
	}
}
