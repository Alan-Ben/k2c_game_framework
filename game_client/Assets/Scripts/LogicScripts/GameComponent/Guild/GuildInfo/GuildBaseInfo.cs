using System;
using System.Collections.Generic;
using ALPackage;
using Common.GuildEnum;
using Common.GuildObj;
using JetBrains.Annotations;
using NPCommon;

namespace GOE
{
    /// <summary>
    /// 联盟信息
    /// </summary>
    public class GuildBaseInfo
    {
        //联盟展示信息
        protected Guild_ShowInfo _m_guildShowInfo;
        protected GuildLevelRefObj _m_guildLevelRefObj;//联盟等级配表数据
        
        //成员列表
        [NotNull] protected List<GuildMemberInfo> _m_lMemberList;
        //加入限制信息列表
        protected List<_AGuildJoinLimitInfo> _m_lJoinLimitInfoList;

        //缓存的盟主信息
        private PlayerInfo_IconShow _m_leaderInfo;
        //盟主信息请求序列号
        private long _m_lLeaderReqSerialize;

        /// <summary>
        /// 联盟id
        /// </summary>
        public long guildId { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getGuidId() : 0; } }
        /// <summary>
        /// 旗帜id
        /// </summary>
        public long flagId { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getFlagId() : 0; } }
        /// <summary>
        /// 联盟名称
        /// </summary>
        public string name { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getName() : string.Empty; } }
        /// <summary>
        /// 联盟简称
        /// </summary>
        public string simpleName { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getSimpleName() : string.Empty; } }
        /// <summary>
        /// 联盟宣言
        /// </summary>
        public string declaration { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getDeclaration() : string.Empty; } }
        /// <summary>
        /// 盟主cid
        /// </summary>
        public long leaderId { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getLeaderId() : 0; } }
        /// <summary>
        /// 联盟等级
        /// </summary>
        public long level { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getLevel() : 0; } }
        /// <summary>
        /// 联盟等级配表数据
        /// </summary>
        public GuildLevelRefObj guildLevelRefObj { get { return _m_guildLevelRefObj; } }
        /// <summary>
        /// 联盟经验
        /// </summary>
        public long exp { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getExp() : 0; } }
        /// <summary>
        /// 成员数量
        /// </summary>
        public int memberCount { get { return _m_lMemberList != null ? _m_lMemberList.Count : 0; } }
        /// <summary>
        /// 联盟总收益
        /// </summary>
        public long totalEarnings { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getTotalEarnings() : 0; } }
        /// <summary>
        /// 加入类型
        /// </summary>
        public EGuildJoinType joinType { get { return _m_guildShowInfo != null ? _m_guildShowInfo.getJoinType() : EGuildJoinType.NONE; } }
        /// <summary>
        /// 成员列表
        /// </summary>
        public List<GuildMemberInfo> memberList { get { return _m_lMemberList; } }
        /// <summary>
        /// 加入限制信息列表
        /// </summary>
        public List<_AGuildJoinLimitInfo> joinLimitInfoList { get { return _m_lJoinLimitInfoList; } }
        /// <summary>
        /// 缓存的盟主信息（需要先请求数据）
        /// </summary>
        public PlayerInfo_IconShow leaderInfo { get { return _m_leaderInfo; }}

        public GuildBaseInfo(Guild_ShowInfo _showInfo, List<Guild_MemberBaseInfo> _memberBaseInfoList)
        {
            _m_lMemberList = new List<GuildMemberInfo>();

            updateShowInfo(_showInfo);
            updateMemberBaseInfoList(_memberBaseInfoList);
        }

        /// <summary>
        /// 更新展示信息
        /// </summary>
        /// <param name="_showInfo"></param>
        public void updateShowInfo(Guild_ShowInfo _showInfo)
        {
            if (_showInfo == null)
                return;

            _m_guildShowInfo = _showInfo;
            _m_guildLevelRefObj = GRefdataCoreMgr.instance.guildLevelRefCore.getRef(level);
            
            _updateJoinLimitInfoList(_showInfo.getJoinLimitInfo());
        }

        /// <summary>
        /// 更新成员信息列表基础信息
        /// </summary>
        /// <param name="_memberBaseInfoList"></param>
        public void updateMemberBaseInfoList(List<Guild_MemberBaseInfo> _memberBaseInfoList)
        {
            _m_lMemberList.Clear();
            
            if (_memberBaseInfoList != null)
            {
                for (int i = 0; i < _memberBaseInfoList.Count; i++)
                {
                    GuildMemberInfo memberInfo = new GuildMemberInfo(_memberBaseInfoList[i]);
                    _m_lMemberList.Add(memberInfo);
                }
            }
        }

        /// <summary>
        /// 获取排序后的成员列表(按照职位 -> 国力排序)
        /// </summary>
        public void getSortMemberList(Action<List<GuildMemberInfo>> _sortDone)
        {
            List<GuildMemberInfo> memberInfoList = new List<GuildMemberInfo>();
            Dictionary<GuildMemberInfo, NPCommonSimplePlayerInfo> memberDetailInfoDic = new Dictionary<GuildMemberInfo, NPCommonSimplePlayerInfo>();
            if (_m_lMemberList == null || _m_lMemberList.Count <= 0)
            {
                _sortDone?.Invoke(memberInfoList);
                return;
            }
            
            ALStepCounter stepCounter = new ALStepCounter();
            stepCounter.chgTotalStepCount(_m_lMemberList.Count);
            stepCounter.regAllDoneDelegate(() =>
            {
                memberInfoList.Sort((_member1, _member2) =>
                {
                    if (_member2 == null)
                        return -1;
                    if (_member1 == null)
                        return 1;

                    GuildPositionRefObj guildPositionRefObj1 = GRefdataCoreMgr.instance.guildPositionRefCore.getRef(_member1.positionId);
                    GuildPositionRefObj guildPositionRefObj2 = GRefdataCoreMgr.instance.guildPositionRefCore.getRef(_member2.positionId);
                    if (guildPositionRefObj2 == null)
                        return -1;
                    if (guildPositionRefObj1 == null)
                        return 1;

                    if (guildPositionRefObj1.type != guildPositionRefObj2.type)
                        return -guildPositionRefObj1.type.CompareTo(guildPositionRefObj2.type);//因为枚举是按照职位从小到大写的, 所以这里就直接按照枚举逆序排序
                    
                    memberDetailInfoDic.TryGetValue(_member1, out NPCommonSimplePlayerInfo detailInfo1);
                    memberDetailInfoDic.TryGetValue(_member2, out NPCommonSimplePlayerInfo detailInfo2);
                    
                    if (detailInfo2 == null)
                        return -1;
                    if (detailInfo1 == null)
                        return 1;

                    return -detailInfo1.totalPower.CompareTo(detailInfo2.totalPower);//国力从大到小排序
                });
                
                _sortDone?.Invoke(memberInfoList);
            });

            foreach (GuildMemberInfo memberInfo in _m_lMemberList)
            {
                if (memberInfo == null)
                {
                    stepCounter.addDoneStepCount();
                    continue;
                }
                
                memberInfo.getPlayerDetailInfo((_detailInfo) =>
                {
                    memberInfoList.Add(memberInfo);
                    memberDetailInfoDic.Add(memberInfo, _detailInfo);
                    stepCounter.addDoneStepCount();
                });
            }
        }

        /// <summary>
        /// 更新限制条件列表
        /// </summary>
        /// <param name="_joinLimitInfoList"></param>
        private void _updateJoinLimitInfoList(List<Guild_JoinLimitInfo> _joinLimitInfoList)
        {
            if (_m_lJoinLimitInfoList == null)
                _m_lJoinLimitInfoList = new List<_AGuildJoinLimitInfo>();
            _m_lJoinLimitInfoList.Clear();
            
            if (_joinLimitInfoList != null)
            {
                _AGuildJoinLimitInfo limitInfo = null;
                _joinLimitInfoList.ForEach((serverLimitInfo) =>
                {
                    limitInfo = GuildLimitInfoFactory.getLimitInfo(serverLimitInfo);
                    if(limitInfo != null)
                        _m_lJoinLimitInfoList.Add(limitInfo);
                });
            }
        }

        /// <summary>
        /// 获取玩家自身不满足的加入限制条件
        /// </summary>
        /// <returns></returns>
        public List<_AGuildJoinLimitInfo> getSelfNotConformJoinLimitingConditions()
        {
            if (_m_lJoinLimitInfoList == null)
                return null;

            List<_AGuildJoinLimitInfo> notConformLimitInfoList = new List<_AGuildJoinLimitInfo>();
            _AGuildJoinLimitInfo limitInfo = null;
            for (int i = 0; i < _m_lJoinLimitInfoList.Count; i++)
            {
                limitInfo = _m_lJoinLimitInfoList[i];
                if (limitInfo != null && !limitInfo.selfMeetLimitingCondition())
                {
                    notConformLimitInfoList.Add(limitInfo);
                }
            }

            return notConformLimitInfoList;
        }

        /// <summary>
        /// 根据类型获取限制条件
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public _AGuildJoinLimitInfo getJoinLimitInfoByType(EGuildJoinLimitType _type)
        {
            if (_m_lJoinLimitInfoList == null)
                return null;

            for (int i = 0; i < _m_lJoinLimitInfoList.Count; i++)
            {
                if (_m_lJoinLimitInfoList[i].type == _type)
                    return _m_lJoinLimitInfoList[i];
            }

            return null;
        }

        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <returns></returns>
        public GuildMemberInfo getMemberInfo(long _cid)
        {
            foreach (GuildMemberInfo memberInfo in _m_lMemberList)
            {
                if(memberInfo != null && memberInfo.cid == _cid)
                    return memberInfo;
            }

            return null;
        }

        /// <summary>
        /// 获取盟主信息
        /// </summary>
        /// <param name="_onDone"></param>
        public void getLeaderInfo(Action<PlayerInfo_IconShow> _onDone)
        {
            if (leaderId <= 0)
            {
                _onDone?.Invoke(null);
                return;
            }

            _m_lLeaderReqSerialize = ALSerializeOpMgr.next();
            long curSerialize = _m_lLeaderReqSerialize;

            NPPlayer.instance.rankCommonComp.reqPlayerBriefInfo(leaderId, (_msg) =>
            {
                if (curSerialize != _m_lLeaderReqSerialize)
                    return;

                _m_leaderInfo = _msg;
                _onDone?.Invoke(_msg);
            });
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        public void clear()
        {
            _m_guildShowInfo = null;
            _m_guildLevelRefObj = null;
            
            _m_lMemberList.Clear();
            _m_lJoinLimitInfoList?.Clear();
        }
    }
}