package NPPlatServer.NPGeneralListener.RequsetDispather;

import NP2PS_R.p003_LSOp.NP2PS_R_003_001_ReqGateServer;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PSErr;
import NPPlatServer.NPPS_Listener.LSCallBack.NPPS2GSRBDealerRequestReqUserKey;
import NPPlatServer.NPPS_Listener.PS_GSListener;
import NPPlatServer.NPPS_Listener.PS_LSListener;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaInfo;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaMgr;
import NPServerProtocolWriter.NP2GS.Request.NP2GS_R_Writer_001_PSOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPPSGeneral_003_RequestDispather extends NPRequestDispatcher
{
    public static void init(NPPSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new NPRequestDealer<NP2PS_R_003_001_ReqGateServer>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_003_001_ReqGateServer _msg)
            {
                //获取服务器对象
                PS_LSListener lsListener = (PS_LSListener) _committer.getRequestDealer();
                if (null == lsListener)
                {
                    _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
                    return;
                }

                //判断是否还有空间，如有则申请，如无则返回失败
                NPPlatAreaInfo areaInfo = NPPlatAreaMgr.getInstance().getAreaInfo(lsListener.getAreaTagIdx());
                if (null == areaInfo)
                {
                    _committer.commitFailRes(PSErr.PS_NO_AREA.getCode());
                    return;
                }

                //尝试获取通用的gs
                PS_GSListener gsListener = areaInfo.tryHandleUser();
                if (null == gsListener)
                {
                    areaInfo = NPPlatAreaMgr.getInstance().getCommonAreaInfo();
                    if (null != areaInfo)
                        gsListener = areaInfo.tryHandleUser();

                    if (null == gsListener)
                    {
                        _committer.commitFailRes(PSErr.PS_NO_GS.getCode());
                        return;
                    }
                }

                //注册处理用户的服务器对象，在服务器退出或用户进入时需要先进行用户在线的判断和相关处理

                //发送请求给Gate服务器,申请进入验证串,带上uid和user服务器的id,Gate服务器就知道该玩家在哪个user服务器上.
                gsListener.sendRequest(NP2GS_R_Writer_001_PSOp.make_001_ReqUserKey(_msg.getUid())
                        , new NPPS2GSRBDealerRequestReqUserKey(gsListener, _committer));
            }
        });
    }
}
