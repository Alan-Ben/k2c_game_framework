using System;
using System.Collections.Generic;
using UnityEngine;
using ALPackage;
using UnityEngine.UI;

namespace GOE
{
    /// <summary>
    /// 每日任务信息item
    /// </summary>
    public class GGUIWndDailyQuestContainerItem : _ATALBasicUISubWnd<GGUIMonoDailyQuestContainerItem>
    {
        private DailyQuestItem _m_dailyQuestInfo;//每日任务信息
        private GGUIWndCommonRewardContainer _m_rewardItemContainer;//每日任务奖励列表
        private NPGGuiWndTexture _m_wIcon;//图标
        private Action<GGUIWndDailyQuestContainerItem, RectTransform> _m_aOnClickGetReward;//点击领取奖励事件
        private bool _m_isShowOnceGet;//是否显示一键领取
        private Action _m_aOnClickOnceGetReward;//一键领取奖励事件

        private ALCommonEnableTaskController _m_task;//刷新任务

        /// <summary>
        /// 点击领取奖励事件
        /// </summary>
        public Action<GGUIWndDailyQuestContainerItem, RectTransform> onClickGetReward
        {
            get { return _m_aOnClickGetReward; }
            set { _m_aOnClickGetReward = value; }
        }

        /// <summary>
        /// 点击一键领取奖励事件
        /// </summary>
        public Action onClickOnceGetReward
        {
            get { return _m_aOnClickOnceGetReward; }
            set { _m_aOnClickOnceGetReward = value; }
        }

        public DailyQuestItem dailyQuestInfo { get { return _m_dailyQuestInfo; } }

        public GGUIWndDailyQuestContainerItem(GGUIMonoDailyQuestContainerItem _wnd) : base(_wnd)
        {
        }

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            if(_m_rewardItemContainer != null)
                _m_rewardItemContainer.hideWnd();

            if (_m_wIcon != null)
                _m_wIcon.hideWnd();

            _m_task.setDisable();
        }

        protected override void _onReset()
        {
            if (_m_rewardItemContainer != null)
                _m_rewardItemContainer.resetWnd();

            if (_m_wIcon != null)
                _m_wIcon.discardTexture();
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (null != _m_rewardItemContainer)
                _m_rewardItemContainer.discard();
            _m_rewardItemContainer = null;

            if (_m_wIcon != null)
                _m_wIcon.discard();
            _m_wIcon = null;

            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);//点击领取奖励
            ALUGUICommon.uncombineBtnClick(wnd.btnOnceGetReward, _onClickOnceGetReward);//点击一键领取奖励
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoTo);//点击前往
        }

        protected override void _onWndInitDone()
        {
            if (null == wnd)
                return;

            if (null != wnd.rewardItemContainer)
                _m_rewardItemContainer = new GGUIWndCommonRewardContainer(wnd.rewardItemContainer);

            if (wnd.imgIcon != null)
                _m_wIcon = new NPGGuiWndTexture(wnd.imgIcon);

            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);//点击领取奖励
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoTo);//点击前往
            ALUGUICommon.combineBtnClick(wnd.btnOnceGetReward, _onClickOnceGetReward);//点击一键领取奖励

        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(DailyQuestItem _info,bool _isShowOnceGet)
        {
            _m_dailyQuestInfo = _info;
            _m_isShowOnceGet = _isShowOnceGet;
            //重置动画
            if(wnd != null && wnd.getRewardAni != null && !string.IsNullOrEmpty(wnd.getRewardAniName))
                wnd.getRewardAni.Sample(wnd.getRewardAniName, 0);
            //重置任务
            _m_task.setDisable();

            if (wnd != null && wnd.transform)
                LayoutRebuilder.MarkLayoutForRebuild((RectTransform)wnd.transform);

            _refreshWnd();
        }

        /// <summary>
        /// 播放领取奖励动画
        /// </summary>
        /// <param name="_onPlayDone"></param>
        public void playGetRewardAnimation(Action _onPlayDone = null)
        {
            if (wnd == null || wnd.getRewardAni == null || string.IsNullOrEmpty(wnd.getRewardAniName))
            {
                _onPlayDone?.Invoke();
                return;
            }

            //开启任务刷新列表
            _m_task.setDisable();
            _m_task = ALCommonTaskController.CommonEnableTickActionAddMonoTask(() =>
            {
                if(wnd != null && wnd.transform)
                    LayoutRebuilder.MarkLayoutForRebuild((RectTransform)wnd.transform);
            });

            //播放动画
            wnd.getRewardAni.Play(wnd.getRewardAniName, ()=>
            {
                _m_task.setDisable();
                _onPlayDone?.Invoke();
            });
        }

        /// <summary>
        /// 刷新显示
        /// </summary>
        private void _refreshWnd()
        {
            _refreshInfo();
            _refreshState();
        }

        //刷新信息
        private void _refreshInfo()
        {
            if (wnd == null || _m_dailyQuestInfo == null || _m_dailyQuestInfo.dailyQuestRef == null)
                return;

            //图标
            if (_m_wIcon != null)
            {
                _m_wIcon.showWnd();
                _m_wIcon.setTexture(_m_dailyQuestInfo.dailyQuestRef.icon);
            }

            //描述
            ALUGUICommon.setLabelTxt(wnd.txtName, _m_dailyQuestInfo.dailyQuestRef.daily_quest_title);

            //奖励列表
            List<NPCommonCostItem> rewardItemList = new List<NPCommonCostItem>();
            if(_m_dailyQuestInfo.dailyQuestRef.reward_item_list != null)
                rewardItemList.AddRange(_m_dailyQuestInfo.dailyQuestRef.reward_item_list);
            if(_m_dailyQuestInfo.dailyQuestRef.activation_item != null)
                rewardItemList.Add(_m_dailyQuestInfo.dailyQuestRef.activation_item);
            EDailyQuestState curState = _m_dailyQuestInfo.getDailyQuestState();
            ECommonRewardType rewardType = ECommonRewardType.NOT_GET_REWARD;
            switch (curState)
            {
                case EDailyQuestState.CAN_GET:
                    rewardType = ECommonRewardType.CAN_GET_REWARD;
                    break;
                case EDailyQuestState.CAN_NOT_GET:
                    rewardType = ECommonRewardType.NOT_GET_REWARD;
                    break;
                case EDailyQuestState.LOCK:
                    rewardType = ECommonRewardType.NOT_GET_REWARD;
                    break;
                case EDailyQuestState.ALREADY_GET:
                    rewardType = ECommonRewardType.HAS_GET_REWARD;
                    break;
            }
            if (_m_rewardItemContainer != null)
            {
                _m_rewardItemContainer.showWnd();
                _m_rewardItemContainer.setRewardList(rewardItemList, rewardType);
            }
        }

        //刷新进度
        private void _refreshProcess(GGUIDailyQuestState _state)
        {
            if (wnd == null || _m_dailyQuestInfo == null || _m_dailyQuestInfo.dailyQuestRef == null)
                return;

            long curCount = _m_dailyQuestInfo.curFinallyCount;
            long targetCount = _m_dailyQuestInfo.dailyQuestRef.process_count;
            string curCountStr = GCommon.getValueFormatStr(_m_dailyQuestInfo.dailyQuestRef.process_num_format, curCount);
            string targetCountStr = GCommon.getValueFormatStr(_m_dailyQuestInfo.dailyQuestRef.process_num_format, targetCount);
            curCountStr = GCommon.addSizeForRichText(curCountStr, wnd.curProcessTextSize);
            curCountStr = GCommon.addColorForRichText(curCountStr, _state.txtColor);
            ALUGUICommon.setLabelTxt(wnd.txtProcess, TextTranslate.instance.getLanguage(TransKeyConst.common_useNum_num_num, curCountStr, targetCountStr));
        }

        //刷新状态
        private void _refreshState()
        {
            if (wnd == null || _m_dailyQuestInfo == null)
                return;

            //展示任务状态
            EDailyQuestState curState = _m_dailyQuestInfo.getDailyQuestState();
            GGUIDailyQuestState targetState = null;
            if (wnd.stateList != null)
            {
                for (int i = 0; i < wnd.stateList.Count; i++)
                {
                    if (wnd.stateList[i] != null && wnd.stateList[i].state == curState)
                    {
                        targetState = wnd.stateList[i];
                        break;
                    }
                }
            }

            if (curState == EDailyQuestState.CAN_GET && _m_isShowOnceGet)
            {
                ALUGUICommon.setGameObjEnable(wnd.onceGetShowGoList, true);
                ALUGUICommon.setGameObjEnable(wnd.onceGetHideGoList, false);
            }
            else
            {
                ALUGUICommon.setGameObjEnable(wnd.onceGetShowGoList, false);
                ALUGUICommon.setGameObjEnable(wnd.onceGetHideGoList, true);
                ALUGUICommon.setGameObjEnable(targetState.goHideList, false);
                ALUGUICommon.setGameObjEnable(targetState.goShowList, true);
            }
            _refreshProcess(targetState);
        }

        //模拟点击领取奖励
        public void simulateClickClickGetReward()
        {
            _onClickGetReward(null);
        }
        
        #region 点击事件
        
        //点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            if (wnd == null || _m_dailyQuestInfo == null || _m_dailyQuestInfo.getDailyQuestState() != EDailyQuestState.CAN_GET)
                return;

            if (_m_aOnClickGetReward != null)
                _m_aOnClickGetReward(this, wnd.particleStartTrans);
        }

        //一键领取
        private void _onClickOnceGetReward(GameObject _go)
        {
            if (wnd == null || _m_dailyQuestInfo == null || _m_dailyQuestInfo.getDailyQuestState() != EDailyQuestState.CAN_GET)
                return;

            if (_m_aOnClickOnceGetReward != null)
                _m_aOnClickOnceGetReward();
        }


        //点击前往
        private void _onClickGoTo(GameObject _go)
        {
            if (wnd == null || _m_dailyQuestInfo == null || _m_dailyQuestInfo.dailyQuestRef == null)
                return;

            if (_m_dailyQuestInfo.canGetReward)
                return;
            
            //执行跳转效果
            NPSimpleTutorialRefObj simpleTutorialRefObj = GRefdataCoreMgr.instance.simpleTutorialRefCore.getRef(_m_dailyQuestInfo.dailyQuestRef.go_to_simple_tutorial_id);
            if (null == simpleTutorialRefObj)
            {
                _m_dailyQuestInfo.dailyQuestRef.go_to?.dealEffect();
                return;
            }
            
            //设置简易引导
            SimpleTutorialController.instance.setCurSimpleGuide(simpleTutorialRefObj);
            //如果还不在引导中，触发简易引导
            if(Game.instance.isInTutorial)
                return;

            //如果没执行成功，执行默认跳转
            if (!SimpleTutorialController.instance.checkStartSimpleTutorial(QueueMgr.instance._lastNode.nodeTag))
            {
                _m_dailyQuestInfo.dailyQuestRef.go_to?.dealEffect();
            }
        }

        #endregion
    }
}
