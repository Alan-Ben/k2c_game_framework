package NPUSServer.NPUSUserMgr.UserComp.PlayerBuffComp;

import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import EventSystem.NPHandlerEntry;
import NPCommon.DB.BM.BM;
import NPCommon.Log.CommLog;
import NPCommon.NPCommon_PlayerBuffInfo;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.PlayerBuff.RefPlayerBuff;
import NPGameRes.Refs.PlayerBuff.RefPlayerBuffEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.EventParamVarTypeMap;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_004_PlayerOp;
import USDB.Bo.PlayerBuffBO;

import java.util.ArrayList;


public class PlayerBuffInfo implements _IHandlerHolder
{
	//玩家数据
	private NPUSUserData _m_udUserData;
	
	//buff配置对象
    private RefPlayerBuff _m_refPlayerBuffRef;
    //buff数据
    private PlayerBuffBO _m_boBuffBo;

    //buff触发事件
    private RefPlayerBuffEvent _m_refPlayerBuffEventRef;
    //触发事件对象列表
    private ArrayList<NPHandlerEntry<NPUSUserData>> _m_evtEntryList;
    
    public PlayerBuffInfo(NPUSUserData _userData, RefPlayerBuff _ref, PlayerBuffBO _bo)
    {
    	_m_udUserData = _userData;
        _m_refPlayerBuffRef = _ref;
        _m_boBuffBo = _bo;
        
		//放入属性容器
		getUserData().getBonusMgr().addBonus(_ref.bonus_add, getLayer());
		//玩家属性
		getUserData().getBuffComponent().getPlayerPropertyContainer().addModifier(_ref.player_pro_add, getLayer());
		//火星属性
		getUserData().getBuffComponent().getMarsPropertyContainer().addModifier(_ref.mars_pro_add, getLayer());
		
		//buff事件配置
		_m_refPlayerBuffEventRef = RefPlayerBuffEvent.getMgr().get(getBuffId());
		//事件列表
		_m_evtEntryList = new ArrayList<>();
		//注册事件监听
		_regEvent();
    }

    public NPUSUserData getUserData() {return _m_udUserData;}

    public RefPlayerBuff getRef() {return _m_refPlayerBuffRef;}
    public long getBuffId() {return getRef().id;}

    public PlayerBuffBO getBo() {return _m_boBuffBo;}
    public int getLayer() {return getBo().getLayer();}
    public long getEndMs() {return getBo().getEndTimeMs();}
    public long getStartMs() {return getBo().getStartTimeMs();}

    public RefPlayerBuffEvent getEventRef() {return _m_refPlayerBuffEventRef;}

    protected void _onInited() 
    {
    	//执行初始化效果
        NPPlayerEffectDealer.dealEffect(_m_refPlayerBuffRef.on_init_effect, getUserData(), null, getUserData().getPlayerInitContext());
	}
    
    /**
     * 设置层数
     * @param _layer
     */
    protected void _setLayer(int _layer) 
    {
    	int preLayer = getLayer();
    	if(preLayer == _layer)
    		return;
    	
		getBo().saveLayer(getUserData().getUSServer().getBM(), _layer);
		
		//替换属性
		//放入属性容器
		getUserData().getBonusMgr().replaceBonus(getRef().bonus_add, preLayer, getRef().bonus_add, _layer);
		//玩家属性
		getUserData().getBuffComponent().getPlayerPropertyContainer().replaceModifier(getRef().player_pro_add, preLayer, getRef().player_pro_add, _layer);
		//火星属性
		getUserData().getBuffComponent().getMarsPropertyContainer().replaceModifier(getRef().mars_pro_add, preLayer, getRef().mars_pro_add, _layer);

		//buff变更事件
		getUserData().getBuffComponent().getChgDelegate().onAsyncEvent(getBuffId(), getLayer(), getEndMs());
    }
    
    /**
     * 设置buff数据
     * @param _layer
     * @param _secs
     */
    protected void _set(int _layer, long _secs) 
    {
    	//层数调整
		int preLayer = getLayer();
    	int newLayer = _layer;
		
		//截至时间调整，永久buff情况单独处理
		long nowTimeMs = CommonFunc.getNowTimeMS();
		long newEndMs = getEndMs();
		if(-1 == _secs)
		{
			newEndMs = -1;
		}
		else if(newEndMs > 0)
		{	
			newEndMs = Math.max(newEndMs, nowTimeMs) + _secs * 1000;
		}
		
		//更新数据
		BM bmObj = getUserData().getUSServer().getBM();
		
		getBo().setLayer(bmObj, newLayer);
		getBo().setStartTimeMs(bmObj, nowTimeMs);
		getBo().setEndTimeMs(bmObj, newEndMs);
		
		getBo().saveAllMarked(bmObj);

		if(preLayer != getLayer())
		{
			//替换属性容器
			getUserData().getBonusMgr().replaceBonus(getRef().bonus_add, preLayer, getRef().bonus_add, getLayer());
			//替换玩家属性
			getUserData().getBuffComponent().getPlayerPropertyContainer().replaceModifier(getRef().player_pro_add, preLayer, getRef().player_pro_add, getLayer());
			//替换火星属性
			getUserData().getBuffComponent().getMarsPropertyContainer().replaceModifier(getRef().mars_pro_add, preLayer, getRef().mars_pro_add, getLayer());
		}

		//buff变更事件
		getUserData().getBuffComponent().getChgDelegate().onAsyncEvent(getBuffId(), getLayer(), getEndMs());
	}
    
    /**
     * 调整buff数据
     * @param _layer
     * @param _secs
     */
    protected void _chg(int _layer, long _secs) 
    {
    	//层数调整
		int preLayer = getLayer();
		int newLayer = getLayer() + _layer;

		//截至时间调整，永久buff情况单独处理
		long newEndMs = getEndMs();
		if(-1 == _secs)
		{
			newEndMs = -1;
		}
		else if(newEndMs > 0)
		{
			newEndMs = Math.max(newEndMs, CommonFunc.getNowTimeMS()) + _secs * 1000;
		}
		
		//更新数据
		BM bmObj = getUserData().getUSServer().getBM();
		
		getBo().setLayer(bmObj, newLayer);
		getBo().setEndTimeMs(bmObj, newEndMs);
		
		getBo().saveAllMarked(bmObj);

		if(preLayer != getLayer())
		{
			//替换属性容器
			getUserData().getBonusMgr().replaceBonus(getRef().bonus_add, preLayer, getRef().bonus_add, getLayer());
			//替换玩家属性
			getUserData().getBuffComponent().getPlayerPropertyContainer().replaceModifier(getRef().player_pro_add, preLayer, getRef().player_pro_add, getLayer());
			//替换火星属性
			getUserData().getBuffComponent().getMarsPropertyContainer().replaceModifier(getRef().mars_pro_add, preLayer, getRef().mars_pro_add, getLayer());
		}
		
		//buff变更事件
		getUserData().getBuffComponent().getChgDelegate().onAsyncEvent(getBuffId(), getLayer(), getEndMs());
	}

    /**
     * 移除buff
     */
    protected void _del(NPPlayerContext _context) 
    {
		getBo().del(getUserData().getUSServer().getBM());
		
		//移除属性容器
		getUserData().getBonusMgr().removeBonus(getRef().bonus_add, getLayer());
		//移除玩家属性
		getUserData().getBuffComponent().getPlayerPropertyContainer().removeModifier(getRef().player_pro_add, getLayer());
		//移除火星属性
		getUserData().getBuffComponent().getMarsPropertyContainer().removeModifier(getRef().mars_pro_add, getLayer());
		
		//注销事件监听
		_unregEvent();
		
		//执行移除效果
        NPPlayerEffectDealer.dealEffect(_m_refPlayerBuffRef.on_remove_effect, getUserData(), null, _context);
		
		//buff变更事件
		getUserData().getBuffComponent().getDelDelegate().onAsyncEvent(getBuffId());
	}
    
    /**
     * 检查是否过期
     * @return
     */
    protected	boolean _checkExpired()
    {
    	return getEndMs() > 0 && CommonFunc.getNowTimeMS() > getEndMs();
    }
    
    /**
     * 注册事件
     */
    private void _regEvent()
    {
    	if(null == _m_refPlayerBuffEventRef)
    		return;
    	
    	for(String eventStr : _m_refPlayerBuffEventRef.logic_event_list)
    	{
    		EventMeta eventMeta = EventMetaMgr_Refdata.getInstance().lookupMetaByEventName(eventStr.toUpperCase());
            if (eventMeta == null)
            {
                CommLog.error("PlayerBuffInfo._regEvent failed, event not found, buffId:{} event:{}", getBuffId(), eventStr, new Exception());
                continue;
            }
            
            //注册事件监听
            NPHandlerEntry<NPUSUserData> entry = getUserData().getEventHandlerMgr().regHandler(eventMeta.getEventId(), this, 
            		new HandlerTwo<_ALogicEventBase, NPUSUserData>()
            {
                @Override
                public void handle(_ALogicEventBase _evt, NPUSUserData _userData)
                {
                    NPPlayerContext context = (NPPlayerContext) _evt.getContext();
                    _onLogicEvt(_evt, context);
                }
            });
            _m_evtEntryList.add(entry);
    	}
    }
    
    /**
     * 触发事件
     * @param _evt
     * @param _context
     */
    private void _onLogicEvt(_ALogicEventBase _evt, NPPlayerContext _context)
    {
    	if(null == _m_refPlayerBuffEventRef)
    		return;
    	
    	//层数不够
    	if(getLayer() < _m_refPlayerBuffEventRef.trigger_reduce_layers)
    		return;

    	NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);
    	
        //检查触发条件
        if (!NPPlayerConditionDealerMgr.IsEnable(_m_refPlayerBuffEventRef.trigger_condition, getUserData(), varInfo))
            return;
        
        //检查概率
    	long per = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getUserData(), _m_refPlayerBuffEventRef.trigger_per_form, varInfo);
    	long perV = CommonFunc.randomLong(10000);
    	if(perV > per)
    		return;
    	
    	//扣除触发层数
    	getUserData().lockUser();
    	try
    	{
    		//计算扣除层数
    		int curLayer = Math.max(0, getLayer() - _m_refPlayerBuffEventRef.trigger_reduce_layers);
    		
    		//计算当前层数扣除后是否buff移除
    		if(curLayer <= 0 && getRef().layer_empty_remove) //结束的buff需要移除
            {
    			getUserData().getBuffComponent()._removeBuff(this, _context);
            }
    		else
    		{
    			//更新层数
    			_setLayer(curLayer);
    			 //推送数据
    	    	getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_053_OnPlayerBuffChg(this));
    		}
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    	
    	//触发效果
    	ALSynTaskManager.getInstance().regTask(()->
    	{
            NPPlayerEffectDealer.dealEffect(_m_refPlayerBuffEventRef.trigger_effect, getUserData(), varInfo, _context);
            
            //推送buff触发协议
    		getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_061_OnPlayerBuffTrigger(getBuffId()));
    	});
    }
    
    /**
     * 注销事件
     */
    protected void _unregEvent()
    {
    	//注销对象监听
        getUserData().getEventHandlerMgr().unregHandler(this);
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
     * 检查事件触发，只适用于外部触发
     * @param _evt
     * @param _context
     * @return
     */
    public boolean checkLogicEvt(_ALogicEventBase _evt, NPPlayerContext _context)
    {
    	if(null == _m_refPlayerBuffEventRef)
    		return false;
    	
    	//层数不够
    	if(getLayer() < _m_refPlayerBuffEventRef.trigger_reduce_layers)
    		return false;

    	NPVarInfo varInfo = EventParamVarTypeMap.getInstance().makeVarInfo(_evt);
        //检查触发条件
        if (!NPPlayerConditionDealerMgr.IsEnable(_m_refPlayerBuffEventRef.trigger_condition, getUserData(), varInfo))
            return false;
        
        //检查概率
    	long per = NPPlayerVariableDeal.getInstance().CalculateVariableResult(getUserData(), _m_refPlayerBuffEventRef.trigger_per_form, varInfo);
    	long perV = CommonFunc.randomLong(10000);
    	if(perV > per)
    		return false;
    	
    	//扣除触发层数
    	getUserData().lockUser();
    	try
    	{
    		//计算扣除层数
    		int curLayer = Math.max(0, getLayer() - _m_refPlayerBuffEventRef.trigger_reduce_layers);
    		
    		//计算当前层数扣除后是否buff移除
    		if(curLayer <= 0 && getRef().layer_empty_remove) //结束的buff需要移除
            {
    			getUserData().getBuffComponent()._removeBuff(this, _context);
            }
    		else
    		{
    			//更新层数
    			_setLayer(curLayer);
    			 //推送数据
    	    	getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_053_OnPlayerBuffChg(this));
    		}
    		 
	    	//触发效果
	    	ALSynTaskManager.getInstance().regTask(()->
	    	{
	            NPPlayerEffectDealer.dealEffect(_m_refPlayerBuffEventRef.trigger_effect, getUserData(), varInfo, _context);
	            
	            //推送buff触发协议
	    		getUserData().sendMsgToGC(US2GCWriter_004_PlayerOp.make_061_OnPlayerBuffTrigger(getBuffId()));
	    	});
    		 
    		 return true;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
    
    /**
     * 数据协议对象
     * @return
     */
    public NPCommon_PlayerBuffInfo toProto()
    {
        NPCommon_PlayerBuffInfo proto = new NPCommon_PlayerBuffInfo();
        proto.setBuffId(getBuffId());
        proto.setLayer(getLayer());
        proto.setEndMs(getEndMs());
        proto.setStartMs(getStartMs());
        
        return proto;
    }
    
    @Override
    public String toString()
    {
    	getUserData().lockUser();
    	
    	try
    	{
    		StringBuilder sb = new StringBuilder();
    		sb.append("buffId:").append(getBuffId())
    			.append("\tlayer:").append(getLayer());
    		
    		//永久buff
    		if(getEndMs() == -1)
    		{
    			sb.append("\tendMs:-1");
    		}
    		else
    		{
    			sb.append("\tendMs:").append(CommonFunc.getTimeStringMs(getEndMs()));
    		}
    		
    		return sb.toString();
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }
}
