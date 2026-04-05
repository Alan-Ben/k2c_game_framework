package NPUSServer.NPUSUserMgr.GameSystem.MailSystem.AllServerMail;

import ALBasicProtocolPack._IALProtocolStructure;
import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.NpServerObj.NpServerObj_PlatFormMail;
import NP2HS_RB.p001_HSOp.NP2HS_RB_001_002_RetAllServerMail;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPServerProtocolWriter.NP2HS.Np2HS_R_Writer_001_HSOP;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_S_ALL_SERVER_MAIL_CHG;
import NPUSServer.NPUserServer;
import NPUSServer.SynTask.USServerServerEventBroadCastAllUser;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

import java.util.ArrayList;

/**
 * @description: 全服邮件管理
 * 这里存放邮件的模版，给每个邮件模版设置序列号，玩家存储一个序列号，通过序列号取还未读过的邮件
 * @author: ricci
 * @date: 2023-03-28 16:05:24
 */
public class NPAllServerMailTemplateMgr
{
    private NPUserServer _m_usUSServer;

    private ArrayList<NpServerObj_PlatFormMail> _m_allServerMailTemplateList;

    /**
     * 从HS取到的最后一条邮件记录的id
     */
    private long _m_lMaxMailId;

    private MutexAtom _m_mutex;

    public NPAllServerMailTemplateMgr(NPUserServer _usServer)
    {
        _m_usUSServer = _usServer;

        _m_allServerMailTemplateList = new ArrayList<>();
        _m_mutex = new MutexAtom();
    }

    protected void _lock()
    {
        _m_mutex.lock();
    }

    protected void _unlock()
    {
        _m_mutex.unlock();
    }

    public NPUserServer getUSServer() {return _m_usUSServer;}

    public long getMailMaxId()
    {
        return _m_lMaxMailId;
    }

    public void setMaxMailId(long _maxMailId)
    {
        _lock();
        try
        {
            //只能设置更大的值
            if (_m_lMaxMailId >= _maxMailId)
            {
                return;
            }
            this._m_lMaxMailId = _maxMailId;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 初始化筛选邮件列表
     */
    public void init()
    {
        refreshMailTemplate(null);
        ALSynTaskManager.getInstance().regTask(new SynTask_USCheckPHPAllServerMailExpired(getUSServer()));
    }

    /**
     * 刷新邮件列表
     */
    public void refreshMailTemplate(_ICallBackBool _callBack)
    {
        getUSServer().sendRequestToBSServer(EServerType.SINGLE.ordinal(),
                ENPSingleServerType.HTTP.ordinal(), Np2HS_R_Writer_001_HSOP.make_001_002_ReqAllServerMail(getMailMaxId(),
                        getUSServer().getServerTypeId()), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2HS_RB_001_002_RetAllServerMail();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _ret)
                    {
                        NP2HS_RB_001_002_RetAllServerMail retProto = (NP2HS_RB_001_002_RetAllServerMail) _ret;
                        _lock();
                        try
                        {
                            //设置邮件最大id
                            setMaxMailId(retProto.getMailMaxId());
                            //更新邮件数据
                            for (NpServerObj_PlatFormMail platFormMail : retProto.getPlatformMailList())
                            {
                                NpServerObj_PlatFormMail serverMailTemplate = lookupMailTemplate(platFormMail.getMailUid());
                                if (serverMailTemplate != null)
                                {
                                    //已经存在此邮件
                                    USLog.error(getUSServer(), "NPAllServerMailTemplateMgr refreshMailTemplate has exist mail :{}",
                                            platFormMail.getMailUid());
                                    continue;
                                }
                                _m_allServerMailTemplateList.add(platFormMail);
                                
                                //输出日志
                                USLog.info(getUSServer(), "NPAllServerMailTemplateMgr refreshMailTemplate add server mail, mailUid:{} phpMailId:{} expiredTime:{}",
                                        platFormMail.getMailUid(), platFormMail.getPhpMailId(), platFormMail.getExpiredTime());
                            }
                        } finally
                        {
                            _unlock();
                        }

                        //通知所有内存在线玩家更新邮件数据
                        __broadcastMailChg();

                        if (_callBack != null)
                        {
                            _callBack.onRunOver(true);
                        }
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(getUSServer(), "NPAllServerMailTemplateMgr refreshMailTemplate deal Fail errCode:{}",
                                _errCode);
                        if (_callBack != null)
                        {
                            _callBack.onRunOver(false);
                        }
                    }
                });

    }

    /**
     * 广播邮件变化
     */
    private void __broadcastMailChg()
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ALL_SEVER_MAIL_CHG);
        Event_S_ALL_SERVER_MAIL_CHG event = new Event_S_ALL_SERVER_MAIL_CHG(context, getMailMaxId());
        ALSynTaskManager.getInstance().regTask(new USServerServerEventBroadCastAllUser(getUSServer(), event));
    }

    /**
     * 查找邮件模版
     * @param _allServerMailId 邮件唯一id
     * @return NpServerObj_PlatFormMail
     */
    public NpServerObj_PlatFormMail lookupMailTemplate(long _allServerMailId)
    {
        _lock();
        try
        {
            for (NpServerObj_PlatFormMail mail : _m_allServerMailTemplateList)
            {
                if (mail == null)
                {
                    continue;
                }
                if (mail.getMailUid() == _allServerMailId)
                {
                    return mail;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 获取比指定id大的邮件信息列表
     * @param _mailMaxId 邮件id
     * @param _list      邮件列表
     * @return maxMailId
     */
    public long lookupMailTemplateListByMailMaxId(long _mailMaxId, ArrayList<NpServerObj_PlatFormMail> _list)
    {
        _lock();
        try
        {
            for (NpServerObj_PlatFormMail mail : _m_allServerMailTemplateList)
            {
                if (mail == null)
                {
                    continue;
                }
                if (mail.getMailUid() > _mailMaxId)
                {
                    _list.add(mail);
                }
            }
            return _m_lMaxMailId;
        } finally
        {
            _unlock();
        }

    }

    /**
     * 检查邮件过期
     */
    public void checkExpired()
    {
        _lock();

        try
        {
            long nowTimeSec = CommonFunc.getNowTimeSec();

            for (int i = _m_allServerMailTemplateList.size() - 1; i >= 0; i--)
            {
                NpServerObj_PlatFormMail info = _m_allServerMailTemplateList.get(i);
                if (null == info)
                    continue;

                //移除过期邮件
                if (nowTimeSec >= CommonFunc.simpleDateFormatTimeSec(info.getExpiredTime()))
                {
                    _m_allServerMailTemplateList.remove(i);
                }
            }
        } finally
        {
            _unlock();
        }
    }
    
    /**
     * 移除指定的全服邮件
     * @param _phpMailId
     */
    public void delMailTemplate(long _phpMailId)
    {
        _lock();

        try
        {
            for (int i = _m_allServerMailTemplateList.size() - 1; i >= 0; i--)
            {
                NpServerObj_PlatFormMail info = _m_allServerMailTemplateList.get(i);
                if (null == info)
                    continue;

                //移除过期邮件
                if (info.getPhpMailId() == _phpMailId)
                {
                    _m_allServerMailTemplateList.remove(i);
                    break;
                }
            }
            
            //输出日志
            USLog.info(getUSServer(), "NPAllServerMailTemplateMgr delMailTemplate phpMailId:{}", _phpMailId);
        } finally
        {
            _unlock();
        }
    }
}
