package NPUSServer.NPUserMsgDispather.Write;

import GS2GC.p040_MarsPeopleOp.*;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.MarsStep3_PeopleComp.*;

/**
 * 40 - 火星居民系统
 * @author mj
 *
 */
public class US2GCWriter_040_MarsPeopleOp
{
    public static GS2GC_040_001_RetPeopleImmigrant make_001_RetPeopleImmigrant()
    {
    	GS2GC_040_001_RetPeopleImmigrant proto = new GS2GC_040_001_RetPeopleImmigrant();
    	
        return proto;
    }

    public static GS2GC_040_002_RetPeopleConfirmImmigrant make_002_RetPeopleConfirmImmigrant(long _num, NPPlayerContext _context)
    {
    	GS2GC_040_002_RetPeopleConfirmImmigrant proto = new GS2GC_040_002_RetPeopleConfirmImmigrant();
    	proto.setNum(_num);
    	_context.getCollector().fillProtoList(proto.getItemList());
    	
        return proto;
    }

    public static GS2GC_040_003_RetDealIntelligent make_003_RetDealIntelligent()
    {
    	GS2GC_040_003_RetDealIntelligent proto = new GS2GC_040_003_RetDealIntelligent();
    	
        return proto;
    }

    public static GS2GC_040_004_RetDealRewardHelp make_004_RetDealRewardHelp(NPPlayerContext _context)
    {
    	GS2GC_040_004_RetDealRewardHelp proto = new GS2GC_040_004_RetDealRewardHelp();
    	_context.getCollector().fillProtoList(proto.getItemList());
    	
        return proto;
    }

    public static GS2GC_040_005_RetDealChoiceHelp make_005_RetDealChoiceHelp(NPPlayerContext _context)
    {
    	GS2GC_040_005_RetDealChoiceHelp proto = new GS2GC_040_005_RetDealChoiceHelp();
    	_context.getCollector().fillProtoList(proto.getItemList());
    	
        return proto;
    }

    public static GS2GC_040_050_OnMarsPeopleNumChg make_050_OnMarsPeopleNumChg(MarsPeopleNumInfo _info)
    {
    	GS2GC_040_050_OnMarsPeopleNumChg proto = new GS2GC_040_050_OnMarsPeopleNumChg();
    	proto.setPeopleNum(_info.toProto());
    	
        return proto;
    }

    public static GS2GC_040_051_OnIntelligentChg make_051_OnIntelligentChg(MarsPeopleIntelligentInfo _info)
    {
    	GS2GC_040_051_OnIntelligentChg proto = new GS2GC_040_051_OnIntelligentChg();
    	proto.setIntelligent(_info.toProto());
    	
        return proto;
    }

    public static GS2GC_040_052_OnSatisfactionChg make_052_OnSatisfactionChg(int _num)
    {
    	GS2GC_040_052_OnSatisfactionChg proto = new GS2GC_040_052_OnSatisfactionChg();
    	proto.setSatisfaction(_num);
    	
        return proto;
    }

    public static GS2GC_040_053_OnLetterChg make_053_OnLetterChg(MarsPeopleLetterInfo _info)
    {
    	GS2GC_040_053_OnLetterChg proto = new GS2GC_040_053_OnLetterChg();
    	proto.setLetter(_info.toProto());
    	
        return proto;
    }
    
    public static GS2GC_040_054_OnLetterDel make_054_OnLetterDel(long _id)
    {
    	GS2GC_040_054_OnLetterDel proto = new GS2GC_040_054_OnLetterDel();
    	proto.setId(_id);
    	
        return proto;
    }
    
    public static GS2GC_040_055_OnHelpChg make_055_OnHelpChg(MarsPeopleHelpInfo _info)
    {
    	GS2GC_040_055_OnHelpChg proto = new GS2GC_040_055_OnHelpChg();
    	proto.setHelp(_info.toProto());
    	
        return proto;
    }
    
    public static GS2GC_040_056_OnHelpDel make_056_OnHelpDel(long _id)
    {
    	GS2GC_040_056_OnHelpDel proto = new GS2GC_040_056_OnHelpDel();
    	proto.setId(_id);
    	
        return proto;
    }
    
    public static GS2GC_040_057_OnMarsEventTrigger make_057_OnMarsEventTrigger(long _eventId)
    {
    	GS2GC_040_057_OnMarsEventTrigger proto = new GS2GC_040_057_OnMarsEventTrigger();
    	proto.getMarsEvent().setEventId(_eventId);
    	
        return proto;
    }
    
    public static GS2GC_040_058_OnPeopleImmigrantAdd make_058_OnPeopleImmigrantAdd(MarsPeopleImmigrantInfo _info)
    {
    	GS2GC_040_058_OnPeopleImmigrantAdd proto = new GS2GC_040_058_OnPeopleImmigrantAdd();
    	proto.setInfo(_info.toImmigrantProto());
    	
        return proto;
    }
    
    public static GS2GC_040_059_OnPeopleImmigrantDel make_059_OnPeopleImmigrantDel(MarsPeopleImmigrantInfo _info)
    {
    	GS2GC_040_059_OnPeopleImmigrantDel proto = new GS2GC_040_059_OnPeopleImmigrantDel();
    	
        return proto;
    }
    
    public static GS2GC_040_060_OnPeopleImmigrantCountChg make_060_OnPeopleImmigrantCountChg(MarsPeopleImmigrantInfo _info)
    {
    	GS2GC_040_060_OnPeopleImmigrantCountChg proto = new GS2GC_040_060_OnPeopleImmigrantCountChg();
    	proto.setInfo(_info.toImmigrantCountProto());
    	
        return proto;
    }
}