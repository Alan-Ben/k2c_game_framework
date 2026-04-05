package NPUSServer.NPUSUserMgr.UserComp.ExpiredItemComp;

import ALBasicCommon.ALBasicCommonFun;
import Common.MailEnum.EMailExtType;
import Common.MailObj.Mail_Data;
import NPCommon.Enum.NPCommonEnum;
import NPCommon.NPCommon_ItemInfo;
import NPCommon.NPCommon_ItemList;
import NPCommon.NPLogDB.CommLogDB;
import NPEnum.ENPGameEvent;
import NPEnum.ENPTimeAddType;
import NPEnum.ENpLogType;
import NPGameRes.Refs.Player._ARefExpiredItem;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUSUserMgr.UserComp._ITickableComponent;
import NPUSServer.USLog;
import USLOGDB.Bo.LogExpiredItemBO;

import java.util.ArrayList;
import java.util.List;

/**
 * 时效饰品组件模板
 * @param <R> 配置数据对象
 * @param <T> 时效饰品数据对象
 */
public abstract class _AExpiredItemComponent<R extends _ARefExpiredItem, T extends _AExpiredItemInfo<R>>
        extends _ANPUserComponent implements _IUserItemBasicDealer, _ITickableComponent
{
    private List<T> _m_expiredItemList;

    public _AExpiredItemComponent(NPUSUserData _userData, NPCommonEnum.ENPPlayerCompType _userComponent)
    {
        super(_userData, _userComponent);

        _m_expiredItemList = new ArrayList<>();
    }

    /**
     * 获取时效饰品数据对象列表
     * @return 时效饰品数据对象列表
     */
    public List<T> getItemList()
    {
        getUserData().lockUser();
        try
        {
            return new ArrayList<>(_m_expiredItemList);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /*************
     * 获取数量
     * @param _itemId
     * @return
     */
    public long getItemCount(long _itemId)
    {
        T item = _lookupItem(_itemId);
        if (null == item)
            return 0;

        return item.enable() ? 1 : 0;
    }

    /**
     * 是否有足够的数量
     * @param _itemId
     * @param _count
     * @return
     */
    public boolean hasItem(long _itemId, long _count)
    {
        return hasExpiredItem(_itemId, _count);
    }

    /**
     * 初始化的获取物品处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void initGainItem(long _itemId, long _count, NPPlayerContext _context)
    {
        addOrChgItemTime(_itemId, (int) _count, false, _context);
    }

    /**
     * 获取物品的处理，返回值表示是否触发gameevent事件
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public void gainItem(long _itemId, long _count, boolean _isNotMerge, NPPlayerContext _context)
    {
        addOrChgItemTime(_itemId, (int) _count, true, _context);
    }

    /**
     * 消耗物品的处理
     * @param _itemId
     * @param _count
     * @param _context
     * @return
     */
    public boolean spendItem(long _itemId, long _count, NPPlayerContext _context)
    {
        return reduceItemTime(_itemId, (int) _count, _context);
    }

    /**
     * 查找时效饰品数据对象
     * @param _refId 配置id
     * @return
     */
    protected T _lookupItem(long _refId)
    {
        getUserData().lockUser();
        try
        {
            for (T item : _m_expiredItemList)
            {
                if (item.getRefId() == _refId)
                {
                    return item;
                }
            }
            return null;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 添加时效饰品数据对象
     * @param _itemInfo 时效饰品数据对象
     */
    protected void _addItemToList(T _itemInfo)
    {
        getUserData().lockUser();
        try
        {
            _m_expiredItemList.add(_itemInfo);
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 创建并添加时效饰品数据对象
     * @param _ref          配置数据对象
     * @param _expiredTimeS 过期时间
     * @param _context      上下文
     * @return 时效饰品数据对象
     */
    protected T _createAndAddItem(R _ref, int _expiredTimeS, NPPlayerContext _context)
    {
        //创建时效饰品数据对象
        T info = _createExpiredItem(_ref, _expiredTimeS);
        //添加到列表
        _addItemToList(info);

        //新增时效饰品时的处理
        onExpiredItemAdd(info);

        return info;
    }

    /**
     * 修改时效饰品有效时间，如果不存在，则自动添加
     * @param _refId            配置id
     * @param _availableTimeSec 有效时长
     * @param _context          上下文
     */
    public void addOrChgItemTime(long _refId, int _availableTimeSec, boolean _isNotMerge, NPPlayerContext _context)
    {
        //查找配置数据对象
        R ref = lookupRef(_refId);
        if (ref == null)
        {
            USLog.error(getUSServer(), "_AExpiredItemComponent addOrChgItemTime ref is null, cid:{} itemType:{} refId:{} availableTimeSec:{}",
                    getUserData().getCid(), getItemType(), _refId, _availableTimeSec);
            return;
        }

        getUserData().lockUser();
        try
        {
            //查找时效饰品数据对象
            T info = _lookupItem(ref.Id());

            //无数据，则添加数据后直接返回
            if (null == info)
            {
                if (_availableTimeSec <= 0)
                {
                    info = _createAndAddItem(ref, 0, _context);
                } else
                {
                    info = _createAndAddItem(ref, ALBasicCommonFun.getNowTime() + _availableTimeSec, _context);
                }

                //添加到结果集
                _context.collectItem(getItemType(), ref.Id(), _availableTimeSec, _isNotMerge);

                //日志数据
                _log(ENpLogType.ADD, info, _context);
            } else
            {
                //添加时间，需要根据配置进行添加
                if (info.getExpireTimeSec() <= 0)
                {
                    //本来就无限则不可添加
                } else
                {
                    //根据时间类型进行不同的处理
                    if (_availableTimeSec <= 0)
                    {
                        //设置无限时长
                        info.chgExpireTimeSec(0);
                        //添加到结果集
                        _context.collectItem(getItemType(), ref.Id(), _availableTimeSec, _isNotMerge);
                    } else if (info.getRef().getAddType() == ENPTimeAddType.SET)
                    {
                        //设置的时候只会替换为更久的
                        //计算当前要设置的结束时间
                        int newExpireTimeS = ALBasicCommonFun.getNowTime() + _availableTimeSec;
                        //如果新时间更久则设置
                        if (newExpireTimeS > info.getExpireTimeSec())
                        {
                            info.chgExpireTimeSec(newExpireTimeS);
                            //添加到结果集
                            _context.collectItem(getItemType(), ref.Id(), _availableTimeSec, _isNotMerge);
                        }
                    } else
                    {
                        int curExpireTimeS = info.getExpireTimeSec();
                        int nowTimeS = ALBasicCommonFun.getNowTime();

                        //最少是当前时间
                        if (curExpireTimeS < nowTimeS)
                            curExpireTimeS = nowTimeS;

                        //累加
                        info.chgExpireTimeSec(curExpireTimeS + _availableTimeSec);
                        //添加到结果集
                        _context.collectItem(getItemType(), ref.Id(), _availableTimeSec, _isNotMerge);
                    }
                }
                
                //再次获得的处理
                info._onGainAgain();
                
                //数据变更处理
                onExpiredItemChg(info);

                //日志数据
                _log(ENpLogType.SET, info, _context);
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 减少时效饰品有效时间
     * @param _refId       配置id
     * @param _reduceTimeS 减少的时间
     * @param _context     上下文
     */
    public boolean reduceItemTime(long _refId, int _reduceTimeS, NPPlayerContext _context)
    {
        //合法性判断, 如果时间小于等于0, 则直接返回错误
        if (_reduceTimeS <= 0)
            return false;

        getUserData().lockUser();
        try
        {
            //查找时效饰品数据对象
            T info = _lookupItem(_refId);
            if (null == info)
                return false;

            //本来就无限则不需要去除
            if (info.getExpireTimeSec() <= 0)
                return true;

            //计算实际截止时间
            int finalExpireTime = info.getExpireTimeSec() - _reduceTimeS;
            int nowS = ALBasicCommonFun.getNowTime();

            //如果扣除之后时间在之前，表示无法扣除
            if (finalExpireTime < nowS)
                return false;

            //设置时间并返回
            info.chgExpireTimeSec(finalExpireTime);

            //数据变更处理
            onExpiredItemChg(info);

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    @Override
    public void tick1Sec()
    {
        getUserData().lockUser();
        try
        {
            NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ITEM_EXPIRED_MAIL_SEND);
            for (T item : _m_expiredItemList)
            {
                if (item == null)
                    continue;

                //判断过期标识是否切换，发送邮件
                if (item.checkNeedSendMailTagChg())
                {
                    sendExpiredMail(item, context);
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 发送过期邮件
     * @param _context
     */
    public void sendExpiredMail(T _item, NPPlayerContext _context)
    {
        //构造邮件数据
        Mail_Data mailData = new Mail_Data();
        mailData.setMailRefId(RefGeneral.Ref().item_expired_mail_id);
        mailData.setExType(EMailExtType.TEST_ITEM_LIST.ordinal());
        NPCommon_ItemList testItemList = new NPCommon_ItemList();
        testItemList.addItemList(new NPCommon_ItemInfo(getItemType().ordinal(), _item.getRefId(), 1, null));
        mailData.setExData(testItemList.makePackage());
        MailSystem.addMail(getUSServer(), getUserData().getCid(), mailData, _context);
    }

    /**
     * 删除时效饰品数据对象
     */
    public void delItem(long _refId)
    {
        getUserData().lockUser();
        try
        {
            for (int i = _m_expiredItemList.size() - 1; i >= 0; i--)
            {
                T info = _m_expiredItemList.get(i);
                if (info == null)
                    continue;

                if (info.getRef().Id() == _refId)
                {
                    _m_expiredItemList.remove(i);
                    //删除Bo数据
                    info.del();
                    //返回消息
                    onExpiredItemDel(_refId);
                    break;
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 设置已查看
     * @param _refId 配置id
     */
    public void setItemViewed(long _refId)
    {
        getUserData().lockUser();

        try
        {
            T info = _lookupItem(_refId);
            //没有称号数据
            if (null == info)
            {
                USLog.warn(getUSServer(),
                        "_AExpiredItemComponent setItemViewed item not found,  cid:{} itemType:{} refId:{}",
                        getUserData().getCid(), getItemType(), _refId);
                return;
            }

            //设置已查看
            info.setViewed();
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 检查时效饰品是否可用
     * @param _refId 配置id
     * @return 是否可用
     */
    public boolean checkExpiredItemEnable(long _refId)
    {
        getUserData().lockUser();
        try
        {
            T info = _lookupItem(_refId);
            if (null == info)
                return false;

            //判断是否过期
            if (!info.enable())
                return false;

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 是否有时效饰品
     * @param _refId
     * @return
     */
    public boolean hasExpiredItem(long _refId, long _timeSec)
    {
        getUserData().lockUser();
        try
        {
            T info = _lookupItem(_refId);
            if (null == info)
                return false;

            //判断是否永久
            if (info.getExpireTimeSec() <= 0)
                return true;

            //判断是否判断永久。是的话，因为info不是永久，所以返回false
            if (_timeSec <= 0)
                return false;

            //判断是否有足够时间，时间差小于判断值则返回false
            int nowS = ALBasicCommonFun.getNowTime();
            if (info.getExpireTimeSec() - nowS < _timeSec)
                return false;

            return true;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 日志数据
     * @param _logType
     * @param _info
     * @param _context
     */
    protected void _log(ENpLogType _logType, T _info, NPPlayerContext _context)
    {
        LogExpiredItemBO logBo = new LogExpiredItemBO();
        logBo.setCid(getUserData().getUSServer().getBM(), getUserData().getCid());
        logBo.setLogType(getUserData().getUSServer().getBM(), _logType.ordinal());
        logBo.setItemType(getUserData().getUSServer().getBM(), getItemType().ordinal());
        logBo.setSubId(getUserData().getUSServer().getBM(), _info.getRefId());
        logBo.setDbid(getUserData().getUSServer().getBM(), _info.getDbId());
        logBo.setExpiredTs(getUserData().getUSServer().getBM(), _info.getExpireTimeSec());
        CommLogDB.log(getUserData().getUSServer().getBM(), logBo, _context);
    }

    /**
     * 查找时效饰品配置数据对象
     * @param _refId 配置id
     * @return 时效饰品配置数据对象
     */
    public abstract R lookupRef(long _refId);

    /**
     * 创建时效饰品数据对象
     * @param _ref          配置数据对象
     * @param _expiredTimeS 过期时间
     * @return 时效饰品数据对象
     */
    protected abstract T _createExpiredItem(R _ref, int _expiredTimeS);

    /**
     * 当时效饰品数据新增时的处理
     *
     * @param _info 时效饰品数据对象
     */
    public abstract void onExpiredItemAdd(T _info);

    /**
     * 当时效饰品数据变更时的处理
     *
     * @param _info 时效饰品数据对象
     */
    public abstract void onExpiredItemChg(T _info);

    /**
     * 当时效饰品数据删除时的处理
     * @param _refId 时效饰品id
     */
    public abstract void onExpiredItemDel(long _refId);
}
