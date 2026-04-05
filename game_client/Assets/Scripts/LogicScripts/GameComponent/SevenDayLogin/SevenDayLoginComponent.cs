using System;
using System.Collections.Generic;
using System.Linq;
using GC2GS.p004_PlayerOp;
using GS2GC.p002_InitOp;
using GS2GC.p004_PlayerOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    public class SevenDayLoginComponent : _ANPBasicPlayerComponent
    {
        [NotNull]private List<int> _m_lHadDrawRewardDays = new List<int>(); //已经领取的奖励天数

        public const long SYSTEM_SIMPLE_UNLOCK_ID = 60211;//系统解锁id
        
        // 已领取奖励奖励次数
        public int hadDrawRewardCount => _m_lHadDrawRewardDays.Count;

        public SevenDayLoginComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.SEVEN_DAY_LOGIN; } }
        public override ENPPlayerCompType[] dependCompList { get { return new []{ENPPlayerCompType.BASIC_INFO}; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
            //请求初始化
            _reqSevenDayLoginInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
            _refreshRedTip();
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_PLAYER_PARAM_CHANGE, _onPlayerParamChg);
        }
        /// <summary>
        /// 返回该天是否已经领取奖励
        /// </summary>
        /// <param name="_day"></param>
        /// <returns></returns>
        public bool isHadDrawReward(int _day)
        {
            return _m_lHadDrawRewardDays.Contains(_day);
        }
        /// <summary>
        /// 是否已经领取了前7天的奖励
        /// </summary>
        /// <returns></returns>
        public bool hasGetSevenDayReward()
        {
            for (int i = 1; i <= 7; i++)
            {
                if (!_m_lHadDrawRewardDays.Contains(i))
                    return false;
            }
            return true;
        }
        
        /// <summary>
        /// 是否已经领取了今天及之前的奖励
        /// </summary>
        /// <returns></returns>
        public bool hasGetTodayAndBeforeReward()
        {
            long loginCount = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SEVEN_DAYS_LOGIN_COUNT);
            
            for (int i = 1; i <= loginCount; i++)
            {
                if (!_m_lHadDrawRewardDays.Contains(i))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 是否已经领取了所有奖励
        /// </summary>
        /// <returns></returns>
        public bool hasGetAllReward()
        {
            return _m_lHadDrawRewardDays.Count >= GRefdataCoreMgr.instance.sevenDayLoginRefCore.refList.Count;
        }
        private void _onPlayerParamChg(object[] _params)
        {
            if (_params is not { Length: > 0 } || _params[0] is not int paramIndex)
                return;

            ENPPlayerParam playerParam = (ENPPlayerParam)paramIndex;
            if (playerParam != ENPPlayerParam.SEVEN_DAYS_LOGIN_COUNT)
                return;
            
            _refreshRedTip();
        }
        private void _refreshRedTip()
        {
            long loginDayCount = NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.SEVEN_DAYS_LOGIN_COUNT);

            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_SEVEN_DAY_LOGIN, loginDayCount - _m_lHadDrawRewardDays.Count);
        }
        #region 消息

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqSevenDayLoginInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_053_ReqLoginCountInit());
        }

        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_info"></param>
        public void retSevenDayLoginInit(GS2GC_002_053_RetLoginCountInit _info)
        {
            _m_lHadDrawRewardDays.Clear();
            _m_lHadDrawRewardDays.AddRange(_info.getHadDrawRewardDays());
            dealPreInitFunc(setInitDone);
        }

        public void OnLoginCountChg(GS2GC_004_058_OnLoginCountChg _msg)
        {
            if (_msg == null)
                return;
            _m_lHadDrawRewardDays.Clear();
            _m_lHadDrawRewardDays.AddRange(_msg.getHadDrawRewardDays());
            _refreshRedTip();
        }

        public void reqDrawLoginCountReward(int _day, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_004_036_ReqDrawLoginCountReward(_day),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_036_RetDrawLoginCountReward>((_isSuc, _msg) =>
                {
                    _refreshRedTip();
                    _callback?.Invoke(_isSuc);
                }, null, false));
            
        }
        #endregion
    }
}