using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取妃子主页面列表高亮位置
    /// </summary>
    public class TutorialHighlightLocationDealer_CONSORT_MAIN_LIST_P : _ATutorialHighlightLocationDealer
    {
        private EConsortMainTargetConsortType _m_targetConsortType;
        private string _m_sCustomParam;

        public override ETutorialHighlightLocationType highlightLocationType { get { return ETutorialHighlightLocationType.CONSORT_MAIN_LIST_P; } }
        
        public override bool getCenterUIRootPosition(out Vector2 centerUIRootPosition)
        {
            centerUIRootPosition = Vector2.zero;
            
            if (GGUIWndConsortChatMain.instance == null || !GGUIWndConsortChatMain.instance.isLoaded || !GGUIWndConsortChatMain.instance.isShow)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_CONSORT_MAIN_LIST_P] 妃子主页面未加载或未显示");
                return false;
            }
            
            RectTransform rectTransform = GGUIWndConsortChatMain.instance.getTargetConsortRectTransform(_m_targetConsortType, _m_sCustomParam);
            if (rectTransform == null)
            {
                Debug.LogError($"[TutorialHighlightLocationDealer_CONSORT_MAIN_LIST_P] 无法获取目标妃子的 RectTransform, 类型：{_m_targetConsortType} _m_sCustomParam:{_m_sCustomParam}");
                return false;
            }

            Vector3 uiPos = rectTransform.position;
            
            centerUIRootPosition = GCommon.getUIRootPos(uiPos, true) + new Vector2(rectTransform.rect.width * (0.5f - rectTransform.pivot.x), rectTransform.rect.height * (0.5f - rectTransform.pivot.y));
            return true;
        }
        
        public static TutorialHighlightLocationDealer_CONSORT_MAIN_LIST_P readVariable(string _str)
        {
            TutorialHighlightLocationDealer_CONSORT_MAIN_LIST_P dealer = new TutorialHighlightLocationDealer_CONSORT_MAIN_LIST_P();
            if (string.IsNullOrEmpty(_str))
                return dealer;

            string[] paramArray = _str.Split(':', 2);
            if (!ALCommon.TryEnumParse(typeof(EConsortMainTargetConsortType), paramArray[0], out EConsortMainTargetConsortType targetType))
                return dealer;

            dealer._m_targetConsortType = targetType;
            dealer._m_sCustomParam = paramArray.Length >= 2 ? paramArray[1] : string.Empty;
            return dealer;
        }
    }
}
