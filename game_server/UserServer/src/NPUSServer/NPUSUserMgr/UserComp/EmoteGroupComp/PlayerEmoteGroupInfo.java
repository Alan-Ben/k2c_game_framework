package NPUSServer.NPUSUserMgr.UserComp.EmoteGroupComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.NpPlayerInfoObj.PlayerInfo_ChatEmoteGroup;
import NPGameRes.Refs.Player.RefPlayerEmoteGroup;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemInfo;
import NPUSServer.NPUserServer;
import USDB.Bo.PlayerEmoteGroupBO;

/**
 * 玩家表情包数据对象
 */
public class PlayerEmoteGroupInfo extends _AExpiredItemInfo<RefPlayerEmoteGroup>
{
    private NPUserServer _m_server;

    public PlayerEmoteGroupInfo(NPUserServer _server, RefPlayerEmoteGroup _ref, PlayerEmoteGroupBO _bo)
    {
        super(_ref, _bo.getId(), _bo.getExpireTimeSec(), _bo.getViewed(), _bo.getNeedCheckSendMail());

        _m_server = _server;
    }

    public NPUserServer getUSServer() {return _m_server;}

    @Override
    protected void saveExpiredTimeSecChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("expire_time_sec", getExpireTimeSec());
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        getUSServer().getBM().getBM(PlayerEmoteGroupBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    protected void saveViewedTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("viewed", getViewed() ? 1 : 0);
        getUSServer().getBM().getBM(PlayerEmoteGroupBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    protected void saveCheckNeedSendMailTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        getUSServer().getBM().getBM(PlayerEmoteGroupBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    public void del()
    {
        getUSServer().getBM().getBM(PlayerEmoteGroupBO.class).delAll("id", getDbId());
    }

    public PlayerInfo_ChatEmoteGroup makeProto()
    {
        PlayerInfo_ChatEmoteGroup obj = new PlayerInfo_ChatEmoteGroup();
        obj.setId(getRefId());
        obj.setExpireTimeTagS(getExpireTimeSec());
        obj.setViewed(getViewed());
        return obj;
    }
}
