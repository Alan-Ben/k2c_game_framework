package NPGateServer.NPGeneralListener.RequestDispather;

import NP2GS_R.p001_PSOp.NP2GS_R_001_001_ReqUserKey;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.ErrMain.LoginErr;
import NPGateServer.NPGCCheckCodeMgr.NPGCCheckCodeInfo;
import NPGateServer.NPGCCheckCodeMgr.NPGCCheckCodeMgr;
import NPGateServer.NPGeneralListener.Writer.NP2GS_RB_Writer_001_PSOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/********************
 * 平台服务器发送来请求的处理对象
 *
 * @author Administrator
 *
 */
public class NPGSGeneralRequestDispather extends NPRequestDispatcher
{
    private static NPGSGeneralRequestDispather _g_instance = new NPGSGeneralRequestDispather();

    public static NPGSGeneralRequestDispather getInstance()
    {
        return _g_instance;
    }

    protected NPGSGeneralRequestDispather()
    {
        NPGSGeneral_255_RequestDispatcher.init(this);
        regHandler(new NPRequestDealer<NP2GS_R_001_001_ReqUserKey>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2GS_R_001_001_ReqUserKey _msg)
            {
                // 创建对应的连接串
                NPGCCheckCodeInfo newCheckCodeInfo = NPGCCheckCodeMgr.getInstance().addUserCheckCode(_msg.getUid());

                if (null == newCheckCodeInfo)
                {
                    _committer.commitFailRes(LoginErr.GS_GEN_USER_CODE_ERR.getCode());
                    return;
                }

                // 返回结果
                _committer.commitSucRes(NP2GS_RB_Writer_001_PSOp.make_001_RetUserKeySuc(newCheckCodeInfo.getUid(),
                        newCheckCodeInfo.getCheckCode()));
            }
        });
    }
}
