using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using UnityEngine;

namespace GOE
{
    /// <summary>
    /// 联盟成员任命界面
    /// </summary>
    public class GGUIWndGuildAppoint : _ANPGGUIBasicWnd<GGUIMonoGuildAppoint>
    {
        private static GGUIWndGuildAppoint _g_instance = new GGUIWndGuildAppoint();
        public static GGUIWndGuildAppoint instance
        {
            get
            {
                if (null == _g_instance)
                    _g_instance = new GGUIWndGuildAppoint();
                return _g_instance;
            }
        }

        //成员信息
        private GuildMemberInfo _m_memberInfo;
        //任命item列表
        private List<GGUIWndGuildAppointItem> _m_lAppointItemList;
        //是否点击了任命
        private bool _m_bIsClickAppoint;
        //是否点击了转让
        private bool _m_bIsClickTransfer;
        //是否点击了踢出
        private bool _m_bIsClickKickOut;
        //显示序列号
        private long _m_lShowSerialize;

        public GGUIWndGuildAppoint() : base(EALUIWndLayer.ADDITION)
        {

        }

        protected override string _monoAssetPath { get { return GGUIMonoGuildAppoint.assetPath; } }

        protected override string _monoObjName { get { return GGUIMonoGuildAppoint.objName; } }

        protected override _AALResourceCore _resourceCore { get { return GameResCore.instance; } }

        protected override void _onShowWnd()
        {
            WinMsg.RegisterMsg(WinMsgType.ON_GUILD_MEMBER_BASE_INFO_CHG, _onMemberInfoChg);
            _m_bIsClickAppoint = false;
            _m_bIsClickTransfer = false;
            _m_bIsClickKickOut = false;
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onHideWnd()
        {
            WinMsg.UnregisterMsg(WinMsgType.ON_GUILD_MEMBER_BASE_INFO_CHG, _onMemberInfoChg);
            _m_bIsClickAppoint = false;
            _m_bIsClickTransfer = false;
            _m_bIsClickKickOut = false;
            _m_lShowSerialize = ALSerializeOpMgr.next();
        }

        protected override void _onReset()
        {
        }

        protected override void _onDiscard()
        {
            if (wnd == null)
                return;

            if (_m_lAppointItemList != null)
            {
                for (int i = 0; i < _m_lAppointItemList.Count; i++)
                {
                    if(_m_lAppointItemList[i] != null)
                        _m_lAppointItemList[i].discard();
                }
                _m_lAppointItemList.Clear();
                _m_lAppointItemList = null;
            }

            ALUGUICommon.uncombineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.uncombineBtnClick(wnd.btnTransferLeader, _onClickTransferLeader);
            ALUGUICommon.uncombineBtnClick(wnd.btnKickOut, _onClickKickOut);
        }

        protected override void _onWndInitDone()
        {
            if (wnd == null)
                return;

            _m_lAppointItemList = new List<GGUIWndGuildAppointItem>();
            if (wnd.monoAppointTypeItemList != null)
            {
                for (int i = 0; i < wnd.monoAppointTypeItemList.Count; i++)
                {
                    GGUIWndGuildAppointItem item = new GGUIWndGuildAppointItem(wnd.monoAppointTypeItemList[i]);
                    item.onClickItem += _onClickAppointItem;
                    _m_lAppointItemList.Add(item);
                }
            }

            ALUGUICommon.combineBtnClick(wnd.btnClose, _onClickClose);
            ALUGUICommon.combineBtnClick(wnd.btnTransferLeader, _onClickTransferLeader);
            ALUGUICommon.combineBtnClick(wnd.btnKickOut, _onClickKickOut);
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="_info"></param>
        public void setInfo(GuildMemberInfo _info)
        {
            if (_info == null)
                return;

            _m_memberInfo = _info;
            _refreshWnd();
        }

        private void _refreshWnd()
        {
            if (wnd == null || _m_memberInfo == null)
                return;

            //根据权限显隐按钮
            ALUGUICommon.setGameObjEnable(wnd.btnTransferLeader,NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.TRANSFER_LEADER));
            ALUGUICommon.setGameObjEnable(wnd.btnKickOut,NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.KICK_OUT));

            //获取比自己职位低的职位列表
            List<EGuildPositionType> positionTypeList = new List<EGuildPositionType>();
            GuildPositionRefObj selfPositionRef = NPPlayer.instance.guildComp.guildInfo?.getSelfPositionRef();
            GuildPositionRefObj tempRef = selfPositionRef;
            while (tempRef != null && tempRef.pre_position != EGuildPositionType.NONE)
            { 
                tempRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef((long) tempRef.pre_position);
                if (tempRef != null)
                    positionTypeList.Add(tempRef.type);
            }

            if (_m_lAppointItemList != null)
            {
                for (int i = 0; i < _m_lAppointItemList.Count; i++)
                {
                    if (_m_lAppointItemList[i] != null)
                    {
                        //根据权限显隐按钮
                        if(!positionTypeList.Contains(_m_lAppointItemList[i].positionType))
                            _m_lAppointItemList[i].hideWnd();
                        else
                        {
                            _m_lAppointItemList[i].showWnd();
                            _m_lAppointItemList[i].setInfo(_m_memberInfo.positionId);
                        }
                    }
                }
            }

            //刷新描述
            GuildPositionRefObj deputyLeaderPositionRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef((long) EGuildPositionType.DEPUTY_LEADER);
            GuildPositionRefObj elitePositionRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef((long) EGuildPositionType.ELITE);
            ALUGUICommon.setLabelTxt(wnd.txtAppointDesc,
                TextTranslate.instance.getLanguage(TransKeyConst.guild_appointConditionDesc_num_num,
                    elitePositionRef?.trans_need_historical_contributions,
                    deputyLeaderPositionRef?.trans_need_historical_contributions));
        }

        //联盟成员信息变更
        private void _onMemberInfoChg(params object[] _objects)
        {
            if (_objects == null || _objects.Length < 1)
                return;

            long cid = (long) _objects[0];

            if (_m_memberInfo != null && _m_memberInfo.cid == cid)
            {
                _refreshWnd();
            }
        }

        #region 点击按钮

        //点击任命按钮
        private void _onClickAppointItem(GGUIWndGuildAppointItem _item)
        {
            if (wnd == null || _m_memberInfo == null || _item == null || _m_bIsClickAppoint)
                return;

            //如果是同一个类型则不处理
            if ((long) _item.positionType == _m_memberInfo.positionId)
                return;

            //检查权限
            if (!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.POSITION_APPOINT, true))
                return;

            //检查该职位人数
            if (_item.positionType != EGuildPositionType.MEMBER)
            {
                GuildInfo guildInfo = NPPlayer.instance.guildComp.guildInfo;
                long limitCount = 0;
                if (guildInfo != null && guildInfo.guildLevelRefObj != null && guildInfo.guildLevelRefObj.position_limit_list != null)
                {
                    for (int j = 0; j < guildInfo.guildLevelRefObj.position_limit_list.Count; j++)
                    {
                        if (guildInfo.guildLevelRefObj.position_limit_list[j] != null &&
                            guildInfo.guildLevelRefObj.position_limit_list[j].enumValue == _item.positionType)
                        {
                            limitCount = guildInfo.guildLevelRefObj.position_limit_list[j].longValue;
                            break;
                        }
                    }
                }

                if (limitCount <= NPPlayer.instance.guildComp.getTargetPositionMemberCount(_item.positionType))
                {
                    //联盟职位人数已达上限
                    NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_positionMemberReachLimit_none);
                    return;
                }
            }

            Action dealAppoint = () =>
            {
                //请求任命
                NPPlayer.instance.guildComp.reqGuildPositionAppoint(_m_memberInfo.cid, (long)_item.positionType, (_isSuc) =>
                {
                    _m_bIsClickAppoint = false;
                    //任命成功
                    if(_isSuc)
                        NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_appointSucceed_none);
                });
            };

            //检查贡献度
            GuildPositionRefObj targetPositionRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef((long)_item.positionType);
            if (targetPositionRef == null)
                return;
            _m_bIsClickAppoint = true;
            //是否需要贡献度
            if (targetPositionRef.trans_need_historical_contributions > 0)
            {
                long serialize = _m_lShowSerialize;
                _m_memberInfo.getContributeInfo(_contributeInfo =>
                {
                    if (_contributeInfo == null || serialize != _m_lShowSerialize)
                        return;

                    //贡献度不足
                    if (_contributeInfo.getTotalContribute() < targetPositionRef.trans_need_historical_contributions)
                    {
                        _m_bIsClickAppoint = false;
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_contributeNotEnough_amount, targetPositionRef.trans_need_historical_contributions));
                        return;
                    }

                    dealAppoint();
                });
            }
            else
            {
                dealAppoint();
            }
        }

        //点击关闭按钮
        private void _onClickClose(GameObject _go)
        {
            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_APPOINT);
        }

        //点击转让盟主
        private void _onClickTransferLeader(GameObject _go)
        {
            if (_m_memberInfo == null || _m_bIsClickTransfer)
                return;

            if (_m_memberInfo.positionId != (long) EGuildPositionType.DEPUTY_LEADER)
            {
                //该成员不是副盟主
                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_memberIsNotDeputyLeader_none);
                return;
            }

            //检查权限
            if (!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.TRANSFER_LEADER, true))
                return;

            
            Action dealTransfer = () =>
            {
                NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guild_transferLeaderDesc_none),
                    TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                    null,
                    TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                    () =>
                    {
                        //请求转让
                        NPPlayer.instance.guildComp.reqTransferGuild(_m_memberInfo.cid, (_isSuc) =>
                        {
                            _m_bIsClickTransfer = false;
                            //转让成功
                            if (_isSuc)
                            {
                                NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_transferLeaderSucceed_none);
                                QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_APPOINT);
                            }
                        });
                    });
            };

            //检查贡献度
            GuildPositionRefObj targetPositionRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef((long)EGuildPositionType.LEADER);
            if (targetPositionRef == null)
                return;
            _m_bIsClickTransfer = true;
            //是否需要贡献度
            if (targetPositionRef.trans_need_historical_contributions > 0)
            {
                long serialize = _m_lShowSerialize;
                _m_memberInfo.getContributeInfo(_contributeInfo =>
                {
                    if (_contributeInfo == null || serialize != _m_lShowSerialize)
                        return;

                    //贡献度不足
                    if (_contributeInfo.getTotalContribute() < targetPositionRef.trans_need_historical_contributions)
                    {
                        _m_bIsClickTransfer = false;
                        NPGUIAddSceneCenterTip.instance.showTextInfo(TextTranslate.instance.getLanguage(TransKeyConst.guild_contributeNotEnough_amount, targetPositionRef.trans_need_historical_contributions));
                        return;
                    }

                    dealTransfer();
                });
            }
            else
            {
                dealTransfer();
            }
        }

        //点击踢出联盟
        private void _onClickKickOut(GameObject _go)
        {
            if (_m_memberInfo == null || _m_bIsClickKickOut)
                return;

            //检查权限
            if (!NPPlayer.instance.guildComp.checkHavePermission(EGuildPermissionType.KICK_OUT, true))
                return;

            NPMesMgr.instance.showTwoBtnMes(TextTranslate.instance.getLanguage(TransKeyConst.guild_kickOutDesc_none),
                TextTranslate.instance.getLanguage(TransKeyConst.cancel),
                null,
                TextTranslate.instance.getLanguage(TransKeyConst.confirm),
                () =>
                {
                    _m_bIsClickKickOut = true;
                    //请求踢出
                    NPPlayer.instance.guildComp.reqGuildKickOutMember(_m_memberInfo.cid, (_isSuc) =>
                    {
                        _m_bIsClickKickOut = false;
                        //踢出成功
                        if (_isSuc)
                        {
                            NPGUIAddSceneCenterTip.instance.showTransTextInfo(TransKeyConst.guild_kickOutSucceed_none);
                            QueueMgr.instance.forceCloseNodeByTag(UINodeTagConst_Guild.C_GUILD_APPOINT);
                        }
                    });
                });
        }

        #endregion
    }
}