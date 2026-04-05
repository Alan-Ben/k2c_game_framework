package NPUSServer.NPUSUserMgr.UserComp.MailComp;

import Common.MailObj.Mail_BriefInfo;
import Common.MailObj.Mail_DetailInfo;
import Common.MailObj.Mail_TitleInfo;
import NPCommon.DB.BM.BM;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.StringFunc;
import NPEnum.ENpLogType;
import NPGameRes.Refs.Mail.RefMail;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.GameSystem.MailSystem.MailSystem;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_009_MailOp;
import USDB.Bo.PlayerMailBO;

import java.nio.ByteBuffer;

/*****
 * 单个邮件类
 */
public class MailInfo
{
    private final MailComponent _m_compMailComponent;

    private PlayerMailBO _m_boMailBo;
    //静态数据
    private RefMail _m_rMailRef;
    private final NPCommon_ItemList _m_alItemList = new NPCommon_ItemList();
    
    public MailInfo(MailComponent _comp, PlayerMailBO _bo)
    {
        _m_compMailComponent = _comp;
        _m_boMailBo = _bo;
        _m_rMailRef = RefMail.getMgr().get(_bo.getMailRefId());
    }

    public MailComponent getComp()
    {
        return _m_compMailComponent;
    }

    public PlayerMailBO getBo()
    {
        return _m_boMailBo;
    }

    public long getUid()
    {
        return _m_boMailBo.getId();
    }

    public long getMailRefId()
    {
        return _m_boMailBo.getMailRefId();
    }

    public long getMailPhpId()
    {
        return _m_boMailBo.getPhpMailId();
    }

    /****
     * 返回是否已读取
     * @return
     */
    public boolean getIsRead()
    {
        return _m_boMailBo.getReadedTs() > 0;
    }

    /*****
     * 返回是否锁定
     * @return
     */
    public boolean getIsLocked()
    {
        return _m_boMailBo.getIsLocked();
    }

    /******
     * 返回是否有物品可以领取
     * @return
     */
    public boolean getHasItem()
    {
        return !_m_alItemList.getItemList().isEmpty();
    }

    /*****
     * 返还是否已领取
     * @return
     */
    public boolean getHasTaken()
    {
        return _m_boMailBo.getTakedTs() > 0;
    }

    /*****
     *返回是否已读完
     * @return
     */
    public boolean getIsReadOver()
    {
        return _m_boMailBo.getIsReadOver();
    }

    /******
     * 返回是否必读邮件
     * @return
     */
    public boolean getIsMustRead()
    {
        return _m_boMailBo.getIsMustRead();
    }
    
    /**
     * 返回截至时间
     * @return
     */
    public int getExpiredTs()
    {
    	return _m_boMailBo.getExpiredTs();
    }
    
    /******
     * 邮件是否过期
     * @return
     */
    public boolean isExpired()
    {
        return !getIsLocked() 
        		&& _m_boMailBo.getExpiredTs() > 0 && _m_boMailBo.getExpiredTs() <= CommonFunc.getNowTimeSec();
    }

    /*****
     *创建成功初始化
     */
    public void initNew()
    {
        if (_m_boMailBo.getAttachList() != null && _m_boMailBo.getAttachList().length > 0)
        {
            _m_alItemList.readPackage(ByteBuffer.wrap(_m_boMailBo.getAttachList()));
        }
    }

    /****
     * 生成Mail_BriefInfo协议
     */
    public Mail_BriefInfo to_Mail_BriefInfo()
    {
        Mail_BriefInfo proto = new Mail_BriefInfo();
        proto.setMailUid(getUid());
        proto.setGainTimeSec(_m_boMailBo.getCreatedTs());
        proto.setIsRead(getIsRead());
        proto.setHasTaken(getHasTaken());
        if (!_m_alItemList.getItemList().isEmpty())
        {
            proto.setItem(_m_alItemList.getItemList().get(0));
        }
        proto.setIsMustRead(_m_boMailBo.getIsMustRead());
        proto.setIsLocked(_m_boMailBo.getIsLocked());
        proto.setMailRefId(_m_boMailBo.getMailRefId());
        return proto;
    }

    /****
     * 生成Mail_TitleInfo协议
     */
    public Mail_TitleInfo to_Mail_TitleInfo()
    {
        Mail_TitleInfo proto = new Mail_TitleInfo();
        proto.setMailUid(getUid());
        proto.setSenderId(_m_boMailBo.getSenderId());
        proto.setMailRefId(_m_boMailBo.getMailRefId());
        proto.setTitle(_m_boMailBo.getTitle());
        proto.setEndTimeSec(_m_boMailBo.getExpiredTs());
        proto.setGainTimeSec(_m_boMailBo.getCreatedTs());
        proto.setIsLocked(_m_boMailBo.getIsLocked());
        proto.setIsRead(getIsRead());
        proto.setHasItem(getHasItem());
        proto.setHasTaken(getHasTaken());
        proto.setIsReadOver(_m_boMailBo.getIsReadOver());
        proto.setIsMustRead(_m_boMailBo.getIsMustRead());
        proto.getContentReplace().addAll(StringFunc.listStringFromString(_m_boMailBo.getContentReplace()));
        if(null != _m_boMailBo.getExTitleData())
        {
        	proto.setExTitleData(_m_boMailBo.getExTitleData());
        }
        
        return proto;
    }
    
    /****
     * 生成Mail_DetailInfo协议
     */
    public Mail_DetailInfo to_Mail_DetailInfo()
    {
        Mail_DetailInfo proto = new Mail_DetailInfo();
        proto.setMailUid(getUid());
        proto.setContent(_m_boMailBo.getContent());
        proto.getContentReplace().addAll(StringFunc.listStringFromString(_m_boMailBo.getContentReplace()));
        proto.getItemList().addAll(_m_alItemList.getItemList());
        proto.setExType(_m_boMailBo.getExDataType());
        proto.setExData(_m_boMailBo.getExData());
        
        return proto;
    }
    
    /****
     * 设置为已领取
     */
    public void markTaken()
    {
        BM bmObj = _m_compMailComponent.getUSServer().getBM();
        int nowTimeSec = CommonFunc.getNowTimeSec();

        _m_boMailBo.setTakedTs(bmObj, nowTimeSec);
        //邮件如果配置表配置的超时时间为-1表示无限，0表示无配置，默认按照7天处理
        int expiredTimeSec = _m_rMailRef == null ? 0 : _m_rMailRef.expired_secs;
        if (expiredTimeSec > 0)
        {
            _m_boMailBo.setExpiredTs(bmObj, nowTimeSec + expiredTimeSec);//读取配表
            _m_boMailBo.setEffectSecs(bmObj, expiredTimeSec);//记录有效时长
        }
        else if (expiredTimeSec == 0)
        {
            _m_boMailBo.setExpiredTs(bmObj, nowTimeSec + MailSystem.MAIL_DEFAULT_EXPIRED_TIME);//默认7天
            _m_boMailBo.setEffectSecs(bmObj, MailSystem.MAIL_DEFAULT_EXPIRED_TIME);//记录有效时长
        }
        else
        {
            _m_boMailBo.setExpiredTs(bmObj, -1);//读取配表，这里用整形乘1000，是因为客户端会转化为int，如果过大会变负数
            _m_boMailBo.setEffectSecs(bmObj, -1);//记录有效时长，-1表示最大数值
        }
        _m_boMailBo.saveAllMarked(bmObj);

        getComp().getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_055_OnMailRewardTaken(this));
        //推送当前截至时间
        getComp().getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_058_OnMailExpiredSecChg(this));
    }

    /***
     * 设置为已读取
     */
    public void markRead(NPPlayerContext _context)
    {
        if (getIsRead())
            return;

        _m_boMailBo.saveReadedTs(_m_compMailComponent.getUSServer().getBM(), CommonFunc.getNowTimeSec());

        getComp().getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_054_OnMailReaded(this));

        //执行邮件查看效果
        if (null != _m_rMailRef && !_m_rMailRef.s_read_effect.isEmpty())
        {
            //执行效果
            NPPlayerEffectDealer.dealEffect(_m_rMailRef.s_read_effect, getComp().getUserData(), null, _context);
        }
    }

    /******
     * 设置是否锁定状态
     * @param _isLocked
     */
    public void setLockState(boolean _isLocked)
    {
        BM bmObj = _m_compMailComponent.getUSServer().getBM();

    	//如果锁定状态不一致，才需要更新数值
    	if(_isLocked != getBo().getIsLocked())
    	{
    		//更新锁定状态
    		_m_boMailBo.setIsLocked(bmObj, _isLocked);
    		
    		//从锁定到非锁定，需要重新记录超时时间
    		if(!_isLocked)
    		{
    			if(_m_boMailBo.getEffectSecs() == -1) //表明是无限大的过期时长
    			{
    				_m_boMailBo.setExpiredTs(bmObj, -1);
    			}
    			else //过期时间=当前时间+有效时长
    			{
    				_m_boMailBo.setExpiredTs(bmObj, CommonFunc.getNowTimeSec() + _m_boMailBo.getEffectSecs());
    			}
    		}
    		
    		_m_boMailBo.saveAllMarked(bmObj);
    	}
    	
    	//推送当前锁定状态
        getComp().getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_053_OnMailLockedUpdated(this));
        //推送当前截至时间
        getComp().getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_058_OnMailExpiredSecChg(this));
    }

    /***
     * 设置已经读完
     */
    public void setReadOver()
    {
        _m_boMailBo.saveIsReadOver(_m_compMailComponent.getUSServer().getBM(), true);
        
        getComp().getUserData().sendMsgToGC(US2GCWriter_009_MailOp.make_057_OnMailReadOver(this));
    }

    @Override
    public String toString()
    {
        return String.format("uid[%d] refId[%d] isRead[%s] item:[%s] hasTaken:[%s] isLock[%s] isExpired[%s]"
                , getUid()
                , _m_boMailBo.getMailRefId()
                , getIsRead()
                , CommonFunc.getProtoString(_m_alItemList)
                , getHasTaken()
                , getIsLocked()
                , isExpired());
    }
    
    /**
     * 销毁邮件数据
     * @param _context
     */
    protected void discard(NPPlayerContext _context)
    {
    	//移除数据
    	_m_boMailBo.del(_m_compMailComponent.getUSServer().getBM());
        getComp().logMail(ENpLogType.DEL, getBo(), _context);

        //这边自主的触发邮件删除效果处理，并返回可能的消息
        if (null != _m_rMailRef && !_m_rMailRef.s_del_effect.isEmpty())
        {
            //执行删除效果处理，这里不使用外部带入的context是由于外部带入的部分不一定都有返回消息
            NPPlayerContext delContext = NPPlayerContext.createNew(_context);

            NPPlayerEffectDealer.dealEffect(_m_rMailRef.s_del_effect, getComp().getUserData(), null, delContext);

            //返回可能获得的奖励信息
            getComp().getUserData().sendMsgToGC(delContext.getCollector().toProto());
        }
    }

    /**************
     * 获取邮件的附件
     * @return
     */
    public boolean takeMailAttach(NPPlayerContext _context)
    {
        getComp().getUserData().lockUser();

        try
        {
            //领取未领取的邮件
            if (!getHasItem() || getHasTaken())
                return false;

            //设置已经取出
            markTaken();

            //领取物品
            getComp().getUserData().gainItemListP(_m_alItemList, _context);

            return true;
        } 
        finally
        {
            getComp().getUserData().unlockUser();
        }
    }
}
