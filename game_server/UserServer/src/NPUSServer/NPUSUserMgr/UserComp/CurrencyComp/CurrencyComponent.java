package NPUSServer.NPUSUserMgr.UserComp.CurrencyComp;

import Common.PlayerEnum.EPlayerEventRecordType;
import CommonEnum.ECurrency;
import CommonEnum.ESpecialItemType;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPCommon_CurrencyInfo;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENCounterDealType;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.RefCurrency;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CONSUME_CURRENCY;
import NPUSServer.Common.Event.Events.Event_P_GAIN_CURRENCY;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.SynTask.NPSynPlayerEvnetRecordTask;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_Gold;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_MarsEnergy;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_PaidGem;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerCurrencyBO;
import USLOGDB.Bo.LogCurrencyBO;

import java.util.ArrayList;
import java.util.List;

public class CurrencyComponent extends _ANPUserComponent implements _IUserItemBasicDealer
{
    private ArrayList<PlayerCurrencyBO> _m_alCurrencyBoList;

    public CurrencyComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.CURRENCY_COMP);

        _m_alCurrencyBoList = new ArrayList<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerCurrencyBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerCurrencyBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Currency Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerCurrencyBO> _list)
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
        return ENPItemType.CURRENCY;
    }

    /*************
     * 获取数量
     * @param _itemId
     * @return
     */
    public long getItemCount(long _itemId)
    {
        if (_itemId == ECurrency.SILVER.ordinal()) //金币部分特殊处理
        {
            SpecialItemDealer_Gold dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
            return dealer == null ? 0 : dealer.getItemCount();
        }
        else if (_itemId == ECurrency.MARS_ENERGY.ordinal()) //火星能量
        {
            SpecialItemDealer_MarsEnergy dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.MARS_ENERGY, SpecialItemDealer_MarsEnergy.class);
            return dealer == null ? 0 : dealer.getItemCount();
        }

        return _getCurrencyCount((int) _itemId);
    }

    /***************
     * 是否有足够的数量
     * @param _itemId
     * @param _count
     * @return
     */
    public boolean hasItem(long _itemId, long _count)
    {
        if (_itemId == ECurrency.SILVER.ordinal()) //金币部分特殊处理
        {
            SpecialItemDealer_Gold dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
            return dealer != null && dealer.hasItem(_count);
        }
        else if (_itemId == ECurrency.MARS_ENERGY.ordinal()) //火星能量部分特殊处理
        {
            SpecialItemDealer_MarsEnergy dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.MARS_ENERGY, SpecialItemDealer_MarsEnergy.class);
            return dealer != null && dealer.hasItem(_count);
        }

        return _hasCurrency((int) _itemId, _count);
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
        gainCurrency(_itemId, _count, false, false, _context);
    }

    /****************
     * 获取物品的处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        gainCurrency(_itemId, _count, true, _isNotMerge, _context);
    }

    /**
     * 获取物品的处理
     * @param _itemId
     * @param _count
     * @param _isNotMerge
     * @param _context
     */
    private void gainCurrency(long _itemId, long _count, boolean _needPush, boolean _isNotMerge, NPPlayerContext _context)
    {
        //金币部分特殊处理
        if (_itemId == ECurrency.SILVER.ordinal())
        {
            SpecialItemDealer_Gold dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
            if (dealer != null)
            {
                dealer.gainItem(_count, _needPush, _isNotMerge, _context);
            }
        }
        //火星能量部分特殊处理
        else if (_itemId == ECurrency.MARS_ENERGY.ordinal())
        {
            SpecialItemDealer_MarsEnergy dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.MARS_ENERGY, SpecialItemDealer_MarsEnergy.class);
            if (dealer != null)
            {
                dealer.gainItem(_count, _needPush, _isNotMerge, _context);
            }
        } else
        {
            _gainCurrency((int) _itemId, _count, _isNotMerge, _context);
        }

        getUserData().onLogicEvent(new Event_P_GAIN_CURRENCY(_context, _itemId, _count));
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
        boolean isSuc = false;
        
        if (_itemId == ECurrency.SILVER.ordinal()) //金币部分特殊处理
        {
            SpecialItemDealer_Gold dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
            if (dealer != null)
                isSuc = dealer.spendItem(_count, _context);
        }
        else if (_itemId == ECurrency.MARS_ENERGY.ordinal()) //火星能量部分特殊处理
        {
        	SpecialItemDealer_MarsEnergy dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.MARS_ENERGY, SpecialItemDealer_MarsEnergy.class);
            if (dealer != null)
                isSuc = dealer.spendItem(_count, _context);
        }
        else
        {
            isSuc = _spendCurrency((int) _itemId, _count, _context);
        }

        //如果成功消耗，触发事件
        if (isSuc)
            getUserData().onLogicEvent(new Event_P_CONSUME_CURRENCY(_context, _itemId, _count));

        return isSuc;
    }

    public void logItem(long _subId, long _oldCount, long _newCount, ELogItem_Type _logType, _IContext _context)
    {
        BM bmObj = getUSServer().getBM();

        LogCurrencyBO bo = new LogCurrencyBO();
        bo.setCid(bmObj, getUserData().getCid());
        bo.setLogType(bmObj, _logType.ordinal());
        bo.setOriginValue(bmObj, _oldCount);
        bo.setFinalValue(bmObj, _newCount);
        bo.setChgValue(bmObj, _newCount - _oldCount);
        bo.setSubId(bmObj, _subId);
        bo.setPlayerLevel(bmObj, (int) getUserData().getParam(ENPPlayerParam.LEVEL));
        CommLogDB.log(bmObj, bo, _context);
    }

    //////////////////////////////
    // ItemDealer处理部分结束
    //////////////////////////////

    private void _initBo(List<PlayerCurrencyBO> _list)
    {
        for (int i = 0; i < _list.size(); i++)
        {
            PlayerCurrencyBO bo = _list.get(i);
            if (null == bo)
                continue;

            if (bo.getType() >= ECurrency.values().length)
            {
                USLog.error(getUserData().getUSServer(), "Can not load Currency Type[type:" + bo.getType() + "]");
                continue;
            }

            _m_alCurrencyBoList.add(bo);
        }

        setInited();
    }

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////

    private PlayerCurrencyBO _getBo(int _type)
    {
        for (int i = 0; i < _m_alCurrencyBoList.size(); i++)
        {
            PlayerCurrencyBO bo = _m_alCurrencyBoList.get(i);
            if (null == bo)
                continue;

            if (bo.getType() == _type)
                return bo;
        }

        return null;
    }

    /**
     * 协议数据
     * @param _list
     */
    public void makeProtocol(ArrayList<NPCommon_CurrencyInfo> _list)
    {
        getUserData().lockUser();

        try
        {
            for (int i = 0; i < _m_alCurrencyBoList.size(); i++)
            {
                PlayerCurrencyBO bo = _m_alCurrencyBoList.get(i);
                if (null == bo)
                    continue;

                NPCommon_CurrencyInfo info = new NPCommon_CurrencyInfo();
                info.setType(bo.getType());
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
    private long _getCurrencyCount(int _type)
    {
        getUserData().lockUser();

        try
        {
            PlayerCurrencyBO bo = _getBo(_type);
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
    private boolean _hasCurrency(int _type, long _count)
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
     * @param _type
     * @param _count
     * @param _context
     * @return
     */
    private void _gainCurrency(int _type, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        if (_count <= 0)
            return;

        getUserData().lockUser();

        try
        {
            long oldCount = 0;
            long newCount;

            BM bmObj = getUSServer().getBM();

            PlayerCurrencyBO bo = _getBo(_type);
            if (null == bo)
            {
                newCount = _count;

                //不存在，则新建数据
                bo = new PlayerCurrencyBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setType(bmObj, _type);
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
            getUserData().pushMsgToGC(US2GCWriter_004_PlayerOp.make_051_PushCurrencyInfo(_type, newCount));

            //放入数据
            _context.collectItem(getItemType(), _type, _count, _isNotMerge);

            //日志数据
            logItem(_type, oldCount, newCount, ELogItem_Type.GAIN, _context);

            //当前货币数量变化
            _onCurrencyCountChg(_type, newCount, _context);
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
    private boolean _spendCurrency(int _type, long _count, NPPlayerContext _context)
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
            PlayerCurrencyBO bo = _getBo(_type);
            if (null == bo)
            {
                return false;
            }

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
            getUserData().pushMsgToGC(US2GCWriter_004_PlayerOp.make_051_PushCurrencyInfo(_type, newCount));

            //消耗记录
            RefCurrency ref = RefCurrency.getMgr().get(_type);
            if (null != ref && ref.is_spend_record)
            {
                NPSynPlayerEvnetRecordTask.asyncRecord(getUserData(), ENCounterDealType.ADD,
                        EPlayerEventRecordType.CURRENCY_SPEND.ordinal(), _type, oldCount - newCount);
            }

            //日志数据
            logItem(_type, oldCount, newCount, ELogItem_Type.CONSUME, _context);

            //当前货币数量变化
            _onCurrencyCountChg(_type, newCount, _context);

            //消耗付费钻石
            SpecialItemDealer_PaidGem dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.PAID_GEM, SpecialItemDealer_PaidGem.class);
            if (dealer != null)
                dealer.spendItem(_count, 0L, (short) 0, _context);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 货币数量变化
     * @param _type
     * @param _curCount
     * @param _context
     */
    private void _onCurrencyCountChg(int _type, long _curCount, NPPlayerContext _context)
    {
        if (_type == ECurrency.P_EXP.ordinal())
        {
            PlayerCacheFunc.updateExp(getUserData(), _curCount);
        } else if (_type == ECurrency.VIP_EXP.ordinal())
        {
        	PlayerCacheFunc.updateVipExp(getUserData(), _curCount);
        	
            getUserData().getPlayerComponent().checkVipLevelUp(_context);
        }

    }

    /**
     * 设置货币数量
     * @param _type
     * @param _count
     * @param _context
     */
    public void setCurrencyCount(int _type, long _count, NPPlayerContext _context)
    {
        if (_type == ECurrency.SILVER.ordinal()) //金币部分特殊处理
        {
            SpecialItemDealer_Gold dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
            if (dealer != null)
                dealer.setItemCount(_count, _context);
        }
        else if (_type == ECurrency.MARS_ENERGY.ordinal()) //火星能量部分特殊处理
        {
            SpecialItemDealer_MarsEnergy dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.MARS_ENERGY, SpecialItemDealer_MarsEnergy.class);
            if (dealer != null)
                dealer.setItemCount(_count, _context);
        }
        else
        {
        	_setCurrency(_type, _count, _context);
        }
    }


    /**
     * 设置货币数量
     * @param _type
     * @param _count
     * @param _context
     */
    private void _setCurrency(int _type, long _count, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            long oldCount = 0;
            PlayerCurrencyBO bo = _getBo(_type);

            BM bmObj = getUSServer().getBM();

            if (null == bo)
            {
                //不存在，则新建数据
                bo = new PlayerCurrencyBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setType(bmObj, _type);
                bo.setCount(bmObj, _count);
                bo.insert(bmObj);

                _m_alCurrencyBoList.add(bo);
            } else
            {
                oldCount = bo.getCount();
                bo.saveCount(bmObj, _count);
            }

            //推送协议
            getUserData().pushMsgToGC(US2GCWriter_004_PlayerOp.make_051_PushCurrencyInfo(_type, _count));

            //日志数据
            logItem(_type, oldCount, _count, ELogItem_Type.SET, _context);

            //当前货币数量变化
            _onCurrencyCountChg(_type, _count, _context);
        } finally
        {
            getUserData().unlockUser();
        }
    }
    
    /**
     * 获取总获取数量
     * @param _type
     * @return
     */
    public long getTotalGainCount(ECurrency _type)
    {
    	getUserData().lockUser();
    	
    	try
    	{
            if (_type == ECurrency.SILVER) //金币部分特殊处理
            {
                SpecialItemDealer_Gold dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
            	return dealer == null ? 0 : dealer.getTotalGainCount();
            }
            else if (_type == ECurrency.MARS_ENERGY) //火星能量部分特殊处理
            {
                SpecialItemDealer_MarsEnergy dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.MARS_ENERGY, SpecialItemDealer_MarsEnergy.class);
            	return dealer == null ? 0 : dealer.getTotalGainCount();
            }
            else
            {
            	PlayerCurrencyBO bo = _getBo(_type.ordinal());
        		if(null == bo)
        			return 0;
        		
        		return bo.getTotalGainCount();
            }
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
    
    /**
     * 获取总消耗数量
     * @param _type
     * @return
     */
    public long getTotalConsumeCount(ECurrency _type)
    {
    	getUserData().lockUser();
    	
    	try
    	{
            if (_type == ECurrency.SILVER) //金币部分特殊处理
            {
                SpecialItemDealer_Gold dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.GOLD, SpecialItemDealer_Gold.class);
            	return dealer == null ? 0 : dealer.getTotalConsumeCount();
            }
            else if (_type == ECurrency.MARS_ENERGY) //火星能量部分特殊处理
            {
                SpecialItemDealer_MarsEnergy dealer = getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.MARS_ENERGY, SpecialItemDealer_MarsEnergy.class);
            	return dealer == null ? 0 : dealer.getTotalConsumeCount();
            }
            else
            {
            	PlayerCurrencyBO bo = _getBo(_type.ordinal());
        		if(null == bo)
        			return 0;
        		
        		return bo.getTotalConsumeCount();
            }
    	}
    	finally 
    	{
			getUserData().unlockUser();
		}
    }
}
