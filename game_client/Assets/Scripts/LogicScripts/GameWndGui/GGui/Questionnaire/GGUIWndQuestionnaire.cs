using System;
using ALPackage;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 问卷调查弹窗
    /// </summary>
    public class GGUIWndQuestionnaire : _ATALBasicUIWnd<GGUIMonoQuestionnaire>
    {
        private static GGUIWndQuestionnaire _g_instance;
        public static GGUIWndQuestionnaire instance
        {
            get
            {
                if (null == _g_instance)
                {
                    _g_instance = new GGUIWndQuestionnaire();
                }
                return _g_instance;
            }
        }

        //问卷调查信息
        private QuestionnaireInfo _m_Info;
        //奖励列表
        private NPGGUIWndCommonItemContainer _m_wRewardContainer;
        //任务刷新定时器
        private ALCommonEnableTaskController _m_tcTickTaskController;
        //玩家数据
        private string _m_sPlayerData;
        //展示序列号
        private long _m_lShowSerialize;
        //获得玩家信息回调
        private Action _m_aOnGetPlayerData;

        public GGUIWndQuestionnaire() : base(EALUIWndLayer.ADDITION)
        {
        }

        protected override string _monoAssetPath { get => GGUIMonoQuestionnaire.assetPath; }
        protected override string _monoObjName { get => GGUIMonoQuestionnaire.objName; }
        protected override _AALResourceCore _resourceCore { get => GameResCore.instance; }
        

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_QUESTIONNAIRE_END, _onQuestionnaireEnd);//问卷调查活动结束
            WinMsg.RegisterMsg(WinMsgType.ON_QUESTIONNAIRE_REWARD_ADD, _onRewardAdd);//可领取奖励推送
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_QUESTIONNAIRE_END, _onQuestionnaireEnd);//问卷调查活动结束
            WinMsg.UnregisterMsg(WinMsgType.ON_QUESTIONNAIRE_REWARD_ADD, _onRewardAdd);//可领取奖励推送

            _m_wRewardContainer?.hideWnd();
            _m_tcTickTaskController.setDisable();
            _m_sPlayerData = null;
            _m_aOnGetPlayerData = null;
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
            _m_wRewardContainer?.resetWnd();
        }

        protected override void _onDiscard()
        {
            _m_wRewardContainer?.discard();
            _m_wRewardContainer = null;

            if (wnd == null)
                return;

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnGoTo, _onClickGoto);
            ALUGUICommon.uncombineBtnClick(wnd.btnGetReward, _onClickGetReward);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            if (wnd.monoRewardContainer != null)
                _m_wRewardContainer = new NPGGUIWndCommonItemContainer(wnd.monoRewardContainer);

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnGoTo, _onClickGoto);
            ALUGUICommon.combineBtnClick(wnd.btnGetReward, _onClickGetReward);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(QuestionnaireInfo _info)
        {
            if (_info == null)
                return;

            _m_Info = _info;
            _refreshWnd();

            //开启倒计时
            _m_tcTickTaskController.setDisable();
            _m_tcTickTaskController = ALCommonTaskController.CommonEnableDurationActionAddMonoTask(_refreshTime,1f);

            //获取玩家数据
            _m_sPlayerData = null;
            _m_lShowSerialize = ALSerializeOpMgr.next();
            //如果还没填写问卷调查，请求玩家数据
            if (_info.getState == ENPCommonGetStat.CAN_NOT_GET)
            {
                long showSerialize = _m_lShowSerialize;
                NPPlayer.instance.questionnaireComp.reqWebEncryptedData(_data =>
                {
                    if (_m_lShowSerialize != showSerialize)
                        return;

                    _m_sPlayerData = _data;
                    _m_aOnGetPlayerData?.Invoke();
                });
            }
        }

        //刷新窗口
        private void _refreshWnd()
        {
            _refreshTime();
            _refreshReward();
            _refreshState();
        }

        //刷新时间
        private void _refreshTime()
        {
            if (wnd == null || _m_Info == null)
                return;

            //开始时间
            DateTime startTime = TimeUtil.FromUTCByTimeZone(_m_Info.startTimeMs);
            //结束时间
            DateTime endTime = TimeUtil.FromUTCByTimeZone(_m_Info.endTimeMs);
            //剩余时间
            long leftTimeMs = _m_Info.endTimeMs - FpsAndPingMgr.instance.serverTimeTag;

            //问卷时间
            ALUGUICommon.setLabelTxt(wnd.txtTime, TimeUtil.DateTime2String_DurationLong_MD(startTime, endTime));
            //问卷倒计时
            if (leftTimeMs >= 0)
                ALUGUICommon.setLabelTxt(wnd.txtCD, TextTranslate.instance.getLanguage(TransKeyConst.time_duration_countdown, TimeUtil.millisecondsToTime_hms(leftTimeMs)));


            //倒计时结束直接关闭窗口
            if (leftTimeMs <= 0)
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_QUESTIONNAIRE);
                //问卷调查已结束
                NPMesMgr.instance.showOneBtnMes(
                    TextTranslate.instance.getLanguage(TransKeyConst.questionnaire_questionnaireIsEnd_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.ok),
                    null);
            }
        }

        //刷新奖励列表
        private void _refreshReward()
        {
            if (_m_Info == null)
                return;

            if (_m_wRewardContainer != null && _m_Info.rewardList != null)
            {
                _m_wRewardContainer.showWnd();
                _m_wRewardContainer.showItemList(_m_Info.rewardList);
            }
        }

        //刷新按钮状态
        public void _refreshState()
        {
            if (wnd == null || _m_Info == null)
                return;

            switch (_m_Info.getState)
            {
                //可领取
                case ENPCommonGetStat.CAN_GET:
                    ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardHideList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, true);
                    break;
                //不可领取
                case ENPCommonGetStat.CAN_NOT_GET:
                    ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardShowList, false);
                    ALUGUICommon.setGameObjEnable(wnd.goCanGetRewardHideList, true);
                    break;
            }
        }

        #region 点击事件

        //点击关闭
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_QUESTIONNAIRE);
        }

        //点击同意前往问卷调查网页
        private void _onClickGoto(GameObject _go)
        {
            //如果还没获取到玩家数据，注册到回调里，获取到玩家数据才执行
            if(string.IsNullOrEmpty(_m_sPlayerData))
                _m_aOnGetPlayerData = _dealGoTo;
            else
                _dealGoTo();
        }

        //处理前往问卷调查网页
        private void _dealGoTo()
        {
            if (_m_Info == null || string.IsNullOrEmpty(_m_Info.urlLink) || string.IsNullOrEmpty(_m_Info.questionnaireCode))
                return;

            //网页地址
            string address = _m_Info.urlLink;
            if (!address.EndsWith("/"))
                address += "/";

            //网页链接
            string url = address + _m_Info.questionnaireCode + "?data=" + _m_sPlayerData;
            //打开网页
            GCommon.openURL(url);
        }

        //点击领取奖励
        private void _onClickGetReward(GameObject _go)
        {
            if (_m_Info == null || _m_Info.getState != ENPCommonGetStat.CAN_GET)
                return;

            NPPlayer.instance.questionnaireComp.reqDrawQuestionnaireReward(_m_Info.questionnaireId);
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_QUESTIONNAIRE);
        }

        #endregion

        #region 消息事件

        /// <summary>
        /// 活动结束
        /// </summary>
        /// <param name="_objects"></param>
        private void _onQuestionnaireEnd(params object[] _objects)
        {
            if (_m_Info == null || _objects == null || _objects[0] == null)
                return;

            long activityId = (long) _objects[0];

            //活动结束直接关闭窗口
            if (activityId == _m_Info.questionnaireId)
            {
                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst.C_QUESTIONNAIRE);
                //问卷调查已结束
                NPMesMgr.instance.showOneBtnMes(
                    TextTranslate.instance.getLanguage(TransKeyConst.questionnaire_questionnaireIsEnd_none), 
                    TextTranslate.instance.getLanguage(TransKeyConst.ok),
                    null);
            }
        }

        /// <summary>
        /// 可领取奖励推送
        /// </summary>
        /// <param name="_objects"></param>
        private void _onRewardAdd(params object[] _objects)
        {
            _refreshState();
        }

        #endregion
    }
}