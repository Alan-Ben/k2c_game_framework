package NPUSServer.NPUSUserMgr.UserComp.IconBgkComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.NpPlayerInfoObj.PlayerInfo_IconBgk;
import NPGameRes.Refs.Player.RefPlayerIconBgk;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemInfo;
import USDB.Bo.PlayerIconBgkBO;

/*****************
 * 玩家头像框信息存储对象
 * @author mj
 *
 */
public class PlayerIconBgkInfo extends _AExpiredItemInfo<RefPlayerIconBgk>
{
    private PlayerIconBgkComponent _m_comp;

    public PlayerIconBgkInfo(RefPlayerIconBgk _ref, PlayerIconBgkBO _bo, PlayerIconBgkComponent _comp)
    {
        super(_ref, _bo.getId(), _bo.getExpireTimeS(), _bo.getViewed(), _bo.getNeedCheckSendMail());
        _m_comp = _comp;
    }

    /**
     * 构造协议对象
     * @return
     */
    public PlayerInfo_IconBgk makeProto()
    {
        PlayerInfo_IconBgk obj = new PlayerInfo_IconBgk();
        obj.setId(getRefId());
        obj.setExpireTimeTagS(getExpireTimeSec());
        obj.setViewed(getViewed());
        return obj;
    }

    @Override
    protected void saveExpiredTimeSecChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("expireTimeS", getExpireTimeSec());
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerIconBgkBO.class).update("id", getDbId(), updateValue);

        //如果是玩家当前佩戴的头像框, 则更新缓存
        _m_comp.checkNeedFreshCacheInfo(getRefId());
    }

    @Override
    protected void saveViewedTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("viewed", getViewed() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerIconBgkBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    protected void saveCheckNeedSendMailTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerIconBgkBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    public void del()
    {
        _m_comp.getUSServer().getBM().getBM(PlayerIconBgkBO.class).delAll("id", getDbId());
    }
}
