using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ResponseCurveValues))]
public class ResponseCurveValuesPropertyDrawer : PropertyDrawer
{
  SerializedProperty m;
  SerializedProperty k;
  SerializedProperty v;
  SerializedProperty h;
  SerializedProperty r;
  string MLabel;
  string KLabel;
  string HLabel;
  string VLabel;
  string RLabel;

  private float DrawProperty(Rect r, SerializedProperty property, string label, float perPropheight)
  {
    if (property != null && label != null && label != "")
    {
      EditorGUI.PropertyField(r, property, new GUIContent(label));
      return perPropheight + 2;
    }
    return 0;
  }

  private float DrawPropertyX(Rect r, SerializedProperty property, string label, float perPropWidth)
  {
    if (property != null && label != null && label != "")
    {
      r.height = r.height / 2;
      EditorGUI.LabelField(r, label);
      r.y += r.height;
      r.width -= 4;
      // r.width /= 1.5f;
      float a = EditorGUIUtility.labelWidth;
      EditorGUIUtility.labelWidth = 25f;
      EditorGUI.PropertyField(r, property, new GUIContent(label.Substring(0, 2)));
      EditorGUIUtility.labelWidth = a;
      // property.floatValue = EditorGUI.FloatField(r, property.floatValue);
      // EditorGUI.PropertyField(r, property, new GUIContent(label));
      return perPropWidth;
    }
    return 0;
  }

  int numberOfValidProperties = 0;
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    float perPropHeight = ((position.height - 1 * numberOfValidProperties) / numberOfValidProperties);
    // float perPropWidth = (position.width / numberOfValidProperties);
    Rect perPropRect = new Rect(position.x, position.y, position.width, perPropHeight);
    // Rect perPropRect = new Rect(position.x, position.y, position.width / numberOfValidProperties, position.height);
    perPropRect.y += DrawProperty(perPropRect, m, MLabel, perPropHeight);
    perPropRect.y += DrawProperty(perPropRect, k, KLabel, perPropHeight);
    perPropRect.y += DrawProperty(perPropRect, v, VLabel, perPropHeight);
    perPropRect.y += DrawProperty(perPropRect, h, HLabel, perPropHeight);
    perPropRect.y += DrawProperty(perPropRect, r, RLabel, perPropHeight);

    // perPropRect.x += DrawPropertyX(perPropRect, m, MLabel, perPropWidth);
    // perPropRect.x += DrawPropertyX(perPropRect, k, KLabel, perPropWidth);
    // perPropRect.x += DrawPropertyX(perPropRect, v, VLabel, perPropWidth);
    // perPropRect.x += DrawPropertyX(perPropRect, h, HLabel, perPropWidth);
    // perPropRect.x += DrawPropertyX(perPropRect, r, RLabel, perPropWidth);
  }

  SerializedProperty curveProp;
  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {

    m = FindIfNull(m, property, "m");
    k = FindIfNull(k, property, "k");
    v = FindIfNull(v, property, "v");
    h = FindIfNull(h, property, "h");
    r = FindIfNull(r, property, "r");

    curveProp = FindIfNull(curveProp, property, "curveType"); ;
    SetPropertyStrings((CurveType)curveProp.enumValueIndex);

    numberOfValidProperties = 0;
    if (m != null && MLabel != null && MLabel != "") { numberOfValidProperties++; }
    if (k != null && KLabel != null && KLabel != "") { numberOfValidProperties++; }
    if (v != null && VLabel != null && VLabel != "") { numberOfValidProperties++; }
    if (h != null && HLabel != null && HLabel != "") { numberOfValidProperties++; }
    if (r != null && RLabel != null && RLabel != "") { numberOfValidProperties++; }


    return (2 + EditorGUIUtility.singleLineHeight) * numberOfValidProperties;
    // return (2 + EditorGUIUtility.singleLineHeight) * 2;
  }


  private SerializedProperty FindIfNull(SerializedProperty prop, SerializedProperty root, string findString)
  {
    if (prop == null)
    {
      prop = root.FindPropertyRelative(findString);
    }
    return prop;
  }


  public void SetPropertyStrings(CurveType curve)
  {
    if (curve == CurveType.Linear)
    {
      MLabel = "M: Slope";
      VLabel = "V: Vertical Shift";
      HLabel = "H: Horizontal Shift";
    }
    else if (curve == CurveType.Gaussian)
    {
      MLabel = "M: Width of Peak";
      KLabel = "K: Height of Peak";
      VLabel = "V: Vertical Shift";
      HLabel = "H: Center of Peak Position";
    }
    else if (curve == CurveType.Polynomial)
    {
      MLabel = "M: Slope (or Y @ 1.0)";
      KLabel = "K: Exponent";
      VLabel = "V: Vertical Shift";
      HLabel = "H: Horizontal Shift";
    }
    else if (curve == CurveType.Logit)
    {
      MLabel = "M: Slope at Inflection Point";
      KLabel = "K: Horizontal Size of Curve";
      VLabel = "V: Vertical Shift";
      HLabel = "H: Horizontal Shift";
    }
    else if (curve == CurveType.Logistic)
    {
      MLabel = "M: Slope at Inflection";
      KLabel = "K: Vertical size of curve";
      VLabel = "V: Vertical Shift";
      HLabel = "H: Horizontal Shift";
    }
    else if (curve == CurveType.Sin || curve == CurveType.Tan)
    {
      MLabel = "M: Frequency";
      KLabel = "K: Amplitude";
      VLabel = "V: Vertical Shift";
      HLabel = "H: Horizontal Shift";
    }
    else if (curve == CurveType.ReflectedLinear)
    {
      MLabel = "M: Slope";
      VLabel = "V: Vertical Shift";
      HLabel = "H: Horizontal Shift";
    }
    else if (curve == CurveType.ReflectedPolynomial)
    {
      MLabel = "M: Slope";
      KLabel = "K: Exponent";
      VLabel = "V: Vertical Shift";
      HLabel = "H: Horizontal Shift";
      RLabel = "R: Reflected Point";
    }
    else if (curve == CurveType.Step)
    {
      MLabel = "M: Step Value";
      KLabel = "K: Start Step";
      VLabel = "V: End Step";
      HLabel = "H: Start Step 2";
      RLabel = "R: End Step 2";
    }
    else if (curve == CurveType.Exponential)
    {
      MLabel = "M: e^(mx)";
      KLabel = "K: k*e^(mx)";
      HLabel = "H: Horizontal Shift";
      VLabel = "V: Vertical Shift";
    }
    else if (curve == CurveType.Logarithmic)
    {
      MLabel = "M: log(m*x) ";
      KLabel = "K: k*log(m*x)";
      HLabel = "H: Horizontal Shift";
      VLabel = "V: Vertical Shift";
    }
    else
    {
    }
  }
}

