package NPUSServer.NPUSUserMgr.CommonEvent;

import Common.EventEnum.ECommonEventType;
import Common.EventObj.CommonEvent_DetailInfo;
import Common.EventObj.CommonEvent_DoneInfo;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultOne;
import NPGameRes.Refs.CommonEvent.RefCommonEvent;
import NPGameRes.Refs.CommonEvent._ARefCommonEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.CommonEvent.Dealer.*;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

import java.util.List;

/**
 * 通用事件处理管理器
 */
public class CommonEventDealerMgr
{
    public static CommonEventDealerMgr _g_instance = new CommonEventDealerMgr();

    public static CommonEventDealerMgr getInstance()
    {
        return _g_instance;
    }

    //效果处理对象数组
    public _ACommonEventDealer<?>[] _m_arrEventDealerArr;

    public CommonEventDealerMgr()
    {
        _m_arrEventDealerArr = new _ACommonEventDealer[ECommonEventType.values().length];

        //注册事件处理对象
        _regDealer(new CommonEventDealer_Award());
        _regDealer(new CommonEventDealer_Choice());
        _regDealer(new CommonEventDealer_Dialog());
        _regDealer(new CommonEventDealer_Dispatch());
        _regDealer(new CommonEventDealer_PlotDialog());
        _regDealer(new CommonEventDealer_MiniGame());
    }

    /**
     * 注册事件处理对象
     * @param _dealer 事件处理对象
     */
    private void _regDealer(_ACommonEventDealer<?> _dealer)
    {
        _m_arrEventDealerArr[_dealer.eventType().ordinal()] = _dealer;
    }

    /**
     * 获取事件处理对象
     * @param _type 事件类型
     * @return 事件处理对象
     */
    private _ACommonEventDealer<?> _getDealer(ECommonEventType _type)
    {
        return _m_arrEventDealerArr[_type.ordinal()];
    }

    /**
     * 创建事件
     * @param _userData 玩家信息
     * @param _eventId  事件id
     * @return 事件详细信息
     */
    public static CommonEvent_DetailInfo createEvent(NPUSUserData _userData, long _eventId)
    {
        //获取事件配置
        RefCommonEvent refCommonEvent = RefCommonEvent.getMgr().get(_eventId);
        if (null == refCommonEvent)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr.dealEvent: can not find event id = " + _eventId);
            return null;
        }

        //获取事件详细配置
        _ARefCommonEvent detailRef = refCommonEvent.detailRef;
        if (null == detailRef)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr.dealEvent: can not find detail ref, event id = " + _eventId);
            return null;
        }

        //查找事件处理对象
        _ACommonEventDealer<?> dealer = CommonEventDealerMgr.getInstance()._getDealer(detailRef.getEventType());
        if (null == dealer)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr.dealEvent: can not find dealer, event id = " + _eventId);
            return null;
        }

        //使用事件处理对象创建事件，返回事件详细信息
        return dealer.createEvent(_userData, _eventId);
    }

    /**
     * 处理事件
     * @param _userData 玩家信息
     * @param _eventId  事件id
     * @param _dealInfo 事件处理数据
     * @param _context  玩家上下文
     * @return 事件处理结果
     */
    public static ResultOne<CommonEvent_DoneInfo> dealEvent(NPUSUserData _userData, long _eventId, byte[] _dealInfo, NPPlayerContext _context)
    {
        //获取事件配置
        RefCommonEvent refCommonEvent = RefCommonEvent.getMgr().get(_eventId);
        if (null == refCommonEvent)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr.dealEvent: can not find event id = " + _eventId);
            return ResultOne.failed(CommErr.REF_NOT_FOUND);
        }

        //获取事件详细配置
        _ARefCommonEvent detailRef = refCommonEvent.detailRef;
        if (null == detailRef)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr.dealEvent: can not find detail ref, event id = " + _eventId);
            return ResultOne.failed(CommErr.REF_NOT_FOUND);
        }

        //查找事件处理对象
        _ACommonEventDealer<?> dealer = CommonEventDealerMgr.getInstance()._getDealer(detailRef.getEventType());
        if (null == dealer)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr.dealEvent: can not find dealer, event id = " + _eventId);
            return ResultOne.failed(CommErr.SYS_ERR);
        }

        //使用事件处理对象处理事件，返回事件处理结果
        return dealer.dealEvent(_userData, detailRef, _dealInfo, _context);
    }

    /**
     * 获取指定事件的默认奖励
     * @param _userData
     * @param _eventId  事件id
     * @return 奖励列表
     */
    public static List<NPCommonCostItem> getCmdRewardList(NPUSUserData _userData, long _eventId)
    {
        //获取事件配置
        RefCommonEvent refCommonEvent = RefCommonEvent.getMgr().get(_eventId);
        if (null == refCommonEvent)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr.dealEvent: can not find event id = " + _eventId);
            return null;
        }

        //获取事件详细配置
        _ARefCommonEvent detailRef = refCommonEvent.detailRef;
        if (null == detailRef)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr.dealEvent: can not find detail ref, event id = " + _eventId);
            return null;
        }

        //查找事件处理对象
        _ACommonEventDealer<?> dealer = CommonEventDealerMgr.getInstance()._getDealer(detailRef.getEventType());
        if (null == dealer)
        {
            USLog.error(_userData.getUSServer(), "CommonEventDealerMgr.dealEvent: can not find dealer, event id = " + _eventId);
            return null;
        }

        return dealer.getCmdRewardList(detailRef);
    }
}
