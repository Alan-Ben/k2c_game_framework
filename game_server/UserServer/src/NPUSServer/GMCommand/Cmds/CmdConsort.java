package NPUSServer.GMCommand.Cmds;


import Common.Common_AiChatMessage;
import Common.ConsortObj.Consort_BusinessSkillPropertySum;
import CommonEnum.EAiChatRoleType;
import CommonEnum.ESpecAttrType;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Game.EChildBirthRes;
import NPCommon.Log.CommLog;
import NPCommon.Util.CallBack._ICallBackIntT;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Consort.*;
import NPGameRes.Refs.ConsortChat.RefConsortChatAi;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBless.ConsortBlessSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBusiness.ConsortBusinessSkillInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

@ACommander(comment = "家人系统", name = "consort")
public class CmdConsort extends UsCmdBase
{
	@ACommand(comment = "获得家人[家人配置ID]")
	public String gain(long _consortId)
	{
		getOwner().gainItem(ENPItemType.CONSORT, _consortId, getContext());

		getOwner().sendMsgToGC(getContext().getCollector().toProto());

		return "ok";
	}

	@ACommand(comment = "增加家人亲密度[家人配置ID, 增加值]")
	public String gainIntimacy(long _consortId, long _addValue)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		info.incrIntimacy(_addValue, getContext());

		return "ok";
	}

	@ACommand(comment = "增加家人加护力[家人配置ID, 增加值]")
	public String gainCharm(long _consortId, long _addValue)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		info.incrCharm(_addValue, getContext());

		return "ok";
	}

	@ACommand(comment = "增加家人加护点[家人配置ID, 增加值]")
	public String gainCharmPoint(long _consortId, long _addValue)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		info.incrCharmPoint(_addValue, getContext());

		return "ok";
	}

	@ACommand(comment = "设置家人亲密度[家人配置ID, 目标值]")
	public String setIntimacy(long _consortId, long _value)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		info.updateIntimacy(_value, getContext());

		return "ok";
	}

	@ACommand(comment = "设置家人加护力[家人配置ID, 目标值]")
	public String setCharm(long _consortId, long _value)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		info.updateCharm(_value, getContext());

		return "ok";
	}

	@ACommand(comment = "设置家人吸引力[家人配置ID, 目标值]")
	public String setCharmPoint(long _consortId, long _value)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		info.updateCharmPoint(_value, getContext());

		return "ok";
	}

	@ACommand(comment = "设置家人经营技能数值[家人配置ID, 经营技能ID, 数值]")
	public String setBusinessProAdd(long _consortId, long _skillId, int _value)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		ConsortBusinessSkillInfo skill = info.getBusinessSkillMgr().lookup(_skillId);
		if (null == skill)
			return "fail, not find consort business skill";

		skill.setProAdd(_value, getContext());

		return "ok";
	}

	@ACommand(comment = "获得家人CG[家人CG配置ID]")
	public String gainCg(long _cgId)
	{
		getOwner().getConsortComponent().getCGMgr().gainCG(_cgId, getContext());

		return "ok";
	}

	@ACommand(comment = "获得所有家人CG图鉴")
	public String gainAllCg()
	{
		List<RefConsortCg> list = RefConsortCg.getMgr().getList();
		int gainCount = 0;
		for (RefConsortCg refCg : list)
		{
			int result = getOwner().getConsortComponent().getCGMgr().gainCG(refCg.Id(), getContext());
			gainCount += result;
		}
		return "done, gained " + gainCount + " cg(s).";
	}

	@ACommand(comment = "重置家人星辉[家人配置ID]")
	public String resetHalo(long _consortId)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		info.getHaloInfo().cmdReset(getContext());

		return "ok";
	}

	@ACommand(comment = "移除家人皮肤[家人配置ID, 皮肤ID]")
	public String delSkin(long _consortId, long _skinId)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		info.getSkinMgr().cmdDelSkin(_skinId, getContext());

		return "ok";
	}

	@ACommand(comment = "测试随机邀约结果[测试次数]")
	public String testCallRes(int _count)
	{
		int[] testCount = new int[EChildBirthRes.values().length];

		for (int i = 0; i < _count; i++)
		{
			EChildBirthRes callChildRes = RefGeneral.Ref().consortCallRandPerWeight.random();
			if (null == callChildRes)
				continue;

			testCount[callChildRes.ordinal()]++;
		}

		StringBuilder sb = new StringBuilder();
		sb.append("\ngm: ").append(CommonFunc.list2String(RefGeneral.Ref().consort_call_rand_per)).append("\n");
		for (int i = 0; i < testCount.length; i++)
		{
			sb.append("\n").append(EChildBirthRes.values()[i]).append(":").append(testCount[i]);
		}

		return sb.toString();
	}

	@ACommand(comment = "获得所有妃子")
	public String gainAll()
	{
		List<RefConsort> list = RefConsort.getMgr().getList();
		for (RefConsort refConsort : list)
		{
			getOwner().gainItem(ENPItemType.CONSORT, refConsort.Id(), getContext());
		}
		return "done";
	}

	@ACommand(comment = "打印已拥有妃子列表和基础数据")
	public String printConsortList()
	{
		List<ConsortInfo> consortList = getOwner().getConsortComponent().getConsortList();
		if (consortList.isEmpty())
		{
			return "dont have consort.";
		}

		StringBuilder sb = new StringBuilder();
		sb.append("\n=== 已拥有妃子列表 ===");

		for (ConsortInfo consort : consortList)
		{
			sb.append("\n").append(consort.getConsortId()).append(" ").append(consort.getIntimacy()).append(" ").append(consort.getCharm());
		}

		return sb.toString();
	}

	@ACommand(comment = "打印妃子羁绊技能ID和等级")
	public String printConsortFettersSkills()
	{
		List<ConsortInfo> consortList = getOwner().getConsortComponent().getConsortList();
		if (consortList.isEmpty())
		{
			return "dont have consort.";
		}

		StringBuilder sb = new StringBuilder();
		sb.append("\n=== 妃子羁绊技能信息 ===");

		for (ConsortInfo consort : consortList)
		{
			if (consort.getRef().consort_fetters_skill_id > 0)
			{
				sb.append("\n").append(consort.getConsortId()).append(" ")
						.append(consort.getRef().consort_fetters_skill_id).append(" ")
						.append(consort.getFettersInfo().getSkillLvl()).append(" ");
			}
		}

		return sb.toString();
	}

	@ACommand(comment = "打印妃子加护技能ID和等级")
	public String printConsortBlessSkills()
	{
		List<ConsortInfo> consortList = getOwner().getConsortComponent().getConsortList();
		if (consortList.isEmpty())
		{
			return "dont have consort.";
		}

		StringBuilder sb = new StringBuilder();
		sb.append("\n=== 妃子加护技能信息 ===");

		for (ConsortInfo consort : consortList)
		{
			if (!consort.getRef().bless_skill_id_list.isEmpty())
			{
				StringBuilder skillInfo = new StringBuilder();
				for (int i = 0; i < consort.getRef().bless_skill_id_list.size(); i++)
				{
					long skillId = consort.getRef().bless_skill_id_list.get(i);
					ConsortBlessSkillInfo skillInfoObj = consort.getBlessSkillInfoMgr().lookup(skillId);
					if (skillInfoObj != null)
					{
						if (skillInfo.length() > 0)
							skillInfo.append(" ");

						skillInfo.append(skillId).append(" ").append(skillInfoObj.getLvl()).append(" ");
					}
				}

				sb.append("\n").append(consort.getConsortId()).append(" ").append(skillInfo);
			}
		}

		return sb.toString();
	}

	@ACommand(comment = "打印妃子商业技能总加成")
	public String printConsortBusinessSkillSum()
	{
		StringBuilder sb = new StringBuilder();
		sb.append("\n=== 妃子商业技能总加成 ===");

		// 初始化各类型加成统计 - 使用现有的makePropertySumProto机制
		ArrayList<Consort_BusinessSkillPropertySum> sumList = new ArrayList<>();

		List<ConsortInfo> consortList = getOwner().getConsortComponent().getConsortList();

		// 统计所有妃子的商业技能加成
		for (ConsortInfo consort : consortList)
		{
			consort.getBusinessSkillMgr().makePropertySumProto(sumList);
		}

		// 合并相同属性类型的加成
		Map<ESpecAttrType, Long> finalSumMap = new HashMap<>();

		for (Consort_BusinessSkillPropertySum sum : sumList)
		{
			ESpecAttrType attrType = sum.getAttr();
			finalSumMap.put(attrType, finalSumMap.getOrDefault(attrType, 0L) + sum.getProAddSum());
		}

		// 输出结果
		for (Map.Entry<ESpecAttrType, Long> entry : finalSumMap.entrySet())
		{
			sb.append("\n").append(entry.getKey().name()).append(": ").append(entry.getValue());
		}

		return sb.toString();
	}

	@ACommand(comment = "妃子聊天测试")
	public String consortChatTest(long _consortId)
	{
		RefConsortChatAi ref = RefConsortChatAi.getMgr().get(_consortId);
		if (ref == null)
			return "fail, consort chat ai ref not found.";

		String chatCode = ref.getChatCode(RefGeneral.Ref().consort_ai_chat_default_language);
		if (chatCode == null || chatCode.isEmpty())
			return "fail, consort chat ai code not found.";

		List<Common_AiChatMessage> messages = new ArrayList<>();
		messages.add(new Common_AiChatMessage(EAiChatRoleType.SYSTEM, "I've got music, candles, and nowhere else to be. You in?"));
		messages.add(new Common_AiChatMessage(EAiChatRoleType.USER, "什么类型的音乐"));
		messages.add(new Common_AiChatMessage(EAiChatRoleType.SYSTEM, "Oh, you know I do electropop. But for you, I'll play whatever makes your heart sing."));
		messages.add(new Common_AiChatMessage(EAiChatRoleType.SYSTEM, "Sorry, I don't understand what you're saying."));
		messages.add(new Common_AiChatMessage(EAiChatRoleType.SYSTEM, "I was just asking if you're in the mood for some music. How about we dive into a playlist that matches our vibe?"));

		//构造消息列表
		getUserServer().getAiServiceFunc().sendAIRequest(getOwner(), chatCode, messages, new _ICallBackIntT<String>()
		{
			@Override
			public void onRunOver(int _errCode, String _response)
			{
				CommLog.info("CmdConsort consortChatTest callback: errCode {} response {}", _errCode, _response);
			}
		});

		return "ok";
	}

	@ACommand(comment = "设置家人羁绊等级[家人配置ID, 羁绊等级]")
	public String setFettersLvl(long _consortId, int _lvl)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		RefConsortFettersLvl lvlRef = RefConsortFettersLvl.getMgr().get(_lvl);
		if(null == lvlRef)
			return "fail, not find lvl ref.";

		info.getFettersInfo().setLvl(lvlRef, getContext());

		return "ok";
	}

	@ACommand(comment = "设置家人加护技能等级[家人配置ID, 技能ID, 目标等级]")
	public String setBlessSkillLvl(long _consortId, long _skillId, int _targetLvl)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		ConsortBlessSkillInfo skill = info.getBlessSkillInfoMgr().lookup(_skillId);
		if (null == skill)
			return "fail, not find consort bless skill.";

		RefConsortBlessSkill skillRef = RefConsortBlessSkill.getMgr().get(_skillId);
		if (null == skillRef)
			return "fail, not find bless skill ref.";

		RefConsortBlessSkillLvl lvlRef = skillRef.getLevelMapMgr().getLevelData(_targetLvl);
		if (null == lvlRef)
			return "fail, not find bless skill level ref.";

		skill.setLvl(lvlRef, getContext());

		return "ok";
	}

	@ACommand(comment = "增加家人加护技能等级[家人配置ID, 技能ID, 增加值]")
	public String incrBlessSkillLvl(long _consortId, long _skillId, int _incrValue)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		ConsortBlessSkillInfo skill = info.getBlessSkillInfoMgr().lookup(_skillId);
		if (null == skill)
			return "fail, not find consort bless skill.";

		int currentLvl = skill.getLvl();
		int targetLvl = currentLvl + _incrValue;

		RefConsortBlessSkill skillRef = RefConsortBlessSkill.getMgr().get(_skillId);
		if (null == skillRef)
			return "fail, not find bless skill ref.";

		RefConsortBlessSkillLvl lvlRef = skillRef.getLevelMapMgr().getLevelData(targetLvl);
		if (null == lvlRef)
			return "fail, not find bless skill level ref.";

		skill.setLvl(lvlRef, getContext());

		return "ok";
	}

	@ACommand(comment = "删除指定家人及其所有数据[家人配置ID] 可能导致数值计算错误，慎用")
	public String delConsort(long _consortId)
	{
		return getOwner().getConsortComponent().cmdDelConsort(_consortId);
	}

	@ACommand(comment = "删除所有家人及其所有数据 可能导致数值计算错误，慎用")
	public String delAllConsort()
	{
		int count = getOwner().getConsortComponent().cmdDelAllConsort();
		return "done, deleted " + count + " consort(s).";
	}

	@ACommand(comment = "重置家人经营技能[家人配置ID, 技能ID]")
	public String resetBusinessSkill(long _consortId, long _skillId)
	{
		ConsortInfo info = getOwner().getConsortComponent().lookup(_consortId);
		if (null == info)
			return "fail, not find consort.";

		ConsortBusinessSkillInfo skill = info.getBusinessSkillMgr().lookup(_skillId);
		if (null == skill)
			return "fail, not find consort business skill.";

		// 重置属性加成为初始值100（1%）
		skill.setProAdd(100, getContext());
		// 重置普通领悟次数为0
		skill.setOpCount(false, 0);
		// 重置高级领悟次数为0
		skill.setOpCount(true, 0);

		return "ok";
	}
}
