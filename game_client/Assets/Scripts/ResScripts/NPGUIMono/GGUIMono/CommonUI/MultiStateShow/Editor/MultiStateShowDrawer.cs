using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace GOE
{
    // 注释掉的原因是 MultiStateShowDrawer 增加了一个 animator，所以不用吧 Inspector 上显示的消掉一层了，就是需要两层
    // /// <summary>
    // /// <see cref="MultiStateShow{T}"/> 的 PropertyDrawer
    // /// </summary>
    // /// <remarks>
    // /// 因为这个类实际上只用关注里面的列表，为了让 Inspector 看起来舒服一点，把外面一层取消掉
    // /// </remarks>
    // [CustomPropertyDrawer(typeof(MultiStateShow<,>))]
    // [CustomPropertyDrawer(typeof(MultiStateShow<>))]
    // public class MultiStateShowDrawer : PropertyDrawer
    // {
    //     private const string _k_propertyName = "stateShowDataList";
    //     
    //     public override float GetPropertyHeight([NotNull] SerializedProperty _property, GUIContent _label)
    //     {
    //         SerializedProperty listProperty = _property.FindPropertyRelative(_k_propertyName);
    //         return EditorGUI.GetPropertyHeight(listProperty);
    //     }
    //
    //     public override void OnGUI(Rect _position, [NotNull] SerializedProperty _property, GUIContent _label)
    //     {
    //         GUIContent label1 = new GUIContent(_label);
    //         EditorGUI.PropertyField(_position, _property.FindPropertyRelative(_k_propertyName), label1);
    //     }
    // }
}