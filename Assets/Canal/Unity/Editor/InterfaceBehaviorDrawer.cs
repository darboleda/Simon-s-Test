using UnityEditor;
using UnityEngine;

namespace Canal.Unity.Editor
{
    [CustomPropertyDrawer(typeof(InterfaceBehaviorAttribute))]
    public class InterfaceBehaviorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            InterfaceBehaviorAttribute ib = attribute as InterfaceBehaviorAttribute;
            if (ib == null)
                return;
                
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, label);
            {
                property.objectReferenceValue =
                    InterfaceBehaviorField(position, ib, property.objectReferenceValue, !EditorUtility.IsPersistent(property.serializedObject.targetObject));
            }
            EditorGUI.EndProperty();
        }

        private Object InterfaceBehaviorField(Rect position, InterfaceBehaviorAttribute ib, Object o, bool allowSceneObjects)
        {
            Rect rect = new Rect(position);;
            position.height *= 0.5f;
            rect.height *= 0.5f;
            rect.y += position.height;
                
            EditorGUI.LabelField(position,
                (o != null ? string.Format("{0}", o.GetType().Name) : "No Reference"));
            Object assigned = EditorGUI.ObjectField(rect, o, typeof(Behavior), allowSceneObjects);

            if (ib.IsValidTarget(assigned as Behavior))
            {
                return assigned;
            }
            if (ib.IsValidTarget(o as Behavior))
            {
                return o;
            }
            return null;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 2;
        }
    }
}
