package NPUSServer.NPUSUserMgr.UserComp.IconComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.NpPlayerInfoObj.PlayerInfo_Icon;
import NPGameRes.Refs.Player.RefPlayerIcon;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemInfo;
import USDB.Bo.PlayerIconBO;

/*****************
 * 玩家称号信息存储对象
 * @author mj
 *
 */
public class PlayerIconInfo extends _AExpiredItemInfo<RefPlayerIcon>
{
    //数据库数据对象
    private PlayerIconComponent _m_comp;

    public PlayerIconInfo(RefPlayerIcon _ref, PlayerIconBO _bo, PlayerIconComponent _comp)
    {
        super(_ref, _bo.getId(), _bo.getExpireTimeS(), _bo.getViewed(), _bo.getNeedCheckSendMail());

        _m_comp = _comp;
    }

    /**
     * 构造协议对象
     * @return
     */
    public PlayerInfo_Icon makeProto()
    {
        PlayerInfo_Icon obj = new PlayerInfo_Icon();
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
        _m_comp.getUSServer().getBM().getBM(PlayerIconBO.class).update("id", getDbId(), updateValue);

        //如果是玩家当前佩戴的头像, 则更新缓存
        _m_comp.checkNeedFreshCacheInfo(getRefId());
    }

    @Override
    protected void saveViewedTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("viewed", getViewed() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerIconBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    protected void saveCheckNeedSendMailTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerIconBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    public void del()
    {
        _m_comp.getUSServer().getBM().getBM(PlayerIconBO.class).delAll("id", getDbId());
    }
}
