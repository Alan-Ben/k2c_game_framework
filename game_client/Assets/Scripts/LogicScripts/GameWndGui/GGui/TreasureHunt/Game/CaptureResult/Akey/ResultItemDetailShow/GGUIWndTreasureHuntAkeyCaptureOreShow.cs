using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIWndTreasureHuntAkeyCaptureOreShow : _ATALBasicLoadPrefabSubUIWnd<GGUIMonoTreasureHuntAkeyCaptureOreShow>, _ITreasureHuntAkeyCaptureResultItemDetailShowWnd
    {
        private NPCommonAssetPathInfo _m_iAssetPathInfo;
        
        private TreasureHuntCaptureResultOre _m_iOreResultInfo;
        
        private GGUIWndTreasureHuntOreInfo _m_wOreInfo;

        public GGUIWndTreasureHuntAkeyCaptureOreShow(NPCommonAssetPathInfo _assetPathInfo, Transform _parent) : base(_parent)
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

            // 初始化矿石信息子窗口
            if (wnd.monoOreInfo != null)
                _m_wOreInfo = new GGUIWndTreasureHuntOreInfo(wnd.monoOreInfo);
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wOreInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOreInfo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wOreInfo?.discard();
            _m_wOreInfo = null;
            
            _m_iOreResultInfo = null;
        }

        public _AALBasicLoadUIWndBasicClass wndInstance { get { return this; } }

        public void setData([NotNull] TreasureHuntCaptureResultBase _captureResultBase)
        {
            _m_iOreResultInfo = _captureResultBase as TreasureHuntCaptureResultOre;
            if (_m_iOreResultInfo == null)
            {
                Debug.LogError($"[GGUIWndTreasureHuntAkeyCaptureOreShow setData] {_captureResultBase.gainType}类型无法使用GGUIWndTreasureHuntAkeyCaptureOreShow进行展示");
                return;
            }
            
            _refreshWnd();
        }
        
        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iOreResultInfo == null || _m_iOreResultInfo.oreInfo == null)
                return;


            // 刷新矿石信息
            if (_m_wOreInfo != null)
            {
                _m_wOreInfo.showWnd();
                _m_wOreInfo.setData(_m_iOreResultInfo.oreInfo);
            }

            // 刷新矿石质量信息
            ALUGUICommon.setLabelTxt(wnd.txtServerOreMassRecord, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_serverMaxOreMass_num, TreasureHuntUtil.getOreMassShowStr(_m_iOreResultInfo.serverMaxWeight)));
            
            // 异步获取服务器记录拥有者名称
            GCommon.reqPlayerInfo(_m_iOreResultInfo.serverMaxCid, (playerInfo) =>
            {
                if (playerInfo == null || wnd == null)
                    return;
                
                ALUGUICommon.setLabelTxt(wnd.txtServerOreMassRecordOwnerName, playerInfo.name);
            });

            // 控制首次获得显示/隐藏
            bool isFirstCapture = _m_iOreResultInfo.isFirstCapture;
            ALUGUICommon.setGameObjEnable(wnd.firstGotShowList, isFirstCapture);
            ALUGUICommon.setGameObjEnable(wnd.firstGotHideList, !isFirstCapture);
        }
    }
}