using UnityEngine;
using UnityEditor;
[CustomPropertyDrawer(typeof(ResponseCurve), true)]
public class ResponseCurveDrawer : PropertyDrawer
{
  Editor curveEditor;
  // bool editCurve = false;
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    EditorGUI.BeginProperty(position, label, property);
    EditorGUI.PropertyField(position, property, label, true);
    // causes error if the editor is open when the object is destroyed.
    // if (property.objectReferenceValue != null)
    // {
    //   editCurve = EditorGUILayout.Foldout(editCurve, "Edit Curve");
    //   if (curveEditor == null)
    //   {
    //     curveEditor = Editor.CreateEditor(property.objectReferenceValue);
    //   }
    //   if (editCurve)
    //   {
    //     curveEditor.OnInspectorGUI();
    //   }
    // }
    EditorGUI.EndProperty();
  }

  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {
    return EditorGUI.GetPropertyHeight(property);
  }
}