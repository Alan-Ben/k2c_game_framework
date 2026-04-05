package NPHttpServer.GMCommand.Cmds;

import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.GMCommand.CmdClassBase;
import NPCommon.Http.HttpService.server.CmdResponse;
import NPCommon.Util.CallBack._ICallBackT;
import NPHttpServer.Http.HttpService.MsgDispather.NPHttpDealerDispatcher;
import NPHttpServer.Http.HttpService.MsgDispather._ANPPlatFormHttpSubDealer;

@ACommander(comment = "http平台相关", name = "http")
public class CmdHttp extends CmdClassBase
{
    @ACommand(comment = "执行平台命令")
    public void dealCommand(int _mainId, int _subId, String _base64Command)
    {
        //base64解码
        String command = new String(java.util.Base64.getDecoder().decode(_base64Command));

        //执行命令
        _ANPPlatFormHttpSubDealer<?> dealer = NPHttpDealerDispatcher.getInstance().lookupDealer(_mainId, _subId);
        if (dealer == null)
        {
            takeCallBack().onRunOver(false,"dealer not found");
            return;
        }

        dealer.dealMsg("0", command, new CmdResponse(new _ICallBackT<String>()
        {
            @Override
            public void onRunOver(String s)
            {
                takeCallBack().onRunOver(true, s);
            }
        }));
    }
}
