using System;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 奇物选择Item
    /// </summary>
    public class GGUIWndTreasureHuntTreasureSelectItem : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntTreasureSelectItem>
    {
        private GGUIWndTreasureHuntTreasureInfo _m_wTreasureInfo;
        private _ITreasureHuntTreasureInfo _m_treasureInfo;
        private bool _m_bIsSelected;

        public GGUIWndTreasureHuntTreasureSelectItem(GGUIMonoTreasureHuntTreasureSelectItem wnd) : base(wnd)
        {
            initWnd();
        }

        public _ITreasureHuntTreasureInfo treasureInfo => _m_treasureInfo;
        public bool isSelected => _m_bIsSelected;
        
        public event Action<GGUIWndTreasureHuntTreasureSelectItem> onSelectItem; // 选中事件

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.treasureInfoMono != null)
            {
                _m_wTreasureInfo = new GGUIWndTreasureHuntTreasureInfo(wnd.treasureInfoMono);
                _m_wTreasureInfo.onTreasureClick += _onClickTreasure;
            }
        }

        protected override void _onDiscard()
        {
            onSelectItem = null;

            if (_m_wTreasureInfo != null)
            {
                _m_wTreasureInfo.onTreasureClick -= _onClickTreasure;
                _m_wTreasureInfo.discard();
                _m_wTreasureInfo = null;
            }
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wTreasureInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTreasureInfo?.resetWnd();
            _setSelect(false);
        }

        /// <summary>
        /// 设置奇物数据
        /// </summary>
        public void setData(_ITreasureHuntTreasureInfo treasureInfo)
        {
            _m_treasureInfo = treasureInfo;
            _refreshWnd();
        }

        /// <summary>
        /// 设置选中状态
        /// </summary>
        public void setSelect(bool isSelected)
        {
            _m_bIsSelected = isSelected;
            _setSelect(isSelected);
        }

        private void _refreshWnd()
        {
            if (wnd == null || _m_treasureInfo == null)
                return;

            if (_m_wTreasureInfo != null)
            {
                _m_wTreasureInfo.showWnd();
                _m_wTreasureInfo.setData(_m_treasureInfo);
            }
        }

        private void _setSelect(bool isSelected)
        {
            if (wnd == null)
                return;

            // 显示选中时的物体
            ALUGUICommon.setGameObjEnable(wnd.goSelectShowList, isSelected);
            // 隐藏选中时的物体
            ALUGUICommon.setGameObjEnable(wnd.goSelectHideList, !isSelected);
        }
        
        private void _onClickTreasure(_ITreasureHuntTreasureInfo treasureInfo)
        {
            onSelectItem?.Invoke(this);
        }
    }
}
