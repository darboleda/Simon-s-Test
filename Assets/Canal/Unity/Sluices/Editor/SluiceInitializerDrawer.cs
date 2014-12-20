using UnityEngine;
using System.Collections;

using UnityEditor;

namespace Canal.Unity.Sluices.Editor
{
    [CustomPropertyDrawer(typeof(SluiceManager.SluiceInitializer))]
    public class SluiceInitializerDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            SerializedProperty id = property.FindPropertyRelative("Id");
            SerializedProperty scale = property.FindPropertyRelative("TimeScale");
            SerializedProperty paused = property.FindPropertyRelative("Paused");
            SerializedProperty groupness = property.FindPropertyRelative("IsGroup");

            SerializedProperty parent = property.FindPropertyRelative("Parent");
            SerializedProperty priority = property.FindPropertyRelative("Priority");

            position.height /= 3;
            id.stringValue = EditorGUI.TextField(new Rect(position.xMin, position.yMin, position.width - 10, position.height), id.stringValue);
            position.y += position.height;
            position.x += 20;
            position.width -= 20;
            EditorGUI.LabelField(new Rect(position.xMin, position.yMin, 100, position.height), "Parent");
            parent.stringValue = EditorGUI.TextField(
                new Rect(position.xMin + 50, position.yMin, position.width - 60, position.height), parent.stringValue);


            position.y += position.height;
            EditorGUI.LabelField(
                new Rect(position.xMin, position.yMin, 100, position.height), "Priority");
            priority.intValue = EditorGUI.IntField(
                new Rect(position.xMin + 50, position.yMin, 50, position.height), priority.intValue);
            EditorGUI.LabelField(new Rect(position.xMin + 100, position.yMin, 100, position.height), "Time Scale");
            scale.floatValue = EditorGUI.FloatField(
                new Rect(position.xMin + 175, position.yMin, 50, position.height), scale.floatValue);
            EditorGUI.LabelField(new Rect(position.xMin + 250, position.yMin, 100, position.height), "Paused");
            paused.boolValue = EditorGUI.Toggle(
                new Rect(position.xMin + 300, position.yMin, 25, position.height), paused.boolValue);
            EditorGUI.LabelField(
                new Rect(position.xMin + 325, position.yMin, 100, position.height), "Is Group");
            groupness.boolValue = EditorGUI.Toggle(
                new Rect(position.xMin + 380, position.yMin, 25, position.height), groupness.boolValue);


            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label) * 3;
        }
    }
}