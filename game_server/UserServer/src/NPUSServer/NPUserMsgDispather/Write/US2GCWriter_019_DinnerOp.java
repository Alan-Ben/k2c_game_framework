package NPUSServer.NPUserMsgDispather.Write;

import Common.DinnerObj.Dinner_Idx;
import Common.DinnerObj.Dinner_ResultInfo;
import Common.DinnerObj.Dinner_StartLogIdx;
import Common.DinnerObj.Dinner_StartLogInfo;
import Common.ServerObj.ServerObj_DinnerJoiner;
import GS2GC.p019_DinnerOp.*;
import NPGameRes.GameObjs.Dinner.DinnerGetInfoResult;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.DinnerMgr.DinnerInfo;
import NPUSServer.DinnerMgr.DinnerJoiner;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.DinnerComp.DinnerPermitInfo;

import java.util.ArrayList;
import java.util.List;

/**
 * 018 衣橱协议 writer
 */
public class US2GCWriter_019_DinnerOp
{
	public static GS2GC_019_001_RetStartDinner make_001_RetStartDinner(DinnerInfo _info)
    {
		GS2GC_019_001_RetStartDinner proto = new GS2GC_019_001_RetStartDinner();
        proto.setInfo(_info.toProto(0));
        
        return proto;
    }

	public static GS2GC_019_002_RetStartDinnerByPermit make_002_RetStartDinnerByPermit(DinnerInfo _info)
    {
		GS2GC_019_002_RetStartDinnerByPermit proto = new GS2GC_019_002_RetStartDinnerByPermit();
        proto.setInfo(_info.toProto(0));
        
        return proto;
    }

	public static GS2GC_019_003_RetJoinedPlayerList make_003_RetJoinedPlayerList(NPUSUserData _userData)
    {
		GS2GC_019_003_RetJoinedPlayerList proto = new GS2GC_019_003_RetJoinedPlayerList();
        _userData.getDinnerComponent().makeLastEachLogProto(proto.getJoinerList());
        
        return proto;
    }

	public static GS2GC_019_004_RetGetStartLogIdxList make_004_RetGetStartLogIdxList(List<Dinner_StartLogIdx> _list)
    {
		GS2GC_019_004_RetGetStartLogIdxList proto = new GS2GC_019_004_RetGetStartLogIdxList();
        for(int i = 0; i < _list.size(); i++)
        {
        	Dinner_StartLogIdx idx = _list.get(i);
        	if(null == idx)
        		continue;
        	
        	proto.addIdxList(idx);
        }
        
        return proto;
    }

	public static GS2GC_019_005_RetGetStartLogInfo make_005_RetGetStartLogInfo(Dinner_StartLogInfo _info)
    {
		GS2GC_019_005_RetGetStartLogInfo proto = new GS2GC_019_005_RetGetStartLogInfo();
        proto.setInfo(_info);
        
        return proto;
    }

	public static GS2GC_019_006_RetGetDinnerIdxList make_006_RetGetDinnerIdxList(ArrayList<Dinner_Idx> _idxList, boolean _hasNext)
    {
		GS2GC_019_006_RetGetDinnerIdxList proto = new GS2GC_019_006_RetGetDinnerIdxList();
        for(int i = 0; i < _idxList.size(); i++)
        {
        	Dinner_Idx idx = _idxList.get(i);
        	if(null == idx)
        		continue;
        	
        	proto.addIdxLIst(idx);
        }
        proto.setHasNext(_hasNext);
        
        return proto;
    }

	public static GS2GC_019_007_RetGetDinnerInfo make_007_RetGetDinnerInfo(DinnerGetInfoResult _dinnerResult)
    {
		GS2GC_019_007_RetGetDinnerInfo proto = new GS2GC_019_007_RetGetDinnerInfo();
        proto.setInfo(_dinnerResult.getDinner());
        proto.setIdx(_dinnerResult.getIdx());
        proto.setHasPre(_dinnerResult.hasPre());
        proto.setHasNext(_dinnerResult.hasNext());
        
        return proto;
    }

	public static GS2GC_019_008_RetJoinDinner make_008_RetJoinDinner(NPPlayerContext _context)
    {
		GS2GC_019_008_RetJoinDinner proto = new GS2GC_019_008_RetJoinDinner();
		_context.getCollector().fillProtoList(proto.getGainItemList());
        
        return proto;
    }
	
	public static GS2GC_019_009_RetTakeOpenReward make_009_RetTakeOpenReward(Dinner_ResultInfo _result)
    {
		GS2GC_019_009_RetTakeOpenReward proto = new GS2GC_019_009_RetTakeOpenReward();
        proto.setResult(_result);
        
        return proto;
    }

	public static GS2GC_019_011_RetGetPreDinnerInfo make_011_RetGetPreDinnerInfo(DinnerGetInfoResult _dinnerResult)
    {
		GS2GC_019_011_RetGetPreDinnerInfo proto = new GS2GC_019_011_RetGetPreDinnerInfo();
		proto.setInfo(_dinnerResult.getDinner());
        proto.setIdx(_dinnerResult.getIdx());
        proto.setHasPre(_dinnerResult.hasPre());
        proto.setHasNext(_dinnerResult.hasNext());
        
        return proto;
    }
	
	public static GS2GC_019_012_RetGetNextDinnerInfo make_012_RetGetNextDinnerInfo(DinnerGetInfoResult _dinnerResult)
    {
		GS2GC_019_012_RetGetNextDinnerInfo proto = new GS2GC_019_012_RetGetNextDinnerInfo();
		proto.setInfo(_dinnerResult.getDinner());
        proto.setIdx(_dinnerResult.getIdx());
        proto.setHasPre(_dinnerResult.hasPre());
        proto.setHasNext(_dinnerResult.hasNext());
        
        return proto;
    }
	
	public static GS2GC_019_013_RetSendInviteToPlayer make_013_RetSendInviteToPlayer(long _msgId)
    {
		GS2GC_019_013_RetSendInviteToPlayer proto = new GS2GC_019_013_RetSendInviteToPlayer();
		proto.setMsgId(_msgId);
		
        return proto;
    }
	
	public static GS2GC_019_014_RetCheckDinnerInvite make_014_RetCheckDinnerInvite(Dinner_Idx _dinnerIdx)
    {
		GS2GC_019_014_RetCheckDinnerInvite proto = new GS2GC_019_014_RetCheckDinnerInvite();
		proto.setJoinerCount(_dinnerIdx.getJoinerCount());
		
        return proto;
    }
	
	public static GS2GC_019_050_OnDinnerAdd make_050_OnDinnerAdd(DinnerInfo _info)
    {
		GS2GC_019_050_OnDinnerAdd proto = new GS2GC_019_050_OnDinnerAdd();
        proto.setInstanceId(_info.getInstanceId());
        
        return proto;
    }
	
	public static GS2GC_019_051_OnDinnerEnd make_051_OnDinnerEnd(long _instanceId)
    {
		GS2GC_019_051_OnDinnerEnd proto = new GS2GC_019_051_OnDinnerEnd();
        proto.setInstanceId(_instanceId);
        
        return proto;
    }
	
	public static GS2GC_019_052_OnPermitAdd make_052_OnPermitAdd(DinnerPermitInfo _permit)
    {
		GS2GC_019_052_OnPermitAdd proto = new GS2GC_019_052_OnPermitAdd();
        proto.setPermit(_permit.toProto());
        
        return proto;
    }
	
	public static GS2GC_019_053_OnJoinerAdd make_053_OnJoinerAdd(DinnerJoiner _joiner, ServerObj_DinnerJoiner _player)
    {
		GS2GC_019_053_OnJoinerAdd proto = new GS2GC_019_053_OnJoinerAdd();
        proto.setCid(_player.getCid());
        proto.setCostId(_player.getCostId());
        proto.setDinnerId(_joiner.getDinnerId());
        
        proto.setCname(_player.getCname());
        
        return proto;
    }
}
