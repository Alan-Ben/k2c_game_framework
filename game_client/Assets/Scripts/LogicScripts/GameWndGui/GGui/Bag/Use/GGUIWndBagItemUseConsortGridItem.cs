using ALPackage;
using System;
using Common.BagItemUseEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 使用物品-情人列表item
    /// </summary>
    public class GGUIWndBagItemUseConsortGridItem : _ANPGGUIBasicGridItemWnd<GGUIMonoBagItemUseConsortGridItem>
    {
        //道具数据
        private BagItem _m_bagItem;
        //情人数据
        private GGottenConsortInfo _m_consortInfo;
        //情人头像
        private GGUIWndConsortSpIconItem _m_wConsortIconItem;
        //属性类型图标
        private NPGGuiWndTexture _m_wTypeIcon;
        //点击使用回调
        private Action<BagItem, long> _m_useAction;

        public GGUIWndBagItemUseConsortGridItem(GGUIMonoBagItemUseConsortGridItem _wnd, Action<BagItem,long> _useAction) : base(_wnd)
        {
            _m_useAction = _useAction;
            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wConsortIconItem?.hideWnd();
            _m_wTypeIcon?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wConsortIconItem?.resetWnd();
            _m_wTypeIcon?.discardTexture();
        }

        protected override void _resetGridItem()
        {
            _m_wConsortIconItem?.resetWnd();
            _m_wTypeIcon?.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            _m_wConsortIconItem?.discard();
            _m_wConsortIconItem = null;
            _m_wTypeIcon?.discard();
            _m_wTypeIcon = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnUse, _onClickUse);
            ALUGUICommon.uncombineBtnClick(wnd.btnNotEnough, _onClickUse);
        }

        protected override void _onWndInitDone()
        {
            if(null == wnd)
                return;

            if (wnd.monoConsortIcon != null)
                _m_wConsortIconItem = new GGUIWndConsortSpIconItem(wnd.monoConsortIcon);

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
        public void setItem(BagItem _bagItem, GGottenConsortInfo _heroInfo)
        {
            if(null == wnd)
                return;
            
            if (_bagItem == null || _heroInfo == null)
                return;

            _m_bagItem = _bagItem;
            _m_consortInfo = _heroInfo;

            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (null == wnd || null == _m_bagItem || null == _m_consortInfo)
                return;

            GGameCommonInfo.grayImage(wnd.grayImgList,_m_bagItem.count == 0);
            //设置显隐
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughShowList, _m_bagItem.count == 0);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughHideList, _m_bagItem.count > 0);

            _m_wConsortIconItem?.showWnd();
            _m_wConsortIconItem?.setInfo(_m_consortInfo);

            BagItemConsortRefObj bagItemConsortRef = GRefdataCoreMgr.instance.bagItemConsortCore.getRef(_m_bagItem.itemId);
            if (bagItemConsortRef == null)
                return;

            _m_wTypeIcon?.showWnd();
            _m_wTypeIcon.setTexture(bagItemConsortRef.gain_item_icon);

            switch (bagItemConsortRef.show_type)
            {
                case EBagItemUse_ConsortType.INTIMACY:
                    ALUGUICommon.setLabelTxt(wnd.txtValue, _m_consortInfo.intimacy.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                    break;
                case EBagItemUse_ConsortType.CHARM:
                    ALUGUICommon.setLabelTxt(wnd.txtValue, _m_consortInfo.charm.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                    break;
                case EBagItemUse_ConsortType.CHARM_POINT:
                    ALUGUICommon.setLabelTxt(wnd.txtValue, _m_consortInfo.charmPoint.ToLargeString(PrimitiveExtension.ELargeStringType.DEFAULT));
                    break;
            }
        }

        //点击使用按钮
        private void _onClickUse(GameObject _go)
        {
            if(_m_consortInfo == null)
                return;

            if (null != _m_useAction)
                _m_useAction(_m_bagItem, _m_consortInfo.consortId);
        }
    }
}
