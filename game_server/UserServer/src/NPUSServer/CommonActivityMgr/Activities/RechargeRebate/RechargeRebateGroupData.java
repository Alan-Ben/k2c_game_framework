package NPUSServer.CommonActivityMgr.Activities.RechargeRebate;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.RechargeRebateObj.RechargeRebate_GroupInfo;
import GS2GC.p033_SimpleActivityOp.GS2GC_033_109_OnRechargeRebateCountChg;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.PlayerErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RechargeRebate.RefRechargeRebateGroup;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.RechargeRebatePlayerInfoBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 充值返利玩家组数据
 *
 * 管理单个玩家在特定返利组中的数据，包括计数、已领取档位等
 *
 * 设计特点：
 * - 缓存数据库ID，使用增量更新优化性能
 * - 懒加载数据库插入，首次需要时才创建记录
 * - 线程安全的数据访问
 */
public class RechargeRebateGroupData
{
    // 持有返利信息管理器的引用
    private _ARechargeRebateInfo _m_info;

    // 配置引用
    private RefRechargeRebateGroup _m_ref;

    // 数据库记录ID，0表示未创建
    private long _m_dbId;

    // 当前计数（VIP点数或充值天数）
    private long _m_count;

    // 最后充值日期（YYYYMMDD格式）
    private int _m_lastRechargeDate;

    // 已领取的档位ID列表
    private List<Long> _m_hadDrawStepList;

    // 线程安全锁
    private MutexAtom _m_mutex;

    /**
     * 从配置构造（新建记录）
     */
    public RechargeRebateGroupData(_ARechargeRebateInfo _info, RefRechargeRebateGroup _ref)
    {
        _m_info = _info;
        _m_ref = _ref;
        _m_dbId = 0;
        _m_count = 0;
        _m_lastRechargeDate = 0;
        _m_hadDrawStepList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    /**
     * 从BO对象构造（加载已有记录）
     */
    public RechargeRebateGroupData(_ARechargeRebateInfo _info, RefRechargeRebateGroup _ref, RechargeRebatePlayerInfoBO _bo)
    {
        this(_info, _ref);
        _m_dbId = _bo.getId();
        _m_count = _bo.getCount();
        _m_lastRechargeDate = _bo.getLastRechargeDate();

        // 解析已领取档位ID列表
        _m_hadDrawStepList.addAll(CommonFunc.listLongFromString(_bo.getHadDrawStepList()));
    }

    private void _lock()
    {
        _m_mutex.lock();
    }

    private void _unlock()
    {
        _m_mutex.unlock();
    }

    /**
     * 获取BM对象
     */
    public BM getBM()
    {
        return _m_info.getUSServer().getBM();
    }

    /**
     * 获取组ID
     */
    public long getGroupId()
    {
        return _m_ref.id;
    }

    /**
     * 获取当前计数
     */
    public long getCount()
    {
        _lock();
        try
        {
            return _m_count;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 获取最后充值日期
     */
    public int getLastRechargeDate()
    {
        _lock();
        try
        {
            return _m_lastRechargeDate;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 检查档位是否已领取
     */
    public boolean hadDrawStep(long _stepId)
    {
        _lock();
        try
        {
            return _m_hadDrawStepList.contains(_stepId);
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 尝试创建数据库记录
     *
     * @return true=新创建，false=已存在
     */
    private boolean tryCreateInDB(long _cid)
    {
        _lock();
        try{
            if (_m_dbId != 0)
                return false;

            RechargeRebatePlayerInfoBO bo = new RechargeRebatePlayerInfoBO();
            bo.setActivityInstanceId(getBM(), _m_info.getActivity().getInstanceId());
            bo.setCid(getBM(), _cid);
            bo.setGroupId(getBM(), _m_ref.id);
            bo.setCount(getBM(), _m_count);
            bo.setLastRechargeDate(getBM(), _m_lastRechargeDate);
            bo.setHadDrawStepList(getBM(), CommonFunc.list2String(_m_hadDrawStepList));
            bo.insert(getBM());

            _m_dbId = bo.getId();
            return true;
        }finally
        {
            _unlock();
        }
    }

    /**
     * 增加计数
     */
    public void addCount(long _cid, long _addValue)
    {
        _lock();
        try
        {
            _m_count += _addValue;

            if (!tryCreateInDB(_cid))
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("count", _m_count);
                getBM().getBM(RechargeRebatePlayerInfoBO.class).update("id", _m_dbId, updateValue);
            }

            // 推送计数变化
            ALSynTaskManager.getInstance().regTask(() -> {
                NPUSUserData userData = _m_info.getUSServer().getUsUserMgr().lookupCacheUserData(_cid);
                if (userData != null)
                {
                    GS2GC_033_109_OnRechargeRebateCountChg proto = new GS2GC_033_109_OnRechargeRebateCountChg();
                    proto.setActivityInstanceId(_m_info.getActivity().getInstanceId());
                    proto.setGroupId(getGroupId());
                    proto.setCount(getCount());
                    userData.sendMsgToGC(proto);
                }
            });
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 设置计数
     */
    public void setCount(long _cid, long _newCount)
    {
        _lock();
        try
        {
            _m_count = _newCount;

            if (!tryCreateInDB(_cid))
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("count", _m_count);
                getBM().getBM(RechargeRebatePlayerInfoBO.class).update("id", _m_dbId, updateValue);
            }

            // 推送计数变化
            ALSynTaskManager.getInstance().regTask(() -> {
                NPUSUserData userData = _m_info.getUSServer().getUsUserMgr().lookupCacheUserData(_cid);
                if (userData != null)
                {
                    GS2GC_033_109_OnRechargeRebateCountChg proto = new GS2GC_033_109_OnRechargeRebateCountChg();
                    proto.setActivityInstanceId(_m_info.getActivity().getInstanceId());
                    proto.setGroupId(getGroupId());
                    proto.setCount(getCount());
                    userData.sendMsgToGC(proto);
                }
            });
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 更新最后充值日期
     */
    public void addCountAndUpdateLastRechargeDate(long _cid, long _addValue, int _date)
    {

        _lock();
        try
        {
            _m_count += _addValue;
            _m_lastRechargeDate = _date;

            if (!tryCreateInDB(_cid))
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("count", _m_count);
                updateValue.addValueObj("last_recharge_date", _m_lastRechargeDate);
                getBM().getBM(RechargeRebatePlayerInfoBO.class).update("id", _m_dbId, updateValue);
            }

            // 推送计数变化
            ALSynTaskManager.getInstance().regTask(() -> {
                NPUSUserData userData = _m_info.getUSServer().getUsUserMgr().lookupCacheUserData(_cid);
                if (userData != null)
                {
                    GS2GC_033_109_OnRechargeRebateCountChg proto = new GS2GC_033_109_OnRechargeRebateCountChg();
                    proto.setActivityInstanceId(_m_info.getActivity().getInstanceId());
                    proto.setGroupId(getGroupId());
                    proto.setCount(getCount());
                    userData.sendMsgToGC(proto);
                }
            });
        } finally
        {
            _unlock();
        }
    }

    /**
     * 记录领取档位
     */
    public Result recordDrawStep(long _cid, long _stepId)
    {
        _lock();
        try
        {
            if (_m_hadDrawStepList.contains(_stepId))
                return PlayerErr.RECHARGE_REBATE_ALREADY_DRAW;

            _m_hadDrawStepList.add(_stepId);

            if (!tryCreateInDB(_cid))
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("had_draw_step_list", CommonFunc.list2String(_m_hadDrawStepList));
                getBM().getBM(RechargeRebatePlayerInfoBO.class).update("id", _m_dbId, updateValue);
            }

            return Result.SUCC;
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 清空已领取档位列表（用于每日重置）
     */
    public void clearCountAndDrawStepList(long _cid)
    {
        _lock();
        try
        {
            _m_count = 0;
            _m_hadDrawStepList.clear();

            if (!tryCreateInDB(_cid))
            {
                ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
                updateValue.addValueObj("had_draw_step_list", "");
                updateValue.addValueObj("count", _m_count);
                getBM().getBM(RechargeRebatePlayerInfoBO.class).update("id", _m_dbId, updateValue);
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 填充协议对象
     */
    public RechargeRebate_GroupInfo makeProto()
    {
        _lock();
        try
        {
            RechargeRebate_GroupInfo proto = new RechargeRebate_GroupInfo();
            proto.setGroupId(_m_ref.id);
            proto.setCount(_m_count);

            for (Long stepId : _m_hadDrawStepList)
            {
                proto.addHadDrawStepList(stepId);
            }

            return proto;
        }
        finally
        {
            _unlock();
        }
    }
}
