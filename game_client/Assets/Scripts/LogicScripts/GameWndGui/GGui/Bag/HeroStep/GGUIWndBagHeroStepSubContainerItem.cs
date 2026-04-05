using ALPackage;
using Common;
using CommonEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 背包骑士阶段合成的单个道具
    public class GGUIWndBagHeroStepSubContainerItem : _ATALBasicUISubWnd<GGUIMonoBagHeroStepSubContainerItem>
    {
        //物品
        private NPGGUIWndCommonItem _m_itemWnd;

        private NPCommonCostItem _m_item;

        public GGUIWndBagHeroStepSubContainerItem(GGUIMonoBagHeroStepSubContainerItem _wnd) : base(_wnd)
        {
            initWnd();
        }


        // 初始化
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.commonItemMono)
                _m_itemWnd = new NPGGUIWndCommonItem(wnd.commonItemMono);

            ALUGUICommon.combineBtnClick(wnd.combinedGo, _combiendGoDidClick);
            ALUGUICommon.combineBtnClick(wnd.accessWayGo, _accessWayGoDidClick);
        }
        protected override void _onShowWnd()
        {

        }
        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (null == wnd)
                return;

            if (null != _m_itemWnd)
                _m_itemWnd.discard();
            _m_itemWnd = null;

            ALUGUICommon.uncombineBtnClick(wnd.combinedGo, _combiendGoDidClick);
            ALUGUICommon.uncombineBtnClick(wnd.accessWayGo, _accessWayGoDidClick);
        }

        // 刷新item
        public void setItem(NPCommonCostItem _item)
        {
            if (null == _item)
                return;

            _m_item = _item;
            if (null != _m_itemWnd)
                _m_itemWnd.setItem(_item);

            bool isSimpleCombined = GCommon.isItemCanBeCombined(_item.subId);
            ALUGUICommon.setGameObjEnable(wnd.combinedShowGoList, isSimpleCombined);
            ALUGUICommon.setGameObjEnable(wnd.noCombinedShowGoList, !isSimpleCombined);

        }

        //点击合成
        private void _combiendGoDidClick(GameObject _go)
        {
            if (null == _m_item)
                return;

            //打开合成窗口
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndBagItemBeCombined.instance, () =>
            {
                GGUIWndBagItemBeCombined.instance.showWnd();
                GGUIWndBagItemBeCombined.instance.init(_m_item.getItemType(), _m_item.subId);
            }, UINodeTagConst.C_COMMON_SIMPLE_COMBINE);
        }

        //点击获取途径
        private void _accessWayGoDidClick(GameObject _go)
        {
            if (null == _m_item)
                return;

            GCommon.popItemAccessWays(_m_item.getItemType(), _m_item.subId);
        }
    }
}
