package NPUSServer.NPUSUserMgr.UserComp.GuildComp;

import AllRpcData.US_Service.Guild.*;
import Common.CachedObj.CachedObj_CachedGuildInfo;
import Common.Common_IntList;
import Common.GuildEnum.EGuildBoxType;
import Common.GuildObj.Guild_JoinCdInfo;
import Common.GuildObj.Guild_MemberDailyData;
import Common.MailObj.Mail_Data;
import CommonEnum.ESpecAttrType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_002_UserJoinInfo;
import GS2GC.p032_GuildOp.GS2GC_032_058_OnJoinGuildCdChg;
import GS2GC.p032_GuildOp.GS2GC_032_066_OnLeaveGuild;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.GuildErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerThree;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp.PlayerBuffInfo;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.USLog;
import RPC._ARpcCallBack;
import USDB.Bo.PlayerGuildBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public class GuildComponent extends _ANPUserComponent implements _IHandlerHolder {
    private PlayerGuildBO _m_bo;
    private Common_IntList _m_hadDrawConstructRewardList = new Common_IntList();

    private String _m_sGuildName;
    //存储在本地的公会简约名字
    private String _m_sGuildSimpleName;
    //公会等级
    private int _m_iGuildLvl;
    //不同属性的赚速加成
    private int[] _m_arrGuildBuildingAddPer = new int[ESpecAttrType.values().length];

    public GuildComponent(NPUSUserData _userData) {
        super(_userData, ENPPlayerCompType.GUILD);
    }

    @Override
    protected void _init() {
        getUSServer().getBM().getBM(PlayerGuildBO.class).findOne("cid", getUserData().getCid(), new _ASelectCallback<PlayerGuildBO>() {
            @Override
            public void dealFail() {
                if (getHasErr()) {
                    USLog.error(getUSServer(), "player:{} load travel bo fail.", getUserData().getCid());
                    getUserData().setDataLoadFail();
                    return;
                }

                PlayerGuildBO bo = new PlayerGuildBO();
                bo.setCid(getUSServer().getBM(), getUserData().getCid());
                bo.setFreeCdJoinGuildNum(getUSServer().getBM(), RefGeneral.Ref().guild_free_join_cd_num);
                bo.insert(getUSServer().getBM());

                _m_bo = bo;

                setInited();
            }

            @Override
            public void dealSuc(PlayerGuildBO _bo) {
                _m_bo = _bo;

                if (_m_bo.getDayHadDrawConstructRewardList() != null)
                    _m_hadDrawConstructRewardList.readPackage(ByteBuffer.wrap(_m_bo.getDayHadDrawConstructRewardList()));


                if(_m_bo.getGuildId() > 0)
                {
                    //如果公会数据存在需要发送RPC初始化
                    GuildMemberInitState rpc = new GuildMemberInitState();
                    rpc.req().setGuildId(_m_bo.getGuildId());
                    rpc.req().setCid(getCid());

                    //获取公会归属UsId
                    int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_m_bo.getGuildId());

                    getUSServer().rpc2us().requestToRepeat(guildUsId, rpc
                            , new _ARpcCallBack<GuildMemberInitState>()
                            {
                                @Override
                                public void call_back(int _errCode, GuildMemberInitState _rpc)
                                {
                                    if(_errCode == Result.SUCC.getCode()) {
                                        _m_sGuildName = _rpc.retObj().getGuildName();
                                        _m_iGuildLvl = _rpc.retObj().getGuildLvl();
                                        //获取公会名缩写
                                        _m_sGuildSimpleName = _rpc.retObj().getGuildSimpleName();
                                        //初始化赚速加成
                                        onGuildAddValueChg(_rpc.retObj().getBuildingAddPerArr());
                                    }

                                    setInited();
                                }
                            }
                            , 3
                            , ()-> {
                                USLog.error(getUSServer(), "player:{} guild:{} send rpc initStateChg - fail.", getCid(), _m_bo.getGuildId());

                                //此处仍然需要设置成功
                                setInited();
                            });
                }
                else
                {
                    //不在公会直接成功
                    setInited();
                }
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList() {
        return null;
    }

    @Override
    public void onInited() {
        //玩家目前在公会中，更新玩家当前的火星自助名单
        if (_m_bo.getGuildId() != 0)
        {
            //向对应服务器发送RPC
            PlayerBuffInfo guildMarsAutoHelpBuff = getUserData().getBuffComponent().lookupBuff(RefGeneral.Ref().guild_mars_help_auto_deal_buff_id);
            if (null != guildMarsAutoHelpBuff) {
                ensureAutoHelp(guildMarsAutoHelpBuff.getEndMs());
            } else {
                rmvAutoHelp(getGuildId());
            }
        }

        //监听对应的buff
        getUserData().getBuffComponent().getChgDelegate().addHandler(this,
                new HandlerThree<Long, Integer, Long>() {
                    @Override
                    public void handle(Long _buffId, Integer _layer, Long _endMs) {
                        if (RefGeneral.Ref().guild_mars_help_auto_deal_buff_id == _buffId) {
                            ensureAutoHelp(_endMs);
                        }
                    }
                });
        getUserData().getBuffComponent().getDelDelegate().addHandler(this,
                new HandlerOne<Long>() {
                    @Override
                    public void handle(Long _buffId) {
                        if (RefGeneral.Ref().guild_mars_help_auto_deal_buff_id == _buffId) {
                            rmvAutoHelp(getGuildId());
                        }
                    }
                });
    }

    @Override
    public void dispose() {
        getUserData().getBuffComponent().getChgDelegate().clear(this);
        getUserData().getBuffComponent().getDelDelegate().clear(this);
    }

    /**
     * 获取联盟id
     *
     * @return
     */
    public long getGuildId() {return _m_bo.getGuildId();}

    public String getGuildName()
    {
        return _m_sGuildName;
    }
    public int getGuildLvl() {return _m_iGuildLvl;}
    /**
     * 获取联盟简称
     * @return
     */
    public String getGuildSimpleName()
    {
        return _m_sGuildSimpleName;
    }

    /**
     * 检查是否有加入联盟的cd
     */
    public boolean checkJoinGuildCd()
    {
        return CommonFunc.getNowTimeMS() > _m_bo.getJoinGuildCdEndTimeMs();
    }


    /**
     * 离开联盟处理
     * @param _isKick
     * @param _needPush
     * @param _timestamp
     */
    public void onQuitGuild(long _guildId, String _oriGuildName, boolean _isKick, boolean _needPush, long _timestamp)
    {
        getUserData().lockUser();
        try
        {
            //如果是被踢出联盟邮件通知
            if (_isKick)
            {
                Mail_Data mailData = new Mail_Data();
                mailData.getContentReplace().add(_oriGuildName);
                mailData.setMailRefId(RefGeneral.Ref().guild_passive_exit_mail_id);
                MailSystem.addMail(getUSServer(), getUserData().getCid(), mailData, NPPlayerContext.createNew(ENPGameEvent.LEAVE_GUILD));
            }

            //如果公会Id不一致不做后续处理
            if(_guildId != getGuildId())
                return ;

            //刷新加入联盟cd
            refreshNextJoinCd(_timestamp);

            //设置新的联盟Id
            _m_bo.saveGuildId(getUSServer().getBM(), 0);

            //触发建筑重新计算
            getUserData().getBuildingComponent().recalAllBuilding();

            //推送数据变更
            if (_needPush)
                getUserData().sendMsgToGC(new GS2GC_032_066_OnLeaveGuild(_isKick));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 刷新加入联盟cd
     * @param _timestamp
     */
    public void refreshNextJoinCd(long _timestamp)
    {
        getUserData().lockUser();
        try
        {
            //区分是否还有免cd次数的情况
            int freeCdJoinGuildNum = _m_bo.getFreeCdJoinGuildNum();
            if (freeCdJoinGuildNum > 0)
            {
                _m_bo.saveFreeCdJoinGuildNum(getUSServer().getBM(), freeCdJoinGuildNum - 1);
            } else
            {
                long startTimeMs = Math.max(_timestamp, _m_bo.getJoinGuildCdEndTimeMs());
                _m_bo.saveJoinGuildCdEndTimeMs(getUSServer().getBM(), startTimeMs + RefGeneral.Ref().guild_join_cd_hours * 3600 * 1000L);
            }

            //推送cd变更
            getUserData().sendMsgToGC(new GS2GC_032_058_OnJoinGuildCdChg(makeJoinCdInfo()));
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造入盟cd信息
     * @return
     */
    public Guild_JoinCdInfo makeJoinCdInfo()
    {
        getUserData().lockUser();
        try
        {
            Guild_JoinCdInfo cdInfo = new Guild_JoinCdInfo();
            cdInfo.setGuildFreeJoinCdNum(_m_bo.getFreeCdJoinGuildNum());
            cdInfo.setJoinGuildCdEndTimeMs(_m_bo.getJoinGuildCdEndTimeMs());
            return cdInfo;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 玩家上线
     */
    public void onOnline()
    {
        getUserData().lockUser();
        try
        {
            if(getGuildId() <= 0)
                return ;

            //同步在线状态
            onlineStateChg(true);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 玩家下线
     */
    public void onOffline()
    {
        getUserData().lockUser();
        try
        {
            if(getGuildId() <= 0)
                return ;

            //同步在线状态
            onlineStateChg(false);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清除加入cd
     * @return
     */
    public boolean cleanJoinCd()
    {
        getUserData().lockUser();
        try
        {
            _m_bo.saveJoinGuildCdEndTimeMs(getUSServer().getBM(), 0);
            //推送cd变更
            getUserData().sendMsgToGC(new GS2GC_032_058_OnJoinGuildCdChg(makeJoinCdInfo()));
            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加免cd次数
     * @param _times
     */
    public void addFreeJoinCd(int _times)
    {
        getUserData().lockUser();
        try
        {
            _m_bo.saveFreeCdJoinGuildNum(getUSServer().getBM(), _m_bo.getFreeCdJoinGuildNum() + _times);

            //推送cd变更
            getUserData().sendMsgToGC(new GS2GC_032_058_OnJoinGuildCdChg(makeJoinCdInfo()));

        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查是否需要重置当天数据
     */
    private void _checkResetDayData()
    {
        getUserData().lockUser();
        try
        {
            //如果是当天数据不需要重置
            int todayTag = CommonFunc.getNowTagYYYYMMDD();
            if (_m_bo.getDayTag() == todayTag)
                return;

            //需要清除的数据
            _m_hadDrawConstructRewardList = new Common_IntList();

            _m_bo.setDayTag(getUSServer().getBM(), todayTag);
            _m_bo.setDayHadDrawConstructRewardList(getUSServer().getBM(), _m_hadDrawConstructRewardList.makePackage().array());
            _m_bo.saveAllMarked(getUSServer().getBM());
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 记录获得建设奖励
     * @param _num
     * @param _context
     * @return
     */
    public Result recordGainConstructReward(int _num, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //检查是否需要重置当天数据
            _checkResetDayData();

            //检查是否已经领取过
            if (_m_hadDrawConstructRewardList.getValueList().contains(_num))
                return GuildErr.GUILD_HAD_DRAW_CONSTRUCT_REWARD;

            _m_hadDrawConstructRewardList.getValueList().add(_num);

            _m_bo.saveDayHadDrawConstructRewardList(getUSServer().getBM(), _m_hadDrawConstructRewardList.makePackage().array());

            //推送变更
            getUserData().sendMsgToGC(US2GCWriter_032_GuildOp.make_073_OnSelfDailyDataChg(makeDailyInfo()));

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取建筑加成值
     * @param _attrType
     * @return
     */
    public int getBuildingAddPer(ESpecAttrType _attrType)
    {
        getUserData().lockUser();
        try
        {
            return _m_arrGuildBuildingAddPer[_attrType.ordinal()];
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 大臣技能变化
     * @param _heroInfo
     */
    public void onHeroBusinessSkillChg(HeroInfo _heroInfo)
    {
        getUserData().lockUser();
        try
        {
            //校对派遣英雄的Id
            if (_m_bo.getGuildId() <= 0 || null == _heroInfo || _heroInfo.getHeroId() != _m_bo.getDispatchHeroId())
                return;

            sendDispatchHeroValueChg(_heroInfo);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 大臣信息（实力、等级）变化
     * @param _heroInfo
     */
    public void onHeroInfoChg(HeroInfo _heroInfo)
    {
        getUserData().lockUser();
        try
        {
            //校对派遣英雄的Id
            if (_m_bo.getGuildId() <= 0 || null == _heroInfo || _heroInfo.getHeroId() != _m_bo.getDispatchHeroId())
                return;

            sendDispatchHeroInfoChg(_heroInfo);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public Guild_MemberDailyData makeDailyInfo()
    {
        getUserData().lockUser();
        try
        {
            Guild_MemberDailyData proto = new Guild_MemberDailyData();
            proto.setDate(_m_bo.getDayTag());
            proto.getTodayDrawConstructRewardList().addAll(_m_hadDrawConstructRewardList.getValueList());
            return proto;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /*****************
     *
     * ========== 构造跨服联盟相关处理附加消息部分 开始
     *
     */

    public GuildOp_002_UserJoinInfo make_002_UserJoinInfo()
    {
        //结构附加信息
        GuildOp_002_UserJoinInfo addInfo = new GuildOp_002_UserJoinInfo();
        addInfo.setEarnings(getUserData().getPlayerComponent().getEarnings());
        addInfo.setLevel(getUserData().getParam(ENPPlayerParam.LEVEL));
        addInfo.setCanRequest(getUserData().getUSServer().getGuildMgr().getJoinRequestMgr().canPlayerAddJoinRequestCount(getCid()));

        return addInfo;
    }

    /*****************
     *
     * ========== 构造跨服联盟相关处理附加消息部分 结束
     *
     */


    /*****************
     *
     * ========== 构造跨服联盟相关处理RPC 开始
     *
     */

    /**
     * 构造缓存信息
     * @return
     */
    public CachedObj_CachedGuildInfo makeCacheGuildInfo()
    {
        CachedObj_CachedGuildInfo info = new CachedObj_CachedGuildInfo();
        info.setGuildId(getGuildId());
        info.setGuildName(getGuildName());
        info.setGuildSimpleName(getGuildSimpleName());
        return info;
    }

    /**
     * 加入公会的处理
     * @param _newSimpleName
     */
    public Result joinGuild(long _guildId, String _guildName, String _newSimpleName, ArrayList<Integer> _addPerArr)
    {
        getUserData().lockUser();
        try
        {
            if(_m_bo.getGuildId() > 0)
                return GuildErr.PLAYER_ALREADY_IN_GUILD;

            //记录玩家入盟次数
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.JOIN_GUILD);
            getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.JOIN_GUILD_TIMES, 1, context);

            //设置信息
            _m_bo.saveGuildId(getUSServer().getBM(), _guildId);
            _m_sGuildName = _guildName;
            _m_sGuildSimpleName = _newSimpleName;

            //发送邮件
            Mail_Data mailData = new Mail_Data();
            mailData.getContentReplace().add(getGuildName());
            mailData.setMailRefId(RefGeneral.Ref().guild_join_success_mail_id);
            MailSystem.addMail(getUSServer(), getCid(), mailData, context);

            //如果是第一次入盟,需要发带奖励的邮件
            if (getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.JOIN_GUILD_TIMES) == 1)
            {
                Mail_Data mailDataA = new Mail_Data();
                mailDataA.setMailRefId(RefGeneral.Ref().guild_first_time_join_success_mail_id);
                mailData.getContentReplace().add(getGuildName());
                mailDataA.getItemList().getItemList().addAll(CommonFunc.costItemListToProto(RefGeneral.Ref().guild_first_time_join_reward_list));
                MailSystem.addMail(getUSServer(), getCid(), mailDataA, context);
            }

            //修改玩家Cache中数据
            PlayerCacheFunc.updateGuildInfo(getUSServer(), getCid(), makeCacheGuildInfo());

            //如果公会数据存在需要发送RPC初始化
            GuildMemberInitState rpc = new GuildMemberInitState();
            rpc.req().setGuildId(_m_bo.getGuildId());
            rpc.req().setCid(getCid());

            //获取公会归属UsId
            int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_m_bo.getGuildId());

            getUSServer().rpc2us().requestToRepeat(guildUsId, rpc
                    , new _ARpcCallBack<GuildMemberInitState>()
                    {
                        @Override
                        public void call_back(int _errCode, GuildMemberInitState _rpc)
                        {
                            if(_errCode == Result.SUCC.getCode()) {
                                _m_sGuildName = _rpc.retObj().getGuildName();
                                _m_iGuildLvl = _rpc.retObj().getGuildLvl();
                                //获取公会名缩写
                                _m_sGuildSimpleName = _rpc.retObj().getGuildSimpleName();
                                //初始化赚速加成
                                onGuildAddValueChg(_rpc.retObj().getBuildingAddPerArr());

                                //重新计算建筑加成
                                getUserData().getBuildingComponent().recalAllBuilding();

                                //修改玩家Cache中数据
                                PlayerCacheFunc.updateGuildInfo(getUSServer(), getCid(), makeCacheGuildInfo());
                            }
                        }
                    }
                    , 3
                    , ()-> {
                        USLog.error(getUSServer(), "player:{} guild:{} send rpc init state for join guild - fail.", getCid(), getGuildId());
                    });

            return Result.SUCC;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 公会缩写变更
     * @param _newSimpleName
     */
    public void onGuildSimpleNameChg(long _guildId, String _guildName, String _newSimpleName)
    {
        getUserData().lockUser();
        try
        {
            if(_m_bo.getGuildId() != _guildId)
                return ;

            _m_sGuildName = _guildName;
            _m_sGuildSimpleName = _newSimpleName;
        } finally
        {
            getUserData().unlockUser();
        }
    }


    /**
     * 公会等级变更
     * @param _lvl
     */
    public void onGuildLevelChg(long _guildId, int _lvl)
    {
        getUserData().lockUser();
        try
        {
            if(_m_bo.getGuildId() != _guildId)
                return ;

            _m_iGuildLvl = _lvl;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /***
     * 联盟赚速加成变动的处理
     * @param _addValueList
     */
    public void onGuildAddValueChg(ArrayList<Integer> _addValueList)
    {
        //初始化赚速加成
        for(int i = 0; i < ESpecAttrType.values().length; i++)
        {
            if(_addValueList.size() <= i)
            {
                USLog.error(getUSServer(), "player:{} guild:{} send rpc initStateChg - fail, buildingAddPerArr size err.", getCid(), _m_bo.getGuildId());
                break;
            }

            _m_arrGuildBuildingAddPer[i] = _addValueList.get(i);
        }
    }

    /**
     * 向公会设置确保自动互助生效
     */
    public void ensureAutoHelp(long _endTimeMS)
    {
        if(0 == _m_bo.getGuildId())
            return ;

        //获取公会归属UsId
        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_m_bo.getGuildId());

        GuildMarsEnsureAutoHelp rpc = new GuildMarsEnsureAutoHelp();
        rpc.req().setGuildId(_m_bo.getGuildId());
        rpc.req().setCid(getCid());
        rpc.req().setEndTimeMS(_endTimeMS);

        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                , ()-> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc GuildMarsEnsureAutoHelp fail.", getCid(), _m_bo.getGuildId());
                });
    }
    public void rmvAutoHelp(long _guidId)
    {
        if(0 == _m_bo.getGuildId())
            return ;

        GuildMarsRmvAutoHelp rpc = new GuildMarsRmvAutoHelp();
        rpc.req().setGuildId(_m_bo.getGuildId());
        rpc.req().setCid(getCid());

        //获取公会归属UsId
        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_m_bo.getGuildId());

        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                , ()-> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc GuildMarsRmvAutoHelp fail.", getCid(), _m_bo.getGuildId());
                });
    }


    public void onlineStateChg(boolean _onLineState)
    {
        if(0 == _m_bo.getGuildId())
            return ;

        GuildMemberOnlineState rpc = new GuildMemberOnlineState();
        rpc.req().setGuildId(_m_bo.getGuildId());
        rpc.req().setCid(getCid());
        rpc.req().setIsOnLine(_onLineState);
        rpc.req().setTimeMS(CommonFunc.getNowTimeMS());

        //获取公会归属UsId
        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_m_bo.getGuildId());

        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc
                , null
                , 3
                , ()-> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc onlineStateChg -state[{}]- fail.", getCid(), _m_bo.getGuildId(), _onLineState);
                });
    }


    /****
     * 发送玩家派遣数据变化的RPC
     * @param _heroInfo
     */
    public void sendDispatchHeroValueChg(HeroInfo _heroInfo)
    {
        if(null == _heroInfo || _heroInfo.getHeroId() != _m_bo.getDispatchHeroId())
            return ;

        GuildMemberHeroDispatchValueChg rpc = new GuildMemberHeroDispatchValueChg();
        rpc.req().setGuildId(_m_bo.getGuildId());
        rpc.req().setCid(getCid());
        rpc.req().setHeroId(_heroInfo.getHeroId());
        rpc.req().setDispatchValue(_heroInfo.getBusinessSkillMgr().calGuildDispatchAddValue());

        //获取公会归属UsId
        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_m_bo.getGuildId());

        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                , ()-> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc sendDispatchHeroValueChg fail.", getCid(), _m_bo.getGuildId());
                });
    }
    public void sendDispatchHeroInfoChg(HeroInfo _heroInfo)
    {
        if(null == _heroInfo || _heroInfo.getHeroId() != _m_bo.getDispatchHeroId())
            return ;

        GuildMemberHeroDispatchInfoChg rpc = new GuildMemberHeroDispatchInfoChg();
        rpc.req().setGuildId(_m_bo.getGuildId());
        rpc.req().setCid(getCid());
        rpc.req().setHeroId(_heroInfo.getHeroId());
        rpc.req().setLevel(_heroInfo.getLevel());
        rpc.req().setPower(_heroInfo.getPower());
        rpc.req().setSkinId(_heroInfo.getSkinId());

        //获取公会归属UsId
        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_m_bo.getGuildId());

        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                , ()-> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc sendDispatchHeroInfoChg fail.", getCid(), _m_bo.getGuildId());
                });
    }

    /**
     * 移除火星互助信息
     * @param _helpId
     */
    public void sendRmvMarsHelpRPC(long _helpId, boolean _needPush)
    {
        //清除公会求助数据
        GuildRmvMarsHelp rpc = new GuildRmvMarsHelp();
        rpc.req().setCid(getUserData().getCid());
        rpc.req().setGuildId(getGuildId());
        rpc.req().setHelpId(_helpId);
        rpc.req().setNeedPush(_needPush);

        //获取公会归属UsId
        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(_m_bo.getGuildId());

        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                , ()-> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc sendRmvMarsHelpRPC fail.", getCid(), _m_bo.getGuildId());
                });
    }

    /*********
     * 增加所属公会宝箱活跃度
     */
    public void addGuildBoxPoint(int _addPoint, NPPlayerContext _context)
    {
        if(getGuildId() <= 0)
            return ;

        //获取公会归属UsId
        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(getGuildId());

        GuildAddGuildPoint rpc = new GuildAddGuildPoint();
        rpc.req().setCid(getUserData().getCid());
        rpc.req().setGuildId(getGuildId());
        rpc.req().setAddPoint(_addPoint);
        rpc.req().setContextType(_context.getType().ordinal());
        rpc.req().setGuildId(_context.getGuid());

        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                , ()-> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc addGuildBoxPoint fail.", getCid(), _m_bo.getGuildId());
                });
    }

    /*********
     * 增加所属公会宝箱
     */
    public void addGuildBox(EGuildBoxType _boxType, long _boxId, long _count, NPPlayerContext _context)
    {
        if(getGuildId() <= 0)
            return ;

        //获取公会归属UsId
        int guildUsId = CommonFunc.parseServerTypeIdFromInstanced(getGuildId());

        GuildAddGuildBox rpc = new GuildAddGuildBox();
        rpc.req().setCid(getUserData().getCid());
        rpc.req().setGuildId(getGuildId());
        rpc.req().setBoxType(_boxType);
        rpc.req().setBoxId(_boxId);
        rpc.req().setCount(_count);
        rpc.req().setContextType(_context.getType().ordinal());
        rpc.req().setGuildId(_context.getGuid());

        getUSServer().rpc2us().requestToRepeat(guildUsId, rpc, null, 3
                , ()-> {
                    USLog.error(getUSServer(), "player:{} guild:{} send rpc addGuildBox fail.", getCid(), _m_bo.getGuildId());
                });
    }


    /*****************
     *
     * ========== 构造跨服联盟相关处理RPC 结束
     *
     */
}
