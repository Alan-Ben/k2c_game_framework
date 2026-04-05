using Common.TreasureHuntEnum;

namespace GOE
{
    public class GGUIWndTreasureHuntAkeyCaptureResultItem : _ANPGGUIBasicSubWnd<GGUIMonoTreasureHuntAkeyCaptureResultItem>
    {
        private TreasureHuntCaptureResultBase _m_iCaptureResultInfo;
        
        private _ITreasureHuntAkeyCaptureResultItemDetailShowWnd _m_iDetailShowWnd;
        
        public GGUIWndTreasureHuntAkeyCaptureResultItem(GGUIMonoTreasureHuntAkeyCaptureResultItem _wnd) : base(_wnd)
        {
            initWnd();
        }

        protected override void _onWndInitDone()
        {
        }
        
        protected override void _onDiscard()
        {
            _m_iDetailShowWnd?.wndInstance.discard();
            _m_iDetailShowWnd = null;
        }
        
        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_iDetailShowWnd?.wndInstance.hideWnd();
        }

        protected override void _onReset()
        {
            _m_iDetailShowWnd?.wndInstance.resetWnd();
        }

        public void setData(TreasureHuntCaptureResultBase _captureResultBase)
        {
            _m_iCaptureResultInfo = _captureResultBase;
            
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(wnd == null || !isShow || _m_iCaptureResultInfo == null)
                return;

            NPCommonAssetPathInfo showDetailPathInfo = null;
            switch (_m_iCaptureResultInfo.gainType)
            {
                case ETreasureHuntGainType.ORE:
                    showDetailPathInfo = wnd.showOrePathInfo;
                    break;
                
                case ETreasureHuntGainType.REWARD:
                    showDetailPathInfo = wnd.showRewardPathInfo;
                    break;
                
                case ETreasureHuntGainType.TREASURE:
                    showDetailPathInfo = wnd.showTreasurePathInfo;
                    break;
            }
            
            if (showDetailPathInfo == null)
            {
                Debug.LogError($"[GGUIWndTreasureHuntAkeyCaptureResultItem _refreshWnd] 未配置{_m_iCaptureResultInfo.gainType}类型的详情展示路径", wnd);
                return;
            }

            // 若原来已经存在的详情展示窗口与当前需要展示的详情路径不一致，则销毁原来的窗口
            if (_m_iDetailShowWnd != null && _m_iDetailShowWnd.assetPathInfo != showDetailPathInfo)
            {
                _m_iDetailShowWnd.wndInstance.discard();
                _m_iDetailShowWnd = null;
            }

            if (_m_iDetailShowWnd == null)
            {
                switch (_m_iCaptureResultInfo.gainType)
                {
                    case ETreasureHuntGainType.ORE:
                        _m_iDetailShowWnd = new GGUIWndTreasureHuntAkeyCaptureOreShow(showDetailPathInfo, wnd.detailShowParent);
                        break;
                
                    case ETreasureHuntGainType.REWARD:
                        _m_iDetailShowWnd = new GGUIWndTreasureHuntAkeyCaptureRewardShow(showDetailPathInfo, wnd.detailShowParent);
                        break;
                
                    case ETreasureHuntGainType.TREASURE:
                        _m_iDetailShowWnd = new GGUIWndTreasureHuntAkeyCaptureTreasureShow(showDetailPathInfo, wnd.detailShowParent);
                        break;
                }
                if(null != _m_iDetailShowWnd)
                    _m_iDetailShowWnd.wndInstance.load();
            }

            if (_m_iDetailShowWnd == null)
            {
                Debug.LogError($"[GGUIWndTreasureHuntAkeyCaptureResultItem _refreshWnd] 未实现{_m_iCaptureResultInfo.gainType}类型的详情展示窗口");
                return;
            }
         
            _m_iDetailShowWnd.wndInstance.regLoadDoneDelegate(() =>
            {
                _m_iDetailShowWnd.setData(_m_iCaptureResultInfo);
                _m_iDetailShowWnd.wndInstance.showWnd();
            });
        }
    }
}