using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励步骤界面
    /// </summary>
    public class GGUIWndActivityStepRewardStep : _ANPGGUIBasicWnd<GGUIMonoActivityStepRewardStep>
    {
        private static GGUIWndActivityStepRewardStep _g_instance = new GGUIWndActivityStepRewardStep();
        public static GGUIWndActivityStepRewardStep instance
        {
            get
            {
                if(null == _g_instance)
                    _g_instance = new GGUIWndActivityStepRewardStep();
                return _g_instance;
            }
        }

        private GGUIWndActivityStepRewardStepGrid _m_wStepRewardGrid;//活动阶段奖励步骤列表
        private ActivityStepRewardInfo _m_info;//活动阶段奖励信息

        public GGUIWndActivityStepRewardStep() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoActivityStepRewardStep.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoActivityStepRewardStep.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if (_m_wStepRewardGrid != null)
                _m_wStepRewardGrid.hideWnd();
        }

        protected override void _onReset()
        {
            if (_m_wStepRewardGrid != null)
                _m_wStepRewardGrid.resetWnd();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_wStepRewardGrid != null)
                _m_wStepRewardGrid.discard();
            _m_wStepRewardGrid = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoStepGrid != null)
                _m_wStepRewardGrid = new GGUIWndActivityStepRewardStepGrid(wnd.monoStepGrid);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickCloseBtn);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(ActivityStepRewardInfo _info)
        {
            if (_info == null)
                return;

            _m_info = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (_m_wStepRewardGrid != null)
            {
                _m_wStepRewardGrid.showWnd();
                _m_wStepRewardGrid.setInfo(_m_info);
            }
        }

        //点击关闭按钮
        private void _onClickCloseBtn(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ACTIVITY_STEP_REWARD_STEP);
        }
    }
}
