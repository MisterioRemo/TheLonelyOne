using UnityEngine;
using UnityEditor;

namespace TheLonelyOne
{
  /// <summary>
  /// Drawer for the RequireInterface attribute.
  /// </summary>
  [CustomPropertyDrawer(typeof(RequireInterfaceAttribute))]
  public class RequireInterfaceDrawer : PropertyDrawer
  {
    /// <summary>
    /// Overrides GUI drawing for the attribute.
    /// </summary>
    public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
    {
      if (_property.propertyType != SerializedPropertyType.ObjectReference)
      {
        // If field is not reference, show error message
        Color previousColor = GUI.color;
        GUI.color           = Color.red;

        EditorGUI.LabelField(_position, _label, new GUIContent("Property is not a reference type"));

        GUI.color = previousColor;

        return;
      }

      var requiredAttribute = this.attribute as RequireInterfaceAttribute;

      if (DragAndDrop.objectReferences.Length > 0)
      {
        var draggedObject = DragAndDrop.objectReferences[0] as GameObject;

        // Prevent dragging of an object that doesn't contain the interface type
        if (draggedObject == null || (draggedObject != null && draggedObject.GetComponent(requiredAttribute.requiredType) == null))
          DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
      }

      // If a value was set through other means (e.g. ObjectPicker)
      if (_property.objectReferenceValue != null)
      {
        GameObject go = _property.objectReferenceValue as GameObject;
        if (go != null && go.GetComponent(requiredAttribute.requiredType) == null)
        {
          // Clean out invalid references.
          _property.objectReferenceValue = null;
        }
      }

      _property.objectReferenceValue = EditorGUI.ObjectField(_position, _label, _property.objectReferenceValue, typeof(GameObject), true);
    }
  }
}