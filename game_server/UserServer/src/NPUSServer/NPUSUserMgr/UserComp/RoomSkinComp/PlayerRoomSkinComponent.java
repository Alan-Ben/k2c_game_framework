package NPUSServer.NPUSUserMgr.UserComp.RoomSkinComp;

import Common.NpPlayerInfoObj.PlayerInfo_RoomSkin;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerParam;
import NPGameRes.Refs.Player.RefPlayerRoomSkin;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerRoomSkinBO;
import USLOGDB.Bo.LogRoomSkinBO;

import java.util.List;

/**
 * 玩家房间皮肤组件
 *
 * 主要功能：
 * 1. 管理玩家拥有的房间皮肤数据
 * 2. 处理房间皮肤的获取、过期和删除
 * 3. 提供房间皮肤的查询和验证接口
 *
 * 线程安全：通过玩家级别锁保护数据一致性
 */
public class PlayerRoomSkinComponent extends _AExpiredItemComponent<RefPlayerRoomSkin, PlayerRoomSkinInfo>
{
    /**
     * 构造函数
     * @param _userData 玩家数据对象
     */
    public PlayerRoomSkinComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.ROOM_SKIN_COMP);
    }

    /**
     * 组件初始化
     * 异步加载数据库数据，初始化完成后设置组件为已初始化状态
     */
    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerRoomSkinBO.class).findAll("cid", getUserData().getCid(),
            new _ASelectCallback<List<PlayerRoomSkinBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "PlayerRoomSkinComponent._init - load data failed: cid={}",
                           getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerRoomSkinBO> _list)
            {
                _initBo(_list);
            }
        });
    }

    /**
     * 初始化数据
     * 从数据库加载的BO对象列表初始化组件数据
     * @param _list BO对象列表
     */
    private void _initBo(List<PlayerRoomSkinBO> _list)
    {
        for (int i = 0; i < _list.size(); i++)
        {
            PlayerRoomSkinBO bo = _list.get(i);
            if (null == bo)
                continue;

            RefPlayerRoomSkin ref = RefPlayerRoomSkin.getMgr().get(bo.getRoomSkinId());
            if (null == ref)
            {
                USLog.error(getUserData().getUSServer(),
                           "PlayerRoomSkinComponent._initBo - ref not found: cid={}, boId={}, refId={}",
                           getUserData().getCid(), bo.getId(), bo.getRoomSkinId());
                continue;
            }

            PlayerRoomSkinInfo info = new PlayerRoomSkinInfo(ref, bo, this);
            _addItemToList(info);
        }

        setInited();
    }

    /**
     * 获取需要依赖的加载组件项
     * @return 依赖组件列表，无依赖则返回null
     */
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    /**
     * 所有组件初始化完成后的回调
     */
    @Override
    public void onInited()
    {
        // 无需特殊处理
    }

    /**
     * 玩家数据释放时的处理
     * 用于清理资源，如注销监听等
     */
    @Override
    public void dispose()
    {
        // 无需特殊处理
    }

    //////////////////////////////
    // ItemDealer处理部分
    //////////////////////////////

    /**
     * 对应处理的物品类型
     * @return 物品类型枚举
     */
    public ENPItemType getItemType()
    {
        return ENPItemType.ROOM_SKIN;
    }

    /**
     * 记录物品变更日志
     * @param _subId 房间皮肤ID
     * @param _oldCount 旧数量
     * @param _newCount 新数量
     * @param _logType 日志类型
     * @param _context 操作上下文
     */
    public void logItem(long _subId, long _oldCount, long _newCount, ELogItem_Type _logType, _IContext _context)
    {
        BM bmObj = getUSServer().getBM();

        LogRoomSkinBO bo = new LogRoomSkinBO();
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
     * 构造返回的协议数据列表
     * @param _list 协议数据列表
     */
    public void makeProtocol(List<PlayerInfo_RoomSkin> _list)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerRoomSkinInfo info : getItemList())
            {
                _list.add(info.makeProto());
            }
        }
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检测当前玩家使用的房间皮肤是否合法
     * 不合法则设置为默认值
     */
    public void checkCurPlayerRoomSkin()
    {
        // 获取玩家当前房间皮肤
        long curUseRoomSkinId = getUserData().getPlayerComponent().getParamV(ENPPlayerParam.ROOM_SKIN);
        if (0 == curUseRoomSkinId)
            return;

        // 判断是否拥有此房间皮肤，如未拥有则重置
        if (checkExpiredItemEnable(curUseRoomSkinId))
            return;

        // 重置处理
        getUserData().getPlayerComponent().setParam(ENPPlayerParam.ROOM_SKIN, 0);
    }

    /**
     * 查找配置对象
     * @param _refId 配置ID
     * @return 配置对象
     */
    @Override
    public RefPlayerRoomSkin lookupRef(long _refId)
    {
        return RefPlayerRoomSkin.getMgr().get(_refId);
    }

    /**
     * 创建过期物品
     * @param _ref 配置对象
     * @param _expiredTimeS 过期时间（秒）
     * @return 新创建的房间皮肤信息对象
     */
    @Override
    protected PlayerRoomSkinInfo _createExpiredItem(RefPlayerRoomSkin _ref, int _expiredTimeS)
    {
        BM bmObj = getUSServer().getBM();

        PlayerRoomSkinBO bo = new PlayerRoomSkinBO();
        bo.setRoomSkinId(bmObj, _ref.id);
        bo.setCid(bmObj, getUserData().getCid());
        bo.setExpireTimeS(bmObj, _expiredTimeS);
        bo.setViewed(bmObj, false);
        bo.setNeedCheckSendMail(bmObj, true);
        bo.insert(bmObj);

        return new PlayerRoomSkinInfo(_ref, bo, this);
    }

    /**
     * 新增过期物品时的推送
     *
     * @param _info 房间皮肤信息
     */
    @Override
    public void onExpiredItemAdd(PlayerRoomSkinInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_092_OnRoomSkinAdd(_info));
    }

    /**
     * 过期物品变更时的推送
     *
     * @param _info 房间皮肤信息
     */
    @Override
    public void onExpiredItemChg(PlayerRoomSkinInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_093_OnRoomSkinChg(_info));
    }

    /**
     * 过期物品删除时的推送
     * @param _refId 房间皮肤ID
     */
    @Override
    public void onExpiredItemDel(long _refId)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_094_OnRoomSkinDel(_refId));
    }
}
