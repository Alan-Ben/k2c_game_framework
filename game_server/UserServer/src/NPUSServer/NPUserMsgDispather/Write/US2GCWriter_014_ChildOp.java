package NPUSServer.NPUserMsgDispather.Write;

import Common.ChildObj.Adult_PoolBaseInfo;
import Common.ChildObj.Adult_PoolInfo;
import GS2GC.p014_ChildOp.*;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.MarriedAdultInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Adult.UnmarryAdultInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildSeatInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.ToMeMarryApplyMgr.ToMeMarryApplyInfo;

import java.util.ArrayList;

/**
 * @description: 014 子嗣协议 writer
 * @author: mark
 * @date: 2022-04-08 11:58:05
 */
public class US2GCWriter_014_ChildOp
{
	public static GS2GC_014_001_RetSetChildName make_001_RetSetChildName()
	{
		GS2GC_014_001_RetSetChildName proto = new GS2GC_014_001_RetSetChildName();
		
		return proto;
	}
	
	public static GS2GC_014_002_RetTrainChild make_002_RetTrainChild(long _costValue, long _gainValue, long _addBonus)
	{
		GS2GC_014_002_RetTrainChild proto = new GS2GC_014_002_RetTrainChild();
		proto.setCostValue(_costValue);
		proto.setGainValue(_gainValue);
		proto.setAddBonus(_addBonus);
		
		return proto;
	}
	
	public static GS2GC_014_003_RetAddSeatEnergy make_003_RetAddSeatEnergy()
	{
		GS2GC_014_003_RetAddSeatEnergy proto = new GS2GC_014_003_RetAddSeatEnergy();
		
		return proto;
	}
	
	public static GS2GC_014_004_RetSetChildGraduate make_004_RetSetChildGraduate(NPPlayerContext _context, UnmarryAdultInfo _adult)
	{
		GS2GC_014_004_RetSetChildGraduate proto = new GS2GC_014_004_RetSetChildGraduate();
		_context.getCollector().fillProtoList(proto.getGainItemList());
		proto.setAdult(_adult.toUnmarriedProto());
		
		return proto;
	}
	
	public static GS2GC_014_006_RetGetUnMarriedAdult make_006_RetGetUnMarriedAdult(UnmarryAdultInfo _adult)
	{
		GS2GC_014_006_RetGetUnMarriedAdult proto = new GS2GC_014_006_RetGetUnMarriedAdult();
		proto.setAdult(_adult.toUnmarriedProto());
		
		return proto;
	}
	
	public static GS2GC_014_007_RetGetMarriedAdult make_007_RetGetMarriedAdult(MarriedAdultInfo _adult)
	{
		GS2GC_014_007_RetGetMarriedAdult proto = new GS2GC_014_007_RetGetMarriedAdult();
		proto.setAdult(_adult.toMarriedProto());
		
		return proto;
	}
	
	public static GS2GC_014_008_RetGetToMeApply make_008_RetGetToMeApply(ToMeMarryApplyInfo _toMeApply)
	{
		GS2GC_014_008_RetGetToMeApply proto = new GS2GC_014_008_RetGetToMeApply();
		proto.setApply(_toMeApply.toProto());
		
		return proto;
	}
	
	public static GS2GC_014_009_RetRefuseToMeApply make_009_RetRefuseToMeApply()
	{
		GS2GC_014_009_RetRefuseToMeApply proto = new GS2GC_014_009_RetRefuseToMeApply();
		
		return proto;
	}
	
	public static GS2GC_014_010_RetAkeyRefuseToMeApply make_010_RetAkeyRefuseToMeApply()
	{
		GS2GC_014_010_RetAkeyRefuseToMeApply proto = new GS2GC_014_010_RetAkeyRefuseToMeApply();
		
		return proto;
	}
	
	public static GS2GC_014_011_RetAgreeToMeApply make_011_RetAgreeToMeApply()
	{
		GS2GC_014_011_RetAgreeToMeApply proto = new GS2GC_014_011_RetAgreeToMeApply();
		
		return proto;
	}
	
	public static GS2GC_014_012_RetGetRecommendPlayerList make_012_RetGetRecommendPlayerList(ArrayList<Adult_PoolBaseInfo> _matchAdultList)
	{
		GS2GC_014_012_RetGetRecommendPlayerList proto = new GS2GC_014_012_RetGetRecommendPlayerList();
		for(int i = 0; i < _matchAdultList.size(); i++)
		{
			proto.addMatchList(_matchAdultList.get(i));
		}
		
		return proto;
	}
	
	public static GS2GC_014_013_RetApplyToPlayer make_013_RetApplyToPlayer(boolean _isPlayerRefuseAllRequest)
	{
		return new GS2GC_014_013_RetApplyToPlayer(_isPlayerRefuseAllRequest);
	}
	
	public static GS2GC_014_014_RetApplyToGroup make_014_RetApplyToGroup()
	{
		GS2GC_014_014_RetApplyToGroup proto = new GS2GC_014_014_RetApplyToGroup();
		
		return proto;
	}
	
	public static GS2GC_014_015_RetAgreeApplyGroup make_015_RetAgreeApplyGroup()
	{
		GS2GC_014_015_RetAgreeApplyGroup proto = new GS2GC_014_015_RetAgreeApplyGroup();
		
		return proto;
	}
	
	public static GS2GC_014_016_RetCancelApplyToPlayer make_016_RetCancelApplyToPlayer()
	{
		GS2GC_014_016_RetCancelApplyToPlayer proto = new GS2GC_014_016_RetCancelApplyToPlayer();
		
		return proto;
	}
	
	public static GS2GC_014_017_RetCancelApplyToGroup make_017_RetCancelApplyToGroup()
	{
		GS2GC_014_017_RetCancelApplyToGroup proto = new GS2GC_014_017_RetCancelApplyToGroup();
		
		return proto;
	}
	
	public static GS2GC_014_018_RetGetPoolAdult make_018_RetGetPoolAdult(Adult_PoolInfo _poolAdult)
	{
		GS2GC_014_018_RetGetPoolAdult proto = new GS2GC_014_018_RetGetPoolAdult();
		proto.setPoolAdult(_poolAdult);
		
		return proto;
	}

	public static GS2GC_014_019_RetRandomGainChild make_019_RetRandomGainChild()
	{
		return new GS2GC_014_019_RetRandomGainChild();
	}

	public static GS2GC_014_030_RetCidAdultIsMarried make_030_RetCidAdultIsMarried(boolean _isGiftde, boolean _isMarried)
	{
		return new GS2GC_014_030_RetCidAdultIsMarried(_isGiftde, _isMarried);
	}
	
	public static GS2GC_014_050_OnChildAdd make_050_OnChildAdd(ChildInfo _child)
	{
		GS2GC_014_050_OnChildAdd proto = new GS2GC_014_050_OnChildAdd();
		proto.setChild(_child.toProto());
		
		return proto;
	}
	
	public static GS2GC_014_051_OnChildNameChg make_051_OnChildNameChg(ChildInfo _child)
	{
		GS2GC_014_051_OnChildNameChg proto = new GS2GC_014_051_OnChildNameChg();
		proto.setId(_child.getChildId());
		proto.setName(_child.getName());
		
		return proto;
	}
	
	public static GS2GC_014_052_OnChildLvlChg make_052_OnChildLvlChg(ChildInfo _child)
	{
		GS2GC_014_052_OnChildLvlChg proto = new GS2GC_014_052_OnChildLvlChg();
		proto.setId(_child.getChildId());
		proto.setLvl(_child.getLvl());
		
		return proto;
	}
	
	public static GS2GC_014_053_OnSeatChg make_053_OnSeatChg(ChildSeatInfo _seat)
	{
		GS2GC_014_053_OnSeatChg proto = new GS2GC_014_053_OnSeatChg();
		proto.setSeat(_seat.toProto());
		
		return proto;
	}

	public static GS2GC_014_054_OnUnmarriedAdultAdd make_054_OnUnmarriedAdultAdd(UnmarryAdultInfo _adult)
	{
		GS2GC_014_054_OnUnmarriedAdultAdd proto = new GS2GC_014_054_OnUnmarriedAdultAdd();
		proto.setAdult(_adult.toUnmarriedProto());
		
		return proto;
	}
	
	public static GS2GC_014_055_OnMarriedAdultAdd make_055_OnMarriedAdultAdd(MarriedAdultInfo _adult)
	{
		GS2GC_014_055_OnMarriedAdultAdd proto = new GS2GC_014_055_OnMarriedAdultAdd();
		proto.setAdult(_adult.toMarriedProto());
		
		return proto;
	}
	
	public static GS2GC_014_056_OnToMeApplyAdd make_056_OnToMeApplyAdd(ToMeMarryApplyInfo _toMeApply)
	{
		GS2GC_014_056_OnToMeApplyAdd proto = new GS2GC_014_056_OnToMeApplyAdd();
		proto.setToMeApply(_toMeApply.toBaseProto());
		
		return proto;
	}

	public static GS2GC_014_057_OnChildRemove make_057_OnChildRemove(long _id)
	{
		GS2GC_014_057_OnChildRemove proto = new GS2GC_014_057_OnChildRemove();
		proto.setId(_id);
		
		return proto;
	}

	public static GS2GC_014_058_OnToMeApplyDel make_058_OnToMeApplyDel(long _applyAdultId)
	{
		GS2GC_014_058_OnToMeApplyDel proto = new GS2GC_014_058_OnToMeApplyDel();
		proto.setApplyAdultId(_applyAdultId);
		
		return proto;
	}
	
	public static GS2GC_014_059_OnUnmarriedAdultStatusChg make_059_OnUnmarriedAdultStatusChg(UnmarryAdultInfo _adult)
	{
		GS2GC_014_059_OnUnmarriedAdultStatusChg proto = new GS2GC_014_059_OnUnmarriedAdultStatusChg();
		proto.setId(_adult.getAdultId());
		proto.setStatus(_adult.getStatus());
		proto.setExpiredTs(_adult.getApplyExpiredTs());
        proto.setMinAllowBonus(_adult.getApplyMinBonus());
		
		return proto;
	}

	public static GS2GC_014_060_OnToMeApplyClear make_060_OnToMeApplyClear()
	{
		GS2GC_014_060_OnToMeApplyClear proto = new GS2GC_014_060_OnToMeApplyClear();
		
		return proto;
	}

	public static GS2GC_014_061_OnChildBonusSumChg make_061_OnChildBonusSumChg(long _bonus)
	{
		GS2GC_014_061_OnChildBonusSumChg proto = new GS2GC_014_061_OnChildBonusSumChg();
		proto.setBonus(_bonus);
		
		return proto;
	}

	public static GS2GC_014_062_OnChildBonusChg make_062_OnChildBonusChg(ChildInfo _child)
	{
		GS2GC_014_062_OnChildBonusChg proto = new GS2GC_014_062_OnChildBonusChg();
		proto.setId(_child.getChildId());
		proto.setBonus(_child.getTrainBonus());
		
		return proto;
	}

	public static GS2GC_014_063_OnAdultBonusSumChg make_063_OnAdultBonusSumChg(long _bonus)
	{
		GS2GC_014_063_OnAdultBonusSumChg proto = new GS2GC_014_063_OnAdultBonusSumChg();
		proto.setBonus(_bonus);

		return proto;
	}

	/**
	 * 创建设置拒绝联姻响应协议
	 *
	 * @return 设置拒绝联姻响应协议对象
	 */
	public static GS2GC_014_020_RetSetRefuseMarry make_020_RetSetRefuseMarry()
	{
		GS2GC_014_020_RetSetRefuseMarry proto = new GS2GC_014_020_RetSetRefuseMarry();
		return proto;
	}
}
