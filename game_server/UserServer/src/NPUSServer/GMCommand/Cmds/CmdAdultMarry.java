package NPUSServer.GMCommand.Cmds;


import ALBasicCommon.ALSerializeMaker;
import CommonEnum.ECurrency;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Child.RefChildAttr;
import NPGameRes.Refs.Child.RefChildCareer;
import NPGameRes.Refs.Child.RefChildInitRes;
import NPGameRes.Refs.Child.RefChildQuality;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSMain;
import NPUSServer.NPUSUserMgr.GameSystem.ChildSystem.ChildSystem;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserServer;

@ACommander(comment = "子嗣联姻", name = "adultMarry")
public class CmdAdultMarry extends UsCmdBase
{
	@ACommand(comment = "子嗣联姻系统[US服务器ID]")
	public String info(int _usId)
	{
		NPUserServer usServer = NPUSMain.GetServerByTypeId(_usId);
		if(null == usServer)
			return "fail, not find us[" + _usId + "]";
		
		StringBuilder sb = new StringBuilder();
		sb.append("\ngroup:").append(usServer.getMatchAdultPool().getGroupId());
		
		return sb.toString();
	}

	@ACommand(comment = "设置子嗣联姻服务器分组IDD[US服务器ID，分组ID]")
	public String setGroup(int _usId, long _groupId)
	{
		NPUserServer usServer = NPUSMain.GetServerByTypeId(_usId);
		if(null == usServer)
			return "fail, not find us[" + _usId + "]";
		
		usServer.getMatchAdultPool().chgGroupId(_groupId);
		
		return "ok";
	}
	
	@ACommand(comment = "创建随机成年未婚子嗣[是否卷王]")
	public String testCreateRandAdult(boolean _isGiftde)
	{
		ConsortInfo consort = getOwner().getConsortComponent().lookupRnd();
		if(null == consort)
			return "fail, not find consort.";
		
		//出生配置
		RefChildInitRes initResRef = ChildSystem.chooseInitRes();
		if(null == initResRef)
			return "fail, not find rand init-res ref.";
		
		//相性配置
		RefChildAttr attrRef = ChildSystem.chooseAttr();
		if(null == attrRef)
			return "fail, not find rand attr ref.";
		
		//职业配置
		RefChildCareer careerRef = attrRef.careerWeiList.random();
		if(null == careerRef)
			return "fail, not find rand career ref.";
		
		//子嗣品质配置
		RefChildQuality qualityRef = RefChildQuality.getMgr().get(consort.getFettersInfo().getLvlRef().adopt_child_quality_id);
		if(null == qualityRef)
			return "fail, not find rand quality ref.";
		
		getOwner().getChildComponent().getAdultMgr().createAdult(consort.getConsortId(), consort.getIntimacy()
				, initResRef.id, qualityRef.id, attrRef.type.ordinal(), careerRef.career_id
				, _isGiftde
				, 5000
				, "test-"+CommonFunc.getNowTagYYYYMMDD()+ALSerializeMaker.makeNewSerialize()
				, 10000
				, 10000
				, new NPCommon_ItemInfo(ENPItemType.CURRENCY.ordinal(), ECurrency.GEM.ordinal(), 1, null)
				, getContext());

		return "ok";
	}
}
