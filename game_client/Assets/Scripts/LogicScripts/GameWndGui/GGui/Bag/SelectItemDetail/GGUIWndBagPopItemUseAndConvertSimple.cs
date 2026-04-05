using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 使用和合成道具弹窗
    /// </summary>
    public class GGUIWndBagPopItemUseAndConvertSimple : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoBagPopItemUseAndConvertSimple>
    {
        //物品数据
        private BagItem _m_iItem = null;

        public GGUIWndBagPopItemUseAndConvertSimple(Transform _parent) : base(_parent)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoBagPopItemUseAndConvertSimple.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoBagPopItemUseAndConvertSimple.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            //监听物品增加和移除
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _bagItemChangeMsg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _bagItemChangeMsg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _bagItemChangeMsg);
            //监听模拟点击使用按钮消息
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_BAG_POP_ITEM_USE_SIMPLE_USE_BTN, _onSimulateUseBtnClick);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _bagItemChangeMsg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _bagItemChangeMsg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _bagItemChangeMsg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_BAG_POP_ITEM_USE_SIMPLE_USE_BTN, _onSimulateUseBtnClick);
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            _m_iItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnUse, _onUseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnUseNotEnough, _onUseBtnClick);
            ALUGUICommon.uncombineBtnClick(wnd.btnCombine, _onclickCombine);
            ALUGUICommon.uncombineBtnClick(wnd.btnCombineNotEnough, _onclickCombine);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            ALUGUICommon.combineBtnClick(wnd.btnUse, _onUseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnUseNotEnough, _onUseBtnClick);
            ALUGUICommon.combineBtnClick(wnd.btnCombine, _onclickCombine);
            ALUGUICommon.combineBtnClick(wnd.btnCombineNotEnough, _onclickCombine);
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="_item"></param>
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
            ALUGUICommon.setGameObjEnable(wnd.itemUseDescParent, _item.itemUseRefObj != null && !string.IsNullOrEmpty(_item.itemUseRefObj.cond_desc));
            //使用条件
            ALUGUICommon.setLabelTxt(wnd.itemUseDesc, _item.itemUseRefObj != null ? TextTranslate.instance.getLanguage(_item.itemUseRefObj.cond_desc, _item.itemUseRefObj.cond_desc_args) : string.Empty);
            //刷新使用按钮状态
            refreshUseBtnState();
            //刷新合成按钮状态
            refreshCombineBtnState();
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
                GGameCommonInfo.grayImage(wnd.useGrayImgList);
            else
                GGameCommonInfo.disgrayImage(wnd.useGrayImgList);
            ALUGUICommon.setGameObjEnable(wnd.goUseItemNotEnoughHideList, isEnable);
            ALUGUICommon.setGameObjEnable(wnd.goUseItemNotEnoughShowList, !isEnable);
        }

        /// <summary>
        /// 刷新合成按钮状态
        /// </summary>
        public void refreshCombineBtnState()
        {
            if (wnd == null) 
                return;

            bool canCombine = GCommon.isItemCanCombine(_m_iItem.itemId, 1);
            if (canCombine)
                GGameCommonInfo.disgrayImage(wnd.notCombineEnoughGrayList);
            else
                GGameCommonInfo.grayImage(wnd.notCombineEnoughGrayList);
            ALUGUICommon.setGameObjEnable(wnd.goCombineItemNotEnoughShowList, !canCombine);
            ALUGUICommon.setGameObjEnable(wnd.goCombineItemNotEnoughHideList, canCombine);
        }

        /// <summary>
        /// 检查物品是否可用
        /// </summary>
        /// <param name="_showTips"></param>
        /// <returns></returns>
        private bool _checkItemCanUse(bool _showTips)
        {
            return NPPlayer.instance.bagComp.isItemCanUse(_m_iItem, 1, _showTips);
        }

        #region 点击事件

        //点击使用按钮
        private void _onUseBtnClick(GameObject _go)
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
        /// 点击合成按钮
        /// </summary>
        /// <param name="_go"></param>
        private void _onclickCombine(GameObject _go)
        {
            if (null == _m_iItem)
                return;

            //如果物品为空时提示
            if (_m_iItem.count == 0)
            {
                //使用key
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.bag_notSelect_tip);
                return;
            }

            //打开合成窗口
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemBeCombined.instance, () =>
            {
                GGUIWndBagItemBeCombined.instance.showWnd();
                GGUIWndBagItemBeCombined.instance.initOri(_m_iItem.itemId);
            }, UINodeTagConst.C_COMMON_SIMPLE_COMBINE);
        }

        #endregion

        #region 消息事件

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
                refreshCombineBtnState();
            }
        }

        /// <summary>
        /// 模拟点击使用按钮（无参数消息回调）
        /// </summary>
        private void _onSimulateUseBtnClick()
        {
            _onUseBtnClick(null);
        }

        #endregion
    }
}
