package NPUSServer.NPUserMsgDispather.p041_MarsExploreOp;

import GC2GS.p041_MarsExploreOp.GC2GS_041_013_ReqNoticeMarsMine;
import NPCommon.ErrMain.MarsErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MarsMineSystem.MarsMineSystem;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_041_MarsExploreOp;

public class MsgDealer_GC2GS_041_013_ReqNoticeMarsMine extends NPUserMsgDealer<GC2GS_041_013_ReqNoticeMarsMine>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_041_013_ReqNoticeMarsMine _msg)
    {
        NPUSUserData userData = _commiter.getUserData();
        if (null == userData)
            return;
        
        MarsMineSystem.GetMarsMine(getUSServer(), _msg.getId(), (err, p) -> 
        {
        	//火星矿已经移除
        	if(err == MarsErr.MARS_MINE_NOT_FOUND.getCode())
        	{
        		userData.getMarsMineComponent().removeMine(_msg.getId(), NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));

				_commiter.commitFailRes(err);
        		return;
        	}
        	
        	//其他错误
        	if(err > 0)
        	{
        		_commiter.commitFailRes(err);
        		return;
        	}
        	
        	//检查玩家火星矿数据
        	if(p.getCid() != userData.getCid() && CommonFunc.getNowTimeMS() > p.getEndShowMs()) //没有被自己的矿占领且超过了展示时间，移除该矿数据
        	{
        		userData.getMarsMineComponent().removeMine(_msg.getId(), NPPlayerContext.createNew(ENPGameEvent.MARS_MINE_CHECK));

				//返回错误
				_commiter.commitFailRes(MarsErr.MARS_MINE_NOT_FOUND.getCode());
        	}
        	else //矿还在，推送最新的数据给客户端
        	{
				//判断检测状态序列号是否有效，有效且一致则不返回
				if(_msg.getCheckSerialize() > 0 && _msg.getCheckSerialize() == p.getSerialize())
					_commiter.commitFailRes(Result.SUCC.getCode());
				else
					_commiter.commitSucRes(US2GCWriter_041_MarsExploreOp.make_013_RetNoticeMarsMine(p));
        	}

        });
    }
}
