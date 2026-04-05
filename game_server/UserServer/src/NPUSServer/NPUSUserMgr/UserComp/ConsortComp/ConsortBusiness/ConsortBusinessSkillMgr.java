package NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortBusiness;

import Common.ConsortObj.Consort_BusinessSkill;
import Common.ConsortObj.Consort_BusinessSkillPropertySum;
import CommonEnum.EBonusFilterType;
import CommonEnum.ESpecAttrType;
import NPGameRes.Refs.Consort.RefConsortBusinessSkill;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_015_ConsortOp;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.PlayerConsortBusinessSkillBO;

import java.util.ArrayList;

/**
 * 家人经营技能管理
 * @author mj
 *
 */
public class ConsortBusinessSkillMgr 
{
	//家人数据
	private final ConsortInfo _m_ciConsort;
	
	//技能数据
	private ArrayList<ConsortBusinessSkillInfo> _m_alSkillList;

	public ConsortBusinessSkillMgr(ConsortInfo _consort)
	{
		_m_ciConsort = _consort;
		
		_m_alSkillList = new ArrayList<>();
	}

	//玩家数据对象
	public NPUSUserData getUserData() {return _m_ciConsort.getUserData();}
	//服务器数据对象
	public NPUserServer getUSServer() {return getUserData().getUSServer();}
	
	//家人相关数据
	public ConsortInfo getConsort() {return _m_ciConsort;}
	public long getConsortId() {return _m_ciConsort.getConsortId();}

	/**
	 * bo数据初始化
	 * @param _bo
	 */
	public void _initBo(PlayerConsortBusinessSkillBO _bo)
	{
		RefConsortBusinessSkill refSkill = RefConsortBusinessSkill.getMgr().get(_bo.getSkillId());
		if (refSkill == null)
		{
			USLog.error(getUSServer(), "player:{} consort:{} init business skill:{} bo fail, not find ref."
					, getUserData().getCid(), getConsortId(), _bo.getSkillId());
			return;
		}

		ConsortBusinessSkillInfo info = new ConsortBusinessSkillInfo(_m_ciConsort, refSkill, _bo);
		_m_alSkillList.add(info);
	}

	/**
	 * 检查解锁技能
	 */
	public void checkUnlockSkill(boolean _isInit)
	{
		getUserData().lockUser();
		try{
			for (RefConsortBusinessSkill skillRef : RefConsortBusinessSkill.getMgr().getList())
			{
				if(null == skillRef)
					continue;

				if (lookup(skillRef.Id()) != null)
					continue;

				if (skillRef.unlock_need_intimacy > getConsort().getIntimacy())
					continue;

				ConsortBusinessSkillInfo info = new ConsortBusinessSkillInfo(_m_ciConsort, skillRef);
				_m_alSkillList.add(info);

				//推送数据
				if (!_isInit)
					getUserData().sendMsgToGC(US2GCWriter_015_ConsortOp.make_058_OnBusinessSkillChg(info));
			}
		}finally
		{
			getUserData().unlockUser();
		}
	}

	/**********
	 * 构造数据列表协议
	 * @param _list
	 */
	public void makeProto(ArrayList<Consort_BusinessSkill> _list)
	{
		getUserData().lockUser();
		
		try
		{
            for (ConsortBusinessSkillInfo skill : _m_alSkillList)
            {
                if (null == skill)
                    continue;

                _list.add(skill.toProto());
            }
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 计算有操作过的技能数量
	 * @return
	 */
	public int calOpSkillCount()
	{
		getUserData().lockUser();
		
		try
		{
			int sum = 0;
			for(int i = 0; i < _m_alSkillList.size(); i++)
			{
				ConsortBusinessSkillInfo skill = _m_alSkillList.get(i);
				if(null == skill)
					continue;
				
				if(skill.getNormalOpCount() > 0 || skill.getAdvanceOpCount() > 0)
					sum++;
			}
			
			return sum;
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
	
	/**
	 * 构造家人的相性加成属性和数据
	 * @param _list
	 */
	public void makePropertySumProto(ArrayList<Consort_BusinessSkillPropertySum> _list)
	{
		getUserData().lockUser();
		
		try
		{
			int[] attrArr = new int[ESpecAttrType.ESpecAttrType_Length];
			for(int i = 0; i < _m_alSkillList.size(); i++)
			{
				ConsortBusinessSkillInfo skill = _m_alSkillList.get(i);
				if(null == skill)
					continue;
				
				attrArr[skill.getRef().property.ordinal()] += skill.getProAdd();
			}
			
			for(int i = 0; i < attrArr.length; i++)
			{
				Consort_BusinessSkillPropertySum sum = new Consort_BusinessSkillPropertySum();
				sum.setAttr(ESpecAttrType.ESpecAttrType_FromInt(i));
				sum.setProAddSum(attrArr[i]);
				
				_list.add(sum);
			}
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}
	
	/***
	 * 查找指定技能数据
	 * @param _skillId
	 * @return
	 */
	public ConsortBusinessSkillInfo lookup(long _skillId)
	{
		getUserData().lockUser();
		
		try
		{
			for(int i = 0; i < _m_alSkillList.size(); i++)
			{
				ConsortBusinessSkillInfo skill = _m_alSkillList.get(i);
				if(null == skill)
					continue;
				
				if(skill.getSkillId() == _skillId)
					return skill;
			}
			
			return null;
		}
		finally 
		{
			getUserData().unlockUser();
		}
	}

	/**
	 * 移除所有经营技能全局属性加成（GM删除妃子时调用）
	 */
	public void cmdDiscardBonus()
	{
		for (ConsortBusinessSkillInfo skill : _m_alSkillList)
		{
			if (null == skill) continue;
			if (skill.getAttrType() == ESpecAttrType.NONE)
			{
				for (ESpecAttrType type : ESpecAttrType.ESpecAttrType_Values)
				{
					getUserData().getBonusMgr().removeModifierToFilter(
							EBonusFilterType.BUILDING_ATTR, type.ordinal(), skill.getBonusPropertyModifier());
				}
			}
			else
			{
				getUserData().getBonusMgr().removeModifierToFilter(
						EBonusFilterType.BUILDING_ATTR, skill.getAttrType().ordinal(), skill.getBonusPropertyModifier());
			}
		}
	}

	/**
	 * 截面数据（仅供上级截面方法使用）
	 * @return
	 */
	public String _sectionLog()
	{
		StringBuilder sb = new StringBuilder();

		for(int i = 0; i < _m_alSkillList.size(); i++)
		{
			ConsortBusinessSkillInfo info = _m_alSkillList.get(i);
			if(null == info)
				continue;

			sb.append(info.getAttrType()).append(":").append(info.getProAdd()).append(";");
		}

		return sb.toString();
	}
}
