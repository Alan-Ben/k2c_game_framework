package NPUSServer.NPUSUserMgr.UserComp.MarsStep5_ExploreComp;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.MarsEnum.EMarsExplorePVPLogType;
import Common.MarsObj.Mars_ExplorePVPLogIdx;
import NPCommon.CommonLoader.CommonDataLoader;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerTwo;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerMarsExplorePvpLogBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

/**
 * 火星探险PVP日志数据
 * @author mj
 *
 */
public class MarsExplorePVPLogList
{
	//组件对象
    private MarsExploreComponent _m_comp;
    //日志数据加载器
    private CommonDataLoader<List<PlayerMarsExplorePvpLogBO>> _m_dataLoader;
    //锁对象
    private MutexObject _m_mutex;
    
    public MarsExplorePVPLogList(MarsExploreComponent _comp) 
    {
    	_m_comp = _comp;
    
    	_m_dataLoader = new CommonDataLoader<>();
    	
    	_m_mutex = new MutexObject();
    }
    
    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    public MarsExploreComponent getComp() {return _m_comp;}
    public NPUSUserData getUserData() {return _m_comp.getUserData();}

    /**
     * 加载数据
     * @param _handler
     */
    public void ensureLoaded(HandlerOne<Result> _handler)
    {
        _m_dataLoader.loadData(_handler1 -> //加载数据
        {
            getComp().getUSServer().getBM().getBM(PlayerMarsExplorePvpLogBO.class).findAll("cid", getComp().getUserData().getCid(), new _ASelectCallback<List<PlayerMarsExplorePvpLogBO>>()
            {
                @Override
                public void dealSuc(List<PlayerMarsExplorePvpLogBO> _boList)
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
                    _m_dataLoader.getData().sort(Comparator.comparingLong(PlayerMarsExplorePvpLogBO::getCreatedAt));
                    
                	//移除过期数据
                    _removeExpiredLogs();
                }
                
                _handler.handle(result);
            }
        });
    }
    /**
     * 移除过期数据
     */
    private void _removeExpiredLogs()
    {
        _lock();
        
        try
        {
        	//无数据或允许数据范围内不处理
            if (_m_dataLoader.getData() == null
            		|| _m_dataLoader.getData().size() <= RefGeneral.Ref().mars_explore_pvp_record_save_limit)
                return;

            //移除超过100的数据
            while (_m_dataLoader.getData().size() > 0 
            		&& _m_dataLoader.getData().size() > RefGeneral.Ref().mars_explore_pvp_record_save_limit)
            {
            	PlayerMarsExplorePvpLogBO removed = _m_dataLoader.getData().remove(0);
            	if(null == removed)
            		continue;
            	
                removed.del(getComp().getUSServer().getBM());
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 记录：索引数据，详细数据
     */
    public void addStartLog(EMarsExplorePVPLogType _logType, long _createdAt, byte[] _logData)
    {
        //启动加载流程
        ensureLoaded(new HandlerOne<Result>() 
        {
            @Override
            public void handle(Result result)
            {
                if(!result.isSucc())
                    return;

                BM bmObj = getComp().getUSServer().getBM();
                
                PlayerMarsExplorePvpLogBO bo = new PlayerMarsExplorePvpLogBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setLogType(bmObj, _logType.ordinal());
                bo.setCreatedAt(bmObj, _createdAt);
                bo.setLogData(bmObj, _logData);
                bo.insert(bmObj);
                
                _addLogBoToList(bo);
            }
        });
    }
    /**
     * 插入玩家数据
     * @param _bo
     */
    private void _addLogBoToList(PlayerMarsExplorePvpLogBO _bo)
    {
        _lock();
        
        try
        {
            if (null != _m_dataLoader.getData())
            {
                _m_dataLoader.getData().add(_bo);
            	//获取数据后进行排序
                _m_dataLoader.getData().sort(Comparator.comparingLong(PlayerMarsExplorePvpLogBO::getCreatedAt));
                
                //更新最后标记数据
                ALSynTaskManager.getInstance().regTask(()->
                {
                	getComp().getExploreInfo().setPVPLogLastCreated(_bo.getCreatedAt());
                });
                
                //删除过期数据
                _removeExpiredLogs();
            }
        }
        finally
        {
            _unlock();
        }
    }
    
    /**
     * 构造日志
     * @param _handler
     */
    public void makeLogList(HandlerTwo<Result, List<Mars_ExplorePVPLogIdx>> _handler)
    {
        queryLogList(new HandlerTwo<Result, List<PlayerMarsExplorePvpLogBO>>()
        {
            @Override
            public void handle(Result result, List<PlayerMarsExplorePvpLogBO> _boList)
            {
                if (!result.isSucc())
                {
                    _handler.handle(result, null);
                } 
                else
                {
                	List<Mars_ExplorePVPLogIdx> retList = new ArrayList<>();
                	
                    for (int i = 0; i < _boList.size(); i++)
                    {
                    	PlayerMarsExplorePvpLogBO bo = _boList.get(i);
                    	if(null == bo)
                    		continue;
                    	
                        retList.add(toIdxProto(bo));
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
    public void queryLogList(HandlerTwo<Result, List<PlayerMarsExplorePvpLogBO>> _handler)
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
    private List<PlayerMarsExplorePvpLogBO> getBoList()
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
     * 构造数据对象
     * @param _bo
     * @return
     */
    public Mars_ExplorePVPLogIdx toIdxProto(PlayerMarsExplorePvpLogBO _bo)
    {
    	Mars_ExplorePVPLogIdx proto = new Mars_ExplorePVPLogIdx();
    	proto.setId(_bo.getId());
        proto.setLogType(EMarsExplorePVPLogType.EMarsExplorePVPLogType_FromInt(_bo.getLogType()));
    	proto.setCreatedAt(_bo.getCreatedAt());
        if (_bo.getLogData() != null)
            proto.setExData(_bo.getLogData());
        return proto;
    }
    
    /**
     * 获取指定日志数据
     * @param _instanceId
     * @param _handler
     */
    public void getLog(long _id, HandlerTwo<Result, ByteBuffer> _handler)
    {
        queryLogList(new HandlerTwo<Result, List<PlayerMarsExplorePvpLogBO>>()
        {
            @Override
            public void handle(Result result, List<PlayerMarsExplorePvpLogBO> _boList)
            {
                if (!result.isSucc())
                {
                    _handler.handle(result, null);
                } 
                else
                {
                    for (int i = 0; i < _boList.size(); i++)
                    {
                    	PlayerMarsExplorePvpLogBO bo = _boList.get(i);
                    	if(null == bo)
                    		continue;
                    	
                    	if(bo.getId() == _id)
                    	{
                    		if(null == bo.getLogData())
                    		{
                        		_handler.handle(result, null);	
                    		}
                    		else
                    		{
                    			ByteBuffer buff = ByteBuffer.wrap(bo.getLogData());
                        		_handler.handle(result, buff);
                    		}
                    		
                    		return;
                    	}
                    }
                    
                    _handler.handle(Result.failed(CommErr.DATA_LOST.getCode()), null);
                }
            }
        });
    }
}
