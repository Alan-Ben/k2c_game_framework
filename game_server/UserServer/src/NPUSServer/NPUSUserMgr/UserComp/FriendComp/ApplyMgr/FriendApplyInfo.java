package NPUSServer.NPUSUserMgr.UserComp.FriendComp.ApplyMgr;

import Common.FriendObj.Friend_ApplyInfo;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.FriendComponent;
import USDB.Bo.PlayerFriendApplyBO;

public class FriendApplyInfo
{
    private FriendComponent _m_comp;
    //主键ID
    private long _m_lDbid;
    //申请数据
    private long _m_lApplyCid;
    //申请时间
    private int _m_iApplyTimeS;

    public FriendApplyInfo(FriendComponent _comp, PlayerFriendApplyBO _bo)
    {
        _m_comp = _comp;

        _m_lDbid = _bo.getId();
        _m_lApplyCid = _bo.getApplyCid();
        _m_iApplyTimeS = _bo.getApplyTimeS();
    }

    public FriendComponent getComp() {return _m_comp;}
    public long getDbid() {return _m_lDbid;}
    public long getApplyCid() {return _m_lApplyCid;}
    public int getApplyTimeS() {return _m_iApplyTimeS;}

    /**
     * 构造协议数据
     * @return
     */
    public Friend_ApplyInfo toProto()
    {
        Friend_ApplyInfo proto = new Friend_ApplyInfo();
        proto.setApplyCid(_m_lApplyCid);
        proto.setApplyTimeS(_m_iApplyTimeS);

        return proto;
    }

    /**
     * 移除数据
     */
    public void del()
    {
        getComp().getUSServer().getBM().getBM(PlayerFriendApplyBO.class).delAll("id", _m_lDbid);
    }
    
    /**
     * 检查好友申请是否超时
     * @return
     */
    public boolean isExpired()
    {
    	return CommonFunc.getNowTimeSec() > _m_iApplyTimeS + RefGeneral.Ref().friend_apply_avaiable_secs;
    }
}
