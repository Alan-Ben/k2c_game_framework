using ALPackage;
using System;
using UnityEngine;

namespace GOE
{
    // 背包弹窗:使用物品
    public abstract class _AGGUIWndBagItemUse<T> : _ANPGGUIBasicWnd<T> where T: _AGGUIMonoBagItemUse
    {
        // 物品数据
        protected BagItem _m_iItem = null;

        // 使用数量
        private long _m_lUseCount = 1;

        //最大使用数量
        private long _m_useMaxCount = -1;

        // 弹窗属性
        private NPGGUIWndCommonItem _m_wItemInfo = null;
        // 数量计数器
        private GGUIWndBagPopCounter _m_wCounter = null;

        //消耗按钮
        private GGUIWndBagCostUseBtn _m_costUseBtn;
        //物品不足按钮
        private GGUIWndBagCostUseBtn _m_notEnoughUseBtn;

        //实际获得物品的图片
        private NPGGuiWndTexture _m_realGainItemWnd;

        //使用回调，如果不为null则执行这个
        private Action<long> _m_useAction = null;

        protected  _AGGUIWndBagItemUse(EALUIWndLayer _layer)
            : base(_layer)
        {
        }

        protected abstract string _m_nodeTag { get; }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected abstract void _initEx();
        protected abstract void _onCounterChangedEx(long _newCount);

        protected override void _onDiscard()
        {
            _m_iItem = null;
            _m_lUseCount = 1;

            if (_m_wItemInfo != null)
                _m_wItemInfo.discard();
            _m_wItemInfo = null;

            if (_m_wCounter != null)
                _m_wCounter.discard();
            _m_wCounter = null;

            if (null != _m_costUseBtn)
                _m_costUseBtn.discard();
            _m_costUseBtn = null;

            if (null != _m_notEnoughUseBtn)
                _m_notEnoughUseBtn.discard();
            _m_notEnoughUseBtn = null;

            if (null != _m_realGainItemWnd)
                _m_realGainItemWnd.discard();
            _m_realGainItemWnd = null;

        }

        protected override void _onHideWnd()
        {
            // 重置出售数量
            _m_lUseCount = 1;

            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BAG_ITEM_USE_WND_USE_BTN, _onSimulateUseBtnClick);
        }

        protected override void _onReset()
        {
            if (_m_wItemInfo != null)
                _m_wItemInfo.resetWnd();
            if (_m_wCounter != null)
                _m_wCounter.resetWnd();
        }

        protected override void _onShowWnd()
        {
            if (_m_wItemInfo != null)
                _m_wItemInfo.showWnd();
            if (_m_wCounter != null)
                _m_wCounter.showWnd();

            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BAG_ITEM_USE_WND_USE_BTN, _onSimulateUseBtnClick);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (wnd.itemWnd != null)
                _m_wItemInfo = new NPGGUIWndCommonItem(wnd.itemWnd);

            if (wnd.useCounter != null)
            {
                _m_wCounter = new GGUIWndBagPopCounter(wnd.useCounter);
                _m_wCounter.regCounterChangedEvent(_onCounterChanged);
            }

            if (null != wnd.costUseBtn)
                _m_costUseBtn = new GGUIWndBagCostUseBtn(wnd.costUseBtn, _onConfirmBtnClick);

            if (null != wnd.notEnoughUseBtn)
                _m_notEnoughUseBtn = new GGUIWndBagCostUseBtn(wnd.notEnoughUseBtn, _onConfirmBtnClick);

            //绑定按钮
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onCloseBtnClick);
        }

        // 初始化
        public void init(BagItem _item, Action<long> _useAction, long _useMaxCount = -1, string _specialTitle = null, string _specialDesc = null)
        {
            if (null == wnd)
                return;

            _m_iItem = _item;
            _m_useAction = _useAction;
            _m_useMaxCount = _useMaxCount;

            _m_wCounter.showWnd();
            // 初始化计数器
            _m_wCounter.init(_item, _useMaxCount);

            // 窗口属性
            CommonItemData itemData = new CommonItemData(_item);
            if (_m_wItemInfo != null)
                _m_wItemInfo.setItem(itemData);

            if (null != _m_costUseBtn && null != _item.itemUseRefObj)
                _m_costUseBtn.setItem(_item.itemUseRefObj.cost_item_list);

            if (null != _m_notEnoughUseBtn && null != _item.itemUseRefObj)
                _m_notEnoughUseBtn.setItem(_item.itemUseRefObj.cost_item_list);

            if (null != _specialDesc)
                ALUGUICommon.setLabelTxt(wnd.itemDetail, _specialDesc);
            else
                ALUGUICommon.setLabelTxt(wnd.itemDetail, _item.baseItemData.transDesc);

            if (null != _specialTitle)
                ALUGUICommon.setLabelTxt(wnd.titleTxt, _specialTitle);
            else
                ALUGUICommon.setLabelTxt(wnd.titleTxt, TextTranslate.instance.getLanguage(TransKeyConst.bag_useTitle_none));

            _refreshUseBtnState();
            _initEx();
        }

        /// <summary>
        /// 刷新使用按钮状态 
        /// </summary>
        private void _refreshUseBtnState()
        {
            if (wnd == null)
                return;

            bool isEnable = NPPlayer.instance.bagComp.isItemCanUse(_m_iItem, _m_lUseCount, false);
            if (!isEnable)
                GGameCommonInfo.grayImage(wnd.grayImgList);
            else
                GGameCommonInfo.disgrayImage(wnd.grayImgList);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughHideList, isEnable);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughShowList, !isEnable);
        }

        // 响应确定按钮点击事件

        private void _onConfirmBtnClick()
        {
            if (null == _m_iItem ||  !GCommon.isItemEnough(_m_iItem.itemType, _m_iItem.itemId, 1, true))
                return;

            long count = _m_lUseCount;
            QueueMgr.instance.forceCloseNodeByTag(_m_nodeTag);
            _m_useAction?.Invoke(count);
        }
        // 响应关闭按钮点击事件
        private void _onCloseBtnClick(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(_m_nodeTag);
        }

        // 响应计数器更改事件
        private void _onCounterChanged(long _newCount)
        {
            if (null == wnd)
                return;

            _m_lUseCount = _newCount;

            if (null != _m_costUseBtn)
                _m_costUseBtn.setSelectCount(_newCount);

            if (null != _m_notEnoughUseBtn)
                _m_notEnoughUseBtn.setSelectCount(_newCount);

            _refreshUseBtnState();

            _onCounterChangedEx(_newCount);
        }

        /// <summary>
        /// 模拟点击使用按钮回调（无参数消息）
        /// </summary>
        private void _onSimulateUseBtnClick()
        {
            if (null == wnd || null == _m_iItem)
                return;

            bool canUse = NPPlayer.instance.bagComp.isItemCanUse(_m_iItem, _m_lUseCount, false);
            if (canUse)
            {
                if (_m_costUseBtn != null)
                {
                    _m_costUseBtn.simulateClickUseBtn();
                }
                else
                {
                    _onConfirmBtnClick();
                }
            }
            else
            {
                if (_m_notEnoughUseBtn != null)
                {
                    _m_notEnoughUseBtn.simulateClickUseBtn();
                }
                else
                {
                    _onConfirmBtnClick();
                }
            }
        }
    }
}
