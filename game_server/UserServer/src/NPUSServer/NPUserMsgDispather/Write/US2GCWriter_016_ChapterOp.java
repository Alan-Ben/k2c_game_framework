package NPUSServer.NPUserMsgDispather.Write;

import Common.ChapterObj.Chapter_EventInfo;
import Common.ChapterObj.Chapter_InspireInfo;
import Common.ChapterObj.Chapter_PosInfo;
import GS2GC.p016_ChapterOp.*;
import NPCommon.NPCommon_ItemInfo;
import NPUSServer.Common.Context.NPPlayerContext;

import java.util.List;

public class US2GCWriter_016_ChapterOp
{
	public static GS2GC_016_001_RetChapterForward make_001_RetChapterForward(
			long _chapterId, int _point, int _coefficient, long _heroExp, long _playerExp, long _costGoldNum, long _eventId, NPPlayerContext _dealEventContext)
	{
		GS2GC_016_001_RetChapterForward proto = new GS2GC_016_001_RetChapterForward();
		proto.setChapterId(_chapterId);
		proto.setPoint(_point);
		proto.setCoefficient(_coefficient);
		proto.setRewardExp(_heroExp);
		proto.setRewardPlayerExp(_playerExp);
		proto.setCostGoldNum(_costGoldNum);
		proto.setEventId(_eventId);
		if (_dealEventContext != null)
			_dealEventContext.getCollector().fillProtoList(proto.getItemList());
		return proto;
	}

	public static GS2GC_016_002_RetChapterFightBoss make_002_RetChapterFightBoss(long _chapterId, List<NPCommon_ItemInfo> _data)
    {
		GS2GC_016_002_RetChapterFightBoss proto = new GS2GC_016_002_RetChapterFightBoss();
		proto.setChapterId(_chapterId);
		proto.getRewardList().addAll(_data);
    	return proto;
    }

	public static GS2GC_016_003_RetChapterFightBossInspire make_003_RetChapterFightBossInspire()
    {
    	return new GS2GC_016_003_RetChapterFightBossInspire();
    }

	public static GS2GC_016_004_RetDealChapterRewardEvent make_004_RetDealChapterRewardEvent(NPPlayerContext _context)
    {
		GS2GC_016_004_RetDealChapterRewardEvent proto = new GS2GC_016_004_RetDealChapterRewardEvent();
		_context.getCollector().fillProtoList(proto.getItemList());
		return proto;
    }

	public static GS2GC_016_005_RetDealChapterChoiceEvent make_005_RetDealChapterChoiceEvent(NPPlayerContext _context)
    {
		GS2GC_016_005_RetDealChapterChoiceEvent proto = new GS2GC_016_005_RetDealChapterChoiceEvent();
		_context.getCollector().fillProtoList(proto.getItemList());
		return proto;
    }

	public static GS2GC_016_006_RetDealChapterDispatchEvent make_006_RetDealChapterDispatchEvent(int _reachNum, NPPlayerContext _context)
	{
		GS2GC_016_006_RetDealChapterDispatchEvent proto = new GS2GC_016_006_RetDealChapterDispatchEvent();
		proto.setReachNum(_reachNum);
		_context.getCollector().fillProtoList(proto.getItemList());
		return proto;
	}

	public static GS2GC_016_007_RetDrawChapterPlotReward make_007_RetDrawChapterPlotReward()
	{
		return new GS2GC_016_007_RetDrawChapterPlotReward();
	}

	public static GS2GC_016_051_OnChapterPosChg make_051_OnChapterPosChg(Chapter_PosInfo _posInfo)
    {
    	return new GS2GC_016_051_OnChapterPosChg(_posInfo);
    }

	public static GS2GC_016_052_OnChapterInspireChg make_052_OnChapterInspireChg(Chapter_InspireInfo _inspireInfo)
    {
    	return new GS2GC_016_052_OnChapterInspireChg(_inspireInfo);
    }

	public static GS2GC_016_053_OnChapterEventChg make_053_OnChapterEventChg(Chapter_EventInfo _eventInfo)
    {
    	return new GS2GC_016_053_OnChapterEventChg(_eventInfo);
    }

	public static GS2GC_016_054_OnChapterGmPosChg make_054_OnChapterGmPosChg(Chapter_PosInfo _posInfo)
	{
		return new GS2GC_016_054_OnChapterGmPosChg(_posInfo);
	}

	/**
	 * 构造剧情奖励领取推送协议
	 *
	 * @param _plotIdList 剧情ID列表
	 * @return 推送协议对象
	 */
	public static GS2GC_016_055_OnChapterPlotRewardDraw make_055_OnChapterPlotRewardDraw(List<Long> _plotIdList)
	{
		GS2GC_016_055_OnChapterPlotRewardDraw proto = new GS2GC_016_055_OnChapterPlotRewardDraw();
		proto.getPlotIdList().addAll(_plotIdList);
		return proto;
	}
}
