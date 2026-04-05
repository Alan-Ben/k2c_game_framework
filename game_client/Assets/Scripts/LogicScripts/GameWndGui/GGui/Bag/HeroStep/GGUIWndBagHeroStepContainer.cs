using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using ClientEnum;
using NPEnum;


namespace GOE
{
    // 背包骑士阶段容器容器
    public class GGUIWndBagHeroStepContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoBagHeroStepContainerItem, GGUIMonoBagHeroStepContainer, GGUIWndBagHeroStepContainerItem>
    {

        private List<GGUIWndBagHeroStepContainerItem> _m_lItemWndList;

        public GGUIWndBagHeroStepContainer(GGUIMonoBagHeroStepContainer _containerMono) : base(_containerMono)
        {

            initWnd();
        }

        #region override方法
        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            _m_lItemWndList = new List<GGUIWndBagHeroStepContainerItem>();

        }

        protected override void _onShowWnd()
        {
            _refresh();

            WinMsg.RegisterMsgAct(WinMsgType.ON_BAG_ITEM_ADD, _refresh);
            WinMsg.RegisterMsgAct(WinMsgType.ON_BAG_ITEM_REMOVE, _refresh);
            WinMsg.RegisterMsgAct(WinMsgType.ON_BAG_ITEM_UPDATE, _refresh);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_BAG_ITEM_ADD, _refresh);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_BAG_ITEM_REMOVE, _refresh);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_BAG_ITEM_UPDATE, _refresh);
        }
        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (_m_lItemWndList != null)
            {
                GGUIWndBagHeroStepContainerItem temp = null;
                for (int i = 0; i < _m_lItemWndList.Count; ++i)
                {
                    temp = _m_lItemWndList[i];
                    if (temp == null)
                        continue;
                    temp.discard();
                }
                _m_lItemWndList.Clear();
                _m_lItemWndList = null;
            }
        }
        // 创建对象
        protected override GGUIWndBagHeroStepContainerItem _createItemWnd(GGUIMonoBagHeroStepContainerItem _itemMono)
        {
            // 创建对象
            GGUIWndBagHeroStepContainerItem gridItem = new GGUIWndBagHeroStepContainerItem(_itemMono);
            return gridItem;
        }

        #endregion

        private void _refresh()
        {
            List<HeroStepRefObj>  refObjList = new List<HeroStepRefObj>();
            GRefdataCoreMgr.instance.heroStepRefCore.dealAllRef((item) =>
            {
                if (item.cost_item_list.Count > 0)
                    refObjList.Add(item);
            });

            ALUGUICommon.setGameObjEnable(wnd.noneItemsTips, refObjList.Count == 0);

            GGUIWndBagHeroStepContainerItem itemWnd = null;
            int count = 0;
            //遍历数据
            for (int i = 0; i < refObjList.Count; i++)
            {
                //取数据
                HeroStepRefObj temp = refObjList[i];
                if (null == temp)
                    continue;

                //如果容器内部个数不足则新增视图
                if (i >= _m_lItemWndList.Count)
                {
                    itemWnd = addItemWnd();
                    if (null == itemWnd)
                        continue;
                    _m_lItemWndList.Add(itemWnd);
                }
                //如果容器个数足够，则取出
                else
                {
                    itemWnd = _m_lItemWndList[i];
                }
                itemWnd.showWnd();
                itemWnd.refreshItem(temp, i == refObjList.Count - 1);
                count++;
            }
            //隐藏容器中多余的视图
            for (int j = count; j < _m_lItemWndList.Count; j++)
            {
                _m_lItemWndList[j].hideWnd();
            }
        }
    }
}
