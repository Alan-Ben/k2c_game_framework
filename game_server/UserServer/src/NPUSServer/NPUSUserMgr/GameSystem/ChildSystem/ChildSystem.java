package NPUSServer.NPUSUserMgr.GameSystem.ChildSystem;

import CommonEnum.EBonusPropertyType;
import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Child.RefChildAttr;
import NPGameRes.Refs.Child.RefChildCareer;
import NPGameRes.Refs.Child.RefChildInitRes;
import NPGameRes.Refs.Child.RefChildQuality;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildInfo;
import NPUSServer.NPUSUserMgr.UserComp.ChildComp.Child.ChildSeatInfo;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.USLog;

/**
 * 子嗣系统接口
 * @author mj
 *
 */
public class ChildSystem 
{
	/**
	 * 选择出生配置
	 * @return
	 */
	public static RefChildInitRes chooseInitRes()
	{
		return RefChildInitRes.getMgr().getRnd();
	}
	
	/**
	 * 选择相性
	 * @return
	 */
	public static RefChildAttr chooseAttr()
	{
		return RefChildAttr.getMgr().getRnd();
	}
	
	/**
	 * 创建子嗣
	 * @param _consort
	 * @param _isGiftde
	 * @param _seatInfo
	 * @param _context
	 * @return
	 */
	public static ChildInfo createChild(ConsortInfo _consort, boolean _isGiftde, ChildSeatInfo _seatInfo, NPPlayerContext _context)
	{
		//子嗣天资系数 consort_fetters_lvl.income_bonus
		if(null == _consort.getFettersInfo().getLvlRef())
		{
			USLog.error(_consort.getUSServer(), "player:{} consort:{} create child fail, not find consort fetters lvl ref.", _consort.getUserData().getCid(), _consort.getConsortId());
			return null;
		}
		
		//出生配置
		RefChildInitRes initResRef = chooseInitRes();
		if(null == initResRef)
		{
			USLog.error(_consort.getUSServer(), "player:{} create child fail, not find initResRef.", _consort.getUserData().getCid());
			return null;
		}
		
		//相性配置
		RefChildAttr attrRef = chooseAttr();
		if(null == attrRef)
		{
			USLog.error(_consort.getUSServer(), "player:{} create child fail, not find attrRef.", _consort.getUserData().getCid());
			return null;
		}
		
		//职业配置
		RefChildCareer careerRef = attrRef.careerWeiList.random();
		if(null == careerRef)
		{
			USLog.error(_consort.getUSServer(), "player:{} attr:{} create child fail, not find careerRef.", _consort.getUserData().getCid(), attrRef.type);
			return null;
		}
		
		//子嗣品质配置
		RefChildQuality qualityRef = RefChildQuality.getMgr().get(_consort.getFettersInfo().getLvlRef().adopt_child_quality_id);
		if(null == qualityRef)
		{
			USLog.error(_consort.getUSServer(), "player:{} consort:{} fettersLvl:{} create child fail, not find qualityRef."
					, _consort.getUserData().getCid(), _consort.getConsortId(), _consort.getFettersInfo().getLvl());
			return null;
		}

		//获得训练位
		ChildSeatInfo seat;
		if (_seatInfo != null)
		{
			seat = _seatInfo.tryUse();
		}else
		{
			seat = _consort.getUserData().getChildComponent().getChildMgr().takeSeat();
		}

		if(null == seat)
		{
			USLog.error(_consort.getUSServer(), "player:{} create child fail, not take seat.", _consort.getUserData().getCid());
			return null;
		}
		
		//子嗣基础转速 = 子嗣初始亲密度（获得子嗣时对应妃子的亲密度）* 子嗣天资系数 * 子嗣系数万分比 / 20 
		//Denis备注：培养收益 = 基础转速 * 配置计算的次数（20次）
		long bonusPer = _consort.getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.CHILD_BONUS_PER);
		long baseBonus = (long) Math.ceil(1.0f * _consort.getIntimacy() * (1.0f * _consort.getFettersInfo().getLvlRef().income_bonus / 10000f) * (1.0f * bonusPer / 10000f) / 20f);
		
		//生成子嗣数据
		ChildInfo child = _consort.getUserData().getChildComponent().getChildMgr().gainChild(_consort.getConsortId(), 
				_consort.getIntimacy(), 
				initResRef, 
				qualityRef, 
				attrRef, 
				careerRef, 
				seat.getSeatId(), 
				_isGiftde, 
				baseBonus, 
				_consort.getFettersInfo().getStudyBonus(), 
				_context);
		
		return child;
	}

	/***
	 * 计算成年子嗣毕业收益
	 * 
		基础村庄收益 = （配表）player_lvl.child_educate_get_base_earnings
		监护者加成（万分比）= 亲密度 * 监护者加成计算系数 / 100
			监护者加成计算系数 = （配表）consort_fetters_lvl.caretaker_bonus_calculate_coefficient
		天资加成（万分比）=（配表）子嗣天资系数（已存在的配表字段）= consort_fetters_lvl.income_bonus
		职业加成（万分比）=（配表）child_career.career_add
		评级加成（万分比）=（配表）player_lvl.child_graduate_get_earnings_add
		伙伴技能 = 大臣系统的 EBonusPropertyType.FIN_STUDY_BONUS, //6 ==== 毕业收益加成
		家人羁绊 = 妃子系统的 EBonusPropertyType.FIN_STUDY_BONUS, //6 ==== 毕业收益加成
		----
		其他：待确认
	 *	
	 * @param _child
	 * @return
	 */
	public static long calAdultBonus(ChildInfo _child)
	{
		//村庄基础收益
		long baseBuildingBonus = _child.getUserData().getPlayerComponent().getCurLevel().child_educate_get_base_earnings;
		
		//计算加成
		long per = 0;
		//评级加成（配表）：player_lvl.child_graduate_get_earnings_add
		per += _child.getUserData().getPlayerComponent().getCurLevel().child_graduate_get_earnings_add;
		//职业加成（配表）：child_career.career_add
		if(null != _child.getCareerRef())
		{
			per += _child.getCareerRef().career_add;
		}
		//全局属性加成 EBonusPropertyType.FIN_STUDY_BONUS
		per += _child.getBonusPropertyValue(EBonusPropertyType.FIN_STUDY_BONUS_PER);
		//监护者加成
		ConsortInfo consort = _child.getUserData().getConsortComponent().lookup(_child.getConsortId());
		if(null != consort)
		{
			if(null != consort.getFettersInfo().getLvlRef())
			{
				//天资加成（配表）：子嗣天资系数（已存在的配表字段）= consort_fetters_lvl.income_bonus
				per += consort.getFettersInfo().getLvlRef().income_bonus;

				//监护者加成 = 监护者加成（万分比）= 亲密度 * 监护者加成计算系数 / 100
				per +=  Math.ceil(1.0f * _child.getInitIntimacy() * consort.getFettersInfo().getLvlRef().caretaker_bonus_calculate_coefficient / 100f);
			}
		}
		
		//计算子嗣毕业收益
		long graduateBonus = (long) Math.ceil(baseBuildingBonus * (1 + 1.0f * per / 10000f)) + _child.getBonusPropertyValue(EBonusPropertyType.FIN_STUDY_BONUS);;
		
		//卷王子嗣，毕业收益X2
		if(_child.isGiftde())
		{
			long giftExtraAddPer = _child.getBonusPropertyValue(EBonusPropertyType.GIFT_CHILD_FIN_STUDY_BONUS);
			graduateBonus = (long) Math.ceil((graduateBonus * 2) * (10000 + giftExtraAddPer) / 10000d);
		}
		
		return graduateBonus;
	}
}
