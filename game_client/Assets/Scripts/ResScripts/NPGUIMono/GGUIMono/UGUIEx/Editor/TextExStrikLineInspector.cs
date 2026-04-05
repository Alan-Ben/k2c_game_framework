using UnityEditor;
using UnityEngine;

namespace GOE
{
    [CustomEditor(typeof(TextExStrikLine))]
    public class TextExStrikLineEditor : Editor
    {
        private const float kWidth = 160f;
        private const float kThickHeight = 30f;
        private const float kThinHeight = 20f;
        protected static Vector2 s_ThickElementSize = new Vector2(kWidth, kThickHeight);
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            // TextExStrikLine widget = target as TextExStrikLine;
        }


        protected static Font getDefaultFont()
        {
            return Resources.Load<Font>("GUI/font/font_content");
        }

        [MenuItem("GameObject/UI/TextExStrikLine", false, 2001)]
        private static void addTextExStrikLineComp(MenuCommand _menuCommand)
        {
            GameObject go = UIEditorHelp.CreateUIElementRoot("TextExStrikLine", s_ThickElementSize);
        
            TextExStrikLine textEx = go.AddComponent<TextExStrikLine>();
            if(textEx != null)
            {
                textEx.raycastTarget = false;
                Font defaultFont = getDefaultFont();
                if(defaultFont != null)
                    textEx.font = defaultFont;
            }
            UIEditorHelp.PlaceUIElementRoot(go, _menuCommand);
        }
        
        [MenuItem("Component/UI/TextExStrikLine", false, 2001)]
        private static void addComp()
        {
            foreach (var obj  in Selection.objects )
            {
                var go = obj as GameObject;
                if(go != null)
                {
                    TextExStrikLine textEx = go.AddComponent<TextExStrikLine>();
                    if(textEx != null)
                    {
                        textEx.raycastTarget = false;
                        Font defaultFont = getDefaultFont();
                        if(defaultFont != null)
                            textEx.font = defaultFont;
                    }
                }
            }
        }
        
    }
}