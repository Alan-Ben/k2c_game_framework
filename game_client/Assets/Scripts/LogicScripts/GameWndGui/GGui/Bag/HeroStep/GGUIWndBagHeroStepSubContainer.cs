using ALPackage;
using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonEnum;

namespace GOE
{
    //背包骑士阶段合成道具列表
    public class GGUIWndBagHeroStepSubContainer : _ATNPGGUIWndShowAnimContainer<GGUIMonoBagHeroStepSubContainerItem, GGUIMonoBagHeroStepSubContainer, GGUIWndBagHeroStepSubContainerItem>
    {
        
        private List<GGUIWndBagHeroStepSubContainerItem> _m_lItemWndList;

        private List<NPCommonCostItem> _m_itemList;

        public GGUIWndBagHeroStepSubContainer(GGUIMonoBagHeroStepSubContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
            //初始化
            _m_lItemWndList = new List<GGUIWndBagHeroStepSubContainerItem>();
        }

        protected override void _onShowWnd()
        {
        }


        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            if (_m_lItemWndList != null)
            {
                GGUIWndBagHeroStepSubContainerItem temp = null;
                for (int i = 0; i < _m_lItemWndList.Count; ++i)
                {
                    temp = _m_lItemWndList[i];
                    if (temp == null)
                        continue;
                    temp.resetWnd();
                }
                _m_lItemWndList.Clear();
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lItemWndList != null)
            {
                GGUIWndBagHeroStepSubContainerItem temp = null;
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

        protected override GGUIWndBagHeroStepSubContainerItem _createItemWnd(GGUIMonoBagHeroStepSubContainerItem _itemMono)
        {
            // 创建对象
            return new GGUIWndBagHeroStepSubContainerItem(_itemMono);
        }

        public void setData(List<NPCommonCostItem> _itemList)
        {
            if (null == _itemList)
                return;

            _m_itemList = _itemList;
            _refresh();
        }

        private void _refresh()
        {
            if (null == _m_itemList)
                return;

            GGUIWndBagHeroStepSubContainerItem itemWnd = null;
            int count = 0;
            //遍历数据
            for (int i = 0; i < _m_itemList.Count; i++)
            {
                //取数据
                NPCommonCostItem temp = _m_itemList[i];
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
                itemWnd.setItem(temp);
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
