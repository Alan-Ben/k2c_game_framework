using System;
using UnityEngine;
using ALPackage;

namespace GOE
{
    /// <summary>
    /// 活动阶段奖励任务列表item
    /// </summary>
    public class GGUIWndActivityStepRewardGridItem : _ATALUGUIBasicGridItemWnd<GGUIMonoActivityStepRewardGridItem>
    {
        private ActivityStepRewardInfo _m_stepRewardInfo;//阶段奖励信息
        private NPGGUIWndCommonMaskItemContainer _m_rewardItemContainer;//奖励列表
        private Action<GGUIWndActivityStepRewardGridItem> _m_aClickGetReward;//点击领取奖励

        /// <summary>
        /// 阶段奖励信息
        /// </summary>
        public ActivityStepRewardInfo stepRewardInfo { get { return _m_stepRewardInfo; } }
        /// <summary>
        /// 点击领取奖励
        /// </summary>
        public Action<GGUIWndActivityStepRewardGridItem> onClickGetReward { get { return _m_aClickGetReward; } set { _m_aClickGetReward = value; } }

        public GGUIWndActivityStepRewardGridItem(GGUIMonoActivityStepRewardGridItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
        }

        protected override void _onReset()
        {
        }

        protected override void _resetGridItem()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (null != _m_rewardItemContainer)
                _m_rewardItemContainer.discard();
            _m_rewardItemContainer = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.uncombineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.rewardItemContainer)
                _m_rewardItemContainer = new NPGGUIWndCommonMaskItemContainer(wnd.rewardItemContainer);

            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
            ALUGUICommon.combineBtnClick(wnd.btnDetail, _onClickDetail);
        }

        public void setInfo(ActivityStepRewardInfo _info)
        {
            _m_stepRewardInfo = _info;
            _refreshWnd();
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            if (wnd == null || _m_stepRewardInfo == null || _m_stepRewardInfo.stepRewardSetRef == null)
                return;

            GActivityStepRewardRefObj curStepRewardRef = _m_stepRewardInfo.getFirstNotGetRewardStep();
            if (curStepRewardRef == null)
                return;

            //刷新信息展示
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_stepRewardInfo.stepRewardSetRef.step_reward_set_name));
            
            //刷新状态
            EAchievePointProgressState state = EAchievePointProgressState.CAN_NOT_GET;
            if (_m_stepRewardInfo.isAllDone())
                state = EAchievePointProgressState.ALL_DONE;
            else if (_m_stepRewardInfo.getStepRewardState(curStepRewardRef) == EStepRewardState.CanGet)
                state = EAchievePointProgressState.CAN_GET;
            else
                state = EAchievePointProgressState.CAN_NOT_GET;
            if (wnd.stateList != null)
            {
                GGUIAchievePointProgressState curState = null;
                for (int i = 0; i < wnd.stateList.Count; i++)
                {
                    if (wnd.stateList[i] != null && wnd.stateList[i].stete == state)
                    {
                        curState = wnd.stateList[i];
                        break;
                    }
                }
            
                if (curState != null)
                {
                    ALUGUICommon.setGameObjEnable(curState.goShowList, true);
                    ALUGUICommon.setGameObjEnable(curState.goHideList, false);
            
                    //获取对应格式进度字符串
                    string curCountStr = GCommon.getValueFormatStr(_m_stepRewardInfo.stepRewardSetRef.process_num_format, _m_stepRewardInfo.totalScore);
                    string targetCountStr = GCommon.getValueFormatStr(_m_stepRewardInfo.stepRewardSetRef.process_num_format, curStepRewardRef.complete_count);
                    //设置进度文本
                    string valueStr = null;
                    if (state == EAchievePointProgressState.CAN_GET)
                        valueStr = TextTranslate.instance.getLanguage(TransKeyConst.achieve_canGetRewardProcess_num_num, curCountStr, targetCountStr);
                    else
                        valueStr = TextTranslate.instance.getLanguage(TransKeyConst.achieve_canNotGetRewardProcess_num_num, curCountStr, targetCountStr);
                    ALUGUICommon.setLabelTxt(wnd.txtProcess, valueStr);
                    ALUGUICommon.setLabelTxt(wnd.txtCanNotGetProcess, valueStr);
                }
            }
            
            //刷新奖励列表
            if (_m_rewardItemContainer != null)
            {
                _m_rewardItemContainer.showWnd();
                _m_rewardItemContainer.showItemList(curStepRewardRef.reward_item_list, state == EAchievePointProgressState.ALL_DONE, false);
                _m_rewardItemContainer.moveToLeft();
            }
        }

        #region 点击事件

        //点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            if (wnd == null || _m_stepRewardInfo == null)
                return;

            if (_m_aClickGetReward != null)
                _m_aClickGetReward(this);
        }

        //点击查看详情
        private void _onClickDetail(GameObject _go)
        {
            QueueMgr.instance.addNode_InGame_SingleWnd(GGUIWndActivityStepRewardStep.instance, () =>
            {
                GGUIWndActivityStepRewardStep.instance.showWnd();
                GGUIWndActivityStepRewardStep.instance.setInfo(_m_stepRewardInfo);
            }, UINodeTagConst.C_ACTIVITY_STEP_REWARD_STEP);
        }

        #endregion
    }
}
