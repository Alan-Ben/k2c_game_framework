package NPUSServer.NPUserMsgDispather.Write;

import Common.ConsortEnum.EConsortSourceType;
import Common.ConsortObj.Consort_CallRes;
import GS2GC.p015_ConsortOp.*;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBless.ConsortBlessSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBusiness.ConsortBusinessSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortCGMgr.ConsortCGInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortFetters.ConsortFettersInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortHalo.ConsortHaloInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortSkin.ConsortSkinInfo;

import java.util.ArrayList;

/**
 * @description: 015 妃子协议 writer
 * @author: mark
 * @date: 2022-04-08 11:58:05
 */
public class US2GCWriter_015_ConsortOp
{
	public static GS2GC_015_001_RetUpgradeFettersLvl make_001_RetUpgradeFettersLvl()
    {
		GS2GC_015_001_RetUpgradeFettersLvl proto = new GS2GC_015_001_RetUpgradeFettersLvl();
    	
    	return proto;
    }
	
	public static GS2GC_015_002_RetUnderstandBusinessSkill make_002_RetUnderstandBusinessSkill(boolean _isProUp)
    {
		GS2GC_015_002_RetUnderstandBusinessSkill proto = new GS2GC_015_002_RetUnderstandBusinessSkill();
		proto.setIsProUp(_isProUp);
    	
    	return proto;
    }
	
	public static GS2GC_015_003_RetUpgradeBlessSkill make_003_RetUpgradeBlessSkill()
    {
		GS2GC_015_003_RetUpgradeBlessSkill proto = new GS2GC_015_003_RetUpgradeBlessSkill();
    	
    	return proto;
    }
	
	public static GS2GC_015_004_RetCallRand make_004_RetCallRand(Consort_CallRes _callRes)
    {
		GS2GC_015_004_RetCallRand proto = new GS2GC_015_004_RetCallRand();
		proto.setRes(_callRes);
    	
    	return proto;
    }

	public static GS2GC_015_005_RetCallAkey make_005_RetCallAkey(ArrayList<Consort_CallRes> _callResList)
    {
		GS2GC_015_005_RetCallAkey proto = new GS2GC_015_005_RetCallAkey();
    	for(int i = 0; i < _callResList.size(); i++)
    	{
    		Consort_CallRes callRes = _callResList.get(i);
    		if(null == callRes)
    			continue;
    		
    		proto.addResList(callRes);
    	}
		
    	return proto;
    }
	
	public static GS2GC_015_006_RetCallAppoint make_006_RetCallAppoint(long _addCharmPoint, long _childId)
    {
		GS2GC_015_006_RetCallAppoint proto = new GS2GC_015_006_RetCallAppoint();
    	proto.setAddCharmPoint(_addCharmPoint);
        proto.setChildId(_childId);
		
    	return proto;
    }
	
	public static GS2GC_015_007_RetSetCurSkin make_007_RetSetCurSkin()
    {
		GS2GC_015_007_RetSetCurSkin proto = new GS2GC_015_007_RetSetCurSkin();
    	
    	return proto;
    }

	public static GS2GC_015_008_RetGetRoundTravelCount make_008_RetGetRoundTravelCount(long _counter)
    {
		GS2GC_015_008_RetGetRoundTravelCount proto = new GS2GC_015_008_RetGetRoundTravelCount();
		proto.setCounter(_counter);
    	
    	return proto;
    }
	
	public static GS2GC_015_009_RetGetAllBusinessSkill make_GS2GC_009_RetGetAllBusinessSkill(ConsortInfo _consort)
    {
		GS2GC_015_009_RetGetAllBusinessSkill proto = new GS2GC_015_009_RetGetAllBusinessSkill();
		_consort.getBusinessSkillMgr().makeProto(proto.getDataList());
    	
    	return proto;
    }
	
	public static GS2GC_015_010_RetUpgradeHaloLvl make_010_RetUpgradeHaloLvl()
    {
		GS2GC_015_010_RetUpgradeHaloLvl proto = new GS2GC_015_010_RetUpgradeHaloLvl();
    	
    	return proto;
    }
	
	public static GS2GC_015_011_RetGetCgUnlockReward make_011_RetGetCgUnlockReward()
    {
		GS2GC_015_011_RetGetCgUnlockReward proto = new GS2GC_015_011_RetGetCgUnlockReward();
    	
    	return proto;
    }
	
	public static GS2GC_015_012_RetUnlockSkin make_012_RetUnlockSkin()
    {
		GS2GC_015_012_RetUnlockSkin proto = new GS2GC_015_012_RetUnlockSkin();
    	
    	return proto;
    }
	
	public static GS2GC_015_013_RetUnlockHalo make_013_RetUnlockHalo()
    {
		GS2GC_015_013_RetUnlockHalo proto = new GS2GC_015_013_RetUnlockHalo();
    	
    	return proto;
    }

	public static GS2GC_015_020_RetConsortChooseDialogueOption make_020_RetConsortChooseDialogueOption()
    {
    	return new GS2GC_015_020_RetConsortChooseDialogueOption();
    }

	public static GS2GC_015_021_RetConsortDrawDialogueReward make_021_RetConsortDrawDialogueReward()
    {
    	return new GS2GC_015_021_RetConsortDrawDialogueReward();
    }

	public static GS2GC_015_022_RetConsortAiChat make_022_RetConsortAiChat()
    {
    	return new GS2GC_015_022_RetConsortAiChat();
    }

	public static GS2GC_015_025_RetConsortInitiateAiChat make_025_RetConsortInitiateAiChat()
    {
    	return new GS2GC_015_025_RetConsortInitiateAiChat();
    }

	public static GS2GC_015_026_RetConsortEvaluateReply make_026_RetConsortEvaluateReply()
	{
		return new GS2GC_015_026_RetConsortEvaluateReply();
	}

	public static GS2GC_015_073_OnConsortAiChatMsgAdd make_073_OnConsortAiChatMsgAdd(long _consortId, long _clientDataId, int _errCode, String _msg)
	{
		GS2GC_015_073_OnConsortAiChatMsgAdd proto = new GS2GC_015_073_OnConsortAiChatMsgAdd();
		proto.setConsortId(_consortId);
		proto.setClientDataId(_clientDataId);
		proto.setErrCode(_errCode);
		proto.setMsg(_msg);
		return proto;
	}

	public static GS2GC_015_023_RetConsortAddChatFriend make_023_RetConsortAddChatFriend()
    {
    	return new GS2GC_015_023_RetConsortAddChatFriend();
    }

	public static GS2GC_015_024_RetConsortAiChatMoment make_024_RetConsortAiChatMoment()
    {
    	return new GS2GC_015_024_RetConsortAiChatMoment();
    }

	public static GS2GC_015_076_OnConsortAiChatMomentMsgAdd make_076_OnConsortAiChatMomentMsgAdd(long _instanceId, long _consortId, int _errCode, String _msg)
	{
		GS2GC_015_076_OnConsortAiChatMomentMsgAdd proto = new GS2GC_015_076_OnConsortAiChatMomentMsgAdd();
		proto.setInstanceId(_instanceId);
		proto.setConsortId(_consortId);
		proto.setErrCode(_errCode);
		proto.setMsg(_msg);
		return proto;
	}

	public static GS2GC_015_050_OnConsortAdd make_050_OnConsortAdd(ConsortInfo _consort, EConsortSourceType _sourceType)
    {
		GS2GC_015_050_OnConsortAdd proto = new GS2GC_015_050_OnConsortAdd();
    	proto.setConsort(_consort.toProto());
    	proto.setSourceType(_sourceType);
		
    	return proto;
    }

	public static GS2GC_015_051_OnConsortIntimacyChg make_051_OnConsortIntimacyChg(ConsortInfo _consort)
    {
		GS2GC_015_051_OnConsortIntimacyChg proto = new GS2GC_015_051_OnConsortIntimacyChg();
		proto.setConsortId(_consort.getConsortId());
		proto.setIntimacy(_consort.getIntimacy());
    	
    	return proto;
    }

	public static GS2GC_015_052_OnConsortCharmChg make_052_OnConsortCharmChg(ConsortInfo _consort)
    {
		GS2GC_015_052_OnConsortCharmChg proto = new GS2GC_015_052_OnConsortCharmChg();
		proto.setConsortId(_consort.getConsortId());
		proto.setCharm(_consort.getCharm());
    	
    	return proto;
    }

	public static GS2GC_015_053_OnConsortCharmPointChg make_053_OnConsortCharmPointChg(ConsortInfo _consort)
    {
		GS2GC_015_053_OnConsortCharmPointChg proto = new GS2GC_015_053_OnConsortCharmPointChg();
		proto.setConsortId(_consort.getConsortId());
		proto.setCharmPoint(_consort.getCharmPoint());
    	
    	return proto;
    }
	
	public static GS2GC_015_054_OnTriggeredCallDlgIdAdd make_054_OnTriggeredCallDlgIdAdd(ConsortInfo _consort, long _storyId)
    {
		GS2GC_015_054_OnTriggeredCallDlgIdAdd proto = new GS2GC_015_054_OnTriggeredCallDlgIdAdd();
		proto.setConsortId(_consort.getConsortId());
		proto.setTriggeredCallStroryId(_storyId);
    	
    	return proto;
    }

	public static GS2GC_015_055_OnSkinAdd make_055_OnSkinAdd(ConsortSkinInfo _skin)
    {
		GS2GC_015_055_OnSkinAdd proto = new GS2GC_015_055_OnSkinAdd();
		proto.setConsortId(_skin.getConsortId());
		proto.setSkin(_skin.toProto());
    	
    	return proto;
    }
	
	public static GS2GC_015_056_OnCurSkinChg make_056_OnCurSkinChg(ConsortInfo _consort)
    {
		GS2GC_015_056_OnCurSkinChg proto = new GS2GC_015_056_OnCurSkinChg();
		proto.setConsortId(_consort.getConsortId());
		proto.setCurSkinId(_consort.getCurSkinId());
    	
    	return proto;
    }
	
	public static GS2GC_015_057_OnFettersChg make_057_OnFettersChg(ConsortFettersInfo _fetters)
    {
		GS2GC_015_057_OnFettersChg proto = new GS2GC_015_057_OnFettersChg();
    	proto.setConsortId(_fetters.getConsortId());
    	proto.setFetters(_fetters.toProto());
		
    	return proto;
    }
	
	public static GS2GC_015_058_OnBusinessSkillChg make_058_OnBusinessSkillChg(ConsortBusinessSkillInfo _bussinessSkill)
    {
		GS2GC_015_058_OnBusinessSkillChg proto = new GS2GC_015_058_OnBusinessSkillChg();
    	proto.setConsortId(_bussinessSkill.getConsortId());
    	proto.setSkill(_bussinessSkill.toProto());
		
    	return proto;
    }
	
	public static GS2GC_015_059_OnBlessSkillChg make_059_OnBlessSkillChg(ConsortBlessSkillInfo _blessSkill)
    {
		GS2GC_015_059_OnBlessSkillChg proto = new GS2GC_015_059_OnBlessSkillChg();
    	proto.setConsortId(_blessSkill.getConsortId());
    	proto.setSkill(_blessSkill.toProto());
		
    	return proto;
    }
	
	public static GS2GC_015_060_OnHaloChg make_060_OnHaloChg(ConsortHaloInfo _halo)
    {
		GS2GC_015_060_OnHaloChg proto = new GS2GC_015_060_OnHaloChg();
    	proto.setConsortId(_halo.getConsortId());
		proto.setHalo(_halo.toProto());
		
    	return proto;
    }
	
	public static GS2GC_015_061_OnCgChg make_061_OnCgChg(ConsortCGInfo _cg)
    {
		GS2GC_015_061_OnCgChg proto = new GS2GC_015_061_OnCgChg();
    	proto.setCg(_cg.toProto());
		
    	return proto;
    }
	
	public static GS2GC_015_062_OnRandCallConsortChg make_062_OnRandCallConsortChg(NPUSUserData _userData)
    {
		GS2GC_015_062_OnRandCallConsortChg proto = new GS2GC_015_062_OnRandCallConsortChg();
    	_userData.getConsortComponent().makeRandCallConsortProto(proto.getConsortIdList());
		
    	return proto;
    }
}
