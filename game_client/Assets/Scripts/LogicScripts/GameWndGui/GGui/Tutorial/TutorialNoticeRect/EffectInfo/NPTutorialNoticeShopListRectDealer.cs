using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 商店主页面列表位置
    /// </summary>
    public class NPTutorialNoticeShopListRectDealer : _AWCGTutorialNoticeRectDealer
    {
        private int _m_iIndex; // 商店物品下标
        public override EWCGTutorialNoticeRectType noticeRectType { get { return EWCGTutorialNoticeRectType.SHOP_LIST; } }

        public override bool getRect(out Rect _rect)
        {
            RectTransform rectTransform = GGUIWndShopMain.instance?.getShopItemRectTransformByIndex(_m_iIndex);
            if (null == rectTransform)
            {
                _rect = new Rect();
                return false;
            }

            Vector2 pos = GCommon.getUIRootPos(rectTransform);
            Vector2 minimumCornerPos = new Vector2(pos.x - rectTransform.pivot.x * rectTransform.rect.width, 
                                                   pos.y - rectTransform.pivot.y * rectTransform.rect.height);
            _rect = new Rect(minimumCornerPos, rectTransform.rect.size);
            return true;
        }

        public static NPTutorialNoticeShopListRectDealer readVariable(string _str)
        {
            NPTutorialNoticeShopListRectDealer effectObj = new NPTutorialNoticeShopListRectDealer();

            if (!string.IsNullOrEmpty(_str))
            {
                effectObj._m_iIndex = ALCommon.GetInt(_str);
            }

            return effectObj;
        }
    }
}

