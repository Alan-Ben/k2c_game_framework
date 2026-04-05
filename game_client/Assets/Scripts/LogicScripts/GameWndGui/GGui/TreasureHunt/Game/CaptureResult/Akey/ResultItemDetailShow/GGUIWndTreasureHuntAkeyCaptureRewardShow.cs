using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntAkeyCaptureRewardShow : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoTreasureHuntAkeyCaptureRewardShow>, _ITreasureHuntAkeyCaptureResultItemDetailShowWnd
    {
        private NPCommonAssetPathInfo _m_iAssetPathInfo;
        
        private TreasureHuntCaptureResultReward _m_iRewardResultInfo;
        
        private GGUISubWndCommonItemDetail _m_wItemInfo;

        public GGUIWndTreasureHuntAkeyCaptureRewardShow(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
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

            // 初始化物品信息子窗口
            if (wnd.monoItemInfo != null)
                _m_wItemInfo = new GGUISubWndCommonItemDetail(wnd.monoItemInfo);
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wItemInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wItemInfo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wItemInfo?.discard();
            _m_wItemInfo = null;
            
            _m_iRewardResultInfo = null;
        }

        public _AALBasicLoadUIWndBasicClass wndInstance { get { return this; } }

        public void setData([NotNull] TreasureHuntCaptureResultBase _captureResultBase)
        {
            _m_iRewardResultInfo = _captureResultBase as TreasureHuntCaptureResultReward;
            if (_m_iRewardResultInfo == null)
            {
                Debug.LogError($"[GGUIWndTreasureHuntAkeyCaptureRewardShow setData] {_captureResultBase.gainType}类型无法使用GGUIWndTreasureHuntAkeyCaptureRewardShow进行展示");
                return;
            }
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iRewardResultInfo == null)
                return;

            // 刷新奖励物品信息
            if (_m_wItemInfo != null)
            {
                if (_m_iRewardResultInfo.rewardItemList != null && _m_iRewardResultInfo.rewardItemList.Count > 0)
                {
                    _m_wItemInfo.showWnd();
                    _m_wItemInfo.setShowData(_m_iRewardResultInfo.rewardItemList[0]);       
                }
                else
                {
                    _m_wItemInfo.hideWnd();
                }
            }
        }
    }
}