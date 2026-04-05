package NPUSServer.NPUSUserMgr.UserComp.BagItemComp;

import Common.ActivityEnum.EActivityState;
import Common.MailObj.Mail_Data;
import CommonEnum.ESpecialItemType;
import MJLog.MJLog;
import NPCommon.CommonObj.NPItemCostCollector_nosafe;
import NPCommon.Context._IContext;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ELogItem_Type;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.NPCommon_BagItemInfo;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.Activity.RefActivity;
import NPGameRes.Refs.Activity.RefActivityBagItem;
import NPGameRes.Refs.BagItem.RefBagItem;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CONSUME_BAG_ITEM;
import NPUSServer.Common.ItemAcquisitionMonitor;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.SpecialItemComponent.Dealer.SpecialItemDealer_PaidVoucher;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_006_BagItemOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerBagItemBO;
import USLOGDB.Bo.LogBagItemBO;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

/*************************
 * 玩家背包系统的组件
 * @author mj
 *
 */
public class BagItemComponent extends _ANPUserComponent implements _IUserItemBasicDealer, _IHandlerHolder
{
    /**
     * 背包物品信息存储队列
     */
    private List<BagItemInfo> _m_alBagItemInfoList;
    /**
     * 方便Id检索的背包物品信息存储数据集
     */
    private Map<Long, BagItemInfo> _m_htBagItemTable;

    public BagItemComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.BAG_ITEM);

        _m_alBagItemInfoList = new ArrayList<>();
        _m_htBagItemTable = new HashMap<>();
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerBagItemBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerBagItemBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUserData().getUSServer(), "Can not load Bag Item Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerBagItemBO> _list)
            {
                _initBo(_list);
            }
        });
    }

    //获取需要依赖的加载组件项，无依赖则返回null
    @Override
    public ENPPlayerCompType[] getDependCompList()
    {
        return null;
    }

    @Override
    public void onInited()
    {
        getUSServer().getCommActivityMgr().activityStateChg.addHandler(this, new HandlerTwo<_AActivityBase, EActivityState>()
        {
            @Override
            public void handle(_AActivityBase _activity, EActivityState _state)
            {
                getUserData().lockUser();
                try
                {
                    Map<Long, NPItemCostCollector_nosafe> collectorMap = new HashMap<>();
                    NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_ITEM_EXPIRED);

                    for (BagItemInfo bagItemInfo : _m_alBagItemInfoList)
                    {
                        if (bagItemInfo == null)
                            continue;

                        //过期处理
                        bagItemInfo.checkExpire(_activity.getInstanceId(), _state, collectorMap, context);
                    }

                    if (collectorMap.isEmpty())
                        return;

                    //构造邮件数据并发送
                    _sendExpiredActivityItemMail(collectorMap, context);
                } finally
                {
                    getUserData().unlockUser();
                }
            }
        });

        //检查所有活动物品过期
        checkAllActivityItemExpire();
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
        getUSServer().getCommActivityMgr().activityStateChg.clear(this);
    }

    //////////////////////////////
    // ItemDealer处理部分
    //////////////////////////////

    /**********
     * 对应处理的类型
     * @return
     */
    public ENPItemType getItemType()
    {
        return ENPItemType.BAG_ITEM;
    }


    /**
     * 获取背包物品数量
     * @param _itemId
     * @return
     */
    public long getItemCount(long _itemId)
    {
        getUserData().lockUser();

        try
        {
            BagItemInfo info = getBagItem(_itemId);
            if (null == info)
                return 0l;

            return info.getItemCount();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取背包物品总获取数量
     * @param _itemId
     * @return
     */
    public long getTotalGainItemCount(long _itemId)
    {
        getUserData().lockUser();

        try
        {
            BagItemInfo info = getBagItem(_itemId);
            if (null == info)
                return 0l;

            return info.getTotalGainCount();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取背包物品总消耗数量
     * @param _itemId
     * @return
     */
    public long getTotalConsumeItemCount(long _itemId)
    {
        getUserData().lockUser();

        try
        {
            BagItemInfo info = getBagItem(_itemId);
            if (null == info)
                return 0l;

            return info.getTotalConsumeCount();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /***************
     * 是否有足够的数量
     * @param _itemId
     * @param _count
     * @return
     */
    public boolean hasItem(long _itemId, long _count)
    {
        return hasBagItem(_itemId, _count);
    }

    /****************
     * 初始化的获取物品处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        gainBagItem(_itemId, _count, false, _context);
    }

    /****************
     * 获取物品的处理，返回值表示是否触发gameevent事件
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        gainBagItem(_itemId, _count, _isNotMerge, _context);
    }

    /****************
     * 消耗物品的处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return spendBagItem(_itemId, _count, _context);
    }

    /**
     * 记录日志
     */
    public void  logItem(long _subId, long _oldCount, long _newCount, ELogItem_Type _logType, _IContext _context)
    {
        BM bmObj = getUSServer().getBM();

        LogBagItemBO bo = new LogBagItemBO();
        bo.setCid(bmObj, getUserData().getCid());
        bo.setLogType(bmObj, _logType.ordinal());
        bo.setOriginValue(bmObj, _oldCount);
        bo.setFinalValue(bmObj, _newCount);
        bo.setChgValue(bmObj, _newCount - _oldCount);
        bo.setSubId(bmObj, _subId);
        CommLogDB.log(bmObj, bo, _context);
    }

    //////////////////////////////
    // ItemDealer处理部分结束
    //////////////////////////////

    private void _initBo(List<PlayerBagItemBO> _list)
    {
        for (int i = 0; i < _list.size(); i++)
        {
            PlayerBagItemBO bo = _list.get(i);
            if (null == bo)
                continue;

            RefBagItem ref = RefBagItem.getMgr().get(bo.getItemId());
            if (null == ref)
            {
                USLog.error(getUserData().getUSServer(), "Can not get Bag Item Ref from db[refId:" + bo.getItemId() + "]");
                continue;
            }

            BagItemInfo info = new BagItemInfo(this, bo, ref);
            _m_alBagItemInfoList.add(info);
            _m_htBagItemTable.put(bo.getItemId(), info);
        }

        setInited();
    }

    /////////////////////////////////////// 组件方法 ///////////////////////////////////////

    /**
     * 初始化数据协议
     * @param _list
     */
    public void makeGetBagItemList(ArrayList<NPCommon_BagItemInfo> _list)
    {
        getUserData().lockUser();

        try
        {
            for (int i = _m_alBagItemInfoList.size() - 1; i >= 0; i--)
            {
                BagItemInfo info = _m_alBagItemInfoList.get(i);
                if (null == info)
                    continue;

                //物品数量为0则跳过不下发，但是不删除数据
                if (info.getItemCount() == 0)
                    continue;

                _list.add(info.toProto());
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 获取背包物品对象
     * @param _itemId
     * @return
     */
    public BagItemInfo getBagItem(long _itemId)
    {
        getUserData().lockUser();

        try
        {
            return _m_htBagItemTable.get(_itemId);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查物品存在
     * @param _itemId
     * @param _itemCount
     * @return
     */
    public boolean hasBagItem(long _itemId, long _itemCount)
    {
        getUserData().lockUser();

        try
        {
            BagItemInfo info = getBagItem(_itemId);
            if (null == info)
                return false;

            return info.getItemCount() >= _itemCount;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 增加背包物品
     * @param _itemId
     * @param _itemCount
     * @param _context
     * @return
     */
    public long gainBagItem(long _itemId, long _itemCount, boolean _isNotMerge, NPPlayerContext _context)
    {
        RefBagItem refBagItem = RefBagItem.getMgr().get(_itemId);
        if (refBagItem == null)
            return 0L;

        if (_itemCount <= 0)
            return 0L;

        getUserData().lockUser();

        try
        {
        	long oldCount = 0;

            long activityInstanceId = 0L;
            //查询是否有活动相关配置
            RefActivityBagItem refActivityBagItem = refBagItem.relativeActivityBagItemRef;
            if (refActivityBagItem != null)
            {
                _AActivityBase activity = getUSServer().getCommActivityMgr().lookupOneActivityByActivityId(refActivityBagItem.activity_id);
                if (activity != null)
                {
                    activityInstanceId = activity.getInstanceId();
                } else
                {
                    USLog.warn(getUSServer(), "player gain bag item, when activity not exist, itemId:{} count:{} activityId:{} cid:{}",
                            _itemId, _itemCount, refActivityBagItem.activity_id, getUserData().getCid());

                    // 记录无效丢失日志 (action 3)
                    MJLog.logItemChg(getUserData(), ENPItemType.BAG_ITEM, _itemId, 3,
                            0L, _itemCount, 0L, _context.getContextId());

                    return 0L;
                }
            }

            BagItemInfo info = getBagItem(_itemId);
            if (null == info)
            {
                RefBagItem ref = RefBagItem.getMgr().get(_itemId);
                if (null == ref)
                {
                    USLog.error(getUSServer(), "Can not find bagitem ref[" + _itemId + "]!!!");
                    return 0L;
                }

                BM bmObj = getUSServer().getBM();

                //新增数据
                PlayerBagItemBO bo = new PlayerBagItemBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setItemId(bmObj, _itemId);
                bo.setItemCount(bmObj, _itemCount);
                bo.setLastGetTimeS(bmObj, CommonFunc.getNowTimeSec());
                bo.setLastClickTimeS(bmObj, 0);
                bo.setNewGetTimeS(bmObj, CommonFunc.getNowTimeSec());
                bo.setRelativeActivityInstanceId(bmObj, activityInstanceId);
                bo.insert(bmObj);

                info = new BagItemInfo(this, bo, ref);

                _m_alBagItemInfoList.add(info);
                //添加到数据集合
                _m_htBagItemTable.put(_itemId, info);
            } else
            {
            	oldCount = info.getItemCount();

                info.addCount(_itemCount, activityInstanceId);
            }

            //推送协议
            getUserData().pushMsgToGC(US2GCWriter_006_BagItemOp.make_050_PushBagItemInfo(info));

            //放入数据
            _context.collectItem(getItemType(), _itemId, _itemCount, _isNotMerge);

            //日志
            logItem(_itemId, oldCount, info.getItemCount(), ELogItem_Type.GAIN, _context);
            MJLog.logItemChg(getUserData(), ENPItemType.BAG_ITEM, _itemId, 1,
                    oldCount, _itemCount, info.getItemCount(), _context.getContextId());

            //道具获取途径监控
            ItemAcquisitionMonitor.checkVoucherItemGain(_itemId, _itemCount, info.getItemCount(),
                    _context, getUserData(), getUSServer().getDDAlert());

            return _itemCount;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 扣除物品
     * @param _itemId
     * @param _itemCount
     * @param _context
     * @return
     */
    public boolean spendBagItem(long _itemId, long _itemCount, NPPlayerContext _context)
    {
        if (_itemCount < 0)
            return false;

        if (_itemCount == 0)
            return true;

        getUserData().lockUser();

        try
        {
            BagItemInfo info = getBagItem(_itemId);
            if (null == info || info.getItemCount() < _itemCount)
                return false;

            long oldCount = info.getItemCount();

            info.spendCount(_itemCount, _context);

            //推送协议
            if (0 == info.getItemCount())
                getUserData().pushMsgToGC(US2GCWriter_006_BagItemOp.make_051_PushRemoveBagItem(_itemId));
            else
                getUserData().pushMsgToGC(US2GCWriter_006_BagItemOp.make_050_PushBagItemInfo(info));

            //日志
            logItem(_itemId, oldCount, info.getItemCount(), ELogItem_Type.CONSUME, _context);
            MJLog.logItemChg(getUserData(), ENPItemType.BAG_ITEM, _itemId, 2,
                    oldCount, _itemCount, info.getItemCount(), _context.getContextId());

            if (RefGeneral.Ref().voucher_item_bag_item_id == _itemId)
            {
                SpecialItemDealer_PaidVoucher dealer =
                        getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.PAID_VOUCHER, SpecialItemDealer_PaidVoucher.class);
                if (dealer != null)
                {
                    dealer.spendItem(_itemCount, _context);
                }
            }

            getUserData().onLogicEvent(new Event_P_CONSUME_BAG_ITEM(_context, _itemId, _itemCount));

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 强制扣除物品，支持扣到负数（仅用于GM命令）
     *
     * 功能：
     * 1. 支持扣除背包里的道具
     * 2. 支持扣到负数
     *
     * @param _itemId 物品ID
     * @param _itemCount 扣除数量
     * @param _context 上下文
     * @return 是否成功
     */
    public boolean gmSpendItem(long _itemId, long _itemCount, NPPlayerContext _context)
    {
        if (_itemCount <= 0)
            return false;

        RefBagItem refBagItem = RefBagItem.getMgr().get(_itemId);
        if (refBagItem == null)
            return false;

        getUserData().lockUser();

        try
        {
            BagItemInfo info = getBagItem(_itemId);
            long oldCount = 0;

            // 如果物品不存在，需要先创建
            if (null == info)
            {
                RefBagItem ref = RefBagItem.getMgr().get(_itemId);
                if (null == ref)
                {
                    USLog.error(getUSServer(), "BagItemComponent.forceSpendBagItem - ref not found: itemId={}, cid={}",
                               _itemId, getUserData().getCid());
                    return false;
                }

                BM bmObj = getUSServer().getBM();

                int nowTimeSec = CommonFunc.getNowTimeSec();

                // 新增数据，初始数量为0
                PlayerBagItemBO bo = new PlayerBagItemBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setItemId(bmObj, _itemId);
                bo.setItemCount(bmObj, -_itemCount);
                bo.setLastGetTimeS(bmObj, nowTimeSec);
                bo.setLastClickTimeS(bmObj, 0);
                bo.setNewGetTimeS(bmObj, nowTimeSec);
                bo.setRelativeActivityInstanceId(bmObj, 0);
                bo.insert(bmObj);

                info = new BagItemInfo(this, bo, ref);

                _m_alBagItemInfoList.add(info);
                _m_htBagItemTable.put(_itemId, info);
            }
            else
            {
                oldCount = info.getItemCount();
                // 调用强制扣除方法，允许扣到负数
                info.gmConsumeCount(_itemCount, _context);
            }

            // 推送协议（即使数量为负数也推送物品信息）
            getUserData().pushMsgToGC(US2GCWriter_006_BagItemOp.make_050_PushBagItemInfo(info));

            // 日志
            logItem(_itemId, oldCount, info.getItemCount(), ELogItem_Type.CONSUME, _context);

            MJLog.logItemChg(getUserData(), ENPItemType.BAG_ITEM, _itemId, 2,
                    oldCount, _itemCount, info.getItemCount(), _context.getContextId());

            if (RefGeneral.Ref().voucher_item_bag_item_id == _itemId)
            {
                SpecialItemDealer_PaidVoucher dealer =
                        getUserData().getSpecialItemComponent().getDealer(ESpecialItemType.PAID_VOUCHER, SpecialItemDealer_PaidVoucher.class);
                if (dealer != null)
                {
                    dealer.spendItem(_itemCount, _context);
                }
            }

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置物品最后一次点击时间，用于判断new标签
     * @param _itemId
     * @return
     */
    public void refreshItemClickTime(long _itemId)
    {
        getUserData().lockUser();

        try
        {
            BagItemInfo info = getBagItem(_itemId);
            if (null == info)
                return;

            info.refreshLastClickTimeS();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置背包物品数量
     * @param _itemId    物品id
     * @param _itemCount 数量
     * @param _context   上下文
     */
    public void setItem(long _itemId, int _itemCount, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            long oldCount = 0;

            BagItemInfo info = getBagItem(_itemId);
            if (null == info)
            {
                RefBagItem ref = RefBagItem.getMgr().get(_itemId);
                if (null == ref)
                {
                    USLog.error(getUSServer(), "Can not find bagitem ref[" + _itemId + "]!!!");
                    return;
                }

                BM bmObj = getUSServer().getBM();

                //新增数据
                PlayerBagItemBO bo = new PlayerBagItemBO();
                bo.setCid(bmObj, getUserData().getCid());
                bo.setItemId(bmObj, _itemId);
                bo.setItemCount(bmObj, _itemCount);
                int nowTimeSec = CommonFunc.getNowTimeSec();
                bo.setLastGetTimeS(bmObj, nowTimeSec);
                bo.setLastClickTimeS(bmObj, 0);
                bo.setNewGetTimeS(bmObj, nowTimeSec);
                bo.insert(bmObj);

                info = new BagItemInfo(this, bo, ref);

                _m_alBagItemInfoList.add(info);
                //添加到数据集合
                _m_htBagItemTable.put(_itemId, info);
            } else
            {
                oldCount = info.getItemCount();

                info.setCount(_itemCount);
            }

            //推送协议
            getUserData().pushMsgToGC(US2GCWriter_006_BagItemOp.make_050_PushBagItemInfo(info));

            //日志
            logItem(_itemId, oldCount, _itemCount, ELogItem_Type.SET, _context);

            long exchange = info.getItemCount() - oldCount;
            int action = exchange >= 0 ? 1 : 2;
            MJLog.logItemChg(getUserData(), ENPItemType.BAG_ITEM, _itemId, action,
                    oldCount, Math.abs(exchange), info.getItemCount(), _context.getContextId());
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 清除背包物品
     * @param _itemId
     * @param _context
     */
    public void clearItemCount(long _itemId, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            if (_itemId > 0)
            {
                BagItemInfo info = _m_htBagItemTable.get(_itemId);
                if (null != info)
                {
                    info.setCount(0);

                    //推送协议
                    getUserData().pushMsgToGC(US2GCWriter_006_BagItemOp.make_050_PushBagItemInfo(info));
                }
            } else
            {
                for (int i = 0; i < _m_alBagItemInfoList.size(); i++)
                {
                    BagItemInfo info = _m_alBagItemInfoList.get(i);
                    if (null == info)
                        continue;

                    info.setCount(0);

                    //推送协议
                    getUserData().pushMsgToGC(US2GCWriter_006_BagItemOp.make_050_PushBagItemInfo(info));
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查所有活动物品过期
     */
    private void checkAllActivityItemExpire()
    {
        getUserData().lockUser();

        try
        {
            Map<Long, NPItemCostCollector_nosafe> collectorMap = new HashMap<>();
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ACTIVITY_ITEM_EXPIRED);

            for (BagItemInfo bagItemInfo : _m_alBagItemInfoList)
            {
                if (bagItemInfo == null || bagItemInfo.getRelativeActivityInstanceId() == 0)
                    continue;

                EActivityState state = EActivityState.CAN_DISCARD;

                _AActivityBase activity = getUSServer().getCommActivityMgr().lookupActivity(bagItemInfo.getRelativeActivityInstanceId());
                if (activity != null)
                    state = activity.getMachine().getCurState().getStateType();

                //过期处理
                bagItemInfo.checkExpire(bagItemInfo.getRelativeActivityInstanceId(), state, collectorMap, context);
            }

            if (collectorMap.isEmpty())
                return;

            _sendExpiredActivityItemMail(collectorMap, context);
        } finally
        {
            getUserData().unlockUser();
        }

    }

    /**
     * 发送过期活动物品邮件
     * @param _collectorMap
     * @param _context
     */
    private void _sendExpiredActivityItemMail(Map<Long, NPItemCostCollector_nosafe> _collectorMap, NPPlayerContext _context)
    {
        _collectorMap.forEach((_activityId1, _collector) ->
        {
            RefActivity refActivity = RefActivity.getMgr().get(_activityId1);

            //构造邮件数据
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(RefGeneral.Ref().activity_bag_item_expired_mail_id);
            if (refActivity != null)
            {
            	mailData.addContentReplace(refActivity.name);
            }
            mailData.getItemList().getItemList().addAll(_collector.getItemListP());

            getUserData().getMailComponent().addMail(mailData, _context);
        });
    }
}
