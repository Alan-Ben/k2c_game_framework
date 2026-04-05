package NPUSServer.Guild;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALBasicMutex.MutexInstance;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import AllRpcData.US_Service.Guild.GuildRequestLoadRankData;
import Common.GuildEnum.EGuildJoinType;
import Common.GuildEnum.EGuildPositionType;
import Common.RankObj.Rank_BaseItem;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_002_UserJoinInfo;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Guild.RefGuildEntrustQuality;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.GeneralV.UsID;
import NPUSServer.Guild.EntrustWeight.GuildEntrustWeightList;
import NPUSServer.Guild.GuildDungeon.GuildDungeonInstanceInfo;
import NPUSServer.Guild.JoinRequest.GuildJoinRequestMgr;
import NPUSServer.Guild.Msg.GuildLogFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.RankFixedMgr.RankFixedInfo;
import NPUSServer.USLog;
import USDB.Bo.*;
import USLOGDB.Bo.LogGuildMemberChgBO;

import java.util.*;

public class GuildMgr
{
    private NPUserServer _m_server;

    //工会列表
    private List<GuildInfo> _m_guildList;
    //公会信息
    private Map<Long, GuildInfo> _m_guildMap;
    //锁
    private MutexAtom _m_mutex;
    //联盟成员变动操作锁(由于涉及到本服成员变动，所以这边用一个大锁统一处理)
    private MutexInstance _m_memberMutex;

    //入盟申请管理器
    private GuildJoinRequestMgr _m_joinRequestMgr;
    //名字检查
    private GuildNameChecker _m_nameChecker;
    
    //公会副本下次结算时间
    private long _m_lGuildDungeonNextSettleMs;

    public GuildMgr(NPUserServer _server)
    {
        _m_server = _server;

        _m_guildList = new ArrayList<>();
        _m_guildMap = new HashMap<>();

        _m_mutex = new MutexAtom();
        _m_memberMutex = new MutexInstance();

        _m_joinRequestMgr = new GuildJoinRequestMgr(this);
        _m_nameChecker = new GuildNameChecker();
    }

    public NPUserServer getServer()
    {
        return _m_server;
    }

    public GuildNameChecker getNameChecker()
    {
        return _m_nameChecker;
    }

    public GuildJoinRequestMgr getJoinRequestMgr()
    {
        return _m_joinRequestMgr;
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    public MutexInstance getMemberMutex()
    {
        return _m_memberMutex;
    }
    
    /**
     * 公会副本结算时间数据处理
     * @return
     */
    public long getGuildDungeonNextSettleMs() {return _m_lGuildDungeonNextSettleMs;}
    public void setGuildDungeonNextSettleMs(long _ms) {_m_lGuildDungeonNextSettleMs = _ms;}

    /**
     * 初始化
     * @return
     */
    public boolean init()
    {
        //加载工会数据
        List<GuildBO> guildBOList = _m_server.getBM().getBM(GuildBO.class).s_findAll();
        for (GuildBO guildBO : guildBOList)
        {
            GuildInfo guildInfo = new GuildInfo(guildBO, this);
            _onGuildAdd(guildInfo);
        }

        //加载工会成员数据
        List<GuildMemberBO> memberBOList = _m_server.getBM().getBM(GuildMemberBO.class).s_findAll();
        for (GuildMemberBO memberBO : memberBOList)
        {
            GuildInfo guildInfo = lookupGuild(memberBO.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getMemberMgr().initMember(memberBO);
            }
        }

        //加载工会申请数据
        List<GuildJoinRequestBO> requestBOList = _m_server.getBM().getBM(GuildJoinRequestBO.class).s_findAll();
        for (GuildJoinRequestBO requestBO : requestBOList)
        {
            _m_joinRequestMgr.initGuildJoinRequest(requestBO);
        }
        List<GuildPlayerRequestBO> playerRequestBOList = _m_server.getBM().getBM(GuildPlayerRequestBO.class).s_findAll();
        for (GuildPlayerRequestBO requestBO : playerRequestBOList)
        {
            _m_joinRequestMgr.initPlayerJoinRequest(requestBO);
        }

        //加载工会日志数据
        List<GuildLogBO> logBOList = _m_server.getBM().getBM(GuildLogBO.class).s_findAll();
        for (GuildLogBO logBO : logBOList)
        {
            GuildInfo guildInfo = lookupGuild(logBO.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getGuildLogMgr().initLog(logBO);
            }
        }

        //加载事件数据
        List<GuildEventBO> eventBOList = _m_server.getBM().getBM(GuildEventBO.class).s_findAll();
        for (GuildEventBO eventBO : eventBOList)
        {
            GuildInfo guildInfo = lookupGuild(eventBO.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getEventMgr().initEvent(eventBO);
            }
        }

        //加载大臣派遣数据
        List<GuildHeroDispatchBO> heroDispatchBoList = _m_server.getBM().getBM(GuildHeroDispatchBO.class).s_findAll();
        for (GuildHeroDispatchBO heroDispatchBo : heroDispatchBoList)
        {
            GuildInfo guildInfo = lookupGuild(heroDispatchBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getHeroDispatchMgr()._initBo(heroDispatchBo);
            }
        }

        //加载联盟副本全局配置数据
        List<GuildDungeonGlobalSetBO> dungeonGlobalSetBoList = _m_server.getBM().getBM(GuildDungeonGlobalSetBO.class).s_findAll();
        for (GuildDungeonGlobalSetBO dungeonGlobalSetBo : dungeonGlobalSetBoList)
        {
            GuildInfo guildInfo = lookupGuild(dungeonGlobalSetBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getDungeonMgr().getGlobalSetInfo()._loadBo(dungeonGlobalSetBo);
            }
        }

        //加载联盟副本配置数据
        List<GuildDungeonSetBO> dungeonSetBoList = _m_server.getBM().getBM(GuildDungeonSetBO.class).s_findAll();
        for (GuildDungeonSetBO dungeonSetBo : dungeonSetBoList)
        {
            GuildInfo guildInfo = lookupGuild(dungeonSetBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getDungeonMgr().getSetMgr()._loadBo(dungeonSetBo);
            }
        }

        //加载联盟副本伤害排行榜数据
        List<GuildDungeonDamageRankBO> dungeonDamageRankBoList = _m_server.getBM().getBM(GuildDungeonDamageRankBO.class).s_findAll();
        for (GuildDungeonDamageRankBO damageRankBo : dungeonDamageRankBoList)
        {
            GuildInfo guildInfo = lookupGuild(damageRankBo.getGuildId());
            if(null == guildInfo)
            	continue;
            
            guildInfo.getDungeonMgr().getDamageRank()._initBo(damageRankBo);
        }

        //加载联盟副本实例数据
        List<GuildDungeonInstanceBO> dungeonInstanceBoList = _m_server.getBM().getBM(GuildDungeonInstanceBO.class).s_findAll();
        for (GuildDungeonInstanceBO dungeonInstanceBo : dungeonInstanceBoList)
        {
            GuildInfo guildInfo = lookupGuild(dungeonInstanceBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getDungeonMgr().getInstanceMgr()._initBo(dungeonInstanceBo);
            }
        }

        //加载联盟副本怪物数据
        List<GuildDungeonMonsterBO> dungeonMonsterBoList = _m_server.getBM().getBM(GuildDungeonMonsterBO.class).s_findAll();
        for (GuildDungeonMonsterBO monsterBo : dungeonMonsterBoList)
        {
            GuildInfo guildInfo = lookupGuild(monsterBo.getGuildId());
            if(null == guildInfo)
            	continue;
            
            GuildDungeonInstanceInfo dungeonInfo = guildInfo.getDungeonMgr().getInstanceMgr().lookup(monsterBo.getInstaceId());
            if(null == dungeonInfo)
            	continue;
            
            dungeonInfo.getMonsterMgr()._initBo(monsterBo);
        }

        //加载联盟副本日志数据
        List<GuildDungeonLogBO> dungeonLogBoList = _m_server.getBM().getBM(GuildDungeonLogBO.class).s_findAll();
        for (GuildDungeonLogBO logBo : dungeonLogBoList)
        {
            GuildInfo guildInfo = lookupGuild(logBo.getGuildId());
            if(null == guildInfo)
            	continue;
            
            GuildDungeonInstanceInfo dungeonInfo = guildInfo.getDungeonMgr().getInstanceMgr().lookup(logBo.getInstaceId());
            if(null == dungeonInfo)
            	continue;
            
            dungeonInfo.getLogMgr()._initBo(logBo);
        }

        //加载联盟协作主数据
        List<GuildCooperateMainBO> cooperateMainBoList = _m_server.getBM().getBM(GuildCooperateMainBO.class).s_findAll();
        for (GuildCooperateMainBO cooperateMainBo : cooperateMainBoList)
        {
            GuildInfo guildInfo = lookupGuild(cooperateMainBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getCooperateInfo()._initBo(cooperateMainBo);
            }
        }

        //加载联盟协作据点信息数据
        List<GuildCooperatePointInfoBO> cooperatePointInfoBoList = _m_server.getBM().getBM(GuildCooperatePointInfoBO.class).s_findAll();
        for (GuildCooperatePointInfoBO cooperatePointInfoBo : cooperatePointInfoBoList)
        {
            GuildInfo guildInfo = lookupGuild(cooperatePointInfoBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getCooperateInfo()._initPointInfoBo(cooperatePointInfoBo);
            }
        }

        //加载联盟协作攻击日志数据
        List<GuildCooperateAttackLogBO> cooperateAttackLogBoList = _m_server.getBM().getBM(GuildCooperateAttackLogBO.class).s_findAll();
        for (GuildCooperateAttackLogBO cooperateAttackLogBo : cooperateAttackLogBoList)
        {
            GuildInfo guildInfo = lookupGuild(cooperateAttackLogBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getCooperateInfo()._initAttackLogBo(cooperateAttackLogBo);
            }
        }

        //加载联盟协作攻击日志数据
        List<GuildCooperateDamageRankBO> cooperateDamageBoList = _m_server.getBM().getBM(GuildCooperateDamageRankBO.class).s_findAll();
        for (GuildCooperateDamageRankBO cooperateDamageBo : cooperateDamageBoList)
        {
            GuildInfo guildInfo = lookupGuild(cooperateDamageBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getCooperateInfo()._initDamageRankBo(cooperateDamageBo);
            }
        }

        //加载联盟集结主数据（初始化阶段，单线程，先主表后成员表）
        List<GuildRallyMainBO> rallyMainBoList = _m_server.getBM().getBM(GuildRallyMainBO.class).s_findAll();
        for (GuildRallyMainBO rallyMainBo : rallyMainBoList)
        {
            //按guild_id定位所属联盟，再将主数据交给该联盟的RallyMgr
            GuildInfo guildInfo = lookupGuild(rallyMainBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getRallyMgr().s_initRallyMainFromDB(rallyMainBo);
            }
        }

        //加载联盟集结成员数据（依赖主数据已初始化）
        List<GuildRallyMemberBO> rallyMemberBoList = _m_server.getBM().getBM(GuildRallyMemberBO.class).s_findAll();
        for (GuildRallyMemberBO rallyMemberBo : rallyMemberBoList)
        {
            //按guild_id分发到对应联盟，再按rally_id注入成员
            GuildInfo guildInfo = lookupGuild(rallyMemberBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getRallyMgr().s_initRallyMemberFromDB(rallyMemberBo);
            }
        }

        //加载工会火星求助数据
        List<GuildMarsHelpBO> marsHelpBOList = _m_server.getBM().getBM(GuildMarsHelpBO.class).s_findAll();
        for (GuildMarsHelpBO marsHelpBO : marsHelpBOList)
        {
            GuildInfo guildInfo = lookupGuild(marsHelpBO.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getMarsHelpMgr()._initFromMarsHelpBo(marsHelpBO);
            }
        }
        
        //加载联盟宝箱
        List<GuildBoxBO> guildBoxBOList = _m_server.getBM().getBM(GuildBoxBO.class).s_findAll();
        for (GuildBoxBO guildBoxBo : guildBoxBOList)
        {
            GuildInfo guildInfo = lookupGuild(guildBoxBo.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getGuildBoxMgr()._initFromBo(guildBoxBo);
            }
        }

        //联盟火星矿分享管理器
        List<GuildMarsMineShareBO> guildMarsMineShareBOList = _m_server.getBM().getBM(GuildMarsMineShareBO.class).s_findAll();
        guildMarsMineShareBOList.sort(Comparator.comparingLong(GuildMarsMineShareBO::getId));
        for (GuildMarsMineShareBO guildMarsMineShareBO : guildMarsMineShareBOList)
        {
            GuildInfo guildInfo = lookupGuild(guildMarsMineShareBO.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getMarsMineShareMgr()._initFromBO(guildMarsMineShareBO);
            }
        }

        //联盟火星矿战报管理器
        List<GuildMarsMineBattleReportBO> guildMarsMineBattleReportBOList = _m_server.getBM().getBM(GuildMarsMineBattleReportBO.class).s_findAll();
        guildMarsMineBattleReportBOList.sort(Comparator.comparingLong(GuildMarsMineBattleReportBO::getId));
        for (GuildMarsMineBattleReportBO battleReportBO : guildMarsMineBattleReportBOList)
        {
            GuildInfo guildInfo = lookupGuild(battleReportBO.getGuildId());
            if (guildInfo != null)
            {
                guildInfo.getMarsMineBattleReportMgr()._initFromBO(battleReportBO);
            }
        }

        //加载总赚速
        _loadRankData();

        //初始化工会
        for (GuildInfo guildInfo : _m_guildList)
        {
            guildInfo.onInited();
        }

        //启动tick
        tick();

        return true;
    }

    /**
     * 加载联盟国力数据
     */
    private void _loadRankData()
    {
        RankFixedInfo rankFixedInfo = getServer().getRankFixedMgr().lookupRank(RefGeneral.Ref().guild_rank_fixed_id);
        if (rankFixedInfo == null)
            return;

        //同步联盟国力数据到联盟上
        rankFixedInfo.makeRankBaseList(false, 0, (_result, _data) ->
        {
            if (!_result.isSucc())
                return;

            for (Rank_BaseItem item : _data)
            {
                GuildInfo guildInfo = lookupGuild(item.getKey());
                if (guildInfo != null)
                    guildInfo.onTotalEarningsChg(item.getScore());
            }
        });
    }
    
    public ArrayList<GuildInfo> getGuildList()
    {
    	_lock();
    	
    	try
    	{
    		return new ArrayList<>(_m_guildList);
    	}
    	finally
    	{
    		_unlock();
    	}
    }

    /**
     * 查询工会
     * @param _guildId
     */
    public GuildInfo lookupGuild(long _guildId)
    {
        _lock();
        try
        {
            return _m_guildMap.get(_guildId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 创建工会
     */
    public ResultOne<GuildInfo> createGuild(long _flagId, String _name, String _simpleName, String _declaration, boolean _isFreeJoin)
    {
        //随机委托事件
        GuildEntrustWeightList entrustWeightList = new GuildEntrustWeightList();
        RefGuildEntrustQuality refGuildEntrustQuality = entrustWeightList.randomRef();
        if (refGuildEntrustQuality == null)
            USLog.error(_m_server, "GuildMgr createGuild failed, random refGuildEntrustQuality failed.");

        //构造数据对象
        GuildBO guildBO = new GuildBO();
        guildBO.setGuildId(_m_server.getBM(), UsID.makeGuildId(_m_server));
        guildBO.setName(_m_server.getBM(), _name);
        guildBO.setSimpleName(_m_server.getBM(), _simpleName);
        guildBO.setFlagId(_m_server.getBM(), _flagId);
        guildBO.setDeclaration(_m_server.getBM(), _declaration);
        guildBO.setJoinType(_m_server.getBM(), _isFreeJoin ? EGuildJoinType.FREE_JOIN.ordinal() : EGuildJoinType.APPROVAL_JOIN.ordinal());
        guildBO.setLevel(_m_server.getBM(), 1);
        guildBO.setActivePointTargetLvl(_m_server.getBM(), 1);
        //委托相关信息
        if (refGuildEntrustQuality != null)
        {
            guildBO.setEntrustRefId(_m_server.getBM(), refGuildEntrustQuality.Id());
            Long eventId = CommonFunc.randSelect(refGuildEntrustQuality.guild_random_requests_id_list);
            guildBO.setEntrustEventId(_m_server.getBM(), eventId == null ? 0 : eventId);
            guildBO.setEntrustWeightBaseList(_m_server.getBM(), entrustWeightList.toString());
        }
        guildBO.insert(_m_server.getBM());

        //构造工会信息
        GuildInfo guildInfo = new GuildInfo(guildBO, this);

        _onGuildAdd(guildInfo);

        return ResultOne.succ(guildInfo);
    }

    /**
     * 添加工会
     * @param _guildInfo
     */
    private void _onGuildAdd(GuildInfo _guildInfo)
    {
        _lock();
        try
        {
            //如果已经解散
            if (_guildInfo.isDissolve())
            {

            } else
            {
                _m_guildList.add(_guildInfo);
                _m_guildMap.put(_guildInfo.getGuildId(), _guildInfo);

                //名字加入
                getNameChecker().initName(_guildInfo.getName(), _guildInfo.getSimpleName());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 加入联盟
     */
    public Result joinGuild(long _guildId, long _cid, GuildOp_002_UserJoinInfo _userJoinInfo)
    {
        //查找对应联盟
        GuildInfo guildInfo = lookupGuild(_guildId);
        if (guildInfo == null)
            return GuildErr.GUILD_NOT_EXIST;

        //根据联盟的加入方式进行处理
        if (guildInfo.getJoinType() == EGuildJoinType.DECLINE_JOIN)
        {
            //如果拒绝加入直接返回
            return GuildErr.GUILD_NOT_ALLOW_TO_JOIN;
        } else
        {
            //检查是否可以满足要求
            boolean canFit = guildInfo.checkCanFitJoinLimit(_userJoinInfo);
            if (!canFit)
                return GuildErr.NOT_FIT_GUILD_JOIN_LIMIT;

            if (guildInfo.getJoinType() == EGuildJoinType.FREE_JOIN)
            {
                return joinGuild(guildInfo, _cid, EGuildPositionType.MEMBER, false, false);
            } else if (guildInfo.getJoinType() == EGuildJoinType.APPROVAL_JOIN)
            {
                if(_userJoinInfo.getCanRequest())
                    return _m_joinRequestMgr.addLocalGuildRequest(guildInfo, _cid);
                else
                    return GuildErr.JOIN_REQUEST_REACH_LIMIT;
            } else
            {
                return GuildErr.GUILD_NOT_ALLOW_TO_JOIN;
            }
        }
    }

    /**
     * 随机加入联盟
     * @param _userdata
     * @return
     */
    public Result randomJoinGuild(NPUSUserData _userdata)
    {
        //过滤可以自由加入的联盟
        List<GuildInfo> needCheckGuildList = new ArrayList<>();

        _lock();
        try
        {
            for (GuildInfo guildInfo : _m_guildList)
            {
                if (guildInfo.getJoinType() != EGuildJoinType.FREE_JOIN)
                    continue;

                //判断联盟是否满员
                if (guildInfo.getMemberMgr().getCanJoinNum() <= 0)
                    continue;

                needCheckGuildList.add(guildInfo);
            }
        } finally
        {
            _unlock();
        }

        //打乱顺序
        Collections.shuffle(needCheckGuildList);

        //遍历可以加入的联盟
        for (GuildInfo guildInfo : needCheckGuildList)
        {
            if (guildInfo.checkCanFitJoinLimit(_userdata.getGuildComponent().make_002_UserJoinInfo()))
            {
                return joinGuild(guildInfo, _userdata.getCid(), EGuildPositionType.MEMBER, false, false);
            }
        }

        return GuildErr.NO_GUILD_TO_JOIN;
    }

    /**
     * 用简称查找
     * @param _simpleName
     * @return
     */
    public GuildInfo lookupBySimpleName(String _simpleName)
    {
        _lock();
        try
        {
            for (GuildInfo guildInfo : _m_guildList)
            {
                if (guildInfo.getSimpleName().equals(_simpleName))
                {
                    return guildInfo;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 用名称查找
     * @param _name
     * @return
     */
    public GuildInfo lookupByName(String _name)
    {
        _lock();
        try
        {
            for (GuildInfo guildInfo : _m_guildList)
            {
                if (guildInfo.getName().equals(_name))
                {
                    return guildInfo;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 联盟的解散处理
     * @param _guildInfo
     */
    public void onGuildDissolve(GuildInfo _guildInfo)
    {
        _lock();
        try
        {
            _m_guildMap.remove(_guildInfo.getGuildId());
        } finally
        {
            _unlock();
        }

        //后续处理
        //联盟玩家处理
        _guildInfo.getMemberMgr().onGuildDissolve();
        //移除入盟申请
        getJoinRequestMgr().removeJoinRequestByGuildId(_guildInfo.getGuildId());
        //名字删除
        getNameChecker().removeRecord(_guildInfo.getName(), _guildInfo.getSimpleName());
    }

    /**
     * 加入操作
     * @param _guildInfo
     * @param _cid
     * @return
     */
    public Result joinGuild(GuildInfo _guildInfo, long _cid, EGuildPositionType _position, boolean _isCreateJoin, boolean _isRequestJoin)
    {
        Result result;
        _m_memberMutex.lock();
        try
        {
            result = _guildInfo.joinGuild(_cid, _position, _isCreateJoin, _isRequestJoin);
        } finally
        {
            _m_memberMutex.unlock();
        }

        int logType = _isCreateJoin ? 2 : 1;
        BM bmObj = getServer().getBM();
        LogGuildMemberChgBO logBo = new LogGuildMemberChgBO();
        logBo.setGuildId(bmObj, _guildInfo.getGuildId());
        logBo.setType(bmObj, logType);
        logBo.setCid(bmObj, _cid);
        CommLogDB.log(bmObj, logBo, null);

        //发送日志
        if (_isCreateJoin)
        {
            GuildLogFunc.sendGuildCreationLog(_cid, _guildInfo);
        } else
        {
            GuildLogFunc.sendMemberJoinLog(_cid, _guildInfo);
        }

        return result;
    }

    /**
     * 移除成员
     * @param _guildInfo
     * @param _cid
     * @return
     */
    public Result removeMember(GuildInfo _guildInfo, long _cid, boolean _isKick)
    {
        Result result;
        _m_memberMutex.lock();
        try
        {
            result = _guildInfo.removeMember(_cid, _isKick);
        } finally
        {
            _m_memberMutex.unlock();
        }

        int logType = _isKick ? 5 : 3;
        BM bmObj = getServer().getBM();
        LogGuildMemberChgBO logBo = new LogGuildMemberChgBO();
        logBo.setGuildId(bmObj, _guildInfo.getGuildId());
        logBo.setType(bmObj, logType);
        logBo.setCid(bmObj, _cid);
        CommLogDB.log(bmObj, logBo, null);

        return result;
    }

    /**
     * tick处理
     */
    public synchronized void tick()
    {
        long nowTimeMS = CommonFunc.getNowTimeMS();

        ArrayList<GuildInfo> guildList;
        _m_mutex.lock();
        try
        {
            guildList = new ArrayList<>(_m_guildList);
        } finally
        {
            _m_mutex.unlock();
        }

        for (GuildInfo guildInfo : guildList)
        {
            guildInfo.tick(nowTimeMS);
        }

        //5s后处理一次
        ALSynTaskManager.getInstance().regTask(this::tick, 5000);
    }

    /**
     * 联盟排行数据变更处理
     * @param _cid
     */
    public void onGuildRankScoreChg(long _cid)
    {
        NPUSUserData userData = getServer().getUsUserMgr().lookupCacheUserData(_cid);
        if(null == userData)
            return ;

        if(userData.getGuildComponent().getGuildId() <= 0)
            return ;

        GuildRequestLoadRankData rpc = new GuildRequestLoadRankData();
        rpc.req().setCid(_cid);
        rpc.req().setGuildId(userData.getGuildComponent().getGuildId());

        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(userData.getGuildComponent().getGuildId());

        getServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                , ()-> {
                    USLog.error(getServer(), "player:{} guild:{} send rpc requestLoadRandData fail.", _cid, userData.getGuildComponent().getGuildId());
                });
    }
}
