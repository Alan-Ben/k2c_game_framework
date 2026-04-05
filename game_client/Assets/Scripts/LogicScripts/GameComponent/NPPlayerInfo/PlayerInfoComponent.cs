using System;
using GS2GC.p004_PlayerOp;

namespace GOE
{
    //玩家数据
    public class PlayerInfoComponent : _ANPBasicPlayerComponent
    {
        //玩家信息
        private PlayerInfo _m_piPlayerInfo;
        //通用clientData
        private PlayerCommonClientDataRemarkInfo _m_clientDataRemarkInfo;
        
        public PlayerInfoComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_piPlayerInfo = new PlayerInfo();
            _m_clientDataRemarkInfo = new PlayerCommonClientDataRemarkInfo();
        }

        //是否必要型初始化组件
        public PlayerInfo playerInfo { get { return _m_piPlayerInfo; } }
        //是否必要型初始化组件
        public override bool isMustInit { get { return true; } }
        //获取组件类型
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.BASIC_INFO; } }
        //依赖的组件队列
        public override ENPPlayerCompType[] dependCompList { get { return new [] { ENPPlayerCompType.HERO }; } }

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public PlayerCommonClientDataRemarkInfo clientDataRemarkInfo { get { return _m_clientDataRemarkInfo; } }

        /// <summary>
        /// 发送初始化协议提前申请内容
        /// </summary>
        public override void presendInitProtocol()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_001_ReqPlayerInfo());
        }

        //处理初始化操作
        protected override void _dealInit()
        {
            
        }
        //初始化结果操作
        protected override void _onInitDone()
        {
            //跨天消息
            WinMsg.RegisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);
            //初始化玩家账号存储相关信息
            AccountSettingMgr.instance.init();
        }

        protected override void _onInitFail()
        {

        }
        //释放资源函数
        protected override void _discard()
        {
            //跨天消息
            WinMsg.UnregisterMsgAct(WinMsgType.ON_CROSS_DAY, _onCrossDay);

            if (_m_piPlayerInfo != null)
                _m_piPlayerInfo.clear();
            _m_piPlayerInfo = null;
        }

        public override void onAllCompInited()
        {
            //初始化每日红点提示
            GCommon.initDailyRedTip();
            _m_piPlayerInfo?.refrshAllRedTip();
        }

        /// <summary>
        /// 发生跨天
        /// </summary>
        private void _onCrossDay()
        {
            //初始化每日红点提示
            GCommon.initDailyRedTip();
        }

        /****************
         * 初始化数据
         **/
        public void init(GS2GC.p002_InitOp.GS2GC_002_001_RetPlayerInfo _msg)
        {
            if (_m_piPlayerInfo != null) 
                _m_piPlayerInfo.setData(_msg);

            if (_m_clientDataRemarkInfo != null) 
                _m_clientDataRemarkInfo.sendRequest(setInitDone);
        }
        
        //修改名字
        public void reqChangeName(string _name,Action _doneAction)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_004_ReqSetName(_name)
                 , new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_004_004_SetNameRes>(
                        (_res) =>
                        {
                            if (null != _doneAction)
                                _doneAction();
                        }
                    )
                );
        }

        /// <summary>
        /// 请求某个玩家的简要信息
        /// </summary>
        /// <param name="_cid"></param>
        /// <param name="_callBack"></param>
        public void reqSomeOnePlayerBriefInfo(long _cid, Action<GS2GC_004_011_RetSomeOnePlayerBriefInfo> _callBack,Action _failAction = null)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_011_ReqSomeOnePlayerBriefInfo(_cid),
                new CommonRequestCallbackProtocolDealer<GS2GC_004_011_RetSomeOnePlayerBriefInfo>((info) =>
                {
                    if (null != _callBack)
                        _callBack(info);
                }, (_errorCode) => {
                    if (null != _failAction)
                        _failAction();
                }));
        }
        
        //请求玩家升级
        public void reqPlayerLvlUp(Action<bool, long> _doneAction)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_001_ReqLevelUp(NPPlayer.instance.playerInfo.curLevelRef.lvl)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_001_RetReqLevelUp>(
                    (_isSuc, _msg) =>
                    {
                        _doneAction?.Invoke(_isSuc, _msg?.getNewLevel() ?? 0);
                    }
                )
            );
        }

        /// <summary>
        /// 请求领取VIP等级奖励
        /// </summary>
        /// <param name="_vipLevel"></param>
        public void reqDrawVipLevelReward(long _vipLevel, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_004_037_ReqDrawVipLevelReward(_vipLevel)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_037_RetDrawVipLevelReward>((_isSuc, _msg) =>
                    {
                        _callback?.Invoke(_isSuc);
                    }
                )
            );
        }

        /// <summary>
        /// 请求领取vip充值奖励
        /// </summary>
        /// <param name="_vipLevel"></param>
        /// <param name="_callback"></param>
        public void reqDrawVipLevelRechargeReward(long _vipLevel, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(NPGSWriter_004_PlayerOp.make_004_038_ReqDrawVipLevelRechargeReward(_vipLevel)
                , new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_004_038_RetDrawVipLevelRechargeReward>((_isSuc, _msg) =>
                    {
                        _callback?.Invoke(_isSuc);
                    }
                )
            );
        }
    }
}
