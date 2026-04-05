package NPUSServer.NPUserMsgDispather.p014_ChildOp;

import AllRpcData.US_Service.Child.GetAdultIsMarried;
import GC2GS.p014_ChildOp.GC2GS_014_030_ReqCidAdultIsMarried;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.USLog;
import RPC._ARpcCallBack;

public class MsgDealer_GC2GS_014_030_ReqCidAdultIsMarried extends NPUserMsgDealer<GC2GS_014_030_ReqCidAdultIsMarried>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_014_030_ReqCidAdultIsMarried _msg)
    {
        //查询查询对象的Cid是否同服务器
        int targetCidUSId = CommonFunc.parseServerTypeIdFromCid(_msg.getCid());

        //跨服发送消息到服务器做处理
        GetAdultIsMarried rpc = new GetAdultIsMarried();
        rpc.req().setCid(_msg.getCid());
        rpc.req().setAdultId(_msg.getAdultId());

        getUSServer().rpc2us().requestToRepeat(targetCidUSId, rpc,
                new _ARpcCallBack<GetAdultIsMarried>()
                {
                    @Override
                    public void call_back(int _errCode, GetAdultIsMarried _rpc)
                    {
                        if(0 != _errCode)
                        {
                            _commiter.commitFailRes(_errCode);
                            return ;
                        }

                        //直接返回消息
                        _commiter.commitSucRes(_rpc.retObj());
                    }
                },
                10,
                () ->
                {
                    USLog.error(getUSServer(), "try get player:{} adultId:{} is married send rpc MarsMineSettle fail.", _msg.getCid(), _msg.getAdultId());
                });
    }
}