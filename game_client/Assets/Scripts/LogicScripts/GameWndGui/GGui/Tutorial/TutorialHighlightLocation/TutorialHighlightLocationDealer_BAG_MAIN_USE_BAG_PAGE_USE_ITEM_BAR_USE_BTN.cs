using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取背包主页面可使用物品使用栏使用按钮高亮位置
    /// 无需参数
    /// </summary>
    public class TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_USE_ITEM_BAR_USE_BTN : _ATutorialHighlightLocationDealer
    {
        public override ETutorialHighlightLocationType highlightLocationType { get { return ETutorialHighlightLocationType.BAG_MAIN_USE_BAG_PAGE_USE_ITEM_BAR_USE_BTN; } }

        public override bool getCenterUIRootPosition(out Vector2 centerUIRootPosition)
        {
            centerUIRootPosition = Vector2.zero;

            // 检查背包主界面是否已加载并显示
            if (GGUIWndBagMain.instance == null || !GGUIWndBagMain.instance.isLoaded || !GGUIWndBagMain.instance.isShow)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_USE_ITEM_BAR_USE_BTN] 背包主界面未加载或未显示");
                return false;
            }

            // 获取使用按钮的RectTransform
            RectTransform rectTransform = GGUIWndBagMain.instance.getUseBagPageUseSimpleBarUseBtnRectTransform();
            if (rectTransform == null)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_USE_ITEM_BAR_USE_BTN] 无法获取使用按钮的RectTransform");
                return false;
            }

            Vector3 uiPos = rectTransform.position;

            // 计算UI根节点中的位置
            centerUIRootPosition = GCommon.getUIRootPos(uiPos, true) + new Vector2(rectTransform.rect.width * (0.5f - rectTransform.pivot.x), rectTransform.rect.height * (0.5f - rectTransform.pivot.y));
            return true;
        }

        /// <summary>
        /// 从字符串解析高亮位置处理器
        /// </summary>
        public static TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_USE_ITEM_BAR_USE_BTN readVariable(string _str)
        {
            return new TutorialHighlightLocationDealer_BAG_MAIN_USE_BAG_PAGE_USE_ITEM_BAR_USE_BTN();
        }
    }
}
