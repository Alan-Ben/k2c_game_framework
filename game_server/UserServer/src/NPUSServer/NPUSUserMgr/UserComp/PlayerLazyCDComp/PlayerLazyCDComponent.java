package NPUSServer.NPUSUserMgr.UserComp.PlayerLazyCDComp;

import GS2GC.p021_PlayerInfo.GS2GC_021_051_OnAddNewLazyCd;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPCommon_PlayerLazyCD;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerThree;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.RefPlayerLazyCd;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;
import USDB.Bo.PlayerLazyCdBO;
import USLOGDB.Bo.LogLazyCdBO;

import java.util.ArrayList;
import java.util.List;


public class PlayerLazyCDComponent extends _ANPUserComponent implements _IHandlerHolder, _IUserItemBasicDealer
{
    //存储lazycd的数据集合
    private ArrayList<PlayerLazyCD> _m_lCDList;

    public PlayerLazyCDComponent(NPUSUserData _userData)
    {
        super(_userData, NPCommonEnum.ENPPlayerCompType.PLAYER_LAZY_CD_COMP);

        _m_lCDList = new ArrayList<PlayerLazyCD>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerLazyCdBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerLazyCdBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load PlayerTrainSlotBO[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerLazyCdBO> _list)
            {
                _initBo(_list);

                setInited();
            }
        });
    }

    protected void _initBo(List<PlayerLazyCdBO> _list)
    {
        for (PlayerLazyCdBO bo : _list)
        {
            if (bo.getCdId() == 0)
                continue;

            RefPlayerLazyCd ref = RefPlayerLazyCd.getMgr().get(bo.getCdId());
            if (null == ref)
            {
                USLog.error(getUSServer(), "Init Lazy CD failed,ref not found for type:{}:", bo.getCdId());
                continue;
            }

            PlayerLazyCD cd = new PlayerLazyCD(this, bo, ref);
            _m_lCDList.add(cd);
        }

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
                            for (PlayerLazyCD cdObj : _m_lCDList)
                            {
                                if (null == cdObj)
                                    continue;

                                cdObj._onPlayerPropertyChg(_propertyType);
                            }
                        } finally
                        {
                            getUserData().unlockUser();
                        }
                    }
                });
    }

    //加到内存列表
    private void _addToList(PlayerLazyCD cd)
    {
        _m_lCDList.add(cd);

        GS2GC_021_051_OnAddNewLazyCd proto = new GS2GC_021_051_OnAddNewLazyCd();
        proto.setCdInfo(cd.makeProto());
        getUserData().sendMsgToGC(proto);
    }

    @Override
    public NPCommonEnum.ENPPlayerCompType[] getDependCompList()
    {
        return new ENPPlayerCompType[]{ENPPlayerCompType.PLAYER_COMP};
    }

    @Override
    public void onInited()
    {
        //由于会涉及到属性的变化，所以需要在玩家数据加载完之后进行初始化
        getUserData().safeCall(() ->
        {
            for (RefPlayerLazyCd refCd : RefPlayerLazyCd.getMgr().getList())
            {
                //初始化数据不推送
                ensureLazyCD(refCd.cd_id);
            }
        });
    }

    @Override
    public void dispose()
    {
    	getUserData().getPlayerComponent().getPropertyMgr().propertyChgDelegate().clear(this);
    }

    /*******
     * 查找CD对象
     * @return
     */
    public PlayerLazyCD lookupCD(int _iCdId)
    {
        getUserData().lockUser();

        try
        {
            PlayerLazyCD cdObj = null;
            for (int i = 0; i < _m_lCDList.size(); i++)
            {
                cdObj = _m_lCDList.get(i);
                if (null == cdObj)
                    continue;

                if (cdObj.getCdID() == _iCdId)
                    return cdObj;
            }
        } finally
        {
            getUserData().unlockUser();
        }

        return null;
    }

    /************
     * 查询对应CD，当无数据的时候会创建一个确保有数据
     * @param iCdType
     * @return
     */
    public PlayerLazyCD ensureLazyCD(int iCdType)
    {
        return ensureLazyCD(iCdType, getUserData().getPlayerInitContext());
    }
    /***************
     * 查询对应CD，当无数据的时候会创建一个确保有数据
     * @param _iCdId
     * @param _context
     * @return
     */
    public PlayerLazyCD ensureLazyCD(int _iCdId, NPPlayerContext _context)
    {
        getUserData().lockUser();
        try{
            PlayerLazyCD cd = lookupCD(_iCdId);
            if (null == cd)
            {
                RefPlayerLazyCd ref = RefPlayerLazyCd.getMgr().get(_iCdId);
                if (null == ref)
                {
                    USLog.error(getUSServer(), "can not find cd ref for:{}", _iCdId, new Exception());
                    return null;
                }

                BM bmObj = getUSServer().getBM();

                PlayerLazyCdBO bo = new PlayerLazyCdBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setCdId(bmObj, _iCdId);
                bo.setCount(bmObj, PlayerLazyCD.calMaxCount(true, ref, getUserData().getPlayerComponent().getPropertyMgr().getValue(ref.property_add_count_max)));
                bo.setLastCalcTime(bmObj, CommonFunc.getNowTimeMS());
                bo.insert(bmObj);

                cd = new PlayerLazyCD(this, bo, ref);
                _addToList(cd);
            }
            return cd;
        }finally
        {
            getUserData().unlockUser();
        }
    }

    /*******
     * 填充协议列表
     * @return
     */
    public void fillProto(ArrayList<NPCommon_PlayerLazyCD> cdList)
    {
        //遍历所有数据进行处理
        getUserData().lockUser();

        try
        {
            PlayerLazyCD cdObj = null;
            for (int i = 0; i < _m_lCDList.size(); i++)
            {
                cdObj = _m_lCDList.get(i);
                if (null == cdObj)
                    continue;

                cdList.add(cdObj.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    public String toString()
    {
        //遍历所有数据进行处理
        getUserData().lockUser();

        try
        {
        	StringBuilder sb = new StringBuilder();
        	
            PlayerLazyCD cdObj = null;
            for (int i = 0; i < _m_lCDList.size(); i++)
            {
                cdObj = _m_lCDList.get(i);
                if (null == cdObj)
                    continue;

                sb.append("\n").append(cdObj.toString()).append("\n");
            }
            
            return "LazyCd {" + sb.toString() + "}";
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    public String gmChg(int _cdid, int _chg, NPPlayerContext _context)
    {
        PlayerLazyCD cd = lookupCD(_cdid);
        if (null == cd)
        {
            return "not found";
        }

        //计算差值
        int count = _chg - cd.getCount();
        if (count > 0)
        {
            cd.addCountExt(count);
        } else
        {
            count = -count;
            cd.descCount(count, _context);
        }
        return "ok";
    }


    ///////////////////////////////// 实现物品管理接口 /////////////////////////////////

    //对应处理的类型
    @Override
    public ENPItemType getItemType()
    {
        return ENPItemType.LAZY_CD;
    }

    //获取数量
    @Override
    public long getItemCount(long _itemId)
    {
        PlayerLazyCD cd = lookupCD((int) _itemId);
        if (null == cd)
            return 0;
        return cd.getCount();
    }

    //是否有足够的数量
    @Override
    public boolean hasItem(long _itemId, long _count)
    {
        return getItemCount((int) _itemId) >= _count;
    }

    //初始化的获取物品处理
    @Override
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
    }

    //获取物品的处理
    @Override
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        RefPlayerLazyCd refCd = RefPlayerLazyCd.getMgr().get((int) _itemId);
        if (refCd == null)
            return;
        PlayerLazyCD cd = ensureLazyCD(refCd.cd_id);
        int addCountExt = cd.addCountExt((int) _count);

        //放入数据
        _context.collectItem(getItemType(), _itemId, addCountExt, _isNotMerge);
    }

    //消耗物品的处理
    @Override
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        PlayerLazyCD cd = lookupCD((int) _itemId);
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

        LogLazyCdBO bo = new LogLazyCdBO();
        bo.setCid(bmObj, getUserData().getCid());
        bo.setLogType(bmObj, _logType.ordinal());
        bo.setOriginValue(bmObj, _oldCount);
        bo.setFinalValue(bmObj, _newCount);
        bo.setChgValue(bmObj, _newCount - _oldCount);
        bo.setSubId(bmObj, _subId);
        bo.setVipLvl(bmObj, (int) getUserData().getParam(ENPPlayerParam.VIP_LVL));
        CommLogDB.log(bmObj, bo, _context);
    }

}
