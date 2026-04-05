package NPHttpServer.NPHSAllServerMail;

import ALBasicServer.ALBasicMutex.MutexAtom;
import ALBasicServer.ALServerSynTask.ALSynTaskManager;
import Common.NpServerObj.NpServerObj_PlatFormMail;
import Common.NpServerObj.NpServerObj_PlatFormMailText;
import HSDB.Bo.HsAllServerMailBO;
import HSDB.Bo.HsAllServerMailItemBO;
import HSDB.Bo.HsAllServerMailServerIdBO;
import HSDB.Bo.HsAllServerMailTextBO;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityAllServerMailText;
import NPHttpServer.Http.Entity.NPEntityServerMail;
import NPHttpServer.NPHttpServer;

import java.util.ArrayList;

/**
 * @description: 全服邮件管理
 * @author: ricci
 * @date: 2023-03-27 10:22:20
 */
public class NPHSAllServerMailMgr
{
    //////单例的//////
    private static final NPHSAllServerMailMgr _s_instance = new NPHSAllServerMailMgr();

    public static NPHSAllServerMailMgr getInstance()
    {
        return _s_instance;
    }

    private ArrayList<NPAllServerMail> _m_allServerMailList;
    private MutexAtom _m_mutex;

    private NPHSAllServerMailMgr()
    {
        _m_allServerMailList = new ArrayList<>();
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

    /**
     * 从数据库恢复数据
     * @return 是否成功执行
     */
    public boolean initFromDB()
    {
        for (HsAllServerMailBO bo : NPHttpServer.getInstance().getBM().getBM(HsAllServerMailBO.class).s_findAll())
        {
            if (bo == null)
            {
                continue;
            }
            //初始化serverMail对象
            NPAllServerMail allServerMail = new NPAllServerMail();
            allServerMail.initFromDB(bo);
            _m_allServerMailList.add(allServerMail);
        }
        //初始化全服邮件对象的服务器列表
        for (HsAllServerMailServerIdBO bo : NPHttpServer.getInstance().getBM().getBM(HsAllServerMailServerIdBO.class).s_findAll())
        {
            if (bo == null)
            {
                continue;
            }
            //查找相关的serverMail对象，初始化其serverId列表
            NPAllServerMail allServerMail = lookupMail(bo.getMailDbId());
            if (allServerMail == null)
            {
                CommLog.error("NPHSAllServerMailMgr initFromDB HsAllServerMailServerIdBO NPAllServerMail not found dbId:{}"
                        , bo.getMailDbId());
                continue;
            }
            allServerMail.initServerIdFromDB(bo);
        }
        //初始化全服邮件对象的邮件内容列表
        for (HsAllServerMailTextBO bo : NPHttpServer.getInstance().getBM().getBM(HsAllServerMailTextBO.class).s_findAll())
        {
            if (bo == null)
            {
                continue;
            }
            NPAllServerMail allServerMail = lookupMail(bo.getMailDbId());
            if (allServerMail == null)
            {
                CommLog.error("NPHSAllServerMailMgr initFromDB HsAllServerMailTextBO NPAllServerMail not found dbId:{}"
                        , bo.getMailDbId());
                continue;
            }
            allServerMail.initMailTextFromDB(bo);
        }
        //初始化全服邮件附件数据
        for (HsAllServerMailItemBO bo : NPHttpServer.getInstance().getBM().getBM(HsAllServerMailItemBO.class).s_findAll())
        {
            if (bo == null)
            {
                continue;
            }
            NPAllServerMail allServerMail = lookupMail(bo.getMailDbId());
            if (allServerMail == null)
            {
                CommLog.error("NPHSAllServerMailMgr initFromDB HsAllServerMailItemBO NPAllServerMail not found dbId:{}"
                        , bo.getMailDbId());
                continue;
            }
            allServerMail.initMailItemFromDB(bo);
        }

        //过期检查任务
        ALSynTaskManager.getInstance().regTask(new SynTask_HSCheckPHPAllServerMailExpired());

        return true;
    }

    /**
     * 增加全服邮件
     * @param _allServerMail 全服邮件
     */
    public void addServerMail(NPAllServerMail _allServerMail)
    {
        _lock();
        try
        {
            //如果已存在，则无法添加
            if (lookupMail(_allServerMail.getMailDBId()) != null)
            {
                return;
            }
            _m_allServerMailList.add(_allServerMail);
            
            //日志数据
            _logMailAdd(_allServerMail);
            
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过id查找邮件列表
     * @param _mailDbId 指定id
     */
    public NPAllServerMail lookupMail(long _mailDbId)
    {
        _lock();
        try
        {
            for (NPAllServerMail serverMail : _m_allServerMailList)
            {
                if (serverMail == null)
                {
                    continue;
                }
                if (serverMail.getMailDBId() == _mailDbId)
                {
                    return serverMail;
                }
            }
            return null;
        } finally
        {
            _unlock();
        }
    }

    /**
     * 通过最大id查找邮件列表
     * @param _maxMailDbId    id限制
     * @param _serverMailList 邮件列表
     */
    public long makeMailList(long _maxMailDbId, int _usTypeId, ArrayList<NpServerObj_PlatFormMail> _serverMailList)
    {
        _lock();
        try
        {
            long maxMailDBid = 0;
            for (NPAllServerMail serverMail : _m_allServerMailList)
            {
                //跳过指定小于指定id的邮件
                if (serverMail.getMailDBId() <= _maxMailDbId)
                {
                    continue;
                }
                //不是全服邮件&&不包含自己，认为是需要跳过的
                if (!serverMail.isAllServer() && !serverMail.getServerIdList().contains(_usTypeId))
                {
                    continue;
                }

                NpServerObj_PlatFormMail platFormMail = new NpServerObj_PlatFormMail();
                platFormMail.setMailUid(serverMail.getMailDBId());
                platFormMail.setMailRefId(serverMail.getMailRefId());
                platFormMail.setPhpMailId(serverMail.getPhpMailId());
                //邮件基础数据
                NPEntityServerMail entityMail = serverMail.getServerMail().getPlatFromMail();
                platFormMail.setSendTime(entityMail.getSendTime());
                platFormMail.setExpiredTime(entityMail.getExpiredTime());
                platFormMail.setDefaultLang(entityMail.getDefaultLang());
                platFormMail.setPassedTimeMs(entityMail.getPassedTimeMs());
                //邮件标题内容数据
                for (NPEntityAllServerMailText mailText : entityMail.getMailTextList())
                {
                    NpServerObj_PlatFormMailText platFormMailText = new NpServerObj_PlatFormMailText(mailText.getLang(),
                            mailText.getTitle(), mailText.getContent());
                    platFormMail.addPlatformMailTextList(platFormMailText);
                }
                //邮件附件
                for (NPCommonCostItem costItem : entityMail.getItemList())
                {
                    platFormMail.addItemList(costItem.toProto());
                }
                //邮件替换数据
                for(int i = 0; i < entityMail.getContentReplace().size(); i++)
                {
                	platFormMail.addContentReplace(entityMail.getContentReplace().get(i));
                }

                _serverMailList.add(platFormMail);

                //选取最大的邮件id
                if (platFormMail.getMailUid() > maxMailDBid)
                {
                    maxMailDBid = platFormMail.getMailUid();
                }
            }
            return maxMailDBid;
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

            for (int i = _m_allServerMailList.size() - 1; i >= 0; i--)
            {
                NPAllServerMail info = _m_allServerMailList.get(i);
                if (null == info)
                    continue;

                //移除过期邮件
                if (nowTimeSec >= CommonFunc.simpleDateFormatTimeSec(info.getServerMail().getPlatFromMail().getExpiredTime()))
                {
                    NPAllServerMail remove = _m_allServerMailList.remove(i);
                    if (remove != null)
                    {
                        remove.dispose();
                    }
                }
            }
        } finally
        {
            _unlock();
        }
    }
    
    /**
     * 移除全服邮件
     * @param _phpMailId
     * @return
     */
    public NPAllServerMail delPHPMail(long _phpMailId)
    {
    	_lock();
    	
    	try
    	{
    		for (int i = 0; i < _m_allServerMailList.size(); i++)
            {
                NPAllServerMail info = _m_allServerMailList.get(i);
                if (null == info)
                    continue;
            
                if(info.getPhpMailId() == _phpMailId)
                {
                	_m_allServerMailList.remove(i);
                	info.dispose();
                	
                	return info;
                }
            }
    		
    		return null;
    	}
    	finally
    	{
    		_unlock();
    	}
    }
    
    /**
     * 增加邮件日志
     * @param _allServerMail
     */
    private void _logMailAdd(NPAllServerMail _allServerMail)
    {
    	try
    	{
    		//US列表
    		String usListStr = CommonFunc.list2String(_allServerMail.getServerMail().getUsTypeIdList());
    		
    		//物品列表
    		StringBuilder itemListSb = new StringBuilder();
    		if(null != _allServerMail.getServerMail().getPlatFromMail().getItemList() 
    				&& !_allServerMail.getServerMail().getPlatFromMail().getItemList().isEmpty())
    		{
    			for(int i = 0; i < _allServerMail.getServerMail().getPlatFromMail().getItemList().size(); i++)
    			{
    				NPCommonCostItem attachItem = _allServerMail.getServerMail().getPlatFromMail().getItemList().get(i);
    				if(null == attachItem)
    					continue;
    				
    				itemListSb.append(attachItem.toString()).append(";");
    			}
    		}
    		else
    		{
    			itemListSb.append("no item");
    		}
    		
    		//输出日志
            CommLog.info("NPHSAllServerMailMgr addServerMail mailDBID:{}, phpMailId:{}, mailRefId:{}, sendTime:{}, expiredTime:{}, itemList:{}, usList:{}"
                    , _allServerMail.getMailDBId()
                    , _allServerMail.getPhpMailId()
                    , _allServerMail.getMailRefId()
                    , _allServerMail.getServerMail().getPlatFromMail().getSendTime()
                    , _allServerMail.getServerMail().getPlatFromMail().getExpiredTime()
                    , itemListSb.toString()
                    , usListStr);
    	}
    	catch (Exception e) 
    	{
			e.printStackTrace();
			CommLog.error("", e);
		}
    }
}
