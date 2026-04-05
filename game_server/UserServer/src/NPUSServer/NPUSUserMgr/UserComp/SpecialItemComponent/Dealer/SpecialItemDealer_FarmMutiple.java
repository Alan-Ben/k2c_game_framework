package NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.BuildingObj.Building_FarmMultipleInfo;
import Common.ServerObj.ServerObj_SpecialItem_FarmMultiple;
import CommonEnum.ESpecialItemType;
import GS2GC.p010_BuildingOp.GS2GC_010_058_OnFarmMultipleInfoReset;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Game.CS.CSSyncRandom;
import NPCommon.Game.CS.CSWeightRandomList;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.SpecialItemComponent;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent._ASpecialItemDealer;
import USDB.Bo.PlayerSpecialItemBO;

import java.nio.ByteBuffer;

public class SpecialItemDealer_FarmMutiple extends _ASpecialItemDealer implements _IHandlerHolder
{
    private long _m_dbId;
    private long _m_lastRefreshTimeMs;
    private int _m_hadMultipleTimes;

    private CSSyncRandom _m_random;

    //保存数据到数据库的处理，减少数据库操作的次数
    private LazyTaskDealer _m_lazySaveDealer;

    public SpecialItemDealer_FarmMutiple(SpecialItemComponent _comp)
    {
        super(_comp);

        _m_dbId = 0;
        _m_lastRefreshTimeMs = 0;
        _m_hadMultipleTimes = 0;

        //初始化随机数生成器
        _m_random = new CSSyncRandom(getComp().getUserData().getCid());

        //延迟处理器
        _m_lazySaveDealer = new LazyTaskDealer(this::_dealUpdateSave, 500);
    }

    @Override
    public ESpecialItemType getSpecialItemType()
    {
        return ESpecialItemType.FARM_MULTIPLE;
    }

    @Override
    public void initBo(PlayerSpecialItemBO _bo)
    {
        _m_dbId = _bo.getId();

        if (_bo.getData() != null)
        {
            ServerObj_SpecialItem_FarmMultiple proto = new ServerObj_SpecialItem_FarmMultiple();
            proto.readPackage(ByteBuffer.wrap(_bo.getData()));

            _m_hadMultipleTimes= proto.getHadMultipleTimes();
            _m_lastRefreshTimeMs = proto.getLastRefreshTimeMs();
        }
    }

    @Override
    public void onInited()
    {
    }

    @Override
    public void dispose()
    {
    }

    /**
     * 计算农田暴击倍数
     * @param _clientClickNum
     * @param _clientOpTimeMs
     * @return
     */
    public ResultOne<Long> calFarmMultipleClickOutput(CSWeightRandomList _weightList, long _clientClickNum, long _clientOpTimeMs)
    {
        // 检查客户端操作时间戳是否合法
        if (_clientOpTimeMs < _m_lastRefreshTimeMs)
            return ResultOne.failed(CommErr.SYS_BUSY);

        // 如果发生了跨天, 且客户端操作时间戳距离当前时间超过5秒, 则返回报错
        long todayZeroClockMS = CommonFunc.getTodayZeroClockMS(0);
        if (_clientOpTimeMs < todayZeroClockMS)
        {
            if (_clientOpTimeMs + 5 * 1000L > CommonFunc.getNowTimeMS())
            {
                return ResultOne.failed(CommErr.SYS_BUSY);
            }
        }else
        {
            //如果客户端操作时间戳大于最后刷新时间戳, 则需要检查是否跨天
            _checkRefreshMultipleTimes(todayZeroClockMS);
        }

        // 检查客户端点击次数是否正确, 如果错误, 则发送同步信息
        if (_m_random.getOperationCount() != _clientClickNum)
        {
            getComp().getUserData().sendMsgToGC(new GS2GC_010_058_OnFarmMultipleInfoReset(toProto()));
            return ResultOne.failed(CommErr.SYS_BUSY);
        }

        // 计算暴击次数
        Long multiple = _weightList.randomSelect(_m_random);

        // 检查是否超过每日暴击次数限制
        if (_m_hadMultipleTimes >= RefGeneral.Ref().farming_building_trigger_multiple_daily_limit)
            return ResultOne.succ(1L);

        return ResultOne.succ(multiple == null ? 1L : multiple);
    }

    /**
     * 检查是否需要刷新暴击倍数
     */
    private void _checkRefreshMultipleTimes(long _todayZeroClockMS)
    {
        if (_todayZeroClockMS > _m_lastRefreshTimeMs)
        {
            // 如果当前时间大于最后刷新时间, 则重置暴击次数
            _m_hadMultipleTimes = 0;
            _m_lastRefreshTimeMs = CommonFunc.getNowTimeMS();
            // 保存数据到数据库
            _saveData();
        }
    }

    /**
     * 记录暴击次数
     */
    public void addHadMultipleTimes()
    {
        _m_hadMultipleTimes++;
        _saveData();
    }

    /**
     * 保存数据
     */
    private void _saveData()
    {
        //如果未保存数据库则需要马上插入，如果已经保存则处理更新行为
        _lock();
        try
        {
            if (0 == _m_dbId)
            {
                BM bmObj = getComp().getUSServer().getBM();
                //构造保存数据
                ServerObj_SpecialItem_FarmMultiple proto = new ServerObj_SpecialItem_FarmMultiple();
                proto.setHadMultipleTimes(_m_hadMultipleTimes);
                proto.setLastRefreshTimeMs(_m_lastRefreshTimeMs);

                PlayerSpecialItemBO bo = new PlayerSpecialItemBO();
                bo.setCid(getComp().getUSServer().getBM(), getComp().getUserData().getCid());
                bo.setType(getComp().getUSServer().getBM(), getSpecialItemType().ordinal());
                bo.setData(getComp().getUSServer().getBM(), CommonFunc.ByteBfferToBytes(proto.makePackage()));
                bo.insert(bmObj);

                _m_dbId = bo.getId();
            } else
            {
                //保存的处理需要通过LasyDealer处理
                _m_lazySaveDealer.setNeedDeal();
            }
        } finally
        {
            _unlock();
        }
    }

    /********
     * 保存数据库的更新处理，insert需要马上处理
     */
    private void _dealUpdateSave()
    {
        _lock();
        try
        {
            if (_m_dbId == 0)
                return;

            BM bmObj = getComp().getUSServer().getBM();
            //构造保存数据
            ServerObj_SpecialItem_FarmMultiple proto = new ServerObj_SpecialItem_FarmMultiple();
            proto.setHadMultipleTimes(_m_hadMultipleTimes);
            proto.setLastRefreshTimeMs(_m_lastRefreshTimeMs);

            ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
            updateValue.addValueObj("data", proto.makePackage());

            bmObj.getBM(PlayerSpecialItemBO.class).update("id", _m_dbId, updateValue);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造协议
     * @return
     */
    public Building_FarmMultipleInfo toProto()
    {
        Building_FarmMultipleInfo proto = new Building_FarmMultipleInfo();
        proto.setClickNum(_m_random.getOperationCount());
        proto.setRandomSeed(_m_random.getSeed());
        proto.setCritCount(_m_hadMultipleTimes);
        proto.setLastRefreshTimeMs(_m_lastRefreshTimeMs);
        return proto;
    }
}
