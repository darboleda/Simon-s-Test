using UnityEngine;

namespace Canal.Unity
{
    public class PositionTransform : Behavior
    {
		public PositionModel Model;
        public float ZOverride = -1;

		public void Translate(Vector2 delta)
		{
			//Model.Position = TransformWorldPointToModel(Position);
			Vector3 newDelta = TransformModelPointToWorld(delta) - Position;
			Model.Position += delta;
			transform.Translate(newDelta, Space.World);

			Vector3 position = Position;
			position.x = (0.0625f * Mathf.FloorToInt(position.x * 16));
			position.y = (0.0625f * Mathf.FloorToInt(position.y * 16));
			transform.position = position;

            position = transform.localPosition;
            position.z = ZOverride;
            transform.localPosition = position;
		}

		public Vector3 Position
		{
			get { return transform.position; }
			set { transform.position = value; Model.Position = TransformWorldPointToModel(Position); }
		}

		public Vector3 TransformModelPointToWorld(Vector2 localPoint)
		{
			Vector3 position = new Vector3(localPoint.x + Model.Position.x,
				localPoint.y + Model.Position.y);
			Transform parent = transform.parent;
			if (parent != null)
			{
				return parent.TransformPoint(position);
			}

			return position;
		}

		public Vector2 TransformWorldPointToModel(Vector3 worldPoint)
		{
			Vector3 position = worldPoint;
			Transform parent = transform.parent;
			if (parent != null)
			{
				position = parent.InverseTransformPoint(worldPoint);
			}

			return (Vector2)position;
		}
    }
}