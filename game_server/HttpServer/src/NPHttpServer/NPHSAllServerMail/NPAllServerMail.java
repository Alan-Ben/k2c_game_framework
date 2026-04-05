package NPHttpServer.NPHSAllServerMail;

import Common.Common_StringList;
import HSDB.Bo.HsAllServerMailBO;
import HSDB.Bo.HsAllServerMailItemBO;
import HSDB.Bo.HsAllServerMailServerIdBO;
import HSDB.Bo.HsAllServerMailTextBO;
import NP2US.p002_UserOp.NP2US_002_005_OnServerMailUpdate;
import NP2US_B.p001_BasicOp.NP2US_B_001_003_AllServerMailUpdate;
import NPCommon.CommonObj.NPCommonCostItem;
import NPEnum.ENPItemType;
import NPHttpServer.Http.Entity.NPEntityAllServerMail;
import NPHttpServer.Http.Entity.NPEntityServerMail;
import NPHttpServer.NPHttpServer;
import NPServerProtocolWriter.NP2US.Msg.NP2US_B_Writer_001_BasicOp;
import NPServerProtocolWriter.NP2US.Msg.NP2US_Writer_002_UserOp;
import WCGCommon.Enum.NPEnum.EServerType;

import java.nio.ByteBuffer;
import java.util.ArrayList;

/**
 * @description: 全服邮件
 * @author: ricci
 * @date: 2023-03-27 11:00:18
 */
public class NPAllServerMail
{
    /**
     * 服务器邮件
     */
    private NPEntityAllServerMail _m_ServerMail;

    private long _m_mailDBId;

    public NPAllServerMail()
    {
        _m_ServerMail = new NPEntityAllServerMail();
    }

    public long getMailDBId()
    {
        return _m_mailDBId;
    }

    /**
     * 从数据库初始化邮件
     * @param _bo NpHsAllServerMailBO
     */
    public void initFromDB(HsAllServerMailBO _bo)
    {
        _m_mailDBId = _bo.getId();
        NPEntityServerMail platFromMail = _m_ServerMail.getPlatFromMail();
        platFromMail.setMailRefId(_bo.getMailRefId());
        platFromMail.setPhpMailId(_bo.getPhpMailId());
        platFromMail.setSendTime(_bo.getSendTime());
        platFromMail.setExpiredTime(_bo.getExpiredTime());
        platFromMail.setDefaultLang(_bo.getDefaultLang());
        platFromMail.setPassedTimeMs(_bo.getPassedTimeMs());
        //邮件内容替换数据
        if(null != _bo.getContentReplace())
        {
        	ByteBuffer buff = ByteBuffer.wrap(_bo.getContentReplace());
        	Common_StringList listObj = new Common_StringList();
        	listObj.readPackage(buff);
        	
        	for(int i = 0; i < listObj.getValueList().size(); i++)
        	{
        		platFromMail.getContentReplace().add(listObj.getValueList().get(i));
        	}
        }
    }

    public NPEntityAllServerMail getServerMail()
    {
        return _m_ServerMail;
    }

    /**
     * 从数据库初始化邮件发放的服务器id
     * @param _bo
     */
    public void initServerIdFromDB(HsAllServerMailServerIdBO _bo)
    {
        _m_ServerMail.getUsTypeIdList().add(_bo.getUsServerTypeId());
    }

    /****************
     * 初始化文本数据
     * @param _bo
     */
    public void initMailTextFromDB(HsAllServerMailTextBO _bo)
    {
        _m_ServerMail.getPlatFromMail().addText(_bo.getLang(), _bo.getTitle(), _bo.getContent());
    }

    /*****************
     * 初始化附件数据
     * @param _bo
     */
    public void initMailItemFromDB(HsAllServerMailItemBO _bo)
    {
        _m_ServerMail.getPlatFromMail().addItem(
                new NPCommonCostItem(
                        ENPItemType.ENPItemType_FromInt(_bo.getItemType())
                        , _bo.getSubId()
                        , _bo.getItemCount()));
    }

    public ArrayList<Integer> getServerIdList()
    {
        return _m_ServerMail.getUsTypeIdList();
    }

    public long getMailRefId()
    {
        return _m_ServerMail.getPlatFromMail().getMailRefId();
    }

    public long getPhpMailId()
    {
        return _m_ServerMail.getPlatFromMail().getPhpMailId();
    }

    /**
     * 推送到US
     */
    public void pushUS()
    {
        if (isAllServer())
        {
            //向所有服务器广播
            NP2US_B_001_003_AllServerMailUpdate proto = NP2US_B_Writer_001_BasicOp.make_003_AllServerMailUpdate
                    (false, getMailRefId(), getServerIdList());
            NPHttpServer.getInstance().broadcastMessage(EServerType.USER.ordinal(), proto);
        } else
        {
            //向指定服务器通知邮件新增
            NP2US_002_005_OnServerMailUpdate proto = NP2US_Writer_002_UserOp.make_005_OnServerMailUpdate(getMailDBId());
            for (Integer usTypeId : getServerIdList())
            {
                NPHttpServer.getInstance().sendMessageToBSServer(EServerType.USER.ordinal(), usTypeId, proto);
            }
        }
    }

    /**
     * 是否全服邮件,暂时没有这个设定，预留一个这样的接口，
     * 如果为true，代表不区分 usTypeId ，向所有服务器发邮件
     * @return boolean
     */
    public boolean isAllServer()
    {
        return false;
    }

    /**
     * 销毁邮件相关数据
     */
    public void dispose()
    {
        NPHttpServer.getInstance().getBM().getBM(HsAllServerMailBO.class).delAll("id", _m_mailDBId);
        NPHttpServer.getInstance().getBM().getBM(HsAllServerMailServerIdBO.class).delAll("mail_db_id", _m_mailDBId);
        NPHttpServer.getInstance().getBM().getBM(HsAllServerMailTextBO.class).delAll("mail_db_id", _m_mailDBId);
        NPHttpServer.getInstance().getBM().getBM(HsAllServerMailItemBO.class).delAll("mail_db_id", _m_mailDBId);
    }

}
