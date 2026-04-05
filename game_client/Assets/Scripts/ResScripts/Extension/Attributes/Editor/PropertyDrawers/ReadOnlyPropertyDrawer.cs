using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyPropertyDrawer : PropertyDrawer
{

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(rect, label, property);

		using (new EditorGUI.DisabledScope(disabled: true))
		{
			EditorGUI.PropertyField(rect, property, label, true);
		}

		EditorGUI.EndProperty();
	}
}
