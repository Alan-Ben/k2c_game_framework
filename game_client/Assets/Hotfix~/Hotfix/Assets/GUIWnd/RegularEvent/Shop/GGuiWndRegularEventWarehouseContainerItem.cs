using System;
using ALPackage;
using GOE;
using UnityEngine;

namespace Hotfix
{
    /// <summary>
    /// 万能活动仓库商品item
    /// </summary>
    public class GGuiWndRegularEventWarehouseContainerItem : _AHotfixBaseSubWnd<GGUIMonoRegularEventWarehouseContainerItem>
    {
        //商品配置
        private RegularEventShopItemRefObj _m_eventShopItemRefObj;
        //物品展示
        private NPGGUIWndCommonItem _m_wCommonItem;
        //点击使用按钮
        private Action<RegularEventShopItemRefObj> _m_aOnUseItem;

        /// <summary>
        /// 点击使用按钮回调
        /// </summary>
        public Action<RegularEventShopItemRefObj> onUseItem { get { return _m_aOnUseItem; } set { _m_aOnUseItem = value; } }

        public GGuiWndRegularEventWarehouseContainerItem(GGUIHotfixCommonMono _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wCommonItem?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wCommonItem?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (hotfixWnd == null)
                return;

            _m_wCommonItem?.discard();
            _m_wCommonItem = null;

            _m_aOnUseItem = null;

            ALUGUICommon.uncombineBtnClick(hotfixWnd.btnUse, _onClickUse);
        }

        protected override void _onWndInitDoneHotfix()
        {
            if (hotfixWnd == null)
                return;

            if (hotfixWnd.monoItem != null)
                _m_wCommonItem = new NPGGUIWndCommonItem(hotfixWnd.monoItem);

            ALUGUICommon.combineBtnClick(hotfixWnd.btnUse, _onClickUse);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_shopItemRef"></param>
        public void setInfo(RegularEventShopItemRefObj _shopItemRef)
        {
            if (null == hotfixWnd)
                return;

            _m_eventShopItemRefObj = _shopItemRef;
            _refreshWnd();
        }

        //刷新界面
        private void _refreshWnd()
        {
            if (_m_eventShopItemRefObj == null || _m_eventShopItemRefObj.item == null)
                return;

            NPCommonCostItem costItem = new NPCommonCostItem(_m_eventShopItemRefObj.item);
            costItem.setCount(GCommon.getItemCount(_m_eventShopItemRefObj.item.item));
            if (_m_wCommonItem != null)
            {
                _m_wCommonItem.showWnd();
                _m_wCommonItem.setItem(costItem);
            }

            //增加积分描述
            ALUGUICommon.setLabelTxt(hotfixWnd.txtUseAddPoint, TextTranslate.instance.getLanguage(HotfixTransKeyConst.regularEvent_addScore_num.replaceActivity(_m_eventShopItemRefObj.activity_id), _m_eventShopItemRefObj.use_gain_activity_currency_count));
        }

        //点击使用按钮
        private void _onClickUse(GameObject _go)
        {
            if (_m_eventShopItemRefObj == null)
                return;

            _m_aOnUseItem?.Invoke(_m_eventShopItemRefObj);
        }
    }
}