using ALPackage;
using Common.ActivityEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励界面
    /// </summary>
    public class GGUIWndActivityStepReward : _ANPGGUIBasicWnd<GGUIMonoActivityStepReward>
    {
        private static GGUIWndActivityStepReward _g_instance;
        public static GGUIWndActivityStepReward instance
        {
            get
            {
                if(_g_instance == null)
                    _g_instance = new GGUIWndActivityStepReward();
                return _g_instance;
            }
        }

        //活动信息
        private _ABaseActivityInfo _m_activityInfo;
        //阶段奖励列表
        private GGUIWndActivityStepRewardGrid _m_wStepRewardGrid;
        //定时任务
        private ALCommonEnableTaskController _m_iTickTask;

        public GGUIWndActivityStepReward() : base(EALUIWndLayer.NORMAL)
        {
        }

        protected override string _monoAssetPath { get { return GGUIMonoActivityStepReward.assetPath; } }
        protected override string _monoObjName { get { return GGUIMonoActivityStepReward.objName; } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }
        public override bool needDiscardOnSwitch { get { return true; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_COMMON_ACTIVITY_STATE_CHG, _onActivityStateChg);
            _m_wStepRewardGrid?.hideWnd();
            _m_iTickTask.setDisable();
        }

        protected override void _onReset()
        {
            _m_wStepRewardGrid?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wStepRewardGrid?.discard();
            _m_wStepRewardGrid = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoStepRewardGrid != null)
            {
                _m_wStepRewardGrid = new GGUIWndActivityStepRewardGrid(wnd.monoStepRewardGrid);
                _m_wStepRewardGrid.onClickGetReward += _onClickGetReward;
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_activityId"></param>
        public void setInfo(long _activityId)
        {
            if (wnd == null)
                return;

            //取最后一个活动
            _m_activityInfo = NPPlayer.instance.commonActivityComp.getValidActivityInfoByActivityId(_activityId);

            //刷新阶段奖励列表
            _refreshStepRewardList();

            //显示活动倒计时
            _m_iTickTask.setDisable();
            _m_iTickTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tick, 0.2f);
        }

        /// <summary>
        /// 刷新阶段奖励列表
        /// </summary>
        private void _refreshStepRewardList()
        {
            if (_m_activityInfo == null)
                return;

            GActivityMainRefObj activityMainRef = GRefdataCoreMgr.instance.activityMainRefCore.getRef(_m_activityInfo.activityId);
            _m_wStepRewardGrid?.showWnd();
            _m_wStepRewardGrid?.setInfo(_m_activityInfo.stepRewardInfoList, activityMainRef);
        }

        //检查活动是否还在进行中
        private void _checkActivity()
        {
            //活动不存在或者不在活动期，弹窗提示关闭窗口
            if (_m_activityInfo == null || !_m_activityInfo.isEnable)
            {
                NPMesMgr.instance.showOneBtnMes(
                    TextTranslate.instance.getLanguage(TransKeyConst.common_activity_alreadyEnd_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        _onClickClose(null);
                    });
            }
        }

        //每秒倒计时
        private void _tick()
        {
            if (wnd == null)
                return;

            long leftTimeMs = 0;
            if (_m_activityInfo != null)
                leftTimeMs = _m_activityInfo.closeTimeMs - FpsAndPingMgr.instance.serverTimeTag;

            if (leftTimeMs < 0)
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, "");
            else
                ALUGUICommon.setLabelTxt(wnd.txtLeftTime, TimeUtil.millisecondsToTime_hms(leftTimeMs));
        }

        #region 点击事件

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_ACTIVITY_STEP_REWARD);
        }

        //点击领取奖励
        private void _onClickGetReward(GGUIWndActivityStepRewardGridItem _item)
        {
            if (_item == null || _item.stepRewardInfo == null)
                return;

            GActivityStepRewardRefObj curStepRewardRef = _item.stepRewardInfo.getFirstNotGetRewardStep();
            if (_item.stepRewardInfo.getStepRewardState(curStepRewardRef) != EStepRewardState.CanGet)
                return;

            //请求领取奖励
            NPPlayer.instance.commonActivityComp.reqDrawActivityStepReward(_item.stepRewardInfo.activityInstanceId, _item.stepRewardInfo.stepRewardSetId, curStepRewardRef.step, 
            (_isSuc, _msg) =>
            {
                if(_isSuc)
                    _refreshStepRewardList();
            });
        }

        #endregion

        #region 消息事件

        //活动状态变更
        private void _onActivityStateChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 4 || _m_activityInfo == null)
                return;

            long activityId = (long)_objects[0];
            if (activityId != _m_activityInfo.activityId)
                return;

            _checkActivity();
        }

        #endregion
    }
}