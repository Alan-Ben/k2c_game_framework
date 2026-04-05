using System;
using System.Collections.Generic;
using Common.GuildEnum;
using Common.GuildObj;
using CommonEnum;
using GS2GC.p002_InitOp;
using GS2GC.p032_GuildOp;
using JetBrains.Annotations;
using NPEnum;

namespace GOE
{
    /// <summary>
    /// 联盟信息
    /// </summary>
    public class GuildInfo : GuildBaseInfo
    {
        //联盟财富
        private long _m_lGuildWealth;
        //公告
        private string _m_sAnnouncement;
        //入盟请求列表
        private List<GuildJoinRequestInfo> _m_lJoinRequestList;
        //联盟建设信息(不要直接使用, 因为可能存在过期, 要获取联盟建设信息使用guildConstructList)
        private GuildConstructList _m_guildConstructList; 
        //联盟事件列表
        private List<Guild_EventInfo> _m_lGuildEventList;
        //下次可招募时间
        private long _m_lNextCanRecruitTimeMs;
        /// <summary>
        /// 联盟委托信息
        /// </summary>
        private GuildEntrustInfo _m_iGuildEntrustInfo;
        /// <summary>
        /// 联盟派遣信息
        /// </summary>
        [NotNull] private List<GuildDispatchInfo> _m_lDispatchInfoList = new List<GuildDispatchInfo>();
        
        /// <summary>
        /// 联盟财富
        /// </summary>
        public long guildWealth { get { return _m_lGuildWealth; } }
        /// <summary>
        /// 联盟公告
        /// </summary>
        public string announcement { get { return _m_sAnnouncement; } }
        /// <summary>
        /// 入盟请求列表
        /// </summary>
        public List<GuildJoinRequestInfo> joinRequestList { get { return _m_lJoinRequestList; } }
        /// <summary>
        /// 联盟建设信息
        /// </summary>
        public GuildConstructList guildConstructList
        {
            get
            {
                // 若跨天了, 联盟建设数据还未更新的话, 清除联盟建设数据
                if (_m_guildConstructList != null && _m_guildConstructList.date != NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.LAST_LOGIN_DATE)) //找服务端确认过LAST_LOGIN_DATE跨天会更新
                    _m_guildConstructList.resetData();

                return _m_guildConstructList;
            }
        }
        /// <summary>
        /// 联盟事件列表
        /// </summary>
        public List<Guild_EventInfo> guildEventList { get { return _m_lGuildEventList; } }
        /// <summary>
        /// 下次可招募时间
        /// </summary>
        public long nextCanRecruitTimeMs { get { return _m_lNextCanRecruitTimeMs; } }
        /// <summary>
        /// 联盟委托信息
        /// </summary>
        public GuildEntrustInfo guildEntrustInfo { get { return _m_iGuildEntrustInfo; } }
        [NotNull] public List<GuildDispatchInfo> dispatchInfoList { get { return _m_lDispatchInfoList; } }
     

        public GuildInfo(GS2GC_002_063_RetGuildInit _msg) : base(_msg?.getGuildInfo()?.getGuildInfo(), _msg?.getGuildInfo()?.getMemberList())
        {
            if (_msg == null || _msg.getGuildInfo() == null)
                return;

            updateWealth(_msg.getGuildInfo().getGuildWealth());
            updateAnnouncement(_msg.getGuildInfo().getAnnouncement());
            updateJoinRequestList(_msg.getGuildInfo().getJoinRequestList());
            _updateEventList(_msg.getGuildInfo().getEventList());
            updateNextCanRecruitTimeMs(_msg.getGuildInfo().getNextCanRecruitTimeMs());
            updateGuildEntrustInfo(_msg.getGuildInfo().getEntrustInfo());
            _initDispatchInfoList(_msg.getGuildInfo().getDispatchData());
            updateConstructInfo(_msg.getGuildInfo().getConstructList());
        }

        public GuildInfo(GS2GC_032_065_OnJoinGuild _msg) : base(_msg?.getGuildInfo()?.getGuildInfo(), _msg?.getGuildInfo()?.getMemberList())
        {
            if (_msg == null || _msg.getGuildInfo() == null)
                return;

            updateWealth(_msg.getGuildInfo().getGuildWealth());
            updateAnnouncement(_msg.getGuildInfo().getAnnouncement());
            updateJoinRequestList(_msg.getGuildInfo().getJoinRequestList());
            _updateEventList(_msg.getGuildInfo().getEventList());
            updateNextCanRecruitTimeMs(_msg.getGuildInfo().getNextCanRecruitTimeMs());
            updateGuildEntrustInfo(_msg.getGuildInfo().getEntrustInfo());
            _initDispatchInfoList(_msg.getGuildInfo().getDispatchData());
            updateConstructInfo(_msg.getGuildInfo().getConstructList());
        }

        /// <summary>
        /// 当天标记变更
        /// </summary>
        public void onDayTagChg()
        {
            // 获取当前日期tag
            int curDayTag = (int)NPPlayer.instance.playerInfo.getValue(ENPPlayerParam.LAST_LOGIN_DATE);
            
            // 若跨天了, 联盟建设数据还未更新的话, 重置联盟建设数据
            if (_m_guildConstructList != null && _m_guildConstructList.date != curDayTag)
                _m_guildConstructList.resetData();
        }
        
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateWealth(long _guildWealth)
        {
            _m_lGuildWealth = _guildWealth;
        }

        /// <summary>
        /// 更新公告
        /// </summary>
        /// <param name="_announcement"></param>
        public void updateAnnouncement(string _announcement)
        {
            _m_sAnnouncement = _announcement;
        }

        /// <summary>
        /// 更新入盟请求列表
        /// </summary>
        /// <param name="_joinRequestLsit"></param>
        public void updateJoinRequestList(List<Guild_JoinRequestInfo> _joinRequestLsit)
        {
            if (_joinRequestLsit == null)
                return;

            if (_m_lJoinRequestList == null)
                _m_lJoinRequestList = new List<GuildJoinRequestInfo>();
            _m_lJoinRequestList.Clear();

            for (int i = 0; i < _joinRequestLsit.Count; i++)
            {
                _m_lJoinRequestList.Add(new GuildJoinRequestInfo(_joinRequestLsit[i]));
            }
        }

        /// <summary>
        /// 更新联盟委托信息
        /// </summary>
        /// <param name="_serverEntrustInfo"></param>
        public void updateGuildEntrustInfo(Guild_EntrustInfo _serverEntrustInfo)
        {
            if (_serverEntrustInfo == null)
                return;

            if (_m_iGuildEntrustInfo == null)
                _m_iGuildEntrustInfo = new GuildEntrustInfo(_serverEntrustInfo);
            else
                _m_iGuildEntrustInfo.updateInfo(_serverEntrustInfo);
        }
        
        /// <summary>
        /// 更新联盟建设信息
        /// </summary>
        /// <param name="_info"></param>
        public void updateConstructInfo(Guild_ConstructList _info)
        {
            if (_info == null)
                return;

            if (_m_guildConstructList == null)
                _m_guildConstructList = new GuildConstructList(_info);
            else
                _m_guildConstructList.updateInfo(_info);
        }

        /// <summary>
        /// 更新下次可招募时间
        /// </summary>
        /// <param name="_timeMs"></param>
        public void updateNextCanRecruitTimeMs(long _timeMs)
        {
            _m_lNextCanRecruitTimeMs = _timeMs;
        }

        /// <summary>
        /// 更新事件列表
        /// </summary>
        /// <param name="_guildEventList"></param>
        private void _updateEventList(List<Guild_EventInfo> _guildEventList)
        {
            if (_guildEventList == null)
                return;

            if (_m_lGuildEventList == null)
                _m_lGuildEventList = new List<Guild_EventInfo>();
            _m_lGuildEventList?.Clear();

            for (int i = 0; i < _guildEventList.Count; i++)
            {
                addGuildEvent(_guildEventList[i]);
            }
        }

        /// <summary>
        /// 更新成员基础信息
        /// </summary>
        /// <param name="_memberBaseInfo"></param>
        public void updateMemberBaseInfo(Guild_MemberBaseInfo _memberBaseInfo)
        {
            if (_memberBaseInfo == null)
                return;

            for (int i = 0; i < _m_lMemberList.Count; i++)
            {
                if (_m_lMemberList[i] != null && _m_lMemberList[i].cid == _memberBaseInfo.getCid())
                {
                    _m_lMemberList[i].updateBaseInfo(_memberBaseInfo);
                    break;
                }
            }
        }

        /// <summary>
        /// 更新联盟总国力
        /// </summary>
        /// <param name="_nationPower"></param>
        public void updateTotalEarnings(long _nationPower)
        {
            _m_guildShowInfo?.setTotalEarnings(_nationPower);
        }

        /// <summary>
        /// 新增成员
        /// </summary>
        /// <param name="_memberBaseInfo"></param>
        public void addMember(Guild_MemberBaseInfo _memberBaseInfo)
        {
            if (_memberBaseInfo == null)
                return;

            GuildMemberInfo memberInfo = new GuildMemberInfo(_memberBaseInfo);
            _m_lMemberList.Add(memberInfo);
        }

        /// <summary>
        /// 移除成员
        /// </summary>
        /// <param name="_cid"></param>
        public void removeMember(long _cid)
        {
            for (int i = 0; i < _m_lMemberList.Count; i++)
            {
                if (_m_lMemberList[i] != null && _m_lMemberList[i].cid == _cid)
                {
                    _m_lMemberList.RemoveAt(i);
                    break;
                }
            }
        }

        /// <summary>
        /// 新增入盟申请
        /// </summary>
        /// <param name="_info"></param>
        public void addJoinRequest(Guild_JoinRequestInfo _info)
        {
            if (_m_lJoinRequestList == null)
                _m_lJoinRequestList = new List<GuildJoinRequestInfo>();

            _m_lJoinRequestList.Add(new GuildJoinRequestInfo(_info));
        }

        /// <summary>
        /// 移除入盟申请
        /// </summary>
        /// <param name="_dbIdList"></param>
        public void removeJoinRequest(List<long> _dbIdList)
        {
            if (_m_lJoinRequestList == null || _dbIdList == null)
                return;

            for (int i = 0; i < _dbIdList.Count; i++)
            {
                for (int j = 0; j < _m_lJoinRequestList.Count; j++)
                {
                    if (_m_lJoinRequestList[j] != null && _m_lJoinRequestList[j].dbId == _dbIdList[i])
                    {
                        _m_lJoinRequestList.RemoveAt(j);
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 获取自己的职位配置
        /// </summary>
        /// <returns></returns>
        public GuildPositionRefObj getSelfPositionRef()
        {
            for (int i = 0; i < _m_lMemberList.Count; i++)
            {
                if (_m_lMemberList[i] != null && _m_lMemberList[i].cid == NPPlayer.instance.playerInfo.CID)
                {
                    GuildPositionRefObj positionRef = GRefdataCoreMgr.instance.guildPositionRefCore.getRef(_m_lMemberList[i].positionId);
                    return positionRef;
                }
            }

            return null;
        }

        #region 联盟建设

        /// <summary>
        /// 根据类型获取今日建设数据
        /// </summary>
        /// <param name="_type"></param>
        /// <returns></returns>
        public Guild_ConstructInfo getTodayConstructInfo(EGuildConstructType _type)
        {
            return guildConstructList?.getTodayConstructInfo(_type);
        }

        /// <summary>
        /// 获取今日联盟建设经验和财富
        /// </summary>
        public void getTodayConstructExpAndWealth(out long _exp, out long _wealth)
        {
            _exp = 0;
            _wealth = 0;

            guildConstructList?.getTodayConstructExpAndWealth(out _exp, out _wealth);
        }
        
        #endregion
        
        /// <summary>
        /// 新增联盟事件
        /// </summary>
        /// <param name="_eventInfo"></param>
        public void addGuildEvent(Guild_EventInfo _eventInfo)
        {
            if (_eventInfo == null)
                return;

            if (_m_lGuildEventList == null)
                _m_lGuildEventList = new List<Guild_EventInfo>();

            _m_lGuildEventList.Add(_eventInfo);
        }

        /// <summary>
        /// 联盟事件变更
        /// </summary>
        /// <param name="_eventInfo"></param>
        public void chgGuildEvent(Guild_EventInfo _eventInfo)
        {
            if (_eventInfo == null)
                return;

            if (_m_lGuildEventList == null)
                _m_lGuildEventList = new List<Guild_EventInfo>();

            for (int i = 0; i < _m_lGuildEventList.Count; i++)
            {
                if (_m_lGuildEventList[i] != null && _m_lGuildEventList[i].getDbId() == _eventInfo.getDbId())
                {
                    _m_lGuildEventList[i] = _eventInfo;
                    break;
                }
            }
        }

        //移除联盟事件
        public void removeGuildEvent(long _dbId)
        {
            if (_m_lGuildEventList == null)
                return;

            for (int i = 0; i < _m_lGuildEventList.Count; i++)
            {
                if (_m_lGuildEventList[i] != null && _m_lGuildEventList[i].getDbId() == _dbId)
                {
                    _m_lGuildEventList.RemoveAt(i);
                    break;
                }
            }
        }

        #region 联盟派遣

        /// <summary>
        /// 初始化派遣信息
        /// </summary>
        private void _initDispatchInfoList()
        {
            _m_lDispatchInfoList.Clear();
            
            // 遍历ESpecAttrType所有枚举
            foreach (ESpecAttrType attrType in Enum.GetValues(typeof(ESpecAttrType)))
            {
                _m_lDispatchInfoList.Add(new GuildDispatchInfo(attrType));
            }
        }
        
        /// <summary>
        /// 初始化派遣信息
        /// </summary>
        private void _initDispatchInfoList(Guild_DispatchData _serverDispatchData)
        {
            _initDispatchInfoList();

            updateDispatchInfo(_serverDispatchData);
        }

        /// <summary>
        /// 根据相性获取派遣信息
        /// </summary>
        /// <param name="_specAttrType"></param>
        /// <returns></returns>
        public GuildDispatchInfo getDispatchHeroInfo(ESpecAttrType _specAttrType)
        {
            foreach (var dispatchInfo in _m_lDispatchInfoList)
            {
                if (dispatchInfo != null && dispatchInfo.specAttrType == _specAttrType)
                    return dispatchInfo;
            }

            return null;
        }
        
        /// <summary>
        /// 获取某一相性加成总万分比
        /// </summary>
        /// <returns></returns>
        public int getDispatchTotalAddPer(ESpecAttrType _specAttrType)
        {
            GuildDispatchInfo dispatchInfo = getDispatchHeroInfo(_specAttrType);
            return dispatchInfo?.totalAddPer ?? 0;
        }
        
        /// <summary>
        /// 更新派遣信息
        /// </summary>
        /// <param name="_serverAttrDispatchInfo"></param>
        public void updateDispatchInfo(Guild_AttrDispatchInfo _serverAttrDispatchInfo)
        {
            if (_serverAttrDispatchInfo == null)
                return;

            GuildDispatchInfo dispatchInfo = getDispatchHeroInfo(_serverAttrDispatchInfo.getAttr());
            if (dispatchInfo == null)
            {
                dispatchInfo = new GuildDispatchInfo(_serverAttrDispatchInfo.getAttr());
                _m_lDispatchInfoList.Add(dispatchInfo);
            }
        
            dispatchInfo.updateInfo(_serverAttrDispatchInfo.getHeroList());
        }

        /// <summary>
        /// 更新派遣信息
        /// </summary>
        /// <param name="_serverDispatchData"></param>
        public void updateDispatchInfo(Guild_DispatchData _serverDispatchData)
        {
            if (_serverDispatchData == null || _serverDispatchData.getDispatchList() == null)
                return;

            foreach (var serverDispatchInfo in _serverDispatchData.getDispatchList())
            {
                if(serverDispatchInfo != null)
                    updateDispatchInfo(serverDispatchInfo);
            }
        }

        /// <summary>
        /// 获取派遣大臣id
        /// </summary>
        /// <returns></returns>
        public long getDispatchHeroId(long _cid)
        {
            foreach (var dispatchInfo in _m_lDispatchInfoList)
            {
                if(dispatchInfo == null)
                    continue;

                if (dispatchInfo.dispatchHeroInfoList != null)
                {
                    foreach (var dispatchHeroInfo in dispatchInfo.dispatchHeroInfoList)
                    {
                        if (dispatchHeroInfo != null && dispatchHeroInfo.getCid() == _cid)
                            return dispatchHeroInfo.getHeroId();
                    }
                }
            }

            return 0;
        }
        
        /// <summary>
        /// 计算总的派遣收益
        /// </summary>
        /// <returns></returns>
        public long calTotalDispatchEarnings()
        {
            long totalEarnings = 0;
            
            NPPlayer.instance.buildingComp.dealAllBusinessBuildingList((_buildingInfo) =>
            {
                if(_buildingInfo == null)
                    return;

                long baseEarnings = _buildingInfo.getTotalBaseEarningsPerS();
                int addPer = getDispatchTotalAddPer(_buildingInfo.baseRef.attr_type);

                long earnings = (baseEarnings * 1f * addPer / 10000f).ToCeilingLongValue();
                totalEarnings += earnings;
            });

            return totalEarnings;
        }
        
        #endregion
    }
}