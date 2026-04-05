package NPUSServer.NPUSUserMgr.UserComp.ActivityCurrency;

import Common.Common_ActivityCurrencyInfo;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Activity.RefActivityCurrency;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GAIN_ACTIVITY_CURRENCY;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerActivityCurrencyBO;
import USLOGDB.Bo.LogActivityCurrencyBO;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import java.util.ArrayList;
import java.util.List;

public class ActivityCurrencyComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
    private static final Log log = LogFactory.getLog(ActivityCurrencyComponent.class);
    private ArrayList<PlayerActivityCurrencyBO> _m_alCurrencyBoList;

    public ActivityCurrencyComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.ACTIVITY_CURRENCY);

        _m_alCurrencyBoList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerActivityCurrencyBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerActivityCurrencyBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Activity Currency Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerActivityCurrencyBO> _list)
            {
                _initBo(_list);
            }
        });
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

    }

    //////////////////////////////
    // ItemDealer处理部分
    //////////////////////////////

    /**********
     * 对应处理的类型
     * @return
     */
    public ENPItemType getItemType()
    {
        return ENPItemType.ACTIVITY_CURRENCY;
    }

    /*************
     * 获取数量
     * @param _itemId
     * @return
     */
    public long getItemCount(long _itemId)
    {
        return _getCurrencyCount(_itemId);
    }

    /***************
     * 是否有足够的数量
     * @param _itemId
     * @param _count
     * @return
     */
    public boolean hasItem(long _itemId, long _count)
    {
        return _hasCurrency(_itemId, _count);
    }

    /****************
     * 初始化的获取物品处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        _gainCurrency(_itemId, _count, false, _context);
    }

    /****************
     * 获取物品的处理，返回值表示是否触发gameevent事件
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        _gainCurrency(_itemId, _count, _isNotMerge, _context);
    }

    /****************
     * 消耗物品的处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return _spendCurrency(_itemId, _count, _context);
    }

    public void logItem(long _subId, long _oldCount, long _newCount, ELogItem_Type _logType, _IContext _context)
    {
        BM bmObj = getUSServer().getBM();

        LogActivityCurrencyBO bo = new LogActivityCurrencyBO();
        bo.setCid(bmObj, getUserData().getCid());
        bo.setLogType(bmObj, _logType.ordinal());
        bo.setOriginValue(bmObj, _oldCount);
        bo.setFinalValue(bmObj, _newCount);
        bo.setChgValue(bmObj, _newCount - _oldCount);
        bo.setSubId(bmObj, _subId);
        CommLogDB.log(bmObj, bo, _context);
    }

    //////////////////////////////
    // ItemDealer处理部分结束
    //////////////////////////////

    private void _initBo(List<PlayerActivityCurrencyBO> _list)
    {
        for (int i = 0; i < _list.size(); i++)
        {
            PlayerActivityCurrencyBO bo = _list.get(i);
            if (null == bo)
                continue;

            _m_alCurrencyBoList.add(bo);
        }

        setInited();
    }

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////

    private PlayerActivityCurrencyBO _getBo(long _refId)
    {
        for (int i = 0; i < _m_alCurrencyBoList.size(); i++)
        {
            PlayerActivityCurrencyBO bo = _m_alCurrencyBoList.get(i);
            if (null == bo)
                continue;

            if (bo.getRefId() == _refId)
                return bo;
        }

        return null;
    }

    /**
     * 协议数据
     * @param _list
     */
    public void makeProtocol(ArrayList<Common_ActivityCurrencyInfo> _list)
    {
        getUserData().lockUser();
        try
        {
            for (int i = 0; i < _m_alCurrencyBoList.size(); i++)
            {
                PlayerActivityCurrencyBO bo = _m_alCurrencyBoList.get(i);
                if (null == bo)
                    continue;

                Common_ActivityCurrencyInfo info = new Common_ActivityCurrencyInfo();
                info.setRefId(bo.getRefId());
                info.setCount(bo.getCount());
                _list.add(info);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取货币
     * @param _type
     * @return
     */
    private long _getCurrencyCount(long _type)
    {
        getUserData().lockUser();
        try
        {
            PlayerActivityCurrencyBO bo = _getBo(_type);
            if (null == bo)
                return 0L;

            return bo.getCount();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 是否有足够的货币
     * @param _type
     * @param _count
     * @return
     */
    private boolean _hasCurrency(long _type, long _count)
    {
        getUserData().lockUser();

        try
        {
            return _getCurrencyCount(_type) >= _count;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 添加货币
     * @param _refId
     * @param _count
     * @param _context
     * @return
     */
    private void _gainCurrency(long _refId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        if (_count <= 0)
            return;

        getUserData().lockUser();

        try
        {
            long oldCount = 0;
            long newCount;

            BM bmObj = getUSServer().getBM();

            PlayerActivityCurrencyBO bo = _getBo(_refId);
            if (null == bo)
            {
                RefActivityCurrency ref = RefActivityCurrency.getMgr().get(_refId);
                if (null == ref)
                {
                    USLog.error(getUserData().getUSServer(), "ActivityCurrencyComponent gainCurrency error, _refId not found, cid:{} refId:{}", getUserData().getCid(), _refId);
                    return;
                }
                
                newCount = _count;

                //不存在，则新建数据
                bo = new PlayerActivityCurrencyBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setRefId(bmObj, _refId);
                bo.setCount(bmObj, newCount);
                bo.setTotalGainCount(bmObj, newCount);
                bo.insert(bmObj);

                _m_alCurrencyBoList.add(bo);
            } else
            {
                //修改数据
                oldCount = bo.getCount();
                newCount = oldCount + _count;

                bo.setCount(bmObj, newCount);
                bo.setTotalGainCount(bmObj, bo.getTotalGainCount() + _count);
                bo.saveAllMarked(bmObj);
            }

            //推送协议
            getUserData().pushMsgToGC(US2GCWriter_004_PlayerOp.make_059_PushActivityCurrencyInfo(_refId, newCount));

            //放入数据
            _context.collectItem(getItemType(), _refId, _count, _isNotMerge);

            //日志数据
            logItem(_refId, oldCount, newCount, ELogItem_Type.GAIN, _context);

            //触发事件
            getUserData().onLogicEvent(new Event_P_GAIN_ACTIVITY_CURRENCY(_context, _refId, _count));
        } finally
        {
            getUserData().unlockUser();
        }
    }


    /**
     * 消耗货币
     * @param _type
     * @param _count
     * @param _context
     * @return
     */
    private boolean _spendCurrency(long _type, long _count, NPPlayerContext _context)
    {
        if (_count < 0)
        {
            return false;
        }

        if (_count == 0)
            return true;

        getUserData().lockUser();

        try
        {
        	//消耗逻辑
            PlayerActivityCurrencyBO bo = _getBo(_type);
            if (null == bo)
                return false;

            long oldCount = bo.getCount();
            long newCount = oldCount - _count;
            if (newCount < 0)
            {
                return false;
            }

            //成功处理
            bo.setCount(getUSServer().getBM(), newCount);
            bo.setTotalConsumeCount(getUSServer().getBM(), bo.getTotalConsumeCount() + _count);
            bo.saveAllMarked(getUSServer().getBM());

            //推送协议
            getUserData().pushMsgToGC(US2GCWriter_004_PlayerOp.make_059_PushActivityCurrencyInfo(_type, newCount));

            //日志数据
            logItem(_type, oldCount, newCount, ELogItem_Type.CONSUME, _context);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }
    
    /**
     * 获取总获取数量
     * @param _refId
     * @return
     */
    public long getTotalGainCount(long _refId)
    {
    	getUserData().lockUser();
    	
    	try
    	{
            PlayerActivityCurrencyBO bo = _getBo(_refId);
            if(null == bo)
                return 0;

            return bo.getTotalGainCount();
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
    
    /**
     * 获取总消耗数量
     * @param _refId
     * @return
     */
    public long getTotalConsumeCount(long _refId)
    {
    	getUserData().lockUser();
    	try
    	{
            PlayerActivityCurrencyBO bo = _getBo(_refId);
            if(null == bo)
                return 0;

            return bo.getTotalConsumeCount();
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
}
