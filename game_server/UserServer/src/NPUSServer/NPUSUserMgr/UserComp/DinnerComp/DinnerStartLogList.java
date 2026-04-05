package NPUSServer.NPUSUserMgr.UserComp.DinnerComp;

import ALBasicServer.ALBasicMutex.MutexObject;
import Common.DinnerObj.Dinner_StartLogIdx;
import Common.DinnerObj.Dinner_StartLogInfo;
import NPCommon.CommonLoader.CommonDataLoader;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerOne;
import NPCommon.Util.Delegate.HandlerTwo;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import USDB.Bo.PlayerDinnerStartLogBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

/**
 * 开始宴会日志数据
 * @author mj
 *
 */
public class DinnerStartLogList
{
	//宴会组件对象
    private DinnerComponent _m_comp;
    //开宴日志数据加载器
    private CommonDataLoader<List<PlayerDinnerStartLogBO>> _m_dataLoader;
    //锁对象
    private MutexObject _m_mutex;
    
    public DinnerStartLogList(DinnerComponent _comp) 
    {
    	_m_comp = _comp;
    
    	_m_dataLoader = new CommonDataLoader<>();
    	
    	_m_mutex = new MutexObject();
    }
    
    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}

    public DinnerComponent getComp() {return _m_comp;}
    public NPUSUserData getUserData() {return _m_comp.getUserData();}

    /**
     * 加载数据
     * @param _handler
     */
    public void ensureLoaded(HandlerOne<Result> _handler)
    {
        _m_dataLoader.loadData(_handler1 -> //加载数据
        {
            getComp().getUSServer().getBM().getBM(PlayerDinnerStartLogBO.class).findAll("cid", getComp().getUserData().getCid(), new _ASelectCallback<List<PlayerDinnerStartLogBO>>()
            {
                @Override
                public void dealSuc(List<PlayerDinnerStartLogBO> _boList)
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
                    _m_dataLoader.getData().sort(Comparator.comparingInt(PlayerDinnerStartLogBO::getStartTs));
                    
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
            		|| _m_dataLoader.getData().size() <= RefGeneral.Ref().dinner_open_record_save_limit)
                return;

            //移除超过100的数据
            while (_m_dataLoader.getData().size() > RefGeneral.Ref().dinner_open_record_save_limit)
            {
                PlayerDinnerStartLogBO removed = _m_dataLoader.getData().remove(_m_dataLoader.getData().size() - 1);
                removed.del(getComp().getUSServer().getBM());
            }
        }
        finally
        {
            _unlock();
        }
    }

    /**
     * 开宴记录：索引数据，详细数据
     * @param _idx
     * @param _info
     */
    public void addStartLog(Dinner_StartLogIdx _idx, Dinner_StartLogInfo _info)
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
                
            	PlayerDinnerStartLogBO bo = new PlayerDinnerStartLogBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setInstanceId(bmObj, _idx.getInstanceId());
                bo.setLogIdx(bmObj, CommonFunc.ByteBfferToBytes(_idx.makePackage()));
                bo.setLogInfo(bmObj, CommonFunc.ByteBfferToBytes(_info.makePackage()));
                bo.setStartTs(bmObj, _idx.getStartTs());
                bo.insert(bmObj);
                
                _addLogBoToList(bo);
            }
        });
    }
    /**
     * 插入玩家数据
     * @param _bo
     */
    private void _addLogBoToList(PlayerDinnerStartLogBO _bo)
    {
        _lock();
        
        try
        {
            if (null != _m_dataLoader.getData())
            {
                _m_dataLoader.getData().add(_bo);
            	//获取数据后进行排序
                _m_dataLoader.getData().sort(Comparator.comparingInt(PlayerDinnerStartLogBO::getStartTs));
                
                _removeExpiredLogs();
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
    public void makeStartLogList(HandlerTwo<Result, List<Dinner_StartLogIdx>> _handler)
    {
        queryLogList(new HandlerTwo<Result, List<PlayerDinnerStartLogBO>>()
        {
            @Override
            public void handle(Result result, List<PlayerDinnerStartLogBO> _boList)
            {
                if (!result.isSucc())
                {
                    _handler.handle(result, null);
                } 
                else
                {
                	List<Dinner_StartLogIdx> retList = new ArrayList<>();
                	
                    for (int i = 0; i < _boList.size(); i++)
                    {
                    	PlayerDinnerStartLogBO bo = _boList.get(i);
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
    public void queryLogList(HandlerTwo<Result, List<PlayerDinnerStartLogBO>> _handler)
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
    private List<PlayerDinnerStartLogBO> getBoList()
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
    public Dinner_StartLogIdx toIdxProto(PlayerDinnerStartLogBO _bo)
    {
    	Dinner_StartLogIdx proto = new Dinner_StartLogIdx();
        
    	ByteBuffer buff = ByteBuffer.wrap(_bo.getLogIdx());
    	proto.readPackage(buff);

        return proto;
    }
    
    /**
     * 获取指定开宴日志数据
     * @param _instanceId
     * @param _handler
     */
    public void getLog(long _instanceId, HandlerTwo<Result, Dinner_StartLogInfo> _handler)
    {
        queryLogList(new HandlerTwo<Result, List<PlayerDinnerStartLogBO>>()
        {
            @Override
            public void handle(Result result, List<PlayerDinnerStartLogBO> _boList)
            {
                if (!result.isSucc())
                {
                    _handler.handle(result, null);
                } 
                else
                {
                    for (int i = 0; i < _boList.size(); i++)
                    {
                    	PlayerDinnerStartLogBO bo = _boList.get(i);
                    	if(null == bo)
                    		continue;
                    	
                    	if(bo.getInstanceId() == _instanceId)
                    	{
                    		Dinner_StartLogInfo info = new Dinner_StartLogInfo();  		
                    		ByteBuffer buff = ByteBuffer.wrap(bo.getLogInfo());
                    		info.readPackage(buff);
                    		
                    		_handler.handle(result, info);
                    		return;
                    	}
                    }
                    
                    _handler.handle(Result.failed(CommErr.DATA_LOST.getCode()), null);
                }
            }
        });
    
    }
}
