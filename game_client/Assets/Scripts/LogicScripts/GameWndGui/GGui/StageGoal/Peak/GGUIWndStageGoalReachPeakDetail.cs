using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标到达时代之巅详情弹窗
    /// </summary>
    public class GGUIWndStageGoalReachPeakDetail : _ANPGGUIBasicWnd<GGUIMonoStageGoalReachPeakDetail>
    {
        private static GGUIWndStageGoalReachPeakDetail _g_instance;
        public static GGUIWndStageGoalReachPeakDetail instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndStageGoalReachPeakDetail();
                return _g_instance;
            }
        }

        // 列表容器
        private GGUIWndStageGoalReachPeakDetailContainer _m_wReachDetailContainer;
        // 显示操作序列号
        private long _m_lShowSerialize;

        public GGUIWndStageGoalReachPeakDetail() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoStageGoalReachPeakDetail.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalReachPeakDetail.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wReachDetailContainer?.hideWnd();
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wReachDetailContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wReachDetailContainer?.discard();
            _m_wReachDetailContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.monoDetailContainer != null)
                _m_wReachDetailContainer = new GGUIWndStageGoalReachPeakDetailContainer(wnd.monoDetailContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_bigStapId"></param>
        public void setInfo(StageGoalBigStepRefObj _bigStapRef)
        {
            if (_bigStapRef == null || wnd == null)
                return;

            // 设置标题
            ALUGUICommon.setLabelTxt(wnd.txtTitle, _bigStapRef.getTitle);

            // 请求获取列表信息
            _m_lShowSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lShowSerialize;
            _m_wReachDetailContainer?.hideWnd();
            NPPlayer.instance.stageGoalComp.reqStageGoalFirstReachDetailInfo(_bigStapRef.big_step, _msg =>
            {
                if (_msg == null || curSerialize != _m_lShowSerialize)
                    return;

                _m_wReachDetailContainer?.showWnd();
                _m_wReachDetailContainer?.showItemList(_msg.getInfoList());
            });
        }

        // 点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STAGE_GOAL_PEAK_DETAIL);
        }
    }
}