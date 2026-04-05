package NPUSServer.NPUSUserMgr.UserComp.ClockRewardComp;

import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Battle.RefClockReward;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUSUserMgr.UserComp._ITickableComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerClockRewardBO;

import java.util.ArrayList;
import java.util.List;

/**
 * @description: 玩家定时奖励组件
 * @author: ricci
 * @date: 2022-10-24 13:41:51
 */
public class ClockRewardComponent extends _ANPUserComponent implements _ITickableComponent
{

    private final ArrayList<ClockRewardObj> _m_listClockRewardObjList;

    public ClockRewardComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.CLOCK_REWARD);

        _m_listClockRewardObjList = new ArrayList<>();
    }

    protected void _lock()
    {
        getUserData().lockUser();
    }

    protected void _unlock()
    {
        getUserData().unlockUser();
    }

    public long getCid()
    {
        return getUserData().getCid();
    }

    private ClockRewardComponent getThis()
    {
        return this;
    }

    /**
     * 通过配置文件创建定时奖励刷新数据
     * @param _ref 配置
     */
    protected void refreshByRef(RefClockReward _ref)
    {
        _lock();
        try
        {
            long nowTimeMs = CommonFunc.getNowTimeMS();
            ClockRewardObj obj = _lookup(_ref.id);
            if (obj == null)
            {
                BM bmObj = getUSServer().getBM();

                PlayerClockRewardBO bo = new PlayerClockRewardBO();
                bo.setCid(bmObj, getCid());
                bo.setRefId(bmObj, _ref.id);
                long nextFreshTimeTagMS = _ref.refresh_clock.getNextFreshTimeTagMS(nowTimeMs);
                bo.setNextRefreshTimeMs(bmObj, nextFreshTimeTagMS);
                bo.setNum(bmObj, 0);
                bo.insert(bmObj);
                //创建刷新管理对象加入到管理列表中
                obj = new ClockRewardObj(this, _ref, bo);
                _m_listClockRewardObjList.add(obj);
            }
            obj.trySendClockReward(nowTimeMs);
        } finally
        {
            _unlock();
        }
    }


    /**
     * 通过配置id查找定时奖励下发数据
     * @param _refId 配置id
     * @return NPClockRewardObj
     */
    private ClockRewardObj _lookup(int _refId)
    {
        _lock();
        try
        {
            for (ClockRewardObj obj : _m_listClockRewardObjList)
            {
                if (obj == null)
                {
                    continue;
                }
                if (obj.getRefId() == _refId)
                {
                    return obj;
                }
            }
        } finally
        {
            _unlock();
        }
        return null;
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerClockRewardBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerClockRewardBO>>()
        {
            @Override
            public void dealSuc(List<PlayerClockRewardBO> _boList)
            {
                for (PlayerClockRewardBO bo : _boList)
                {
                    if (bo == null)
                    {
                        continue;
                    }
                    //检查配置是否存在
                    RefClockReward ref = RefClockReward.getMgr().get(bo.getRefId());
                    if (ref == null)
                    {
                        USLog.error(getUSServer(), "NPClockRewardComponent init ref is null cid:{},refId:{}"
                                , getCid(), bo.getRefId());
                        continue;
                    }
                    //将数据加入到管理列表
                    ClockRewardObj obj = new ClockRewardObj(getThis(), ref, bo);
                    _m_listClockRewardObjList.add(obj);
                }
                //设置组件加载完成
                setInited();
            }

            @Override
            public void dealFail()
            {
                getUserData().setDataLoadFail();
            }
        });
    }


    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        //初始化从配置中加载定时奖励发放数据
        for (RefClockReward ref : RefClockReward.getMgr().getList())
        {
            if (ref == null)
            {
                continue;
            }
            //通过配置文件初始化
            refreshByRef(ref);
        }
    }

    @Override
    public void dispose()
    {

    }

    @Override
    public void tick1Sec()
    {
        try
        {
            long nowTimeMS = CommonFunc.getNowTimeMS();
            //每秒检测定时奖励是否应该被发放
            for (ClockRewardObj obj : _m_listClockRewardObjList)
            {
                if (obj == null)
                {
                    continue;
                }
                obj.trySendClockReward(nowTimeMS);
            }

        } catch (Exception e)
        {
            e.printStackTrace();
        }

    }
}
