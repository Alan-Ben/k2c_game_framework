using System;
using System.Collections.Generic;
using ALPackage;

namespace GOE
{
    // 通用奖励容器 展示reward_item_list
    public class GGUIWndCreatePlayerPrefabContainer : _ANPGGUIBasicSubWndContainer<GGUIMonoCreatePlayerPrefabItem, GGUIMonoCreatePlayerPrefabContainer, GGUIWndCreatePlayerPrefabItem>
    {
        //item 复用列表
        private List<GGUIWndCreatePlayerPrefabItem> _m_lItemList;
        private GGUIWndCreatePlayerPrefabItem _m_curItem;

        public GGUIWndCreatePlayerPrefabContainer(GGUIMonoCreatePlayerPrefabContainer _containerMono) : base(_containerMono)
        {
            _m_lItemList = new List<GGUIWndCreatePlayerPrefabItem>();

            initWnd();
        }
        public event Action<PlayerCreatPlayerPrefabRefObj> onSelectItemChg;
        public PlayerCreatPlayerPrefabRefObj curRefObj { get { return _m_curItem?.refObj; } }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {

        }

        protected override void _onReset()
        {
            _m_curItem = null;

        }

        protected override void _onDiscard()
        {
            _m_lItemList.Clear();
            _m_lItemList = null;
            
            _m_curItem = null;
        }

        protected override GGUIWndCreatePlayerPrefabItem _createItemWnd(GGUIMonoCreatePlayerPrefabItem _itemMono)
        {
            // 创建对象
            return new GGUIWndCreatePlayerPrefabItem(_itemMono, _clickItemWnd);
        }

        public void setInfoList(List<PlayerCreatPlayerPrefabRefObj> _list)
        {
            if (null == _list)
                return;
            _m_curItem = null;

            GGUIWndCreatePlayerPrefabItem itemWnd = null;
            PlayerCreatPlayerPrefabRefObj temp = null;
            //逐个添加Item
            int itemIdx = 0;
            for (int i = 0; i < _list.Count; i++)
            {
                temp = _list[i];
                if (null == temp)
                    continue;
                if (itemIdx < _m_lItemList.Count)
                {
                    itemWnd = _m_lItemList[itemIdx];
                }
                else
                {
                    itemWnd = addItemWnd();
                    if (null != itemWnd)
                        _m_lItemList.Add(itemWnd);
                }
                //累加索引
                itemIdx++;

                if (null != itemWnd)
                {
                    itemWnd.showWnd();
                    itemWnd.setInfo(temp);
                }
            }

            //隐藏容器中多余的视图
            for (int j = _m_lItemList.Count - 1; j >= itemIdx; j--)
            {
                itemWnd = _m_lItemList[j];
                itemWnd.hideWnd();
            }

        }
        
        private void _clickItemWnd(GGUIWndCreatePlayerPrefabItem _itemWnd)
        {
            if (null == _itemWnd || null == _itemWnd.refObj)
                return;

            if (null != _m_curItem && null != _m_curItem.refObj && _m_curItem.refObj.id == _itemWnd.refObj.id)
                return;

            _m_curItem?.setSelected(false);

            _m_curItem = _itemWnd;

            _m_curItem?.setSelected(true);

            onSelectItemChg?.Invoke(_m_curItem.refObj);
        }
        
        public void setIndexSelected(int _index)
        {
            if(null == _m_lItemList)
                return;
            
            if (_index < 0 || _index >= _m_lItemList.Count)
                return;

            GGUIWndCreatePlayerPrefabItem itemWnd = _m_lItemList[_index];
            if (null == itemWnd)
                return;

            _clickItemWnd(itemWnd);
        }
    }

}
