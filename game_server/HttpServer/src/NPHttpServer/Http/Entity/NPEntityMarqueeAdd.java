package NPHttpServer.Http.Entity;

import Common.ServerObj.ServerObj_PHPMarquee;
import Common.ServerObj.ServerObj_PHPMarqueeContent;
import CommonEnum.EMarqueeCanDelType;
import NPEnum.EMarqueeOfflineNeedShowType;

import java.util.ArrayList;

/**
 * 跑马灯数据结构
 */
public class NPEntityMarqueeAdd
{
	//后台跑马灯唯一ID
    private long _m_lPHPId;
    //必填，US列表
    private ArrayList<Integer> _m_alUsIdList;
    //跑马灯配置ID
    private long _m_lMarqueeId;
    //窗口展示位置
    private int _m_iShowPos;
    //优先级（越大越优先）
    private int _m_iPriority;
    //预制体ID（展示使用）
    private long _m_lUiResId;
    //循环播放时长秒（优先于次数）
    private int _m_iDurationSec;
    //循环播放次数
    private int _m_iDurationCount;
    //生存时间秒
    private int _m_iLifeSec;
    //跑马灯是否可删除类型
    private EMarqueeCanDelType _m_eCanDelType;
    //玩家离线期间是否需要展示
    private EMarqueeOfflineNeedShowType _m_eOfflineNeedShowType;
    //默认语言（不填写则使用跑马灯信息列表第1条）
    private String _m_sDefaultLang;
    //跑马灯内容信息（多语言）
    private ArrayList<ServerObj_PHPMarqueeContent> _m_alLangContentList;
    //渠道列表
    private ArrayList<String> _m_alChannelList;
    //展示条件（仅客户端使用）
    private String _m_sShowCondition;
	
    public NPEntityMarqueeAdd()
    {
    	_m_alUsIdList = new ArrayList<>();
    	_m_eCanDelType = EMarqueeCanDelType.READ_REF;
    	_m_eOfflineNeedShowType = EMarqueeOfflineNeedShowType.READ_REF;
    	_m_sDefaultLang = "";
    	_m_alLangContentList = new ArrayList<>();
    	_m_alChannelList = new ArrayList<>();
    	_m_sShowCondition = "";
    }
    
    public long getPHPId() {return _m_lPHPId;}
    public void setPHPId(long _value) {_m_lPHPId = _value;}

    public ArrayList<Integer> getUsIdList() {return _m_alUsIdList;}
    public void addUsIdList(Integer _value) {_m_alUsIdList.add(_value);}
    
    public long getMarqueeId() {return _m_lMarqueeId;}
    public void setMarqueeId(long _value) {_m_lMarqueeId = _value;}
    
    public long getShowPos() {return _m_iShowPos;}
    public void setShowPos(int _value) {_m_iShowPos = _value;}
    
    public long getPriority() {return _m_iPriority;}
    public void setPriority(int _value) {_m_iPriority = _value;}
    
    public long getUiResId() {return _m_lUiResId;}
    public void setUiResId(long _value) {_m_lUiResId = _value;}
    
    public long getDurationSec() {return _m_iDurationSec;}
    public void setDurationSec(int _value) {_m_iDurationSec = _value;}
    
    public long getDurationCount() {return _m_iDurationCount;}
    public void setDurationCount(int _value) {_m_iDurationCount = _value;}
    
    public long getLifeSec() {return _m_iLifeSec;}
    public void setLifeSec(int _value) {_m_iLifeSec = _value;}
    
    public EMarqueeCanDelType getCanDelType() {return _m_eCanDelType;}
    public void setCanDelType(EMarqueeCanDelType _value) {_m_eCanDelType = _value;}

    public String getDefaultLang() {return _m_sDefaultLang;}
    public void setDefaultLang(String _value) {_m_sDefaultLang = _value;}
    
    public EMarqueeOfflineNeedShowType getOfflineNeedShowType() {return _m_eOfflineNeedShowType;}
    public void setOfflineNeedShowType(EMarqueeOfflineNeedShowType _value) {_m_eOfflineNeedShowType = _value;}
    
    public ArrayList<ServerObj_PHPMarqueeContent> getLangContentList() {return _m_alLangContentList;}
    public void addLangContentList(ServerObj_PHPMarqueeContent _value) {_m_alLangContentList.add(_value);}
    
    public ArrayList<String> getChannelList() {return _m_alChannelList;}
    public void addChannelList(String _value) {_m_alChannelList.add(_value);}
    
    public String getShowCondition() {return _m_sShowCondition;}
    public void setShowCondition(String _showCondition) {_m_sShowCondition = _showCondition;}
    
    /**
     * 构造协议数据
     * @return
     */
    public ServerObj_PHPMarquee toProto()
    {
    	ServerObj_PHPMarquee proto = new ServerObj_PHPMarquee();
    	proto.setPhpId(_m_lPHPId);
    	proto.setRefId(_m_lMarqueeId);
    	proto.setShowPosId(_m_iShowPos);
    	proto.setPriorityId(_m_iPriority);
    	proto.setDurationSec(_m_iDurationSec);
    	proto.setDurationCount(_m_iDurationCount);
    	proto.setUiResId(_m_lUiResId);
    	proto.setCanDelType(_m_eCanDelType);
    	proto.setLifeSec(_m_iLifeSec);
    	proto.setOfflineNeedShowType(_m_eOfflineNeedShowType);
    	proto.setDefaultLang(_m_sDefaultLang);
    	
    	for(int i = 0; i < _m_alLangContentList.size(); i++)
    	{
    		ServerObj_PHPMarqueeContent content = _m_alLangContentList.get(i);
    		if(null == content)
    			continue;
    		
    		proto.addMarqueeContentList(content);
    	}
    	
    	for(int i = 0; i < _m_alChannelList.size(); i++)
    	{
    		proto.addChannelList(_m_alChannelList.get(i));
    	}
    	
    	proto.setShowCondition(_m_sShowCondition);
    	
    	return proto;
    }
}
