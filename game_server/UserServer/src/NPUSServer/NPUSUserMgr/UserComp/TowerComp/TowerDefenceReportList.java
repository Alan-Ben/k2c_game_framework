package NPUSServer.NPUSUserMgr.UserComp.TowerComp;

import ALBasicServer.ALBasicMutex.MutexObject;
import Common.TowerObj.Tower_PosInfo;
import Common.TowerObj.Tower_ReportInfo;
import NPCommon.CommonLoader.CommonDataLoader;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerTwo;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerTowerDefenceReportBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

/**
 * 防守战报数据
 * @author mj
 *
 */
public class TowerDefenceReportList
{
	//组件对象
    private TowerComponent _m_comp;
    //数据加载器
    private CommonDataLoader<List<PlayerTowerDefenceReportBO>> _m_dataLoader;
    //锁对象
    private MutexObject _m_mutex;
    
    public TowerDefenceReportList(TowerComponent _comp) 
    {
    	_m_comp = _comp;
    
    	_m_dataLoader = new CommonDataLoader<>();
    	
    	_m_mutex = new MutexObject();
    }
    
    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    public TowerComponent getComp() {return _m_comp;}
    public NPUSUserData getUserData() {return _m_comp.getUserData();}

    /**
     * 加载数据
     * @param _handler
     */
    public void ensureLoaded(HandlerOne<Result> _handler)
    {
        _m_dataLoader.loadData(_handler1 -> //加载数据
        {
            getComp().getUSServer().getBM().getBM(PlayerTowerDefenceReportBO.class).findAll("cid", getComp().getUserData().getCid(), 
            		new _ASelectCallback<List<PlayerTowerDefenceReportBO>>()
            {
                @Override
                public void dealSuc(List<PlayerTowerDefenceReportBO> _boList)
                {
                    //成功回调
                    _handler1.handle(Result.SUCC, _boList);
                }

                @Override
                public void dealFail()
                {
                    _handler1.handle(CommErr.SYS_ERR, null);
                }
            });
        }, 
        new HandlerOne<Result>() //加载完成后的处理
        {
            @Override
            public void handle(Result result)
            {
                if(result.isSucc())
                {
                	//获取数据后进行排序
                    _m_dataLoader.getData().sort(Comparator.comparingLong(PlayerTowerDefenceReportBO::getId));
                    
                	//移除过期数据
                    _removeExpiredData();
                }
                
                _handler.handle(result);
            }
        });
    }
    /**
     * 移除过期数据
     */
    private void _removeExpiredData()
    {
        _lock();
        
        try
        {
        	//无数据
        	if(null == _m_dataLoader.getData())
        		return;
        	
            //移除超过上限数量的数据
            while (_m_dataLoader.getData().size() > RefGeneral.Ref().tower_defence_report_limit)
            {
            	PlayerTowerDefenceReportBO removed = _m_dataLoader.getData().remove(_m_dataLoader.getData().size() - 1);
                removed.del(getComp().getUSServer().getBM());
            }
            
            //移除超过上限天数的数据
            ArrayList<PlayerTowerDefenceReportBO> removeList = new ArrayList<>();
            long expiredMs = CommonFunc.getNowTimeMS() - RefGeneral.Ref().tower_defence_report_time_limit * 86400000;
            for(int i = 0; i < _m_dataLoader.getData().size(); i++)
            {
            	PlayerTowerDefenceReportBO bo = _m_dataLoader.getData().get(i);
            	if(null == bo)
            		continue;
            	
            	if(bo.getTimestamp() < expiredMs)
            	{
            		removeList.add(bo);
            	}
            }
            for(int i = 0; i < removeList.size(); i++)
            {
            	PlayerTowerDefenceReportBO bo = removeList.get(i);
            	
            	_m_dataLoader.getData().remove(bo);
            	bo.del(getComp().getUSServer().getBM());
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 构造宴会开启日志
     * @param _handler
     */
    public void makeDataList(HandlerTwo<Result, List<Tower_ReportInfo>> _handler)
    {
    	queryDataList(new HandlerTwo<Result, List<PlayerTowerDefenceReportBO>>()
        {
            @Override
            public void handle(Result result, List<PlayerTowerDefenceReportBO> _boList)
            {
                if (!result.isSucc())
                {
                    _handler.handle(result, null);
                } 
                else
                {
                	List<Tower_ReportInfo> retList = new ArrayList<>();
                	
                    for (int i = 0; i < _boList.size(); i++)
                    {
                    	PlayerTowerDefenceReportBO bo = _boList.get(i);
                    	if(null == bo)
                    		continue;
                    	
                    	//构造数据
                    	Tower_ReportInfo info = new Tower_ReportInfo();
                    	
                    	Tower_PosInfo posInfo = new Tower_PosInfo();
                    	posInfo.setChapterId(bo.getChapterId());
                    	posInfo.setLevel(bo.getChapterLevel());
                    	info.setPosInfo(posInfo);
                    	
                    	info.setAttackerCid(bo.getAttackerCid());
                    	info.setIsSucc(bo.getIsSucc());
                    	info.setDownLevel(bo.getCurChapterLevel() - bo.getChapterLevel());
                    	info.setTimestamp(bo.getTimestamp());
                    	
                        retList.add(info);
                    }
                    
                    _handler.handle(Result.SUCC, retList);
                }
            }
        });
    }
    
    /**
     * 查询日志数据
     * @param _handler
     */
    public void queryDataList(HandlerTwo<Result, List<PlayerTowerDefenceReportBO>> _handler)
    {
        ensureLoaded(new HandlerOne<Result>() 
        {
            @Override
            public void handle(Result result)
            {
                if(!result.isSucc())
                {
                    _handler.handle(result, null);
                    return;
                }
                
                _handler.handle(Result.SUCC, getBoList());
            }
        });
    }
    
    /**
     * 构造Bo列表，用于查询后的结果对象使用
     * @return
     */
    private List<PlayerTowerDefenceReportBO> getBoList()
    {
        _lock();
        
        try
        {
            if (_m_dataLoader.getData() == null)
            {
                return new ArrayList<>();
            }
            else
            {
            	return new ArrayList<>(_m_dataLoader.getData());
            }
        }
        finally
        {
            _unlock();
        }
    }
    
    /**
     * 加载数据（外部调用）
     * @param _bo
     */
    public void addData(PlayerTowerDefenceReportBO _bo)
    {
        ensureLoaded(new HandlerOne<Result>() 
        {
            @Override
            public void handle(Result result)
            {
                if(!result.isSucc())
                {
                    return;
                }
                
                _addData(_bo);
            }
        });
    }
    /**
     * 加载数据
     * @param _bo
     */
    private void _addData(PlayerTowerDefenceReportBO _bo)
    {
    	_lock();
    	
    	try
    	{
    		//尚未初始化完成
    		if(null == _m_dataLoader.getData())
    			return;
    		
    		//检查是否已经加载
    		for(int i = 0; i < _m_dataLoader.getData().size(); i++)
    		{
    			PlayerTowerDefenceReportBO bo = _m_dataLoader.getData().get(i);
    			if(null == bo)
    				continue;
    			
    			if(bo.getId() == _bo.getId())
    				return;
    		}
    		
    		//加载新数据
    		_m_dataLoader.getData().add(_bo);
        	//获取数据后进行排序
            _m_dataLoader.getData().sort(Comparator.comparingLong(PlayerTowerDefenceReportBO::getId));
            
            _removeExpiredData();
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
