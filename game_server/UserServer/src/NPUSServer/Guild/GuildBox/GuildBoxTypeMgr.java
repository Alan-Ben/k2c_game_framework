package NPUSServer.Guild.GuildBox;

import ALBasicServer.ALBasicMutex.MutexAtom;
import Common.GuildEnum.EGuildBoxType;
import Common.GuildObj.Guild_BoxInfo;
import Common.ServerObj.ServerObj_GuildBoxSettle;
import Common.ServerObj.ServerObj_GuildBoxSettleList;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.Guild.RefGuildBox;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Guild.GuildInfo;
import NPUSServer.Guild.Member.GuildMemberInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import USDB.Bo.GuildBoxBO;

import java.util.ArrayList;
import java.util.Comparator;

/**
 * 联盟宝箱 - 按类型分类
 * @author mj
 *
 */
public class GuildBoxTypeMgr 
{
	//联盟宝箱类型
	private EGuildBoxType _m_eType;
	//公会数据对象
    private GuildInfo _m_guildInfo;
	//联盟宝箱列表
	private ArrayList<GuildBoxInfo> _m_alGuildBoxList;
	//宝箱最大实例ID
	private long _m_lMaxBoxInstanceId;
    //锁对象
    private MutexAtom _m_mutex;
    
    public GuildBoxTypeMgr(EGuildBoxType _type, GuildInfo _guild)
    {
    	_m_eType = _type;
    	_m_guildInfo = _guild;
    	_m_alGuildBoxList = new ArrayList<>();
    	_m_mutex = new MutexAtom();
    }
    
    public EGuildBoxType getType() {return _m_eType;}
    
    public GuildInfo getGuild() {return _m_guildInfo;}
    public NPUserServer getUSServer() {return getGuild().getGuildMgr().getServer();}
    public BM getBM() {return getUSServer().getBM();}
    
    public long getMaxBoxInstanceId() {return _m_lMaxBoxInstanceId;}
    
    private void _lock() {_m_mutex.lock();}
    private void _unlock() {_m_mutex.unlock();}
    
    protected void _initFromBo(GuildBoxBO _bo) 
    {
    	GuildBoxInfo info = new GuildBoxInfo(_m_guildInfo, _bo);
		_m_alGuildBoxList.add(info);
		
		if(_m_lMaxBoxInstanceId == 0 || _m_lMaxBoxInstanceId < info.getId())
			_m_lMaxBoxInstanceId = info.getId();
    }
    
    protected void _onInited()
    {
    	//ID升序排列
    	CommonFunc.sortAscList(_m_alGuildBoxList, 
    			new Comparator<GuildBoxInfo>() 
    	{
			@Override
			public int compare(GuildBoxInfo o1, GuildBoxInfo o2) 
			{
				return Long.compare(o1.getId(), o2.getId());
			}
		});
    }
    
    /**
     * 增加宝箱数据
     * @param _cid
     * @param _boxId
     * @param _count
     * @param _context
     */
    public void addBox(long _cid, long _boxId, long _count, NPPlayerContext _context)
    {
    	//获取玩家在联盟的数据，并获取相关配置（是否匿名）
    	boolean isGuildBoxShareAnonymous = false;
    	//玩家发送的宝箱需要检查，系统发送的无视该规则
    	if(_cid > 0)
    	{
    		GuildMemberInfo member = getGuild().getMemberMgr().lookup(_cid);
        	if(null == member)
        		return;
        	
        	isGuildBoxShareAnonymous = member.isGuildBoxShareAnonymous();
    	}
    	
    	_lock();
    	
    	try
    	{
    		RefGuildBox ref = RefGuildBox.getMgr().get(_boxId);
    		if(null == ref)
    		{
    			USLog.error(getUSServer(), "guild:{} player:{} box:{} add guild box fail, not find ref.", getGuild().getGuildId(), _cid, _boxId);
    			return;
    		}
    		
    		//增加宝箱数据
    		for(int i = 0; i < _count; i++)
    		{
    			GuildBoxBO bo = new GuildBoxBO();
        		bo.setGuildId(getBM(), getGuild().getGuildId());
        		bo.setBoxId(getBM(), _boxId);
        		//宝箱过期时间
        		if(ref.type == EGuildBoxType.GUILD_ACTIVE_BOX) //活跃宝箱设置7天
        			bo.setEndTime(getBM(), CommonFunc.getNowTimeMS() + RefGeneral.Ref().guild_box_active_effect_secs * 1000);
        		else //其他宝箱过期时间设置
        			bo.setEndTime(getBM(), CommonFunc.getNowTimeMS() + RefGeneral.Ref().guild_box_effect_secs * 1000);
        		//宝箱匿名数据处理
        		if(ref.type == EGuildBoxType.GUILD_FREE_BOX || !isGuildBoxShareAnonymous) //匿名处理：免费宝箱 或 非匿名处理
        			bo.setShareCid(getBM(), _cid);
        		bo.insert(getBM());
        		
        		GuildBoxInfo info = new GuildBoxInfo(_m_guildInfo, bo, ref);
        		_m_alGuildBoxList.add(info);
        		
        		_m_lMaxBoxInstanceId = info.getId();
    		}
    	}
    	finally 
    	{
    		_unlock();
    	}
    	
    	//同步联盟所有玩家
    	getGuild().getMemberMgr().incrClientAddCount(_m_eType, (int) _count);
    }

    /**
     * 检查并移除过期宝箱
     */
    public void check()
    {
    	_lock();
    	
    	try
    	{
    		long nowTime = CommonFunc.getNowTimeMS();
    		
    		//联盟宝箱按ID（等同截至时间）从小到大有序排列
    		do
    		{
    			if(_m_alGuildBoxList.isEmpty())
    				break;
    			
    			GuildBoxInfo info = _m_alGuildBoxList.get(0);
    			if(null == info)
    			{
    				_m_alGuildBoxList.remove(0);
    				continue;
    			}
    			
    			//后面的宝箱截至时间只会更大
    			if(info.getEndTime() > nowTime)
    				break;
    			
    			//移除宝箱
    			_m_alGuildBoxList.remove(0);
    			info._del();
    			
    		} while(!_m_alGuildBoxList.isEmpty());
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 刷新对应玩家的宝箱列表数据
     * @param _member
     * @param _playerCanLimit
     * @return
     */
    public ArrayList<Guild_BoxInfo> refreshPlayerBoxList(GuildMemberInfo _member, int _playerCanLimit)
    {
    	//玩家当前无可领取额度
    	if(_playerCanLimit == 0)
    		return null;
    	
    	//获取玩家上次的领取的宝箱标记的数据，用于获取之后的宝箱数据
    	long preLastMarkedBoxInstanceId = _member.getMemberGuildBoxInfo().getLastGuildBox(_m_eType);
    	if(-1 == preLastMarkedBoxInstanceId)
    		return null;
    	
    	_lock();
    	
    	try
    	{
    		//刷新过期宝箱数据
    		check();
    		
    		//刷新的宝箱列表数据
    		ArrayList<Guild_BoxInfo> list = null;
    		//最新标记的宝箱实例ID，作为下次查询的起始标志
        	long newLastMarkedBoxInstanceId = 0;
        	
        	//检测玩家可以领取的宝箱数量
    		for(int i = 0; i < _m_alGuildBoxList.size(); i++)
    		{
    			GuildBoxInfo info = _m_alGuildBoxList.get(i);
    			if(null == info)
    				continue;
    			
    			if(preLastMarkedBoxInstanceId >= info.getId())
    				continue;
    			
    			if(null == list)
    				list = new ArrayList<>();

    			//新增宝箱数据
    			list.add(info.toProto());
    			//更新玩家最后一次宝箱的标记
    			if(0 == newLastMarkedBoxInstanceId || newLastMarkedBoxInstanceId < info.getId())
    				newLastMarkedBoxInstanceId = info.getId();
    			
    			//检查数量
    			if(_playerCanLimit != -1 && list.size() >= _playerCanLimit)
    				break;
    		}

    		//修改联盟玩家内存标志位
    		if(newLastMarkedBoxInstanceId > preLastMarkedBoxInstanceId)
    			_member.getMemberGuildBoxInfo().updateLastGuildBox(_m_eType, newLastMarkedBoxInstanceId);
    		
    		//重置玩家的预存的可领取次数
    		_member.getMemberGuildBoxInfo().resetClientAddCount(_m_eType);
    		
    		return list;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 结算玩家数据（当玩家离开联盟时触发，此时不再更新玩家最后领取标记）
     * @param _member
     * @param _listObj
     */
    public void settlePlayerBox(GuildMemberInfo _member, ServerObj_GuildBoxSettleList _listObj)
    {
    	//刷新当前玩家的宝箱数据，不设置限制，在下发邮件时处理
    	ArrayList<Guild_BoxInfo> list = refreshPlayerBoxList(_member, -1);
    	if(null == list)
    		return;
    	
    	//构造联盟宝箱结算数据
    	ServerObj_GuildBoxSettle serverObj = new ServerObj_GuildBoxSettle(getType(), list);
    	_listObj.addSettleList(serverObj);
    }
    
    /**
     * 销毁数据
     */
    public void discard()
    {
    	_lock();
    	
    	try
    	{
    		_m_alGuildBoxList.clear();
    		getBM().getBM(GuildBoxBO.class).delAll("guild_id", _m_guildInfo.getGuildId());
    	}
    	finally
    	{
    		_unlock();
    	}
    }
}
