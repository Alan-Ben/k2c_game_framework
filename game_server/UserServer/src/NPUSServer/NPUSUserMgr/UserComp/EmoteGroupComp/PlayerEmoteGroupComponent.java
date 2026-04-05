package NPUSServer.NPUSUserMgr.UserComp.EmoteGroupComp;

import Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Player.RefPlayerEmoteGroup;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_021_PlayerInfo;
import NPUSServer.USLog;
import USDB.Bo.PlayerEmoteGroupBO;

import java.util.List;


/**
 * 玩家表情包组件
 */
public class PlayerEmoteGroupComponent extends _AExpiredItemComponent<RefPlayerEmoteGroup, PlayerEmoteGroupInfo>
{
    public PlayerEmoteGroupComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.EMOTE_GROUP);
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerEmoteGroupBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerEmoteGroupBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load EmoteGroup Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerEmoteGroupBO> _list)
            {
                _initBo(_list);
            }
        });
    }

    //初始化数据
    private void _initBo(List<PlayerEmoteGroupBO> _list)
    {
        for (PlayerEmoteGroupBO bo : _list)
        {
            if (null == bo)
                continue;

            RefPlayerEmoteGroup ref = RefPlayerEmoteGroup.getMgr().get(bo.getRefId());
            if (null == ref)
            {
                USLog.error(getUSServer(), "PlayerEmoteGroupComponent _initBo ref not found, cid:{} refId:{}", getUserData().getCid(), bo.getRefId());
                continue;
            }

            PlayerEmoteGroupInfo info = new PlayerEmoteGroupInfo(getUSServer(), ref, bo);
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
        return ENPItemType.CHAT_EMOTE_GROUP;
    }

    /**
     * 构造返回的协议数据
     * @param _list 协议数据
     */
    public void makeProtocol(List<PlayerInfo_ChatEmoteGroup> _list)
    {
        getUserData().lockUser();
        try
        {
            for (PlayerEmoteGroupInfo itemInfo : getItemList())
            {
                _list.add(itemInfo.makeProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    public RefPlayerEmoteGroup lookupRef(long _refId)
    {
        return RefPlayerEmoteGroup.getMgr().get(_refId);
    }

    @Override
    protected PlayerEmoteGroupInfo _createExpiredItem(RefPlayerEmoteGroup _ref, int _expiredTimeS)
    {
        BM bmObj = getUSServer().getBM();

        PlayerEmoteGroupBO bo = new PlayerEmoteGroupBO();
        bo.setRefId(bmObj, _ref.id);
        bo.setCid(bmObj, getUserData().getCid());
        bo.setExpireTimeSec(bmObj, _expiredTimeS);
        bo.setViewed(bmObj, false);
        bo.setNeedCheckSendMail(bmObj, true);
        bo.insert(bmObj);
        return new PlayerEmoteGroupInfo(getUSServer(), _ref, bo);
    }

    @Override
    public void onExpiredItemAdd(PlayerEmoteGroupInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_087_OnChatEmoteGroupAdd(_info.makeProto()));
    }

    @Override
    public void onExpiredItemChg(PlayerEmoteGroupInfo _info)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_084_OnChatEmoteGroupChg(_info.makeProto()));
    }

    @Override
    public void onExpiredItemDel(long _refId)
    {
        getUserData().sendMsgToGC(US2GCWriter_021_PlayerInfo.make_083_OnChatEmoteGroupDel(_refId));
    }
}
