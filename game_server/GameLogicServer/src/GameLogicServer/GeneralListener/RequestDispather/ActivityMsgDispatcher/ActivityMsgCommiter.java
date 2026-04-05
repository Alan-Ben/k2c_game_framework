package GameLogicServer.GeneralListener.RequestDispather.ActivityMsgDispatcher;

import ALBasicProtocolPack.BasicObj._IALProtocolReceiver;
import ALBasicProtocolPack._IALProtocolStructure;
import GameLogicServer.GroupMgr.ActivityMgr._ATActivityInfo;
import NP2GLS_R.p001_BasicOp.NP2GLS_RB_001_020_DealMsg;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.nio.ByteBuffer;


public class ActivityMsgCommiter implements _IWCGBasicRequestCommiter
{
    //提交对象
    private _IWCGBasicRequestCommiter _m_cCommiter;
    //玩家CID
    private long _m_lCid;
    //活动实例信息
    private _ATActivityInfo _m_aActivity;
    //附加信息的内容
    private ByteBuffer _m_addInfo;

    public ActivityMsgCommiter(_IWCGBasicRequestCommiter _commiter, long _cid, _ATActivityInfo _activity, ByteBuffer _addInfo)
    {
        this._m_cCommiter = _commiter;
        this._m_lCid = _cid;
        this._m_aActivity = _activity;
        this._m_addInfo = _addInfo;
    }

    public long getCid() {return _m_lCid;}
    public _ATActivityInfo getActivity() {return _m_aActivity;}
    public ByteBuffer getAddInfo() {return _m_addInfo;}

    @Override
    public _IALProtocolReceiver getRequestDealer()
    {
        return _m_cCommiter.getRequestDealer();
    }

    @Override
    public void commitSucRes(_IALProtocolStructure _retMsg)
    {
        _m_cCommiter.commitSucRes(new NP2GLS_RB_001_020_DealMsg(_retMsg.makeFullPackage().array()));
    }

    @Override
    public void commitFailRes(int _errCode)
    {
        _m_cCommiter.commitFailRes(_errCode);
    }
}
