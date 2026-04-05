using System;
using System.Collections.Generic;
using ALPackage;
using GC2GS.p007_CommOp;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using GC2GS.p011_ClientDataOp;
using GS2GC.p011_ClientDataOp;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 问卷调查组件
    /// </summary>
    public class PlayerQuestionnaireComponent : _ANPBasicPlayerComponent
    {
        //构造函数
        public PlayerQuestionnaireComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_lQuestionnaireInfoList = new List<QuestionnaireInfo>();
        }

        //问卷信息列表
        private List<QuestionnaireInfo> _m_lQuestionnaireInfoList;

        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.QUESTIONNAIRE; } }
        public override ENPPlayerCompType[] dependCompList { get { return null; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            //请求初始化
            reqQuestionnaireInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("PlayerQuestionnaireComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _clear();
        }

        //析构函数
        private void _clear()
        {
            _m_lQuestionnaireInfoList?.Clear();
        }

        /// <summary>
        /// 获取问卷调查信息
        /// </summary>
        /// <returns></returns>
        public QuestionnaireInfo getQuestionnaireInfo()
        {
            if (_m_lQuestionnaireInfoList == null)
                return null;

            for (int i = 0; i < _m_lQuestionnaireInfoList.Count; i++)
            {
                if (_m_lQuestionnaireInfoList[i] != null &&
                    FpsAndPingMgr.instance.serverTimeTag >= _m_lQuestionnaireInfoList[i].startTimeMs &&
                    FpsAndPingMgr.instance.serverTimeTag <= _m_lQuestionnaireInfoList[i].endTimeMs &&
                    _m_lQuestionnaireInfoList[i].getState != ENPCommonGetStat.HAS_GET)
                    return _m_lQuestionnaireInfoList[i];
            }

            return null;
        }

        /// <summary>
        /// 是否可以展示入口
        /// </summary>
        /// <returns></returns>
        public bool canShowEntrance()
        {
            if (_m_lQuestionnaireInfoList == null)
                return false;

            for (int i = 0; i < _m_lQuestionnaireInfoList.Count; i++)
            {
                if (_m_lQuestionnaireInfoList[i] != null &&
                    FpsAndPingMgr.instance.serverTimeTag >= _m_lQuestionnaireInfoList[i].startTimeMs &&
                    FpsAndPingMgr.instance.serverTimeTag <= _m_lQuestionnaireInfoList[i].endTimeMs &&
                    _m_lQuestionnaireInfoList[i].getState != ENPCommonGetStat.HAS_GET)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 刷新红点
        /// </summary>
        private void _refreshRedTip()
        {
            if (_m_lQuestionnaireInfoList == null || _m_lQuestionnaireInfoList.Count == 0)
            {
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_QUESTIONNAIRE, 0);
            }
            else
            {
                //只显示第一个可展示的红点
                QuestionnaireInfo info = getQuestionnaireInfo();
                long count = (info != null && info.getState == ENPCommonGetStat.CAN_GET) ? 1 : 0;
                RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_QUESTIONNAIRE, count);
            }
        }

        #region S2C

        /// <summary>
        /// 初始化问卷调查
        /// </summary>
        /// <param name="_msg"></param>
        public void retQuestionnaireInit(GS2GC_002_042_RetQuestionnaireInit _msg)
        {
            if (_m_lQuestionnaireInfoList == null)
                _m_lQuestionnaireInfoList = new List<QuestionnaireInfo>();
            _m_lQuestionnaireInfoList.Clear();

            if (_msg != null)
            {
                //获取数据
                for (int i = 0; i < _msg.getInfoList().Count; i++)
                {
                    QuestionnaireInfo info = new QuestionnaireInfo(_msg.getInfoList()[i]);
                    _m_lQuestionnaireInfoList.Add(info);
                }

                //更新领奖状态
                for (int i = 0; i < _msg.getRewardList().Count; i++)
                {
                    for (int j = 0; j < _m_lQuestionnaireInfoList.Count; j++)
                    {
                        if(_m_lQuestionnaireInfoList[j].questionnaireId == _msg.getRewardList()[i].getQuestionnaireId())
                            _m_lQuestionnaireInfoList[j].setGetState(_msg.getRewardList()[i].getHasDraw());
                    }
                }
            }

            //刷新红点
            _refreshRedTip();

            setInitDone();
        }

        /// <summary>
        /// 问卷调查活动开始
        /// </summary>
        public void onQuestionnaireStart(GS2GC_007_063_OnQuestionnaireStart _msg)
        {
            if (_msg == null || _msg.getInfo() == null)
                return;

            if (_m_lQuestionnaireInfoList == null)
                _m_lQuestionnaireInfoList = new List<QuestionnaireInfo>();

            _m_lQuestionnaireInfoList.Add(new QuestionnaireInfo(_msg.getInfo()));

            //查询问卷信息
            reqQuestionnaireInfo(_msg.getInfo().getQuestionnaireId(), (_stateMsg) =>
            {
                if (_stateMsg == null)
                    return;

                for (int i = 0; i < _m_lQuestionnaireInfoList.Count; i++)
                {
                    if (_stateMsg.getHasReward() && 
                        _m_lQuestionnaireInfoList != null && 
                        _stateMsg.getRewardInfo() != null && 
                        _m_lQuestionnaireInfoList[i] != null && 
                        _m_lQuestionnaireInfoList[i].questionnaireId == _stateMsg.getRewardInfo().getQuestionnaireId())
                    {
                        //更新领奖状态
                        _m_lQuestionnaireInfoList[i].setGetState(_stateMsg.getRewardInfo().getHasDraw());
                        //刷新ui显示
                        GCommon.reloadCustomLoadPrefab();
                        break;
                    }
                }

            });

            //刷新ui显示
            GCommon.reloadCustomLoadPrefab();
            WinMsg.SendMsg(WinMsgType.ON_QUESTIONNAIRE_START, _msg.getInfo()?.getQuestionnaireId());
        }

        /// <summary>
        /// 问卷调查活动结束
        /// </summary>
        public void onQuestionnaireClose(GS2GC_007_064_OnQuestionnaireClose _msg)
        {
            if (_msg == null || _m_lQuestionnaireInfoList == null)
                return;

            for (int i = 0; i < _m_lQuestionnaireInfoList.Count; i++)
            {
                if (_m_lQuestionnaireInfoList[i] != null && _m_lQuestionnaireInfoList[i].questionnaireId == _msg.getQuestionnaireId())
                {
                    _m_lQuestionnaireInfoList.RemoveAt(i);
                    break;
                }
            }

            //刷新ui显示
            GCommon.reloadCustomLoadPrefab();
            WinMsg.SendMsg(WinMsgType.ON_QUESTIONNAIRE_END, _msg.getQuestionnaireId());
        }

        /// <summary>
        /// 可领取奖励推送
        /// </summary>
        public void onQuestionnaireRewardAdd(GS2GC_007_065_OnQuestionnaireRewardAdd _msg)
        {
            if (_msg == null || _m_lQuestionnaireInfoList == null)
                return;

            for (int i = 0; i < _m_lQuestionnaireInfoList.Count; i++)
            {
                if (_m_lQuestionnaireInfoList[i] != null && _m_lQuestionnaireInfoList[i].questionnaireId == _msg.getQuestionnaireId())
                {
                    _m_lQuestionnaireInfoList[i].setGetState(false);
                    break;
                }
            }

            //刷新红点
            _refreshRedTip();

            WinMsg.SendMsg(WinMsgType.ON_QUESTIONNAIRE_REWARD_ADD, _msg.getQuestionnaireId());
        }

        /// <summary>
        /// 已领取奖励推送
        /// </summary>
        public void onQuestionnaireRewardChg(GS2GC_007_067_OnQuestionnaireRewardChg _msg)
        {
            if (_msg == null || _msg.getRewardInfo() == null || _m_lQuestionnaireInfoList == null)
                return;


            for (int i = 0; i < _m_lQuestionnaireInfoList.Count; i++)
            {
                if (_m_lQuestionnaireInfoList[i] != null && _m_lQuestionnaireInfoList[i].questionnaireId == _msg.getRewardInfo().getQuestionnaireId())
                {
                    _m_lQuestionnaireInfoList[i].setGetState(_msg.getRewardInfo().getHasDraw());
                    break;
                }
            }

            //刷新红点
            _refreshRedTip();

            //刷新ui显示
            GCommon.reloadCustomLoadPrefab();
            WinMsg.SendMsg(WinMsgType.ON_QUESTIONNAIRE_REWARD_GET, _msg.getRewardInfo().getQuestionnaireId());
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化骑士列表
        /// </summary>
        public void reqQuestionnaireInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_042_RetQuestionnaireInit());
        }
        
        /// <summary>
        /// 请求玩家问卷调查数据
        /// </summary>
        public void reqWebEncryptedData(Action<string> _callBack)
        {
            float userPay = SDKLoginSetting.instance.hasToken() ? SDKLoginSetting.instance.getTokenData().user_pay : 0f;
            string langCode = GameSetting.instance.getCurrentLanguage().toPHPLanguageCode();
            string project = NPConst.GAME_ID;
            string packageName = Application.identifier;
            string products = "";
            NPGSClientListener.sendRequestByLog(new GC2GS_011_003_ReqWebEncryptedData(userPay, langCode, project, packageName, products),
            new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_011_003_RetWebEncryptedData>(_msg =>
            {
                _callBack?.Invoke(_msg?.getData());
            }));
        }

        /// <summary>
        /// 请求领取奖励
        /// </summary>
        /// <param name="_id"></param>
        public void reqDrawQuestionnaireReward(long _id)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_007_003_ReqDrawQuestionnaireReward(_id),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_003_RetDrawQuestionnaireReward>(null));
        }

        /// <summary>
        /// 查询问卷信息
        /// </summary>
        /// <param name="_id"></param>
        public void reqQuestionnaireInfo(long _id, Action<GS2GC_007_004_RetQuestionnaireInfo> _callBack = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_007_004_ReqQuestionnaireInfo(_id),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_004_RetQuestionnaireInfo>(_msg =>
                {
                    _callBack?.Invoke(_msg);
                }));
        }

        #endregion
    }
}
