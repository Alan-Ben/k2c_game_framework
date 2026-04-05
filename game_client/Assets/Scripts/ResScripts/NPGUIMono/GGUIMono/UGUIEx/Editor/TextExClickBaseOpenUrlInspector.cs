using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomEditor(typeof(TextExClickBaseOpenUrl))]
    public class TextExClickBaseOpenUrlInspector : TextExEditor
    {
        [MenuItem("GameObject/UI/TextExClickBaseOpenUrl", false, 2001)]
        private static void addTextExClickBaseOpenUrlComp(MenuCommand _menuCommand)
        {
            GameObject go = UIEditorHelp.CreateUIElementRoot("TextExClickBaseOpenUrl", s_ThickElementSize);
        
            TextExClickBaseOpenUrl textEx = go.AddComponent<TextExClickBaseOpenUrl>();
            if(textEx != null)
            {
                textEx.raycastTarget = false;
                Font defaultFont = getDefaultFont();
                if(defaultFont != null)
                    textEx.font = defaultFont;            }
            UIEditorHelp.PlaceUIElementRoot(go, _menuCommand);
        }
        [MenuItem("Component/UI/TextExClickBaseOpenUrl", false, 2001)]
        private static void addComp()
        {
            foreach (var obj  in Selection.objects )
            {
                var go = obj as GameObject;
                if(go != null)
                {
                    TextExClickBaseOpenUrl textEx = go.AddComponent<TextExClickBaseOpenUrl>();
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