package NPGameRes.GameObjs.Dinner;

import Common.DinnerObj.Dinner_Idx;

import java.util.ArrayList;

/**
 * 查询宴会索引数据结果
 * @author mj
 *
 */
public class DinnerGetIdxListResult 
{
	private ArrayList<Dinner_Idx> _m_alDinnerIdxList;
	private boolean _m_bHasNext;
	
	public DinnerGetIdxListResult()
	{
		_m_alDinnerIdxList = new ArrayList<>();
		_m_bHasNext = false;
	}
	
	public ArrayList<Dinner_Idx> getIdxList() {return _m_alDinnerIdxList;}
	public void addDinnerIdx(Dinner_Idx _idx) {_m_alDinnerIdxList.add(_idx);}
	public void setDinnerIdxList(ArrayList<Dinner_Idx> _idxList) 
	{
		for(int i = 0; i < _idxList.size(); i++)
		{
			Dinner_Idx idx = _idxList.get(i);
			if(null == idx)
				continue;
			
			_m_alDinnerIdxList.add(idx);
		}
	}
	
	public boolean hasNext() {return _m_bHasNext;}
	public void setHasNext() {_m_bHasNext = true;}
	public void setHasNext(boolean _hasNext) {_m_bHasNext = _hasNext;}
}
