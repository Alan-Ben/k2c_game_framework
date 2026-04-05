using ALPackage;
using Common.TreasureHuntEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 打捞收获物 - 奇物item窗口
    /// </summary>
    public class GGUIWndTreasureHuntCaptureHarvestItem_Treasure : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntCaptureHarvestItem_Treasure>, _ITreasureHuntCaptureHarvestItemShowWnd
    {
        private TreasureHuntCaptureHarvestItemInfo_Treasure _m_iHarvestItemInfo;

        private GGUIWndTreasureHuntTreasureInfo _m_wTreasureInfo;


        public GGUIWndTreasureHuntCaptureHarvestItem_Treasure(GGUIMonoTreasureHuntCaptureHarvestItem_Treasure _wnd) : base(_wnd)
        {
            initWnd();
        }


        public _IALBasicUIWndInterface wndInstance { get { return this; } }
        public ETreasureHuntGainType havestType { get { return ETreasureHuntGainType.TREASURE; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建奇物信息子窗口
            if (wnd.monoTreasureInfo != null)
            {
                _m_wTreasureInfo = new GGUIWndTreasureHuntTreasureInfo(wnd.monoTreasureInfo);
            }
        }
        
        protected override void _onDiscard()
        {
            _m_iHarvestItemInfo = null;
            
            // 销毁奇物信息子窗口
            _m_wTreasureInfo?.discard();
            _m_wTreasureInfo = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏奇物信息子窗口
            _m_wTreasureInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置奇物信息子窗口
            _m_wTreasureInfo?.resetWnd();
        }


        public void setData(_ATreasureHuntCaptureHarvestItemInfo _harvestItemInfo)
        {
            // 类型检查和转换
            if (_harvestItemInfo is TreasureHuntCaptureHarvestItemInfo_Treasure treasureHarvestInfo)
            {
                _m_iHarvestItemInfo = treasureHarvestInfo;
                _refreshWnd();
            }
            else
            {
                Debug.Log_EditorOnly($"[GGUIWndTreasureHuntCaptureHarvestItem_Treasure] setData failed, wrong type: {_harvestItemInfo?.GetType().Name}");
            }
        }


        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iHarvestItemInfo == null)
                return;

            // 刷新奇物信息显示
            if (_m_wTreasureInfo != null && _m_iHarvestItemInfo.treasureInfo != null)
            {
                _m_wTreasureInfo.showWnd();
                _m_wTreasureInfo.setData(_m_iHarvestItemInfo.treasureInfo);
            }
        }
    }
}