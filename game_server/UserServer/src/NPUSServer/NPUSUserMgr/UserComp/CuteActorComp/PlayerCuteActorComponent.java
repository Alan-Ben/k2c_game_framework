package NPUSServer.NPUSUserMgr.UserComp.CuteActorComp;

import Common.CachedObj.CachedObj_CachedCuteActorInfo;
import Common.NpPlayerInfoObj.PlayerInfo_CuteActor;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Player.RefPlayerCuteActor;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerCuteActorBO;

import java.util.List;


/**
 * 玩家Q版形象组件
 */
public class PlayerCuteActorComponent extends _AExpiredItemComponent<RefPlayerCuteActor, PlayerCuteActorInfo>
{
    public PlayerCuteActorComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.CUTE_ACTOR);
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerCuteActorBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerCuteActorBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load CuteActor Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerCuteActorBO> _list)
            {
                _initBo(_list);
            }
        });
    }

    //初始化数据
    private void _initBo(List<PlayerCuteActorBO> _list)
    {
        for (PlayerCuteActorBO bo : _list)
        {
            if (null == bo)
                continue;

            RefPlayerCuteActor ref = RefPlayerCuteActor.getMgr().get(bo.getRefId());
            if (null == ref)
            {
                USLog.error(getUSServer(), "PlayerCuteActorComponent _initBo ref not found, cid:{} refId:{}", getUserData().getCid(), bo.getRefId());
                continue;
            }

            PlayerCuteActorInfo info = new PlayerCuteActorInfo(ref, bo, this);
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

    /**
     * 对应处理的物品类型
     * @return 物品类型
     */
    public ENPItemType getItemType()
    {
        return ENPItemType.CUTE_ACTOR;
    }

    /**
     * 构造返回的协议数据
     * @param _list 协议数据
     */
    public void makeProtocol(List<PlayerInfo_CuteActor> _list)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerCuteActorInfo itemInfo : getItemList())
            {
                _list.add(itemInfo.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    public RefPlayerCuteActor lookupRef(long _refId)
    {
        return RefPlayerCuteActor.getMgr().get(_refId);
    }

    @Override
    protected PlayerCuteActorInfo _createExpiredItem(RefPlayerCuteActor _ref, int _expiredTimeS)
    {
        BM bmObj = getUSServer().getBM();

        PlayerCuteActorBO bo = new PlayerCuteActorBO();
        bo.setRefId(bmObj, _ref.id);
        bo.setCid(bmObj, getUserData().getCid());
        bo.setExpireTimeSec(bmObj, _expiredTimeS);
        bo.setViewed(bmObj, false);
        bo.setNeedCheckSendMail(bmObj, true);
        bo.insert(bmObj);
        return new PlayerCuteActorInfo(_ref, bo, this);
    }

    @Override
    public void onExpiredItemAdd(PlayerCuteActorInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_090_OnCuteActorAdd(_info.makeProto()));
    }

    @Override
    public void onExpiredItemChg(PlayerCuteActorInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_089_OnCuteActorChg(_info.makeProto()));
    }

    @Override
    public void onExpiredItemDel(long _refId)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_088_OnCuteActorDel(_refId));
    }

    /**
     * 构造缓存数据
     * @param _cuteActorId 头像框id
     * @return 缓存数据
     */
    public CachedObj_CachedCuteActorInfo makeCachedInfo(long _cuteActorId)
    {
        CachedObj_CachedCuteActorInfo proto = new CachedObj_CachedCuteActorInfo();
        proto.setCuteActorId(_cuteActorId);

        PlayerCuteActorInfo cuteActorInfo = _lookupItem(_cuteActorId);
        if (null == cuteActorInfo)
            return proto;

        proto.setExpiredTimeS(cuteActorInfo.getExpireTimeSec());
        return proto;
    }

    /**
     * 检查是否需要刷新缓存数据
     * @param _refId 头像框id
     */
    public void checkNeedFreshCacheInfo(long _refId)
    {
        long cuteActorId = getUserData().getParam(ENPPlayerParam.CUTE_ACTOR);
        if (cuteActorId != _refId)
            return;

        PlayerCacheFunc.updateCuteActorInfo(getUserData(), makeCachedInfo(_refId));
    }

    /**
     * 检测当前玩家使用的是否合法，不合法则设置为默认值
     */
    public void checkCurPlayerCuteActor()
    {
        //获取玩家当前id
        long curUseCuteActorId = getUserData().getPlayerComponent().getParamV(ENPPlayerParam.CUTE_ACTOR);
        if (0 == curUseCuteActorId)
            return;

        //判断当前id是否有效
        if (checkExpiredItemEnable(curUseCuteActorId))
            return;

        //无效则直接重置处理
        getUserData().getPlayerComponent().setParam(ENPPlayerParam.CUTE_ACTOR, 0);
    }
}
