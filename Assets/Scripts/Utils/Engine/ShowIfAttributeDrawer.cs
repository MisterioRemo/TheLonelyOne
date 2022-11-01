using System.Reflection;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace TheLonelyOne
{
  [CustomPropertyDrawer(typeof(ShowIfAttribute), true)]
  public class ShowIfAttributeDrawer : PropertyDrawer
  {
    #region REFLECTION
    private static MethodInfo GetMethod(object _target, string _methodName)
    {
      return GetAllMethods(_target, m => m.Name
                                           .Equals(_methodName, StringComparison.InvariantCulture))
                                           .FirstOrDefault();
    }

    private static FieldInfo GetField(object _target, string _fieldName)
    {
      return GetAllFields(_target, f => f.Name
                                          .Equals(_fieldName, StringComparison.InvariantCulture))
                                          .FirstOrDefault();
    }
    private static IEnumerable<FieldInfo> GetAllFields(object _target, Func<FieldInfo, bool> _predicate)
    {
      List<Type> types = new List<Type>() { _target.GetType() };

      while (types.Last().BaseType != null)
        types.Add(types.Last().BaseType);

      for (int i = types.Count - 1; i >= 0; i--)
      {
        IEnumerable<FieldInfo> fieldInfos = types[i]
                                              .GetFields(BindingFlags.Instance | BindingFlags.Static
                                                         | BindingFlags.NonPublic | BindingFlags.Public
                                                         | BindingFlags.DeclaredOnly)
                                              .Where(_predicate);

        foreach (var fieldInfo in fieldInfos)
          yield return fieldInfo;
      }
    }

    private static IEnumerable<MethodInfo> GetAllMethods(object _target, Func<MethodInfo, bool> _predicate)
    {
      return _target
               .GetType()
               .GetMethods(BindingFlags.Instance | BindingFlags.Static
                           | BindingFlags.NonPublic | BindingFlags.Public)
               .Where(_predicate);
    }
    #endregion

    private bool MeetsConditions(SerializedProperty _property)
    {
      ShowIfAttribute showIfAttribute = this.attribute as ShowIfAttribute;
      object          target          = _property.serializedObject.targetObject;
      List<bool>      conditionValues = new List<bool>();

      foreach (var condition in showIfAttribute.Conditions)
      {
        FieldInfo conditionField = GetField(target, condition);
        if (conditionField != null && conditionField.FieldType == typeof(bool))
          conditionValues.Add((bool)conditionField.GetValue(target));

        MethodInfo conditionMethod = GetMethod(target, condition);
        if (conditionMethod != null
            && conditionMethod.ReturnType == typeof(bool)
            && conditionMethod.GetParameters().Length == 0)
        {
          conditionValues.Add((bool)conditionMethod.Invoke(target, null));
        }
      }

      if (conditionValues.Count > 0)
      {
        bool met;
        if (showIfAttribute.Operator == ConditionOperator.And)
        {
          met = true;
          foreach (var value in conditionValues)
            met = met && value;
        }
        else
        {
          met = false;
          foreach (var value in conditionValues)
            met = met || value;
        }

        return met;
      }
      else
      {
        Debug.LogError("Invalid boolean condition fields or methods used!");
        return true;
      }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      // Calcluate the property height, if we don't meet the condition and the draw mode is DontDraw, then height will be 0.
      ShowIfAttribute showIfAttribute = this.attribute as ShowIfAttribute;

      return (!MeetsConditions(property) && showIfAttribute.Action == ActionOnConditionFail.DontDraw)
              ? 0.0f
              : base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (MeetsConditions(property))
      {
        EditorGUI.PropertyField(position, property, label, true);
        return;
      }

      ShowIfAttribute showIfAttribute = this.attribute as ShowIfAttribute;
      if (showIfAttribute.Action == ActionOnConditionFail.DontDraw)
      {
        return;
      }
      else if (showIfAttribute.Action == ActionOnConditionFail.JustDisable)
      {
        EditorGUI.BeginDisabledGroup(true);
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.EndDisabledGroup();
      }

    }
  }
}
