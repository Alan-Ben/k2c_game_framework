package NPGameRes.GameObjs.Mars;

import NPCommon.CommonObj.NPCommonCostItem;
import NPEnum.EQuality;
import NPGameRes.GameObjs.Mars.UsMarsBattle._IUsMarsBattleObj;

import java.util.ArrayList;

public class MarsBattleQuality implements _IUsMarsBattleObj
{
	private EQuality _m_eQuality;
	public void setQuality(EQuality _value) {_m_eQuality = _value;}
	public EQuality getQuality() {return _m_eQuality;}

	private long _m_lSoliderPower;
	public void setSoliderPower(long _value) {_m_lSoliderPower = _value;}
	public long getSoliderPower() {return _m_lSoliderPower;}

	private long _m_lSoliderNum;
	public void setSoliderNum(long _value) {_m_lSoliderNum = _value;}
	public long getSoliderNum() {return _m_lSoliderNum;}
	
	private ArrayList<NPCommonCostItem> _m_alCostItemList = new ArrayList<>();
	public void addItem(NPCommonCostItem _item) {_m_alCostItemList.add(_item);}
	public ArrayList<NPCommonCostItem> getItemList() {return _m_alCostItemList;}

	/************************* 战斗处理对象的防守方重载函数 ******************/
	/**
	 * 获取当前可参战的战斗人员数量
	 * @return
	 */
	public long getTeamSoldierNum()
	{
		return _m_lSoliderNum;
	}

	/**
	 * 获取参战的单兵实力
	 * @return
	 */
	public long getTeamSoldierPower()
	{
		return _m_lSoliderPower;
	}

	/**
	 * 增加伤兵数量，注意这里是增量不是全量
	 * @param _hurtNum
	 */
	public void addHurtSoldierNum(long _hurtNum)
	{
	}

	/**
	 * 记录战斗日志的接口
	 * @param _isAttacker
	 * @param _isAttWin
	 * @param _attackPower
	 * @param _attckHurtNum
	 * @param _attackTroopNum
	 * @param _defencePower
	 * @param _defenceHurtNum
	 * @param _defenceTroopNum
	 */
	public void logBattle(boolean _isAttacker, boolean _isAttWin, _IUsMarsBattleObj _enemy, long _attackPower, long _attckHurtNum
			, long _attackTroopNum, long _defencePower, long _defenceHurtNum, long _defenceTroopNum)
	{
	}
	/************************* 战斗处理对象的防守方重载函数 end ******************/
}
