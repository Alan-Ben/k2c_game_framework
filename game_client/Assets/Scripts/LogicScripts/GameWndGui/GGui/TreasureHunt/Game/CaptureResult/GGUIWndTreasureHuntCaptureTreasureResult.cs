using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 太空寻宝获取奇物结果窗口
    /// </summary>
    public class GGUIWndTreasureHuntCaptureTreasureResult : _ANPGGUIBasicWnd<GGUIMonoTreasureHuntCaptureTreasureResult>
    {
        private static GGUIWndTreasureHuntCaptureTreasureResult _g_instance;
        public static GGUIWndTreasureHuntCaptureTreasureResult instance { get { return _g_instance ??= new GGUIWndTreasureHuntCaptureTreasureResult(); } }

        /// <summary>
        /// 奇物捕捉结果数据
        /// </summary>
        private TreasureHuntCaptureResultTreasure _m_iTreasureResultInfo;

        // 奇物信息子窗口
        private GGUIWndTreasureHuntTreasureInfo _m_wTreasureInfo;
        // 技能信息子窗口
        private GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo> _m_wSkillInfo;

        public GGUIWndTreasureHuntCaptureTreasureResult() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoTreasureHuntCaptureTreasureResult.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoTreasureHuntCaptureTreasureResult.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            // 初始化奇物信息子窗口
            if (wnd.monoTreasureInfo != null)
                _m_wTreasureInfo = new GGUIWndTreasureHuntTreasureInfo(wnd.monoTreasureInfo);

            // 初始化技能信息子窗口
            if (wnd.monoTreasureSkillInfo != null)
                _m_wSkillInfo = new GGUIWndTreasureHuntSkillInfo<GGUIMonoTreasureHuntSkillInfo>(wnd.monoTreasureSkillInfo);

            // 绑定确认按钮点击事件
            ALUGUICommon.combineBtnClick(wnd.btnSure, _onClickSure);
        }

        protected override void _onShowWnd()
        {
            _refreshWnd();
        }

        protected override void _onHideWnd()
        {
            _m_wTreasureInfo?.hideWnd();
            _m_wSkillInfo?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTreasureInfo?.resetWnd();
            _m_wSkillInfo?.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnSure, _onClickSure);
            }

            _m_wTreasureInfo?.discard();
            _m_wTreasureInfo = null;

            _m_wSkillInfo?.discard();
            _m_wSkillInfo = null;

            _m_iTreasureResultInfo = null;
        }

        /// <summary>
        /// 设置奇物捕捉结果数据
        /// </summary>
        /// <param name="_treasureResult">奇物捕捉结果</param>
        public void setData(TreasureHuntCaptureResultTreasure _treasureResult)
        {
            _m_iTreasureResultInfo = _treasureResult;

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
                Debug.LogError($"[GGUIWndTreasureHuntCaptureTreasureResult _refreshWnd] 当前正展示获取奇物:{_m_iTreasureResultInfo.treasureId}窗口, 但是从treasureHuntComponent中获取不到对应的奇物信息");
                return;
            }

            // 刷新奇物信息
            if (_m_wTreasureInfo != null)
            {
                _m_wTreasureInfo.showWnd();
                _m_wTreasureInfo.setData(treasureInfo);
            }

            // 刷新技能信息
            if (_m_wSkillInfo != null)
            {
                if (treasureInfo.skillInfo != null)
                {
                    _m_wSkillInfo.showWnd();
                    _m_wSkillInfo.setData(treasureInfo.skillInfo);
                }
                else
                {
                    _m_wSkillInfo.hideWnd();
                }
            }
        }

        /// <summary>
        /// 点击确认按钮
        /// </summary>
        /// <param name="_go">按钮对象</param>
        private void _onClickSure(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_TREASURE_HUNT_CAPTURE_TREASURE_RESULT);
        }
    }
}