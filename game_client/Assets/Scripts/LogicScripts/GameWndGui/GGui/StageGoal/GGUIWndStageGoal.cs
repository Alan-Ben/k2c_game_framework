
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 阶段目标界面逻辑
    /// </summary>
    public class GGUIWndStageGoal : _ANPGGUIBasicWnd<GGUIMonoStageGoal>
    {
        [NotNull] public static GGUIWndStageGoal instance { get { return _g_instance ??= new GGUIWndStageGoal(); } }
        private static GGUIWndStageGoal _g_instance;


        private GGUISubWndStageGoalPageTabList _m_pageTabList;
        

        public GGUIWndStageGoal()
            : base(EALUIWndLayer.NORMAL)
        {
        }
        

        protected override string _monoAssetPath { get { return GGUIMonoStageGoal.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoal.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }


        protected override void _onShowWnd()
        {
            _m_pageTabList?.showWnd();
        }
        protected override void _onHideWnd()
        {
            _m_pageTabList?.hideWnd();
        }
        protected override void _onReset()
        {
            _m_pageTabList?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_pageTabList?.discard();
            _m_pageTabList = null;

            if (wnd == null)
                return;
            
            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoPageTabList != null)
                _m_pageTabList = new GGUISubWndStageGoalPageTabList(wnd.monoPageTabList);
            
            ALUGUICommon.combineBtnClick(wnd.btnClose, _onCloseBtnClick);
        }

        public void setDefaultTab()
        {
            if (wnd == null)
                return;

            // 判断有没有播放过新阶段解锁动画并且总览界面已解锁，没有播放过并且已解锁则打开总览界面，否则打开默认界面
            // StageGoalBigStepRefObj curBigStageRefObj = NPPlayer.instance.stageGoalComp.bigStepRefObj;
            // bool showOverview = curBigStageRefObj != null && 
            //                     curBigStageRefObj.big_step > AccountSettingMgr.instance.accountSetting.alreadyShowUnlockAniBigStageGoalId &&
            //                     wnd.overviewSimpleUnlockId >  0 &&
            //                     GCommon.isSimpleUnlock(wnd.overviewSimpleUnlockId);

            if (_m_pageTabList != null && _m_pageTabList.wnd != null)
            {
                // GGUIMonoStageGoalTabType targetTabType = showOverview ? GGUIMonoStageGoalTabType.OVERVIEW : _m_pageTabList.wnd.defaultTabType;
                GGUIMonoStageGoalTabType targetTabType = _m_pageTabList.wnd.defaultTabType;
                targetTabType = _checkCanOpenTaskPage(targetTabType);
                _m_pageTabList.setSelectTab(targetTabType);
            }   
        }

        public void setSelectTab(GGUIMonoStageGoalTabType _tabType)
        {
            GGUIMonoStageGoalTabType curSelectTab = _checkCanOpenTaskPage(_tabType);

            if (_m_pageTabList != null)
                _m_pageTabList.setSelectTab(curSelectTab);
        }

        /// <summary>
        /// 检查是否可以打开任务页面
        /// </summary>
        /// <param name="_tabType"></param>
        /// <returns></returns>
        private GGUIMonoStageGoalTabType _checkCanOpenTaskPage(GGUIMonoStageGoalTabType _tabType)
        {
            //检查任务是否全部完成，完成则切换到总览界面
            GGUIMonoStageGoalTabType curSelectTab = _tabType;
            if (_tabType == GGUIMonoStageGoalTabType.TASK && NPPlayer.instance.stageGoalComp.isAllDone)
                curSelectTab = GGUIMonoStageGoalTabType.OVERVIEW;
            return curSelectTab;
        }

        private void _onCloseBtnClick(GameObject _)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_STAGE_GOAL);
        }
    }
}