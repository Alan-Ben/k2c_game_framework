using System;
using System.Collections.Generic;
using ALPackage;
using JetBrains.Annotations;
using UnityEngine;

namespace GOE
{
    public class GGUIPrefabSubWndStageGoalPageOverview : _ANPGGUIBasicLoadPrefabSubWnd<GGUIMonoStageGoalPageOverview>
    {
        private readonly Action _m_jumpToFunc;
        private GGUISubWndStageGoalOverviewGrid _m_itemGrid;
        //是否可以设置背景动画
        private bool _m_bCanSetBgAni;

        public GGUIPrefabSubWndStageGoalPageOverview(Transform _parent, Action _jumpToFunc) : base(_parent)
        {
            _m_jumpToFunc = _jumpToFunc;
        }


        protected override string _monoAssetPath { get { return GGUIMonoStageGoalPageOverview.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoStageGoalPageOverview.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        public override bool needDiscardOnSwitch { get { return true; } }


        protected override void _onShowWnd()
        {
            _m_itemGrid?.showWnd();
            //注册列表滚动事件
            _m_itemGrid?.wnd?.scrollRect?.onValueChanged?.AddListener(_onScrollRectValueChg);

            refreshWnd();
            _setBgAni();

            WinMsg.RegisterMsgAct(WinMsgType.ON_STAGE_GOAL_BIG_STEP_REWARD_DRAW, _refreshGridItems);
        }
        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsgAct(WinMsgType.ON_STAGE_GOAL_BIG_STEP_REWARD_DRAW, _refreshGridItems);
            //反注册列表滚动事件
            _m_itemGrid?.wnd?.scrollRect?.onValueChanged?.RemoveAllListeners();
            _m_itemGrid?.hideWnd();
            _m_bCanSetBgAni = false;
        }
        protected override void _onReset()
        {
            _m_itemGrid?.resetWnd();
        }
        protected override void _onDiscard()
        {
            _m_itemGrid?.discard();
            _m_itemGrid = null;
        }
        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoGrid != null)
                _m_itemGrid = new GGUISubWndStageGoalOverviewGrid(wnd.monoGrid, _onJumpToBtnClick);
        }

        public void refreshWnd()
        {
            if (wnd == null || !_m_bIsShow)
                return;

            List<StageGoalBigStepRefObj> allStageGoal = GRefdataCoreMgr.instance.stageGoalBigStepRefCore.refList;
            _m_itemGrid?.refreshWnd(allStageGoal, wnd.privateCount);

        }

        //设置背景动画状态
        private void _setBgAni()
        {
            //获取列表初始展示位置，刷新一次背景动画
            if (_m_itemGrid != null)
                wnd?.aniBg?.sample(1 - _m_itemGrid.getCurShowTargetVerticalRate());

            //设置下一帧才能设置背景动画，避免闪一帧
            ALCommonActionMonoTask.addNextFrameTask(() =>
            {
                _m_bCanSetBgAni = true;
                //再刷新一次背景动画
                if (_m_itemGrid != null)
                    wnd?.aniBg?.sample(1 - _m_itemGrid.getCurShowTargetVerticalRate());
            });
        }

        private void _onJumpToBtnClick()
        {
            _m_jumpToFunc?.Invoke();
        }
        private void _refreshGridItems()
        {
            _m_itemGrid?.forceRefreshAllItem();   
        }

        //列表滚动事件
        private void _onScrollRectValueChg(Vector2 _arg)
        {
            if (!_m_bCanSetBgAni)
                return;

            float value = _m_itemGrid.wnd.scrollRect.verticalNormalizedPosition;

            if(value < 0)
                value = 0;
            else if(value > 1)
                value = 1;

            //设置动画显示帧
            wnd?.aniBg?.sample(value);
        }
    }
}