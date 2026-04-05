using System;
using System.Collections.Generic;
using Common.NpPlayerInfoObj;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 组合称号文本列表
    /// </summary>
    public class GGUIWndPlayerTitleComboTextGrid : _ATNPGGUIWndShowAnimGrid<GGUIMonoPlayerTitleComboTextGridItem, GGUIMonoPlayerTitleComboTextGrid, GGUIWndPlayerTitleComboTextGridItem>
    {
        //类型
        private ENPItemType _m_eItemType;
        //信息列表
        private List<_IPlayerTitleCombo> _m_lInfoList;
        //当前选中的id
        private long _m_lCurSelectId;
        //点击选中
        private Action<GGUIWndPlayerTitleComboTextGridItem> _m_aClickSelect;

        /// <summary>
        /// 点击选中
        /// </summary>
        public Action<GGUIWndPlayerTitleComboTextGridItem> onClickSelect
        {
            get { return _m_aClickSelect; }
            set { _m_aClickSelect = value; }
        }

        public GGUIWndPlayerTitleComboTextGrid(GGUIMonoPlayerTitleComboTextGrid _wnd) : base(_wnd)
        {
            _m_lInfoList = new List<_IPlayerTitleCombo>();
            initWnd();
        }

        protected override GGUIWndPlayerTitleComboTextGridItem _createItemWnd(GGUIMonoPlayerTitleComboTextGridItem _itemMono)
        {
            GGUIWndPlayerTitleComboTextGridItem item = new GGUIWndPlayerTitleComboTextGridItem(_itemMono);
            item.onClickSelect += _onClickSelect;
            return item;
        }

        protected override void _onRefreshItemWnd(GGUIWndPlayerTitleComboTextGridItem _itemWnd, int _itemIdx)
        {
            if(_itemIdx<0 || _itemIdx >= _m_lInfoList.Count)
                return;

            _itemWnd.setInfo(_m_eItemType, _m_lInfoList[_itemIdx].id);
            _itemWnd.setSelect(_m_lCurSelectId == _m_lInfoList[_itemIdx].id);
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
        }

        protected override void _onWndInitDone()
        {
        }

        /// <summary>
        /// 设置显示数据
        /// </summary>
        /// <param name="_list"></param>
        public void setShowData(ENPItemType _itemType, List<_IPlayerTitleCombo> _idList)
        {
            _m_eItemType = _itemType;
            _m_lInfoList.Clear();
            _m_lInfoList.AddRange(_idList);
            setItemCount(_m_lInfoList.Count);
        }

        /// <summary>
        /// 设置选中
        /// </summary>
        /// <param name="_id"></param>
        public void setSelect(long _id)
        {
            _m_lCurSelectId = _id;
            forceRefreshAllItem();
        }

        private void _onClickSelect(GGUIWndPlayerTitleComboTextGridItem _item)
        {
            if (_item == null)
                return;

            //是否已经解锁
            bool isUnlock = false;
            switch (_item.itemType)
            {
                case ENPItemType.TITLE_PRE:
                    PlayerTitleComboPreInfo preInfo = NPPlayer.instance.titleComp.getTitleComboPreInfo(_item.id);
                    isUnlock = preInfo != null && preInfo.isUnlock;
                    break;
                case ENPItemType.TITLE_SFX:
                    PlayerTitleComboSfxInfo sfxInfo = NPPlayer.instance.titleComp.getTitleComboSfxInfo(_item.id);
                    isUnlock = sfxInfo != null && sfxInfo.isUnlock;
                    break;
            }

            if (isUnlock)
            {
                int preIndex = -1;
                int curIndex = -1;
                for (int i = 0; i < _m_lInfoList.Count; i++)
                {
                    if (_m_lInfoList[i] == null)
                        continue;

                    if (_m_lInfoList[i].id == _m_lCurSelectId)
                        preIndex = i;
                    if (_m_lInfoList[i].id == _item.id)
                        curIndex = i;
                }

                _m_lCurSelectId = _item.id;

                //刷新对应item
                if (preIndex >= 0)
                    forceRefreshItem(preIndex);
                if (curIndex >= 0)
                    forceRefreshItem(curIndex);
            }

            _m_aClickSelect?.Invoke(_item);
        }
    }
}
