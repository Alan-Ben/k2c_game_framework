package NPUSServer.NPUSUserMgr.UserComp.FriendComp.FriendMgr;

import Common.FriendObj.Friend_Info;
import NPUSServer.NPUSUserMgr.UserComp.FriendComp.FriendComponent;
import USDB.Bo.PlayerFriendBO;

public class FriendInfo
{
	//好友组件对象
    private FriendComponent _m_comp;
    //好友CID
    private long _m_lFCid;

    public FriendInfo(FriendComponent _comp, PlayerFriendBO _bo)
    {
        _m_comp = _comp;

        _m_lFCid = _bo.getFcid();
    }

    public FriendComponent getComp() {return _m_comp;}
    public long getFCid() {return _m_lFCid;}

    /**
     * 构造协议对象
     * @return
     */
    public Friend_Info toProto()
    {
        Friend_Info proto = new Friend_Info();
        proto.setFcid(_m_lFCid);

        return proto;
    }
}
