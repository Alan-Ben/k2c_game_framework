package NPUSServer.NPUSUserMgr.UserComp.IconBgkComp;

import Common.CachedObj.CachedObj_CachedIconBgkInfo;
import Common.NpPlayerInfoObj.PlayerInfo_IconBgk;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Player.RefPlayerIconBgk;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerIconBgkBO;
import USLOGDB.Bo.LogIconBgkBO;

import java.util.ArrayList;
import java.util.List;


/****************************
 * 用户头像框组件
 * @author Administrator
 *
 */
public class PlayerIconBgkComponent extends _AExpiredItemComponent<RefPlayerIconBgk, PlayerIconBgkInfo>
{
    public PlayerIconBgkComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.ICON_BGK_COMP);
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerIconBgkBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerIconBgkBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load IconBgk Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerIconBgkBO> _list)
            {
                _initBo(_list);
            }
        });
    }

    //初始化数据
    private void _initBo(List<PlayerIconBgkBO> _list)
    {
        //获取当前时间
        //int nowTimeS = ALBasicCommonFun.getNowTime();
        for (int i = 0; i < _list.size(); i++)
        {
            PlayerIconBgkBO bo = _list.get(i);
            if (null == bo)
                continue;

            RefPlayerIconBgk ref = RefPlayerIconBgk.getMgr().get(bo.getIconBgkId());
            if (null == ref)
            {
                USLog.error(getUserData().getUSServer(), "Can not get iconBgk ref for bo[id:" + bo.getId() + ", refid:" + bo.getIconBgkId() + "]");
                continue;
            }

            PlayerIconBgkInfo info = new PlayerIconBgkInfo(ref, bo, this);
            _addItemToList(info);
        }

        setInited();
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
        return ENPItemType.ICON_BGK;
    }

    public void logItem(long _subId, long _oldCount, long _newCount, ELogItem_Type _logType, _IContext _context)
    {
        BM bmObj = getUSServer().getBM();

        LogIconBgkBO bo = new LogIconBgkBO();
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

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////

    /**
     * 构造返回的协议数据
     * @param _list
     */
    public void makeProtocol(ArrayList<PlayerInfo_IconBgk> _list)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerIconBgkInfo info : getItemList())
            {
                _list.add(info.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /******************
     * 检测当前玩家使用的头像框是否合法，不合法则设置为默认值
     */
    public void checkCurPlayerIconBgk()
    {
        //获取玩家当前头像
        long curUseIconBgkId = getUserData().getPlayerComponent().getParamV(ENPPlayerParam.ICON_BGK);
        if (0 == curUseIconBgkId)
            return;

        //判断是否拥有此头像框，如未拥有则重置
        if (checkExpiredItemEnable(curUseIconBgkId))
            return;

        //重置处理
        getUserData().getPlayerComponent().setParam(ENPPlayerParam.ICON_BGK, 0);
    }

    /**
     * 构造缓存数据
     * @param _iconBgkId 头像框id
     * @return 缓存数据
     */
    public CachedObj_CachedIconBgkInfo makeCachedInfo(long _iconBgkId)
    {
        CachedObj_CachedIconBgkInfo proto = new CachedObj_CachedIconBgkInfo();
        proto.setIconBgkId(_iconBgkId);

        PlayerIconBgkInfo iconBgkInfo = _lookupItem(_iconBgkId);
        if (null == iconBgkInfo)
            return proto;

        proto.setExpiredTimeS(iconBgkInfo.getExpireTimeSec());
        return proto;
    }

    @Override
    public RefPlayerIconBgk lookupRef(long _refId)
    {
        return RefPlayerIconBgk.getMgr().get(_refId);
    }

    @Override
    protected PlayerIconBgkInfo _createExpiredItem(RefPlayerIconBgk _ref, int _expiredTimeS)
    {
        BM bmObj = getUSServer().getBM();

        PlayerIconBgkBO bo = new PlayerIconBgkBO();
        bo.setIconBgkId(bmObj, _ref.id);
        bo.setCid(bmObj, getUserData().getCid());
        bo.setExpireTimeS(bmObj, _expiredTimeS);
        bo.setViewed(bmObj, false);
        bo.setNeedCheckSendMail(bmObj, true);
        bo.insert(bmObj);

        return new PlayerIconBgkInfo(_ref, bo, this);
    }

    @Override
    public void onExpiredItemAdd(PlayerIconBgkInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_011_PlayerIconBgkAdd(_info));
    }

    @Override
    public void onExpiredItemChg(PlayerIconBgkInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_012_PlayerIconBgkChg(_info));
    }

    @Override
    public void onExpiredItemDel(long _refId)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_014_PlayerIconBgkDel(_refId));
    }

    /**
     * 检查是否需要刷新缓存信息
     * @param _refId 头像id
     */
    public void checkNeedFreshCacheInfo(long _refId)
    {
        long iconBgkId = getUserData().getParam(ENPPlayerParam.ICON_BGK);
        if (iconBgkId != _refId)
            return;

        PlayerCacheFunc.updateIconBgkInfo(getUserData(), makeCachedInfo(_refId));
    }
}
