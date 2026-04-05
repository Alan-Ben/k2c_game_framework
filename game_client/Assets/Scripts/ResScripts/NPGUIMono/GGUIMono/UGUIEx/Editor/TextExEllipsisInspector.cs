using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomEditor(typeof(TextExEllipsis))]
    public class TextExEllipsisInspector : TextExEditor
    {
        [MenuItem("GameObject/UI/TextExEllipsis", false, 2001)]
        private static void addTextExEllipsisComp(MenuCommand _menuCommand)
        {
            GameObject go = UIEditorHelp.CreateUIElementRoot("TextExEllipsis", s_ThickElementSize);
        
            TextExEllipsis textEx = go.AddComponent<TextExEllipsis>();
            if(textEx != null)
            {
                textEx.raycastTarget = false;
                Font defaultFont = getDefaultFont();
                if(defaultFont != null)
                    textEx.font = defaultFont;            }
            UIEditorHelp.PlaceUIElementRoot(go, _menuCommand);
        }
        [MenuItem("Component/UI/TextExEllipsis", false, 2001)]
        private static void addComp()
        {
            foreach (var obj  in Selection.objects )
            {
                var go = obj as GameObject;
                if(go != null)
                {
                    TextExEllipsis textEx = go.AddComponent<TextExEllipsis>();
                    if(textEx != null)
                    {
                        textEx.raycastTarget = false;
                        Font defaultFont = getDefaultFont();
                        if(defaultFont != null)
                            textEx.font = defaultFont;                    }
                }
            }
        }
    }
}