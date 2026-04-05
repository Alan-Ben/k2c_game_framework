package NPUSServer.NPUSUserMgr.UserComp.IconComp;

import Common.CachedObj.CachedObj_CachedIconInfo;
import Common.NpPlayerInfoObj.PlayerInfo_Icon;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Player.RefPlayerIcon;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerIconBO;
import USLOGDB.Bo.LogIconBO;

import java.util.List;


/****************************
 * 用户头像组件
 * @author Administrator
 *
 */
public class PlayerIconComponent extends _AExpiredItemComponent<RefPlayerIcon, PlayerIconInfo>
{
    public PlayerIconComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.ICON_COMP);

    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerIconBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerIconBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Icon Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerIconBO> _list)
            {
                _initBo(_list);
            }
        });
    }

    //初始化数据
    private void _initBo(List<PlayerIconBO> _list)
    {
        //获取当前时间
        //int nowTimeS = ALBasicCommonFun.getNowTime();
        for (int i = 0; i < _list.size(); i++)
        {
            PlayerIconBO bo = _list.get(i);
            if (null == bo)
                continue;

            RefPlayerIcon ref = RefPlayerIcon.getMgr().get(bo.getIconId());
            if (null == ref)
            {
                USLog.error(getUserData().getUSServer(), "Can not get icon ref for bo[id:" + bo.getId() + ", refid:" + bo.getIconId() + "]");
                continue;
            }

            PlayerIconInfo info = new PlayerIconInfo(ref, bo, this);
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
        return ENPItemType.ICON;
    }

    public void logItem(long _subId, long _oldCount, long _newCount, ELogItem_Type _logType, _IContext _context)
    {
        BM bmObj = getUSServer().getBM();

        LogIconBO bo = new LogIconBO();
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
    public void makeProtocol(List<PlayerInfo_Icon> _list)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerIconInfo info : getItemList())
            {
                _list.add(info.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /******************
     * 检测当前玩家使用的头像是否合法，不合法则设置为默认值
     */
    public void checkCurPlayerIcon()
    {
        //获取玩家当前头像
        long curUseIconId = getUserData().getPlayerComponent().getParamV(ENPPlayerParam.ICON);
        if (0 == curUseIconId)
            return;

        //判断是否拥有此头像，如未拥有则重置
        if (checkExpiredItemEnable(curUseIconId))
            return;

        //重置处理
        getUserData().getPlayerComponent().setParam(ENPPlayerParam.ICON, 0);
    }

    /**
     * 构造缓存数据
     * @param _iconId 头像id
     * @return 缓存数据
     */
    public CachedObj_CachedIconInfo makeCachedInfo(long _iconId)
    {
        CachedObj_CachedIconInfo proto = new CachedObj_CachedIconInfo();
        proto.setIconId(_iconId);

        PlayerIconInfo iconInfo = _lookupItem(_iconId);
        if (null == iconInfo)
            return proto;

        proto.setExpiredTimeS(iconInfo.getExpireTimeSec());
        return proto;
    }

    @Override
    public RefPlayerIcon lookupRef(long _refId)
    {
        return RefPlayerIcon.getMgr().get(_refId);
    }

    @Override
    protected PlayerIconInfo _createExpiredItem(RefPlayerIcon _ref, int _expiredTimeS)
    {
        BM bmObj = getUSServer().getBM();

        PlayerIconBO bo = new PlayerIconBO();
        bo.setIconId(bmObj, _ref.id);
        bo.setCid(bmObj, getUserData().getCid());
        bo.setExpireTimeS(bmObj, _expiredTimeS);
        bo.setViewed(bmObj, false);
        bo.setNeedCheckSendMail(bmObj, true);
        bo.insert(bmObj);

        return new PlayerIconInfo(_ref, bo, this);
    }

    @Override
    public void onExpiredItemAdd(PlayerIconInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_006_PlayerIconAdd(_info));
    }

    @Override
    public void onExpiredItemChg(PlayerIconInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_007_PlayerIconChg(_info));
    }

    @Override
    public void onExpiredItemDel(long _refId)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_008_PlayerIconDel(_refId));
    }

    /**
     * 检查是否需要刷新缓存信息
     * @param _refId 头像id
     */
    public void checkNeedFreshCacheInfo(long _refId)
    {
        long iconId = getUserData().getParam(ENPPlayerParam.ICON);
        if (iconId != _refId)
            return;

        PlayerCacheFunc.updateIconInfo(getUserData(), makeCachedInfo(_refId));
    }
}
