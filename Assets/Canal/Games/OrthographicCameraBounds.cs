using UnityEngine;
using System.Collections;

using Canal.Unity;

public class OrthographicCameraBounds : CameraPositioner {

    public Transform target;

    public Vector2 Min;
    public Vector2 Max;

	public Vector2 Offset;

    public override Vector3 GetCameraWorldPosition(Camera camera)
    {
        Vector3 targetPos = this.transform.InverseTransformPoint(target.position);
        Vector3 finalPos = targetPos;
		finalPos.x += Offset.x;
		finalPos.y += Offset.y;
        finalPos.z = 0;

        if (camera.isOrthoGraphic)
        { 
            float halfHeight = camera.orthographicSize;
            float halfWidth = halfHeight * camera.aspect;

            finalPos.x = Mathf.Clamp(finalPos.x, Min.x + halfWidth, Max.x - halfWidth);
            finalPos.y = Mathf.Clamp(finalPos.y, Min.y + halfHeight, Max.y - halfHeight);

        }

        return this.transform.TransformPoint(finalPos);
    }

    public override Vector3 GetCameraWorldEulerAngles(Camera camera)
    {
        return this.transform.eulerAngles;
    }

}
