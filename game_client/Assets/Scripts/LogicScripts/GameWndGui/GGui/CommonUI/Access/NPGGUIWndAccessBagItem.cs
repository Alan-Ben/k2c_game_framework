using ALPackage;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 获取途径使用物品item
    /// </summary>
    public class NPGGUIWndAccessBagItem : _ATALBasicUISubWnd<NPGGUIMonoAccessBagItem>
    {
        private BagItemRefObj _m_rBagItemRef;//物品配表数据
        private BagItem _m_itemInfo;//玩家实际拥有的物品信息
        private NPGGUIWndCommonItem _m_wItem;//物品item
        private AccessAdditionData _m_additionData;//附加信息
        private bool _m_bUseOne;//是否使用过一次

        public NPGGUIWndAccessBagItem(NPGGUIMonoAccessBagItem _mono) : base(_mono)
        {
            initWnd();
        }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemAdd);
            WinMsg.RegisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemRemove);
            WinMsg.RegisterMsgAct(WinMsgType.ON_ACCESS_BAG_ITEM_USE_ONE, _onUseOne);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_UPDATE, _onBagItemChg);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_ADD, _onBagItemAdd);
            WinMsg.UnregisterMsg(WinMsgType.ON_BAG_ITEM_REMOVE, _onBagItemRemove);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_ACCESS_BAG_ITEM_USE_ONE, _onUseOne);

            if (_m_wItem != null)
                _m_wItem.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wItem != null)
                _m_wItem.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wItem != null)
                _m_wItem.discard();
            _m_wItem = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnUse, _onClickUse);
            ALUGUICommon.uncombineBtnClick(wnd.btnUseAll, _onClickUseAll);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoItem != null)
                _m_wItem = new NPGGUIWndCommonItem(wnd.monoItem);

            ALUGUICommon.combineBtnClick(wnd.btnUse, _onClickUse);
            ALUGUICommon.combineBtnClick(wnd.btnUseAll, _onClickUseAll);
        }


        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_accessInfo"></param>
        public void setInfo(NPAccessBagItemInfo _itemInfo, AccessAdditionData _additionData = null)
        {
            if (_itemInfo == null)
                return;

            _m_bUseOne = false;
            _m_rBagItemRef = _itemInfo.bagItemRefObj;
            if (_m_rBagItemRef != null)
                _m_itemInfo = NPPlayer.instance.bagComp.getItem(_m_rBagItemRef.id);
            else
                _m_itemInfo = null;
            
            _m_additionData = _additionData;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_rBagItemRef == null)
                return;

            //设置物品item
            if (_m_wItem != null)
                _m_wItem.showWnd(new NPCommonCostItem(ENPItemType.BAG_ITEM, _m_rBagItemRef.id, _m_itemInfo?.count ?? 0));

            //设置描述
            ALUGUICommon.setLabelTxt(wnd.txtDesc, GCommon.getItemDesc(ENPItemType.BAG_ITEM, _m_rBagItemRef.id));

            //设置全部使用按钮文本
            long itemCount = _m_itemInfo != null ? _m_itemInfo.count : 0;
            if (itemCount > wnd.useAllLimit)
                itemCount = wnd.useAllLimit;
            ALUGUICommon.setLabelTxt(wnd.txtUseAll, TextTranslate.instance.getLanguage(TransKeyConst.common_multiple_num, itemCount));

            //设置是否置灰
            if (itemCount > 0)
                GGameCommonInfo.disgrayImage(wnd.emptyGrayList);
            else
                GGameCommonInfo.grayImage(wnd.emptyGrayList);

            //是否使用过一次了
            ALUGUICommon.setGameObjEnable(wnd.goUseOneShowList, _m_bUseOne);
            ALUGUICommon.setGameObjEnable(wnd.goUseOneHideList, !_m_bUseOne);

            //是否使用完了
            ALUGUICommon.setGameObjEnable(wnd.goEmptyShowList, itemCount <= 0);
            ALUGUICommon.setGameObjEnable(wnd.goEmptyHideList, itemCount > 0);
        }

        //点击使用
        private void _onClickUse(GameObject _go)
        {
            if (wnd == null || _m_rBagItemRef == null)
                return;

            // 背包物品数量小于0
            if (_m_itemInfo == null || _m_itemInfo.count <= 0)
            {
                // 提示道具数量不足
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_itemNotEnough_itemStr, GCommon.getItemName(ENPItemType.BAG_ITEM, _m_rBagItemRef.id)));
                return;
            }

            _m_bUseOne = true;
            //使用物品
            GCommon.useItemByCount(_m_itemInfo, 1, _m_additionData, null, true);

            //发送消息获取途径道具使用过一次，刷新其他item的使用按钮状态
            WinMsg.SendMsg(WinMsgType.ON_ACCESS_BAG_ITEM_USE_ONE);
        }

        //点击全部使用
        private void _onClickUseAll(GameObject _go)
        {
            if (wnd == null || _m_rBagItemRef == null)
                return;

            // 背包物品数量小于0
            if (_m_itemInfo == null || _m_itemInfo.count <= 0)
            {
                // 提示道具数量不足
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.common_itemNotEnough_itemStr, GCommon.getItemName(ENPItemType.BAG_ITEM, _m_rBagItemRef.id)));
                return;
            }

            //获取全部数量，并且不能超过批量使用的上限
            long useCount = _m_itemInfo.count;
            if (useCount > wnd.useAllLimit)
                useCount = wnd.useAllLimit;

            //使用物品
            GCommon.useItemByCount(_m_itemInfo, useCount, _m_additionData, null, true);
        }

        //背包信息变更
        private void _onBagItemChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] == null || _m_rBagItemRef == null)
                return;

            BagItem tempItem = _objects[0] as BagItem;
            if (tempItem == null)
                return;
            
            if(tempItem.itemId == _m_rBagItemRef.id)
                _refreshWnd();
        }

        /// <summary>
        /// 当背包物品添加
        /// </summary>
        private void _onBagItemAdd(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] == null || _m_rBagItemRef == null)
                return;

            BagItem tempItem = _objects[0] as BagItem;
            if (tempItem == null)
                return;

            if (tempItem.itemId == _m_rBagItemRef.id)
            {
                _m_itemInfo = tempItem;
                _refreshWnd();
            }
        }

        /// <summary>
        /// 当背包物品移除
        /// </summary>
        private void _onBagItemRemove(params object[] _objects)
        {
            if (_objects == null || _objects.Length == 0 || _objects[0] == null || _m_rBagItemRef == null)
                return;

            BagItem tempItem = _objects[0] as BagItem;
            if (tempItem == null)
                return;

            if (tempItem.itemId == _m_rBagItemRef.id)
            {
                _m_itemInfo = null;
                _refreshWnd();
            }
        }

        /// <summary>
        /// 获取途径道具使用过一次
        /// </summary>
        private void _onUseOne()
        {
            _m_bUseOne = true;
            _refreshWnd();
        }
    }
}
