package NPUSServer.NPUserMsgDispather.Write;

import Common.TravelObj.Travel_EventResult;
import GS2GC.p008_TravelOp.*;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelCanDealEventInfo;
import NPUSServer.NPUSUserMgr.UserComp.TravelComp.TravelConsortInfo;

import java.util.ArrayList;

/**
 * 008 协议 Travel 游历系统
 */
public class US2GCWriter_008_TravelOp
{
    public static GS2GC_008_001_RetStartTravel make_001_RetStartTravel()
    {
    	GS2GC_008_001_RetStartTravel proto = new GS2GC_008_001_RetStartTravel();
    	
        return proto;
    }
    
    public static GS2GC_008_002_RetAkeyTravel make_002_RetAkeyTravel(ArrayList<Travel_EventResult> _dealedEventResultList)
    {
    	GS2GC_008_002_RetAkeyTravel proto = new GS2GC_008_002_RetAkeyTravel();
    	for(int i = 0; i < _dealedEventResultList.size(); i++)
    	{
    		Travel_EventResult result = _dealedEventResultList.get(i);
    		if(null == result)
    			continue;
    		
    		proto.addResultList(result);
    	}
    	
        return proto;
    }

    public static GS2GC_008_003_RetDealRewardTravel make_003_RetDealRewardTravel(Travel_EventResult _result)
    {
    	GS2GC_008_003_RetDealRewardTravel proto = new GS2GC_008_003_RetDealRewardTravel();
    	proto.setResult(_result);
    	
        return proto;
    }

    public static GS2GC_008_004_RetDealConsortBarTravel make_004_RetDealConsortBarTravel(Travel_EventResult _result)
    {
    	GS2GC_008_004_RetDealConsortBarTravel proto = new GS2GC_008_004_RetDealConsortBarTravel();
    	proto.setResult(_result);
    	
        return proto;
    }

    public static GS2GC_008_005_RetDealChangeTravel make_005_RetDealChangeTravel(Travel_EventResult _result)
    {
    	GS2GC_008_005_RetDealChangeTravel proto = new GS2GC_008_005_RetDealChangeTravel();
    	proto.setResult(_result);
    	
        return proto;
    }
    
    public static GS2GC_008_006_RetDealInvitationTravel make_006_RetDealInvitationTravel(Travel_EventResult _result)
    {
    	GS2GC_008_006_RetDealInvitationTravel proto = new GS2GC_008_006_RetDealInvitationTravel();
    	proto.setResult(_result);
    	
        return proto;
    }

    public static GS2GC_008_007_RetDealAddPowerTravel make_007_RetDealAddPowerTravel(Travel_EventResult _result)
    {
    	GS2GC_008_007_RetDealAddPowerTravel proto = new GS2GC_008_007_RetDealAddPowerTravel();
    	proto.setResult(_result);
    	
        return proto;
    }

    public static GS2GC_008_008_RetDealConsortLikeTravel make_008_RetDealConsortLikeTravel(Travel_EventResult _result)
    {
    	GS2GC_008_008_RetDealConsortLikeTravel proto = new GS2GC_008_008_RetDealConsortLikeTravel();
    	proto.setResult(_result);
    	
        return proto;
    }

    public static GS2GC_008_009_RetDealConsortIntimacyTravel make_009_RetDealConsortIntimacyTravel(Travel_EventResult _result)
    {
    	GS2GC_008_009_RetDealConsortIntimacyTravel proto = new GS2GC_008_009_RetDealConsortIntimacyTravel();
    	proto.setResult(_result);
    	
        return proto;
    }

    public static GS2GC_008_010_RetDealGiftedTravel make_010_RetDealGiftedTravel(Travel_EventResult _result)
    {
    	GS2GC_008_010_RetDealGiftedTravel proto = new GS2GC_008_010_RetDealGiftedTravel();
    	proto.setResult(_result);
    	
        return proto;
    }
    
    /** 博彩事件处理结果回包 */
    public static GS2GC_008_011_RetDealGamblingTravel make_011_RetDealGamblingTravel(Travel_EventResult _result)
    {
    	GS2GC_008_011_RetDealGamblingTravel proto = new GS2GC_008_011_RetDealGamblingTravel();
    	proto.setResult(_result);

    	return proto;
    }

    public static GS2GC_008_050_OnEventAdd make_050_OnEventAdd(TravelCanDealEventInfo _info)
    {
    	GS2GC_008_050_OnEventAdd proto = new GS2GC_008_050_OnEventAdd();
    	proto.setEventInfo(_info.toProto());
    	
    	return proto;
    }
    
    public static GS2GC_008_051_OnEventDel make_051_OnEventDel(long _instanceId)
    {
    	GS2GC_008_051_OnEventDel proto = new GS2GC_008_051_OnEventDel();
    	proto.setInstanceId(_instanceId);
    	
        return proto;
    }
    
    public static GS2GC_008_052_OnConsortChg make_052_OnConsortChg(TravelConsortInfo _info)
    {
    	GS2GC_008_052_OnConsortChg proto = new GS2GC_008_052_OnConsortChg();
    	proto.setConsort(_info.toProto());
    	
        return proto;
    }
}
