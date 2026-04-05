package NPUSServer.NPUSUserMgr.UserComp.ConsortComp;

import CommonEnum.EBonusPropertyType;

/**
 * 家人属性计算
 * @author mj
 *
 */
public class ConsortCalculator 
{
	/***
	 * 计算亲密度
	 * @param _consort
	 * @return
	 */
	public static long calIntimacy(ConsortInfo _consort)
	{
		if(null == _consort)
			return 0;
		
		//1. 自身数据
		long consortValue = _consort.getBo().getIntimacy();
		//2. 星辉技能加成数据
		long consortHaloValue = _consort.getHaloInfo().getIntimacyAdd();
		//3. 获取全局属性加成
		long playerValue = _consort.getPlayerBonusAddValue(EBonusPropertyType.INTIMACY);
		
		return consortValue + consortHaloValue + playerValue;
	}
	
	/***
	 * 计算加护力
	 * @param _consort
	 * @return
	 */
	public static long calCharm(ConsortInfo _consort)
	{
		if(null == _consort)
			return 0;
		
		//1. 自身数据
		long consortValue = _consort.getBo().getCharm();
		//2. 星辉技能加成数据
		long consortHaloValue = _consort.getHaloInfo().getCharmAdd();
		//3. 获取全局属性加成
		long playerValue = _consort.getPlayerBonusAddValue(EBonusPropertyType.CHARM);
		
		return consortValue + consortHaloValue + playerValue;
	}
}
