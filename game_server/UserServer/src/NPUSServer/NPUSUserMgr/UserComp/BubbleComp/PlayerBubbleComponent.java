package NPUSServer.NPUSUserMgr.UserComp.BubbleComp;

import Common.CachedObj.CachedObj_CachedBubbleInfo;
import Common.NpPlayerInfoObj.PlayerInfo_Bubble;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Player.RefPlayerBubble;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerBubbleBO;
import USLOGDB.Bo.LogBubbleBO;

import java.util.List;


/****************************
 * 用户气泡框组件
 * @author Administrator
 *
 */
public class PlayerBubbleComponent extends _AExpiredItemComponent<RefPlayerBubble, PlayerBubbleInfo>
{
    public PlayerBubbleComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.BUBBLE_COMP);
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerBubbleBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerBubbleBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Bubble Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerBubbleBO> _list)
            {
                _initBo(_list);
            }
        });
    }

    //初始化数据
    private void _initBo(List<PlayerBubbleBO> _list)
    {
        //获取当前时间
        //int nowTimeS = ALBasicCommonFun.getNowTime();
        for (int i = 0; i < _list.size(); i++)
        {
            PlayerBubbleBO bo = _list.get(i);
            if (null == bo)
                continue;

            RefPlayerBubble ref = RefPlayerBubble.getMgr().get(bo.getBubbleId());
            if (null == ref)
            {
                USLog.error(getUserData().getUSServer(), "Can not get bubble ref for bo[id:" + bo.getId() + ", refid:" + bo.getBubbleId() + "]");
                continue;
            }

            PlayerBubbleInfo info = new PlayerBubbleInfo(ref, bo, this);
            _addItemToList(info);
        }

        setInited();
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


    //region ItemDealer处理部分

    /**
     * 对应处理的类型
     * @return
     */
    public ENPItemType getItemType()
    {
        return ENPItemType.BUBBLE;
    }

    public void logItem(long _subId, long _oldCount, long _newCount, ELogItem_Type _logType, _IContext _context)
    {
        BM bmObj = getUSServer().getBM();

        LogBubbleBO bo = new LogBubbleBO();
        bo.setCid(bmObj, getUserData().getCid());
        bo.setLogType(bmObj, _logType.ordinal());
        bo.setOriginValue(bmObj, _oldCount);
        bo.setFinalValue(bmObj, _newCount);
        bo.setChgValue(bmObj, _newCount - _oldCount);
        bo.setSubId(bmObj, _subId);
        CommLogDB.log(bmObj, bo, _context);
    }

    //endregion ItemDealer处理部分

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////
    /**
     * 构造返回的协议数据
     * @param _list
     */
    public void makeProtocol(List<PlayerInfo_Bubble> _list)
    {
        getUserData().lockUser();
        try
        {
            List<PlayerBubbleInfo> itemList = getItemList();
            for (PlayerBubbleInfo info : itemList)
            {
                _list.add(info.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检测当前玩家使用的气泡框是否合法，不合法则设置为默认值
     */
    public void checkCurPlayerBubble()
    {
        //获取玩家当前头像
        long curUseBubbleId = getUserData().getPlayerComponent().getParamV(ENPPlayerParam.BUBBLE);
        if (0 == curUseBubbleId)
            return;

        //判断当前头像是否有效
        if (checkExpiredItemEnable(curUseBubbleId))
            return;

        //无效则直接重置处理
        getUserData().getPlayerComponent().setParam(ENPPlayerParam.BUBBLE, 0);
    }

    /**
     * 构造缓存数据
     * @param _bubbleId 头像框id
     * @return 缓存数据
     */
    public CachedObj_CachedBubbleInfo makeCachedInfo(long _bubbleId)
    {
        CachedObj_CachedBubbleInfo proto = new CachedObj_CachedBubbleInfo();
        proto.setBubbleId(_bubbleId);

        PlayerBubbleInfo bubbleInfo = _lookupItem(_bubbleId);
        if (null == bubbleInfo)
            return proto;

        proto.setExpiredTimeS(bubbleInfo.getExpireTimeSec());
        return proto;
    }

    @Override
    public RefPlayerBubble lookupRef(long _refId)
    {
        return RefPlayerBubble.getMgr().get(_refId);
    }

    @Override
    protected PlayerBubbleInfo _createExpiredItem(RefPlayerBubble _ref, int _expiredTimeS)
    {
        BM bmObj = getUSServer().getBM();

        PlayerBubbleBO bo = new PlayerBubbleBO();
        bo.setBubbleId(bmObj, _ref.id);
        bo.setCid(bmObj, getUserData().getCid());
        bo.setExpireTimeS(bmObj, _expiredTimeS);
        bo.setViewed(bmObj, false);
        bo.setNeedCheckSendMail(bmObj, true);
        bo.insert(bmObj);

        return new PlayerBubbleInfo(_ref, bo, this);
    }

    @Override
    public void onExpiredItemAdd(PlayerBubbleInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_016_PlayerBubbleAdd(_info));
    }

    @Override
    public void onExpiredItemChg(PlayerBubbleInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_017_PlayerBubbleChg(_info));
    }

    @Override
    public void onExpiredItemDel(long _refId)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_018_PlayerBubbleDel(_refId));
    }

    /**
     * 检查是否需要刷新缓存数据
     * @param _refId 头像框id
     */
    public void checkNeedFreshCacheInfo(long _refId)
    {
        long bubbleId = getUserData().getParam(ENPPlayerParam.BUBBLE);
        if (bubbleId != _refId)
            return;

        PlayerCacheFunc.updateBubbleInfo(getUserData(), makeCachedInfo(_refId));
    }
}
