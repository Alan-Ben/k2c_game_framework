package NPUSServer.UsMars.MineCore;

import ALBasicCommon.ALSerializeMaker;
import ALBasicServer.ALBasicMutex.MutexAtom;
import AllRpcData.US_Service.Mars.MarsMineSettle;
import Common.Common_LongList;
import Common.CrossDataType.MarsMineInfo;
import Common.MarsEnum.EMarsExplorePVPLogType;
import Common.MarsObj.Mars_ExploreBattlePlayerInfo;
import Common.MarsObj.Mars_MineAttackLog;
import Common.MarsObj.Mars_MineCollectLog;
import Common.MarsObj.Mars_MineDefenceLog;
import Common.ServerObj.ServerObj_MarsMine;
import Common.ServerObj.ServerObj_MarsMineSettle;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPGameRes.GameObjs.Mars.UsMarsBattle.UsMarsBattleCal;
import NPGameRes.GameObjs.Mars.UsMarsBattle.UsMarsBattleCal.FightResult;
import NPGameRes.GameObjs.Mars.UsMarsBattle._IUsMarsBattleObj;
import NPGameRes.Refs.Mars.RefMarsExploreMine;
import NPUSServer.CrossDataBasicPack._ATCrossDataInfo;
import NPUSServer.GeneralV.UsID;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.USLog;
import NPUSServer.UsMars.UsMarsAction.UsMineAction.UsMarsMineAttackAction;
import RPC._ARpcCallBack;
import USDB.Bo.UsMarsMineBO;
import USLOGDB.Bo.LogUsMarsMineFightBO;
import USLOGDB.Bo.LogUsMarsMineOccupyBO;
import USLOGDB.Bo.LogUsMarsMineSettleBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

/**
 * US内单个矿的信息存储对象
 */
public class UsMarsMineObj extends _ATCrossDataInfo<MarsMineInfo> implements _IUsMarsBattleObj
{
    //归属管理器
    private UsMarsMineCore _m_mcMineCore;

    //数据库数据对象
    private UsMarsMineBO _m_MineBo;
    //内存的状态序列号，每次占领状态变更都会刷新，客户端用此判断是否需要推送
    private long _m_lStateSerialize;
    //静态数据
    private RefMarsExploreMine _m_ref;
    //结束采集的时间戳
    private long _m_lEndCollectTimeMS;
    //进攻过的联盟ID列表（内存缓存，使用List维护插入顺序）
    private List<Long> _m_attackedGuildIds;

    //锁对象，矿本身的计算是需要多线程处理的
    private MutexAtom _m_mutex;

    public UsMarsMineObj(UsMarsMineCore _core, UsMarsMineBO _bo)
    {
        _m_mcMineCore = _core;

        _m_MineBo = _bo;
        _m_lStateSerialize = ALSerializeMaker.makeNewSerialize();

        _m_ref = RefMarsExploreMine.getMgr().get(getRefId());
        if(null == _m_ref)
        {
            USLog.error(_m_mcMineCore.getUSServer(), "player:{} refId:{} mars us mine init ref fail.", getCid(), getRefId());
        }

        //刷新采集结束时间
        _recalEndCollectTime();

        //初始化进攻联盟ID列表
        _m_attackedGuildIds = new ArrayList<>();
        _loadAttackedGuildIds();

        _m_mutex = new MutexAtom();
    }

    public UsMarsMineBO getBo() {return _m_MineBo;}
    public BM getBM() {return _m_mcMineCore.getUSServer().getBM();}
    public long getDbId() {return getBo().getId();}
    public long getCid() {return getBo().getCid();}
    public long getRefId() {return getBo().getRefId();}
    public RefMarsExploreMine getRef() {return _m_ref;}
    public long getEndShowMs() {return getBo().getEndShowMs();}
    public long getRemainNum() {return getBo().getRemainNum();}
    public long getTeamId() {return getBo().getTeamId();}
    public long getTeamLossValue() {return getBo().getTeamLossValue();}
    public long getTeamTroopNum() {return getBo().getTeamTroopNum();}
    public long getStartCollectMs() {return getBo().getStartCollectMs();}
    public long getCollectSpeed() {return getBo().getCollectSpeed();}
    public long getPlayerGuildId() {return getBo().getGuildId();}
    public String getCname() {return getBo().getCname();}
    public String getGuildSimpleName() {return getBo().getGuildSimpleName();}

    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    /**
     * 从数据库加载进攻过的联盟ID列表
     * 使用Common_LongList协议对象反序列化byte[]数据
     */
    private void _loadAttackedGuildIds()
    {
        byte[] data = getBo().getAttackedGuildIds();
        if(data == null || data.length == 0)
        {
            _m_attackedGuildIds.clear();
            return;
        }

        try
        {
            Common_LongList proto = new Common_LongList();
            proto.readPackage(ByteBuffer.wrap(data));
            _m_attackedGuildIds = proto.getValueList();
        }
        catch(Exception e)
        {
            USLog.error(_m_mcMineCore.getUSServer(), 
                "UsMarsMineObj._loadAttackedGuildIds - load failed: exception occurred, mineId={}, error={}", 
                getDataId(), e.getMessage());
            _m_attackedGuildIds.clear();
        }
    }

    /**
     * 将内存中的进攻联盟ID列表序列化并保存到数据库
     * 使用Common_LongList协议对象进行序列化
     */
    private void _saveAttackedGuildIds()
    {
        try
        {
            Common_LongList proto = new Common_LongList();
            proto.getValueList().addAll(_m_attackedGuildIds);
            getBo().saveAttackedGuildIds(getBM(), proto.makePackage().array());
        }
        catch(Exception e)
        {
            USLog.error(_m_mcMineCore.getUSServer(), 
                "UsMarsMineObj._saveAttackedGuildIds - save failed: exception occurred, mineId={}, error={}", 
                getDataId(), e.getMessage());
        }
    }

    /**
     * 记录进攻的联盟ID
     * 检查联盟ID是否已存在，不存在则添加并保存到数据库
     * 
     * @param _guildId 进攻方联盟ID
     * @return 是否是新增的联盟ID（true=新增，false=已存在）
     * 
     * 线程安全：调用前需要持有锁
     */
    public void recordAttackedGuildId(long _guildId)
    {
        _lock();
        try{
            // 联盟ID为0表示无联盟，不记录
            if(_guildId <= 0)
                return;

            // 检查是否已存在
            if(_m_attackedGuildIds.contains(_guildId))
                return;

            // 添加到列表
            _m_attackedGuildIds.add(_guildId);

            // 保存到数据库
            _saveAttackedGuildIds();
        }finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否有其他联盟进攻过
     * @param _guildId 联盟ID
     * @return 是否进攻过
     */
    public boolean hasOtherGuildAttacked(long _guildId)
    {
        if(_guildId <= 0)
            return false;

        for (long otherGuildId : _m_attackedGuildIds)
        {
            if(otherGuildId != _guildId)
                return true;
        }

        return false;
    }

    /**
     * 重新计算采矿结束时间
     */
    private void _recalEndCollectTime()
    {
        //无采集速度则设置结束时间为无效时间
        if(getCollectSpeed() <= 0) {
            _m_lEndCollectTimeMS = getEndShowMs();
            return;
        }

        //如已经无矿则直接设置为0
        if(getRemainNum() <= 0) {
            _m_lEndCollectTimeMS = 0;
            return;
        }

        //计算采集时间
        _m_lEndCollectTimeMS = getStartCollectMs() + (getRemainNum() * 1000 / getCollectSpeed());
    }

    public long getStateSerialize() {return _m_lStateSerialize;}

    /**
     * 判断当前矿是否超过有效期
     * @return
     */
    public boolean isOutofDate()
    {
        //超过展示时间视为超时
        return CommonFunc.getNowTimeMS() > getEndShowMs();
    }

    /**
     * 判断当前矿是否有效
     * @return
     */
    public boolean isDisable()
    {
        //大于采集时间都是无效
        return CommonFunc.getNowTimeMS() > _m_lEndCollectTimeMS;
    }


    /**
     * 发起占领
     * @param _attckInfo
     * @return
     */
    protected boolean _occupy(UsMarsMineAttackAction _attckInfo)
    {
        if(null == _attckInfo)
            return false;

        _lock();

        try
        {
            //不能攻击自己的火星矿, 和同盟玩家的火星矿
            if(getCid() == _attckInfo.getPlayer().getCid()
                    || (getPlayerGuildId() != 0 && getPlayerGuildId() == _attckInfo.getPlayer().getGuildId()))
            {
                //队伍返回
                _attckInfo.teamBack();
                return false;
            }

            //如果矿超时，且无队伍，直接不让占领
            if(isDisable() && getCid() <= 0)
            {
                //队伍返回
                _attckInfo.teamBack();
                return false;
            }

            //记录进攻的联盟ID（在战斗计算前记录，无论胜负都记录）
            long attackerGuildId = _attckInfo.getPlayer().getGuildId();
            recordAttackedGuildId(attackerGuildId);

            //进行战斗过程计算
            //FightResult fightResult = UsMarsBattleCal.calFight(_attckInfo, this);
            FightResult fightResult = UsMarsBattleCal.curUseCalFight(_attckInfo, this);

            //刷新状态序列号，这里刷新是因为战斗防守方即使没变化，实力也会变化
            _m_lStateSerialize = ALSerializeMaker.makeNewSerialize();

            //记录战斗日志（只有在有防守方玩家时才记录）
            if(fightResult != null && getCid() > 0)
            {
                //构造战斗双方数据（进攻方和防守方数据只构造一次，复用于两个日志）
                //进攻方信息
                Mars_ExploreBattlePlayerInfo attackerInfo = new Mars_ExploreBattlePlayerInfo();
                attackerInfo.setCid(_attckInfo.getCid());
                attackerInfo.setOriSoldierNum(fightResult.attackerOriSoldierNum);
                attackerInfo.setHurtSoldierNum(fightResult.attackerHurtSoldierNum);
                attackerInfo.setSingleSoldierPower(fightResult.attackerSingleSoldierPower);

                //防守方信息
                Mars_ExploreBattlePlayerInfo defenderInfo = new Mars_ExploreBattlePlayerInfo();
                defenderInfo.setCid(getCid());
                defenderInfo.setOriSoldierNum(fightResult.defencerOriSoldierNum);
                defenderInfo.setHurtSoldierNum(fightResult.defencerHurtSoldierNum);
                defenderInfo.setSingleSoldierPower(fightResult.defencerSingleSoldierPower);

                //进攻方日志
                Mars_MineAttackLog attackLog = new Mars_MineAttackLog();
                attackLog.setIsSucc(fightResult.win);
                attackLog.setMineRefId(getRefId());
                attackLog.setAttacker(attackerInfo);
                attackLog.setDefender(defenderInfo);

                //发送进攻方日志
                NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem.SendLog(
                        _m_mcMineCore.getUSServer(),
                        _attckInfo.getCid(),
                        EMarsExplorePVPLogType.MINE_ATTACK,
                        CommonFunc.ByteBfferToBytes(attackLog.makePackage()));

                //如果有防守方玩家，发送防守方日志
                if (getCid() > 0)
                {
                    //防守方日志（复用相同的进攻方和防守方数据对象）
                    Mars_MineDefenceLog defenceLog = new Mars_MineDefenceLog();
                    defenceLog.setIsSucc(!fightResult.win);
                    defenceLog.setMineRefId(getRefId());
                    defenceLog.setDefender(defenderInfo);
                    defenceLog.setAttacker(attackerInfo);

                    //发送防守方日志
                    MarsMineSystem.SendLog(
                            _m_mcMineCore.getUSServer(),
                            getCid(),
                            Common.MarsEnum.EMarsExplorePVPLogType.MINE_DEFEND,
                            CommonFunc.ByteBfferToBytes(defenceLog.makePackage()));

                    //发送联盟频道消息
                    MarsMineSystem.SendDefenceChatMessage(
                            _m_mcMineCore.getUSServer(),
                            getDataId(),
                            getPlayerGuildId(),
                            getCname(),
                            _attckInfo
                    );
                }
            }

            //处理战斗完成
            if(fightResult != null && fightResult.win) //战胜防守玩家，进驻火星矿，开始采矿
            {
                _settleAndOff(_attckInfo);
                return true;
            }
            else //失败，返回队伍
            {
                //队伍返回
                _attckInfo.teamAttFail();
                return false;
            }
        }
        finally
        {
            _unlock();
        }
    }
    /**
     * 清空占领信息
     * @return
     */
    protected boolean _forceClear()
    {
        _lock();

        try
        {
            //结算采集，直接使用空占有
            _settleAndOff(null);

            //刷新状态序列号
            _m_lStateSerialize = ALSerializeMaker.makeNewSerialize();

            return true;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 矿无效时需要移除的时候进行的结算处理
     * 如果矿已经采集完毕则直接关闭
     */
    protected boolean _settleEnd()
    {
        _lock();

        try
        {
            long teamId = getTeamId();

            //如果有队伍，且未采集完则不处理
            if(teamId > 0 && !isDisable())
                return false;

            //如果未超时且未采集完则不处理
            if(!isOutofDate() && !isDisable())
                return false;

            //结算当前队伍
            if(teamId > 0)
            {
                int usId = CommonFunc.parseServerTypeIdFromCid(getCid());
                //计算当前玩家产出持续时间
                long settleSecs = (CommonFunc.getNowTimeMS() - getStartCollectMs()) / 1000;
                long outSum = settleSecs * getCollectSpeed();

                //当前矿剩余数量
                //对比总产出和剩余，不会超过剩余，如总产出直接超出剩余，则直接结算矿产
                if(outSum >= getRemainNum())
                    outSum = getRemainNum();

                //保存剩余资源
                getBo().saveRemainNum(getBM(), getRemainNum() - outSum);

                //结算当前玩家数据
                ServerObj_MarsMineSettle obj = new ServerObj_MarsMineSettle();
                obj.setCid(getCid());
                obj.setTeamId(teamId);
                obj.setId(getDataId());
                obj.setRefId(getRefId());
                obj.setNum(outSum);
                obj.setLossValue(getTeamLossValue());
                //推送当前玩家结算数据（失败重试10次），统一通过offlineReward进行下发处理
                MarsMineSettle rpc = new MarsMineSettle();
                rpc.req().setInfo(obj);
                _m_mcMineCore.getUSServer().rpc2us().requestToRepeat(usId, rpc,
                        new _ARpcCallBack<MarsMineSettle>()
                        {
                            @Override
                            public void call_back(int _errCode, MarsMineSettle _rpc)
                            {
                                if(_errCode > 0)
                                {
                                }
                            }
                        },
                        10,
                        () ->
                        {
                            USLog.error(_m_mcMineCore.getUSServer(), "player:{} teamId:{} mine:{} send rpc MarsMineSettle fail.", getCid(), getTeamId(), getDataId());
                        });

                //结算日志
                LogUsMarsMineSettleBO logBo = new LogUsMarsMineSettleBO();
                logBo.setMineInstanceId(getBM(), getDataId());
                logBo.setMineRefId(getBM(), getRefId());
                logBo.setCid(getBM(), getCid());
                logBo.setTeamId(getBM(), getTeamId());
                logBo.setSettleSecs(getBM(), settleSecs);
                logBo.setSettleNum(getBM(), outSum);
                logBo.setLossValue(getBM(), getTeamLossValue());
                logBo.setRemainNum(getBM(), getRemainNum());
                CommLogDB.log(getBM(), logBo);

                //采集完成日志
                Mars_MineCollectLog collectLog = new Mars_MineCollectLog();
                collectLog.setTeamId(teamId);
                collectLog.setMineRefId(getRefId());
                collectLog.setCollectNum(outSum);
                MarsMineSystem.SendLog(_m_mcMineCore.getUSServer(), getCid(),
                        EMarsExplorePVPLogType.MINE_COLLECT_COMPLETE, CommonFunc.ByteBfferToBytes(collectLog.makePackage()));
            }

            //刷新结束时间
            _recalEndCollectTime();

            //刷新状态序列号
            _m_lStateSerialize = ALSerializeMaker.makeNewSerialize();

            //清空进攻联盟ID列表（矿即将被删除，清理历史数据）
            _m_attackedGuildIds.clear();

            //移除数据（删除BO，矿彻底移除）
            _m_MineBo.del(_m_mcMineCore.getUSServer().getBM());

            return true;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 主动离开发起结算
     * @param _cid
     * @param _teamId
     */
    protected Result _settleByLeave(long _cid, long _teamId)
    {
        _lock();

        try
        {
            //检查归属玩家
            if(getCid() != _cid || getTeamId() != _teamId)
                return MarsErr.MARS_MINE_OTHER_PLAYER_OCCUPY;

            //结算采集，直接使用空占有
            _settleAndOff(null);

            //刷新状态序列号
            _m_lStateSerialize = ALSerializeMaker.makeNewSerialize();

            return Result.SUCC;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * GM命令：修改矿的剩余资源数量
     * <p>
     * 执行流程：
     * 1. 加锁保证线程安全
     * 2. 参数校验（不能为负数）
     * 3. 更新剩余资源数量到数据库
     * 4. 重新计算采集结束时间
     * 5. 刷新状态序列号（触发客户端推送）
     * @param _newRemainNum 新的剩余资源数量
     * @return 操作结果
     * <p>
     * 线程安全：使用_lock()/_unlock()保护
     */
    public Result gmSetRemainNum(long _newRemainNum)
    {
        _lock();

        try
        {
            //参数校验
            if (_newRemainNum < 0)
                return CommErr.PARAM_ERROR;

            //记录原始数量用于日志
            long oldRemainNum = getRemainNum();

            //更新剩余资源数量到数据库
            getBo().saveRemainNum(getBM(), _newRemainNum);

            //重新计算采集结束时间
            _recalEndCollectTime();

            //刷新状态序列号，触发客户端推送
            _m_lStateSerialize = ALSerializeMaker.makeNewSerialize();

            USLog.error(_m_mcMineCore.getUSServer(),
                    "UsMarsMineObj.gmSetRemainNum - set remain num success: mineId={}, cid={}, oldRemainNum={}, newRemainNum={}",
                    getDataId(), getCid(), oldRemainNum, _newRemainNum);

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }


    /**
     * 构造服务器内业务处理的传递消息结构体
     * @return
     */
    public ServerObj_MarsMine toServerProto()
    {
        ServerObj_MarsMine proto = new ServerObj_MarsMine();
        proto.setId(getDataId());
        proto.setCid(getCid());
        proto.setRefId(getRefId());
        proto.setSerialize(getStateSerialize());
        proto.setEndShowMs(getEndShowMs());
        proto.setRemainNum(getRemainNum());
        proto.setTeamId(getTeamId());
        proto.setStartCollectMs(getStartCollectMs());
        proto.setCollectSpeed(getCollectSpeed());
        proto.setGuildId(getPlayerGuildId());
        proto.setTroopNum((getTeamTroopNum() - getTeamLossValue()));
        //队伍实力 = 当前队伍带兵量 * 队伍单兵实力
        proto.setTeamPower((proto.getTroopNum() * getTeamSoldierPower()));
        return proto;
    }

    /**
     * 更换当前占领数据
     * @param _newOccupy
     */
    private void _settleAndOff(UsMarsMineAttackAction _newOccupy)
    {
        long nowMs = CommonFunc.getNowTimeMS();

        //结算当前队伍
        long preTeamId = getTeamId();
        int usId = 0;
        final MarsMineSettle rpc = new MarsMineSettle();
        //先做结算
        if(preTeamId > 0)
        {
            //计算当前玩家产出持续时间
            long settleSecs = (CommonFunc.getNowTimeMS() - getStartCollectMs()) / 1000;
            long outSum = settleSecs * getCollectSpeed();

            //当前矿剩余数量
            //对比总产出和剩余，不会超过剩余，如总产出直接超出剩余，则直接结算矿产
            if(outSum >= getRemainNum())
                outSum = getRemainNum();

            //保存剩余资源
            getBo().saveRemainNum(getBM(), getRemainNum() - outSum);

            usId = CommonFunc.parseServerTypeIdFromCid(getCid());
            //结算当前玩家数据，创建RPC对象
            ServerObj_MarsMineSettle obj = new ServerObj_MarsMineSettle();
            obj.setCid(getCid());
            obj.setTeamId(preTeamId);
            obj.setId(getDataId());
            obj.setRefId(getRefId());
            obj.setNum(outSum);
            obj.setEndCollectMs(nowMs);
            obj.setLossValue(getTeamLossValue());
            //推送当前玩家结算数据（失败重试10次），统一通过offlineReward进行下发处理
            rpc.req().setInfo(obj);

            //结算日志
            LogUsMarsMineSettleBO logBo = new LogUsMarsMineSettleBO();
            logBo.setMineInstanceId(getBM(), getDataId());
            logBo.setMineRefId(getBM(), getRefId());
            logBo.setCid(getBM(), getCid());
            logBo.setTeamId(getBM(), getTeamId());
            logBo.setSettleSecs(getBM(), settleSecs);
            logBo.setSettleNum(getBM(), outSum);
            logBo.setLossValue(getBM(), getTeamLossValue());
            logBo.setRemainNum(getBM(), getRemainNum());
            CommLogDB.log(getBM(), logBo);

            //采集完成日志
            Mars_MineCollectLog collectLog = new Mars_MineCollectLog();
            collectLog.setTeamId(preTeamId);
            collectLog.setMineRefId(getRefId());
            collectLog.setCollectNum(outSum);
            MarsMineSystem.SendLog(_m_mcMineCore.getUSServer(), getCid(),
                    EMarsExplorePVPLogType.MINE_COLLECT_COMPLETE, CommonFunc.ByteBfferToBytes(collectLog.makePackage()));
        }

        //新占领玩家数据检查
        if(null == _newOccupy) //无新占领玩家，重置占领玩家数据
        {
            getBo().setCid(getBM(), 0);
            getBo().setTeamId(getBM(), 0);
            getBo().setTeamSoldierPower(getBM(), 0);
            getBo().setTeamTroopNum(getBM(), 0);
            getBo().setTeamLossValue(getBM(), 0);
            getBo().setStartCollectMs(getBM(), 0);
            getBo().setCollectSpeed(getBM(), 0);
            getBo().setGuildId(getBM(), 0);

            //保存数据库
            getBo().saveAll(getBM());

            //刷新结算时间
            _recalEndCollectTime();
        }
        else //更换新占领玩家
        {
            getBo().setCid(getBM(), _newOccupy.getPlayer().getCid());
            getBo().setTeamId(getBM(), _newOccupy.getPlayer().getTeamId());
            getBo().setTeamSoldierPower(getBM(), _newOccupy.getPlayer().getTeamSoldierPower());
            getBo().setTeamTroopNum(getBM(), _newOccupy.getPlayer().getTeamTroopNum());
            getBo().setTeamLossValue(getBM(), _newOccupy.getPlayer().getTeamLossValue());
            getBo().setStartCollectMs(getBM(), nowMs);
            getBo().setCollectSpeed(getBM(), _newOccupy.getCollectSpeed());
            getBo().setGuildId(getBM(), _newOccupy.getPlayer().getGuildId());
            getBo().setCname(getBM(), _newOccupy.getPlayer().getCname());
            getBo().setGuildSimpleName(getBM(), _newOccupy.getPlayer().getGuildBName());

            getBo().saveAll(getBM());

            //刷新结算时间
            _recalEndCollectTime();

            //推送玩家数据，修正玩家对应队伍的状态数据
            _newOccupy.teamAttSuc(getStartCollectMs(), _m_lEndCollectTimeMS);

            //占领日志
            LogUsMarsMineOccupyBO logBo = new LogUsMarsMineOccupyBO();
            logBo.setMineInstanceId(getBM(), getDataId());
            logBo.setMineRefId(getBM(), getRefId());
            logBo.setCid(getBM(), getCid());
            logBo.setTeamId(getBM(), getTeamId());
            logBo.setLossValue(getBM(), getTeamLossValue());
            logBo.setRemainNum(getBM(), getRemainNum());
            logBo.setCollectSpeed(getBM(), getCollectSpeed());
            logBo.setStartCollectMs(getBM(), getStartCollectMs());
            logBo.setEndCollectMs(getBM(), _m_lEndCollectTimeMS);
            CommLogDB.log(getBM(), logBo);
        }

        //最后遣返队伍，避免队伍遣返请求状态还是旧状态
        if(preTeamId > 0)
        {
            _m_mcMineCore.getUSServer().rpc2us().requestToRepeat(usId, rpc,
                    new _ARpcCallBack<MarsMineSettle>()
                    {
                        @Override
                        public void call_back(int _errCode, MarsMineSettle _rpc)
                        {
                            if(_errCode > 0)
                            {
                            }
                        }
                    },
                    10,
                    () ->
                    {
                        USLog.error(_m_mcMineCore.getUSServer(), "player:{} teamId:{} mine:{} send rpc MarsMineSettle fail."
                                , rpc.req().getInfo().getCid(), rpc.req().getInfo().getTeamId(), getDataId());
                    });
        }
    }


    /************************ CrossData 重载函数 ******************/
    /**
     * 由于数据可能跨服，因此数据使用数据库Idx10000 + usId组成
     * @return
     */
    @Override
    public long getDataId() {
        return UsID.makeUsId(_m_mcMineCore.getUSServer(), getDbId());
    }

    @Override
    public MarsMineInfo makeCrossDataProtocolObj() {
        return new MarsMineInfo(getDataId(), getRefId(), getEndShowMs());
    }
    /************************ CrossData 重载函数 end ******************/

    /************************* 战斗处理对象的防守方重载函数 ******************/
    /**
     * 获取当前可参战的战斗人员数量
     * @return
     */
    public long getTeamSoldierNum(){return getTeamTroopNum() - getTeamLossValue();}

    /**
     * 获取参战的单兵实力
     * @return
     */
    public long getTeamSoldierPower(){return getBo().getTeamSoldierPower();}

    /**
     * 增加伤兵数量，注意这里是增量不是全量
     * @param _hurtNum
     */
    public void addHurtSoldierNum(long _hurtNum){
        //计算新伤兵数量
        long totalHurtNum = getTeamLossValue() + _hurtNum;
        //超出上限则按照上限
        if(totalHurtNum > getTeamTroopNum())
            totalHurtNum = getTeamTroopNum();

        //设置伤兵数量
        getBo().saveTeamLossValue(getBM(), totalHurtNum);
    }

    /**
     * 记录战斗日志的接口
     * @param _isAttacker
     * @param _isAttWin
     * @param _attackPower
     * @param _attckHurtNum
     * @param _attackTroopNum
     * @param _defencePower
     * @param _defenceHurtNum
     * @param _defenceTroopNum
     */
    public void logBattle(boolean _isAttacker, boolean _isAttWin, _IUsMarsBattleObj _enemy, long _attackPower, long _attckHurtNum
            , long _attackTroopNum, long _defencePower, long _defenceHurtNum, long _defenceTroopNum)
    {
        //TODO ： 发送邮件给防守玩家，同步战斗结果

        //记录日志
        LogUsMarsMineFightBO logBo = new LogUsMarsMineFightBO();
        logBo.setMineInstanceId(getBM(), getDataId());

        //进攻结果，矿永远是防守方
        logBo.setIsWin(getBM(), !_isAttWin);
        logBo.setAttackLoseValue(getBM(), 0);
        logBo.setAttackTroopNum(getBM(), _attackTroopNum);

        //进攻方类型转化
        UsMarsMineAttackAction enemyInfo = (UsMarsMineAttackAction)_enemy;
        //攻击方玩家数据
        logBo.setAttackPlayerCid(getBM(), enemyInfo.getCid());
        logBo.setAttackCostValue(getBM(), _attckHurtNum);
        logBo.setAttackTeamId(getBM(), enemyInfo.getTeamId());
        logBo.setAttackTeamPower(getBM(), _attackPower);
        logBo.setAttackTeamSoldierPower(getBM(), _enemy.getTeamSoldierPower());
        logBo.setAttackTeamTroopNum(getBM(), _attackTroopNum);
        logBo.setAttackTeamLossValue(getBM(), 0);

        //防守方玩家数据
        logBo.setDefencePlayerCid(getBM(), getCid());
        logBo.setDefenceCostValue(getBM(), _defenceHurtNum);
        logBo.setDefenceTeamId(getBM(), getTeamId());
        logBo.setDefenceTeamPower(getBM(), _defencePower);
        logBo.setDefenceTeamTroopNum(getBM(), getTeamTroopNum());
        logBo.setDefenceTeamLossValue(getBM(), getTeamLossValue());

        CommLogDB.log(getBM(), logBo);
    }
    /************************* 战斗处理对象的防守方重载函数 end ******************/
}
