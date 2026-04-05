using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取背包主页面可使用物品高亮位置
    /// 参数格式: EGGUIMonoBagItemGridTargetItemType:附加参数
    /// 例如: ITEM_ID:12345
    /// </summary>
    public class TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_ITEM_P : _ATutorialHighlightLocationDealer
    {
        // 目标物品类型
        private EGGUIMonoBagItemGridTargetItemType _m_targetItemType;
        // 附加参数字符串
        private string _m_paramsStr;

        public override ETutorialHighlightLocationType highlightLocationType { get { return ETutorialHighlightLocationType.BAG_MAIN_USE_BAG_PAGE_ITEM_P; } }

        public override bool getCenterUIRootPosition(out Vector2 centerUIRootPosition)
        {
            centerUIRootPosition = Vector2.zero;

            // 检查背包主界面是否已加载并显示
            if (GGUIWndBagMain.instance == null || !GGUIWndBagMain.instance.isLoaded || !GGUIWndBagMain.instance.isShow)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_ITEM_P] 背包主界面未加载或未显示");
                return false;
            }

            // 获取目标物品的RectTransform
            RectTransform rectTransform = GGUIWndBagMain.instance.getUseBagPageItemRectTransform(_m_targetItemType, _m_paramsStr);
            if (rectTransform == null)
            {
                Debug.LogError($"[TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_ITEM_P] 无法获取目标物品的RectTransform, 类型: {_m_targetItemType}, 参数: {_m_paramsStr}");
                return false;
            }

            Vector3 uiPos = rectTransform.position;

            // 计算UI根节点中的位置
            centerUIRootPosition = GCommon.getUIRootPos(uiPos, true) + new Vector2(rectTransform.rect.width * (0.5f - rectTransform.pivot.x), rectTransform.rect.height * (0.5f - rectTransform.pivot.y));
            return true;
        }

        /// <summary>
        /// 从字符串解析高亮位置处理器
        /// 参数格式: EGGUIMonoBagItemGridTargetItemType:附加参数
        /// 例如: ITEM_ID:12345
        /// </summary>
        public static TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_ITEM_P readVariable(string _str)
        {
            TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_ITEM_P dealer = new TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_ITEM_P();

            if (string.IsNullOrEmpty(_str))
                return dealer;

            // 解析参数，格式: EGGUIMonoBagItemGridTargetItemType:附加参数
            int splitPos = _str.IndexOf(':');
            if (splitPos > 0)
            {
                string typeStr = _str.Substring(0, splitPos);
                dealer._m_paramsStr = _str.Substring(splitPos + 1);

                if (ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemGridTargetItemType), typeStr, out EGGUIMonoBagItemGridTargetItemType targetType))
                {
                    dealer._m_targetItemType = targetType;
                }
            }
            else
            {
                // 只有类型，没有附加参数
                if (ALCommon.TryEnumParse(typeof(EGGUIMonoBagItemGridTargetItemType), _str, out EGGUIMonoBagItemGridTargetItemType targetType))
                {
                    dealer._m_targetItemType = targetType;
                }
            }

            return dealer;
        }
    }
}
