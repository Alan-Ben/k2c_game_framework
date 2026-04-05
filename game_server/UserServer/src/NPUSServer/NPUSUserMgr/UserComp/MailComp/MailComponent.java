package NPUSServer.NPUSUserMgr.UserComp.MailComp;

import Common.MailObj.Mail_BriefInfo;
import Common.MailObj.Mail_Data;
import Common.MailObj.Mail_StatInfo;
import Common.NpServerObj.NpServerObj_PlatFormMail;
import Common.NpServerObj.NpServerObj_PlatFormMailText;
import NPCommon.DB.BM.BM;
import NPCommon.DB._ASelectCallback;
import NPCommon.Enum.NPCommonEnum.ENPPlayerCompType;
import NPCommon.LazyTaskDealer.LazyTaskDealer;
import NPCommon.NPCommon_ItemList;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.Delegate._IHandlerHolder;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPEnum.ENpLogType;
import NPGameRes.LogicEvent._ALogicEventBase;
import NPGameRes.Refs.Mail.RefMail;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_S_ALL_SERVER_MAIL_CHG;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUSUserMgr.ItemDealer._IUserItemBasicDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp._ANPUserComponent;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;
import NPUSServer.USLog;
import USDB.Bo.PlayerMailBO;
import USLOGDB.Bo.LogMailBO;

import java.util.*;
import java.util.function.Predicate;

public class MailComponent extends _ANPUserComponent implements _IHandlerHolder, _IUserItemBasicDealer
{
	//邮件列表
    private final List<MailInfo> _m_alMailInfoList = new ArrayList<>();
    private final Map<Long, MailInfo> _m_mailMap = new HashMap<>();

    private LazyTaskDealer _m_lazyTaskDealer;

    public MailComponent(NPUSUserData _userData)
    {
        super(_userData, ENPPlayerCompType.MAIL_COMP);

        _m_lazyTaskDealer = new LazyTaskDealer(() ->
                _checkRemoveExceedMails(false, NPPlayerContext.createNew(ENPGameEvent.REFRESH_MAIL)), 1000);
    }

    @Override
    protected void _init()
    {
        getUSServer().getBM().getBM(PlayerMailBO.class).findAll("cid", getUserData().getCid(), new _ASelectCallback<List<PlayerMailBO>>()
        {
            @Override
            public void dealFail()
            {
                USLog.error(getUSServer(), "Can not load Mail Data[cid:" + getUserData().getCid() + "]");
                getUserData().setDataLoadFail();
            }

            @Override
            public void dealSuc(List<PlayerMailBO> _list)
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
        //删除过期邮件
        removeExpiredMails(false, getUserData().getPlayerInitContext());//删除过期邮件
        //最后删除超限邮件，确保不会误删邮件
        _checkRemoveExceedMails(true, getUserData().getPlayerInitContext());//删除超出数量限制的邮件

        //初始化处理一次邮件
        dealServerMail(getUserData().getPlayerInitContext());

        //Event_S_ALL_SERVER_MAIL_CHG 全服邮件变更
        getUserData().getEventHandlerMgr().regHandler(Event_S_ALL_SERVER_MAIL_CHG.ID, this, new HandlerTwo<_ALogicEventBase, NPUSUserData>()
        {
            @Override
            public void handle(_ALogicEventBase _event, NPUSUserData _userData)
            {
                if (!(_event instanceof Event_S_ALL_SERVER_MAIL_CHG))
                {
                    return;
                }
                
                Event_S_ALL_SERVER_MAIL_CHG event = (Event_S_ALL_SERVER_MAIL_CHG) _event;
                //当前记录的服务器mailId大于事件提供的id
                long maxServerMailId = getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.MAX_SERVER_MAIL_ID);
                if (maxServerMailId >= event.get_MAX_MAIL_ID())
                {
                    return;
                }
                
                NPPlayerContext eventContext = NPPlayerContext.createNew(ENPGameEvent.PHP_OPTION);
                dealServerMail(eventContext);
            }
        });
    }

    /**********************
     * 增加运营邮件
     * @param _formMail
     * @param _context
     */
    public void addFormMail(NpServerObj_PlatFormMail _formMail, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            //检查邮件是否过期，过期就直接失效
            int ExpiredTimeSec = CommonFunc.simpleDateFormatTimeSec(_formMail.getExpiredTime());
            if (ExpiredTimeSec < CommonFunc.getNowTimeSec())
            {
                return;
            }

            //把邮件添加到个人邮件列表中
            Mail_Data mailData = new Mail_Data();
            mailData.setMailRefId(_formMail.getMailRefId());
            NpServerObj_PlatFormMailText mailText =
                    CommonFunc.lookupPlatFormMailTextById(_formMail.getPlatformMailTextList()
                            , getUserData().getSdkInfo().language
                            , _formMail.getDefaultLang());
            if (mailText != null)
            {
                mailData.setTitle(mailText.getTitle());
                mailData.setContent(mailText.getContent());
            }
            //构造替换数据
            for(int i = 0; i < _formMail.getContentReplace().size(); i++)
            {
            	mailData.getContentReplace().add(_formMail.getContentReplace().get(i));
            }
            //构造附件
            NPCommon_ItemList npCommon_itemList = new NPCommon_ItemList();
            npCommon_itemList.getItemList().addAll(_formMail.getItemList());
            mailData.setItemList(npCommon_itemList);
            mailData.setCreateTimeSec(CommonFunc.simpleDateFormatTimeSec(_formMail.getSendTime()));
            mailData.setExpiredTimeSec(ExpiredTimeSec);
            mailData.setIsMustRead(true);
            mailData.setPhpMailId(_formMail.getPhpMailId());

            addMail(mailData, getUserData().getPlayerInitContext());
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /**
     * 处理全服邮件
     * @param _context NPPlayerContext
     */
    private void dealServerMail(NPPlayerContext _context)
    {
        getUserData().lockUser();
        
        try
        {
            //全服邮件容器
            ArrayList<NpServerObj_PlatFormMail> formMails = new ArrayList<>();

            //获取全部全服邮件并更新玩家最大的服务器ID
            long maxServerMailId = getUserData().getRecordComponent().getRecordCount(ENPPlayerRecordParam.MAX_SERVER_MAIL_ID);
            long maxMailId = getUserData().getUSServer().getAllServerMailTemplateMgr().lookupMailTemplateListByMailMaxId(maxServerMailId, formMails);
            getUserData().getRecordComponent().setGtRecord(ENPPlayerRecordParam.MAX_SERVER_MAIL_ID, maxMailId, _context);

            //处理全服邮件
            for (NpServerObj_PlatFormMail formMail : formMails)
            {
                if (formMail == null)
                {
                    continue;
                }
                //把邮件添加到个人邮件列表中
                Mail_Data mailData = new Mail_Data();
                mailData.setMailRefId(formMail.getMailRefId());
                NpServerObj_PlatFormMailText mailText =
                        CommonFunc.lookupPlatFormMailTextById(formMail.getPlatformMailTextList()
                                , getUserData().getSdkInfo().language
                                , formMail.getDefaultLang());
                if (mailText != null)
                {
                    mailData.setTitle(mailText.getTitle());
                    mailData.setContent(mailText.getContent());
                }
                //构造替换数据
                for(int i = 0; i < formMail.getContentReplace().size(); i++)
                {
                	mailData.getContentReplace().add(formMail.getContentReplace().get(i));
                }
                //构造附件
                NPCommon_ItemList npCommon_itemList = new NPCommon_ItemList();
                npCommon_itemList.getItemList().addAll(formMail.getItemList());
                mailData.setItemList(npCommon_itemList);
                mailData.setCreateTimeSec(CommonFunc.simpleDateFormatTimeSec(formMail.getSendTime()));
                mailData.setExpiredTimeSec(CommonFunc.simpleDateFormatTimeSec(formMail.getExpiredTime()));
                mailData.setIsMustRead(true);
                mailData.setPhpMailId(formMail.getPhpMailId());
                mailData.setMailRefId(formMail.getMailRefId());

                addMail(mailData, getUserData().getPlayerInitContext());
            }
        } 
        finally
        {
            getUserData().unlockUser();
        }
    }

    /***********
     * 玩家数据释放时候的处理，一般需要做关联处理。如注销监听等的处理
     */
    @Override
    public void dispose()
    {
        getUserData().getEventHandlerMgr().unregHandler(this);
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
        return ENPItemType.MAIL;
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
            //统计总数量
            int count = 0;
            MailInfo mailInfo = null;
            for (int i = 0; i < _m_alMailInfoList.size(); i++)
            {
                mailInfo = _m_alMailInfoList.get(i);
                if (null == mailInfo)
                    continue;

                if (mailInfo.getBo().getMailRefId() == _itemId)
                    count++;
            }

            return count;
        } 
        finally
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
        getUserData().lockUser();

        try
        {
            //统计总数量
            int count = 0;
            MailInfo mailInfo = null;
            for (int i = 0; i < _m_alMailInfoList.size(); i++)
            {
                mailInfo = _m_alMailInfoList.get(i);
                if (null == mailInfo)
                    continue;

                if (mailInfo.getBo().getMailRefId() == _itemId)
                    count++;

                //判断数量是否匹配
                if (count > 0 && count >= _count)
                    return true;
            }

            return false;
        } 
        finally
        {
            getUserData().unlockUser();
        }
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
        initAddMail(_itemId, _context);
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
        addSysMail(_itemId, _context);
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
        getUserData().lockUser();

        try
        {
            //统计总数量
            long count = _count;
            //0表示无限，设置为-1
            if (count == 0)
                count = -1;

            MailInfo mailInfo = null;
            for (int i = 0; i < _m_alMailInfoList.size(); i++)
            {
                mailInfo = _m_alMailInfoList.get(i);
                if (null == mailInfo)
                    continue;

                //判断Id是否匹配，是则删除
                if (mailInfo.getBo().getMailRefId() == _itemId)
                {
                    deleteMail(mailInfo, _context);
                    count--;
                }

                //判断数量是否有效
                if (count == 0)
                    return true;
            }

            //如果是无限扣除则返回成功，否则返回失败
            if (count < 0)
                return true;
            else
                return false;
        } finally
        {
            getUserData().unlockUser();
        }
    }

    //////////////////////////////
    // ItemDealer处理部分结束
    //////////////////////////////

    private void _initBo(List<PlayerMailBO> _list)
    {
        for (PlayerMailBO bo : _list)
        {
            RefMail ref;
            //配置邮件检查
            if (bo.getMailRefId() > 0)
            {
                ref = RefMail.getMgr().get(bo.getMailRefId());
                if (null == ref)
                {
                    USLog.error(getUSServer(), "Can not get mail ref for bo[cid:{},  mailId:{}]", getUserData().getCid(), bo.getMailRefId());
                    continue;
                }
            }
            //检查过期
            if (bo.getExpiredTs() > 0 && CommonFunc.getNowTimeSec() > bo.getExpiredTs())
            {
                bo.del(getUSServer().getBM());
                //记录日志
                logMail(ENpLogType.DEL, bo, getUserData().getPlayerInitContext());
                continue;
            }

            MailInfo info = new MailInfo(this, bo);
            //初始化数据
            info.initNew();

            //添加到队列
            _addMailToList(info);
        }

        //对邮件进行排序
        _sortMailList();

        setInited();
    }
    
    /*******
     * 邮件列表排序,按Uid从小到大排序，新邮件在后面
     */
    private void _sortMailList()
    {
        _m_alMailInfoList.sort(Comparator.comparingLong(o -> o.getUid()));
    }

    /**
     * 检查邮件数量上限
     * @return
     */
    private boolean _isNotExceed()
    {
    	return _m_alMailInfoList.size() <= RefGeneral.Ref().mail_max_num;
    }

    /******
     * 邮件加入邮件列表，建立监听
     * @param _info
     */
    private void _addMailToList(MailInfo _info)
    {
        _m_alMailInfoList.add(_info);
        _m_mailMap.put(_info.getUid(), _info);
    }

    /*****
     * 从列表中移除邮件
     * @param _info
     */
    private void _removeMailFromList(MailInfo _info)
    {
        _m_alMailInfoList.remove(_info);
        _m_mailMap.remove(_info.getUid());
    }

    /*****
     * 初始化行为的新增一封邮件
     * 这个处理不会处理数量限制
     * @param _context
     */
    public void initAddMail(long _mailId, NPPlayerContext _context)
    {
        PlayerMailBO bo = MailSystem.createSysMailBo(getUSServer(), getUserData().getCid(), _mailId);
        if (null == bo)
            return;

        //添加到本数据集
        MailInfo mailInfo = new MailInfo(this, bo);
        mailInfo.initNew();

        _addMailToList(mailInfo);
        logMail(ENpLogType.ADD, mailInfo.getBo(), _context);
    }

    /*****
     * 新增一封邮件
     * @param _context
     */
    public void addSysMail(long _mailId, NPPlayerContext _context)
    {
        PlayerMailBO bo = MailSystem.createSysMailBo(getUSServer(), getUserData().getCid(), _mailId);
        if (null == bo)
            return;

        _addMail(_context, bo);
    }

    /**
     * 新增一封邮件
     * @param _mailData
     * @param _context
     */
    public void addMail(Mail_Data _mailData, NPPlayerContext _context)
    {
        PlayerMailBO bo = MailSystem.createMailBoFromData(getUSServer(), getUserData().getCid(), _mailData);
        if (null == bo)
            return;

        //添加到本数据集
        _addMail(_context, bo);
    }

    /**
     * 添加邮件
     * @param _context
     * @param bo
     */
    private void _addMail(NPPlayerContext _context, PlayerMailBO bo)
    {
        //添加到本数据集
        MailInfo mailInfo = new MailInfo(this, bo);
        mailInfo.initNew();

        _addMailToList(mailInfo);

        getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_051_OnMailAdded(mailInfo));

        logMail(ENpLogType.ADD, mailInfo.getBo(), _context);

        //检查是否超出数量限制
        _m_lazyTaskDealer.setNeedDeal();
    }

    /*******
     * 移除超出数量限制的邮件
     * 自动删除邮件优先顺序：
     * 	1	已读>未读，已读包含已领取奖励的奖励邮件；
     * 	2	无奖励邮件>有奖励邮件；
     * 	3	非必读邮件> 必读邮件；
     * 	4	按照时间先后顺序删除，先发送的邮件先删；
     */
    private void _checkRemoveExceedMails(boolean _bInit, NPPlayerContext _context)
    {
        if (_isNotExceed())
            return;

        //先删已读邮件（无附件）
        removeMailPredicate(_bInit, _context, npMailInfo -> npMailInfo.getIsRead() && !npMailInfo.getHasItem());
        if (_isNotExceed())
            return;
        
        //删除已领取邮件
        removeMailPredicate(_bInit, _context, npMailInfo -> npMailInfo.getHasTaken());
        if (_isNotExceed())
            return;
        
        //删除其他已读邮件（有附件未领取）
        removeMailPredicate(_bInit, _context, npMailInfo -> npMailInfo.getIsRead());
        if (_isNotExceed())
            return;

        //删除无奖励邮件
        removeMailPredicate(_bInit, _context, npMailInfo -> !npMailInfo.getHasItem());
        if (_isNotExceed())
            return;

        //删除非必读邮件
        removeMailPredicate(_bInit, _context, npMailInfo -> !npMailInfo.getIsMustRead());
        if (_isNotExceed())
            return;

        //按顺序删除邮件
        removeMailPredicate(_bInit, _context, npMailInfo -> true);
    }

    /*****
     * 根据条件，删除超出数量限制的邮件。
     * @param _bInit
     * @param _predicate
     */
    private void removeMailPredicate(boolean _bInit, NPPlayerContext _context, Predicate<MailInfo> _predicate)
    {
    	getUserData().lockUser();

    	try
    	{
    		List<MailInfo> toRemoveList = null;
            for (MailInfo mailInfo : _m_alMailInfoList)
            {
            	if(null == mailInfo)
            		continue;
            	
            	//锁定邮件不删除
            	if(mailInfo.getIsLocked())
            		continue;
            	
                if (_predicate.test(mailInfo))
                {
                    if (null == toRemoveList)
                    {
                        toRemoveList = new ArrayList<>();
                    }
                    toRemoveList.add(mailInfo);
                    //已经没有超限了，停止遍历移除
                    if ((_m_alMailInfoList.size() - toRemoveList.size()) <= RefGeneral.Ref().mail_max_num)
                    {
                        break;
                    }
                }
            }

            if (null != toRemoveList)
            {
                for (MailInfo mailInfo : toRemoveList)
                {
                    _removeMailFromList(mailInfo);
                    mailInfo.discard(_context);
                    if (!_bInit)
                    {
                        getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_052_OnMailRemoved(mailInfo));
                    }
                }
            }
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 生成协议 Mail_StatInfo,邮件统计信息
     * @return
     */
    public Mail_StatInfo makeMailStatInfo()
    {
    	getUserData().lockUser();

    	try
    	{	
    		Mail_StatInfo proto = new Mail_StatInfo();
            proto.setTotalMailCount(_m_alMailInfoList.size());
            
            int unReadCount = 0;
            int unTakeCount = 0;
            for (MailInfo npMailInfo : _m_alMailInfoList)
            {
                if (!npMailInfo.getIsRead())
                {
                    unReadCount++;
                }
                if (!npMailInfo.getHasTaken() && npMailInfo.getHasItem())
                {
                    unTakeCount++;
                }
            }
            proto.setUnReadCount(unReadCount);
            proto.setUnTakeCount(unTakeCount);
            
            return proto;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /*****
     * 根据邮件id，获取单个邮件
     * @param _mailUid
     * @return
     */
    public MailInfo lookupMail(long _mailUid)
    {
    	getUserData().lockUser();

    	try
    	{
    		return _m_mailMap.get(_mailUid);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /****
     * 返还所有邮件列表
     * @return
     */
    public List<MailInfo> getMailList()
    {
    	getUserData().lockUser();

    	try
    	{
    		return new ArrayList<>(_m_alMailInfoList);
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**************
     * 获取邮件的附件
     * @return
     */
    public void takeMailAttach(long _mailId, NPPlayerContext _context)
    {
        getUserData().lockUser();

        try
        {
            for (MailInfo mailInfo : _m_alMailInfoList)
            {
                if (mailInfo.getMailRefId() == _mailId)
                {
                    //尝试获取附件
                    mailInfo.takeMailAttach(_context);
                }
            }
        } finally
        {
            getUserData().unlockUser();
        }
    }

    public void deleteMail(MailInfo npMailInfo, NPPlayerContext context)
    {
    	getUserData().lockUser();

    	try
    	{
    		if (!_m_alMailInfoList.remove(npMailInfo))
            {
                return;
            }
    		
            npMailInfo.discard(context);

            getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_052_OnMailRemoved(npMailInfo));
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 返回已收藏的邮件数量
     * @return
     */
    public int getLockedCount()
    {
    	getUserData().lockUser();

    	try
    	{
    		int count = 0;
            for (MailInfo npMailInfo : _m_alMailInfoList)
            {
                if (npMailInfo.getIsLocked())
                {
                    count++;
                }
            }
            
            return count;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /******
     *移除过期的邮件
     * @param _bNotifyClient //是否通知客户端删除
     * @param _context
     */
    public boolean removeExpiredMails(boolean _bNotifyClient, NPPlayerContext _context)
    {
    	getUserData().lockUser();

    	try
    	{
    		List<MailInfo> toRemoveList = null;
            //从小往大遍历
            for (MailInfo mailInfo : _m_alMailInfoList)
            {
                if (mailInfo.isExpired())
                {
                    if (null == toRemoveList)//延时创建列表，避免内存碎片
                    {
                        toRemoveList = new ArrayList<>();
                    }
                    toRemoveList.add(mailInfo);
                }
            }
            if (null != toRemoveList)
            {
                for (MailInfo mailInfo : toRemoveList)
                {
                    _removeMailFromList(mailInfo);
                    mailInfo.discard(_context);
                    
                    if (_bNotifyClient)
                    {
                        getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_052_OnMailRemoved(mailInfo));
                    }
                }
                return true;
            } 
            
            return false;
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /*******
     * 填充简要信息列表
     * @param _briefList
     */
    public void fillBriefList(ArrayList<Mail_BriefInfo> _briefList)
    {
    	getUserData().lockUser();

    	try
    	{
    		for (MailInfo mailInfo : _m_alMailInfoList)
            {
                _briefList.add(mailInfo.to_Mail_BriefInfo());
            }
    	}
    	finally
    	{
    		getUserData().unlockUser();
    	}
    }

    /**
     * 日志数据
     * @param _logType
     * @param _bo
     * @param _context
     */
    public void logMail(ENpLogType _logType, PlayerMailBO _bo, NPPlayerContext _context)
    {
        BM bmObj = getUSServer().getBM();

        LogMailBO logBo = new LogMailBO();
        logBo.setCid(bmObj, getUserData().getCid());
//        logBo.setMailUid(bmObj, _bo.getMailUid());
        logBo.setLogType(bmObj, _logType.ordinal());
        logBo.setMailRefId(bmObj, _bo.getMailRefId());
        logBo.setPhpMailId(bmObj, _bo.getPhpMailId());
        logBo.setExpiredTimeSec(bmObj, _bo.getExpiredTs());
        logBo.setCurMaxGMailId(bmObj, 0);
        logBo.setPlayerLanguage(bmObj, getUserData().getSdkInfo().language);
        CommLogDB.log(bmObj, logBo, _context);
    }
}
