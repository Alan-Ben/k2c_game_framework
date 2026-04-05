using ALPackage;
using Common.TreasureHuntEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝 - 打捞收获物item（根据类型动态加载对应的展示窗口）
    /// </summary>
    public class GGUIWndTreasureHuntCaptureHarvestItem : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntCaptureHarvestItem>
    {
        private _ATreasureHuntCaptureHarvestItemInfo _m_iHarvestItemInfo;

        private _ITreasureHuntCaptureHarvestItemShowWnd _m_wCurrentShowWnd;


        public GGUIWndTreasureHuntCaptureHarvestItem(GGUIMonoTreasureHuntCaptureHarvestItem _wnd) : base(_wnd)
        {
            initWnd();
        }


        protected override void _onWndInitDone()
        {
            // 不在这里预先构建子窗口，等待setData时根据类型动态创建
        }
        
        protected override void _onDiscard()
        {
            _m_iHarvestItemInfo = null;
            
            // 销毁当前显示的子窗口
            _discardCurrentShowWnd();
        }
        
        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            // 隐藏当前显示的子窗口
            _m_wCurrentShowWnd?.wndInstance.hideWnd();
        }

        protected override void _onReset()
        {
            // 重置当前显示的子窗口
            _m_wCurrentShowWnd?.wndInstance.resetWnd();
        }

        public void setData(_ATreasureHuntCaptureHarvestItemInfo _harvestItemInfo)
        {
            if (_harvestItemInfo == null)
            {
                Debug.Log_EditorOnly("[GGUIWndTreasureHuntCaptureHarvestItem] setData failed, harvestItemInfo is null");
                return;
            }

            _m_iHarvestItemInfo = _harvestItemInfo;

            // 如果当前显示的窗口类型与新数据类型不一致，需要切换窗口
            if (_m_wCurrentShowWnd == null || _m_wCurrentShowWnd.havestType != _harvestItemInfo.havestType)
            {
                _createShowWndByType(_harvestItemInfo.havestType);
            }

            _refreshWnd();
        }


        /// <summary>
        /// 根据类型创建对应的展示窗口
        /// </summary>
        private void _createShowWndByType(ETreasureHuntGainType _type)
        {
            // 先销毁旧窗口
            _discardCurrentShowWnd();
            if (wnd == null)
                return;

            // 根据类型创建新窗口
            switch (_type)
            {
                case ETreasureHuntGainType.ORE:
                    if (wnd.showOrePrefab != null)
                    {
                        GameObject oreGo = Object.Instantiate(wnd.showOrePrefab.gameObject, wnd.itemShowParent);
                        if (oreGo != null)
                        {
                            GGUIMonoTreasureHuntCaptureHarvestItem_Ore oreMono = oreGo.GetComponent<GGUIMonoTreasureHuntCaptureHarvestItem_Ore>();
                            if (oreMono != null)
                            {
                                _m_wCurrentShowWnd = new GGUIWndTreasureHuntCaptureHarvestItem_Ore(oreMono);
                            }
                        }
                    }
                    break;

                case ETreasureHuntGainType.REWARD:
                    if (wnd.showRewardPrefab != null)
                    {
                        GameObject rewardGo = Object.Instantiate(wnd.showRewardPrefab.gameObject, wnd.itemShowParent);
                        if (rewardGo != null)
                        {
                            GGUIMonoTreasureHuntCaptureHarvestItem_Reward rewardMono = rewardGo.GetComponent<GGUIMonoTreasureHuntCaptureHarvestItem_Reward>();
                            if (rewardMono != null)
                            {
                                _m_wCurrentShowWnd = new GGUIWndTreasureHuntCaptureHarvestItem_Reward(rewardMono);
                            }
                        }
                    }
                    break;

                case ETreasureHuntGainType.TREASURE:
                    if (wnd.showTreasurePrefab != null)
                    {
                        GameObject treasureGo = Object.Instantiate(wnd.showTreasurePrefab.gameObject, wnd.itemShowParent);
                        if (treasureGo != null)
                        {
                            GGUIMonoTreasureHuntCaptureHarvestItem_Treasure treasureMono = treasureGo.GetComponent<GGUIMonoTreasureHuntCaptureHarvestItem_Treasure>();
                            if (treasureMono != null)
                            {
                                _m_wCurrentShowWnd = new GGUIWndTreasureHuntCaptureHarvestItem_Treasure(treasureMono);
                            }
                        }
                    }
                    break;

                default:
                    Debug.Log_EditorOnly($"[GGUIWndTreasureHuntCaptureHarvestItem] Unknown harvest type: {_type}");
                    break;
            }
        }

        /// <summary>
        /// 销毁当前显示的子窗口
        /// </summary>
        private void _discardCurrentShowWnd()
        {
            if (_m_wCurrentShowWnd != null)
            {
                GameObject go = _m_wCurrentShowWnd.wndInstance.getGameObj();
                _m_wCurrentShowWnd.wndInstance.discard();
                
                if (go != null)
                {
                    Object.Destroy(go);
                }

                _m_wCurrentShowWnd = null;
            }
        }

        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iHarvestItemInfo == null)
                return;

            // 刷新当前显示的子窗口
            if (_m_wCurrentShowWnd != null)
            {
                _m_wCurrentShowWnd.wndInstance.showWnd();
                _m_wCurrentShowWnd.setData(_m_iHarvestItemInfo);       
            }
        }
    }
}