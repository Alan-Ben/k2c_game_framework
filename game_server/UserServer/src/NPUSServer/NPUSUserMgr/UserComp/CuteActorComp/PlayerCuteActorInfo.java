package NPUSServer.NPUSUserMgr.UserComp.CuteActorComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.NpPlayerInfoObj.PlayerInfo_CuteActor;
import NPGameRes.Refs.Player.RefPlayerCuteActor;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemInfo;
import USDB.Bo.PlayerCuteActorBO;

/**
 * 玩家Q版形象数据对象
 */
public class PlayerCuteActorInfo extends _AExpiredItemInfo<RefPlayerCuteActor>
{
    private PlayerCuteActorComponent _m_comp;

    public PlayerCuteActorInfo(RefPlayerCuteActor _ref, PlayerCuteActorBO _bo, PlayerCuteActorComponent _comp)
    {
        super(_ref, _bo.getId(), _bo.getExpireTimeSec(), _bo.getViewed(), _bo.getNeedCheckSendMail());

        _m_comp = _comp;
    }

    @Override
    protected void saveExpiredTimeSecChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("expire_time_sec", getExpireTimeSec());
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerCuteActorBO.class).update("id", getDbId(), updateValue);

        //如果是玩家当前佩戴的Q版形象, 则更新缓存
        _m_comp.checkNeedFreshCacheInfo(getRefId());
    }

    @Override
    protected void saveViewedTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("viewed", getViewed() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerCuteActorBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    protected void saveCheckNeedSendMailTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerCuteActorBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    public void del()
    {
        _m_comp.getUSServer().getBM().getBM(PlayerCuteActorBO.class).delAll("id", getDbId());
    }

    public PlayerInfo_CuteActor makeProto()
    {
        PlayerInfo_CuteActor obj = new PlayerInfo_CuteActor();
        obj.setId(getRefId());
        obj.setExpireTimeTagS(getExpireTimeSec());
        obj.setViewed(getViewed());
        return obj;
    }
}
