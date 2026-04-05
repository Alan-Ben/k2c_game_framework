using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 倒计时事件弹窗
    /// </summary>
    public class GGUIWndCountdownEvent : _ANPGGUIBasicWnd<GGUIMonoCountdownEvent>
    {
        //资源id
        private long _m_lUIResId;
        //倒计时事件数据
        private CountdownEventInfo _m_cdEventInfo;
        //倒计时控件
        private NPGGUIWndCommonCountDown _m_wCommonCD;
        //任务列表
        private GGUIWndCountdownEventTaskContainer _m_wTaskContainer;
        //是否正在处理重置事件对话
        private bool _m_bIsDealResetDialogue;
        //是否正在播放完成动画
        private bool _m_bIsPlayDoneAni;

        public GGUIWndCountdownEvent(long _uiResId) : base(EALUIWndLayer.ADDITION)
        {
            _m_lUIResId = _uiResId;
        }

        protected override string _monoAssetPath { get { return UIResPathAssistant.getAssetPath(_m_lUIResId); } }
        protected override string _monoObjName { get { return UIResPathAssistant.getObjName(_m_lUIResId); } }
        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }
        protected override bool isShowAniPlayOnlyOne { get { return true; } }

        protected override void _onShowWnd()
        {
            _m_bIsDealResetDialogue = false;
            _m_bIsPlayDoneAni = false;
            WinMsg.RegisterMsg(WinMsgType.ON_CD_EVENT_CHG, _onCDEventChg);
            WinMsg.RegisterMsgAct(WinMsgType.SIMULATE_CLICK_COUNTDOWN_EVENT_GET_REWARD, _onSimulateClickGetReward);
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_CD_EVENT_CHG, _onCDEventChg);
            WinMsg.UnregisterMsgAct(WinMsgType.SIMULATE_CLICK_COUNTDOWN_EVENT_GET_REWARD, _onSimulateClickGetReward);
            _m_wCommonCD?.hideWnd();
            _m_wTaskContainer?.hideWnd();
            _m_bIsPlayDoneAni = false;
        }

        protected override void _onReset()
        {
            _m_wCommonCD?.resetWnd();
            _m_wTaskContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wCommonCD?.discard();
            _m_wCommonCD = null;

            _m_wTaskContainer?.discard();
            _m_wTaskContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoCD != null)
                _m_wCommonCD = new NPGGUIWndCommonCountDown(wnd.monoCD);

            if (wnd.monoTaskContainer != null)
                _m_wTaskContainer = new GGUIWndCountdownEventTaskContainer(wnd.monoTaskContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(CountdownEventInfo _info)
        {
            if (_info == null)
                return;

            _m_cdEventInfo = _info;
            _refreshWnd();
        }

        //刷新窗口
        private void _refreshWnd()
        {
            if (wnd == null || _m_cdEventInfo == null || _m_cdEventInfo.countdownEventRef == null)
                return;

            //设置名称描述
            ALUGUICommon.setLabelTxt(wnd.txtName, TextTranslate.instance.getLanguage(_m_cdEventInfo.countdownEventRef.name));
            ALUGUICommon.setLabelTxt(wnd.txtDesc, TextTranslate.instance.getLanguage(_m_cdEventInfo.countdownEventRef.desc, _m_cdEventInfo.countdownEventRef.desc_args));

            //设置倒计时
            if (NPPlayer.instance.countdownEventComp.checkNeedShowCD())
            {
                long leftTimeMs = _m_cdEventInfo.finishTimeMs - FpsAndPingMgr.instance.serverTimeTag;
                _m_wCommonCD?.showWnd();
                _m_wCommonCD?.setInfo(TimeUtil.msToSecCeiling(leftTimeMs), null);
            }
            else
                _m_wCommonCD?.setInfo(0, null);

            //设置任务列表
            QuestItem questItem = NPPlayer.instance.countdownEventComp.getCurEventQuestItem();
            _m_wTaskContainer?.showWnd();
            _m_wTaskContainer?.setInfo(questItem?.stepItem?.questTargetItemList);

            //设置显隐
            bool canGetReward = NPPlayer.instance.countdownEventComp.curCanGetReward();
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, canGetReward);
            ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardHideList, !canGetReward);
        }

        //检查是否需要播放重置倒计时事件对话
        private void _checkNeedShowResetDialogue()
        {
            if (_m_cdEventInfo == null || _m_bIsDealResetDialogue)
                return;

            bool isAlreadyShow = AccountSettingMgr.instance.accountSetting.getCDEventIsAlreadyShowResetDialogue(_m_cdEventInfo.dbId, _m_cdEventInfo.finishTimeMs);
            if (!isAlreadyShow)
            {
                _m_bIsDealResetDialogue = true;
                //播放重置对话
                GCommon.enterDialogueNode(_m_cdEventInfo.countdownEventRef.undone_dialogue_id, ()=> 
                {
                    _m_bIsDealResetDialogue = false;
                    AccountSettingMgr.instance.accountSetting.setRecordCDEvent(_m_cdEventInfo.dbId, _m_cdEventInfo.finishTimeMs);
                });
            }
        }

        #region 点击事件

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COUNTDOWN_EVENT_WND);
        }

        //点击领取奖励按钮
        private void _onClickGetReward(GameObject _go)
        {
            if (!NPPlayer.instance.countdownEventComp.curCanGetReward() || _m_cdEventInfo == null)
                return;

            GCommon.enterDialogueNode(_m_cdEventInfo.countdownEventRef.done_dialogue_id, () =>
            {
                _m_bIsPlayDoneAni = true;
                //设置完成任务和移除倒计时事件
                NPPlayer.instance.questComp.reqFinishQuest(_m_cdEventInfo.countdownEventRef.quest_id);
                NPPlayer.instance.countdownEventComp.reqRemoveCountdownEvent(_m_cdEventInfo.dbId);

                //动画播放完关闭窗口
                Action onFinish = () =>
                {
                    _m_bIsPlayDoneAni = false;
                    QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_COUNTDOWN_EVENT_WND);
                };

                if (wnd == null || wnd.aniFinish == null)
                    onFinish();
                else
                    wnd.aniFinish.forcePlay(onFinish);
            });
        }

        #endregion

        #region 消息事件

        //倒计时事件变化
        private void _onCDEventChg(params object[] _objects)
        {
            //如果正在播放完成动画，不处理事件变化
            if (_m_bIsPlayDoneAni)
                return;

            _m_cdEventInfo = NPPlayer.instance.countdownEventComp.countdownEventInfo;
            _refreshWnd();
            _checkNeedShowResetDialogue();
        }

        //模拟点击领取奖励
        private void _onSimulateClickGetReward()
        {
            _onClickGetReward(null);
        }

        #endregion
    }
}