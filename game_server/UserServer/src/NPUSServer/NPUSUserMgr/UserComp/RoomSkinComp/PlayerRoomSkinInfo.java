package NPUSServer.NPUSUserMgr.UserComp.RoomSkinComp;

import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlUpdateValue;
import Common.NpPlayerInfoObj.PlayerInfo_RoomSkin;
import NPGameRes.Refs.Player.RefPlayerRoomSkin;
import NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp._AExpiredItemInfo;
import USDB.Bo.PlayerRoomSkinBO;

/**
 * 玩家房间皮肤信息存储对象
 */
public class PlayerRoomSkinInfo extends _AExpiredItemInfo<RefPlayerRoomSkin>
{
    // 组件引用
    private PlayerRoomSkinComponent _m_comp;

    /**
     * 构造函数 - 从BO对象初始化
     * @param _ref 配置对象
     * @param _bo 数据库对象
     * @param _comp 组件引用
     */
    public PlayerRoomSkinInfo(RefPlayerRoomSkin _ref, PlayerRoomSkinBO _bo, PlayerRoomSkinComponent _comp)
    {
        super(_ref, _bo.getId(), _bo.getExpireTimeS(), _bo.getViewed(), _bo.getNeedCheckSendMail());
        _m_comp = _comp;
    }

    /**
     * 构造协议对象
     * @return 协议对象
     */
    public PlayerInfo_RoomSkin makeProto()
    {
        PlayerInfo_RoomSkin obj = new PlayerInfo_RoomSkin();
        obj.setId(getRefId());
        obj.setExpireTimeTagS(getExpireTimeSec());
        obj.setViewed(getViewed());
        return obj;
    }

    /**
     * 过期时间变更时的数据库更新
     */
    @Override
    protected void saveExpiredTimeSecChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("expireTimeS", getExpireTimeSec());
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerRoomSkinBO.class).update("id", getDbId(), updateValue);
    }

    /**
     * 已查看标记变更时的数据库更新
     */
    @Override
    protected void saveViewedTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("viewed", getViewed() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerRoomSkinBO.class).update("id", getDbId(), updateValue);
    }

    /**
     * 邮件发送标记变更时的数据库更新
     */
    @Override
    protected void saveCheckNeedSendMailTagChg()
    {
        ALMySqlUpdateValue updateValue = new ALMySqlUpdateValue();
        updateValue.addValueObj("need_check_send_mail", getNeedCheckSendMail() ? 1 : 0);
        _m_comp.getUSServer().getBM().getBM(PlayerRoomSkinBO.class).update("id", getDbId(), updateValue);
    }

    /**
     * 删除数据库记录
     */
    @Override
    public void del()
    {
        _m_comp.getUSServer().getBM().getBM(PlayerRoomSkinBO.class).delAll("id", getDbId());
    }
}
