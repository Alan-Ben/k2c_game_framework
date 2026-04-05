using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取商店主页面列表高亮位置
    /// </summary>
    public class TutorialHighlightLocationDealer_SHOP_LIST : _ATutorialHighlightLocationDealer
    {
        private int _m_iIndex; // 商店物品下标
        public override ETutorialHighlightLocationType highlightLocationType { get { return ETutorialHighlightLocationType.SHOP_LIST; } }

        public override bool getCenterUIRootPosition(out Vector2 centerUIRootPosition)
        {
            centerUIRootPosition = Vector2.zero;

            if (GGUIWndShopMain.instance == null || !GGUIWndShopMain.instance.isLoaded || !GGUIWndShopMain.instance.isShow)
            {
                Debug.LogError("[TutorialHighlightLocationDealer_SHOP_LIST] 商店界面未加载或未显示");
                return false;
            }

            RectTransform rectTransform = GGUIWndShopMain.instance.getShopItemRectTransformByIndex(_m_iIndex);
            if (rectTransform == null)
            {
                Debug.LogError($"[TutorialHighlightLocationDealer_SHOP_LIST] 无法获取商店的RectTransform, 下标: {_m_iIndex}");
                return false;
            }

            Vector3 uiPos = rectTransform.position;

            centerUIRootPosition = GCommon.getUIRootPos(uiPos, true) + new Vector2(rectTransform.rect.width * (0.5f - rectTransform.pivot.x), rectTransform.rect.height * (0.5f - rectTransform.pivot.y));
            return true;
        }

        public static TutorialHighlightLocationDealer_SHOP_LIST readVariable(string _str)
        {
            TutorialHighlightLocationDealer_SHOP_LIST dealer = new TutorialHighlightLocationDealer_SHOP_LIST();

            if (!string.IsNullOrEmpty(_str))
            {
                dealer._m_iIndex = ALCommon.GetInt(_str);
            }

            return dealer;
        }
    }
}
