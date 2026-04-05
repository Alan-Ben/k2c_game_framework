package NPUSServer.NPUSUserMgr.UserComp.GachaComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.GachaObj.Gacha_PoolInfo;
import NPCommon.CommonProcess._IEZProcessMonitor;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CallBack._ICallBackBool;
import NPGameRes.Refs.AvatarGacha.RefGachaPool;
import NPGameRes.Refs.AvatarGacha.RefGachaPoolStep;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.Record.GachaPoolRecord;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerGachaBO;
import USDB.Bo.PlayerGachaRecordBO;

import java.util.ArrayList;
import java.util.List;

public class GachaComponent extends _ANPUserComponent
{
    private List<GachaPoolInfo> _m_poolList;
    private List<GachaPoolRecord> _m_poolRecordList;

    public GachaComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.GACHA);

        _m_poolList = new ArrayList<>();
        _m_poolRecordList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        final ALProcess process = ALProcess.CreateProcess("hero_comp_init");
        //初始化加载，加载过程如果出现异常，则直接加载失败
        //步骤1: 卡池权重数据加载
        process.addResDelegateProcess(action -> _initPool(action::dealAction), "init_pool",
                () -> USLog.error(getUSServer(), "player:{} load pool bo fail.", getUserData().getCid()), false);
        //步骤2: 卡池抽卡记录数据加载
        process.addResDelegateProcess(action -> _initPoolRecord(action::dealAction), "init_pool_record",
                () -> USLog.error(getUSServer(), "player:{} load pool record bo fail.", getUserData().getCid()), false);

        process.dealProcess(new _IEZProcessMonitor()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "player:{} load gacha comp fail.", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    /**
     * 卡池权重数据加载
     * @param _handler 回调
     */
    private void _initPool(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerGachaBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerGachaBO>>()
        {
            @Override
            public void dealFail()
            {
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerGachaBO> _list)
            {
                for (PlayerGachaBO bo : _list)
                {
                    RefGachaPool refPool = RefGachaPool.getMgr().get(bo.getPoolId());
                    if (refPool == null)
                    {
                        USLog.error(getUSServer(), "GachaComponent _initPool refPool not found cid:{} poolId:{}", getUserData().getCid(), bo.getPoolId());
                        continue;
                    }

                    RefGachaPoolStep refPoolStep = RefGachaPoolStep.getMgr().lookupByPoolStep(bo.getPoolId(), bo.getStep());
                    if (refPoolStep == null)
                    {
                        USLog.error(getUSServer(), "GachaComponent _initPool refPoolStep not found cid:{} poolId:{} step:{}",
                                getUserData().getCid(), bo.getPoolId(), bo.getStep());
                        continue;
                    }

                    GachaPoolInfo poolInfo = new GachaPoolInfo(GachaComponent.this, refPool, refPoolStep, bo);
                    _addPoolToList(poolInfo);
                }

                _handler.onRunOver(true);
            }
        });
    }

    /**
     * 卡池记录数据加载
     * @param _handler 回调
     */
    private void _initPoolRecord(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerGachaRecordBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerGachaRecordBO>>()
        {
            @Override
            public void dealFail()
            {
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerGachaRecordBO> _list)
            {
                for (PlayerGachaRecordBO bo : _list)
                {
                    ensurePoolRecord(bo.getPoolId()).initFromDb(bo);
                }

                _handler.onRunOver(true);
            }
        });
    }

    private void _addPoolToList(GachaPoolInfo _poolInfo)
    {
        _m_poolList.add(_poolInfo);
    }

    /**
     * 使用poolId查找所需要的卡池信息，如果没有则返回null
     * @return 卡池信息
     */
    public GachaPoolInfo lookupPool(long _poolId)
    {
        for (GachaPoolInfo poolInfo : _m_poolList)
        {
            if (poolInfo.getPoolId() == _poolId)
            {
                return poolInfo;
            }
        }
        return null;
    }

    /**
     * 使用poolId查找所需要的卡池信息，如果没有则新建一个卡池信息插入到列表，并返回该对象
     * @return 卡池信息
     */
    public GachaPoolInfo ensurePool(RefGachaPool _refPool)
    {
        long _poolId = _refPool.Id();
        GachaPoolInfo poolInfo = lookupPool(_poolId);
        if (poolInfo == null)
        {
            PlayerGachaBO bo = new PlayerGachaBO();
            bo.setCid(getUSServer().getBM(), getUserData().getCid());
            bo.setPoolId(getUSServer().getBM(), _poolId);
            bo.setWeightInfo(getUSServer().getBM(), "");
            bo.insert(getUSServer().getBM());

            RefGachaPoolStep refPoolStep = RefGachaPoolStep.getMgr().lookupByPoolStep(_poolId, 0);
            if (refPoolStep == null)
            {
                USLog.error(getUSServer(),"GachaComponent ensurePool default refPoolStep not found cid:{} poolId:{}", getUserData().getCid(), _poolId);
                return null;
            }

            poolInfo = new GachaPoolInfo(this, _refPool, refPoolStep, bo);

            _addPoolToList(poolInfo);
        }
        return poolInfo;
    }

    /**
     * 查找对应卡池的抽卡记录
     * @param _poolId 卡池id
     * @return
     */
    public GachaPoolRecord lookupPoolRecord(long _poolId)
    {
        for (GachaPoolRecord poolRecordInfo : _m_poolRecordList)
        {
            if (poolRecordInfo.getPoolId() == _poolId)
            {
                return poolRecordInfo;
            }
        }
        return null;
    }

    /**
     * 查找对应卡池的抽卡记录，如果没有则新建一个卡池记录插入到列表，并返回该对象
     * @param _poolId 卡池id
     * @return
     */
    public GachaPoolRecord ensurePoolRecord(long _poolId)
    {
        GachaPoolRecord recordList = lookupPoolRecord(_poolId);
        if (recordList == null)
        {
            recordList = new GachaPoolRecord(this, _poolId);
            _m_poolRecordList.add(recordList);
        }
        return recordList;
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {

    }

    @Override
    public void dispose()
    {

    }

    public void fillProto(List<Gacha_PoolInfo> _list)
    {
        for (GachaPoolInfo poolInfo : _m_poolList)
        {
            _list.add(poolInfo.makeProto());
        }
    }
}
