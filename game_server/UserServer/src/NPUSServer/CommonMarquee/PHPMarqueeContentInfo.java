package NPUSServer.CommonMarquee;

import Common.Common_StringList;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPUSServer.NPUserServer;
import USDB.Bo.CommonMarqueePhpLangBO;

import java.util.ArrayList;
import java.util.HashMap;

public class PHPMarqueeContentInfo 
{
	private NPUserServer _m_server;

	//后台跑马灯实例ID
	private long _m_lMarqueeDbid;
	//跑马灯不同语言内容数据
	private ArrayList<PHPMarqueeContentLangInfo> _m_alLangContentList;
	
	public PHPMarqueeContentInfo(NPUserServer _server, long _marqueeDbid)
	{
        _m_server = _server;
    	
        _m_lMarqueeDbid = _marqueeDbid;
		
		_m_alLangContentList = new ArrayList<>();
	}

    public NPUserServer getUSServer() {return _m_server;}
    
	public long getMarqueeDbid() {return _m_lMarqueeDbid;}
	
	protected void _initFromDB(CommonMarqueePhpLangBO _bo) 
	{
		PHPMarqueeContentLangInfo info = new PHPMarqueeContentLangInfo(getUSServer(), _bo);
		_m_alLangContentList.add(info);
	}
	
	/**
	 * 查找指定语言的内容（带默认语言）
	 * @param _lang
	 * @param _defaultLang
	 * @return
	 */
	protected PHPMarqueeContentLangInfo _lookupLang(String _lang, String _defaultLang) 
	{
		PHPMarqueeContentLangInfo info = null;
		
		for(int i = 0; i < _m_alLangContentList.size(); i++)
		{
			PHPMarqueeContentLangInfo tmpInfo = _m_alLangContentList.get(i);
			if(null == tmpInfo)
				continue;
			
			//如果等于指定语言，则选定后退出，如果等于默认语言，则选定后需要继续遍历，确认是否有指定语言
			if(_lang.equalsIgnoreCase(tmpInfo.getLang()))
			{
				info = tmpInfo;
				break;
			}
			else if(_defaultLang.equalsIgnoreCase(tmpInfo.getLang()))
			{
				info = tmpInfo;
			}
			else if(null == info) //如果不存在指定语言/默认语言，则使用第一条数据
			{
				info = tmpInfo;
			}
		}
		
		return info;
	}
	
	/**
	 * 移除指定语言的内容
	 * @param _lang
	 */
	protected void _removeLang(String _lang) 
	{
		for(int i = 0; i < _m_alLangContentList.size(); i++)
		{
			PHPMarqueeContentLangInfo tmpInfo = _m_alLangContentList.get(i);
			if(null == tmpInfo)
				continue;
			
			//如果等于指定语言，则选定后退出，如果等于默认语言，则选定后需要继续遍历，确认是否有指定语言
			if(_lang.equalsIgnoreCase(tmpInfo.getLang()))
			{
				_m_alLangContentList.remove(i);
				tmpInfo._remove();
				break;
			}
		}
	}
	
	/**
	 * 保存内容
	 * @param _lang
	 * @param _paramList
	 * @param _content
	 */
	protected void _saveContent(String _lang, ArrayList<String> _paramList, String _content) 
	{
		//先移除指定内容
		_removeLang(_lang);
		
		//添加内容数据
		BM bmObj = getUSServer().getBM();
		
		//对参数列表进行检查
		Common_StringList stringList = null;
		if(null != _paramList && !_paramList.isEmpty())
		{
			stringList = new Common_StringList();
			stringList.getValueList().addAll(_paramList);
		}
		
		CommonMarqueePhpLangBO bo = new CommonMarqueePhpLangBO();
		bo.setMarqueeDbid(bmObj, getMarqueeDbid());
		bo.setLang(bmObj, _lang);
		if(null != stringList)
		{
			bo.setParamList(bmObj, CommonFunc.ByteBfferToBytes(stringList.makePackage()));
		}
		if(null != _content)
		{
			bo.setContent(bmObj, _content);
		}
		bo.insert(bmObj);
		
		PHPMarqueeContentLangInfo info = new PHPMarqueeContentLangInfo(getUSServer(), bo, stringList);
		_m_alLangContentList.add(info);
	}
	
	/**
	 * 销毁该后台所有数据
	 */
	protected void _discard()
	{
		HashMap<String, Object> conditions = new HashMap<>();
		conditions.put("marquee_dbid", getMarqueeDbid());
		getUSServer().getBM().getBM(CommonMarqueePhpLangBO.class).delAll(conditions);
		
		_m_alLangContentList.clear();
	}
}
