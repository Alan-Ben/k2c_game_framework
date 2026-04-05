package NPHttpServer.Http.HttpService.MsgDispather.Http_002_Op;

import ALBasicProtocolPack._IALProtocolStructure;
import Common.NpServerObj.NpServerObj_PlatFormMail;
import Common.NpServerObj.NpServerObj_PlatFormMailText;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Log.CommLog;
import NPCommon.Util.CommonFunc;
import NPHttpServer.Http.Entity.NPEntityAllServerMailText;
import NPHttpServer.Http.Entity.NPEntityServerMail;
import NPHttpServer.Http.Entity.NPEntityUserMail;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormUserMailDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHttpServer;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_014_ReqSendUserMail;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_014_RetSendUserMail;
import WCGCommon.Enum.NPEnum.EServerType;

/**
 * @description: 后台推送服务器列表
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_002_002_PlatFromUserMail extends _ANPPlatFormHttpSubDealer<NPEntityUserMail>
{
    @Override
    public int subOrder()
    {
        return 2;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityUserMail> getDecoder()
    {
        return NPPlatFormUserMailDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityUserMail _decodeObj)
    {
        //参数检查
        if (null == _decodeObj || null == _decodeObj.getPlatFromMail()
                || _decodeObj.getCidList().size() != 1 || _decodeObj.getCid() <= 0)
        {
            _commiter.commitFail(CommErr.PARAM_ERROR);
            return;
        }

        //获取目标玩家US服务器ID
        int targetUsId = CommonFunc.parseServerTypeIdFromCid(_decodeObj.getCid());
        if (targetUsId <= 0)
        {
            _commiter.commitFail(CommErr.PARAM_ERROR, "cid error");
            return;
        }

        //推送给玩家服务器，添加邮件
        NPEntityServerMail platFromMail = _decodeObj.getPlatFromMail();
        CommLog.info("send user mail phpMailId:{},cid:{}", platFromMail.getPhpMailId(), _decodeObj.getCid());

        //发送到指定玩家
        //构造邮件信息
        NpServerObj_PlatFormMail fromMail = new NpServerObj_PlatFormMail();
        fromMail.setMailRefId(platFromMail.getMailRefId());
        fromMail.setPhpMailId(platFromMail.getPhpMailId());
        fromMail.setSendTime(platFromMail.getSendTime());
        fromMail.setExpiredTime(platFromMail.getExpiredTime());
        fromMail.setDefaultLang(platFromMail.getDefaultLang());
        fromMail.setPassedTimeMs(platFromMail.getPassedTimeMs());
        //邮件标题
        for (NPEntityAllServerMailText mailText : platFromMail.getMailTextList())
        {
            if (null == mailText)
                continue;

            NpServerObj_PlatFormMailText platFormMailText = new NpServerObj_PlatFormMailText(mailText.getLang(),
                    mailText.getTitle(), mailText.getContent());
            fromMail.addPlatformMailTextList(platFormMailText);
        }
        //邮件附件
        if (null != platFromMail.getItemList())
        {
            for (NPCommonCostItem costItem : platFromMail.getItemList())
            {
                if (null == costItem)
                    continue;

                fromMail.addItemList(costItem.toProto());
            }
        }
        //邮件替换内容
        for(int i = 0; i < platFromMail.getContentReplace().size(); i++)
        {
        	fromMail.addContentReplace(platFromMail.getContentReplace().get(i));
        }

        //发送US
        NP2US_R_003_014_ReqSendUserMail proto = new NP2US_R_003_014_ReqSendUserMail();
        proto.setCid(_decodeObj.getCid());
        proto.setMailData(fromMail);

        NPHttpServer.getInstance().sendRequestToBSServer(EServerType.USER.ordinal(), targetUsId, proto
                , new _IWCGCallbackDealer()
                {

                    @Override
                    public void dealSuc(_IALProtocolStructure arg0)
                    {
                        _commiter.commitSuc();
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        _commiter.commitFail(ResultMgr.getInstance().lookupResult(_errCode));
                    }

                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2US_RB_003_014_RetSendUserMail();
                    }
                });


    }
}
