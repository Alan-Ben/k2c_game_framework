using Common.CrossTeamEnum;
using Common.CrossTeamObj;
using GC2GS.p012_ActivityTeamOp;
using GC2GS.p017_ActivityOp;
using GS2GC.p012_ActivityTeamOp;
using GS2GC.p017_ActivityOp;
using System;
using System.Collections.Generic;

namespace GOE
{
    public partial class CommonActivityComponent
    {

        #region S2C

        /// <summary>
        /// 加入队伍
        /// </summary>
        /// <param name="_msg"></param>
        public void onJoinActivityTeam(GS2GC_012_051_OnJoinActivityTeam _msg)
        {
            if(null == _msg)
                return;
            
            _ABaseActivityInfo activityInfo = getActivityInfoByTeamId(_msg.getTeamId());
            if (activityInfo == null)
                return;

            if (activityInfo is _ABaseTeamActivityInfo teamActivityInfo)
            {
                teamActivityInfo.JoinTeam(_msg.getTeam());
            }
        }
        
        /// <summary>
        /// 退出队伍
        /// </summary>
        /// <param name="_msg"></param>
        public void onQuitActivityTeam(GS2GC_012_052_OnQuitActivityTeam _msg)
        {
            if(null == _msg)
                return;
            
            _ABaseActivityInfo activityInfo = getActivityInfoByTeamId(_msg.getTeamId());
            if (activityInfo == null)
                return;

            if (activityInfo is _ABaseTeamActivityInfo teamActivityInfo && teamActivityInfo.activityTeamInfo != null && teamActivityInfo.activityTeamInfo.teamId == _msg.getTeamId())
            {
                teamActivityInfo.quitTeam();
            }
        }

        /// <summary>
        /// 队伍成员移除
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityTeamMemberRemove(GS2GC_012_053_OnActivityTeamMemberRemove _msg)
        {
            if (null == _msg)
                return;

            _ABaseActivityInfo activityInfo = getActivityInfoByTeamId(_msg.getTeamId());
            if (activityInfo == null)
                return;

            if (activityInfo is _ABaseTeamActivityInfo teamActivityInfo && teamActivityInfo.activityTeamInfo != null && teamActivityInfo.activityTeamInfo.teamId == _msg.getTeamId())
            {
                teamActivityInfo.removeTeamMember(_msg.getCid());
            }
        }

        /// <summary>
        /// 队伍解散
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityTeamDissolve(GS2GC_012_054_OnActivityTeamDissolve _msg)
        {
            if(null == _msg)
                return;
            
            _ABaseActivityInfo activityInfo = getActivityInfoByTeamId(_msg.getTeamId());
            if (activityInfo == null)
                return;

            if (activityInfo is _ABaseTeamActivityInfo teamActivityInfo && teamActivityInfo.activityTeamInfo != null && teamActivityInfo.activityTeamInfo.teamId == _msg.getTeamId())
            {
                teamActivityInfo.quitTeam();
            }
        }
        
        /// <summary>
        /// 队伍成员加入
        /// </summary>
        /// <param name="_msg"></param>
        public void onActivityTeamMemberAdd(GS2GC_012_055_OnActivityTeamMemberAdd _msg)
        {
            if(null == _msg)
                return;
            
            _ABaseActivityInfo activityInfo = getActivityInfoByTeamId(_msg.getTeamId());
            if (activityInfo == null)
                return;

            if (activityInfo is _ABaseTeamActivityInfo teamActivityInfo && teamActivityInfo.activityTeamInfo != null && teamActivityInfo.activityTeamInfo.teamId == _msg.getTeamId())
            {
                teamActivityInfo.addTeamMember(_msg.getMember());
            }
        }
        
        #endregion
       
        #region C2S
        
        /// <summary>
        /// 请求活动队伍列表
        /// </summary>
        public void reqActivityTeamList(long _instanceId, int _page, Action<GS2GC_012_001_RetActivityTeamList> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_001_ReqActivityTeamList(_instanceId, _page),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_001_RetActivityTeamList>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }
        
        /// <summary>
        /// 请求活动队伍信息
        /// </summary>
        public void reqActivityTeam(long _teamId, Action<GS2GC_012_002_RetActivityTeam> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_002_ReqActivityTeam(_teamId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_002_RetActivityTeam>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 请求玩家自身队伍数据
        /// </summary>
        public void reqSelfActivityTeam(long _instanceId, Action<GS2GC_012_003_RetSelfActivityTeam> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_003_ReqSelfActivityTeam(_instanceId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_003_RetSelfActivityTeam>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 设置队伍申请条件
        /// </summary>
        public void reqSetActivityTeamApplyCond(long _teamId, CrossTeam_SetInfo_Join _applyCond, Action<GS2GC_012_004_RetSetActivityTeamApplyCond> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_004_ReqSetActivityTeamApplyCond(_teamId, _applyCond),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_004_RetSetActivityTeamApplyCond>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 创建活动队伍
        /// </summary>
        public void reqCreateActivityTeam(long _instanceId, ENPCrossTeamJoinType _joinType, CrossTeam_SetInfo_Join _applyCond, string _teamName, string _teamDec, Action<GS2GC_012_005_RetCreateActivityTeam> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_005_ReqCreateActivityTeam(_instanceId, _joinType, _applyCond, _teamName, _teamDec),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_005_RetCreateActivityTeam>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }
        
        /// <summary>
        /// 请求加入活动队伍
        /// </summary>
        public void reqJoinActivityTeam(long _teamId, Action<GS2GC_012_006_RetJoinActivityTeam> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_006_ReqJoinActivityTeam(_teamId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_006_RetJoinActivityTeam>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }
        
        /// <summary>
        /// 请求踢出活动队伍成员
        /// </summary>
        public void reqKickActivityTeamPlayer(long _teamId, long _kickCid, Action<GS2GC_012_007_RetKickActivityTeamPlayer> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_007_ReqKickActivityTeamPlayer(_teamId, _kickCid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_007_RetKickActivityTeamPlayer>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }
        
        /// <summary>
        /// 请求解散活动队伍
        /// </summary>
        public void reqDissolveActivityTeam(long _teamId, Action<GS2GC_012_008_RetDissolveActivityTeam> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_008_ReqDissolveActivityTeam(_teamId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_008_RetDissolveActivityTeam>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }
        
        /// <summary>
        /// 请求退出活动队伍
        /// </summary>
        public void reqQuitActivityTeam(long _teamId, Action<GS2GC_012_009_RetQuitActivityTeam> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_009_ReqQuitActivityTeam(_teamId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_009_RetQuitActivityTeam>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 请求申请加入活动队伍
        /// </summary>
        /// <param name="_teamId"></param>
        /// <param name="_callback"></param>
        public void reqApplyJoinActivityTeam(long _teamId, Action<GS2GC_012_010_RetApplyJoinActivityTeam> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_010_ReqApplyJoinActivityTeam(_teamId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_010_RetApplyJoinActivityTeam>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 请求队伍申请列表
        /// </summary>
        /// <param name="_teamId"></param>
        /// <param name="_callback"></param>
        public void reqActivityTeamApplyList(long _teamId, Action<GS2GC_012_011_RetActivityTeamApplyList> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_011_ReqActivityTeamApplyList(_teamId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_011_RetActivityTeamApplyList>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 请求玩家发起的队伍申请列表
        /// </summary>
        /// <param name="_instanceId"></param>
        /// <param name="_callback"></param>
        public void reqSelfActivityTeamApplyList(long _instanceId, Action<GS2GC_012_012_RetSelfActivityTeamApplyList> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_012_ReqSelfActivityTeamApplyList(_instanceId),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_012_RetSelfActivityTeamApplyList>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 同意玩家组队申请
        /// </summary>
        /// <param name="_teamId"></param>
        /// <param name="_applyCid"></param>
        /// <param name="_callback"></param>
        public void reqAgreeActivityTeamApply(long _teamId, long _applyCid, Action<GS2GC_012_013_RetAgreeActivityTeamApply> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_013_ReqAgreeActivityTeamApply(_teamId, _applyCid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_013_RetAgreeActivityTeamApply>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 拒绝玩家组队申请
        /// </summary>
        /// <param name="_teamId"></param>
        /// <param name="_applyCid"></param>
        /// <param name="_callback"></param>
        public void reqRefuseActivityTeamApply(long _teamId, long _applyCid, Action<GS2GC_012_014_RetRefuseActivityTeamApply> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_014_ReqRefuseActivityTeamApply(_teamId, _applyCid),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_014_RetRefuseActivityTeamApply>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        /// <summary>
        /// 修改队伍设置
        /// </summary>
        /// <param name="_teamId"></param>
        /// <param name="_teamName"></param>
        /// <param name="_teamDec"></param>
        /// <param name="_joinType"></param>
        /// <param name="_applyCond"></param>
        /// <param name="_callback"></param>
        public void reqSetActivityTeamSetting(long _teamId, string _teamName, string _teamDec, ENPCrossTeamJoinType _joinType, CrossTeam_SetInfo_Join _applyCond, Action<GS2GC_012_015_RetSetActivityTeamSetting> _callback)
        {
            NPGSClientListener.sendRequestByLog(new GC2GS_012_015_ReqSetActivityTeamSetting(_teamId, _teamName, _teamDec, _joinType, _applyCond),
                new CommonErrCodeRequestCallbackProtocolDealer<GS2GC_012_015_RetSetActivityTeamSetting>((_msg) =>
                {
                    if (null != _callback)
                        _callback(_msg);
                }));
        }

        #endregion
    }
}