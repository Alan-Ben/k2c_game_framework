using ALPackage;
using Common.GuildEnum;
using JetBrains.Annotations;
using NPEnum;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GOE
{
    public class GGUIWndJoinGuildBtn : _ATALBasicUISubWnd<GGUIMonoJoinGuildBtn>
    {
        private GuildOtherInfo _m_guildOtherInfo;
        
        private bool _m_bIsReqDealProtocol;//是否正在请求处理协议
        private long _m_lReqDealProtocolSerializeId;//请求处理协议的序列号
        
        public GGUIWndJoinGuildBtn(GGUIMonoJoinGuildBtn _wnd) : base(_wnd)
        {
            initWnd();
        }
        
        public event Action<bool> onRetJoinGuild;//收到请求加入联盟回调
        public event Action<bool> onRetApplyJoinGuild;//收到请求申请加入联盟回调
        public event Action<bool> onRetCancelJoinGuild;//收到请求取消加入联盟回调

        protected override void _onShowWnd()
        {
        }

        protected override void _onHideWnd()
        {
            _m_bIsReqDealProtocol = false;
            _m_lReqDealProtocolSerializeId = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd != null)
            {
                ALUGUICommon.uncombineBtnClick(wnd.btnJoin, _clickJoinBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnApply, _clickJoinBtn);
                ALUGUICommon.uncombineBtnClick(wnd.btnCancelJoin, _clickCancelJoinBtn);
            }

            onRetJoinGuild = null;
            onRetApplyJoinGuild = null;
            onRetCancelJoinGuild = null;
        }

        protected override void _onWndInitDone()
        {
            if(wnd == null)
                return;
            
            ALUGUICommon.combineBtnClick(wnd.btnJoin, _clickJoinBtn);
            ALUGUICommon.combineBtnClick(wnd.btnApply, _clickJoinBtn);
            ALUGUICommon.combineBtnClick(wnd.btnCancelJoin, _clickCancelJoinBtn);
        }
        
        public void setData(GuildOtherInfo _guildOtherInfo)
        {
            _m_guildOtherInfo = _guildOtherInfo;

            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if(_m_guildOtherInfo == null || wnd == null)
                return;

            EGuildApplyState guildApplyState = getApplyState();
            
            if(wnd.applyStateShow != null)
                wnd.applyStateShow.setShowData(guildApplyState);

            if (guildApplyState != EGuildApplyState.CAN_FREE_JOIN && guildApplyState != EGuildApplyState.CAN_APPLY_JOIN)
            {
                GGameCommonInfo.grayImage(wnd.onCannotJoinGaryList);
            }
            else
            {
                GGameCommonInfo.disgrayImage(wnd.onCannotJoinGaryList);
            }
        }
        
        private EGuildApplyState getApplyState()
        {
            if (_m_guildOtherInfo == null)
                return EGuildApplyState.NO_DATA;

            if(_m_guildOtherInfo.isApplying)
                return EGuildApplyState.APPLYING;
            
            EGuildCannotJoinReason cannotJoinReason = _getCannotJoinReason(_m_guildOtherInfo, false, out List<_AGuildJoinLimitInfo> _notConformJoinLimitInfoList);
            if (cannotJoinReason != EGuildCannotJoinReason.NONE)//不可加入的情况
            {
                if(_m_guildOtherInfo.joinType == EGuildJoinType.APPROVAL_JOIN)
                    return EGuildApplyState.CANNOT_APPLY_JOIN;
                else if(_m_guildOtherInfo.joinType == EGuildJoinType.FREE_JOIN)
                    return EGuildApplyState.CANNOT_FREE_JOIN;
                else
                    return EGuildApplyState.DECLINE_JOIN;
            }
            else//可加入的情况
            {
                if(_m_guildOtherInfo.joinType == EGuildJoinType.APPROVAL_JOIN)
                    return EGuildApplyState.CAN_APPLY_JOIN;
                else if(_m_guildOtherInfo.joinType == EGuildJoinType.FREE_JOIN)
                    return EGuildApplyState.CAN_FREE_JOIN;
                else
                    return EGuildApplyState.DECLINE_JOIN;
            }
        }
        
        /// <summary>
        /// 获取不可加入联盟的原因
        /// </summary>
        /// <returns></returns>
        private EGuildCannotJoinReason _getCannotJoinReason([NotNull] GuildOtherInfo _guildOtherInfo, bool _showTips, out List<_AGuildJoinLimitInfo> _notConformJoinLimitInfoList)
        {
            _notConformJoinLimitInfoList = null;

            if (NPPlayer.instance.guildComp.isJoinGuild()) //已经加入了一个联盟
            {
                if(_showTips)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_alreadyInAGuildTip_none);
                return EGuildCannotJoinReason.ALREADY_JOIN_A_GUILD;
            }

            if (_guildOtherInfo.isApplying) //正在申请中
            {
                if(_showTips)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_applyingJoinTip_none);
                return EGuildCannotJoinReason.APPLYING;
            }

            if (_guildOtherInfo.joinType == EGuildJoinType.DECLINE_JOIN) //拒绝加入
            {
                if(_showTips)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_guildDeclineJoinTip_none);
                return EGuildCannotJoinReason.DECLINE_JOIN;
            }

            if (_guildOtherInfo.guildLevelRefObj == null || _guildOtherInfo.guildLevelRefObj.member_limit <= _guildOtherInfo.memberCount)
            {
                if(_showTips)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_reqJoinGuildMemberFullTip_none);
                return EGuildCannotJoinReason.MEMBER_FULL;
            }

            _notConformJoinLimitInfoList = _guildOtherInfo.getSelfNotConformJoinLimitingConditions();//获取玩家自身不满足的加入限制条件
            if (_notConformJoinLimitInfoList != null && _notConformJoinLimitInfoList.Count > 0)
            {
                if (_showTips)
                {
                    foreach (var joinLimitInfo in _notConformJoinLimitInfoList)
                    {
                        if(joinLimitInfo != null)
                            NPGUIAddSceneCenterTip.instance.showTransTextInfo(joinLimitInfo.onTryJoinNotConformLimitTip);
                    }
                }
                return EGuildCannotJoinReason.NOT_CONFORM_JOIN_LIMITING;
            }

            if (NPPlayer.instance.guildComp.joinGuildCdEndTimeMs > FpsAndPingMgr.instance.serverTimeTag) //若处于加入联盟操作的CD中
            {
                if (_showTips)
                {
                    long leftTimeMs = NPPlayer.instance.guildComp.joinGuildCdEndTimeMs - FpsAndPingMgr.instance.serverTimeTag;
                    NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_joinGuildInCDTip_str1, TimeUtil.millisecondsToTime_Two(leftTimeMs)));
                }
                return EGuildCannotJoinReason.IN_JOIN_GUILD_CD;
            }

            // 若联盟需要申请加入，且申请加入数量达到上限
            if (_guildOtherInfo.joinType == EGuildJoinType.APPROVAL_JOIN && NPPlayer.instance.guildComp.reqJoinGuildNum >= GRefdataCoreMgr.instance.npGeneral.guild_join_request_limit_num) //申请加入时
            {
                if(_showTips)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_reqJoinGuildNumMax_none);
                return EGuildCannotJoinReason.REQ_JOIN_GUILD_NUM_MAX;
            }
            
            return EGuildCannotJoinReason.NONE;
        }
        
        /// <summary>
        /// 点击加入按钮
        /// </summary>
        private void _clickJoinBtn(GameObject _go)
        {
            if (_m_guildOtherInfo == null || _m_bIsReqDealProtocol)
                return;

            if (!GCommon.isFuncUnlock(ENPFunctionType.GUILD, true))
                return;

            EGuildCannotJoinReason cannotJoinReason = _getCannotJoinReason(_m_guildOtherInfo, true, out List<_AGuildJoinLimitInfo> _notConformJoinLimitInfoList);
            if (cannotJoinReason != EGuildCannotJoinReason.NONE)//有不可加入的情况
                return;

            _m_bIsReqDealProtocol = true;
            long serializeId = _m_lReqDealProtocolSerializeId = ALSerializeOpMgr.next();
            long reqJoinGuildId = _m_guildOtherInfo.guildId;//记录想要加入的联盟的id
            EGuildJoinType guildJoinType = _m_guildOtherInfo.joinType;
            NPPlayer.instance.guildComp.reqJoinGuild(_m_guildOtherInfo.guildId, (_isSucc) =>
            {
                _m_bIsReqDealProtocol = false;
                if (_isSucc)
                {
                    if (guildJoinType == EGuildJoinType.APPROVAL_JOIN)
                    {
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_reqApplyJoinGuildSuccTip_none);
                    }
                    else if (guildJoinType == EGuildJoinType.FREE_JOIN)
                    {
                        
                    }
                }
                
                if(serializeId != _m_lReqDealProtocolSerializeId || (_m_guildOtherInfo != null && _m_guildOtherInfo.guildId != reqJoinGuildId))
                    return;
                
                if (guildJoinType == EGuildJoinType.APPROVAL_JOIN)
                {
                    onRetApplyJoinGuild?.Invoke(_isSucc);
                }
                else if (guildJoinType == EGuildJoinType.FREE_JOIN)
                {
                    onRetJoinGuild?.Invoke(_isSucc);
                }
            });
        }
        
        /// <summary>
        /// 请求取消加入联盟
        /// </summary>
        /// <param name="_go"></param>
        private void _clickCancelJoinBtn(GameObject _go)
        {
            if (_m_guildOtherInfo == null || _m_bIsReqDealProtocol)
                return;

            if (!_m_guildOtherInfo.isApplying)
            {
                Debug.LogError("不是申请中的联盟, 不能取消加入联盟操作");
                return;
            }

            _m_bIsReqDealProtocol = true;
            long serializeId = _m_lReqDealProtocolSerializeId = ALSerializeOpMgr.next();
            long reqJoinGuildId = _m_guildOtherInfo.guildId;//记录想要取消加入的联盟的id
            NPPlayer.instance.guildComp.reqCancelSelfJoinRequest(_m_guildOtherInfo.guildId, (_isSucc) =>
            {
                _m_bIsReqDealProtocol = false;
                if(_isSucc)
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_cancelApplyJoinGuildTip_none);
                
                if(serializeId != _m_lReqDealProtocolSerializeId || (_m_guildOtherInfo != null && _m_guildOtherInfo.guildId != reqJoinGuildId))
                    return;
                
                onRetCancelJoinGuild?.Invoke(_isSucc);
            });
        }
    }
    
    /// <summary>
    /// 联盟不可加入原因
    /// </summary>
    public enum EGuildCannotJoinReason
    {
        [InspectorName("无原因, 直接当作可加入/申请")]
        NONE,
        [InspectorName("已经加入了一个联盟")]
        ALREADY_JOIN_A_GUILD,
        [InspectorName("申请中")]
        APPLYING,
        [InspectorName("拒绝加入(不是申请被拒绝, 是盟主将联盟设置为拒绝加入状态)")]
        DECLINE_JOIN,
        [InspectorName("联盟人数已满")]
        MEMBER_FULL,
        [InspectorName("不满足加入限制")]
        NOT_CONFORM_JOIN_LIMITING,
        [InspectorName("处于加入联盟操作的CD中")]
        IN_JOIN_GUILD_CD,
        [InspectorName("申请加入联盟数量达到上限")]
        REQ_JOIN_GUILD_NUM_MAX,
    }
}