package NPGameRes.GameObjs.Dinner;

import Common.DinnerObj.Dinner_Idx;

public class DinnerGetIdxResult 
{
	//宴会数据
	private Dinner_Idx _m_diDinnerIdx;
	//当前队列所处顺序下标
	private int _m_iIdx;
	//是否有上一条宴会数据
	private boolean _m_bHasPre;
	//是否有下一条宴会数据
	private boolean _m_bHasNext;
	
	public Dinner_Idx getDinnerIdx() {return _m_diDinnerIdx;}
	public void setDinnerIdx(Dinner_Idx _dinnerInfo) {_m_diDinnerIdx = _dinnerInfo;}
	
	public int getIdx() {return _m_iIdx;}
	public void setIdx(int _idx) {_m_iIdx = _idx;}
	
	public boolean hasPre() {return _m_bHasPre;}
	public void setHasPre() {_m_bHasPre = true;}
	public void setHasPre(boolean _hasPre) {_m_bHasPre = _hasPre;}
	
	public boolean hasNext() {return _m_bHasNext;}
	public void setHasNext() {_m_bHasNext = true;}
	public void setHasNext(boolean _hasNext) {_m_bHasNext = _hasNext;}
	
	/**
	 * 重置数据
	 */
	public void reset()
	{
		_m_diDinnerIdx = null;
		_m_bHasPre = false;
		_m_bHasNext = false;
	}
}
