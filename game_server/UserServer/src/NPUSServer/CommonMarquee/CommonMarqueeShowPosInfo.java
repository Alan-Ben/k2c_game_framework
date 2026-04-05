package NPUSServer.CommonMarquee;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_MarqueeShowPosInfo;
import Common.Common_MarqueeShowPosReadInfo;
import Common.Common_StringList;
import CommonEnum.EMarqueeCanDelType;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.CommonMarquee.Task.SynTask_OnMarqueeAddBroadcastPlayer;
import NPUSServer.NPUserServer;
import USDB.Bo.CommonMarqueeBO;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.List;

public class CommonMarqueeShowPosInfo
{
    private NPUserServer _m_server;

    private int _m_showPosId;
    private List<CommonMarqueeInfo> _m_marqueeList;
    private MutexAtom _m_listLock;

    public CommonMarqueeShowPosInfo(NPUserServer _server, int _showPosId)
    {
        _m_server = _server;
        
        _m_showPosId = _showPosId;
        
        _m_marqueeList = new ArrayList<>();
        
        _m_listLock = new MutexAtom();
    }

    public NPUserServer getUSServer() {return _m_server;}
    public int getShowPosId() {return _m_showPosId;}

    private void _lock() {_m_listLock.lock();}
    private void _unlock() {_m_listLock.unlock();}
    
    protected void _init(CommonMarqueeInfo _info)
    {
        _addToList(_info);
    }

    /**
     * 添加跑马灯数据
     * @param _phpId
     * @param _refId
     * @param _showPosId
     * @param _createTimeMs
     * @param _expiredTimeMs
     * @param _paramList
     * @param _priorityId
     * @param _durationSec
     * @param _durationCount
     * @param _uiResId
     * @param _content
     * @param _canDelType
     * @param _offlineNeedShow
     * @param _defaultLang
     * @param _channelList
     * @param _showCondition
     * @return
     */
    protected CommonMarqueeInfo _addMarquee(long _phpId, long _refId,
                                            int _showPosId, long _createTimeMs, long _expiredTimeMs, List<String> _paramList,
                                            int _priorityId, int _durationSec, int _durationCount, long _uiResId,
                                            String _content, EMarqueeCanDelType _canDelType, boolean _offlineNeedShow,
                                            String _defaultLang, ArrayList<String> _channelList, String _showCondition)
    {
        BM bmObj = getUSServer().getBM();

        CommonMarqueeBO bo = new CommonMarqueeBO();
        bo.setRefId(bmObj, _refId);
        bo.setPhpId(bmObj, _phpId);
        bo.setShowPosId(bmObj, _showPosId);
        bo.setCreateTimeMs(bmObj, _createTimeMs);
        bo.setExpiredTimeMs(bmObj, _expiredTimeMs);
        
        Common_StringList paramList = new Common_StringList();
        if (_paramList != null)
        {
        	paramList.getValueList().addAll(_paramList);
        }
        bo.setParamList(bmObj, CommonFunc.ByteBfferToBytes(paramList.makePackage()));
        
        bo.setPriorityId(bmObj, _priorityId);
        bo.setDurationSec(bmObj, _durationSec);
        bo.setDurationCount(bmObj, _durationCount);
        bo.setUiResId(bmObj, _uiResId);
        
        if (_content != null)
        {
            bo.setContent(bmObj, _content);
        }
        
        bo.setCanDelType(bmObj, _canDelType.ordinal());
        bo.setOfflineNeedShow(bmObj, _offlineNeedShow);
        
        //默认语言-php后台数据
        if(null != _defaultLang)
        {
        	bo.setDefaultLang(bmObj, _defaultLang);
        }
        //渠道列表-php后台数据
        Common_StringList channelList = new Common_StringList();
        if(null != _channelList)
        {
        	channelList.getValueList().addAll(_channelList);
        }
        bo.setChannelList(bmObj, CommonFunc.ByteBfferToBytes(channelList.makePackage()));
        
        //展示条件
        if(null != _showCondition)
        {
        	bo.setShowCondition(bmObj, _showCondition);
        }
        
        bo.insert(bmObj);

        CommonMarqueeInfo info = new CommonMarqueeInfo(getUSServer(), bo);
        _addToList(info);

        //推送跑马灯新增
        ALSynTaskManager.getInstance().regTask(new SynTask_OnMarqueeAddBroadcastPlayer(getUSServer(), _m_showPosId, info));

        //检查移除过期跑马灯
        _checkDeleteExpiredMarquee();

        //检查删除跑马灯
        _makeSureQueueSizeInRange();
        
        return info;
    }

    /**
     * 加入到队列
     * @param _info 跑马灯数据
     * @return 跑马灯对象
     */
    private void _addToList(CommonMarqueeInfo _info)
    {
        _lock();
        try
        {
            _m_marqueeList.add(_info);
        } finally
        {
            _unlock();
        }
    }

    /**
     * 初始化完成
     */
    protected void onInited()
    {
        _lock();
        try
        {
            //按创建时间排序
            _m_marqueeList.sort(Comparator.comparingLong(CommonMarqueeInfo::getCreateTimeMs));
        } finally
        {
            _unlock();
        }
    }

    /**
     * 检查移除过期跑马灯
     */
    private void _checkDeleteExpiredMarquee()
    {
        _lock();
        try
        {
            long nowTime = CommonFunc.getNowTimeMS();
            for (int i = 0; i < _m_marqueeList.size(); i++)
            {
                CommonMarqueeInfo info = _m_marqueeList.get(i);
                if (info.isExpired(nowTime))
                {
                    _m_marqueeList.remove(i);
                    info.discard();
                    i--;
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 确保队列大小在范围内
     */
    private void _makeSureQueueSizeInRange()
    {
        _lock();
        try
        {
            //当前队列大小
            int curQueueSize = _m_marqueeList.size();
            //如果超过限制
            if (curQueueSize > RefGeneral.Ref().marquee_limit)
            {
                //需要删除的数量
                int deleteCount = curQueueSize - RefGeneral.Ref().marquee_limit;
                //遍历移除
                for (int i = 0; i < deleteCount; i++)
                {
                    CommonMarqueeInfo info = _m_marqueeList.get(0);
                    _m_marqueeList.remove(0);
                    info.discard();
                }
            }
        } finally
        {
            _unlock();
        }
    }

    /**
     * 构造数据
     * @return 协议
     */
    public Common_MarqueeShowPosInfo makeProto(String _lang, Common_MarqueeShowPosReadInfo _readInfo, long _onlineTimeMs)
    {
        //拷贝一份
        List<CommonMarqueeInfo> marqueeList = null;
        _lock();
        try
        {
            marqueeList = new ArrayList<>(_m_marqueeList);
        } finally
        {
            _unlock();
        }

        long nowTimeMS = CommonFunc.getNowTimeMS();
        //生成协议
        Common_MarqueeShowPosInfo posInfo = new Common_MarqueeShowPosInfo();
        posInfo.setShowPosId(_m_showPosId);
        for (CommonMarqueeInfo info : marqueeList)
        {
            //如果玩家已读则跳过
            if (_readInfo != null && _readInfo.getHadReadDbId() >= info.getDbId())
                continue;

            //离线不显示的不发送
            if (!info.getOfflineNeedShow() && info.getCreateTimeMs() < _onlineTimeMs)
                continue;

            //过期的不发送
            if (info.isExpired(nowTimeMS))
                continue;

            posInfo.getMarqueeList().add(info.makeProto(_lang));
        }
        return posInfo;
    }
    
    /**
     * 移除跑马灯
     * @param _dbid
     */
    public void delMarquee(long _dbid) 
    {
		_lock();
		
		try
		{
			for (int i = 0; i < _m_marqueeList.size(); i++)
            {
                CommonMarqueeInfo info = _m_marqueeList.get(i);
                if(null == info)
                	continue;
                
                if(info.getDbId() == _dbid)
                {
                	_m_marqueeList.remove(i);
                	info.discard();
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
