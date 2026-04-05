package NPUSServer.NPUSUserMgr.UserComp.PlayerFixedCdComp;

import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPCommon_PlayerFixedCD;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerThree;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.RefPlayerFixedCd;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerFixedCdBO;
import USLOGDB.Bo.LogFixedCdBO;

import java.util.ArrayList;
import java.util.List;

/**
 * @description: 定时刷新cd组件
 * @author: ricci
 * @date: 2022-10-24 16:41:33
 */
public class PlayerFixedCdComp extends _ANPUserComponent implements _IHandlerHolder, _IUserItemBasicDealer
{
    /**
     * 定时刷新数据列表
     */
    private final ArrayList<PlayerFixedCD> _m_lFixedCDList;

    public PlayerFixedCdComp(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.FIXED_CD);
        _m_lFixedCDList = new ArrayList<>();
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

    private PlayerFixedCdComp getThis()
    {
        return this;
    }

    /**
     * 通过配置文件初始化
     * @param _ref 配置数据
     * @return NPPlayerFixedCD
     */
    public PlayerFixedCD initByRef(RefPlayerFixedCd _ref)
    {
        _lock();
        try
        {
            PlayerFixedCD cd = __lookupByRefId(_ref.id);
            long nowTimeMS = CommonFunc.getNowTimeMS();
            if (cd == null)
            {
                BM bmObj = getUSServer().getBM();

                PlayerFixedCdBO bo = new PlayerFixedCdBO();
                bo.setCid(bmObj, getCid());
                bo.setCdId(bmObj, _ref.id);
                bo.setLastCalcTime(bmObj, nowTimeMS);
                bo.setCount(bmObj, 0);
                bo.insert(bmObj);

                cd = new PlayerFixedCD(this, _ref, bo);
                _m_lFixedCDList.add(cd);
                //补满 cd 值
                cd.fill();
            }
            return cd;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 设置数量
     * @param _id      cdId
     * @param _count   数量
     * @param _context
     */
    public void setItemCount(long _id, int _count, NPPlayerContext _context)
    {
        _lock();
        try
        {
            RefPlayerFixedCd ref = RefPlayerFixedCd.getMgr().get(_id);
            if (ref == null)
            {
                return;
            }
            long nowTimeMS = CommonFunc.getNowTimeMS();
            PlayerFixedCD cd = __lookupByRefId(_id);
            if (cd == null)
            {
                BM bmObj = getUSServer().getBM();

                PlayerFixedCdBO bo = new PlayerFixedCdBO();
                bo.setCid(bmObj, getCid());
                bo.setCdId(bmObj, (int) _id);
                bo.setLastCalcTime(bmObj, nowTimeMS);
                bo.setCount(bmObj, 0);
                bo.insert(bmObj);

                cd = new PlayerFixedCD(this, ref, bo);
                _m_lFixedCDList.add(cd);
            }
            cd.setCount(_count, _context);

        } finally
        {
            _unlock();
        }
    }

    /**
     *  查找cd数据
     * @param _refId
     * @return
     */
    public PlayerFixedCD lookupByRefId(long _refId)
    {
    	return __lookupByRefId(_refId);
    }
    
    /**
     * 通过配置id查找数据
     * @param _refId RefPlayerFixedCd
     * @return NPPlayerFixedCD
     */
    private PlayerFixedCD __lookupByRefId(long _refId)
    {
        _lock();
        try
        {
            for (PlayerFixedCD cd : _m_lFixedCDList)
            {
                if (cd == null)
                {
                    continue;
                }
                if (cd.getRefId() == _refId)
                {
                    return cd;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造协议数据
     * @param _cdList 协议数据列表
     */
    public void makeProto(ArrayList<NPCommon_PlayerFixedCD> _cdList)
    {
        _lock();
        try
        {
            for (PlayerFixedCD cd : _m_lFixedCDList)
            {
                _cdList.add(cd.makeProto());
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 重置刷新时间
     * @param _id cdId
     */
    public PlayerFixedCD cmdSetFreshTime(long _id)
    {
        _lock();
        try
        {
            PlayerFixedCD cd = __lookupByRefId(_id);
            if (cd == null)
            {
                return null;
            }
            cd.cmdSetFreshTime();

            return cd;
        } finally
        {
            _unlock();
        }
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerFixedCdBO.class).findAll("cid", getCid(), new _ASelectCallback<List<PlayerFixedCdBO>>()
        {
            @Override
            public void dealSuc(List<PlayerFixedCdBO> _boList)
            {
                for (PlayerFixedCdBO bo : _boList)
                {
                    if (bo == null)
                    {
                        continue;
                    }
                    RefPlayerFixedCd ref = RefPlayerFixedCd.getMgr().get(bo.getCdId());
                    if (ref == null)
                    {
                        USLog.error(getUSServer(), "NPPLayerFixedCdComp ref is null cid:{},refId:{}",
                                getCid(), bo.getCdId());
                        continue;
                    }
                    PlayerFixedCD obj = new PlayerFixedCD(getThis(), ref, bo);
                    _m_lFixedCDList.add(obj);
                }
                setInited();
            }

            @Override
            public void dealFail()
            {
                getUserData().setDataLoadFail();
            }
        });

        //监听属性容器变化
        getUserData().getPlayerComponent().getPropertyMgr()
                .propertyChgDelegate().addHandler(this, new HandlerThree<ENPPlayerPropertyType, Long, Long>()
                {
                    @Override
                    public void handle(ENPPlayerPropertyType _propertyType, Long _preValue, Long _value)
                    {
                        //遍历所有数据进行处理
                        getUserData().lockUser();

                        try
                        {

                            for (PlayerFixedCD cdObj : _m_lFixedCDList)
                            {
                                if (null == cdObj)
                                    continue;

                                cdObj._onPlayerPropertyChg(_propertyType, _preValue, _value);
                            }
                        } finally
                        {
                            getUserData().unlockUser();
                        }
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
        //从配置文件初始化
        for (RefPlayerFixedCd ref : RefPlayerFixedCd.getMgr().getList())
        {
            if (ref == null)
            {
                continue;
            }
            initByRef(ref);
        }
    }

    @Override
    public void dispose()
    {
        getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().clear(this);
    }

    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.FIXED_CD;
    }

    @Override
    public long getItemCount(long _itemId)
    {

        PlayerFixedCD cd = __lookupByRefId(_itemId);
        if (cd == null)
        {
            return 0;
        }
        return cd.getCount();
    }

    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        return getItemCount(_itemId) >= _count;
    }

    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        RefPlayerFixedCd ref = RefPlayerFixedCd.getMgr().get(_itemId);
        if (ref == null)
        {
            USLog.error(getUSServer(), "NPPLayerFixedCdComp initGainItem ref is null cid:{},itemId:{}",
                    getCid(), _itemId);
            return;
        }
        long nowTimeMS = CommonFunc.getNowTimeMS();
        PlayerFixedCD cd = __lookupByRefId(_itemId);
        if (cd == null)
        {
            BM bmObj = getUSServer().getBM();

            PlayerFixedCdBO bo = new PlayerFixedCdBO();
            bo.setCid(bmObj, getCid());
            bo.setCdId(bmObj, ref.id);
            bo.setLastCalcTime(bmObj, nowTimeMS);
            bo.setCount(bmObj, 0);
            bo.insert(bmObj);

            cd = new PlayerFixedCD(this, ref, bo);
            _m_lFixedCDList.add(cd);
        }
        cd.setCount((int) _count, _context);

    }

    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        RefPlayerFixedCd ref = RefPlayerFixedCd.getMgr().get(_itemId);
        if (ref == null)
        {
            USLog.error(getUSServer(), "NPPLayerFixedCdComp gainItem ref is null cid:{},itemId:{}",
                    getCid(), _itemId);
            return;
        }
        long nowTimeMS = CommonFunc.getNowTimeMS();
        PlayerFixedCD cd = __lookupByRefId(_itemId);
        if (cd == null)
        {
            BM bmObj = getUSServer().getBM();

            PlayerFixedCdBO bo = new PlayerFixedCdBO();
            bo.setCid(bmObj, getCid());
            bo.setCdId(bmObj, ref.id);
            bo.setLastCalcTime(bmObj, nowTimeMS);
            bo.setCount(bmObj, 0);
            bo.insert(bmObj);

            cd = new PlayerFixedCD(this, ref, bo);
            _m_lFixedCDList.add(cd);
        }
        cd.addCountExt((int) _count);
        _context.collectItem(getItemType(), _itemId, _count, _isNotMerge);
    }

    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        PlayerFixedCD cd = __lookupByRefId(_itemId);
        if (null == cd)
            return false;
        int hasCount = cd.getCount();
        if (hasCount < _count)
        {
            return false;
        }
        cd.descCount((int) _count, _context);
        return true;
    }

    public void logItem(long _subId, long _oldCount, long _newCount, ELogItem_Type _logType, _IContext _context)
    {
        BM bmObj = getUSServer().getBM();

        LogFixedCdBO bo = new LogFixedCdBO();
        bo.setCid(bmObj, getUserData().getCid());
        bo.setLogType(bmObj, _logType.ordinal());
        bo.setOriginValue(bmObj, _oldCount);
        bo.setFinalValue(bmObj, _newCount);
        bo.setChgValue(bmObj, _newCount - _oldCount);
        bo.setSubId(bmObj, _subId);
        CommLogDB.log(bmObj, bo, _context);
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        for (PlayerFixedCD cd : _m_lFixedCDList)
        {
            sb.append("\n").append(cd.toString()).append("\n");
        }
        return "NPPlayerFixedCdComp{" +
                "_m_lFixedCDList=" + sb +
                '}';
    }

    /***************************
     * 只针对当天的CD进行处理
     * @param _date
     * @param _itemId
     * @param _count
     * @param _isNotMerge
     * @param _context
     */
    public void addFixedCdByNowDate(int _date, long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        //不是当天的日期，则不处理
        if (CommonFunc.getNowTagYYYYMMDD() != _date)
            return;

        gainItem(_itemId, _count, _isNotMerge, _context);
    }
}
