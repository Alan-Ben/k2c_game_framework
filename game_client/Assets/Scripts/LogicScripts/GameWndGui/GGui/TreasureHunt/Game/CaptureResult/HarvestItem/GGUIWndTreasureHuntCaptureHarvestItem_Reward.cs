using ALPackage;
using Common.TreasureHuntEnum;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 打捞收获物 - 奖励item窗口
    /// </summary>
    public class GGUIWndTreasureHuntCaptureHarvestItem_Reward : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntCaptureHarvestItem_Reward>, _ITreasureHuntCaptureHarvestItemShowWnd
    {
        private TreasureHuntCaptureHarvestItemInfo_Reward _m_iHarvestItemInfo;

        private NPGGUIWndCommonItem _m_wCommonItem;


        public GGUIWndTreasureHuntCaptureHarvestItem_Reward(GGUIMonoTreasureHuntCaptureHarvestItem_Reward _wnd) : base(_wnd)
        {
            initWnd();
        }


        public _IALBasicUIWndInterface wndInstance { get { return this; } }
        public ETreasureHuntGainType havestType { get { return ETreasureHuntGainType.REWARD; } }


        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 构建通用道具item子窗口
            if (wnd.monoCommonItem != null)
            {
                _m_wCommonItem = new NPGGUIWndCommonItem(wnd.monoCommonItem);
            }
        }
        
        protected override void _onDiscard()
        {
            _m_iHarvestItemInfo = null;
            
            // 销毁通用道具item子窗口
            _m_wCommonItem?.discard();
            _m_wCommonItem = null;
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏通用道具item子窗口
            _m_wCommonItem?.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置通用道具item子窗口
            _m_wCommonItem?.resetWnd();
        }


        public void setData(_ATreasureHuntCaptureHarvestItemInfo _harvestItemInfo)
        {
            // 类型检查和转换
            if (_harvestItemInfo is TreasureHuntCaptureHarvestItemInfo_Reward rewardHarvestInfo)
            {
                _m_iHarvestItemInfo = rewardHarvestInfo;
                _refreshWnd();
            }
            else
            {
                Debug.Log_EditorOnly($"[GGUIWndTreasureHuntCaptureHarvestItem_Reward] setData failed, wrong type: {_harvestItemInfo?.GetType().Name}");
            }
        }


        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iHarvestItemInfo == null)
                return;

            // 刷新通用道具item显示
            if (_m_wCommonItem != null && _m_iHarvestItemInfo.rewardInfo != null)
            {
                _m_wCommonItem.showWnd();
                _m_wCommonItem.setItem(_m_iHarvestItemInfo.rewardInfo);
            }
        }
    }
}