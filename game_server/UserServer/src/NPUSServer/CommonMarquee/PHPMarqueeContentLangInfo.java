package NPUSServer.CommonMarquee;

import Common.Common_StringList;
import NPUSServer.NPUserServer;
import USDB.Bo.CommonMarqueePhpLangBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;

public class PHPMarqueeContentLangInfo 
{
	private NPUserServer _m_server;

	private long _m_lDBID;
	private String _m_sLang;
	private ArrayList<String> _m_alParamList;
	private String _m_sContent;
	
	public PHPMarqueeContentLangInfo(NPUserServer _server, CommonMarqueePhpLangBO _bo)
	{
        _m_server = _server;
        
        _m_lDBID = _bo.getId();
        _m_sLang = _bo.getLang();
        
        _m_alParamList = new ArrayList<>();
        if(null != _bo.getParamList())
        {
        	ByteBuffer buff = ByteBuffer.wrap(_bo.getParamList());
        	Common_StringList stringList = new Common_StringList();
        	stringList.readPackage(buff);
        	
        	_m_alParamList = stringList.getValueList();
        }
        
        _m_sContent = _bo.getContent();
	}
	public PHPMarqueeContentLangInfo(NPUserServer _server, CommonMarqueePhpLangBO _bo, Common_StringList _stringList)
	{
        _m_server = _server;
        
        _m_lDBID = _bo.getId();
        _m_sLang = _bo.getLang();
        
        _m_alParamList = new ArrayList<>();
        if(null != _stringList)
        {
        	_m_alParamList = _stringList.getValueList();
        }
        
        _m_sContent = _bo.getContent();
	}
	
    public NPUserServer getUSServer() {return _m_server;}
    
    public long getDBID() {return _m_lDBID;}
    public String getLang() {return _m_sLang;}
    public ArrayList<String> getParamList() {return _m_alParamList;}
    public String getContent() {return _m_sContent;}
    
    /**
     * 移除数据
     */
    protected void _remove() 
    {
    	getUSServer().getBM().getBM(CommonMarqueePhpLangBO.class).delAll("id", getDBID());
	}
}
