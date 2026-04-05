package NPUSServer.NPUSUserMgr.GameSystem.MailSystem;

import ALBasicCommon.ALBasicCommonFun;
import AllRpcData.US_Service.Player.UsSendMail;
import Common.MailObj.Mail_Data;
import Common.NpServerObj.NpServerObj_PlatFormMail;
import Common.NpServerObj.NpServerObj_PlatFormMailText;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.NPCommon_ItemList;
import NPCommon.Util.CommonFunc;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommon.Util.StringFunc;
import NPGameRes.Refs.Mail.RefMail;
import NPUSServer.Cache.Player.PlayerCacheFunc;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.UsFunc;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import NPUSServer.UserOfflineTmpDataMgr.PlayerCacheInfo.UserOfflineTmpDataInfo_PlayerCache;
import RPC._ARpcCallBack;
import USDB.Bo.PlayerMailBO;

/******
 * 邮件系统
 */
public class MailSystem
{
    public static int MAIL_DEFAULT_EXPIRED_TIME = 7 * 24 * 3600;

    /*********************
     * 后台玩家运营邮件，会直接到玩家所在US
     * @param _cid
     * @param _formMail
     * @param _context
     */
    public static void addFormMail(NPUserServer _server, long _cid, NpServerObj_PlatFormMail _formMail, NPPlayerContext _context)
    {
        NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_cid);
        if (null == userData)
        {
            createMailBoFromFormMail(_server, _cid, _formMail, _context);
        } else
        {
            userData.safeCall(() ->
            {
                userData.getMailComponent().addFormMail(_formMail, _context);
            });
        }
    }

    /*****
     * 邮件系统向玩家发送一封邮件
     * @param _cid
     * @param _context
     */
    public static void addMail(NPUserServer _server, long _cid, Mail_Data _mailData, NPPlayerContext _context)
    {
        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);
        //判断是否本服
        if (usId == _server.getServerTypeId())
        {
            addLocalMail(_server, _cid, _mailData, _context);
        } 
        else
        {
            UsSendMail rpc = new UsSendMail();
            rpc.req().setCid(_cid);
            rpc.req().setGameEvent(_context.getContextId());
            rpc.req().setMailData(_mailData);

            _server.rpc2us().requestTo(usId, rpc, new _ARpcCallBack<UsSendMail>()
            {
				@Override
				public void call_back(int _errCode, UsSendMail _rpc) 
				{
					if(_errCode > 0)
					{
						USLog.error(_server, "Player:{} send mail fail, errCode:{}", _cid, _errCode);
					}
				}
			});
        }
    }

    public static void addLocalMail(NPUserServer _server, long _cid, Mail_Data _mailData, NPPlayerContext context)
    {
        if (!UsFunc.isLocalCid(_server, _cid))
        {
            USLog.error(_server, "Send mail: " + _mailData.getMailRefId() + " to cid: " + _cid + " Deal in US: " + _server.getServerTypeId());
            return;
        }

        //本服直接发送
        NPUSUserData userData = _server.getUsUserMgr().lookupCacheUserData(_cid);
        if (userData != null)
        {
            userData.safeCall(() ->
            {
                userData.getMailComponent().addMail(_mailData, context);
            });
        } else
        {
            MailSystem.createMailBoFromData(_server, _cid, _mailData);
        }
    }


    /******************
     * 从系统邮件中构造邮件数据
     * @param _cid
     * @param _mailId
     * @return
     */
    public static PlayerMailBO createSysMailBo(NPUserServer _server, long _cid, long _mailId)
    {
        RefMail refMail = RefMail.getMgr().get(_mailId);
        if (null == refMail)
        {
            USLog.error(_server, "Mail Id Err!!! [" + _mailId + "]");
            return null;
        }

        BM bmObj = _server.getBM();

        PlayerMailBO bo = new PlayerMailBO();
        bo.setCid(bmObj, _cid);
//        bo.setMailUid(bmObj, UsID.makeMailId(_server));
        bo.setMailRefId(bmObj, _mailId);
        bo.setPhpMailId(bmObj, 0);
        bo.setSenderId(bmObj, refMail.senderId);
        bo.setIsLocked(bmObj, false);
        bo.setTitle(bmObj, refMail.title);
        bo.setContent(bmObj, refMail.content);
        bo.setContentReplace(bmObj, "");

        NPCommon.NPCommon_ItemList itemList = new NPCommon.NPCommon_ItemList();
        //添加邮件原始附件
        NPCommonCostItem tmpItem = null;
        for (int i = 0; i < refMail.attach_items.size(); i++)
        {
            tmpItem = refMail.attach_items.get(i);
            if (null == tmpItem)
                continue;

            itemList.addItemList(tmpItem.toProto());
        }

        //放入数据
        bo.setAttachList(bmObj, CommonFunc.ByteBfferToBytes(itemList.makePackage()));

        bo.setCreatedTs(bmObj, ALBasicCommonFun.getNowTime());


        //区分是否有奖励的情况, 获取过期时间
        int expiredTimeSec;
        if (itemList.getItemList().isEmpty())
        {
            expiredTimeSec = refMail.expired_secs;
        } else
        {
            expiredTimeSec = refMail.expired_secs_attach_items;
        }

        //邮件如果配置表配置的超时时间为-1表示无限，0表示无配置，默认按照7天处理
        if (expiredTimeSec > 0)
        {
        	bo.setExpiredTs(bmObj, bo.getCreatedTs() + expiredTimeSec);//读取配表
        	bo.setEffectSecs(bmObj, expiredTimeSec);//记录有效时长
        }
        else if (expiredTimeSec == 0)
        {
        	bo.setExpiredTs(bmObj, bo.getCreatedTs() + MAIL_DEFAULT_EXPIRED_TIME);//默认7天
        	bo.setEffectSecs(bmObj, MAIL_DEFAULT_EXPIRED_TIME);//记录有效时长
        }
        else
        {
        	bo.setExpiredTs(bmObj, -1);//读取配表，这里用整形乘1000，是因为客户端会转化为int，如果过大会变负数
        	bo.setEffectSecs(bmObj, -1);//记录有效时长，-1表示最大数值
        }

        bo.setIsMustRead(bmObj, true);
        bo.setExDataType(bmObj, 0);

        bo.insert(_server.getBM());

        return bo;
    }

    /******
     * 从邮件数据中创建邮件数据库Bo
     * @param _cid
     * @param _mailData
     * @return
     */
    public static PlayerMailBO createMailBoFromData(NPUserServer _server, long _cid, Mail_Data _mailData)
    {
        RefMail refMail = null;
        if (_mailData.getMailRefId() > 0)
        {
            refMail = RefMail.getMgr().get(_mailData.getMailRefId());
            if (null == refMail)
            {
                USLog.error(_server, "Mail Id Err!!! [" + _mailData.getMailRefId() + "]");
                return null;
            }
        }

        BM bmObj = _server.getBM();

        PlayerMailBO bo = new PlayerMailBO();
        bo.setCid(bmObj, _cid);
//        bo.setMailUid(bmObj, UsID.makeMailId(_server));
        bo.setMailRefId(bmObj, _mailData.getMailRefId());
        bo.setPhpMailId(bmObj, _mailData.getPhpMailId());
        bo.setSenderId(bmObj, _mailData.getSenderId());
        bo.setIsLocked(bmObj, false);
        bo.setTitle(bmObj, _mailData.getTitle());
        bo.setContent(bmObj, _mailData.getContent());
        bo.setContentReplace(bmObj, StringFunc.list2String(_mailData.getContentReplace()));

        NPCommon.NPCommon_ItemList itemList = new NPCommon.NPCommon_ItemList();
        //根据邮件原数据是否有附件判断处理方式
        if (null != refMail && refMail.attach_items != null && refMail.attach_items.size() > 0)
        {
            itemList = new NPCommon.NPCommon_ItemList();
            //添加邮件原始附件
            NPCommonCostItem tmpItem = null;
            for (int i = 0; i < refMail.attach_items.size(); i++)
            {
                tmpItem = refMail.attach_items.get(i);
                if (null == tmpItem)
                    continue;

                itemList.addItemList(tmpItem.toProto());
            }

            //添加附加的额外附件
            itemList.getItemList().addAll(_mailData.getItemList().getItemList());

            //放入数据
            bo.setAttachList(bmObj, CommonFunc.ByteBfferToBytes(itemList.makePackage()));
        } else
        {
            itemList = _mailData.getItemList();
            bo.setAttachList(bmObj, CommonFunc.ByteBfferToBytes(_mailData.getItemList().makePackage()));
        }

        bo.setCreatedTs(bmObj, _mailData.getCreateTimeSec());
        if (bo.getCreatedTs() <= 0)
        {
            bo.setCreatedTs(bmObj, CommonFunc.getNowTimeSec());
        }

        bo.setExpiredTs(bmObj, _mailData.getExpiredTimeSec());
        if (bo.getExpiredTs() == 0)//未设置结束时间
        {
            if (refMail != null)
            {
                //区分是否有奖励的情况, 获取过期时间
                int expiredTimeSec;
                if (itemList.getItemList().isEmpty())
                {
                    expiredTimeSec = refMail.expired_secs;
                } else
                {
                    expiredTimeSec = refMail.expired_secs_attach_items;
                }

                if (expiredTimeSec > 0)
                {
                    bo.setExpiredTs(bmObj, bo.getCreatedTs() + expiredTimeSec);//读取配表
                    bo.setEffectSecs(bmObj, expiredTimeSec);//记录有效时长
                }
                else if (expiredTimeSec == 0)
                {
                    bo.setExpiredTs(bmObj, bo.getCreatedTs() + MAIL_DEFAULT_EXPIRED_TIME);//默认7天
                    bo.setEffectSecs(bmObj, MAIL_DEFAULT_EXPIRED_TIME);//记录有效时长
                }else
                {
                    //邮件如果配置表配置的超时时间为-1表示无限，0表示无配置，默认按照7天处理
                    bo.setExpiredTs(bmObj, -1);//读取配表，这里用整形乘1000，是因为客户端会转化为int，如果过大会变负数
                    bo.setEffectSecs(bmObj, -1);//记录有效时长，-1表示最大数值
                }
            }else
            {
                //邮件如果配置表配置的超时时间为-1表示无限，0表示无配置，默认按照7天处理
                bo.setExpiredTs(bmObj, MAIL_DEFAULT_EXPIRED_TIME);//读取配表，这里用整形乘1000，是因为客户端会转化为int，如果过大会变负数
                bo.setEffectSecs(bmObj, MAIL_DEFAULT_EXPIRED_TIME);//记录有效时长，-1表示最大数值
            }
        }

        bo.setIsMustRead(bmObj, _mailData.getIsMustRead());
        bo.setExDataType(bmObj, _mailData.getExType());
        bo.setExData(bmObj, _mailData.getExData());
        
        if(null != _mailData.getExTitleData())
        {
        	bo.setExTitleData(bmObj, _mailData.getExTitleData());
        }

        bo.insert(bmObj);

        return bo;
    }

    /*******************
     * 创建mail数据
     * @param _formMail
     * @param _context
     */
    public static void createMailBoFromFormMail(NPUserServer _server, long _cid, NpServerObj_PlatFormMail _formMail, NPPlayerContext _context)
    {
        //检查邮件是否过期，过期就直接失效
        int ExpiredTimeSec = CommonFunc.simpleDateFormatTimeSec(_formMail.getExpiredTime());
        if (ExpiredTimeSec < CommonFunc.getNowTimeSec())
        {
            return;
        }

        PlayerCacheFunc.getData(_server, _cid, new HandlerTwo<Boolean, UserOfflineTmpDataInfo_PlayerCache>() {
            @Override
            public void handle(Boolean _isExist, UserOfflineTmpDataInfo_PlayerCache _cacheInfo) {
                if (!_isExist) {
                    return ;
                }

                //获取语言
                String lang = "";
                if(null != _cacheInfo)
                {
                    lang = _cacheInfo.getPlayerCache().getLanguage();
                }

                //把邮件添加到个人邮件列表中
                Mail_Data mailData = new Mail_Data();
                mailData.setMailRefId(_formMail.getMailRefId());
                NpServerObj_PlatFormMailText mailText =
                        CommonFunc.lookupPlatFormMailTextById(_formMail.getPlatformMailTextList()
                                , lang
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

                createMailBoFromData(_server, _cid, mailData);
            }
        });
    }
}
