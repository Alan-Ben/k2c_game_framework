package NPUSServer.Guild.Cooperate;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.GuildCooperateObj.GuildCooperate_AttackLog;
import Common.GuildCooperateObj.GuildCooperate_Info;
import Common.GuildCooperateObj.GuildCooperate_RewardPointPos;
import Common.ServerObj.ServerObj_GuildCooperatePointLayoutList;
import Common.ServerObj.ServerObj_GuildCooperatePropertyPointLayout;
import CommonEnum.ESpecAttrType;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_043_AttackpropertyPointInfo;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.GuildCooperateErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Guild.RefGuildCooperateArea;
import NPGameRes.Refs.Hero.RefHero;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.Cooperate.GuildCooperatePointInfo.AttackResult;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_032_GuildOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GuildCooperateAttackLogBO;
import USDB.Bo.GuildCooperateDamageRankBO;
import USDB.Bo.GuildCooperateMainBO;
import USDB.Bo.GuildCooperatePointInfoBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

public class GuildCooperateInfo
{
    // 所属公会信息
    private GuildInfo _m_guildInfo;

    // 主数据BO对象
    private long _m_dbId;
    private long _m_nextRefreshTimeMs;
    private int _m_resetCount;
    private long _m_recommendAreaId;
    private int _m_recommendPointIndex;
    // 据点布局列表
    private List<ServerObj_GuildCooperatePropertyPointLayout> _m_pointLayoutList;

    // 奖励据点信息列表
    private List<GuildCooperatePointInfo> _m_pointList;
    // 攻击日志列表
    private List<GuildCooperateLogInfo> _m_logList;
    // 伤害排行榜
    private GuildCooperateDamageRankList _m_damageRankList;

    // 下次检查日志移除时间
    private long _m_nextCheckLogDelTimeMs;

    private MutexAtom _m_mutex;

    // 静态缓存有效属性类型列表，避免重复构建
    private static final List<ESpecAttrType> VALID_ATTR_TYPES;

    static
    {
        VALID_ATTR_TYPES = new ArrayList<>();
        for (ESpecAttrType attrType : ESpecAttrType.values())
        {
            if (attrType != ESpecAttrType.NONE)
            {
                VALID_ATTR_TYPES.add(attrType);
            }
        }
    }

    /**
     * 构造函数
     * @param guildInfo 所属公会信息
     */
    public GuildCooperateInfo(GuildInfo guildInfo)
    {
        _m_guildInfo = guildInfo;

        _m_pointList = new ArrayList<>();
        _m_logList = new ArrayList<>();
        _m_pointLayoutList = new ArrayList<>();
        _m_damageRankList = new GuildCooperateDamageRankList(this);

        _m_mutex = new MutexAtom();
        _m_nextCheckLogDelTimeMs = 0;
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    // 便利方法
    public GuildInfo getGuildInfo()
    {
        return _m_guildInfo;
    }

    /**
     * 获取伤害排行榜管理器
     * @return 伤害排行榜管理器
     */
    public GuildCooperateDamageRankList getDamageRankList()
    {
        return _m_damageRankList;
    }

    public NPUserServer getUSServer()
    {
        return _m_guildInfo.getGuildMgr().getServer();
    }

    /**
     * 初始化主数据BO
     * @param _mainBo
     */
    public void _initBo(GuildCooperateMainBO _mainBo)
    {
        _m_dbId = _mainBo.getId();
        _m_nextRefreshTimeMs = _mainBo.getNextRefreshTimeMs();
        _m_resetCount = _mainBo.getResetCount();
        _m_recommendAreaId = _mainBo.getRecommendAreaId();
        _m_recommendPointIndex = _mainBo.getRecommendIndex();

        // 初始化据点布局数据
        if (_mainBo.getPointLayoutData() != null)
        {
            ServerObj_GuildCooperatePointLayoutList proto = new ServerObj_GuildCooperatePointLayoutList();
            proto.readPackage(ByteBuffer.wrap(_mainBo.getPointLayoutData()));
            _m_pointLayoutList = proto.getLayoutList();
        }

        // 跟据据点数据初始化据点信息
        for (ServerObj_GuildCooperatePropertyPointLayout layout : _m_pointLayoutList)
        {
            RefGuildCooperateArea areaRef = RefGuildCooperateArea.getMgr().get(layout.getAreaId());
            if (areaRef != null)
            {
                _m_pointList.add(new GuildCooperatePointInfo(this, areaRef, layout));
            } else
            {
                USLog.error(getUSServer(), "RefGuildCooperateArea not found for areaId: {}", layout.getAreaId());
            }
        }
    }

    /**
     * 初始化积分点信息BO
     * @param _pointBo
     */
    public void _initPointInfoBo(GuildCooperatePointInfoBO _pointBo)
    {
        GuildCooperatePointInfo pointInfo = lookupPoint(_pointBo.getAreaId(), _pointBo.getRewardPointIndex());
        if (pointInfo == null)
        {
            USLog.error(getUSServer(), "GuildCooperateInfo _initPointInfoBo not found point, guildId:{} areaId:{} index:{}"
                    , getGuildInfo().getGuildId(), _pointBo.getAreaId(), _pointBo.getRewardPointIndex());
            return;
        }

        pointInfo._initPointBo(_pointBo);
    }

    /**
     * 初始化攻击日志BO
     * @param _logBo
     */
    public void _initAttackLogBo(GuildCooperateAttackLogBO _logBo)
    {
        _m_logList.add(new GuildCooperateLogInfo(_logBo));
    }

    /**
     * 初始化伤害排行榜BO
     * @param _damageRankBo
     */
    public void _initDamageRankBo(GuildCooperateDamageRankBO _damageRankBo)
    {
        _m_damageRankList.initFromDB(_damageRankBo);
    }

    /**
     * 初始化完成处理
     */
    public void onInited()
    {
        _initUnlockFlags();

        List<Long> memberCidList = _m_guildInfo.getMemberMgr().getMemberCidList(null);
        _m_damageRankList.onInited(memberCidList);
    }

    /**
     * 初始化解锁标记
     */
    private void _initUnlockFlags()
    {
        _m_pointList.sort(Comparator.comparingLong(GuildCooperatePointInfo::getAreaId));

        long hadDefeatAreaId = 0;
        long processAreaId = 1; // 初始化为0，确保第一个区域会被正确处理
        boolean isDefeatAll = true;

        for (GuildCooperatePointInfo pointInfo : _m_pointList)
        {
            // 切换到新区域时检查上一区域完成状态
            if (pointInfo.getAreaId() != processAreaId)
            {
                // 检查区域ID是否连续
                if (pointInfo.getAreaId() != processAreaId + 1)
                {
                    USLog.warn(getUSServer(), "GuildCooperateInfo _initUnlockFlags areaId not continuous, guildId:{} currentAreaId:{} processAreaId:{}"
                            , getGuildInfo().getGuildId(), pointInfo.getAreaId(), processAreaId);
                    break;
                }

                // 如果上一区域没有全部击败，后续区域都不再解锁
                if (!isDefeatAll)
                    break;

                hadDefeatAreaId = processAreaId;
                processAreaId = pointInfo.getAreaId();
            }

            // 设置当前据点的解锁状态
            if (pointInfo.getAreaId() == 1)
            {
                // 第一个区域默认解锁
                pointInfo.setIsUnlock(true);
            }
            else
            {
                // 其他区域需要前一区域全部击败才解锁
                pointInfo.setIsUnlock((pointInfo.getAreaId() - 1) <= hadDefeatAreaId);
            }

            // 检查当前据点是否被击败
            if (!pointInfo.isDefeat())
                isDefeatAll = false;
        }
    }

    /**
     * 查找点位
     * @param _pos
     * @return
     */
    public GuildCooperatePointInfo lookupPoint(GuildCooperate_RewardPointPos _pos)
    {
        _lock();
        try
        {
            return lookupPoint(_pos.getAreaId(),_pos.getIndex());
        } finally
        {
            _unlock();
        }
    }

    /**
     * 查找点位
     * @param _areaId
     * @param _index
     * @return
     */
    public GuildCooperatePointInfo lookupPoint(long _areaId, int _index)
    {
        _lock();
        try
        {
            for (GuildCooperatePointInfo pointInfo : _m_pointList)
            {
                if (pointInfo.getAreaId() == _areaId && pointInfo.getIndex() == _index)
                    return pointInfo;
            }

            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查是否重置数据
     */
    public void checkResetData(boolean _isForce)
    {
        _lock();
        try
        {
            // 如果已初始化且未到刷新时间，直接返回
            if (!_isForce)
            {
                if (_m_dbId != 0 && _m_nextRefreshTimeMs > CommonFunc.getNowTimeMS())
                    return;
            }

            // 清空据点数据
            _m_pointList.clear();
            getUSServer().getBM().getBM(GuildCooperatePointInfoBO.class).delAll("guild_id", getGuildInfo().getGuildId());

            // 清空日志数据
            _m_logList.clear();
            getUSServer().getBM().getBM(GuildCooperateAttackLogBO.class).delAll("guild_id", getGuildInfo().getGuildId());

            // 清空排行榜数据
            getDamageRankList()._clearDamageRankData();

            // 计算下一次的刷新时间
            _m_nextRefreshTimeMs = RefGeneral.Ref().guild_cooperate_refresh_time.getNextFreshTimeTagMS(CommonFunc.getNowTimeMS());
            _m_resetCount += 1;

            // 随机据点信息
            _m_pointLayoutList = new ArrayList<>();
            for (RefGuildCooperateArea areaRef : RefGuildCooperateArea.getMgr().getList())
            {
                List<ServerObj_GuildCooperatePropertyPointLayout> areaLayoutList = _genAreaPointLayout(areaRef);
                _m_pointLayoutList.addAll(areaLayoutList);

                for (ServerObj_GuildCooperatePropertyPointLayout layout : areaLayoutList)
                {
                    _m_pointList.add(new GuildCooperatePointInfo(this, areaRef, layout));
                }
            }

            // 初始化据点解锁标识
            _initUnlockFlags();

            ServerObj_GuildCooperatePointLayoutList layoutListProto = new ServerObj_GuildCooperatePointLayoutList();
            layoutListProto.getLayoutList().addAll(_m_pointLayoutList);

            // 保存数据
            if (makeSureBoInsert())
            {
                // 记录已存在，执行增量更新
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("next_refresh_time_ms", _m_nextRefreshTimeMs);
                updateValue.addValueObj("reset_count", _m_resetCount);
                updateValue.addValueObj("point_layout_data", layoutListProto.makePackage().array());
                getUSServer().getBM().getBM(GuildCooperateMainBO.class).update("id", _m_dbId, updateValue);
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 随机生成指定区域的据点布局
     * <p>
     * 执行流程：
     * 1. 为指定区域的每个奖励据点随机生成属性据点布局
     * 2. 随机分配属性据点数量和属性类型
     * @param _areaRef 联盟协作区域配置
     * @return 该区域的据点布局列表
     */
    private List<ServerObj_GuildCooperatePropertyPointLayout> _genAreaPointLayout(RefGuildCooperateArea _areaRef)
    {
        List<ServerObj_GuildCooperatePropertyPointLayout> layoutList = new ArrayList<>();

        ArrayList<Long> posIdList = new ArrayList<>(_areaRef.pos_id_list);
        Collections.shuffle(posIdList);

        int posNum = Math.min(_areaRef.reward_point_num, posIdList.size());

        // 为每个奖励据点生成布局
        for (int rewardPointIndex = 0; rewardPointIndex < posNum; rewardPointIndex++)
        {
            ServerObj_GuildCooperatePropertyPointLayout layout = new ServerObj_GuildCooperatePropertyPointLayout();
            layout.setAreaId(_areaRef.area_id);
            layout.setRewardPointIndex(rewardPointIndex);
            layout.setPosId(posIdList.get(rewardPointIndex));

            // 随机生成属性据点数量（在配置范围内）
            int propertyPointCount = _areaRef.property_point_num_range.rand();

            // 生成均匀分布的属性类型列表
            List<ESpecAttrType> attrTypeList = _genPointAttrTypeList(propertyPointCount);

            // 添加到布局中
            for (ESpecAttrType attrType : attrTypeList)
            {
                layout.addAttrType(attrType);
            }

            layoutList.add(layout);
        }

        return layoutList;
    }

    /**
     * 生成均匀分布的属性类型列表
     * <p>
     * 算法逻辑：
     * 1. 当据点数量小于等于属性类型数量时，随机抽取不重复的属性
     * 2. 当据点数量大于属性类型数量时，先确保每种属性都有基础数量，剩余数量从属性列表中随机抽取（保证均匀分布）
     * 3. 最后打乱顺序保证随机性
     * @param _totalCount 总属性据点数量
     * @return 均匀分布且随机排序的属性类型列表
     *
     * 规则保证：
     * - 小于等于5个据点时，不会出现属性重复
     * - 等于6个据点时，4种属性各1个，1种属性2个（允许1个重复）
     * - 大于6个据点时，保持均匀分布，每种属性数量差距最多为1
     */
    private List<ESpecAttrType> _genPointAttrTypeList(int _totalCount)
    {
        if (_totalCount <= 0 || VALID_ATTR_TYPES.isEmpty())
        {
            return new ArrayList<>();
        }

        List<ESpecAttrType> result = new ArrayList<>();
        int attrTypeCount = VALID_ATTR_TYPES.size();

        if (_totalCount <= attrTypeCount)
        {
            // 据点数量小于等于属性类型数量：随机抽取不重复的属性
            List<ESpecAttrType> shuffled = new ArrayList<>(VALID_ATTR_TYPES);
            Collections.shuffle(shuffled);
            result.addAll(shuffled.subList(0, _totalCount));
        }
        else
        {
            // 据点数量大于属性类型数量：均匀分配
            int baseCount = _totalCount / attrTypeCount;      // 每种属性的基础数量
            int remainCount = _totalCount % attrTypeCount;    // 剩余数量

            // 先为每种属性分配基础数量
            for (int cycle = 0; cycle < baseCount; cycle++)
            {
                result.addAll(VALID_ATTR_TYPES);
            }

            // 剩余数量从属性列表中随机抽取（保证均匀，每种属性最多比其他多1个）
            if (remainCount > 0)
            {
                List<ESpecAttrType> shuffled = new ArrayList<>(VALID_ATTR_TYPES);
                Collections.shuffle(shuffled);
                result.addAll(shuffled.subList(0, remainCount));
            }

            // 打乱顺序确保随机性
            Collections.shuffle(result);
        }

        return result;
    }

    /**
     * 进攻属性据点
     * @param _pos          进攻据点位置
     * @param _attrIndex    进攻属性据点索引
     * @param _cid          进攻玩家ID
     * @param _addInfo      附加信息
     * @return Result
     */
    public ResultOne<Long> attack(GuildCooperate_RewardPointPos _pos, int _attrIndex, long _cid, GuildOp_043_AttackpropertyPointInfo _addInfo)
    {
        AttackResult result;
        GuildCooperatePointInfo pointInfo;
        List<GuildCooperate_RewardPointPos> unlockedPosList = null;

        RefHero heroRef = RefHero.getMgr().get(_addInfo.getHeroId());
        if(null == heroRef)
            return ResultOne.failed(CommErr.REF_NOT_FOUND);

        _lock();
        try{
            // 检查重置数据
            checkResetData(false);

            // 查找据点信息
            pointInfo = lookupPoint(_pos);
            if (pointInfo == null)
                return ResultOne.failed(GuildCooperateErr.REWARD_POINT_NOT_FOUND);

            // 进攻逻辑
            ResultOne<AttackResult> attackResult = pointInfo.attack(_addInfo.getPlayerName(), _attrIndex, heroRef.spec_attr_type, _addInfo.getPower());
            if (!attackResult.isSucc())
                return ResultOne.failed(attackResult.getResult());

            result = attackResult.getData();

            // 检查解锁区域
            if (result.isDefeat)
                unlockedPosList = checkAreaUnlock(_pos.getAreaId() + 1);

            // 更新公会协助伤害排行
            updateDamageRank(_cid, result.actualDamage);

        }finally
        {
            _unlock();
        }

        // 先推送属性据点信息变化
        getGuildInfo().getMemberMgr().broadcastMsg(
                US2GCWriter_032_GuildOp.make_076_OnPropertyPointInfoChange(_pos, result.newPropertyPointInfo));

        // 如果解锁了奖励据点，推送解锁消息
        if (result.isDefeat)
            getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_077_OnRewardPointDefeat(_pos, result.leaderCid));

        // 如果解锁了新区域，推送解锁消息
        if (unlockedPosList != null && !unlockedPosList.isEmpty())
            getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_080_OnRewardPointUnlock(unlockedPosList));

        return ResultOne.succ(result.actualDamage);
    }

    /**
     * 记录攻击日志
     * @param _playerName 攻击者姓名
     * @param _posId 据点ID
     * @param _targetAttrType 目标属性类型
     * @param _actualDamage 实际伤害
     */
    public void recordAttackLog(String _playerName, long _posId, ESpecAttrType _targetAttrType, long _actualDamage)
    {
        _lock();
        try
        {
            long nowTimeMS = CommonFunc.getNowTimeMS();

            // 记录攻击日志
            GuildCooperateAttackLogBO logBo = new GuildCooperateAttackLogBO();
            logBo.setGuildId(getUSServer().getBM(), getGuildInfo().getGuildId());
            logBo.setTimeMs(getUSServer().getBM(), nowTimeMS);
            logBo.setPlayerName(getUSServer().getBM(), _playerName);
            logBo.setPosId(getUSServer().getBM(), _posId);
            logBo.setAttrType(getUSServer().getBM(), _targetAttrType.ordinal());
            logBo.setAttackHp(getUSServer().getBM(), _actualDamage);
            logBo.insert(getUSServer().getBM());

            _m_logList.add(new GuildCooperateLogInfo(logBo));

            // 检查并删除过期日志
            _checkRemoveExpiredLog(nowTimeMS);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 更新伤害排行榜
     * 
     * @param _playerCid 攻击者CID
     * @param _actualDamage 实际伤害
     */
    public void updateDamageRank(long _playerCid, long _actualDamage)
    {
        _lock();
        try{
            if (_actualDamage <= 0 || _playerCid <= 0)
                return;

            long nowTimeMS = CommonFunc.getNowTimeMS();

            // 更新排行榜
            _m_damageRankList.updatePlayerDamage(_playerCid, _actualDamage, nowTimeMS);
        }finally
        {
            _unlock();
        }
    }

    /**
     * 检查并删除过期日志
     * @param _nowTimeMS
     */
    private void _checkRemoveExpiredLog(long _nowTimeMS)
    {
        _lock();
        try
        {
            // 限制检查频率，避免每次记录日志都检查
            if (_m_nextCheckLogDelTimeMs > _nowTimeMS)
                return;

            // 更新下次检查时间
            _m_nextCheckLogDelTimeMs = _nowTimeMS + 5 * 1000;

            // 检查日志数量是否超过上限
            int maxLogCount = RefGeneral.Ref().guild_cooperate_log_max_count;
            if (_m_logList.size() > maxLogCount)
            {
                // 计算需要删除的日志数量
                int deleteCount = _m_logList.size() - maxLogCount;

                // 获取最早的日志记录ID列表（按时间排序）
                List<Long> deleteLogIdList = new ArrayList<>();
                for (int i = 0; i < deleteCount; i++)
                {
                    if (i < _m_logList.size())
                    {
                        deleteLogIdList.add(_m_logList.get(i).getDbId());
                    }
                }

                // 从数据库批量删除最早的日志
                if (!deleteLogIdList.isEmpty())
                {
                    getUSServer().getBM().getBM(GuildCooperateAttackLogBO.class).delAllInList("id", deleteLogIdList);

                    // 从内存列表中移除最早的日志（创建新列表避免内存泄漏）
                    List<GuildCooperateLogInfo> newLogList = new ArrayList<>();
                    for (int i = deleteCount; i < _m_logList.size(); i++)
                    {
                        newLogList.add(_m_logList.get(i));
                    }
                    _m_logList = newLogList;
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查区域是否已解锁
     * @param _areaId 区域ID
     * @return true表示已解锁，false表示未解锁
     */
    private boolean checkAreaPointAllDefeat(long _areaId)
    {
        _lock();
        try
        {
            // 检查区域的所有奖励据点是否都已解锁
            for (GuildCooperatePointInfo pointInfo : _m_pointList)
            {
                if (pointInfo.getAreaId() == _areaId && !pointInfo.isDefeat())
                    return false;
            }
            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保BO插入数据库
     * @return true表示已存在，false表示刚插入
     */
    private boolean makeSureBoInsert()
    {
        _lock();
        try
        {
            if (_m_dbId != 0)
                return true;

            GuildCooperateMainBO mainBo = new GuildCooperateMainBO();
            mainBo.setGuildId(getUSServer().getBM(), getGuildInfo().getGuildId());
            mainBo.setNextRefreshTimeMs(getUSServer().getBM(), _m_nextRefreshTimeMs);
            mainBo.setResetCount(getUSServer().getBM(), _m_resetCount);
            mainBo.setRecommendAreaId(getUSServer().getBM(), _m_recommendAreaId);
            mainBo.setRecommendIndex(getUSServer().getBM(), _m_recommendPointIndex);

            ServerObj_GuildCooperatePointLayoutList layoutListProto = new ServerObj_GuildCooperatePointLayoutList();
            layoutListProto.getLayoutList().addAll(_m_pointLayoutList);
            mainBo.setPointLayoutData(getUSServer().getBM(), layoutListProto.makePackage().array());

            mainBo.insert(getUSServer().getBM());

            _m_dbId = mainBo.getId();

            return false;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置推荐奖励据点
     * <p>
     * 执行流程：
     * 1. 验证推荐据点是否存在
     * 2. 更新内存中的推荐据点信息
     * 3. 同步更新数据库记录
     * @param _posInfo     推荐据点信息
     * @return 设置结果
     */
    public Result setRecommendPoint(GuildCooperate_RewardPointPos _posInfo)
    {
        // 推荐据点为(0,0)表示取消推荐
        if (_posInfo.getAreaId() != 0 && _posInfo.getIndex() != 0)
        {
            // 验证推荐据点是否存在
            GuildCooperatePointInfo pointInfo = lookupPoint(_posInfo.getAreaId(), _posInfo.getIndex());
            if (pointInfo == null)
                return GuildCooperateErr.REWARD_POINT_NOT_FOUND;
        }

        _lock();
        try
        {
            // 更新内存中的推荐据点信息
            _m_recommendAreaId = _posInfo.getAreaId();
            _m_recommendPointIndex = _posInfo.getIndex();

            // 同步更新数据库
            if (makeSureBoInsert())
            {
                // 记录已存在，执行增量更新
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("recommend_area_id", _m_recommendAreaId);
                updateValue.addValueObj("recommend_index", _m_recommendPointIndex);
                getUSServer().getBM().getBM(GuildCooperateMainBO.class).update("id", _m_dbId, updateValue);
            }
        } finally
        {
            _unlock();
        }

        // 广播推荐据点变更
        getGuildInfo().getMemberMgr().broadcastMsg(US2GCWriter_032_GuildOp.make_075_OnRecommendRewardPointChange(_posInfo));

        return Result.SUCC;
    }

    /**
     * 构造攻击日志列表
     * 
     * 实现分页查询功能：
     * 1. _lastDbId=0时：从最新开始获取日志
     * 2. _lastDbId>0时：获取比该ID更早的日志（向前翻页）
     * 3. 从日志列表末尾开始倒序遍历，返回指定数量的日志
     * 4. 使用线程锁保证数据一致性
     * 
     * @param _lastDbId 上次获取的最小日志ID，0表示从最新开始，>0表示获取更早的记录
     * @param _num 需要获取的日志数量
     * @return 攻击日志协议对象列表，按时间倒序排列
     */
    public List<GuildCooperate_AttackLog> makeLogList(long _lastDbId, int _num)
    {
        List<GuildCooperate_AttackLog> list = new ArrayList<>();
        _lock();
        try
        {
            if (_lastDbId == 0)
            {
                // 首次查询，从最新的日志开始获取
                for (int i = _m_logList.size() - 1; i >= 0; --i)
                {
                    GuildCooperateLogInfo log = _m_logList.get(i);
                    list.add(log.makeProto());

                    if (list.size() >= _num)
                        break;
                }
            }
            else
            {
                // 翻页查询，获取比_lastDbId更早的日志
                for (int i = _m_logList.size() - 1; i >= 0; --i)
                {
                    GuildCooperateLogInfo log = _m_logList.get(i);
                    // 只获取比上次最小ID更早的日志
                    if (log.getDbId() < _lastDbId)
                    {
                        list.add(log.makeProto());

                        if (list.size() >= _num)
                            break;
                    }
                }
            }
        }
        finally
        {
            _unlock();
        }
        return list;
    }

    public void discard()
    {
        _lock();
        try{
            _m_logList.clear();
            _m_pointList.clear();
        }finally
        {
            _unlock();
        }

        getUSServer().getBM().getBM(GuildCooperateMainBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
        getUSServer().getBM().getBM(GuildCooperateAttackLogBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
        getUSServer().getBM().getBM(GuildCooperateDamageRankBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
        getUSServer().getBM().getBM(GuildCooperatePointInfoBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
    }

    /**
     * 构造协议对象
     * @return
     */
    public GuildCooperate_Info makeProto()
    {
        _lock();
        try{
            checkResetData(false);

            GuildCooperate_Info proto = new GuildCooperate_Info();
            proto.setNextRefreshTimeMs(_m_nextRefreshTimeMs);
            proto.setResetCount(_m_resetCount);
            proto.setRecommendPos(new GuildCooperate_RewardPointPos(_m_recommendAreaId, _m_recommendPointIndex));
            for (GuildCooperatePointInfo pointInfo : _m_pointList)
            {
                proto.addPointList(pointInfo.makeProto());
            }
            return proto;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 检查并解锁指定区域
     * @param _areaId
     */
    public List<GuildCooperate_RewardPointPos> checkAreaUnlock(long _areaId)
    {
        List<GuildCooperate_RewardPointPos> posList = new ArrayList<>();

        _lock();
        try
        {
            boolean isAllDefeat = checkAreaPointAllDefeat(_areaId - 1);
            if (!isAllDefeat)
                return posList;

            for (GuildCooperatePointInfo pointInfo : _m_pointList)
            {
                if (pointInfo.getAreaId() == _areaId)
                {
                    pointInfo.setIsUnlock(true);

                    posList.add(pointInfo.getPosInfo());
                }
            }
            return posList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 修改据点已被攻击的血量
     * @param _areaId
     * @param _index
     * @param _propertyPointIndex
     * @param _damage
     * @param _context
     * @return
     */
    public Result chgPropPointHadDamage(long _areaId, int _index, int _propertyPointIndex, long _damage, NPPlayerContext _context)
    {
        GuildCooperatePointInfo pointInfo;
        long leaderCid;
        List<GuildCooperate_RewardPointPos> unlockedPosList;
        GuildCooperate_RewardPointPos pos;

        _lock();
        try
        {
            pointInfo = lookupPoint(_areaId, _index);
            if (pointInfo == null)
                return GuildCooperateErr.REWARD_POINT_NOT_FOUND;

            Result result = pointInfo.chgPropPointHadDamage(_propertyPointIndex, _damage, _context);

            if (!result.isSucc())
                return result;

            // 获取据点信息用于推送
            pos = pointInfo.getPosInfo();
            leaderCid = pointInfo.isDefeat() ? getGuildInfo().getMemberMgr().getLeaderId() : 0;

            // 检查是否解锁下一区域
            unlockedPosList = checkAreaUnlock(_areaId + 1);
        } finally
        {
            _unlock();
        }

        // 推送奖励据点击败消息
        getGuildInfo().getMemberMgr().broadcastMsg(
                US2GCWriter_032_GuildOp.make_077_OnRewardPointDefeat(pos, leaderCid));

        // 如果解锁了新区域，推送解锁消息
        if (unlockedPosList != null && !unlockedPosList.isEmpty())
            getGuildInfo().getMemberMgr().broadcastMsg(
                    US2GCWriter_032_GuildOp.make_080_OnRewardPointUnlock(unlockedPosList));

        return Result.SUCC;
    }

    /**
     * 强制击败指定奖励据点（GM命令使用）
     *
     * 功能：直接设置奖励据点为击败状态，将所有属性据点伤害值设置为满血
     *
     * 执行流程：
     * 1. 查找指定奖励据点
     * 2. 调用据点的forceDefeat方法
     * 3. 推送奖励据点击败消息给所有公会成员
     * 4. 检查并解锁下一区域
     * 5. 推送区域解锁消息
     *
     * @param _areaId 区域ID
     * @param _index 奖励据点索引
     * @param _context 操作上下文
     * @return 操作结果
     */
    public Result forceDefeatRewardPoint(long _areaId, int _index, NPPlayerContext _context)
    {
        GuildCooperatePointInfo pointInfo;
        long leaderCid;
        List<GuildCooperate_RewardPointPos> unlockedPosList;
        GuildCooperate_RewardPointPos pos;

        _lock();
        try
        {
            pointInfo = lookupPoint(_areaId, _index);
            if (pointInfo == null)
                return GuildCooperateErr.REWARD_POINT_NOT_FOUND;

            // 执行强制击败
            Result result = pointInfo.forceDefeat(_context);
            if (!result.isSucc())
                return result;

            // 获取据点信息用于推送
            pos = pointInfo.getPosInfo();
            leaderCid = pointInfo.isDefeat() ? getGuildInfo().getMemberMgr().getLeaderId() : 0;

            // 检查是否解锁下一区域
            unlockedPosList = checkAreaUnlock(_areaId + 1);

        } finally
        {
            _unlock();
        }

        // 推送奖励据点击败消息
        getGuildInfo().getMemberMgr().broadcastMsg(
                US2GCWriter_032_GuildOp.make_077_OnRewardPointDefeat(pos, leaderCid));

        // 如果解锁了新区域，推送解锁消息
        if (unlockedPosList != null && !unlockedPosList.isEmpty())
            getGuildInfo().getMemberMgr().broadcastMsg(
                    US2GCWriter_032_GuildOp.make_080_OnRewardPointUnlock(unlockedPosList));

        return Result.SUCC;
    }
}