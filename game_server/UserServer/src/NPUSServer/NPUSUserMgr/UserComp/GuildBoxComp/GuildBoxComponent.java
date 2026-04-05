package NPUSServer.NPUSUserMgr.UserComp.GuildBoxComp;

import ALBasicServer.ALProcess.ALProcess;
import Common.GuildEnum.EGuildBoxType;
import Common.GuildObj.Guild_BoxInfo;
import Common.ServerObj.ServerObj_GuildBoxSettle;
import Common.ServerObj.ServerObj_GuildBoxSettleList;
import EventSystem.NPHandlerEntry;
import NPCommon.CommonProcess._IEZProcessMonitorNoTimeOut;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPItemType;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Guild.RefGuildBoxEvent;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

/**
 * 联盟宝箱数据，记录玩家可领取的联盟宝箱数据
 * @author mj
 *
 */
public class GuildBoxComponent extends _ANPUserComponent implements _IHandlerHolder
{
	//根据宝箱类型区分宝箱数据
	private _APlayerGuildBoxInfo[] _m_arrGuildBoxArr;
    //触发事件对象列表
    private ArrayList<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;
	
    public GuildBoxComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.GUILD_BOX);
        
        _m_arrGuildBoxArr = new _APlayerGuildBoxInfo[EGuildBoxType.EGuildBoxType_Length];
        
        _regGuildBox(new PlayerGuildBoxInfo_Active(_userData));
        _regGuildBox(new PlayerGuildBoxInfo_Free(_userData));
        _regGuildBox(new PlayerGuildBoxInfo_Gift(_userData));

		//事件列表
		_m_evtEntryList = new ArrayList<>();
		//注册事件监听
		_regEvent();
    }
    
    private void _regGuildBox(_APlayerGuildBoxInfo _info)
    {
    	_m_arrGuildBoxArr[_info.getType().ordinal()] = _info;
    }
    
    public _APlayerGuildBoxInfo getGuildBox(EGuildBoxType _type)
    {
    	return _m_arrGuildBoxArr[_type.ordinal()];
    }
    
    @Override
    protected void _init()
    {
        ALProcess process = ALProcess.CreateProcess("guild_box_comp_init");

        //依次加载对应类型的宝箱数据
        for(int i = 0; i < _m_arrGuildBoxArr.length; i++)
		{
			_APlayerGuildBoxInfo info = _m_arrGuildBoxArr[i];
			if(null == info)
				continue;
			
			process.addResDelegateProcess(action -> info._initFromDB(action::dealAction), "guild_box_init",
	                () -> USLog.error(getUSServer(), "load guild_box_comp fail, cid:{} type:{} ", getUserData().getCid(), info.getType()), false);
        }
        
        process.dealProcess(new _IEZProcessMonitorNoTimeOut()
        {
            @Override
            public void onRootProecssStop()
            {
                USLog.error(getUSServer(), "load guild_box_comp fail[onRootProecssStop], cid:{}", getUserData().getCid());
                getUserData().setDataLoadFail();
            }

            @Override
            public void onRootProecssSuc()
            {
                setInited();
            }
        });
    }

    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
    }

    @Override
    public void dispose()
    {
    	_unregEvent();
    }
    
    /**
     * 注册监听事件
     */
    private void _regEvent()
    {
    	HashMap<Integer, RefGuildBoxEvent> guildBoxEventMap = RefGeneral.Ref().guildBoxEventMap;
    	for(Map.Entry<Integer, RefGuildBoxEvent> entry : guildBoxEventMap.entrySet())
    	{
    		if(null == entry)
    			continue;
    		
    		//注册事件监听
            NPHandlerEntry<NPUSUserData> handler = getUserData().getEventHandlerMgr().regHandler(entry.getKey(), this, 
            		new HandlerTwo<_ALogicEventBase, NPUSUserData>()
            {
                @Override
                public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                {
                    NPPlayerContext context = (NPPlayerContext) _evt.getContext();
                    _onLogicEvt(_evt, context);
                }
            });
            _m_evtEntryList.add(handler);
    	}
    }
    /**
     * 注销监听事件
     */
    protected void _unregEvent()
    {
        //注销触发事件监听
        for(int i = 0; i < _m_evtEntryList.size(); i++)
        {
        	NPHandlerEntry<NPUSUserData> entry = _m_evtEntryList.get(i);
        	if(null == entry)
        		continue;
        	
        	getUserData().getEventHandlerMgr().unregHandler(entry);
        }
    }
    /**
     * 对抛出事件进行处理
     * @param _evt
     * @param _context
     */
    private void _onLogicEvt(_ALogicEventBase _evt, NPPlayerContext _context)
    {
    	RefGuildBoxEvent eventRef = RefGeneral.Ref().guildBoxEventMap.get(_evt.getEventId());
    	if(null == eventRef)
    		return;
    	
    	long count = _evt.getValue(eventRef.trigger_count_rate);
    	if(count <= 0)
    		return;
    	
    	//实际允许获得的宝箱数量
    	int boxCount = 0;
    	for(int i = 0; i < count; i++)
    	{
    		long perV = CommonFunc.randomLong(10000);
        	if(perV > eventRef.trigger_per)
        		continue;
        	
        	boxCount++;
    	}
    	if(boxCount <= 0)
    		return;
    	
    	getUserData().lockUser();
    	
    	try
    	{
    		getUserData().gainItem(ENPItemType.GUILD_BOX, eventRef.box_id, boxCount, _context);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    public void refreshAll()
    {
    	for(int i = 0; i < _m_arrGuildBoxArr.length; i++)
		{
			_APlayerGuildBoxInfo info = _m_arrGuildBoxArr[i];
			if(null == info)
				continue;
		
			info.refreshBox();
		}
    }
    
    public void makeProto(ArrayList<Guild_BoxInfo> _list)
    {
    	for(int i = 0; i < _m_arrGuildBoxArr.length; i++)
		{
			_APlayerGuildBoxInfo info = _m_arrGuildBoxArr[i];
			if(null == info)
				continue;
			
			info.makeProto(_list);
		}
    }
	
    /**
     * 获取指定宝箱可以领取的数量
     * @param _boxType
     * @return
     */
    public int getCanCount(EGuildBoxType _boxType)
    {
    	_APlayerGuildBoxInfo box = getGuildBox(_boxType);
    	if(null == box)
    		return 0;
    	
    	return box.calNeedCount();
    }
    
    /**
     * 增加宝箱
     * @param _boxType
     * @param _boxList
     */
    public void addBox(EGuildBoxType _boxType, ArrayList<Guild_BoxInfo> _boxList)
    {
    	_APlayerGuildBoxInfo box = getGuildBox(_boxType);
    	if(null == box)
    		return;
    	
    	box.add(_boxList);
    }
    
    /**
     * 结算所有类型宝箱
     * @param _settleListObj
     * @param _context
     */
    public void dispatchAll(ServerObj_GuildBoxSettleList _settleListObj, NPPlayerContext _context)
    {
    	for(int i = 0; i < _m_arrGuildBoxArr.length; i++)
		{
			_APlayerGuildBoxInfo info = _m_arrGuildBoxArr[i];
			if(null == info)
				continue;
			
			//获取联盟结算数据，补充到玩家可领取宝箱数据列表
			ArrayList<Guild_BoxInfo> boxList = null;
			if(null != _settleListObj)
			{
				for(int j = 0; j < _settleListObj.getSettleList().size(); j++)
				{
					ServerObj_GuildBoxSettle settleObj = _settleListObj.getSettleList().get(j);
					if(null == settleObj)
						continue;
					
					if(settleObj.getBoxType() == info.getType())
					{
						boxList = settleObj.getBoxList();
						break;
					}
				}
			}
			
			info.dispatch(boxList, _context);
		}
    }
}
