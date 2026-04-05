package NPUSServer.NPUserMsgDispather.Write;

import Common.MarsObj.Mars_GoRoute_StageMsg;
import GS2GC.p038_MarsOp.*;
import NPUSServer.Common.Context.NPPlayerContext;

import java.util.List;

/**
 * 38 - 前往火星系统
 * @author mj
 *
 */
public class US2GCWriter_038_MarsOp
{
    public static GS2GC_038_001_RetStartToGoMars make_001_RetStartToGoMars()
    {
    	GS2GC_038_001_RetStartToGoMars proto = new GS2GC_038_001_RetStartToGoMars();
    	
        return proto;
    }
    
    public static GS2GC_038_002_RetArriveMarsStage make_002_RetArriveMarsStage(NPPlayerContext _context)
    {
    	GS2GC_038_002_RetArriveMarsStage proto = new GS2GC_038_002_RetArriveMarsStage();
    	_context.getCollector().fillProtoList(proto.getItemList());
    	
        return proto;
    }
    
    public static GS2GC_038_003_RetSendDoneMarquee make_003_RetSendDoneMarquee()
    {
    	GS2GC_038_003_RetSendDoneMarquee proto = new GS2GC_038_003_RetSendDoneMarquee();
    	
        return proto;
    }

    public static GS2GC_038_050_OnGoToStageArrived make_050_OnGoToStageArrived(int _stage, long _startMs, long _stageStartMs)
    {
    	GS2GC_038_050_OnGoToStageArrived proto = new GS2GC_038_050_OnGoToStageArrived();
    	proto.setStage(_stage);
    	proto.setStartMs(_startMs);
    	proto.setStageStartMs(_stageStartMs);

        return proto;
    }
    
    public static GS2GC_038_051_OnGoToAllStageDone make_051_OnGoToAllStageDone()
    {
    	GS2GC_038_051_OnGoToAllStageDone proto = new GS2GC_038_051_OnGoToAllStageDone();

        return proto;
    }

    public static GS2GC_038_004_RetSendStageMsg make_004_RetSendStageMsg()
    {
        return new GS2GC_038_004_RetSendStageMsg();
    }

    public static GS2GC_038_005_RetGetStageMsgList make_005_RetGetStageMsgList(int _stage, List<Mars_GoRoute_StageMsg> _msgList)
    {
        GS2GC_038_005_RetGetStageMsgList proto = new GS2GC_038_005_RetGetStageMsgList();
        proto.setStage(_stage);
        proto.getMsgList().addAll(_msgList);
        return proto;
    }
}