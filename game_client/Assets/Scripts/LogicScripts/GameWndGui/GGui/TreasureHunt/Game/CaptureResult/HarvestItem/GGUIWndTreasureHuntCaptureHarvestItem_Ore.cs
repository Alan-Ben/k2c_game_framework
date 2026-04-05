using ALPackage;
using Common.TreasureHuntEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 打捞收获物 - 矿石item窗口
    /// </summary>
    public class GGUIWndTreasureHuntCaptureHarvestItem_Ore : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntCaptureHarvestItem_Ore>, _ITreasureHuntCaptureHarvestItemShowWnd
    {
        private TreasureHuntCaptureHarvestItemInfo_Ore _m_iHarvestItemInfo;

        private GGUIWndTreasureHuntOreInfo _m_wOreInfo;


        public GGUIWndTreasureHuntCaptureHarvestItem_Ore(GGUIMonoTreasureHuntCaptureHarvestItem_Ore _wnd) : base(_wnd)
        {
            initWnd();
        }

        public _IALBasicUIWndInterface wndInstance { get { return this; } }
        public ETreasureHuntGainType havestType { get { return ETreasureHuntGainType.ORE; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建矿石信息子窗口
            if (wnd.monoOreInfo != null)
            {
                _m_wOreInfo = new GGUIWndTreasureHuntOreInfo(wnd.monoOreInfo);
            }
        }
        
        protected override void _onDiscard()
        {
            _m_iHarvestItemInfo = null;
            
            // 销毁矿石信息子窗口
            _m_wOreInfo?.discard();
            _m_wOreInfo = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏矿石信息子窗口
            _m_wOreInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置矿石信息子窗口
            _m_wOreInfo?.resetWnd();
        }


        public void setData(_ATreasureHuntCaptureHarvestItemInfo _harvestItemInfo)
        {
            // 类型检查和转换
            if (_harvestItemInfo is TreasureHuntCaptureHarvestItemInfo_Ore oreHarvestInfo)
            {
                _m_iHarvestItemInfo = oreHarvestInfo;
                _refreshWnd();
            }
            else
            {
                Debug.Log_EditorOnly($"[GGUIWndTreasureHuntCaptureHarvestItem_Ore] setData failed, wrong type: {_harvestItemInfo?.GetType().Name}");
            }
        }


        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iHarvestItemInfo == null)
                return;

            // 刷新矿石信息显示
            if (_m_wOreInfo != null)
            {
                _m_wOreInfo.showWnd();
                _m_wOreInfo.setData(_m_iHarvestItemInfo.oreInfo);
            }
        }
    }
}