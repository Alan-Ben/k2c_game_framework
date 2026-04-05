using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using System;

namespace GOE
{
    // 背包弹窗:使用物品
    public class GGUIWndBagPopItemUseSimple : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoBagPopItemUseSimple>
    {
        // 物品数据
        private BagItem _m_iItem = null;

        //消耗按钮
        private GGUIWndBagCostUseBtn _m_costUseBtn;
        //道具不足按钮
        private GGUIWndBagCostUseBtn _m_notEnoughUseBtn;

        public GGUIWndBagPopItemUseSimple(Transform _parent)
            : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagPopItemUseSimple.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagPopItemUseSimple.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }



        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (null != wnd.costUseBtn)
                _m_costUseBtn = new GGUIWndBagCostUseBtn(wnd.costUseBtn, _onUseBtnClick);
            if (null != wnd.notEnoughUseBtn)
                _m_notEnoughUseBtn = new GGUIWndBagCostUseBtn(wnd.notEnoughUseBtn, _onUseBtnClick);

        }

        protected override void _onShowWnd()
        {
            //监听物品增加和移除
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _bagItemChangeMsg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _bagItemChangeMsg);
            //监听模拟点击使用按钮消息
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BAG_POP_ITEM_USE_SIMPLE_USE_BTN, _onSimulateUseBtnClick);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _bagItemChangeMsg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _bagItemChangeMsg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BAG_POP_ITEM_USE_SIMPLE_USE_BTN, _onSimulateUseBtnClick);
        }

        protected override void _onReset()
        {
        }
        protected override void _onDiscard()
        {
            if (null != _m_costUseBtn)
                _m_costUseBtn.discard();
            _m_costUseBtn = null;

            if (null != _m_notEnoughUseBtn)
                _m_notEnoughUseBtn.discard();
            _m_notEnoughUseBtn = null;

            _m_iItem = null;
        }

        /// <summary>
        /// 物品增或移除回调
        /// </summary>
        private void _bagItemChangeMsg(params object[] _objs)
        {
            if (null == wnd)
                return;

            BagItem _item = (BagItem)_objs[0];
            if (null != _item && _item.itemId == _m_iItem.itemId)
            {
                refreshUseBtnState();
            }
        }

        /// <summary>
        /// 刷新使用按钮状态 
        /// </summary>
        public void refreshUseBtnState()
        {
            if (wnd == null)
                return;

            bool isEnable = _checkItemCanUse(false);
            if (!isEnable)
                GGameCommonInfo.grayImage(wnd.grayImgList);
            else
                GGameCommonInfo.disgrayImage(wnd.grayImgList);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughHideList, isEnable);
            ALUGUICommon.setGameObjEnable(wnd.goItemNotEnoughShowList, !isEnable);
        }

        private bool _checkItemCanUse(bool _showTips)
        {
            return NPPlayer.instance.bagComp.isItemCanUse(_m_iItem, 1, _showTips);
        }
        
        // 初始化
        public void init(BagItem _item)
        {
            if (null == wnd || null == _item)
                return;

            _m_iItem = _item;
            //物品名字
            ALUGUICommon.setLabelTxt(wnd.itemName, _item.baseItemData.transName);
            //物品描述
            ALUGUICommon.setLabelTxt(wnd.itemDesc, _item.baseItemData.transDesc);

            //控制使用条件显隐
            ALUGUICommon.setGameObjEnable(wnd.itemUseDescParent, _item.itemUseRefObj.cond_desc.Length != 0);
            //使用条件
            ALUGUICommon.setLabelTxt(wnd.itemUseDesc, TextTranslate.instance.getLanguage(_item.itemUseRefObj.cond_desc, _item.itemUseRefObj.cond_desc_args));

            refreshUseBtnState();

            if (null != _m_costUseBtn)
                _m_costUseBtn.setItem(_item.itemUseRefObj.cost_item_list);

            if (null != _m_notEnoughUseBtn)
                _m_notEnoughUseBtn.setItem(_item.itemUseRefObj.cost_item_list);
        }

        //点击使用按钮
        private void _onUseBtnClick()
        {
            if (null == _m_iItem)
                return;

            //检查物品是否可用（包含提示）
            if (!_checkItemCanUse(true))
                return;

            //如果有配置背包使用效果，直接执行，不再弹出使用界面
            if (_m_iItem.itemUseRefObj != null &&
                _m_iItem.itemUseRefObj.c_effects_in_bag != null &&
                !_m_iItem.itemUseRefObj.c_effects_in_bag.isEmpty)
            {
                _m_iItem.itemUseRefObj.c_effects_in_bag.dealEffect();
                return;
            }

            //使用道具
            GCommon.showBagUseItemsMono(_m_iItem);
        }

        /// <summary>
        /// 模拟点击使用按钮（无参数消息回调）
        /// </summary>
        private void _onSimulateUseBtnClick()
        {
            bool isEnable = _checkItemCanUse(false);
            if (!isEnable)
            {
                if (_m_notEnoughUseBtn != null)
                    _m_notEnoughUseBtn.simulateClickUseBtn();
                else
                    _onUseBtnClick();
            }
            else
            {
                if (_m_costUseBtn != null)
                    _m_costUseBtn.simulateClickUseBtn();
                else
                    _onUseBtnClick();
            }
        }

        public RectTransform getUseBtnRectTransform()
        {
            bool isEnable = _checkItemCanUse(false);
            if (!isEnable)
            {
                if (_m_notEnoughUseBtn != null)
                    return _m_notEnoughUseBtn.getUseBtnRectTransform();
                else
                    return null;
            }
            else
            {
                if (_m_costUseBtn != null)
                    return _m_costUseBtn.getUseBtnRectTransform();
                else
                    return null;
            }
        }
    }
}
