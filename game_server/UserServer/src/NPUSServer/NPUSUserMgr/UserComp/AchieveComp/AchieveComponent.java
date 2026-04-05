package NPUSServer.NPUSUserMgr.UserComp.AchieveComp;

import ALBasicServer.ALProcess.ALProcess;
import ALBasicServer.ALProcess._AALProcess;
import ALBasicServer.ALProcess._IALProcessMonitor;
import Common.AchieveObj.Achieve_AchievePointInfo;
import Common.AchieveObj.Achieve_Info;
import CommonEnum.EAchieveType;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.ErrMain.AchieveErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CallBack._ICallBackBool;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Achieve.RefAchieve;
import NPGameRes.Refs.Achieve.RefAchievePointStep;
import NPGameRes.Refs.Achieve.RefAchieveStep;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerAchieveBO;
import USDB.Bo.PlayerAchievePointBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class AchieveComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
    //成就点数据 key-EAchieveType
    private HashMap<Integer, AchievePointInfo> _m_achievePointMap;
    //成就数据 key-成就配置ID
    private HashMap<Long, AchieveInfo> _m_achieveMap;

    public AchieveComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.ACHIEVE);

        _m_achievePointMap = new HashMap<>();
        _m_achieveMap = new HashMap<>();
    }

    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("achieve_comp_init");

        //初始化加载，加载过程如果出现异常，则直接加载失败
        //步骤1:任务数据加载
        //步骤2:任务目标数据加载
        process.addResDelegateProcess(_doneAction -> _initAchieveFromDB(
                                _isSucc ->
                                {
                                    _doneAction.dealAction(_isSucc);
                                })
                        , "achieve_init"
                        , () -> //出现异常时的处理
                        {
                            USLog.error(getUSServer(), "load achieve comp fail[achieve_db_init], cid:{}", getUserData().getCid());
                        }
                        , false)
                .addResDelegateProcess(_doneAction -> _initAchievePointFromDB(
                                _isSucc ->
                                {
                                    _doneAction.dealAction(_isSucc);
                                })
                        , "achieve_point_init"
                        , () -> //出现异常时的处理
                        {
                            USLog.error(getUSServer(), "load achieve comp fail[achieve_db_init], cid:{}", getUserData().getCid());
                        }
                        , false);

        //开启执行
        process.dealProcess(new _IALProcessMonitor()
        {
            @Override
            public void onTimeoutDone(long _processTimeMS, String _processTag, String _exInfo)
            {
            }

            @Override
            public void onTimeout(long _processTimeMS, String _processTag, String _exInfo)
            {
            }

            //异常终止的事件函数
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "load achieve comp fail[onRootProecssStop], cid:{}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            //正常结束的事件函数
            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }

            @Override
            public void onProcessFailStop(_AALProcess _process)
            {
            }

            @Override
            public void onRootProecssDone()
            {
            }

            @Override
            public void onErr(long _processTimeMS, String _processTag, String _exInfo, Exception _ex)
            {
                USLog.error(getUserData().getUSServer(), "achieve comp init err: " + _ex.getMessage());
            }

            @Override
            public long monitorTimeMS(String _processTag)
            {
                return 0;
            }
        });
    }

    //初始化数据表成就
    private void _initAchieveFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerAchieveBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerAchieveBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "AchieveComponent _initAchieveFromDB load PlayerAchieveBO fail cid:{}", getUserData().getCid());
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerAchieveBO> _list)
            {
                //加载数据
                for (PlayerAchieveBO bo : _list)
                {
                    RefAchieve ref = RefAchieve.getMgr().get(bo.getAchieveId());
                    if (null == ref)
                    {
                        USLog.error(getUSServer(), "AchieveComponent _initAchieveFromDB RefAchieve is null cid:{} achieve:{}", bo.getCid(), bo.getAchieveId());
                        continue;
                    }

                    _createAchieveInfo(ref, bo);
                }

                //初始化不在列表中的数据
                for (RefAchieve ref : RefAchieve.getMgr().getList())
                {
                    //已经存在的不再处理
                    if (_m_achieveMap.containsKey(ref.achieve_id))
                        continue;

                    _createAchieveInfo(ref, null);
                }

                _handler.onRunOver(true);
            }
        });
    }

    //初始化数据表成就
    private void _initAchievePointFromDB(_ICallBackBool _handler)
    {
        getUSServer().getBM().getBM(PlayerAchievePointBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerAchievePointBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "AchieveComponent _initAchieveFromDB load PlayerAchievePointBO fail cid:{}", getUserData().getCid());
                _handler.onRunOver(false);
            }

            @Override
            public void dealSuc(List<PlayerAchievePointBO> _list)
            {
                for (PlayerAchievePointBO bo : _list)
                {
                    _createAchievePointInfo(bo);
                }

                //初始化不在列表中的数据
                for (EAchieveType type : EAchieveType.values())
                {
                    //已经存在的不再处理
                    if (_m_achievePointMap.containsKey(type.ordinal()))
                        continue;

                    PlayerAchievePointBO bo = new PlayerAchievePointBO();
                    bo.setCid(getUSServer().getBM(), getUserData().getCid());
                    bo.setType(getUSServer().getBM(), type.ordinal());
                    bo.insert(getUSServer().getBM());

                    _createAchievePointInfo(bo);
                }

                _handler.onRunOver(true);
            }
        });
    }

    /**
     * 构造成就数据对象
     * @param _ref 配表对象
     * @param _bo  数据对象(允许为空)
     */
    private void _createAchieveInfo(RefAchieve _ref, PlayerAchieveBO _bo)
    {
        AchieveInfo info = new AchieveInfo(this, _ref, _bo);
        _m_achieveMap.put(info.getAchieveId(), info);
    }

    /**
     * 构造成就点数据对象
     * @param _bo 数据对象
     */
    private void _createAchievePointInfo(PlayerAchievePointBO _bo)
    {
        AchievePointInfo info = new AchievePointInfo(this, _bo);
        _m_achievePointMap.put(info.getType(), info);
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
        getUserData().lockUser();
        try
        {
            //注销全部任务监听
            for (AchieveInfo achieve : _m_achieveMap.values())
            {
                if (null == achieve)
                    continue;

                achieve.unRegEvtEntry();
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造数据列表协议
     * @param _list
     */
    public void makeProto(ArrayList<Achieve_Info> _list, ArrayList<Achieve_AchievePointInfo> _achievePointList)
    {
        getUserData().lockUser();
        try
        {
            for (AchieveInfo achieve : _m_achieveMap.values())
            {
                if (null == achieve)
                    continue;

                _list.add(achieve.toProto());
            }
            for (AchievePointInfo achievePoint : _m_achievePointMap.values())
            {
                if (null == achievePoint)
                    continue;

                _achievePointList.add(achievePoint.toProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查找指定成就数据
     * @param _achieveId 成就ID
     * @return
     */
    public AchieveInfo lookupAchieve(long _achieveId)
    {
        getUserData().lockUser();
        try
        {
            return _m_achieveMap.get(_achieveId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 查找指定成就数据
     * @param _achieveTypeId 成就类型ID
     * @return
     */
    public AchievePointInfo lookupAchievePoint(int _achieveTypeId)
    {
        getUserData().lockUser();
        try
        {
            return _m_achievePointMap.get(_achieveTypeId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加成就点计数
     */
    public void addAchievePointCount(int _type, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            AchievePointInfo info = lookupAchievePoint(_type);
            if (null == info)
                return;

            info.addCount(_count, _context);

            //加入到收集器
            _context.collectItem(ENPItemType.ACHIEVE_POINT, _type, _count, _isNotMerge);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加成就点计数
     */
    public boolean resetAchieve(long _achieveId)
    {
        getUserData().lockUser();
        try
        {
            AchieveInfo info = lookupAchieve(_achieveId);
            if (null == info)
                return false;

            info.reset();
            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取成就点阶段奖励
     */
    public Result drawAchievePointStepReward(long _achievePointStepRewardId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //查找成就点阶段配置
            RefAchievePointStep ref = RefAchievePointStep.getMgr().get(_achievePointStepRewardId);
            if (ref == null)
                return CommErr.REF_NOT_FOUND;

            //检查成就点是否足够
            if (!getUserData().hasItem(ref.need_point))
                return AchieveErr.ACHIEVE_POINT_NOT_ENOUGH;

            //查找成就点数据
            AchievePointInfo info = lookupAchievePoint(ref.type.ordinal());
            if (info == null)
                return AchieveErr.ACHIEVE_POINT_NOT_EXIST;

            return info.drawStepReward(ref, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 领取成就阶段奖励
     */
    public Result drawAchieveStepReward(long _achieveId, int _step, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try
        {
            //查找成就阶段配置
            RefAchieveStep ref = RefAchieveStep.getMgr().lookupByAchieveStep(_achieveId, _step);
            if (ref == null)
                return CommErr.REF_NOT_FOUND;

            //查找成就数据
            AchieveInfo info = lookupAchieve(ref.achieve_id);
            if (null == info)
                return AchieveErr.ACHIEVE_NOT_ENOUGH;

            return info.drawStepReward(ref, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 构造日志数据
     * @return
     */
    public String toLogString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        getUserData().lockUser();
        try
        {
            for (Map.Entry<Long, AchieveInfo> entry : _m_achieveMap.entrySet())
            {
                stringBuilder.append(entry.getKey()).append(":").append(entry.getValue().getMaxDrawStepId()).append(";");
            }
        } finally
        {
            getUserData().unlockUser();
        }
        return stringBuilder.toString();
    }


    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.ACHIEVE_POINT;
    }

    @Override
    public long getItemCount(long _itemId)
    {
        AchievePointInfo info = lookupAchievePoint((int) _itemId);
        return info == null ? 0 : info.getCount();
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        AchievePointInfo info = lookupAchievePoint((int) _itemId);
        return info != null && info.getCount() >= _count;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        addAchievePointCount((int) _itemId, _count, false, _context);
    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        addAchievePointCount((int) _itemId, _count, _isNotMerge, _context);
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return false;
    }

}
