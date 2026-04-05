package NPUSServer.NPUserMsgDispather.p039_MarsBuildingOp;

import GC2GS.p039_MarsBuildingOp.GC2GS_039_014_ReqBagUseItemForTimeReduce;
import NPCommon.CommonObj.NPItemCollector;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.NPCommon_ItemInfo;
import NPEnum.ENPGameEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp.TimeReduce.TimeReduceDealerMgr;
import NPUSServer.NPUSUserMgr.UserComp.MarsComp.TimeReduce._ATimeReduceDealer;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_039_MarsBuildingOp;
import USLOGDB.OptBo.Opt039014MarsBagitemTimeReduceBO;

public class MsgDealer_GC2GS_039_014_ReqBagUseItemForTimeReduce extends NPUserMsgDealer<GC2GS_039_014_ReqBagUseItemForTimeReduce>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_039_014_ReqBagUseItemForTimeReduce _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();
        
        //参数物品数量检查
        if(!userData.checkItemCount(_msg.getUseItemList()))
        {
            _commiter.commitFailRes(CommErr.PARAM_NUM_ERROR.getCode());
            return;
        }
        
        //检查处理对象
        _ATimeReduceDealer dealer = TimeReduceDealerMgr.getInstance().getDealer(_msg.getObjType());
        if(null == dealer)
        {
            _commiter.commitFailRes(CommErr.OBJ_ERR.getCode());
            return;
        }
        
        //重新整理物品列表
        NPItemCollector collector = new NPItemCollector(0);
        for(int i = 0; i < _msg.getUseItemList().size(); i++)
        {
        	NPCommon_ItemInfo item = _msg.getUseItemList().get(i);
        	if(null == item)
        		continue;
        	
        	collector.addItem(item);
        }
        
        //处理流程
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.USE_BAG_ITEM_FOR_TIME_REDUCE);
        Result result = dealer.deal(userData, collector.getAllItemList(), _msg.getObjId(), context);
        if(!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }
        
        _commiter.commitSucRes(US2GCWriter_039_MarsBuildingOp.make_014_RetBagUseItemForTimeReduce());

        //日志数据
        Opt039014MarsBagitemTimeReduceBO optBo = new Opt039014MarsBagitemTimeReduceBO();
        optBo.setObjType(getUSServer().getBM(), _msg.getObjType().ordinal());
        optBo.setObjId(getUSServer().getBM(), _msg.getObjId());
        _commiter.getUserData().logEvent(optBo, context);
    }
}
