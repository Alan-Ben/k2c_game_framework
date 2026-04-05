using System;
using System.Collections.Generic;

namespace GOE
{
    /// <summary>
    /// 玩家卧室皮肤列表 Grid
    /// </summary>
    public class GGUIWndPlayerRoomSkinItemGrid : _ATNPGGUIWndCommonSelectItemGrid<GGUIMonoPlayerRoomSkinItem, GGUIMonoPlayerRoomSkinItemGrid, GGUIWndPlayerRoomSkinItem>
    {
        // 皮肤列表数据
        private List<PlayerRoomSkinRefObj> _m_roomSkinRefObjList;
        private Dictionary<PlayerRoomSkinRefObj, PlayerRoomSkinInfo> _m_roomSkinInfoDict;
        private Dictionary<PlayerRoomSkinRefObj, EPlayerRoomSkinState> _m_roomSkinStateDict;

        public GGUIWndPlayerRoomSkinItemGrid(GGUIMonoPlayerRoomSkinItemGrid _containerMono) : base(_containerMono)
        {
            initWnd();
        }

        public event Action<PlayerRoomSkinRefObj> onSelectRoomSkin;

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onWndInitDone()
        {
            base._onWndInitDone();
        }

        protected override void _dealGridReset()
        {
        }

        protected override void _dealGridDiscard()
        {
            _m_roomSkinRefObjList = null;
            _m_roomSkinInfoDict = null;
            _m_roomSkinStateDict = null;
            
            onSelectRoomSkin = null;
        }

        protected override GGUIWndPlayerRoomSkinItem _createItemWnd(GGUIMonoPlayerRoomSkinItem _itemMono)
        {
            // 创建对象
            return new GGUIWndPlayerRoomSkinItem(this, _itemMono);
        }

        protected override void _refreshGridItemwnd(GGUIWndPlayerRoomSkinItem _itemWnd, bool _isSelected, int _itemIdx)
        {
            if (_itemWnd == null)
                return;

            if (_m_roomSkinRefObjList == null || _itemIdx < 0 || _itemIdx >= _m_roomSkinRefObjList.Count)
                return;

            // 获取配表数据
            PlayerRoomSkinRefObj refObj = _m_roomSkinRefObjList.SafeGet(_itemIdx);
            if (refObj == null)
                return;

            // 获取玩家拥有数据
            PlayerRoomSkinInfo roomSkinInfo = _m_roomSkinInfoDict.GetValueOrDefault(refObj);
            // 获取状态
            EPlayerRoomSkinState state = _m_roomSkinStateDict.GetValueOrDefault(refObj);

            // 刷新 item
            _itemWnd.refreshWnd(refObj, roomSkinInfo, state);
        }

        /// <summary>
        /// 点击对象操作
        /// </summary>
        /// <param name="_idx"></param>
        public override void onClickItem(int _idx)
        {
        }

        public override void onSelectItem(int _idx)
        {
            base.onSelectItem(_idx);
            
            if (_m_roomSkinRefObjList == null || _idx < 0 || _idx >= _m_roomSkinRefObjList.Count)
                return;

            MoveIfCantSeeItem(_idx, EScrollToItemType.NearestEdge);
            PlayerRoomSkinRefObj refObj = _m_roomSkinRefObjList.SafeGet(_idx);
            onSelectRoomSkin?.Invoke(refObj);
        }
        
        /// <summary>
        /// 设置选中皮肤
        /// </summary>
        /// <param name="_refObj"></param>
        public void setSelectedRoomSkin(PlayerRoomSkinRefObj _refObj)
        {
            if (_m_roomSkinRefObjList == null)
                return;

            int idx = _m_roomSkinRefObjList.IndexOf(_refObj);
            if (idx < 0) //若未找到则默认选中第一个
            {
                onSelectItem(0);
            }
            else
            {
                base.onSelectItem(idx);
                MoveIfCantSeeItem(idx, EScrollToItemType.NearestEdge);
                
                onSelectRoomSkin?.Invoke(_refObj);
            }
        }

        /// <summary>
        /// 刷新界面（带参数）
        /// </summary>
        /// <param name="_refObjList">皮肤配表数据列表</param>
        public void refreshWnd(List<PlayerRoomSkinRefObj> _refObjList, Dictionary<PlayerRoomSkinRefObj, PlayerRoomSkinInfo> _infoDict, Dictionary<PlayerRoomSkinRefObj, EPlayerRoomSkinState> _stateDict)
        {
            _m_roomSkinRefObjList = _refObjList;
            _m_roomSkinInfoDict = _infoDict;
            _m_roomSkinStateDict = _stateDict;

            refreshWnd();
        }

        /// <summary>
        /// 刷新界面
        /// </summary>
        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            int count = _m_roomSkinRefObjList?.Count ?? 0;
            setItemCount(count);
        }

        /// <summary>
        /// 强制刷新某个皮肤显示
        /// </summary>
        /// <param name="_refObj"></param>
        public void forceRefreshSkinShow(PlayerRoomSkinRefObj _refObj)
        {
            if (_m_roomSkinRefObjList == null)
                return;

            int idx = _m_roomSkinRefObjList.IndexOf(_refObj);
            forceRefreshItem(idx);
        }
    }
}
