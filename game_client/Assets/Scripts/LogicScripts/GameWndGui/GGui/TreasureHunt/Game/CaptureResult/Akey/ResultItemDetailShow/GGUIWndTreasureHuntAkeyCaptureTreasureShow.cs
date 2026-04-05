using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntAkeyCaptureTreasureShow : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoTreasureHuntAkeyCaptureTreasureShow>, _ITreasureHuntAkeyCaptureResultItemDetailShowWnd
    {
        private NPCommonAssetPathInfo _m_iAssetPathInfo;
        
        private TreasureHuntCaptureResultTreasure _m_iTreasureResultInfo;
        
        private GGUIWndTreasureHuntTreasureInfo _m_wTreasureInfo;

        public GGUIWndTreasureHuntAkeyCaptureTreasureShow(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
        {
            _m_iAssetPathInfo = _assetPathInfo;
        }

        protected override string _monoAssetPath => _m_iAssetPathInfo?.asset_path;
        protected override string _monoObjName => _m_iAssetPathInfo?.obj_name;
        protected override _AALResourceCore _resourceCore => GameResCore.instance;
        
        public NPCommonAssetPathInfo assetPathInfo { get { return _m_iAssetPathInfo; } }
        
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化奇物信息子窗口
            if (wnd.monoTreasureInfo != null)
                _m_wTreasureInfo = new GGUIWndTreasureHuntTreasureInfo(wnd.monoTreasureInfo);
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wTreasureInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTreasureInfo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wTreasureInfo?.discard();
            _m_wTreasureInfo = null;
            
            _m_iTreasureResultInfo = null;
        }

        public _AALBasicLoadUIWndBasicClass wndInstance { get { return this; } }

        public void setData([NotNull] TreasureHuntCaptureResultBase _captureResultBase)
        {
            _m_iTreasureResultInfo = _captureResultBase as TreasureHuntCaptureResultTreasure;
            if (_m_iTreasureResultInfo == null)
            {
                Debug.LogError($"[GGUIWndTreasureHuntAkeyCaptureTreasureShow setData] {_captureResultBase.gainType}类型无法使用GGUIWndTreasureHuntAkeyCaptureTreasureShow进行展示");
                return;
            }
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iTreasureResultInfo == null)
                return;

            // 因为奇物是唯一的, 只能获取一次, 所以奇物数据可以直接从组件中获取, 和获取矿石数据不一样
            TreasureHuntGotTreasureInfo treasureInfo = NPPlayer.instance.treasureHuntComponent.getGotTreasureInfo(_m_iTreasureResultInfo.treasureId);
            if (treasureInfo == null)
            {
                Debug.LogError($"[GGUIWndTreasureHuntAkeyCaptureTreasureShow _refreshWnd] 获取奇物信息失败, treasureId:{_m_iTreasureResultInfo.treasureId}");
                return;
            }

            // 刷新奇物信息
            if (_m_wTreasureInfo != null)
            {
                _m_wTreasureInfo.showWnd();
                _m_wTreasureInfo.setData(treasureInfo);
            }
        }
    }
}