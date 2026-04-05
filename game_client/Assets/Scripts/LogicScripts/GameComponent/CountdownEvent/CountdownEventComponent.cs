using System;
using ALPackage;
using GC2GS.p007_CommOp;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;

namespace GOE
{
    /// <summary>
    /// 倒计时事件组件
    /// </summary>
    public class CountdownEventComponent : _ANPBasicPlayerComponent
    {
        //倒计时事件信息
        private CountdownEventInfo _m_countdownEventInfo;
        //定时任务
        private ALCommonEnableTaskController _m_checkTask;
        //是否正在请求重置
        private bool _m_bIsReqReset;
        //是否正在处理新事件对话
        private bool _m_bDealNewCDEventDialogue;
        //是否正在处理重置事件对话
        private bool _m_bDealResetCDEventDialogue;

        //构造函数
        public CountdownEventComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        protected static ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.QUEST };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.COUNTDOWN_EVENT; } }
        public override ENPPlayerCompType[] dependCompList { get { return _g_DependComp; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 倒计时事件信息
        /// </summary>
        public CountdownEventInfo countdownEventInfo { get { return _m_countdownEventInfo; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            reqCountdownEventInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _m_bIsReqReset = false;
            _m_bDealNewCDEventDialogue = false;
            _m_bDealResetCDEventDialogue = false;
            _startCheck();
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("CountdownEventComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _m_countdownEventInfo = null;
            _stopCheck();
        }

        /// <summary>
        /// 检查是否需要重置事件
        /// </summary>
        /// <returns></returns>
        public bool checkNeedResetEvent()
        {
            //数据无效，不用重置
            if (_m_countdownEventInfo == null || !_m_countdownEventInfo.isValid)
                return false;

            //没有倒计时或者可领取奖励或者倒计时未结束，不用重置
            if (!checkNeedShowCD() || curCanGetReward() || _m_countdownEventInfo.finishTimeMs - FpsAndPingMgr.instance.serverTimeTag > 0)
                return false;

            return true;
        }

        /// <summary>
        /// 当前事件是否可以领取奖励
        /// </summary>
        /// <returns></returns>
        public bool curCanGetReward()
        {
            QuestItem questItem = getCurEventQuestItem();
            if (questItem == null || questItem.stepItem == null)
                return false;

            return questItem.stepItem.getQuestStepStatus() == ENPQuestStepStatusEnum.QUEST_CANGET;
        }

        /// <summary>
        /// 获取当前事件的任务对象
        /// </summary>
        /// <returns></returns>
        public QuestItem getCurEventQuestItem()
        {
            if (_m_countdownEventInfo == null || !_m_countdownEventInfo.isValid || _m_countdownEventInfo.countdownEventRef == null)
                return null;

            return NPPlayer.instance.questComp.questItemMgr.getQuestItem(_m_countdownEventInfo.countdownEventRef.quest_id);
        }

        /// <summary>
        /// 检查是否需要播放新事件对话或者重置对话
        /// </summary>
        public void checkDialogueShow(EMainCityPushNoticeTriggerType _pushNoticeTriggerType, Action _addDealComplete)
        {
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(2);
            stepCounter.regAllDoneDelegate(() =>
            {
                _addDealComplete?.Invoke();
            });
            
            checkNeedShowNewEventDialogue(_pushNoticeTriggerType, stepCounter.addDoneStepCount);
            checkNeedShowResetEventDialogue(_pushNoticeTriggerType, stepCounter.addDoneStepCount);
        }

        /// <summary>
        /// 检查是否需要播放新倒计时事件对话
        /// </summary>
        public void checkNeedShowNewEventDialogue(EMainCityPushNoticeTriggerType _pushNoticeTriggerType, Action _addDealComplete)
        {
            if (_m_bDealNewCDEventDialogue)
            {
                _addDealComplete?.Invoke();
                return;
            }

            if (_m_countdownEventInfo == null || !_m_countdownEventInfo.isValid || _m_countdownEventInfo.countdownEventRef == null)
            {
                _addDealComplete?.Invoke();
                return;
            }

            bool isNew = AccountSettingMgr.instance.accountSetting.getCDEventIsNew(_m_countdownEventInfo.dbId);
            if (isNew)
            {
                _m_bDealNewCDEventDialogue = true;
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_NewCountdownEventShow(_m_countdownEventInfo, () =>
                {
                    _m_bDealNewCDEventDialogue = false;
                }, _pushNoticeTriggerType));
            }
            
            _addDealComplete?.Invoke();
        }

        /// <summary>
        /// 检查是否需要播放重置对话
        /// </summary>
        public void checkNeedShowResetEventDialogue(EMainCityPushNoticeTriggerType _pushNoticeTriggerType, Action _addDealComplete)
        {
            if (_m_bDealResetCDEventDialogue)
            {
                _addDealComplete?.Invoke();
                return;
            }

            if (_m_countdownEventInfo == null || !_m_countdownEventInfo.isValid || _m_countdownEventInfo.countdownEventRef == null)
            {
                _addDealComplete?.Invoke();
                return;
            }

            bool isAlreadyShow = AccountSettingMgr.instance.accountSetting.getCDEventIsAlreadyShowResetDialogue(_m_countdownEventInfo.dbId, _m_countdownEventInfo.finishTimeMs);
            if (!isAlreadyShow)
            {
                _m_bDealResetCDEventDialogue = true;
                NPUINoticeMgr.instance.addDealer(new NoticeDealer_CountdownEventResetShow(_m_countdownEventInfo.countdownEventRef.undone_dialogue_id,
                    () =>
                    {
                        _m_bDealResetCDEventDialogue = false;
                    }, _pushNoticeTriggerType));
            }
            
            _addDealComplete?.Invoke();
        }

        /// <summary>
        /// 检查是否需要展示倒计时
        /// </summary>
        /// <returns></returns>
        public bool checkNeedShowCD()
        {
            if (_m_countdownEventInfo == null || !_m_countdownEventInfo.isValid)
                return false;

            return _m_countdownEventInfo.resetCount <= GRefdataCoreMgr.instance.npGeneral.count_down_event_have_cd_minimum_reset_count;
        }

        //开启定时检查
        private void _startCheck()
        {
            _m_checkTask.setDisable();
            _m_checkTask = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_tickCheck, 1.0f);
        }

        //关闭定时检查
        private void _stopCheck()
        {
            _m_checkTask.setDisable();
        }

        //定时检查函数
        private void _tickCheck()
        {
            if (_m_countdownEventInfo == null || !_m_countdownEventInfo.isValid || _m_bIsReqReset)
                return;

            //不需要展示倒计时，说明重置次数超过上限，关闭任务，不需要处理重置
            if (!checkNeedShowCD())
            {
                _stopCheck();
                return;
            }

            if (checkNeedResetEvent())
            {
                _m_bIsReqReset = true;
                reqResetCountdownEvent(_m_countdownEventInfo.dbId, () =>
                {
                    _m_bIsReqReset = false;
                });
            }
        }

        #region S2C

        /// <summary>
        /// 倒计时事件初始化
        /// </summary>
        public void retCountdownEventInit(GS2GC_002_056_RetCountdownEventInit _msg)
        {
            if (_msg == null || _msg.getEventInfo() == null || _msg.getEventInfo().getDbId() <= 0)
            {
                setInitDone();
                return;
            }

            _m_countdownEventInfo = new CountdownEventInfo(_msg.getEventInfo());
            setInitDone();
        }

        /// <summary>
        /// 倒计时事件变更
        /// </summary>
        /// <param name="_msg"></param>
        public void onCountdownEventChg(GS2GC_007_076_OnCountdownEventChg _msg)
        {
            if (_msg == null)
                return;

            if (_msg.getInfo() == null || _msg.getInfo().getDbId() <= 0)
            {
                _m_countdownEventInfo = null;
                WinMsg.SendMsg(WinMsgType.ON_CD_EVENT_CHG, 0);
            }
            else
            {
                if (_m_countdownEventInfo == null)
                    _m_countdownEventInfo = new CountdownEventInfo(_msg.getInfo());
                else
                    _m_countdownEventInfo.updateInfo(_msg.getInfo());

                WinMsg.SendMsg(WinMsgType.ON_CD_EVENT_CHG, _m_countdownEventInfo.dbId);
            }
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求倒计时事件初始化
        /// </summary>
        public void reqCountdownEventInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_056_ReqCountdownEventInit());
        }

        /// <summary>
        /// 请求移除倒计时事件
        /// </summary>
        /// <param name="_dbId"></param>
        public void reqRemoveCountdownEvent(long _dbId)
        {
            //客户端本地先移除事件数据
            _m_countdownEventInfo = null;
            WinMsg.SendMsg(WinMsgType.ON_CD_EVENT_REMOVE);

            //请求服务端移除事件数据
            NPGSClientListener.sendRequestByLog(new GC2GS_007_023_ReqRemoveCountdownEvent(_dbId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_023_RetRemoveCountdownEvent>(null));
        }

        /// <summary>
        /// 请求重置倒计时事件
        /// </summary>
        /// <param name="_dbId"></param>
        public void reqResetCountdownEvent(long _dbId, Action _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_007_024_ReqResetCountdownEvent(_dbId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_007_024_RetResetCountdownEvent>((_isSuc, _msg) =>
                {
                    _callback?.Invoke();
                }));
        }

        #endregion

    }
}
