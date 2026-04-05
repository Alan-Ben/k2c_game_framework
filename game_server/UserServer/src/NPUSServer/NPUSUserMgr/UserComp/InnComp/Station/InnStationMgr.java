package NPUSServer.NPUSUserMgr.UserComp.InnComp.Station;

import Common.InnObj.Inn_StationInfo;
import MJLog.MJEventLog;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.InnErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.Inn.RefInnStation;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.InnComp.InnComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_034_InnOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerInnStationBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 旅店设施管理器
 */
public class InnStationMgr
{
    private final InnComponent _m_innComp;
    private final List<InnStationInfo> _m_stationList;

    public InnStationMgr(InnComponent innComp)
    {
        _m_innComp = innComp;
        _m_stationList = new ArrayList<>();
    }

    /**
     * 获取旅店组件
     */
    public InnComponent getComp()
    {
        return _m_innComp;
    }

    protected void _lock()
    {
        _m_innComp.getUserData().lockUser();
    }

    protected void _unlock()
    {
        _m_innComp.getUserData().unlockUser();
    }

    /**
     * 初始化设施数据
     */
    public void init(final _ICallBackBool _callBack)
    {
        _m_innComp.getUSServer().getBM().getBM(PlayerInnStationBO.class).findAll("cid", _m_innComp.getUserData().getCid(),
                new _ASelectCallback<List<PlayerInnStationBO>>()
                {
                    @Override
                    public void dealSuc(List<PlayerInnStationBO> boList)
                    {
                        for (PlayerInnStationBO bo : boList)
                        {
                            if (bo != null)
                            {
                                // 检查配表是否存在
                                RefInnStation refStation = RefInnStation.getMgr().get(bo.getStationId());
                                if (refStation == null)
                                {
                                    USLog.error(_m_innComp.getUSServer(), "Player {} inn station {} config not found, skip loading",
                                            _m_innComp.getUserData().getCid(), bo.getStationId());
                                    continue;
                                }

                                InnStationInfo info = new InnStationInfo(InnStationMgr.this, bo, refStation);
                                _m_stationList.add(info);
                            }
                        }
                        _callBack.onRunOver(true);
                    }

                    @Override
                    public void dealFail()
                    {
                        USLog.error(_m_innComp.getUSServer(), "Player {} inn station data load failed", _m_innComp.getUserData().getCid());
                        _callBack.onRunOver(false);
                    }
                });
    }

    /**
     * 根据设施ID获取设施信息
     */
    public InnStationInfo lookupStation(long stationId)
    {
        _lock();
        try
        {
            for (InnStationInfo info : _m_stationList)
            {
                if (info.getStationId() == stationId)
                {
                    return info;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 建造设施
     * @param _stationId 设施ID
     * @return Result
     */
    public Result buildStation(long _stationId, NPPlayerContext _context)
    {
        _lock();
        try
        {
            if (lookupStation(_stationId) != null)
                return InnErr.INN_STATION_ALREADY_BUILT;

            RefInnStation refInnStation = RefInnStation.getMgr().get(_stationId);
            if (refInnStation == null)
                return CommErr.REF_NOT_FOUND;

            // 检查接待人数是否满足
            if (getComp().getHadReceiveGuestNum() < refInnStation.need_receive_guest_num)
                return InnErr.INN_RECEIVE_GUEST_NOT_ENOUGH;

            // 检查道具是否足够
            if (!getComp().getUserData().hasCostItemList(refInnStation.build_cost))
                return CommErr.ITEM_NOT_ENOUGH;

            // 扣除消耗物品
            if (!getComp().getUserData().spendCostItemList(refInnStation.build_cost, _context))
                return CommErr.CONSUME_FAIL;

            // 记录日志
            Result result = build(_stationId, refInnStation);
            if (result.isSucc())
                MJEventLog.logInnUnlock(getComp().getUserData(), _stationId);

            return result;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 建造
     * @param _stationId
     * @param refInnStation
     * @return
     */
    public Result build(long _stationId, RefInnStation refInnStation)
    {
        _lock();
        try
        {
            if (lookupStation(_stationId) != null)
                return InnErr.INN_STATION_ALREADY_BUILT;

            // 创建新的设施信息
            PlayerInnStationBO newStationBO = new PlayerInnStationBO();
            newStationBO.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
            newStationBO.setStationId(getComp().getUSServer().getBM(), _stationId);
            newStationBO.setLevel(getComp().getUSServer().getBM(), 1);  // 默认等级1
            newStationBO.insert(getComp().getUSServer().getBM());

            // 创建设施信息对象
            InnStationInfo newStationInfo = new InnStationInfo(this, newStationBO, refInnStation);
            _m_stationList.add(newStationInfo);

            // 推送到客户端
            getComp().getUserData().sendMsgToGC(US2GCWriter_034_InnOp.make_056_OnInnStationAdd(newStationInfo.makeProto()));

            return Result.SUCC;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查并创建初始默认设施
     * 注意：初始化阶段不推送消息，完整数据通过makeProto下发
     */
    public void checkDefaultStations()
    {
        _lock();
        try
        {
            for (Long stationId : RefGeneral.Ref().inn_init_station_list)
            {
                if (lookupStation(stationId) != null)
                    continue;

                RefInnStation refStation = RefInnStation.getMgr().get(stationId);
                if (refStation == null)
                {
                    USLog.error(_m_innComp.getUSServer(), "InnStationMgr.checkDefaultStations - init station config not found, stationId={}", stationId);
                    continue;
                }

                // 直接创建BO并加入列表，不通过build()避免推送消息
                PlayerInnStationBO bo = new PlayerInnStationBO();
                bo.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
                bo.setStationId(getComp().getUSServer().getBM(), stationId);
                bo.setLevel(getComp().getUSServer().getBM(), 1);
                bo.insert(getComp().getUSServer().getBM());

                _m_stationList.add(new InnStationInfo(this, bo, refStation));
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查相关设施是否存在
     * @param _needStationIdList
     * @return
     */
    public boolean checkStationsExist(List<Long> _needStationIdList)
    {
        _lock();
        try
        {
            for (Long stationId : _needStationIdList)
            {
                if (lookupStation(stationId) == null)
                    return false;
            }
            return true;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取设施等级
     * @param _stationId
     * @return
     */
    public long getStationLevel(long _stationId)
    {
        _lock();
        try
        {
            InnStationInfo stationInfo = lookupStation(_stationId);
            return stationInfo == null ? 0 : stationInfo.getLevel();
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取所有设施的总等级
     * @return
     */
    public long getTotalStationLevel()
    {
        _lock();
        try
        {
            long totalLevel = 0;
            for (InnStationInfo stationInfo : _m_stationList)
            {
                totalLevel += stationInfo.getLevel();
            }
            return totalLevel;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 将设施信息填充到列表中
     * @param _stationList
     */
    public void fillStationList(List<Inn_StationInfo> _stationList)
    {
        _lock();
        try
        {
            for (InnStationInfo stationInfo : _m_stationList)
            {
                _stationList.add(stationInfo.makeProto());
            }
        } finally
        {
            _unlock();
        }
    }
}
