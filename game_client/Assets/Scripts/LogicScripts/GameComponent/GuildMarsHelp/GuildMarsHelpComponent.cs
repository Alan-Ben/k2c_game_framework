using System;
using System.Collections.Generic;
using GC2GS.p002_InitOp;
using GC2GS.p042_GuildRelatedOp;
using GS2GC.p002_InitOp;
using GS2GC.p042_GuildRelatedOp;
using Common.GuildObj;
using Common.GuildEnum;
using Common.MarsEnum;
using JetBrains.Annotations;
using NPCommon;
using NPEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟火星求助管理组件
    /// </summary>
    public class GuildMarsHelpComponent : _ANPBasicPlayerComponent
    {
        [NotNull]private List<_IGuildMarsHelp> _m_myMarsHelpList = new List<_IGuildMarsHelp>();
        [NotNull]private List<long> _m_canDealIdList = new List<long>(); // 可以帮助的火星求助实例ID列表
        public int canDealCount
        {
            get
            {
                if (_checkHavePrivilege())
                    return 0;
                return _m_canDealIdList?.Count ?? 0;
            }
        }

        public GuildMarsHelpComponent(NPPlayerComponentMgr _compMgr) : base(_compMgr)
        {
        }

        //属性
        private static readonly ENPPlayerCompType[] _g_DependComp = { ENPPlayerCompType.GUILD, ENPPlayerCompType.PLAYER_BUFF };
        public override bool isMustInit { get { return true; } }
        public override ENPPlayerCompType compType { get { return ENPPlayerCompType.GUILD_MARS_HELP; } }
        public override ENPPlayerCompType[] dependCompList => _g_DependComp;

        /// <summary>
        /// 是否允许提前初始化。提前初始化的意思是在依赖项没有完成初始化之前就进行初始化操作（一般是提前发送消息）
        /// 在初始化结果消息返回的时候，通过特殊的初始化函数dealPreInitFunc进行处理函数注册，再依赖项完成之后才进行初始化处理
        /// </summary>
        public override bool canPreInit { get { return true; } }

        public override void presendInitProtocol()
        {
            //请求初始化
            _reqGuildMarsHelpInit();
        }

        protected override void _dealInit()
        {
        }

        protected override void _onInitDone()
        {
            WinMsg.RegisterMsgAct(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.RegisterMsgAct(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff += _onBuffChg;
            NPPlayer.instance.playerBuffComp.onRemovePlayerBuff += _onRemoveBuff;
        }

        public override void onAllCompInited()
        {
            base.onAllCompInited();
            _refreshRedTip();
        }

        protected override void _onInitFail()
        {
        }

        protected override void _discard()
        {
            _m_canDealIdList?.Clear();
            _m_myMarsHelpList?.Clear();
            WinMsg.UnregisterMsgAct(WinMsgType.ON_LEAVE_GUILD, _onLeaveGuild);
            WinMsg.UnregisterMsgAct(WinMsgType.ON_JOIN_GUILD, _onJoinGuild);
            NPPlayer.instance.playerBuffComp.onChgPlayerBuff -= _onBuffChg;
            NPPlayer.instance.playerBuffComp.onRemovePlayerBuff -= _onRemoveBuff;
        }

        private void _refreshRedTip()
        {
            int count = 0;
            if (!_checkHavePrivilege() && NPPlayer.instance.guildComp.isJoinGuild())
            {
                // 如果有可以帮助的求助实例，显示红点
                if (_m_canDealIdList != null && _m_canDealIdList.Count > 0)
                    count = _m_canDealIdList.Count;
            }
            RedTipMgr.instance.setCountByRefRedTipId(RedTipConst.RED_GUILD_MARS_HELP, count);
        }

        /// <summary>
        /// 根据ID获取我的求助信息
        /// </summary>
        public _IGuildMarsHelp getMyHelpInfoByHelpId(long _id)
        {
            foreach (var helpInfo in _m_myMarsHelpList)
            {
                if (helpInfo != null && helpInfo.guildHelpId == _id)
                    return helpInfo;
            }
            return null;
        }
        public _IGuildMarsHelp getMyHelpInfoByTypeId(EGuildMarsHelpObjType _type, long _objId)
        {
            foreach (var helpInfo in _m_myMarsHelpList)
            {
                if (helpInfo != null && helpInfo.helpObjType == _type && helpInfo.guildHelpObjId == _objId)
                    return helpInfo;
            }
            return null;
        }
        public bool isJoinGuildAndBuildHelpBuilding()
        {
            if (!NPPlayer.instance.guildComp.isJoinGuild())
                return false;
            bool hasBuildHelpBuilding = false;
            List<MarsBuildingInfo> buildingInfos = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoList();
            foreach (MarsBuildingInfo buildingInfo in buildingInfos)
            {
                if(buildingInfo.refObj.building_type == EMarsBuildingType.HELP && (buildingInfo.state == MarsBuildingInfo.StateType.Normal || buildingInfo.state == MarsBuildingInfo.StateType.Upgrading))
                {
                    hasBuildHelpBuilding = true;
                    break;
                }
            }

            return hasBuildHelpBuilding;
        }

        public bool canAskGuildHelp(EGuildMarsHelpObjType _type, long _objId)
        {
            if (!isJoinGuildAndBuildHelpBuilding())
                return false; 
            var helpInfo = getMyHelpInfoByTypeId(_type, _objId);
            if (helpInfo != null)
                return helpInfo.guildHelpId <= 0;
            return false;
        }

        public void regMyMarsHelp(_IGuildMarsHelp _myMarsHelp)
        {
            if (_m_myMarsHelpList.Contains(_myMarsHelp))
                return;
            _m_myMarsHelpList.Add(_myMarsHelp);
        }
        
        public void unregMyMarsHelp(_IGuildMarsHelp _myMarsHelp)
        {
            _m_myMarsHelpList.Remove(_myMarsHelp);
        }

        /// <summary>
        /// 检查是否有自动处理特权
        /// </summary>
        /// <returns></returns>
        private bool _checkHavePrivilege()
        {
            NPPlayerBuffInfo buffInfo = NPPlayer.instance.playerBuffComp.lookup(GRefdataCoreMgr.instance.npGeneral.guild_mars_help_auto_deal_buff_id);
             return buffInfo != null && buffInfo.layer > 0;
        }
    
        #region 消息事件

        // 离开联盟
        private void _onLeaveGuild()
        {
            _m_canDealIdList?.Clear();
            _refreshRedTip();
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG);
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG);
        }

        // 加入联盟
        private void _onJoinGuild()
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_002_067_ReqGuildMarsHelpInit(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_002_067_RetGuildMarsHelpInit>((_msg) =>
                {
                    _m_canDealIdList.Clear();
                    if(!_checkHavePrivilege())
                        _m_canDealIdList.AddRange( _msg.getCanDealIdList());
                    
                    _refreshRedTip();
                    WinMsg.SendMsg(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG);
                    WinMsg.SendMsg(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG);
                }));
        }

        // buff变化
        private void _onBuffChg(NPPlayerBuffInfo _info, int _layer, long _curLeftTimeMS)
        {
            _refreshRedTip();
        }

        // buff移除
        private void _onRemoveBuff(NPPlayerBuffInfo _info)
        {
            _refreshRedTip();
        }

        #endregion

        #region S2C

        /// <summary>
        /// 初始化回包
        /// </summary>
        public void retGuildMarsHelpInit(GS2GC_002_067_RetGuildMarsHelpInit _info)
        {
            if (_info != null)
            {
                // 初始化可帮助的求助ID列表
                _m_canDealIdList?.Clear();
                if (_info.getCanDealIdList() != null && !_checkHavePrivilege())
                {
                    foreach (var id in _info.getCanDealIdList())
                    {
                        _m_canDealIdList.Add(id);
                    }
                }
            }
            
            _refreshRedTip();
            setInitDone();
        }
        
        /// <summary>
        /// 可以帮助的火星求助实例新增推送
        /// </summary>
        public void onCanDealMarsHelpAdd(GS2GC_042_051_OnCanDealMarsHelpAdd _msg)
        {
            if (_msg == null)
                return;

            long canDealId = _msg.getCanDealId();
            if (_m_canDealIdList != null && !_m_canDealIdList.Contains(canDealId) && !_checkHavePrivilege())
            {
                _m_canDealIdList.Add(canDealId);
            }
            
            // 刷新红点提示
            _refreshRedTip();

            // 发送消息通知UI更新
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG, canDealId);
        }

        /// <summary>
        /// 可以帮助的火星求助实例减少推送
        /// </summary>
        public void onCanDealMarsHelpDel(GS2GC_042_052_OnCanDealMarsHelpDel _msg)
        {
            if (_msg == null)
                return;

            long canDealId = _msg.getCanDealId();
            if (_m_canDealIdList != null && _m_canDealIdList.Contains(canDealId))
            {
                _m_canDealIdList.Remove(canDealId);
            }
            
            // 刷新红点提示
            _refreshRedTip();

            // 发送消息通知UI更新
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG, canDealId);
        }

        /// <summary>
        /// 玩家自身求助被处理推送
        /// </summary>
        public void onMyMarsHelpDealed(GS2GC_042_053_OnMyMarsHelpDealed _msg)
        {
            if (_msg == null)
                return;

            if (_msg.getDealCid() <= 0)
            {
                showHelpBeDealTip("", _msg);
            }
            else
            {
                GCommon.reqPlayerInfo(_msg.getDealCid(), _playerInfo =>
                {
                    showHelpBeDealTip(_playerInfo?.name, _msg);
                });
            }
        }

        private void showHelpBeDealTip(string _playerName, GS2GC_042_053_OnMyMarsHelpDealed _msg)
        {
            switch (_msg.getObjType())
            {
                case EGuildMarsHelpObjType.TEAM_REPAIR:
                    MarsExploreTeamInfo teamInfo = NPPlayer.instance.marsComp.exploreSubComponent.getTeamInfoById(_msg.getObjId());
                    NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(_msg.getIsAuto() ? TransKeyConst.guild_mars_help_auto_my_team_help_be_deal_tip: TransKeyConst.guild_mars_help_my_team_help_be_deal_tip, 
                        _playerName, teamInfo?.name, _msg.getDealedCount(), _msg.getDealLimit()));
                    break;
                case EGuildMarsHelpObjType.TECH_UP: 
                    MarsTechnologyInfo techInfo = NPPlayer.instance.marsComp.technologySubComponent.getTechnologyInfoById(_msg.getObjId());
                    NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(_msg.getIsAuto() ? TransKeyConst.guild_mars_help_auto_my_help_be_deal_tip: TransKeyConst.guild_mars_help_my_help_be_deal_tip, 
                        _playerName, techInfo?.lvl+1, techInfo?.technologyRefObj?.transName, _msg.getDealedCount(), _msg.getDealLimit()));
                    break;
                case EGuildMarsHelpObjType.BUILDING_QUEUE:
                    MarsBuildingInfo buildingInfo = NPPlayer.instance.marsComp.buildingSubComponent.getBuildingInfoByQueueId(_msg.getObjId());
                    if (buildingInfo != null)
                        NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(_msg.getIsAuto() ? TransKeyConst.guild_mars_help_auto_my_help_be_deal_tip: TransKeyConst.guild_mars_help_my_help_be_deal_tip, _playerName, buildingInfo.level, buildingInfo.nameTranslated,
                            _msg.getDealedCount(), _msg.getDealLimit()));
                    break;
            }
        }

        public void onMyMarsHelpChg(GS2GC_042_050_OnMyMarsHelpChg _msg)
        {
            if (_msg == null)
                return;
            _IGuildMarsHelp helpInfo = getMyHelpInfoByTypeId(_msg.getObjType(), _msg.getObjId());
            if (helpInfo != null)
            {
                helpInfo.onGuildHelpChg(_msg.getId(), _msg.getGuildHelpSecs());
            }
            else
            {
                UnityEngine.Debug.Log($"onMyMarsHelpChg 新增的时候，obj还未注册进来");
            }
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _msg.getId());
        }
        /// <summary>
        /// 玩家自身求助数据删除推送
        /// </summary>
        public void onMyMarsHelpDel(GS2GC_042_054_OnMyMarsHelpDel _msg)
        {
            if (_msg == null)
                return;

            // 从我的求助列表中移除
            _IGuildMarsHelp helpInfo = getMyHelpInfoByHelpId(_msg.getId());
            if (helpInfo != null)
            {
                helpInfo.onGuildHelpDel();
                _m_myMarsHelpList.Remove(helpInfo);
            }

            // 发送消息通知UI更新
            WinMsg.SendMsg(WinMsgType.ON_GUILD_MARS_HELP_MY_CHG, _msg.getId());
        }

        #endregion

        #region C2S

        /// <summary>
        /// 请求初始化协议
        /// </summary>
        private void _reqGuildMarsHelpInit()
        {
            NPGSClientListener.sendMsgByLog(new GC2GS_002_067_ReqGuildMarsHelpInit());
        }

        /// <summary>
        /// 请求火星系统求助
        /// </summary>
        /// <param name="_objType">求助目标类型</param>
        /// <param name="_objId">求助目标ID</param>
        /// <param name="_callback"></param>
        public void reqSendMarsHelp(EGuildMarsHelpObjType _objType, long _objId, Action<bool> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_042_001_ReqSendMarsHelp(_objType, _objId),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_042_001_RetSendMarsHelp>((_succ, _msg) =>
                {
                    NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.guild_mars_help_send_help_suc_tip));
                    _callback?.Invoke(_succ);
                }));
        }

        /// <summary>
        /// 处理火星系统求助
        /// </summary>
        /// <param name="_callback"></param>
        public void reqDealMarsHelp(Action<bool> _callback)
        {
            _m_canDealIdList?.Clear();
            NPGSClientListener.sendRequestByLog(new GC2GS_042_002_ReqDealMarsHelp(),
                new CommonRequestSucFailSameCallbackProtocolDealer<GS2GC_042_002_RetDealMarsHelp>((_succ, _msg) =>
                {
                    var itemInfo = GRefdataCoreMgr.instance.npGeneral.guild_mars_help_deal_reward_item;
                    if (itemInfo != null && _msg != null && _msg.getRewardCount() > 0)
                        GCommon.showTextRewardTip(TextTranslate.instance.getLanguage(TransKeyConst.guild_mars_help_deal_help_tip), itemInfo.getItemType(), itemInfo.subId, _msg.getRewardCount() * itemInfo.count);
                    else
                        NPGUIAddSceneCenterTip.instance.showTextTip(TextTranslate.instance.getLanguage(TransKeyConst.guild_mars_help_deal_help_tip));
                    
                    _refreshRedTip();
                    WinMsg.SendMsg(WinMsgType.ON_GUILD_MARS_HELP_CAN_DEAL_CHG);
                    _callback?.Invoke(_succ);
                }));
        }

        /// <summary>
        /// 获取火星求助数据列表（他人）
        /// </summary>
        /// <param name="_callback"></param>
        public void reqMarsHelpList(Action<GS2GC_042_003_RetMarsHelpList> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_042_003_ReqMarsHelpList(),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_042_003_RetMarsHelpList>((_msg) =>
                {
                    _callback?.Invoke(_msg);
                }));
        }
        /// <summary>
        /// 请求火星求助已帮助数量
        /// </summary>
        public void reqGuildMarsHelpInfo(long _guildHelpId, Action<bool, int, int> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_042_004_ReqMarsHelpDealedList(_guildHelpId),
                new CommonRequestCallbackProtocolDealer<GS2GC_042_004_RetMarsHelpDealedList>((_msg) =>
                {
                    if (_msg != null) 
                        _callback?.Invoke(true, _msg.getDealedCount(), _msg.getDealLimit());
                }, (_errorCode) =>
                {
                    _callback?.Invoke(false, 0, 0);
                }));
        }
        #endregion
    }
}