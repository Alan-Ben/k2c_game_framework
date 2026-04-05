package NPUSServer.CommonMarquee;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.ServerObj.ServerObj_PHPMarqueeContent;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.CommonMarqueePhpLangBO;

import java.util.ArrayList;
import java.util.List;

public class PHPMarqueeContentMgr 
{
	private NPUserServer _m_server;

	//跑马灯信息列表
	private ArrayList<PHPMarqueeContentInfo> _m_alContentList;
	
    private MutexAtom _m_mutex;

    public PHPMarqueeContentMgr(NPUserServer _server)
    {
        _m_server = _server;
        
        _m_alContentList = new ArrayList<>();
        
        _m_mutex = new MutexAtom();
    }
    
    public NPUserServer getUSServer() {return _m_server;}
    
    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}
    
    public boolean sInit()
    {
    	List<CommonMarqueePhpLangBO> boList = getUSServer().getBM().getBM(CommonMarqueePhpLangBO.class).s_findAll();
        if (boList == null)
        {
        	USLog.error(getUSServer(), "Inited CommonMarqueePhpLangBO Fail.");
        	return false;
        }
        
        for(int i = 0; i < boList.size(); i++)
        {
        	CommonMarqueePhpLangBO bo = boList.get(i);
        	if(null == bo)
        		continue;
        	
        	//检查对应的PHP跑马灯数据是否存在，不存在则不初始化，需要手动删除
        	if(null == getUSServer().getMarqueeMgr().lookupPHPMarquee(bo.getMarqueeDbid()))
        	{
        		USLog.error(getUSServer(), "Init PHP:{} Marquee Lang Content No Exists.", bo.getMarqueeDbid());
        		continue;
        	}
        	
        	ensureMarquee(bo.getMarqueeDbid())._initFromDB(bo);
        }
        
    	return true;
    }
    
    /**
     * 构造后台跑马灯对象
     * @param _marqueeDbid
     * @return
     */
    public PHPMarqueeContentInfo ensureMarquee(long _marqueeDbid)
    {
    	_lock();
    	
    	try
    	{
    		
    		for(int i = 0; i < _m_alContentList.size(); i++) 
    		{
    			PHPMarqueeContentInfo info = _m_alContentList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getMarqueeDbid() == _marqueeDbid)
    			{
    				return info;
    			}
    		}
    		
    		PHPMarqueeContentInfo info = new PHPMarqueeContentInfo(getUSServer(), _marqueeDbid);
    		_m_alContentList.add(info);
    		
    		return info;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 查找指定后台跑马灯ID的数据
     * @param _marqueeDbid
     * @return
     */
    public PHPMarqueeContentInfo lookup(long _marqueeDbid)
    {
    	_lock();
    	
    	try
    	{
    		
    		for(int i = 0; i < _m_alContentList.size(); i++) 
    		{
    			PHPMarqueeContentInfo info = _m_alContentList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getMarqueeDbid() == _marqueeDbid)
    			{
    				return info;
    			}
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 获取指定后台跑马灯+指定语言的内容（需要默认语言）
     * @param _marqueeDbid
     * @param _lang
     * @param _defaultLang
     * @return
     */
    public PHPMarqueeContentLangInfo lookupLang(long _marqueeDbid, String _lang, String _defaultLang)
    {
    	_lock();
    	
    	try
    	{
    		PHPMarqueeContentInfo info = lookup(_marqueeDbid);
    		if(null == info)
    			return null;
    		
    		return info._lookupLang(_lang, _defaultLang);
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 销毁跑马灯数据
     * @param _marqueeDbid
     */
    public void discard(long _marqueeDbid)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alContentList.size(); i++) 
    		{
    			PHPMarqueeContentInfo info = _m_alContentList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getMarqueeDbid() == _marqueeDbid)
    			{
    				_m_alContentList.remove(i);
    				info._discard();
    				return;
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     *  重新载入后台跑马灯内容数据
     * @param _marqueeDbid
     * @param _langContentList
     */
    public void loadLangContent(long _marqueeDbid, ArrayList<ServerObj_PHPMarqueeContent> _langContentList)
    {
    	_lock();
    	
    	try
    	{
    		//载入新数据
    		for(int i = 0; i < _langContentList.size(); i++)
    		{
    			ServerObj_PHPMarqueeContent obj = _langContentList.get(i);
    			if(null == obj)
    				continue;
    			
    			ensureMarquee(_marqueeDbid)._saveContent(obj.getLang(), obj.getParamList(), obj.getContent());
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
