using ALPackage;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    // 背包一级页签
    public class GGUIWndBagFirstTab : _ATNPGGUIWndTabItem<NPGGUIMonoCommonTab>
    {
        // 点击事件
        private Action<GGUIWndBagFirstTab> _m_dClickEvent;

        private List<GGUIWndBagSecondTab> _m_secondWndList;

        //二级页签回调事件
        private Action<GGUIWndBagSecondTab> _m_secondTabClickDelegate;

        //没有二级页签时 显示的枚举列表
        private List<NPEnum.ENPBagItemType> _m_itemTypeList;

        //初始化
        public GGUIWndBagFirstTab(NPGGUIMonoCommonTab _wnd, List<GGUIBagTabSecondMono> _secondMonoList, List<NPEnum.ENPBagItemType> _itemTypeList, Action<GGUIWndBagSecondTab> _secondTabClickDelegate) : base(_wnd)
        {
            _m_secondTabClickDelegate = _secondTabClickDelegate;
            _m_itemTypeList = _itemTypeList;
            //初始化二级页签列表
            _m_secondWndList = new List<GGUIWndBagSecondTab>();
            if (null != _secondMonoList)
            {
                GGUIBagTabSecondMono temp = null;
                for (int i = 0; i < _secondMonoList.Count; i++)
                {
                    temp = _secondMonoList[i];
                    if (null == temp)
                        continue;
                    GGUIWndBagSecondTab tab = new GGUIWndBagSecondTab(temp);
                    tab.onClickTab += _secondTabDidClick;
                    if (null != tab)
                        _m_secondWndList.Add(tab);
                }
            }
        }

        public Action<GGUIWndBagFirstTab> onClickTab { get { return _m_dClickEvent; } set { _m_dClickEvent = value; } }

        //二级页签列表
        public List<GGUIWndBagSecondTab> secondWndList { get { return _m_secondWndList; } }

        public List<NPEnum.ENPBagItemType> itemTypeList { get { return _m_itemTypeList; } }

        //二级页签列表

        protected override void _onDiscard()
        {
            base._onDiscard();

            if (null != _m_secondWndList && _m_secondWndList.Count > 0)
            {
                GGUIWndBagSecondTab temp = null;
                for (int i = 0; i < _m_secondWndList.Count; i++)
                {
                    temp = _m_secondWndList[i];
                    if (null == temp)
                        continue;

                    temp.discard();
                }
                _m_secondWndList.Clear();
            }
            _m_secondWndList = null;

            _m_dClickEvent = default(Action<GGUIWndBagFirstTab>);
        }

        // 响应点击事件
        protected override void _onClickSelectButton(GameObject _go)
        {
            if (null != _m_dClickEvent)
            {
                _m_dClickEvent(this);
            }
        }

        //刷新红点
        public void refreshRedTip()
        {
            if (wnd == null)
                return;

            if (_m_wRedTipWnd != null)
            {
                //刷新对应页签的红点
                //获取一级页签下的所有红点个数

                _m_wRedTipWnd.showRedTipNum(_getAllRedNum());
            }
        }

        private int _getAllRedNum()
        {
            if (null == _m_secondWndList)
                return 0;

            int redNum = 0;

            GGUIWndBagSecondTab temp = null;
            for (int i = 0; i < _m_secondWndList.Count; i++)
            {
                temp = _m_secondWndList[i];
                if (null == temp)
                    continue;

                redNum += BagRedTipMgr.instance.getTabRedTipNum(temp.secondItemMono.secondItemTypeList);
            }

            return redNum;
        }
        //显隐所有的二级页签
        public void setSecondTabListEnable(bool _enable)
        {
            if (null != _m_secondWndList && _m_secondWndList.Count > 0)
            {
                GGUIWndBagSecondTab temp = null;
                for (int i = 0; i < _m_secondWndList.Count; i++)
                {
                    temp = _m_secondWndList[i];
                    if (null == temp)
                        continue;

                    ALUGUICommon.setGameObjEnable(temp.getGameObj(), _enable);
                }
            }
        }

        //二级页签点击事件
        private void _secondTabDidClick(GGUIWndBagSecondTab _secondTab)
        {
            if (null != _m_secondTabClickDelegate)
                _m_secondTabClickDelegate(_secondTab);
        }
    }
}
