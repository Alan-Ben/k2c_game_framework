package NPUSServer.NPUSUserMgr.CommonEvent;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.EventEnum.ECommonEventType;
import Common.EventObj.CommonEvent_DetailInfo;
import Common.EventObj.CommonEvent_DoneInfo;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.NPCommon_ItemInfo;
import NPGameRes.Refs.CommonEvent.RefCommonEventReward;
import NPGameRes.Refs.CommonEvent._ARefCommonEvent;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.USLog;

import java.util.ArrayList;
import java.util.List;

/**
 * 通用事件处理器基类
 */
public abstract class _ACommonEventDealer<T extends _ARefCommonEvent>
{
    /**
     * 获取事件处理器的事件类型
     * @return 事件类型
     */
    public abstract ECommonEventType eventType();

    /**
     * 创建展示信息
     * @param _userData 用户数据
     * @return 展示信息
     */
    public abstract _IALProtocolStructure createShowInfo(NPUSUserData _userData);

    /**
     * 创建事件
     * @param _userData 用户数据
     * @param _eventId  事件ID
     * @return 事件详细信息
     */
    public CommonEvent_DetailInfo createEvent(NPUSUserData _userData, long _eventId)
    {
        CommonEvent_DetailInfo detailInfo = new CommonEvent_DetailInfo();
        detailInfo.setCommonEventId(_eventId);

        //构造展示信息
        _IALProtocolStructure showInfo = createShowInfo(_userData);
        if (showInfo != null)
            detailInfo.setShowInfo(showInfo.makePackage().array());

        return detailInfo;
    }

    /**
     * 获取事件奖励列表
     * @param _eventRewardId 事件奖励ID
     */
    public List<NPCommonCostItem> getEventRewardList(long _eventRewardId)
    {
        //查询事件奖励配置
        RefCommonEventReward refChapterEventReward = RefCommonEventReward.getMgr().get(_eventRewardId);
        if (refChapterEventReward == null)
            return null;

        return refChapterEventReward.item_list;
    }

    /**
     * 领取事件奖励
     * @param _userData      用户数据
     * @param _eventRewardId 事件奖励ID
     * @param _context       玩家上下文
     * @return 领取的奖励列表
     */
    public List<NPCommon_ItemInfo> drawEventReward(NPUSUserData _userData, long _eventRewardId, NPPlayerContext _context)
    {
        //查询事件奖励配置
        List<NPCommonCostItem> rewardList = getEventRewardList(_eventRewardId);
        if (rewardList == null)
        {
            USLog.error(_userData.getUSServer(), "_ACommonEventDealer drawEventReward fail, cid:{} eventRewardId:{}", _userData.getCid(), _eventRewardId);
            return null;
        }

        NPPlayerContext newContext = NPPlayerContext.createNew(_context);

        //领取奖励
        _userData.gainItemList(rewardList, newContext);

        //用来单次记录单次奖励领取
        List<NPCommon_ItemInfo> gainItemList = new ArrayList<>();
        newContext.getCollector().fillProtoList(gainItemList);

        //需要记录到原来的context上
        _context.getCollector().addItemListP(gainItemList);

        return gainItemList;
    }

    /**
     * 处理事件
     * @param _userData 用户数据
     * @param _eventRef 事件配置
     * @param _dealInfo 事件处理信息
     * @param _context  玩家上下文
     * @return 处理结果
     */
    @SuppressWarnings("unchecked")
    public ResultOne<CommonEvent_DoneInfo> dealEvent(NPUSUserData _userData, _ARefCommonEvent _eventRef, byte[] _dealInfo, NPPlayerContext _context)
    {
        return _dealInfo(_userData, (T) _eventRef, _dealInfo, _context);
    }

    /**
     * 获取默认奖励列表
     * @param _detailRef 事件详细配置
     * @return 默认奖励列表
     */
    @SuppressWarnings("unchecked")
    public List<NPCommonCostItem> getCmdRewardList(_ARefCommonEvent _detailRef)
    {
        return _getCmdRewardList((T) _detailRef);
    }

    /**
     * 处理事件
     * @param _userData 用户数据
     * @param _eventRef 事件配置
     * @param _dealInfo 事件处理信息
     * @param _context  玩家上下文
     * @return 处理结果
     */
    protected abstract ResultOne<CommonEvent_DoneInfo> _dealInfo(NPUSUserData _userData, T _eventRef, byte[] _dealInfo, NPPlayerContext _context);

    /**
     * 获取默认奖励列表
     * @param _detailRef 事件详细配置
     * @return 默认奖励列表 可能为空
     */
    protected abstract List<NPCommonCostItem> _getCmdRewardList(T _detailRef);
}
