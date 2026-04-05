using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标任务详情弹窗
    /// </summary>
    public class GGUIWndStageGoalTaskInfo : _ANPGGUIBasicWnd<GGUIMonoStageGoalTaskInfo>
    {
        private static GGUIWndStageGoalTaskInfo _g_instance;
        public static GGUIWndStageGoalTaskInfo instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndStageGoalTaskInfo();
                return _g_instance;
            }
        }

        //任务信息
        private StageGoalTaskItem _m_taskInfo;
        //任务图标
        private NPGGuiWndTexture _m_wTaskIcon;
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wItemContainer;

        public GGUIWndStageGoalTaskInfo() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoStageGoalTaskInfo.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalTaskInfo.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_wTaskIcon?.hideWnd();
            _m_wItemContainer?.hideWnd();
        }

        protected override void _onReset()
        {
            _m_wTaskIcon?.discardTexture();
            _m_wItemContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wTaskIcon?.discard();
            _m_wTaskIcon = null;
            _m_wItemContainer?.discard();
            _m_wItemContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if(wnd.imgIcon != null)
                _m_wTaskIcon = new NPGGuiWndTexture(wnd.imgIcon);

            if (wnd.monoItemContainer != null)
                _m_wItemContainer = new NPGGUIWndCommonItemContainer(wnd.monoItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        public void setInfo(StageGoalTaskItem _taskInfo)
        {
            _m_taskInfo = _taskInfo;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_taskInfo == null || _m_taskInfo.refObj == null)
                return;

            _m_wTaskIcon?.showWnd();
            _m_wTaskIcon?.setTexture(_m_taskInfo.refObj.icon);

            _m_wItemContainer?.showWnd();
            _m_wItemContainer?.showItemList(_m_taskInfo.refObj.reward_item_list);

            ALUGUICommon.setLabelTxt(wnd.txtDesc, _m_taskInfo.refObj.getDesc);
        }

        //点击前往
        private void _onClickGoTo(GameObject _go)
        {
            if (_m_taskInfo == null || _m_taskInfo.refObj == null || _m_taskInfo.refObj.go_to == null || _m_taskInfo.refObj.go_to.isEmpty)
                return;

            _m_taskInfo.refObj.go_to.dealEffect();
        }
    }
}