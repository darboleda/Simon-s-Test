using UnityEngine;

namespace Canal.Unity
{
    public static class UnityExtensions
    {
        public static void ResetTransform(this GameObject gameObject)
        {
            gameObject.transform.Reset();
        }

        public static void Reset(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localEulerAngles = Vector3.zero;
        }

        public static void ParentAndResetTransform(this GameObject gameObject, Transform parent)
        {
            gameObject.transform.ParentAndReset(parent);
        }

        public static void ParentAndReset(this Transform transform, Transform parent)
        {
            transform.parent = parent;
            transform.Reset();
        }
    }
}

