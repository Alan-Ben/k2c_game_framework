using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 使用物品-伙伴列表item
    /// </summary>
    public class GGUIWndBagItemUseHeroGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoBagItemUseHeroGridItem>
    {
        //道具数据
        private BagItem _m_bagItem;
        //伙伴数据
        private HeroInfo _m_heroInfo;
        //伙伴头像
        private GGUIWndHeroSpIconItem _m_wHeroIconItem;
        //属性类型图标
        private NPGGuiWndTexture _m_wTypeIcon;
        //点击使用回调
        private Action<BagItem, long> _m_useAction;

        public GGUIWndBagItemUseHeroGridItem(GGUIMonoBagItemUseHeroGridItem _wnd, Action<BagItem,long> _useAction) : base(_wnd)
        {
            _m_useAction = _useAction;
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wHeroIconItem?.hideWnd();
            _m_wTypeIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wHeroIconItem?.resetWnd();
            _m_wTypeIcon?.discardTexture();
        }

        protected override void _resetGridItem()
        {
            _m_wHeroIconItem?.resetWnd();
            _m_wTypeIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            _m_wHeroIconItem?.discard();
            _m_wHeroIconItem = null;

            _m_wTypeIcon?.discard();
            _m_wTypeIcon = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnUse, _onClickUse);
            ALUGUICommon.uncombineBtnClick(wnd.btnNotEnough, _onClickUse);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.monoHeroIcon != null)
                _m_wHeroIconItem = new GGUIWndHeroSpIconItem(wnd.monoHeroIcon);

            if (wnd.imgValueType != null)
                _m_wTypeIcon = new NPGGuiWndTexture(wnd.imgValueType);

            ALUGUICommon.combineBtnClick(wnd.btnUse, _onClickUse);
            ALUGUICommon.combineBtnClick(wnd.btnNotEnough, _onClickUse);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_bagItem"></param>
        /// <param name="_heroInfo"></param>
        public void setItem(BagItem _bagItem, HeroInfo _heroInfo)
        {
            if(null == wnd)
                return;
            
            if (_bagItem == null || _heroInfo == null)
                return;

            _m_bagItem = _bagItem;
            _m_heroInfo = _heroInfo;

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (null == wnd || null == _m_bagItem || null == _m_heroInfo)
                return;

            GGameCommonInfo.grayImage(wnd.grayImgList,_m_bagItem.count == 0);
            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughShowList, _m_bagItem.count == 0);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughHideList, _m_bagItem.count > 0);

            _m_wHeroIconItem?.showWnd();
            _m_wHeroIconItem?.setInfo(_m_heroInfo);

            NPCommonItem powerItem = GRefdataCoreMgr.instance.npGeneral.power_sys_info;
            if(powerItem != null)
            {
                _m_wTypeIcon?.showWnd();
                _m_wTypeIcon?.setTexture(GCommon.getItemTexIcon(powerItem.itemType, powerItem.itemId));
            }
        }

        //点击使用按钮
        private void _onClickUse(GameObject _go)
        {
            if(_m_heroInfo == null)
                return;

            if (null != _m_useAction)
                _m_useAction(_m_bagItem, _m_heroInfo.id);
        }
        
        /// <summary>
        /// 模拟点击使用按钮
        /// </summary>
        public void simulateClickUse()
        {
            _onClickUse(null);
        }
    }
}
