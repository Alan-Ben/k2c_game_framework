package NPUSServer.NPGeneralListener.RequestDispather.p003_CommonOp;

import NPCommon.ErrMain.Result.ResultOne;
import NPUSServer.CommonMarquee.CommonMarqueeInfo;
import NPUSServer.NPGeneralListener.RequestDispather._ABasicGeneralRequestDealer;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_015_ReqSendMarquee;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_015_RetSendMarquee;

public class RequestDealer_NP2US_R_003_015_ReqSendMarquee extends _ABasicGeneralRequestDealer<NP2US_R_003_015_ReqSendMarquee>
{
    public RequestDealer_NP2US_R_003_015_ReqSendMarquee(NPUserServer _server) 
    {
        super(_server);
    }

    @Override
    protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2US_R_003_015_ReqSendMarquee _msg)
    {
    	//增加跑马灯数据
    	ResultOne<CommonMarqueeInfo> result = getUSServer().getMarqueeMgr().cmdAddPHPMarquee(_msg.getMarquee());
    	
    	//返回报错
    	if(!result.isSucc())
    	{
    		USLog.error(getUSServer(), "NP2US_R_003_015_ReqSendMarquee Deal Fail PHP:{} Err:{} - {}"
    				, _msg.getMarquee().getPhpId(), result.getCode(), result.getMsg());
    		
    		_committer.commitFailRes(result.getCode());
    	}
    	else
    	{
    		_committer.commitSucRes(new NP2US_RB_003_015_RetSendMarquee());
    		
    		USLog.info(getUSServer(), "NP2US_R_003_015_ReqSendMarquee Deal Suc PHP:{}"
    				, _msg.getMarquee().getPhpId());
    	}
    }
}
