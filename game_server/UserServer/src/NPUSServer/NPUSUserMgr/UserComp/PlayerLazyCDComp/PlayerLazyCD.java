package NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp;


import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.ActivityEnum.EActivityState;
import GS2GC.p021_PlayerInfo.GS2GC_021_050_OnPlayerLazyCdChged;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.NPCommon_PlayerLazyCD;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.RefPlayerLazyCd;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CONSUME_LAZY_CD;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;
import USDB.Bo.PlayerLazyCdBO;

import java.util.List;

/**
 * 玩家懒惰CD（体力系统）管理类
 *
 * 主要功能：
 * 1. 管理单个CD类型的数值和恢复逻辑
 * 2. 支持自动恢复、手动增减、离线结算
 * 3. 处理属性变化对CD的影响
 * 4. 支持溢出CD的计算和消耗
 *
 * 核心机制：
 * - 时间驱动恢复：根据配置的CD时长自动恢复点数
 * - 增量计算：只在需要时计算，避免频繁更新
 * - 差额补偿：记录未满一个CD周期的时间差额，确保精确计算
 * - 上限控制：支持可超上限和不可超上限两种模式
 *
 * 线程安全：所有公开方法都通过getUserData().lockUser()实现玩家级别的线程安全
 */
public class PlayerLazyCD implements _IHandlerHolder
{
    // 所属CD组件
    private final PlayerLazyCDComponent _m_cdComp;

    // 数据库记录ID
    private long _m_lDbId;

    // 当前点数（如体力值）
    private int _m_iCount;
    // 点数最后一次变动的时间戳（毫秒）
    private long _m_lLastCalcTime;
    // 恢复下一点所需的剩余时间（毫秒）
    // 用于精确计算CD恢复，避免因时间差累积导致的误差
    private long _m_lFullGetNextCdRemainTimeMs;

    // 关联的活动实例ID
    private long _m_relativeActivityInstanceId;

    // CD配置对象（缓存）
    private RefPlayerLazyCd _m_ref;
    // 离线期间溢出的CD数量（超过上限的部分）
    private int _m_iOverflowCount;

    /**
     * 构造函数 - 从数据库BO对象加载CD数据
     *
     * @param _comp CD组件引用
     * @param _bo 数据库BO对象
     * @param _ref CD配置对象
     */
    public PlayerLazyCD(PlayerLazyCDComponent _comp, PlayerLazyCdBO _bo, RefPlayerLazyCd _ref)
    {
        _m_cdComp = _comp;
        _m_ref = _ref;
        _m_lDbId = _bo.getId();
        _m_lLastCalcTime = _bo.getLastCalcTime();
        _m_lFullGetNextCdRemainTimeMs = _bo.getFullGetNextCdRemainTimeMs();
        _m_iCount = _bo.getCount();
        _m_iOverflowCount = _bo.getOverflowCount();
        _m_relativeActivityInstanceId = _bo.getRelativeActivityInstanceId();
    }

    public long getDbId()
    {
        return _m_lDbId;
    }

    public long getLastCalcTime()
    {
        return _m_lLastCalcTime;
    }
    
    public long getFullGetNextCdRemainTimeMs()
    {
    	return _m_lFullGetNextCdRemainTimeMs;
    }

    public RefPlayerLazyCd getRef()
    {
        return _m_ref;
    }

    public PlayerLazyCDComponent getCdComp()
    {
        return _m_cdComp;
    }
    
    public NPUSUserData getUserData()
    {
    	return _m_cdComp.getUserData();
    }

    /*******
     * 返回CD类型Id
     * @return
     */
    public int getCdID()
    {
        return _m_ref.cd_id;
    }

    /**
     * 玩家属性变更事件处理
     *
     * 当玩家属性发生变化时，如果该属性影响CD系统（如上限、恢复速度等），
     * 则需要重新计算CD状态并通知客户端
     *
     * @param _propertyType 变更的属性类型
     */
    protected void _onPlayerPropertyChg(ENPPlayerPropertyType _propertyType)
    {
        // 监测是否有关联
        if (_checkHasCDAboutProperty(_propertyType))
        {
            // 重新计算CD（属性变化不额外给予点数）
            __applyCD();
            _sendChgInfo();
        }
    }

    /**
     * 判断属性变动是否会影响CD系统
     *
     * 检查变更的属性是否为以下三种CD相关属性：
     * 1. 最大上限增加属性
     * 2. 每次恢复数量增加属性
     * 3. 恢复时长增加属性
     *
     * @param _chgType 变更的属性类型
     * @return true=影响CD，false=不影响
     */
    private boolean _checkHasCDAboutProperty(ENPPlayerPropertyType _chgType)
    {
        if (_chgType == getRef().property_add_count_max
                || _chgType == getRef().property_add_count_per_time
                || _chgType == getRef().property_add_durantion)
        {
            return true;
        }

        return false;
    }

    /**
     * 生成客户端协议对象
     *
     * 执行流程：
     * 1. 应用CD恢复逻辑，更新到最新状态
     * 2. 构造协议对象
     * 3. 填充所有CD相关数据
     *
     * @return CD协议对象
     */
    public NPCommon_PlayerLazyCD makeProto()
    {
        __applyCD();

        NPCommon_PlayerLazyCD proto = new NPCommon_PlayerLazyCD();
        proto.setLastCalTimeMS(getLastCalcTime());
        proto.setCount(_m_iCount);
        proto.setCdId(getCdID());
        proto.setMaxCount(getMaxCount());
        proto.setAddCountPerTime(getAddCountPerTime());
        proto.setCdDurationMs((int) getDurationMs());
        proto.setFullGetNextCdRemainTimeMs(_m_lFullGetNextCdRemainTimeMs);
        proto.setRelativeActivityInstanceId(_m_relativeActivityInstanceId);
        return proto;
    }

    /**
     * 获取CD恢复时长（毫秒）
     *
     * 计算方式：(配置时长 + 属性加成) * 1000
     * 最小值：1000毫秒（1秒）
     *
     * @param ref CD配置对象
     * @return CD时长（毫秒）
     */
    public long getDurationMs(RefPlayerLazyCd ref)
    {
        long value = 1000L * (ref.durantion + getUserData().getPlayerComponent().getPropertyMgr().getValue(ref.property_add_durantion));
        return Math.max(value, 1000L);
    }

    /**
     * 获取当前CD恢复时长（毫秒）
     *
     * @return CD时长（毫秒）
     */
    public long getDurationMs()
    {
        return getDurationMs(getRef());
    }

    /**
     * 获取每次恢复的点数
     *
     * 计算方式：配置值 + 属性加成
     * 最小值：1（确保至少恢复1点）
     *
     * @return 每次恢复点数
     */
    private int getAddCountPerTime()
    {
        int incCount = (int) (getRef().add_count_per_time + getUserData().getPlayerComponent().getPropertyMgr().getValue(getRef().property_add_count_per_time));
        if (incCount == 0)
            incCount = 1;
        return incCount;
    }

    /**
     * 获取当前最大上限
     *
     * 计算方式：配置最大值 + 属性加成
     *
     * @return 最大上限值
     */
    public int getMaxCount()
    {
        long addMaxCount = getUserData().getPlayerComponent().getPropertyMgr().getValue(getRef().property_add_count_max);
        return calMaxCount(getRef(), addMaxCount);
    }

    /**
     * 计算上限值（静态方法）
     *
     * @param ref CD配置对象
     * @param _addMaxCount 属性加成值
     * @return 最大上限值
     */
    public static int calMaxCount(RefPlayerLazyCd ref, long _addMaxCount)
    {
        return calMaxCount(false, ref, _addMaxCount);
    }

    /**
     * 计算上限值（支持初始化模式）
     *
     * @param _bInit true=初始化计算，false=正常计算
     * @param ref CD配置对象
     * @param _addMaxCount 属性加成值
     * @return 最大上限值
     */
    public static int calMaxCount(boolean _bInit, RefPlayerLazyCd ref, long _addMaxCount)
    {
        if (_bInit)
        {
            return ref.calInitCount(_addMaxCount);
        } else
        {
            return (int) (ref.count_max + _addMaxCount);
        }
    }

    /**
     * 外部增加CD点数（如使用道具）
     *
     * 特点：
     * 1. 根据配置决定是否可超上限
     * 2. 不改变CD恢复时间，但会补偿时间差额
     * 3. 更新时间为当前时间
     *
     * 执行流程：
     * 1. 应用CD自动恢复
     * 2. 检查上限限制
     * 3. 计算最终值并更新
     * 4. 补偿时间差额（如果未达上限）
     * 5. 保存数据并通知客户端
     *
     * @param _iValue 增加的点数
     * @return 实际增加的点数
     *
     * 线程安全：通过玩家锁保护
     */
    public int addCountExt(int _iValue)
    {
    	getUserData().lockUser();
    	
    	try
    	{
            if (_iValue <= 0)
                return 0;

            // 应用CD自动恢复
            __applyCD();

            long oriLastCalcTime = _m_lLastCalcTime;
            int oriCount = _m_iCount;

            // 达到上限且不能超过，退出不做处理
            int maxCount = getMaxCount();
            if (oriCount >= maxCount && !getRef().is_can_excceed_limit)
    	        return 0;

            int finalCount = oriCount + _iValue;
            // 针对不能超过上限的数据，重新计算最终值
            if (finalCount > maxCount && !getRef().is_can_excceed_limit)
            {
            	finalCount = maxCount;
            }

            _m_iCount = finalCount;
            _m_lLastCalcTime = CommonFunc.getNowTimeMS();
            // 对原值尚未到达上限的数据，补上差额
            // 这样可以确保CD恢复时间的连续性
            if(oriCount < maxCount)
            {
            	_m_lFullGetNextCdRemainTimeMs += _m_lLastCalcTime - oriLastCalcTime;
            	// 避免出现异常情况，对数据进行保护
            	long durationMs = getDurationMs();
            	if(_m_lFullGetNextCdRemainTimeMs > durationMs)
            	{
            		USLog.error(getUserData().getUSServer(), "player:{} cd:{} addCountExt._m_lFullGetNextCdRemainTimeMs cal error.", getUserData().getCid(), getCdID());
            		_m_lFullGetNextCdRemainTimeMs = durationMs;
            	}
            }
            // 保存数据
            __saveData();

            // 发送协议通知客户端
            _sendChgInfo();

            return finalCount - oriCount;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 减少CD点数（如消耗体力）
     *
     * 执行流程：
     * 1. 应用CD自动恢复
     * 2. 计算减少后的值（最小为0）
     * 3. 更新时间为当前时间
     * 4. 补偿时间差额（如果未达上限）
     * 5. 保存数据并通知客户端
     *
     * @param _iValue 减少的点数
     * @param _context 操作上下文
     *
     * 线程安全：通过玩家锁保护
     */
    public void descCount(int _iValue, NPPlayerContext _context)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		if (_iValue <= 0)
                return;

            // 应用CD自动恢复
            __applyCD();

            int finalCount = _m_iCount - _iValue;
            if (finalCount < 0)
            	finalCount = 0;

            // 数据没有变化，不做处理
            if(_m_iCount == finalCount)
            	return;

            long oriCount = _m_iCount;
            long maxCount = getMaxCount();
            long oriLastCalcTime = _m_lLastCalcTime;

            _m_iCount = finalCount;
            _m_lLastCalcTime = CommonFunc.getNowTimeMS();
            // 对原值尚未到达上限的数据，补上差额
            // 这样可以确保CD恢复时间的连续性
            if(oriCount < maxCount)
            {
            	_m_lFullGetNextCdRemainTimeMs += _m_lLastCalcTime - oriLastCalcTime;
            	// 避免出现异常情况，对数据进行保护
            	long durationMs = getDurationMs();
            	if(_m_lFullGetNextCdRemainTimeMs > durationMs)
            	{
            		USLog.error(getUserData().getUSServer(), "player:{} cd:{} descCount._m_lFullGetNextCdRemainTimeMs cal error.", getUserData().getCid(), getCdID());
            		_m_lFullGetNextCdRemainTimeMs = durationMs;
            	}
            }
            // 保存数据
            __saveData();

            // 发送协议通知客户端
            _sendChgInfo();

            getUserData().onLogicEvent(new Event_P_CONSUME_LAZY_CD(_context, getCdID(), _iValue));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 获取当前CD点数
     *
     * 会先应用自动恢复逻辑，返回最新的点数
     *
     * @return 当前点数
     *
     * 线程安全：通过玩家锁保护
     */
    public int getCount()
    {
    	getUserData().lockUser();

    	try
    	{
            // 应用CD恢复
            __applyCD();

            return _m_iCount;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }


    /**
     * 发送CD变化通知给客户端
     *
     * 构造协议对象并通过网络发送给客户端
     */
    private void _sendChgInfo()
    {
        GS2GC_021_050_OnPlayerLazyCdChged proto = new GS2GC_021_050_OnPlayerLazyCdChged();
        proto.setCdInfo(makeProto());
        getUserData().sendMsgToGC(proto);
    }

    /**
     * 应用CD自动恢复逻辑（核心方法）
     *
     * 执行流程：
     * 1. 检查关联活动状态
     *    - 如果已关联活动且活动已关闭，清空关联ID
     *    - 如果未关联活动且配置了活动ID，则查找当前正在进行的活动并自动关联
     *    - 活动开启时，设置上次结算时间为活动开启时间
     * 2. 检查数据合法性（CD时长必须大于0）
     * 3. 如果已达上限，不做处理
     * 4. 计算经过的时间和应该恢复的点数
     * 5. 如果不满一个CD周期，不做处理
     * 6. 计算新的点数和剩余时间差额
     * 7. 保存数据并记录日志
     *
     * 核心算法：
     * - spanMs = 当前时间 - 上次计算时间 + 上次剩余时间
     * - addCount = spanMs / CD时长（向下取整）
     * - 剩余时间 = spanMs % CD时长（用于下次计算）
     *
     * 注意：此方法假设已在玩家锁保护下调用
     */
    private void __applyCD()
    {
        boolean needSave = false;

        // 检查关联活动状态
        if (_m_relativeActivityInstanceId != 0)
        {
            // 已关联活动，检查活动是否已关闭
            _AActivityBase activity = getUserData().getUSServer().getCommActivityMgr().lookupActivity(_m_relativeActivityInstanceId);
            // 活动不存在或已关闭
            if (activity == null || activity.getCurState().getStateType() == EActivityState.CLOSED
                || activity.getCurState().getStateType() == EActivityState.CAN_DISCARD)
            {
                // 活动关闭，清空关联活动ID（不重置CD数值）
                _m_relativeActivityInstanceId = 0;
                needSave = true;
            }
        }

        if (_m_ref.relative_activity_id != 0 && _m_relativeActivityInstanceId == 0)
        {
            // 未关联活动，尝试查找当前正在进行的活动
            List<_AActivityBase> activityList = getUserData().getUSServer().getCommActivityMgr().getAllActivity();
            for (_AActivityBase activity : activityList)
            {
                if (activity != null
                    && activity.getActivityId() == _m_ref.relative_activity_id
                    && activity.isRunning())
                {
                    // 找到正在进行的活动，自动关联
                    _m_relativeActivityInstanceId = activity.getInstanceId();
                    // 设置上次结算时间为活动开启时间
                    _m_lLastCalcTime = activity.getStartTimeMs();
                    // 重置当前计数为初始值
                    _m_iCount = _m_ref.init_count == 0 ? getMaxCount() : _m_ref.init_count;
                    // 重置剩余时间差额
                    _m_lFullGetNextCdRemainTimeMs = 0;
                    needSave = true;
                    break;
                }
            }
        }

        // 如果有活动状态变化，保存数据
        if (needSave)
        {
            __saveData();
        }

        // 检查数据合法性
        long durationMs = getDurationMs();
        if(durationMs <= 0)
        {
        	USLog.error(getUserData().getUSServer(), "player:{} cd:{} duration:{} get duration error.", getUserData().getCid(), getCdID(), durationMs);
        	return;
        }

        int oriCount = _m_iCount;
        int maxCount = getMaxCount();
        // 达到上限不做处理
        if (oriCount >= maxCount)
        	return;

        long nowTimeMs = CommonFunc.getNowTimeMS();
        long lastCalcTimeMs = getLastCalcTime();
        long spanMs = nowTimeMs - lastCalcTimeMs + _m_lFullGetNextCdRemainTimeMs;
        // 不满一个回合的不做处理
        if(spanMs < durationMs)
        	return;

    	long addCount = spanMs / durationMs;
        int newCount = (int) (oriCount + addCount);

        // 超过最大值，无需计算差额
        if(newCount >= maxCount)
        {
        	_m_lFullGetNextCdRemainTimeMs = 0;
        	newCount = maxCount;
        }
        else // 没有超过最大值，需要计算差额
        {
        	_m_lFullGetNextCdRemainTimeMs = spanMs % durationMs;
        }

        // count数据发生变更，需要再次更新最后一次触发时间
        _m_lLastCalcTime = nowTimeMs;
    	_m_iCount = newCount;

        // 保存数据
        __saveData();

        NPPlayerContext _context = NPPlayerContext.createNew(ENPGameEvent.LAZY_CD_APPLY);
        getCdComp().logItem(_m_ref.cd_id, oriCount, newCount, ELogItem_Type.AUTO_RECOVER, _context);
    }

    /**
     * 保存CD数据到数据库
     *
     * 使用增量更新方式，只更新变化的字段
     */
    private void __saveData()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("last_calc_time", _m_lLastCalcTime);
        updateValue.addValueObj("count", _m_iCount);
        updateValue.addValueObj("full_get_next_cd_remain_time_ms", _m_lFullGetNextCdRemainTimeMs);
        updateValue.addValueObj("overflow_count", _m_iOverflowCount);
        updateValue.addValueObj("relative_activity_instance_id", _m_relativeActivityInstanceId);
        _m_cdComp.getUSServer().getBM().getBM(PlayerLazyCdBO.class).update("id", _m_lDbId, updateValue);
    }

    /**
     * 玩家上线时进行离线结算
     *
     * 简单应用CD恢复逻辑，将点数恢复到当前时间应有的状态
     *
     * 线程安全：通过玩家锁保护
     */
    public void doOfflineSettle()
    {
    	getUserData().lockUser();

    	try
    	{
    		__applyCD();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 计算离线期间的溢出CD（用于特殊活动如周卡）
     *
     * 执行流程：
     * 1. 计算周卡生效期间的CD恢复（可溢出）
     * 2. 计算周卡结束到当前时间的CD恢复（不可溢出）
     * 3. 如果恢复的点数超过上限，将超出部分记录到溢出CD
     *
     * @param _endSettleTimeMs 周卡等特殊活动结束时间（毫秒）
     * @return 是否有数据变更
     *
     * 线程安全：通过玩家锁保护
     */
    public boolean calculateOfflineOverflowCd(long _endSettleTimeMs)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		boolean isChg = false;
            int maxCount = getMaxCount();
            int addCountPerTime = getAddCountPerTime();
            long durationMs = getDurationMs();

            // 第一阶段：计算周卡生效期间，这部分可溢出
            {
                long spanMs = _endSettleTimeMs - _m_lLastCalcTime;
                if (spanMs >= durationMs)
                {
                    int times = (int) (spanMs / durationMs);
                    int finalCount = times * addCountPerTime + _m_iCount;

                    // 如果会溢出
                    if (finalCount > maxCount)
                    {
                        // 计算溢出数值
                        _m_iOverflowCount = finalCount - maxCount;
                        // 把最终值设置为最大值
                        finalCount = maxCount;
                    }

                    _m_lLastCalcTime += times * durationMs;
                    _m_iCount = finalCount;
                    isChg = true;
                }
            }

            // 第二阶段：结算到当前时间（不可溢出）
            {
                long spanMs = CommonFunc.getNowTimeMS() - _m_lLastCalcTime;
                if (spanMs >= durationMs)
                {
                    int times = (int) (spanMs / durationMs);
                    int finalCount = times * addCountPerTime + _m_iCount;

                    // 超过上限但是无法超过，只能取上限值
                    if (finalCount >= maxCount)
                    {
                        finalCount = maxCount;

                        _m_lLastCalcTime = CommonFunc.getNowTimeMS();
                    } else
                    {
                        _m_lLastCalcTime = times * durationMs;
                    }

                    _m_iCount = finalCount;
                    isChg = true;
                }
            }

            if (isChg)
                __saveData();

            return isChg;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 使用离线期间的溢出CD
     *
     * 执行流程：
     * 1. 计算离线期间的CD变化（包括溢出）
     * 2. 消耗离线期间累积的CD
     * 3. 如果有变化，通知客户端
     *
     * @param _endSettleTimeMs 特殊活动结束时间
     * @param _isLazy true=懒惰模式（只消耗溢出部分），false=全部消耗
     * @return 消耗的溢出CD数量
     *
     * 线程安全：通过玩家锁保护
     */
    public int useOfflineCd(long _endSettleTimeMs, boolean _isLazy)
    {
    	getUserData().lockUser();

    	try
    	{
            // 计算离线期间的CD变化
            boolean hasChg = calculateOfflineOverflowCd(_endSettleTimeMs);
            // 消耗离线期间的CD
            int overflowCount = _consumeOfflineCd(_isLazy);
            // 如果有相关数据的变化就需要推送
            if (hasChg || overflowCount > 0)
            {
                _sendChgInfo();
            }
            return overflowCount;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 消耗离线CD的内部逻辑
     *
     * @param _isLazy true=懒惰模式（只消耗溢出部分），false=全部消耗
     * @return 消耗的溢出CD总数
     */
    private int _consumeOfflineCd(boolean _isLazy)
    {
        int overflowCount = _m_iOverflowCount;
        _m_iOverflowCount = 0;
        if (!_isLazy)
        {
            // 非懒惰模式：消耗所有点数
            overflowCount += _m_iCount;
            _m_iCount = 0;
        } else
        {
            // 懒惰模式：只消耗超过上限的部分
            // 如果有道具溢出的CD值，也需要处理
            int maxCount = getMaxCount();
            if (_m_iCount > maxCount)
            {
                overflowCount += _m_iCount - maxCount;
                _m_iCount = maxCount;
            }
        }
        __saveData();
        return overflowCount;
    }

    /**
     * GM命令：设置恢复下一点CD的时间
     *
     * @param _sec 多少秒后恢复下一点（相对于CD时长）
     * @return 是否设置成功
     *
     * 线程安全：通过玩家锁保护
     */
    public boolean cmdSetTimeToRecoverNextCd(int _sec)
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		__applyCD();

            _m_lLastCalcTime = CommonFunc.getNowTimeMS();
            _m_lFullGetNextCdRemainTimeMs = Math.max(getDurationMs() - _sec * 1000, 0);
            __saveData();

            _sendChgInfo();

            return true;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * GM命令：直接设置CD时间（用于测试）
     *
     * 警告：仅用于测试和调试，不校验数据合法性
     *
     * @param _lastCalTime 上次计算时间
     * @param _remainSecs 剩余秒数
     *
     * 线程安全：通过玩家锁保护
     */
    public void cmdTest(long _lastCalTime, int _remainSecs)
    {
    	getUserData().lockUser();

    	try
    	{
    		__applyCD();

    		_m_lLastCalcTime = _lastCalTime;
    		_m_lFullGetNextCdRemainTimeMs = _remainSecs * 1000;
    		__saveData();

    		_sendChgInfo();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 生成CD状态的字符串表示（用于日志和调试）
     *
     * 输出信息包括：
     * - CD类型ID
     * - 当前点数
     * - 恢复下一点的剩余秒数
     * - 最大上限
     * - CD时长
     * - 上次计算时间
     * - 剩余时间差额
     *
     * @return CD状态字符串
     *
     * 线程安全：通过玩家锁保护
     */
    @Override
    public String toString()
    {
    	getUserData().lockUser();

    	try
    	{
    		// 计算此时恢复点数剩余秒数
    		long realDuration = CommonFunc.getNowTimeMS() - getLastCalcTime() + getFullGetNextCdRemainTimeMs();
    		long remainMs = Math.max(getDurationMs() - realDuration, 0);
    		int remainSecs =  (int) (Math.ceil(remainMs / 1000));

    		StringBuilder sb = new StringBuilder();

    		sb.append("cd:").append(getCdID())
    			.append(", count:").append(getCount())
    			.append(", remainSec:").append(remainSecs)
    			.append(", maxCount:").append(getMaxCount())
    			.append(", duration:").append(getDurationMs())
    			.append(", lastCalTime:").append(CommonFunc.getTimeStringMs(getLastCalcTime()))
    			.append(", fullGetNextCdRemainTimeMs:").append(getFullGetNextCdRemainTimeMs());

    		return sb.toString();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
}
