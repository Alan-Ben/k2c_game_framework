using System;
using System.Collections.Generic;
using ALPackage;
using ChatPackage.Internal;
using Common.DinnerEnum;
using Common.DinnerObj;
using GC2GS.p007_CommOp;
using GC2GS.p019_DinnerOp;
using GS2GC.p002_InitOp;
using GS2GC.p007_CommOp;
using GS2GC.p019_DinnerOp;
using NPEnum;

namespace GOE
{
    //宴会管理器
    public partial class GPlayerDinnerComponent : _ANPBasicPlayerComponent
    {
        private long _m_dinnerInstanceId;
        private bool _m_hasOwenrReward = false;//有开宴奖励未领取
        private RedTipDealer _m_redDealer;
        private List<DinnerPermit> _m_permitList = new List<DinnerPermit>();// 凭证列表
        
        private ALCommonEnableTaskController _m_dinnerPermitTick;
        
        public Action onRewardChg;//奖励数据变化
        public Action onPermitChg;//凭证数据变化
        
        
        private bool _m_hasEnterDinner = false;

        //构造函数
        public GPlayerDinnerComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
            _m_redDealer = new RedTipDealer(this);
        }
        /// <summary>
        /// 当前宴会实例id。0表示当前没开宴会
        /// </summary>
        public long dinnerInstanceId { get { return _m_dinnerInstanceId; } }
        public bool hasOwenrReward { get { return _m_hasOwenrReward; } }
        public List<DinnerPermit> permitList { get { return _m_permitList; } }
        public bool hasDinnerOpen { get { return _m_dinnerInstanceId > 0; } }
        public bool hasEnterDinner { get { return _m_hasEnterDinner; } }

        //属性
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.DINNER; } }
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
            _reqDinnerInit();
        }

        protected override void _dealInit()
        {
        }

        //组件加载完成时的调用
        protected override void _onInitDone()
        {
            _m_hasEnterDinner = false;
            _m_redDealer?.init();
            _m_dinnerPermitTick.setDisable();
            _m_dinnerPermitTick = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_permitTick);
        }

        //组件初始化失败的处理
        protected override void _onInitFail()
        {
            ALLog.Error("GPlayerDinnerComponent init Fail!!!");
        }

        //释放资源函数
        protected override void _discard()
        {
            _m_dinnerInstanceId = 0;
            _m_redDealer?.clear();
            _m_permitList.Clear();
            _m_dinnerPermitTick.setDisable();
        }

        /// <summary>
        /// 展示妃子宴会凭证获得通知
        /// </summary>
        /// <param name="_consortId"></param>
        public void showConsortPermitGetNotice(long _consortId, Action _onDealDone = null)
        {
            if (_m_permitList == null || !GCommon.isFuncUnlock(ENPFunctionType.DINNER))
            {
                _onDealDone?.Invoke();
                return;
            }
            
            ALProcess alProcess = ALProcess.CreateProcess();
            foreach (DinnerPermit permit in _m_permitList)
            {
                if (permit != null && permit.type == EDinnerPermitType.FAMILY && permit.typeId == _consortId)
                {
                    alProcess.addDelegateProcess((_done) =>
                    {
                        GGUIWndDinnerPermitGet.instance.setInfo(permit, null);
                        QueueMgr.instance.AddNode(new GNodeCommonWndWithCloseFunc(GGUIWndDinnerPermitGet.instance, _done));
                    });
                }
            }

            alProcess.addProcess(_onDealDone);
            alProcess.deal();
        }
        
        /// <summary>
        /// 展示宴会凭证获得通知
        /// </summary>
        /// <param name="_permitId"></param>
        public void showPermitGetNotice(EDinnerPermitType _type, long _typeId, Action _onDealDone = null)
        {
            if (_m_permitList == null || !GCommon.isFuncUnlock(ENPFunctionType.DINNER))
            {
                _onDealDone?.Invoke();
                return;
            }
            
            foreach (var permit in _m_permitList)
            {
                if (permit != null && permit.type == _type && permit.typeId == _typeId)
                {
                    NPUINoticeMgr._ANPUINoticeDealer result;
                    if (_type == EDinnerPermitType.GIFTDE_CHILD_CELE)
                        result = new NPNoticeDealer_DinnerPermitGet_Child(permit, _onDealDone);
                    else
                        result = new NPNoticeDealer_DinnerPermitGet(permit, _onDealDone);
                    NPUINoticeMgr.instance.addDealer(result);
                    return;
                }
            }
            
            // 若没有要显示的凭证, 则直接执行完成回调
            _onDealDone?.Invoke();
        }

        /// <summary>
        /// 展示卷王子嗣庆功宴凭证获得通知
        /// </summary>
        /// <param name="_childId">子嗣id</param>
        /// <param name="_onDealDone"></param>
        public void showGiftdeChildPermitGetNotice(long _childId, Action _onDealDone = null)
        {
            showPermitGetNotice(EDinnerPermitType.GIFTDE_CHILD_CELE, _childId, _onDealDone);
        }

        public bool canCreateDinner()
        {
            if (hasDinnerOpen)
                return false;
            
            if (NPPlayer.instance.dinnerComp.permitList.Count > 0)
                return true;

            foreach (GDinnerTypeRefObj dinnerType in GRefdataCoreMgr.instance.dinnerTypeRefCore.refList)
            {
                if(dinnerType.is_permit_open)
                    continue;
                if (GCommon.isItemEnough(dinnerType.open_cost, false))
                {
                    return true;
                }
                
            }

            return false;
        }

        public void setHasEnterDinner()
        {
            _m_hasEnterDinner = true;
            onRewardChg?.Invoke();
        }
        
        public bool hasPermit()
        {
            if (_m_permitList != null && _m_permitList.Count > 0)
                return true;

            return false;
        }

        #region 消息

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqDinnerInit()
        {
            NPGSClientListener.sendMsgByLog(GSWriter_002_InitOp.make_002_060_ReqDinnerInit());
        }

        /// <summary>
        /// 初始化回包
        /// </summary>
        /// <param name="_info"></param>
        public void retDinnerInit(GS2GC_002_060_RetDinnerInit _info)
        {
            _setMyDinnerId(_info.getInstanceId());
            _m_hasOwenrReward = _info.getHasOwnerReward();
            _m_permitList = new List<DinnerPermit>();
            foreach (Dinner_Permit permit in _info.getPermitList())
            {
                DinnerPermit item = new DinnerPermit(permit);
                _m_permitList.Add(item);
            }
            setInitDone();
        }

        private void _permitTick()
        {
            if (_m_permitList == null || _m_permitList.Count <= 0)
            {
                _m_dinnerPermitTick.setDisable();
                return;
            }
            for (int i = _m_permitList.Count - 1; i >= 0; i--)
            {
                DinnerPermit permit = _m_permitList[i];
                // 如果超过了有效时间，就删除
                if (permit.isExpired)
                {
                    _m_permitList.RemoveAt(i);
                    onPermitChg?.Invoke();
                }
            }
        }

        /// <summary>
        /// 删除凭证
        /// </summary>
        /// <param name="_permitInstanceId"></param>
        private void _removePermit(long _permitInstanceId)
        {
            if (_m_permitList == null) return;
            for (var i = _m_permitList.Count - 1; i >= 0; i--)
            {
                var permit = _m_permitList[i];
                if (permit != null && _permitInstanceId == permit.id)
                {
                    _m_permitList.Remove(permit);
                    break;
                }
            }
        }

        /// <summary>
        /// 设置当前宴会信息
        /// </summary>
        /// <param name="_dinnerId"></param>
        private void _setMyDinnerId(long _dinnerId)
        {
            if (_dinnerId == _m_dinnerInstanceId)
                return;
            _m_dinnerInstanceId = _dinnerId;
            WinMsg.SendMsg(WinMsgType.ON_DINNER_CHG);
        }

        
        
        /// <summary>
        /// 开启宴会（普通模式）
        /// </summary>
        public void reqStartDinner(long _dinnerId, Action<GS2GC_019_001_RetStartDinner> _dealDone)
        {
            NPGSClientListener.sendRequestByLog( new GC2GS_019_001_ReqStartDinner(_dinnerId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_019_001_RetStartDinner>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }));
        }
        
        /// <summary>
        /// 请求开启宴会（许可证模式）
        /// </summary>
        /// <param name="_permitInstanceId"></param>
        /// <param name="_dinnerId"></param>
        public void reqStartDinnerByPermit(long _permitInstanceId, Action<GS2GC_019_002_RetStartDinnerByPermit> _dealDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_002_ReqStartDinnerByPermit(_permitInstanceId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_019_002_RetStartDinnerByPermit>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                    _removePermit(_permitInstanceId);
                }));
        }
        
        
        /// <summary>
        /// 请求宴会交互记录
        /// </summary>
        /// <param name="_dealDone"></param>
        public void reqJoinedPlayerList(Action<GS2GC_019_003_RetJoinedPlayerList> _dealDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_003_ReqJoinedPlayerList(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_019_003_RetJoinedPlayerList>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }));
        }
        
        /// <summary>
        /// 开宴索引记录列表
        /// </summary>
        /// <param name="_dealDone"></param>
        public void reqGetStartLogIdxList(Action<GS2GC_019_004_RetGetStartLogIdxList> _dealDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_004_ReqGetStartLogIdxList(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_019_004_RetGetStartLogIdxList>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }));
        }
        
        
        /// <summary>
        /// 开宴记录详情
        /// </summary>
        /// <param name="_dealDone"></param>
        public void reqGetStartLogInfo(long _instanceId, Action<GS2GC_019_005_RetGetStartLogInfo> _dealDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_005_ReqGetStartLogInfo(_instanceId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_019_005_RetGetStartLogInfo>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }));
        }
        
        /// <summary>
        /// 请求宴会列表
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="_num"></param>
        public void reqGetDinnerList(int _page, int _num, Action<GS2GC_019_006_RetGetDinnerIdxList> _dealDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_006_ReqGetDinnerIdxList(_page, _num),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_019_006_RetGetDinnerIdxList>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }));
        }

        /// <summary>
        /// 请求宴会详情
        /// </summary>
        /// <param name="_instanceId"></param>
        public void reqDinnerDetailInfo(long _instanceId, Action<GS2GC_019_007_RetGetDinnerInfo> _dealDone, Action _failDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_007_ReqGetDinnerInfo(_instanceId),
                new CommonRequestCallbackProtocolDealer<GS2GC_019_007_RetGetDinnerInfo>((_info) =>
                {
                    _dealDone?.Invoke(_info);
                }, (_errCode) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _failDone?.Invoke();
                }));
        }
        
        /// <summary>
        /// 请求加入宴会
        /// </summary>
        /// <param name="_dinnerId"></param>
        /// <param name="_seateType"></param>
        /// <param name="_seatIndex"></param>
        /// <param name="_costId"></param>
        public void reqJoinDinner(long _instanceId, long _costId, Action<GS2GC_019_008_RetJoinDinner> _dealDone, Action _onFail)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_008_ReqJoinDinner(_instanceId, _costId),
                new CommonRequestCallbackProtocolDealer<GS2GC_019_008_RetJoinDinner>(_dealDone, (_errCode) =>
                {
                    NPGUIAddSceneCenterTip.instance.showErrorInfo(_errCode);
                    _onFail?.Invoke();
                }));
        }
        
        /// <summary>
        /// 领取开宴结算奖励
        /// </summary>
        /// <param name="_dealDone"></param>
        public void reqTakeOpenReward(Action<GS2GC_019_009_RetTakeOpenReward> _dealDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_009_ReqTakeOpenReward(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_019_009_RetTakeOpenReward>((_info) =>
                {
                    _m_hasOwenrReward = false;
                    _dealDone?.Invoke(_info);
                    onRewardChg?.Invoke();
                }));
        }
        
        /// <summary>
        /// 取指定宴会的上一条宴会数据
        /// </summary>
        public void reqGetPreDinnerInfo(long _instanceId, int _idx, Action<GDinnerInfo> _dealDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_011_ReqGetPreDinnerInfo(_instanceId, _idx),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_019_011_RetGetPreDinnerInfo>((_info) =>
                {
                    _dealDone?.Invoke(new GDinnerInfo(_info.getInfo(), _info.getIdx(), _info.getHasPre(), _info.getHasNext()));
                }));
        }
        /// <summary>
        /// 取指定宴会的下一条宴会数据
        /// </summary>
        public void reqGetNextDinnerInfo(long _instanceId, int _idx, Action<GDinnerInfo> _dealDone)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_012_ReqGetNextDinnerInfo(_instanceId, _idx),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_019_012_RetGetNextDinnerInfo>((_info) =>
                {
                    _dealDone?.Invoke(new GDinnerInfo(_info.getInfo(), _info.getIdx(), _info.getHasPre(), _info.getHasNext()));
                }));
        }

        public void reqServerInviteList(int _count, Action<List<DinnerInviteInfo>> _dealDone)
        {
            if (AccountSettingMgr.instance.dinnerSaver.isHasReqServerInviteList(_m_dinnerInstanceId))
            {
                List<DinnerInviteInfo> inviteInfos = new List<DinnerInviteInfo>();

                foreach (long cid in AccountSettingMgr.instance.dinnerSaver.getServerInviteCidList(_m_dinnerInstanceId))
                {
                    inviteInfos.Add(new DinnerInviteInfo(cid));
                }
                _dealDone?.Invoke(inviteInfos);
                return;
            }
            NPGSClientListener.sendRequestByLog(new GC2GS_007_007_ReqGetOnlineCidList(_count),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_007_007_RetGetOnlineCidList>((_info) =>
                {
                    if(_info == null)
                        return;
                    List<DinnerInviteInfo> inviteInfos = new List<DinnerInviteInfo>();
                    List<long> cidList = _info.getCidList();
                    if(cidList == null || cidList.Count <= 0)
                        return;
                    foreach (long cid in cidList)
                    {
                        inviteInfos.Add(new DinnerInviteInfo(cid));
                    }
                    AccountSettingMgr.instance.dinnerSaver.setReqServerInviteListInstanceId(_m_dinnerInstanceId, cidList);
                    _dealDone?.Invoke(inviteInfos);
                }));
        }
        
        /// <summary>
        /// 请求邀请玩家参加宴会
        /// </summary>
        /// <param name="_callback"></param>
        public void reqSendInviteToPlayer(long _cid, Action<bool> _callback = null)
        {
            reqDinnerDetailInfo(dinnerInstanceId, (_info) =>
            {
                if (_info != null)
                {
                    GDinnerInfo dinnerInfo = new GDinnerInfo(_info.getInfo(), _info.getIdx(), _info.getHasPre(),
                        _info.getHasNext());
                    NPChatMsgDinnerInviteInfo inviteMsgInfo = NPMsgDetailInfoFactory.createDinnerInviteMsgInfo(dinnerInfo);
                    NPPlayer.instance.chatComp.sendPrivateChatMsg(_cid, inviteMsgInfo, () => { _callback?.Invoke(true);});
                }
            }, () => { _callback?.Invoke(false);});
        }

        /// <summary>
        /// 请求分享宴会给全服聊天
        /// </summary>
        /// <param name="_callback"></param>
        public void reqShareDinnerToServer(Action<bool> _callback = null)
        {
            long waitTime = (AccountSettingMgr.instance.dinnerSaver.lastServerShareTimeMs + GRefdataCoreMgr.instance.npGeneral.dinner_server_share_cd) - FpsAndPingMgr.instance.serverTimeTag ;
            if (waitTime > 0)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_share_fail_tip, TimeUtil.millisecondsToTime_ms(waitTime)));
                return;
            }
            reqDinnerDetailInfo(dinnerInstanceId, (_info) =>
            {
                if (_info != null)
                {
                    AccountSettingMgr.instance.dinnerSaver.lastServerShareTimeMs = FpsAndPingMgr.instance.serverTimeTag;
                    GDinnerInfo dinnerInfo = new GDinnerInfo(_info.getInfo(), _info.getIdx(), _info.getHasPre(), _info.getHasNext());
                    NPChatMsgDinnerInviteInfo inviteMsgInfo = NPMsgDetailInfoFactory.createDinnerInviteMsgInfo(dinnerInfo);
                    NPRoomChatInfo chatInfo = NPPlayer.instance.chatComp.getRoomChatInfo(ENPChatRoomType.US_SERVER);
                    if (chatInfo != null)
                    {
                        chatInfo.sendMsg(inviteMsgInfo);
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.dinner_share_server_suc_tip);
                    }
                }
            }, () => { _callback?.Invoke(false);});
        }
        /// <summary>
        /// 请求分享宴会给公会
        /// </summary>
        /// <param name="_callback"></param>
        public void reqShareDinnerToGuild(Action<bool> _callback = null)
        {
            long waitTime = (AccountSettingMgr.instance.dinnerSaver.lastGuildShareTimeMs + GRefdataCoreMgr.instance.npGeneral.dinner_guild_share_cd) - FpsAndPingMgr.instance.serverTimeTag ;
            if (waitTime > 0)
            {
                NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.dinner_share_fail_tip, TimeUtil.millisecondsToTime_ms(waitTime)));
                return;
            }
            reqDinnerDetailInfo(dinnerInstanceId, (_info) =>
            {
                if (_info != null)
                {
                    AccountSettingMgr.instance.dinnerSaver.lastGuildShareTimeMs = FpsAndPingMgr.instance.serverTimeTag;
                    GDinnerInfo dinnerInfo = new GDinnerInfo(_info.getInfo(), _info.getIdx(), _info.getHasPre(), _info.getHasNext());
                    NPChatMsgDinnerInviteInfo inviteMsgInfo = NPMsgDetailInfoFactory.createDinnerInviteMsgInfo(dinnerInfo);
                    NPRoomChatInfo chatInfo = NPPlayer.instance.chatComp.getRoomChatInfo(ENPChatRoomType.GUILD);
                    if (chatInfo != null)
                    {
                        chatInfo.sendMsg(inviteMsgInfo);
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.dinner_share_guild_suc_tip);
                    }
                }
            }, () => { _callback?.Invoke(false);});
        }
        /// <summary>
        /// 请求聊天宴会信息
        /// </summary>
        /// <param name="_callback"></param>
        public void reqCheckDinnerInvite(long _dinnerInstanceId, Action<bool, int> _callback = null)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_019_014_ReqCheckDinnerInvite(_dinnerInstanceId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_019_014_RetCheckDinnerInvite>((_isSuc, _msg) =>
                {
                    _callback?.Invoke(_isSuc, _msg.getJoinerCount());
                }, null, false));
        }
        
        /// <summary>
        /// 宴会开启推送
        /// </summary>
        /// <param name="_info"></param>
        public void OnDinnerAdd(GS2GC_019_050_OnDinnerAdd _info)
        {
            _setMyDinnerId(_info.getInstanceId());
        }

        /// <summary>
        /// 宴会结束推送
        /// </summary>
        /// <param name="_info"></param>
        public void OnDinnerEnd(GS2GC_019_051_OnDinnerEnd _info)
        { 
            _m_hasOwenrReward = true;
            _setMyDinnerId(0);
            onRewardChg?.Invoke();
        }

        /// <summary>
        /// 参加宴会玩家推送
        /// </summary>
        /// <param name="_info"></param>
        public void OnJoinerAdd(GS2GC_019_053_OnJoinerAdd _info)
        {
            if(_info == null)
                return;
            GDinnerJoinCostRefObj costRefObj = GRefdataCoreMgr.instance.dinnerJoinCostRefCore.getRef(_info.getCostId());
            string giftName = costRefObj !=null ? TextTranslate.instance.getLanguage(costRefObj.name) : "";
            var dinnerTypeRefObj = GRefdataCoreMgr.instance.dinnerTypeRefCore.getRef(_info.getDinnerId());
            string dinnerName = dinnerTypeRefObj !=null ? TextTranslate.instance.getLanguage(dinnerTypeRefObj.name) : "";
            GCommon.reqPlayerInfo(_info.getCid(), (_playerInfo) =>
            {
                if(_playerInfo == null)
                    return;
                string name = _playerInfo.name;
                NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.dinner_player_join_my_dinner_tip, name, giftName, dinnerName), NPConst.C_DINNER_JOIN_MY_DINNER_TIP_ID);
            });
            
            WinMsg.SendMsg(WinMsgType.ON_MY_DINNER_ADD, _info.getCid());
        }
        
        /// <summary>
        /// 宴会凭证推送
        /// </summary>
        /// <param name="_info"></param>
        public void OnPermitAdd(GS2GC_019_052_OnPermitAdd _info)
        {
            _m_permitList.Add(new DinnerPermit(_info.getPermit()));
            _m_dinnerPermitTick.setDisable();
            _m_dinnerPermitTick = ALCommonTaskController.CommonEnableTickActionAddMonoTask(_permitTick);
            onPermitChg?.Invoke();
        }
        #endregion
    }
}

