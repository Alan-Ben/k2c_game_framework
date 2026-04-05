package NPUSServer.CommonMarquee;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.Common_MarqueeInfo;
import Common.Common_StringList;
import CommonEnum.EMarqueeCanDelType;
import NPUSServer.NPUserServer;
import USDB.Bo.CommonMarqueeBO;

import java.nio.ByteBuffer;
import java.util.ArrayList;
import java.util.List;

public class CommonMarqueeInfo
{
    private NPUserServer _m_server;
    
    private long _m_dbId;
    private long _m_refId;
    private long _m_phpId;
    private int _m_iShowPos;
    private long _m_createTimeMs;
    private long _m_expiredTimeMs;
    private List<String> _m_paramList;
    private int _m_priorityId;
    private int _m_durationSec;
    private int _m_durationCount;
    private long _m_uiResId;
    private String _m_content;
    private EMarqueeCanDelType _m_canDelType;
    private boolean _m_offlineNeedShow;
    //默认语言-PHP后台数据
    private String _m_sDefaultLang;
    //渠道列表-PHP后台数据
    private ArrayList<String> _m_channelList;
    //展示条件（客户端使用）
    private String _m_sShowCondition;

    public CommonMarqueeInfo(NPUserServer _server, CommonMarqueeBO _bo)
    {
        _m_server = _server;
        _m_dbId = _bo.getId();
        _m_refId = _bo.getRefId();
        _m_phpId = _bo.getPhpId();
        _m_iShowPos = _bo.getShowPosId();
        _m_createTimeMs = _bo.getCreateTimeMs();
        _m_expiredTimeMs = _bo.getExpiredTimeMs();
        if(null != _bo.getParamList())
        {
            Common_StringList list = new Common_StringList();
            list.readPackage(ByteBuffer.wrap(_bo.getParamList()));
            _m_paramList = list.getValueList();
        }
        _m_priorityId = _bo.getPriorityId();
        _m_durationSec = _bo.getDurationSec();
        _m_durationCount = _bo.getDurationCount();
        _m_uiResId = _bo.getUiResId();
        _m_content = _bo.getContent();
        _m_canDelType = EMarqueeCanDelType.EMarqueeCanDelType_FromInt(_bo.getCanDelType());
        _m_offlineNeedShow = _bo.getOfflineNeedShow();
        _m_sDefaultLang = _bo.getDefaultLang();
        if(null != _bo.getChannelList())
        {
            Common_StringList list = new Common_StringList();
            list.readPackage(ByteBuffer.wrap(_bo.getChannelList()));
            _m_channelList = list.getValueList();
        }
        _m_sShowCondition = _bo.getShowCondition();
    }
    public CommonMarqueeInfo(NPUserServer _server, CommonMarqueeBO _bo, Common_StringList _paramListObj, Common_StringList _channelListObj)
    {
        _m_server = _server;
        _m_dbId = _bo.getId();
        _m_refId = _bo.getRefId();
        _m_phpId = _bo.getPhpId();
        _m_createTimeMs = _bo.getCreateTimeMs();
        _m_expiredTimeMs = _bo.getExpiredTimeMs();
        _m_paramList = _paramListObj.getValueList();
        _m_priorityId = _bo.getPriorityId();
        _m_durationSec = _bo.getDurationSec();
        _m_durationCount = _bo.getDurationCount();
        _m_uiResId = _bo.getUiResId();
        _m_content = _bo.getContent();
        _m_canDelType = EMarqueeCanDelType.EMarqueeCanDelType_FromInt(_bo.getCanDelType());
        _m_offlineNeedShow = _bo.getOfflineNeedShow();
        _m_sDefaultLang = _bo.getDefaultLang();
        _m_channelList = _channelListObj.getValueList();
        _m_sShowCondition = _bo.getShowCondition();
    }

    public NPUserServer getUSServer() {return _m_server;}
    
    public long getDbId() {return _m_dbId;}
    public long getRefId() {return _m_refId;}
    public long getPhpId() {return _m_phpId;}
    public int getShowPos() {return _m_iShowPos;}
    public long getCreateTimeMs() {return _m_createTimeMs;}
    public boolean getOfflineNeedShow() {return _m_offlineNeedShow;}
    
    /**
     * 销毁数据
     */
    public void discard()
    {
        getUSServer().getBM().getBM(CommonMarqueeBO.class).delAll("id", _m_dbId);
        
        //检查移除PHP后台跑马灯数据
        if(_m_phpId > 0)
        {
        	ALSynTaskManager.getInstance().regTask(()->
        	{
        		getUSServer().getMarqueeMgr().removePHPMarquee(_m_dbId);
        	});
        }
    }

    /**
     * 是否已经过期
     * @param _nowTimeMs 当前时间
     * @return 是否已经过期
     */
    public boolean isExpired(long _nowTimeMs)
    {
        return _m_expiredTimeMs != -1 && _nowTimeMs >= _m_expiredTimeMs;
    }

    /**
     * 生成协议
     * @return 协议
     */
    public Common_MarqueeInfo makeProto(String _lang)
    {
        Common_MarqueeInfo info = new Common_MarqueeInfo();
        info.setDbId(_m_dbId);
        info.setRefId(_m_refId);
        info.setExpiredTimeMs(_m_expiredTimeMs);
        info.getParamList().addAll(_m_paramList);
        info.setPriorityId(_m_priorityId);
        info.setDurationCount(_m_durationCount);
        info.setDurationSec(_m_durationSec);
        info.setUiResId(_m_uiResId);
        info.setContent(_m_content);
        info.setCanDelType(_m_canDelType);
        info.getChannelList().addAll(_m_channelList);
        info.setShowCondition(_m_sShowCondition);
        
        _fillPHPContent(_lang, info);
        
        return info;
    }
    /**
     * 针对运营邮件的内容处理
     * @param _lang
     * @param _info
     */
    private void _fillPHPContent(String _lang, Common_MarqueeInfo _info)
    {
    	if(_m_phpId <= 0)
    		return;
    	
    	PHPMarqueeContentLangInfo phpContent = _m_server.getPHPMarqueeContentMgr().lookupLang(_m_dbId, _lang, _m_sDefaultLang);
    	if(null != phpContent)
    	{
    		if(!phpContent.getParamList().isEmpty())
    		{
    			_info.getParamList().clear();
        		_info.getParamList().addAll(phpContent.getParamList());
    		}
    		
    		if(!phpContent.getContent().isEmpty())
    		{
    			_info.setContent(phpContent.getContent());
    		}
    	}
    }
}
