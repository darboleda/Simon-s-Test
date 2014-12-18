using UnityEngine;
using System.Collections;

using Canal.Unity;

public class OrthographicCameraBounds : CameraPositioner {

    public Vector2 Min;
    public Vector2 Max;

	public Vector3 Offset;

    public override Vector3 GetCameraWorldPosition(Camera camera, Transform target)
    {
        Vector3 targetPos = this.transform.InverseTransformPoint(target.position);
        Vector3 finalPos = targetPos;
		finalPos.x += Offset.x;
		finalPos.y += Offset.y;
        finalPos.z += Offset.z;

        if (camera.isOrthoGraphic)
        { 
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;

            finalPos.x = Mathf.Clamp(finalPos.x, Min.x + halfWidth, Max.x - halfWidth);
            finalPos.y = Mathf.Clamp(finalPos.y, Min.y + halfHeight, Max.y - halfHeight);

        }

        return this.transform.TransformPoint(finalPos);
    }

    public override Vector3 GetCameraWorldEulerAngles(Camera camera, Transform target)
    {
        return this.transform.eulerAngles;
    }

}
