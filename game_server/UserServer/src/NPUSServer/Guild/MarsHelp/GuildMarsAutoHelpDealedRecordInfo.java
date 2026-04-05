package NPUSServer.Guild.MarsHelp;

import NPCommon.Util.CommonFunc;

/**
 * 每日帮助记录
 * @author mj
 *
 */
public class GuildMarsAutoHelpDealedRecordInfo 
{
	private long _m_lCid;
	private int _m_iLastDate;
	private int _m_iCount;
	
	public GuildMarsAutoHelpDealedRecordInfo(long _cid)
	{
		_m_lCid = _cid;
	}
	public GuildMarsAutoHelpDealedRecordInfo(long _cid, int _lastDate, int _count)
	{
		_m_lCid = _cid;
		_m_iLastDate = _lastDate;
		_m_iCount = _count;
	}
	
	public long getCid() {return _m_lCid;}
	public int getLastDate() {return _m_iLastDate;}
	
	public int getCount()
	{
		int nowTag = CommonFunc.getNowTagYYYYMMDD();
		
		return nowTag == _m_iLastDate ? _m_iCount : 0;
	}
	
	protected void _refresh()
	{
		int nowTag = CommonFunc.getNowTagYYYYMMDD();
		if(nowTag == _m_iLastDate)
			return;

		_m_iLastDate = nowTag;
		_m_iCount = 0;
	}
	
	protected void _setCount(int _count)
	{
		_m_iLastDate = CommonFunc.getNowTagYYYYMMDD();
		_m_iCount = _count;
	}
}
