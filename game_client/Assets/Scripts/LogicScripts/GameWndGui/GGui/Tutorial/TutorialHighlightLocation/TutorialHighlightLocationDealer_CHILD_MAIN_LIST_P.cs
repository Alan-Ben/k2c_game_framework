using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取子嗣页面列表高亮位置
    /// </summary>
    public class TutorialHighlightLocationDealer_CHILD_MAIN_LIST_P : _ATutorialHighlightLocationDealer
    {
        private EChildMainTargetChildType _m_targetChildType;

        public override ETutorialHighlightLocationType highlightLocationType { get { return ETutorialHighlightLocationType.CHILD_MAIN_LIST_P; } }
        
        public override bool getCenterUIRootPosition(out Vector2 centerUIRootPosition)
        {
            centerUIRootPosition = Vector2.zero;
            
            if (GGUIWndChildMain.instance == null || !GGUIWndChildMain.instance.isLoaded || !GGUIWndChildMain.instance.isShow)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_CHILD_MAIN_LIST_P] 子嗣页面未加载或未显示");
                return false;
            }
            
            RectTransform rectTransform = GGUIWndChildMain.instance.getTargetHeroRectTransform(_m_targetChildType);
            if (rectTransform == null)
            {
                Debug.LogError($"[TutorialHighlightLocationDealer_CHILD_MAIN_LIST_P] 无法获取目标子嗣的RectTransform, 类型: {_m_targetChildType}");
                return false;
            }

            Vector3 uiPos = rectTransform.position;
            
            centerUIRootPosition = GCommon.getUIRootPos(uiPos, true) + new Vector2(rectTransform.rect.width * (0.5f - rectTransform.pivot.x), rectTransform.rect.height * (0.5f - rectTransform.pivot.y));
            return true;
        }
        
        public static TutorialHighlightLocationDealer_CHILD_MAIN_LIST_P readVariable(string _str)
        {
            TutorialHighlightLocationDealer_CHILD_MAIN_LIST_P dealer = new TutorialHighlightLocationDealer_CHILD_MAIN_LIST_P();
            
            // 解析枚举类型
            if (!string.IsNullOrEmpty(_str))
            {
                if (ALCommon.TryEnumParse(typeof(EChildMainTargetChildType), _str, out EChildMainTargetChildType _targetType))
                {
                    dealer._m_targetChildType = _targetType;
                }
            }
            
            return dealer;
        }
    }
}
