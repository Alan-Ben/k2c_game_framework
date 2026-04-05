package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp.ExploreTeam;

import NPEnum.ENPPlayerPropertyType;
import NPGameRes.Refs.Hero.RefHeroStar;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.USLog;

import java.util.ArrayList;

public class MarsExploreTeamCalculate 
{
	/**
	 * 计算带兵量
	 * 
	 * 公式：
	 		带兵量=(玩家属性_火星队伍带兵基础数量)*(1+玩家属性_火星队伍带兵量加成万分比)+玩家属性_火星队伍带兵额外数量
	 		单队伍实力=带兵量*单兵实力值
	 		
	 20251211 - 公式调整：
	 		GOB-7494 【优化-0】火星带兵量公式优化----服务端
	 		https://www.teambition.com/task/693a64c66cadaceacc1baf5d
	 		
	 		带兵量=(玩家属性_火星队伍带兵基础数量+队伍大臣实力增加带兵数量)*(1+玩家属性_火星队伍带兵量加成万分比)+玩家属性_火星队伍带兵额外数量
	 		队伍大臣实力增加带兵数量=(√大臣1实力+√大臣2实力+√大臣3实力)/削减系数C
	 		
	 		general表增加字段
				mars_explore_lead_soldier_cut_coefficient    10000    【火星探索】大臣实力换算带兵量削减系数（万分比）
	 * 
	 * @param _team
	 */
	public static void calTroopNum(MarsExploreTeam _team, StringBuilder _sb)
	{
		if(null != _sb)
		{
			_sb.append("\n--------------- cal troop num ---------------");
		}
		
		//尚未解锁
		if(!_team.checkUnlock())
		{
			if(null != _sb)
			{
				_sb.append("\nnot unlock");
			}
			return;
		}
		
		//削减系数C
		int cutCoefficient= RefGeneral.Ref().mars_explore_lead_soldier_cut_coefficient;
		if(cutCoefficient <= 0)
		{
			if(null != _sb)
			{
				_sb.append("\n mars_explore_lead_soldier_cut_coefficient:").append(cutCoefficient).append(" error.");
			}
			return;
		}
		
		//玩家属性_火星队伍带兵基础数量
		long troopNum = _team.getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TEAM_TROOP_NUM);
		if(null != _sb)
		{
			_sb.append("\ntroopNum:").append(troopNum);
		}
		long troopPer = _team.getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TEAM_TROOP_NUM_PER);
		if(null != _sb)
		{
			_sb.append("\ntroopPer:").append(troopPer);
		}
		
		//玩家属性_火星队伍带兵额外数量
		long troopExtNum = _team.getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TEAM_TROOP_EXT_NUM);
		if(null != _sb)
		{
			_sb.append("\ntroopExtNum:").append(troopExtNum);
		}
		
		//队伍大臣实力增加带兵数量=(√大臣1实力+√大臣2实力+√大臣3实力)/削减系数C
		long heroPower = _team.getHeroPowerSum();
		if(null != _sb)
		{
			_sb.append("\nheroPowerSum:").append(heroPower);
		}
		//GOB-7494 【优化-0】火星带兵量公式优化----服务端
		//https://www.teambition.com/task/693a64c66cadaceacc1baf5d
		//削减系数是万分比 修改为先×10000 / 消减系数
		heroPower = heroPower * 10000 / cutCoefficient;
		if(null != _sb)
		{
			_sb.append("\nheroFinalPowerSum:").append(heroPower);
		}
		
		//带兵量=(玩家属性_火星队伍带兵基础数量+队伍大臣实力增加带兵数量)*(1+玩家属性_火星队伍带兵量加成万分比)+玩家属性_火星队伍带兵额外数量
        long value = ((troopNum + heroPower) * (10000 + troopPer) / 10000) + troopExtNum;
		if(null != _sb)
		{
			_sb.append("\nvalue:").append(value);
		}
		
		_team.setTroopNum(value);
	}
	
	 /**
	  * 计算队伍实力
	  * 
	  * 公式：
	  		单兵实力值=玩家属性_单兵基础实力值*(1+玩家属性_单兵实力值加成万分比)
	  *
	  * @param _team
	  */
	public static void calSoldierTeamPower(MarsExploreTeam _team, StringBuilder _sb)
	{
		if(null != _sb)
		{
			_sb.append("\n--------------- cal team power ---------------");
		}
		
		//队伍未解锁
		if(!_team.checkUnlock())
		{
			if(null != _sb)
			{
				_sb.append("\nnot unlock");
			}
			return;
		}
		
		//玩家属性_单兵基础实力值
		long soldierPower = _team.getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER);
		if(null != _sb)
		{
			_sb.append("\nsoldierPower:").append(soldierPower);
		}

		//玩家属性_单兵实力值加成万分比
		long heroAddPer = 0;
		ArrayList<Long> heroIdList = _team.getHeroIdList();
		if(null != _sb)
		{
			_sb.append("\nheroSize:").append(heroIdList.size());
		}
		for(int i = 0; i < heroIdList.size(); i++)
		{
			long heroId = heroIdList.get(i);
			HeroInfo hero = _team.getUserData().getHeroComponent().lookupHero(heroId);
			if(null == hero)
			{
				USLog.error(_team.getUSServer(), "player:{} hero:{} mars team can not get hero.", _team.getCid(), heroId);
				continue;
			}

			RefHeroStar heroStarRef = hero.getHeroStarRef();
			if (heroStarRef == null)
				continue;
			
			heroAddPer += heroStarRef.mars_team_player_property.getPropValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);
			if(null != _sb)
			{
				_sb.append("\nhero:").append(heroId).append("\nheroAddPer:").append(heroAddPer);
			}
		}

		long playerSoldierAddPer = _team.getUserData().getPlayerComponent().getPropertyMgr().getValue(ENPPlayerPropertyType.MARS_TEAM_SOLDIER_POWER_PER);
		long value = soldierPower * (10000 + playerSoldierAddPer + heroAddPer) / 10000;
		if(null != _sb)
		{
			_sb.append("\nTeamPower:").append(value);
		}
		
		_team.setSoldierPower(value);
	}
}
