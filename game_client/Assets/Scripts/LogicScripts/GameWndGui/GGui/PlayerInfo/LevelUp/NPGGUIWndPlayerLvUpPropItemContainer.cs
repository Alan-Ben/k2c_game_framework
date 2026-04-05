using System;
using System.Collections.Generic;
using ALPackage;
using UnityEngine;
using UnityEngine.UI;

namespace GOE
{
    //博物馆收藏品价值等级加成属性变化列表container
    public class NPGGUIWndPlayerLvUpPropItemContainer : _ANPGGUIBasicSubWndContainer<NPGGUIMonoPlayerLvUpPropItem, NPGGUIMonoPlayerLvUpPropItemContainer, NPGGUIWndPlayerLvUpPropItem>
    {

        private List<NPGGUIWndPlayerLvUpPropItem> _m_lItemList;//子控件列表

        private List<NPPlayerLvUpShowInfo> _m_infoList;

        public NPGGUIWndPlayerLvUpPropItemContainer(NPGGUIMonoPlayerLvUpPropItemContainer _containerMono) : base(_containerMono)
        {
            //初始化
            _m_lItemList = new List<NPGGUIWndPlayerLvUpPropItem>();
            _m_infoList = new List<NPPlayerLvUpShowInfo>();

            initWnd();
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
            if (_m_lItemList != null)
            {
                NPGGUIWndPlayerLvUpPropItem temp = null;
                for (int i = 0; i < _m_lItemList.Count; ++i)
                {
                    temp = _m_lItemList[i];
                    if (temp == null)
                        continue;
                    temp.resetWnd();
                }
                _m_lItemList.Clear();
            }
        }

        protected override void _onDiscard()
        {
            if (_m_lItemList != null)
                _m_lItemList.Clear();
            _m_lItemList = null;

            _m_infoList = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;

        }

        protected override NPGGUIWndPlayerLvUpPropItem _createItemWnd(NPGGUIMonoPlayerLvUpPropItem _itemMono)
        {
            return new NPGGUIWndPlayerLvUpPropItem(_itemMono);
        }

        public void setList(List<NPPlayerLvUpShowInfo> _list)
        {
            if (null == _list)
                return;

            NPGGUIWndPlayerLvUpPropItem itemWnd = null;
            NPPlayerLvUpShowInfo temp = null;

            for (int i = 0; i < _list.Count; i++)
            {
                temp = _list[i];
                itemWnd = addItemWnd();
                if (null == itemWnd || null == temp)
                    continue;
                _m_lItemList.Add(itemWnd);
                itemWnd.setInfo(temp);
            }
        }

    }
}