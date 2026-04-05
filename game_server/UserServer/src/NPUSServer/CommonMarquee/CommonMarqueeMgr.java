package NPUSServer.CommonMarquee;

import ALBasicServer.ALBasicMutex.MutexObject;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_MarqueeShowPosInfo;
import Common.Common_MarqueeShowPosReadInfo;
import Common.ServerObj.ServerObj_PHPMarquee;
import CommonEnum.EMarqueeCanDelType;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.Util.CommonFunc;
import NPEnum.EMarqueeOfflineNeedShowType;
import NPGameRes.Refs.Marquee.RefMarquee;
import NPUSServer.CommonMarquee.Task.SynTask_OnMarqueeDelBroadcastPlayer;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.CommonMarqueeBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/**
 * 系统跑马灯管理器 单例
 * 分id管理跑马灯队列SystemMarqueeList
 * 可以通过id获取跑马灯队列
 * 可以通过id添加跑马灯
 */
public class CommonMarqueeMgr
{
    private NPUserServer _m_server;

    //根据POS进行区分的跑马灯数据
    private Map<Integer, CommonMarqueeShowPosInfo> _m_marqueeShowPosMap = new HashMap<>();
    //平台跑马灯数据，key-跑马灯实例ID
    private ArrayList<CommonMarqueeInfo> _m_alPhpMarqueeList = new ArrayList<>();
    
    private MutexObject _m_mapLock = new MutexObject();

    public CommonMarqueeMgr(NPUserServer _server)
    {
        _m_server = _server;
    }

    /**
     * 加锁
     */
    private void _lock()
    {
        _m_mapLock.lock();
    }

    /**
     * 解锁
     */
    private void _unlock()
    {
        _m_mapLock.unlock();
    }

    public NPUserServer getUSServer()
    {
        return _m_server;
    }

    /**
     * 初始化
     * @return 是否初始化成功
     */
    public boolean sInit()
    {
        List<CommonMarqueeBO> boList = getUSServer().getBM().getBM(CommonMarqueeBO.class).s_findAll();
        if (boList == null)
            return false;

        for (CommonMarqueeBO bo : boList)
        {
        	CommonMarqueeInfo info = new CommonMarqueeInfo(getUSServer(), bo);
        	
            CommonMarqueeShowPosInfo marqueeList = ensureMarqueePosList(bo.getShowPosId());
            marqueeList._init(info);
            
            if(info.getPhpId() > 0)
            {
            	_m_alPhpMarqueeList.add(info);
            }
        }

        //遍历调用初始化完成接口
        for (Map.Entry<Integer, CommonMarqueeShowPosInfo> entry : _m_marqueeShowPosMap.entrySet())
        {
            entry.getValue().onInited();
        }

        return true;
    }

    /**
     * 查找跑马灯队列
     * @param _showPosId 跑马灯队列id
     * @return 跑马灯队列
     */
    public CommonMarqueeShowPosInfo lookupMarqueePosList(int _showPosId)
    {
        _lock();
        try
        {
            return _m_marqueeShowPosMap.get(_showPosId);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保跑马灯队列存在
     * @param _showPosId 跑马灯队列id
     * @return 跑马灯队列
     */
    public CommonMarqueeShowPosInfo ensureMarqueePosList(int _showPosId)
    {
        _lock();
        try
        {
            CommonMarqueeShowPosInfo marqueeList = lookupMarqueePosList(_showPosId);
            if (marqueeList == null)
            {
                marqueeList = new CommonMarqueeShowPosInfo(getUSServer(), _showPosId);
                _m_marqueeShowPosMap.put(_showPosId, marqueeList);
            }
            return marqueeList;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 生成协议
     * @return 协议
     */
    public List<Common_MarqueeShowPosInfo> makeProto(String _lang, ArrayList<Common_MarqueeShowPosReadInfo> _marqueeReadList, long _onlineTimeMs)
    {
        //拷贝一份
        List<CommonMarqueeShowPosInfo> posList;
        _lock();
        try
        {
            posList = new ArrayList<>(_m_marqueeShowPosMap.values());
        } finally
        {
            _unlock();
        }

        //遍历收集数据
        List<Common_MarqueeShowPosInfo> posInfoList = new ArrayList<>();
        for (CommonMarqueeShowPosInfo posInfo : posList)
        {
            Common_MarqueeShowPosReadInfo readInfo = _lookupPosInfoInReadList(posInfo.getShowPosId(), _marqueeReadList);
            if (readInfo == null)
            {
                posInfoList.add(posInfo.makeProto(_lang, null, _onlineTimeMs));
            } else
            {
                posInfoList.add(posInfo.makeProto(_lang, readInfo, _onlineTimeMs));
            }
        }
        return posInfoList;
    }

    private Common_MarqueeShowPosReadInfo _lookupPosInfoInReadList(int showPosId, ArrayList<Common_MarqueeShowPosReadInfo> _marqueeReadList)
    {
        for (Common_MarqueeShowPosReadInfo readInfo : _marqueeReadList)
        {
            if (showPosId == readInfo.getShowPosId())
            {
                return readInfo;
            }
        }
        return null;
    }

    /**
     * 增加跑马灯
     * @param _phpId
     * @param _refId
     * @param _paramList
     * @param _showPosId
     * @param _priorityId
     * @param _durationSec
     * @param _durationCount
     * @param _uiResId
     * @param _content
     * @param _canDelType
     * @param _lifeSec
     * @param _offlineNeedShowType
     * @param _defaultLang
     * @param _channelList
     * @param _showCondition
     * @return
     */
    public ResultOne<CommonMarqueeInfo> cmdAddMarquee(long _phpId, long _refId, List<String> _paramList, 
    		int _showPosId, int _priorityId, int _durationSec, int _durationCount,
            long _uiResId, String _content, EMarqueeCanDelType _canDelType, int _lifeSec, 
            EMarqueeOfflineNeedShowType _offlineNeedShowType
            , String _defaultLang, ArrayList<String> _channelList, String _showCondition)
    {
    	//部分需要提前定义的跑马灯参数
    	int showPosId = -1;
		int priorityId = -1;
    	int durationSec = 0;
        int durationCount = 0;
        long uiResId = 0;
    	long nowTimeMS = CommonFunc.getNowTimeMS();
    	long expiredTimeMs = -1;
    	boolean offlineNeedShow = false;
    	
    	//如果传递配置ID，需要检查对应配置，然后根据实际传递的值确认是否使用配置数据
    	RefMarquee refMarquee = null;
    	if(_refId > 0)
    	{
    		refMarquee = RefMarquee.getMgr().get(_refId);
            if (refMarquee == null)
            {
            	USLog.error(getUSServer(), "Add Marquee Fail, not find ref:{}.", _refId);
            	return ResultOne.failed(CommErr.REF_NOT_FOUND);
            }
            
            //================= 检查跑马灯数据，如果为0/未确认，则使用配置数据 ================= //
            //窗口展示队列
            if(_showPosId == 0)
            {
            	showPosId = refMarquee.show_pos_id;
            }
            //优先级（越大越优先）
            if(_priorityId == 0)
            {
            	priorityId = refMarquee.priority_id;
            }
            //循环播放时长秒（优先于次数）
            if(_durationSec == 0)
            {
            	durationSec = refMarquee.duration_sec;
            }
            //循环播放次数
            if(_durationCount == 0)
            {
            	durationCount = refMarquee.duration_count;
            }
            //生存时间秒，-1表示即时过期
            if(_lifeSec == 0)
            {
            	_lifeSec = refMarquee.life_sec;
            	expiredTimeMs = refMarquee.life_sec == -1 ? -1 : (nowTimeMS + refMarquee.life_sec * 1000L);
            }
            //预制体ID
            if(_uiResId == 0)
            {
            	uiResId = refMarquee.ui_res_id;
            }
            //玩家离线期间是否需要展示
            offlineNeedShow = refMarquee.offline_need_show;
    	}
    	
    	//================= 检查跑马灯数据，如果外部有传入，则使用外部数据 ================= //
    	//窗口展示队列
        if(_showPosId > 0)
        {
        	showPosId = _showPosId;
        }
        //优先级（越大越优先）
        if(_priorityId > 0)
        {
        	priorityId = _priorityId;
        }
        //循环播放时长秒（优先于次数）
        if(_durationSec > 0)
        {
        	durationSec = _durationSec;
        }
        //循环播放次数
        if(_durationCount > 0)
        {
        	durationCount = _durationCount;
        }
        //玩家离线期间是否需要展示
        if(EMarqueeOfflineNeedShowType.TRUE == _offlineNeedShowType)
        {
        	offlineNeedShow = true;
        }
        else if(EMarqueeOfflineNeedShowType.FALSE == _offlineNeedShowType)
        {
        	offlineNeedShow = false;
        }
        //生存时间秒，-1表示即时过期
    	if (_lifeSec != 0)
        {
        	expiredTimeMs = _lifeSec == -1 ? -1 : nowTimeMS + _lifeSec * 1000L;
        }
        //预制体ID
        if(_uiResId > 0)
        {
        	uiResId = _uiResId;
        }

    	//================= 检查跑马灯数据 ================= //
    	if(showPosId <= 0)
    	{
        	USLog.error(getUSServer(), "Add Marquee Fail, param[showPosId] error.");
        	return ResultOne.failed(CommErr.PARAM_ERROR);
        }
    	if(durationCount <= 0 && durationSec <= 0)
    	{
        	USLog.error(getUSServer(), "Add Marquee Fail, param[durationCount, durationSec] error.");
        	return ResultOne.failed(CommErr.PARAM_ERROR);
        }
    	if(uiResId <= 0)
    	{
        	USLog.error(getUSServer(), "Add Marquee Fail, param[uiResId] error.");
        	return ResultOne.failed(CommErr.PARAM_ERROR);
        }

    	//================= 生成跑马灯数据 ================= //
        CommonMarqueeShowPosInfo marqueeList = ensureMarqueePosList(showPosId);

        CommonMarqueeInfo marquee = marqueeList._addMarquee(_phpId, _refId, showPosId, nowTimeMS, expiredTimeMs, _paramList, priorityId
        		, durationSec, durationCount, uiResId, _content, _canDelType, offlineNeedShow, _defaultLang, _channelList, _showCondition);

        if(marquee.getPhpId() > 0)
        {
            _m_alPhpMarqueeList.add(marquee);
        }
        
    	return ResultOne.succ(marquee);
    }
    
    /**
     * 增加跑马灯，使用配置数据，需要补充参数
     * @param _ref
     * @param _paramList
     * @return
     */
    public ResultOne<CommonMarqueeInfo> cmdAddMarquee(RefMarquee _ref, List<String> _paramList)
    {
    	return cmdAddMarquee(0, _ref.id, _paramList, 0, 0, 0, 0, 0, null,
                EMarqueeCanDelType.READ_REF, 0, EMarqueeOfflineNeedShowType.READ_REF, null, null, null);
    }
    public ResultOne<CommonMarqueeInfo> cmdAddMarquee(long _refId, List<String> _paramList)
    {
    	RefMarquee ref = RefMarquee.getMgr().get(_refId);
    	if(null == ref)
    	{
    		USLog.error(getUSServer(), "Add Marquee Fail, not find ref:{}.", _refId);
        	return ResultOne.failed(CommErr.REF_NOT_FOUND);
    	}
    	
    	return cmdAddMarquee(ref, _paramList);
    }
    
    /**
     * 增加运营性质跑马灯（参数与内容放在 PHPMarqueeContentMgr 管理器内）
     * @param _phpMarquee
     * @return
     */
    public ResultOne<CommonMarqueeInfo> cmdAddPHPMarquee(ServerObj_PHPMarquee _phpMarquee)
    {
    	//对跑马灯进行检查，同一个后台ID的跑马灯不允许重复创建
    	_lock();
    	try
    	{
    		CommonMarqueeInfo marquee  = lookupPHPMarqueeByPHPId(_phpMarquee.getPhpId());
    		if(null != marquee)
    			return ResultOne.failed(CommErr.OP_DISABLE);
    	}
    	finally 
    	{
			_unlock();
		}
    	
    	//创建跑马灯数据
    	ResultOne<CommonMarqueeInfo> result = cmdAddMarquee(_phpMarquee.getPhpId(), _phpMarquee.getRefId()
                , null, _phpMarquee.getShowPosId(), _phpMarquee.getPriorityId()
    			, _phpMarquee.getDurationSec(), _phpMarquee.getDurationCount(), _phpMarquee.getUiResId(), null
    			, _phpMarquee.getCanDelType(), _phpMarquee.getLifeSec(), _phpMarquee.getOfflineNeedShowType()
    			, _phpMarquee.getDefaultLang(), _phpMarquee.getChannelList(), _phpMarquee.getShowCondition());
    	
    	//成功创建跑马灯数据，同步创建后台多语言数据
    	if(result.isSucc())
    	{
    		getUSServer().getPHPMarqueeContentMgr().loadLangContent(result.getData().getDbId(), _phpMarquee.getMarqueeContentList());
    	}
    	
    	return result;
    }
    /**
     * 移除平台跑马灯数据
     * @param _phpId
     */
    public void cmdDelPHPMarquee(long _phpId)
    {
    	CommonMarqueeInfo marquee = null;
    	
    	_lock();
    	
    	try
    	{
    		marquee  = lookupPHPMarqueeByPHPId(_phpId);
    	}
    	finally 
    	{
			_unlock();
		}
    	
    	if(null != marquee)
    	{
    		ensureMarqueePosList(marquee.getShowPos()).delMarquee(marquee.getDbId());

            //同步所有玩家数据移除
            ALSynTaskManager.getInstance().regTask(new SynTask_OnMarqueeDelBroadcastPlayer(_m_server, marquee.getDbId()));
    	}
    }
    
    /**
     * 增加跑马灯，使用配置数据，需要补充参数，对特殊字符进行解析
     * @param _refId
     * @param _paramList
     * @return
     */
    public ResultOne<CommonMarqueeInfo> cmdAddMarqueeObj(long _refId, List<Object> _paramList)
    {
    	RefMarquee ref = RefMarquee.getMgr().get(_refId);
    	if(null == ref)
    	{
    		USLog.error(getUSServer(), "Add Marquee Fail, not find ref[].", _refId);
        	return ResultOne.failed(CommErr.REF_NOT_FOUND);
    	}
    	
    	//按照传入的对象类型不同解析
    	List<String> paramList = new ArrayList<>();
        for (Object rawObj : _paramList)
        {
            if (rawObj instanceof NPCommonCostItem)
            {
                NPCommonCostItem obj = (NPCommonCostItem) rawObj;
                paramList.add("##COMMON-ITEM##" + obj.getItemType() + "-" + obj.getItemId());
            } 
            else
            {
                paramList.add(String.valueOf(rawObj));
            }
        }
    	
    	return cmdAddMarquee(ref, paramList);
    }
    
    /**
     * 增加跑马灯，使用配置数据，需要补充内容
     * @param _ref
     * @param _content
     * @return
     */
    public ResultOne<CommonMarqueeInfo> cmdAddMarquee(RefMarquee _ref, String _content)
    {
    	return cmdAddMarquee(0, _ref.id, null, 0, 0, 0, 0, 0, _content,
                EMarqueeCanDelType.READ_REF, 0, EMarqueeOfflineNeedShowType.READ_REF, null, null, null);
    }
    
    /**
     * 获取PHP跑马灯数据
     * @param _phpId
     * @return
     */
    public CommonMarqueeInfo lookupPHPMarquee(long _marqueeDbid)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alPhpMarqueeList.size(); i++)
    		{
    			CommonMarqueeInfo info = _m_alPhpMarqueeList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getDbId() == _marqueeDbid)
    				return info;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    public CommonMarqueeInfo lookupPHPMarqueeByPHPId(long _phpId)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alPhpMarqueeList.size(); i++)
    		{
    			CommonMarqueeInfo info = _m_alPhpMarqueeList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getPhpId() == _phpId)
    				return info;
    		}
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    /**
     * 外部删除跑马灯时，需要同步删除PHP跑马灯的信息（该接口不对外开放）
     * @param _dbid
     */
    protected void removePHPMarquee(long _dbid)
    {
    	_lock();
    	
    	try
    	{
    		for(int i = 0; i < _m_alPhpMarqueeList.size(); i++)
    		{
    			CommonMarqueeInfo info = _m_alPhpMarqueeList.get(i);
    			if(null == info)
    				continue;
    			
    			if(info.getDbId() == _dbid)
    			{
    				_m_alPhpMarqueeList.remove(i);
    				
    		    	//移除对应的内容数据
    		    	getUSServer().getPHPMarqueeContentMgr().discard(_dbid);
    		    	
    				return;
    			}
    		}
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
