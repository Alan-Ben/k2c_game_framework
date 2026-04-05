package NPUSServer.NPUSUserMgr.UserComp.BubbleComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.NpPlayerInfoObj.PlayerInfo_Bubble;
import NPGameRes.Refs.Player.RefPlayerBubble;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemInfo;
import USDB.Bo.PlayerBubbleBO;

/*****************
 * 玩家气泡框信息存储对象
 * @author mj
 *
 */
public class PlayerBubbleInfo extends _AExpiredItemInfo<RefPlayerBubble>
{
    private PlayerBubbleComponent _m_comp;

    public PlayerBubbleInfo(RefPlayerBubble _ref, PlayerBubbleBO _bo, PlayerBubbleComponent _comp)
    {
        super(_ref, _bo.getId(), _bo.getExpireTimeS(), _bo.getViewed(), _bo.getNeedCheckSendMail());

        _m_comp = _comp;
    }

    @Override
    protected void saveExpiredTimeSecChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("expireTimeS", getExpireTimeSec());
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerBubbleBO.class).update("id", getDbId(), updateValue);

        //如果是玩家当前佩戴的气泡框, 则更新缓存
        _m_comp.checkNeedFreshCacheInfo(getRefId());
    }

    @Override
    protected void saveViewedTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("viewed", getViewed() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerBubbleBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    protected void saveCheckNeedSendMailTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerBubbleBO.class).update("id", getDbId(), updateValue);
    }

    @Override
    public void del()
    {
        _m_comp.getUSServer().getBM().getBM(PlayerBubbleBO.class).delAll("id", getDbId());
    }

    /**
     * 构造协议对象
     * @return
     */
    public PlayerInfo_Bubble makeProto()
    {
        PlayerInfo_Bubble obj = new PlayerInfo_Bubble();
        obj.setId(getRefId());
        obj.setExpireTimeTagS(getExpireTimeSec());
        obj.setViewed(getViewed());
        return obj;
    }
}
