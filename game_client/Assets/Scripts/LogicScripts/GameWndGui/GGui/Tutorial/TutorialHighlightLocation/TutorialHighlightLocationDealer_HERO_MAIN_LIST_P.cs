using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取骑士主页面列表高亮位置
    /// </summary>
    public class TutorialHighlightLocationDealer_HERO_MAIN_LIST_P : _ATutorialHighlightLocationDealer
    {
        private EHeroMainTargetHeroType _m_targetHeroType;

        public override ETutorialHighlightLocationType highlightLocationType { get { return ETutorialHighlightLocationType.HERO_MAIN_LIST_P; } }
        
        public override bool getCenterUIRootPosition(out Vector2 centerUIRootPosition)
        {
            centerUIRootPosition = Vector2.zero;
            
            if (GGUIWndHeroMain.instance == null || !GGUIWndHeroMain.instance.isLoaded || !GGUIWndHeroMain.instance.isShow)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_HERO_MAIN_LIST_P] 骑士主页面未加载或未显示");
                return false;
            }
            
            RectTransform rectTransform = GGUIWndHeroMain.instance.getTargetHeroRectTransform(_m_targetHeroType);
            if (rectTransform == null)
            {
                Debug.LogError($"[TutorialHighlightLocationDealer_HERO_MAIN_LIST_P] 无法获取目标伙伴的RectTransform, 类型: {_m_targetHeroType}");
                return false;
            }

            Vector3 uiPos = rectTransform.position;
            
            centerUIRootPosition = GCommon.getUIRootPos(uiPos, true) + new Vector2(rectTransform.rect.width * (0.5f - rectTransform.pivot.x), rectTransform.rect.height * (0.5f - rectTransform.pivot.y));
            return true;
        }
        
        public static TutorialHighlightLocationDealer_HERO_MAIN_LIST_P readVariable(string _str)
        {
            TutorialHighlightLocationDealer_HERO_MAIN_LIST_P dealer = new TutorialHighlightLocationDealer_HERO_MAIN_LIST_P();
            
            // 解析枚举类型
            if (!string.IsNullOrEmpty(_str))
            {
                if (ALCommon.TryEnumParse(typeof(EHeroMainTargetHeroType), _str, out EHeroMainTargetHeroType _targetType))
                {
                    dealer._m_targetHeroType = _targetType;
                }
            }
            
            return dealer;
        }
    }
}
