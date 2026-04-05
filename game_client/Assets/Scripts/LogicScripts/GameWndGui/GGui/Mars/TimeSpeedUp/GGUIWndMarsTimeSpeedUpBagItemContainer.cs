using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;

namespace GOE
{
    /// <summary>
    /// 火星时间加速背包物品容器
    /// </summary>
    public class GGUIWndMarsTimeSpeedUpBagItemContainer : _AGGUISubWndCommonContainer<GGUIMonoMarsTimeSpeedUpBagItem, GGUIMonoMarsTimeSpeedUpBagItemContainer, GGUIWndMarsTimeSpeedUpBagItem>
    {
        private List<MarsTimeSpeedUpBagItemShowData> _m_bagItemShowList; // 背包物品显示数据列表
        private int _m_selectedIndex = -1; // 当前选中的索引
        
        public GGUIWndMarsTimeSpeedUpBagItemContainer(GGUIMonoMarsTimeSpeedUpBagItemContainer _containerMono) : base(_containerMono)
        {
            initWnd();
        }
        
        public int selectedIndex { get { return _m_selectedIndex; } }
        public MarsTimeSpeedUpBagItemShowData selectedBagItemShowData { get { return _m_bagItemShowList?.SafeGet(_m_selectedIndex); } }

        public event Action onSelectItemChg; // 选中物品变化事件

        protected override void _onShowWnd()
        {
            base._onShowWnd();
        }

        protected override void _onHideWnd()
        {
            base._onHideWnd();
        }

        protected override void _onReset()
        {
            base._onReset();
        }

        protected override void _onDiscard()
        {
            base._onDiscard();
            
            _m_bagItemShowList = null;
            _m_selectedIndex = -1;
            onSelectItemChg = null;
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
        }


        protected override GGUIWndMarsTimeSpeedUpBagItem _createItemWnd(GGUIMonoMarsTimeSpeedUpBagItem _itemMono)
        {
            if (_itemMono == null)
                return null;

            GGUIWndMarsTimeSpeedUpBagItem itemWnd = new GGUIWndMarsTimeSpeedUpBagItem(_itemMono);
            // 订阅点击事件
            itemWnd.onItemClick += _onItemWndClick;
            return itemWnd;
        }

        protected override void _discardItem(GGUIWndMarsTimeSpeedUpBagItem _itemWnd)
        {
            if(_itemWnd != null)
                _itemWnd.onItemClick -= _onItemWndClick;
            
            base._discardItem(_itemWnd);
        }

        protected override void _refreshItemWnd(GGUIWndMarsTimeSpeedUpBagItem _itemWnd, int _index)
        {
            if (_m_bagItemShowList == null || _index < 0 || _index >= _m_bagItemShowList.Count)
                return;
            
            // 设置物品信息
            _itemWnd.setInfo(_m_bagItemShowList[_index], _index);
            
            // 设置选中状态
            _itemWnd.setSelected(_index == _m_selectedIndex);
        }


        /// <summary>
        /// 设置数据并刷新
        /// </summary>
        /// <param name="_bagItemShowList">背包物品显示数据列表</param>
        public void setData([ItemNotNull] List<MarsTimeSpeedUpBagItemShowData> _bagItemShowList, MarsTimeSpeedUpBagItemShowData _selectedData = null)
        {
            _m_bagItemShowList = _bagItemShowList;
            if(_selectedData == null)
                _m_selectedIndex = -1;
            else
                _m_selectedIndex = _m_bagItemShowList?.FindIndex((_showData) =>
                {
                    return _showData != null && _showData.bagItemId == _selectedData.bagItemId;
                }) ?? -1;

            int dataCount = _m_bagItemShowList?.Count ?? 0;
            // 若列表中存在数据，但选中索引无效，则默认选中第一个
            if (dataCount >= 0 && (_m_selectedIndex < 0 || _m_selectedIndex >= dataCount))
            {
                _m_selectedIndex = 0;
                onSelectItemChg?.Invoke();
            }

            refreshWnd(dataCount);
            
            if (wnd != null)
            {
                ALUGUICommon.setGameObjEnable(wnd.noItemShowGoList, dataCount <= 0);
                ALUGUICommon.setGameObjEnable(wnd.noItemHideGoList, dataCount > 0);
            }
        }

        /// <summary>
        /// item点击事件处理
        /// </summary>
        private void _onItemWndClick(GGUIWndMarsTimeSpeedUpBagItem _itemWnd)
        {
            if (_itemWnd == null)
                return;

            GGUIWndMarsTimeSpeedUpBagItem preSelectedItemWnd = getItem(_m_selectedIndex);
            _m_selectedIndex = _itemWnd.inListIndex;
            
            preSelectedItemWnd?.setSelected(false);
            _itemWnd.setSelected(true);
            
            onSelectItemChg?.Invoke();
        }
    }
}
