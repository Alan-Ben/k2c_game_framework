package NPUSServer.Common.Event;

import NPCommon.Log.CommLog;
import NPEnum.ENPPlayerVariableVarType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarObj;
import NPGameRes.LogicEvent.MetaData.EventMeta;
import NPGameRes.LogicEvent.MetaData.EventMetaMgr_Refdata;
import NPGameRes.LogicEvent.MetaData.EventParam;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPUSServer.Common.Event.Events.*;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import java.util.HashMap;
import java.util.List;

/**
 * 事件参数 与 玩家参数 的映射关系
 * @author mj
 *
 */
public class EventParamVarTypeMap 
{
    private static final Log log = LogFactory.getLog(EventParamVarTypeMap.class);
    public static EventParamVarTypeMap _g_instance = new EventParamVarTypeMap();
	public static EventParamVarTypeMap getInstance() {return _g_instance;}
	
	//key = _eventId * 1000 + eventParam.index
	private HashMap<Long, ENPPlayerVariableVarType> _m_hmEventParamVarTypeMap;
	
	public EventParamVarTypeMap()
	{
		_m_hmEventParamVarTypeMap = new HashMap<>();
		
		//情人邀约
		reg(Event_P_CONSORT_RAND_CALL.ID, "CONSORT_ID", ENPPlayerVariableVarType.CONSORT_ID);
		reg(Event_P_CONSORT_RAND_CALL.ID, "CHARM_POINT_NUM", ENPPlayerVariableVarType.COUNT);
		
		//子嗣训练
		reg(Event_P_TRAIN_CHILD.ID, "CHILD_ID", ENPPlayerVariableVarType.ID);
		reg(Event_P_TRAIN_CHILD.ID, "HERO_EXP", ENPPlayerVariableVarType.COUNT);

		//消耗货币
		reg(Event_P_CONSUME_CURRENCY.ID, "TYPE", ENPPlayerVariableVarType.ID);

		//获取活动货币
		reg(Event_P_GAIN_ACTIVITY_CURRENCY.ID, "ITEM_ID", ENPPlayerVariableVarType.ID);

		//获取ITEM_DEF道具
		reg(Event_P_GAIN_ITEM_DEF.ID, "ITEM_ID", ENPPlayerVariableVarType.ID);
		
		//火星系统-处理决策
		reg(Event_P_MARS_DEAL_INTELLIGENT.ID, "INTELLIGENT_ID", ENPPlayerVariableVarType.ID);

		//商店购买物品
		reg(Event_P_SHOP_BUY_ITEM.ID, "SHOP_ID", ENPPlayerVariableVarType.ID);
		
		//火星系统-触发事件
		reg(Event_P_MARS_TRIGGER_EVENT.ID, "EVENT_ID", ENPPlayerVariableVarType.ID);

		//太空寻宝捕捉-触发事件
		reg(Event_P_TREASURE_HUNT_CAPTURE.ID, "NUM", ENPPlayerVariableVarType.COUNT);

		//阶段奖励分数变更
		reg(Event_P_STEP_REWARD_SCORE_CHG.ID, "SET_ID", ENPPlayerVariableVarType.ID);
		reg(Event_P_STEP_REWARD_SCORE_CHG.ID, "SCORE", ENPPlayerVariableVarType.COUNT);

		//获得子嗣
		reg(Event_P_GAIN_CHILD.ID, "COUNT", ENPPlayerVariableVarType.COUNT);
		
		//子嗣毕业
		reg(Event_P_TRAN_ADULT.ID, "QUALITY", ENPPlayerVariableVarType.QUALITY);
		reg(Event_P_TRAN_ADULT.ID, "COUNT", ENPPlayerVariableVarType.COUNT);
		
		//火星实力增加
		reg(Event_P_MARS_MAX_POWER_UP.ID, "NUM", ENPPlayerVariableVarType.COUNT);

        //消耗体力
        reg(Event_P_CONSUME_LAZY_CD.ID, "ID", ENPPlayerVariableVarType.ID);
        reg(Event_P_CONSUME_LAZY_CD.ID, "COUNT", ENPPlayerVariableVarType.COUNT);

        //消耗背包物品
        reg(Event_P_CONSUME_BAG_ITEM.ID, "ID", ENPPlayerVariableVarType.ID);
        reg(Event_P_CONSUME_BAG_ITEM.ID, "COUNT", ENPPlayerVariableVarType.COUNT);

        //获取货币
        reg(Event_P_GAIN_CURRENCY.ID, "TYPE", ENPPlayerVariableVarType.ID);
        reg(Event_P_GAIN_CURRENCY.ID, "NUM", ENPPlayerVariableVarType.COUNT);
	}
	
	public long getEventVarTypeIdx(int _eventId, int _paramId)
	{
		return _eventId * 1000 + _paramId;
	}

	/**
	 * 注册事件对应参数
	 * @param _evt
	 * @param _paramName
	 * @param _varType
	 */
	public void reg(int _eventId, String _paramName, ENPPlayerVariableVarType _varType)
	{
		EventMeta event = EventMetaMgr_Refdata.getInstance().lookupMetaByEventId(_eventId);
		if(null == event)
		{
			CommLog.error("EventParamVarTypeMap evt:{} reg paramName:{} fail, not find event.", _eventId, _paramName);
			return;
		}
		
		EventParam eventParam = event.lookupParam(_paramName);
		if(null == eventParam)
		{
			CommLog.error("EventParamVarTypeMap evt:{} reg paramName:{} fail, not find param.", event.getName(), _paramName);
			return;
		}
		
		long idx = getEventVarTypeIdx(_eventId, eventParam.index);
		_m_hmEventParamVarTypeMap.put(idx, _varType);
	}
	
	/**
	 * 获取对应事件对应参数的var类型
	 * @param _evt
	 * @param _paramIdx
	 * @return
	 */
	public ENPPlayerVariableVarType getVarType(_ALogicEventBase _evt, int _paramIdx)
	{
		long idx = getEventVarTypeIdx(_evt.getEventId(), _paramIdx);
		return _m_hmEventParamVarTypeMap.get(idx);
	}
	
	/**
	 * 构造玩家参数数据
	 * @param _evt
	 * @return
	 */
	public NPVarInfo makeVarInfo(_ALogicEventBase _evt)
	{
		NPVarInfo var = new NPVarInfo();

		if (_evt == null)
			return var;

		List<EventParam> params = _evt.getMeta().getParamList();
		for(int i = 0; i < params.size(); i++)
		{
			EventParam param = params.get(i);
			if(null == param)
				continue;
			
			long idx = getEventVarTypeIdx(_evt.getEventId(), param.index);
			ENPPlayerVariableVarType type = _m_hmEventParamVarTypeMap.get(idx);
			if(null != type)
			{
				long value = _evt.getParamValue(param.index);
				if(value > 0)
				{
					var.addInfo(new NPVarObj(type.ordinal(), value));
				}
			}
		}
		
		return var;
	}
}
