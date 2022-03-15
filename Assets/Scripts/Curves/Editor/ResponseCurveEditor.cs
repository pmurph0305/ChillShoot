using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(ResponseCurve), true)]
public class ResponseCurveEditor : Editor
{
  SerializedProperty m;
  SerializedProperty k;
  SerializedProperty v;
  SerializedProperty h;
  SerializedProperty r;

  SerializedProperty values;
  SerializedProperty presetValues;
  SerializedProperty presetLabels;
  SerializedProperty curvePropery;
  IResponseCurve curve;
  Material material;
  public List<float> yValues = new List<float>();
  public float resolution = 0.001f;

  private float lineCount = 50;

  string TryFindLabel(string propName)
  {
    SerializedProperty p = serializedObject.FindProperty(propName);
    if (p != null)
    {
      return p.stringValue;
    }
    return "";
  }
  void OnEnable()
  {
    var shader = Shader.Find("Hidden/Internal-Colored");
    material = new Material(shader);
    curve = target as ResponseCurve;

    values = serializedObject.FindProperty("values");
    presetValues = serializedObject.FindProperty("presetValues");
    presetLabels = serializedObject.FindProperty("presetLabels");

    m = values.FindPropertyRelative("m");
    if (!(curve is LinearResponseCurve))
    {
      k = values.FindPropertyRelative("k");
    }
    v = values.FindPropertyRelative("v");
    h = values.FindPropertyRelative("h");
    r = values.FindPropertyRelative("r");
    mLabel = TryFindLabel("MLabel");
    kLabel = TryFindLabel("KLabel");
    hLabel = TryFindLabel("HLabel");
    vLabel = TryFindLabel("VLabel");
    rLabel = TryFindLabel("RLabel");
    CalculatePoints();
  }
  string mLabel;
  string kLabel;
  string hLabel;
  string vLabel;
  string rLabel;





  SerializedProperty currentPresetValues;
  SerializedProperty currentPresetLabel;
  public override void OnInspectorGUI()
  {
    if (curve == null) return;

    base.OnInspectorGUI();

    EditorGUI.BeginChangeCheck();

    if (presetLabels != null && presetValues != null)
    {
      EditorGUILayout.LabelField("Presets:");
      for (int i = 0; i < presetLabels.arraySize; i++)
      {
        currentPresetLabel = presetLabels.GetArrayElementAtIndex(i);
        if (currentPresetLabel != null)
        {
          if (GUILayout.Button(currentPresetLabel.stringValue))
          {
            currentPresetValues = presetValues.GetArrayElementAtIndex(i);
            SerializedProperty cm = currentPresetValues.FindPropertyRelative("m");
            SerializedProperty ck = currentPresetValues.FindPropertyRelative("k");
            SerializedProperty cv = currentPresetValues.FindPropertyRelative("v");
            SerializedProperty ch = currentPresetValues.FindPropertyRelative("h");
            SerializedProperty cr = currentPresetValues.FindPropertyRelative("r");
            SetFloatValueProperty(m, cm.floatValue);
            SetFloatValueProperty(k, ck.floatValue);
            SetFloatValueProperty(v, cv.floatValue);
            SetFloatValueProperty(h, ch.floatValue);
            SetFloatValueProperty(r, cr.floatValue);
            serializedObject.ApplyModifiedProperties();
          }
        }
      }
    }

    showCurve = EditorGUILayout.Foldout(showCurve, "Show Curve");
    if (showCurve)
    {
      CalculatePoints();
      DrawValues();
      // foreach (float x in yValues)
      // {
      //   EditorGUILayout.LabelField(x.ToString());
      // }
    }
    serializedObject.ApplyModifiedProperties();

  }

  bool showCurve = true;

  private void NonNullPropertyField(SerializedProperty property, string label)
  {
    if (property != null)
    {
      EditorGUILayout.PropertyField(property, new GUIContent(label));
    }
  }

  private void SetFloatValueProperty(SerializedProperty property, float f)
  {
    if (property != null)
    {
      property.floatValue = f;
    }
  }


  private void CalculatePoints()
  {
    yValues.Clear();
    for (float x = 0; x <= 1; x += resolution)
    {
      yValues.Add(curve.GetValue(x));
    }
  }


  public void DrawValues()
  {
    // Source from: https://gamedev.stackexchange.com/questions/141302/how-do-i-draw-lines-to-a-custom-inspector

    // Begin to draw a horizontal layout, using the helpBox EditorStyle
    GUILayout.BeginHorizontal();

    // create a square rect.
    Rect layoutRectangle = GUILayoutUtility.GetAspectRect(1);

    if (Event.current.type == EventType.Repaint)
    {
      // If we are currently in the Repaint event, begin to draw a clip of the size of 
      // previously reserved rectangle, and push the current matrix for drawing.
      GUI.BeginClip(layoutRectangle);
      GL.PushMatrix();

      // Clear the current render buffer, setting a new background colour, and set our
      // material for rendering.
      GL.Clear(true, false, Color.black);
      material.SetPass(0);

      // Start drawing in OpenGL Quads, to draw the background canvas. Set the
      // colour black as the current OpenGL drawing colour, and draw a quad covering
      // the dimensions of the layoutRectangle.
      GL.Begin(GL.QUADS);
      GL.Color(Color.black);
      GL.Vertex3(0, 0, 0);
      GL.Vertex3(layoutRectangle.width, 0, 0);
      GL.Vertex3(layoutRectangle.width, layoutRectangle.height, 0);
      GL.Vertex3(0, layoutRectangle.height, 0);
      GL.End();

      // Start drawing in OpenGL Lines, to draw the lines of the grid.
      GL.Begin(GL.LINES);
      // offset for each grid line.
      float gridLineOffset = layoutRectangle.width / lineCount;
      for (int i = 0; i < lineCount; i++)
      {
        // major/minor grid lines.
        Color lineColour = (i % 5 == 0 ? new Color(0.6f, 0.6f, 0.6f, 0.6f) : new Color(0.2f, 0.2f, 0.2f, 0.2f));
        float linePos = i * gridLineOffset;
        GL.Color(lineColour);
        // draw horizontal and vertical lines.
        if (linePos >= 0 && linePos < layoutRectangle.width && linePos < layoutRectangle.height)
        {
          GL.Vertex3(linePos, 0, 0);
          GL.Vertex3(linePos, layoutRectangle.height, 0);
          GL.Vertex3(0, linePos, 0);
          GL.Vertex3(layoutRectangle.width, linePos, 0);
        }
      }

      Color lineColor = new Color(0, 1, 0);
      GL.Color(lineColor);
      // x and y pos for each section of line.
      float xPos = 0;
      float yPos = 0;
      float xPos2 = 0;
      float yPos2 = 0;
      for (int i = 0; i < yValues.Count - 1; i++)
      {
        xPos = i * resolution * layoutRectangle.width;
        yPos = layoutRectangle.height - yValues[i] * layoutRectangle.height;
        xPos2 = (i + 1) * resolution * layoutRectangle.width;
        yPos2 = layoutRectangle.height - yValues[i + 1] * layoutRectangle.height;
        if (xPos >= 0 && xPos <= layoutRectangle.width
        && xPos2 >= 0 && xPos2 <= layoutRectangle.width
        && yPos >= 0 && yPos <= layoutRectangle.height
        && yPos2 >= 0 && yPos2 <= layoutRectangle.height)
        {
          // draw line section
          GL.Vertex3(xPos, yPos, 0);
          GL.Vertex3(xPos2, yPos2, 0);
        }
      }
      // End lines drawing.
      GL.End();
      // Pop the current matrix for rendering, and end the drawing clip.
      GL.PopMatrix();
      GUI.EndClip();
    }

    // End our horizontal 
    GUILayout.EndHorizontal();
  }
}
