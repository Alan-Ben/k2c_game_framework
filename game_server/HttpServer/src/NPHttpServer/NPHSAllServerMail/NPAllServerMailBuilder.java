package NPHttpServer.NPHSAllServerMail;

import Common.Common_StringList;
import HSDB.Bo.HsAllServerMailBO;
import HSDB.Bo.HsAllServerMailItemBO;
import HSDB.Bo.HsAllServerMailServerIdBO;
import HSDB.Bo.HsAllServerMailTextBO;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityAllServerMail;
import NPHttpServer.Http.Entity.NPEntityAllServerMailText;
import NPHttpServer.Http.Entity.NPEntityServerMail;
import NPHttpServer.NPHttpServer;

import java.util.ArrayList;

/**
 * @description: 全服邮件对象创建器
 * @author: ricci
 * @date: 2023-03-28 13:58:23
 */
public class NPAllServerMailBuilder
{
    /**
     * 通过平台发来的数据转换成HS全服邮件管理器中管理的数据,同时创建数据库记录
     * @param _platFromAllServerMail NPEntityAllServerMail
     * @return NPAllServerMail
     */
    public static NPAllServerMail build(NPEntityAllServerMail _platFromAllServerMail)
    {
        NPAllServerMail serverMail = new NPAllServerMail();

        BM bmObj = NPHttpServer.getInstance().getBM();

        NPEntityServerMail platFromMail = _platFromAllServerMail.getPlatFromMail();
        //生成数据库存储对象
        HsAllServerMailBO bo = new HsAllServerMailBO();
        bo.setMailRefId(bmObj, platFromMail.getMailRefId());
        bo.setPhpMailId(bmObj, platFromMail.getPhpMailId());
        bo.setSendTime(bmObj, platFromMail.getSendTime());
        bo.setExpiredTime(bmObj, platFromMail.getExpiredTime());
        bo.setDefaultLang(bmObj, platFromMail.getDefaultLang());
        bo.setPassedTimeMs(bmObj, platFromMail.getPassedTimeMs());
        //内容替换数据
        if(null != platFromMail.getContentReplace())
        {
        	Common_StringList listObj = new Common_StringList();
        	listObj.getValueList().addAll(platFromMail.getContentReplace());
        	
        	bo.setContentReplace(bmObj, CommonFunc.ByteBfferToBytes(listObj.makePackage()));
        }
        
        bo.insert(bmObj);

        //初始化全服邮件对象
        serverMail.initFromDB(bo);
        //初始化服务器列表
        ArrayList<Integer> usTypeIdList = _platFromAllServerMail.getUsTypeIdList();
        for (Integer usTypeId : usTypeIdList)
        {
            HsAllServerMailServerIdBO serverTypeIdBo = new HsAllServerMailServerIdBO();
            serverTypeIdBo.setMailDbId(bmObj, bo.getId());
            serverTypeIdBo.setUsServerTypeId(bmObj, usTypeId);
            serverTypeIdBo.insert(bmObj);
            //初始化接受邮件的服务器列表
            serverMail.initServerIdFromDB(serverTypeIdBo);
        }
        //初始化邮件标题内容
        for (NPEntityAllServerMailText text : platFromMail.getMailTextList())
        {
            HsAllServerMailTextBO textBo = new HsAllServerMailTextBO();
            textBo.setMailDbId(bmObj, bo.getId());
            textBo.setLang(bmObj, text.getLang());
            textBo.setTitle(bmObj, text.getTitle());
            textBo.setContent(bmObj, text.getContent());
            textBo.insert(bmObj);

            serverMail.initMailTextFromDB(textBo);
        }
        //初始化邮件附件
        for (NPCommonCostItem costItem : platFromMail.getItemList())
        {
            HsAllServerMailItemBO itemBo = new HsAllServerMailItemBO();
            itemBo.setMailDbId(bmObj, bo.getId());
            itemBo.setItemType(bmObj, costItem.getItemType().ordinal());
            itemBo.setSubId(bmObj, costItem.getItemId());
            itemBo.setItemCount(bmObj, costItem.getCount());
            itemBo.insert(bmObj);

            serverMail.initMailItemFromDB(itemBo);
        }


        return serverMail;
    }
}
