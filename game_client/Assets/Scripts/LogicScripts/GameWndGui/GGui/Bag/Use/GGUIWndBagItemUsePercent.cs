using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 背包弹窗:使用概率物品
    public class GGUIWndBagItemUsePercent : _ANPGGUIBasicWnd<GGUIMonoBagItemUsePercent>
    {
        private static GGUIWndBagItemUsePercent _g_instance = new GGUIWndBagItemUsePercent();
        public static GGUIWndBagItemUsePercent instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndBagItemUsePercent();

                return _g_instance;
            }
        }

        //列表容器
        private GGUIWndBagPercentContainer _m_gPercentContainer;

        // 物品数据
        private BagItem _m_iItem = null;

        // 使用数量
        private long _m_lUseCount = 1;

        // 弹窗属性
        private NPGGUIWndCommonItem _m_wItemInfo = null;

        // 数量计数器
        private GGUIWndBagPopCounter _m_wCounter = null;

        //消耗按钮
        private GGUIWndBagCostUseBtn _m_costUseBtn;

        //物品不足按钮
        private GGUIWndBagCostUseBtn _m_notEnoughUseBtn;

        //使用回调
        private Action<BagItem, long> _m_useAction;

        public GGUIWndBagItemUsePercent()
            : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagItemUsePercent.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagItemUsePercent.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

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
            if (_m_gPercentContainer != null)
                _m_gPercentContainer.discard();
            _m_gPercentContainer = null;

            if (null != _m_costUseBtn)
                _m_costUseBtn.discard();
            _m_costUseBtn = null;

            if (null != _m_notEnoughUseBtn)
                _m_notEnoughUseBtn.discard();
            _m_notEnoughUseBtn = null;

            //解绑按钮
            ALUGUICommon.uncombineBtnClick(wnd.closeBtn, _onCloseBtnClick);
        }

        protected override void _onHideWnd()
        {
            // 重置数量
            _m_lUseCount = 1;
        }

        protected override void _onReset()
        {
            if (_m_wItemInfo != null)
                _m_wItemInfo.resetWnd();
            if (_m_wCounter != null)
                _m_wCounter.resetWnd();
            if (_m_gPercentContainer != null)
                _m_gPercentContainer.resetWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onWndInitDone()
        {
            if (wnd.itemWnd != null)
                _m_wItemInfo = new NPGGUIWndCommonItem(wnd.itemWnd);

            if (wnd.useCounter != null)
            {
                _m_wCounter = new GGUIWndBagPopCounter(wnd.useCounter);
                //监听数量变化
                _m_wCounter.regCounterChangedEvent(_onCounterChanged);
            }
            if (wnd.percentContainer != null)
                _m_gPercentContainer = new GGUIWndBagPercentContainer(wnd.percentContainer);

            if (null != wnd.costUseBtn)
                _m_costUseBtn = new GGUIWndBagCostUseBtn(wnd.costUseBtn, _onConfirmBtnClick);

            if (null != wnd.notEnoughUseBtn)
                _m_notEnoughUseBtn = new GGUIWndBagCostUseBtn(wnd.notEnoughUseBtn, _onConfirmBtnClick);

            //绑定按钮
            ALUGUICommon.combineBtnClick(wnd.closeBtn, _onCloseBtnClick);

        }

        // 初始化
        public void init(BagItem _item, Action<BagItem, long> _useAction)
        {
            if (null == wnd || null == _item)
                return;

            _m_iItem = _item;
            _m_useAction = _useAction;

            // 初始化计数器
            _m_wCounter.init(_item);

            // 窗口属性
            CommonItemData itemData = new CommonItemData(_item);
            if (_m_wItemInfo != null)
                _m_wItemInfo.setItem(itemData);
            if (_m_gPercentContainer != null)
            {
                _m_gPercentContainer.showWnd();
                _m_gPercentContainer.refreshWindow(_item);

            }
            ALUGUICommon.setLabelTxt(wnd.itemDetail, _item.baseItemData.transDesc);

            if (null != _item.itemUseRefObj)
            {
                ALUGUICommon.setLabelTxt(wnd.getRandomCountText, GCommon.getItemDesc(NPEnum.ENPItemType.REWARD, _item.itemUseRefObj.get_reward_id));
                _m_costUseBtn?.setItem(_item.itemUseRefObj.cost_item_list);
                _m_notEnoughUseBtn?.setItem(_item.itemUseRefObj.cost_item_list);
            }

            _refreshUseBtnState();
        }

        /// <summary>
        /// 刷新使用按钮状态 
        /// </summary>
        private void _refreshUseBtnState()
        {
            if (wnd == null)
                return;

            //置灰
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
            if (null == _m_iItem)
                return;

            if (null != _m_useAction)
                _m_useAction(_m_iItem, _m_lUseCount);
        }

        // 响应关闭按钮点击事件
        private void _onCloseBtnClick(GameObject _btn)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ADD_Bag_Item_Use_Percent);
        }

        // 响应计数器更改事件
        private void _onCounterChanged(long _newCount)
        {
            _m_lUseCount = _newCount;

            if (null != _m_costUseBtn)
                _m_costUseBtn.setSelectCount(_newCount);

            if (null != _m_notEnoughUseBtn)
                _m_notEnoughUseBtn.setSelectCount(_newCount);

            _refreshUseBtnState();
        }
    }
}
