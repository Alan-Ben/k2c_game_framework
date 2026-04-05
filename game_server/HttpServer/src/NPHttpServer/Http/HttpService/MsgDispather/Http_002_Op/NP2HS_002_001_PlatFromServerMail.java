package NPHttpServer.Http.HttpService.MsgDispather.Http_002_Op;

import NPCommon.ErrMain.CommErr;
import NPHttpServer.Http.Entity.NPEntityAllServerMail;
import NPHttpServer.Http.HttpService.Decoder.NPPlatFormAllServerMailDecoder;
import NPHttpServer.Http.HttpService.Decoder._ANPPlatFormHttpDataDecoder;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;
import NPHttpServer.Http.HttpService.NPPlatFormCommiter;
import NPHttpServer.NPHSAllServerMail.NPAllServerMail;
import NPHttpServer.NPHSAllServerMail.NPAllServerMailBuilder;
import NPHttpServer.NPHSAllServerMail.NPHSAllServerMailMgr;

/**
 * @description: 后台推送服务器列表
 * @author: ricci
 * @date: 2023-03-25 00:04:41
 */
public class NP2HS_002_001_PlatFromServerMail extends _ANPPlatFormHttpSubDealer<NPEntityAllServerMail>
{
    @Override
    public int subOrder()
    {
        return 1;
    }

    @Override
    public _ANPPlatFormHttpDataDecoder<NPEntityAllServerMail> getDecoder()
    {
        return NPPlatFormAllServerMailDecoder.getInstance();
    }

    @Override
    protected void _doDealMsg(NPPlatFormCommiter _commiter, NPEntityAllServerMail _decodeObj)
    {
        //参数检查
        if (null == _decodeObj || null == _decodeObj.getPlatFromMail() || _decodeObj.getUsTypeIdList().isEmpty())
        {
            _commiter.commitFail(CommErr.PARAM_ERROR);
            return;
        }

        //返回成功
        _commiter.commitSuc();

        //构造邮件数据
        NPAllServerMail allServerMail = NPAllServerMailBuilder.build(_decodeObj);
        //放入管理器中
        NPHSAllServerMailMgr.getInstance().addServerMail(allServerMail);

        //通知US服务器来取邮件
        allServerMail.pushUS();
    }
}
