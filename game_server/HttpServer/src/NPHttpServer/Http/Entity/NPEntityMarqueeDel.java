package NPHttpServer.Http.Entity;

import java.util.ArrayList;

/**
 * 跑马灯删除
 */
public class NPEntityMarqueeDel
{
	//后台跑马灯唯一ID
    private long _m_lPHPId;
    //必填，US列表
    private ArrayList<Integer> _m_alUsIdList;
	
    public NPEntityMarqueeDel()
    {
    	_m_alUsIdList = new ArrayList<>();
    }
    
    public long getPHPId() {return _m_lPHPId;}
    public void setPHPId(long _value) {_m_lPHPId = _value;}

    public ArrayList<Integer> getUsIdList() {return _m_alUsIdList;}
    public void addUsIdList(Integer _value) {_m_alUsIdList.add(_value);}
}
