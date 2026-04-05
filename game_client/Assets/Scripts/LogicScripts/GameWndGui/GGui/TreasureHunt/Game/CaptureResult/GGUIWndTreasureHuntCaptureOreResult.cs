using ALPackage;

namespace GOE
{
    /// <summary>
    /// 太空寻宝获取矿石结果窗口
    /// </summary>
    public class GGUIWndTreasureHuntCaptureOreResult : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntCaptureOreResult>
    {
        private static GGUIWndTreasureHuntCaptureOreResult _g_instance;
        public static GGUIWndTreasureHuntCaptureOreResult instance { get { return _g_instance ??= new GGUIWndTreasureHuntCaptureOreResult(); } }

        /// <summary>
        /// 矿石捕捉结果数据
        /// </summary>
        private TreasureHuntCaptureResultOre _m_iOreResultInfo;

        // 矿石信息子窗口
        private GGUIWndTreasureHuntOreInfo _m_wOreInfo;
        // 技能信息子窗口
        private GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo> _m_wSkillInfo;

        public GGUIWndTreasureHuntCaptureOreResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntCaptureOreResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntCaptureOreResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化矿石信息子窗口
            if (wnd.monoOreInfo != null)
                _m_wOreInfo = new GGUIWndTreasureHuntOreInfo(wnd.monoOreInfo);

            // 初始化技能信息子窗口
            if (wnd.monoOreSkillInfo != null)
                _m_wSkillInfo = new GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo>(wnd.monoOreSkillInfo);

            // 绑定确认按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickSure);
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wOreInfo?.hideWnd();
            _m_wSkillInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wOreInfo?.resetWnd();
            _m_wSkillInfo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickSure);
            }

            _m_wOreInfo?.discard();
            _m_wOreInfo = null;

            _m_wSkillInfo?.discard();
            _m_wSkillInfo = null;

            _m_iOreResultInfo = null;
        }

        /// <summary>
        /// 设置矿石捕捉结果数据
        /// </summary>
        /// <param name="_oreResult">矿石捕捉结果</param>
        public void setData(TreasureHuntCaptureResultOre _oreResult)
        {
            _m_iOreResultInfo = _oreResult;

            _refreshWnd();
        }

        /// <summary>
        /// 刷新窗口显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || !isShow || _m_iOreResultInfo == null || _m_iOreResultInfo.oreInfo == null)
                return;

            if (_m_wOreInfo != null)
            {
                _m_wOreInfo.showWnd();
                _m_wOreInfo.setData(_m_iOreResultInfo.oreInfo);
            }
            
            ALUGUICommon.setLabelTxt(wnd.txtServerOreMassRecord, TextTranslate.instance.getLanguage(TransKeyConst.treasureHunt_serverMaxOreMass_num, TreasureHuntUtil.getOreMassShowStr(_m_iOreResultInfo.serverMaxWeight)));
            
            GCommon.reqPlayerInfo(_m_iOreResultInfo.serverMaxCid, (playerInfo) =>
            {
                if (playerInfo == null || wnd == null)
                    return;
                
                ALUGUICommon.setLabelTxt(wnd.txtServerOreMassRecordOwnerName, playerInfo.name);
            });

            if (_m_wSkillInfo != null && _m_iOreResultInfo.oreInfo.oreRefObj != null)
            {
                // 达到高级矿石质量时, 展示高级技能
                if (_m_iOreResultInfo.oreInfo.oreRefObj.isReachAdvanceOreMass(_m_iOreResultInfo.weight))
                {
                    _m_wSkillInfo.setData(_m_iOreResultInfo.oreInfo.advanceSkillInfo);   
                }
                else//否则展示普通技能
                {
                    _m_wSkillInfo.setData(_m_iOreResultInfo.oreInfo.normalSkillInfo);   
                }
                _m_wSkillInfo.showWnd();
            }
            
            bool isFirstCapture = _m_iOreResultInfo.isFirstCapture;
            ALUGUICommon.setGameObjEnable(wnd.firstGotShowList, isFirstCapture);
            ALUGUICommon.setGameObjEnable(wnd.firstGotHideList, !isFirstCapture);
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go">按钮对象</param>
        private void _onClickSure(UnityEngine.GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_CAPTURE_ORE_RESULT);
        }
    }
}