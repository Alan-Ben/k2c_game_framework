
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取衣橱页面套装菜单高亮位置
    /// </summary>
    public class TutorialHighlightLocationDealer_CHAPTER_MAP_NODE : _ATutorialHighlightLocationDealer
    {
        public override ETutorialHighlightLocationType highlightLocationType { get { return ETutorialHighlightLocationType.CHAPTER_MAP_NODE; } }
        public override bool getCenterUIRootPosition(out Vector2 centerUIRootPosition)
        {
            centerUIRootPosition = Vector2.zero;
            if (!GGUIWndChapterMap.instance.isLoaded || !GGUIWndChapterMap.instance.isShow)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_CLOTH_MAIN_SUIT_MENU] 关卡地图node位置加载或未显示");
                return false;
            }
            
            RectTransform rectTransform = GGUIWndChapterMap.instance.getCurNodeDoingRectTransform();
            if (!rectTransform)
            {
                Debug.LogError($"[TutorialHighlightLocationDealer_CLOTH_MAIN_SUIT_MENU] 关卡地图node位置加载或未显示");
                return false;
            }

            Vector3 uiPos = rectTransform.position;
            
            centerUIRootPosition = GCommon.getUIRootPos(uiPos, true) + new Vector2(rectTransform.rect.width * (0.5f - rectTransform.pivot.x), rectTransform.rect.height * (0.5f - rectTransform.pivot.y));
            return true;
        }
        
        public static TutorialHighlightLocationDealer_CHAPTER_MAP_NODE readVariable(string _str)
        {
            return new TutorialHighlightLocationDealer_CHAPTER_MAP_NODE();
        }
    }
}