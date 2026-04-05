package NPLoginCheckServer.NPLCSRequestDealer.SynTask;

import ALBasicServer.ALTask._IALSynTask;
import LCSDB.Bo.AccountBO;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.LoginErr;
import NPCommon.Util.CommonFunc;
import NPLoginCheckServer.NPAccInfoMgr.NPAccInfoMgr;
import NPLoginCheckServer.NPAccInfoMgr.NPAccInfoMgr._ILoadAccountCallBack;
import NPLoginCheckServer.NPLoginCheckServer;
import NPServerProtocolWriter.NP2LCS.RequestBack.NP2LCS_RB_Writer_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*************
 * 帐号进入游戏服务器的处理函数
 * @author Administrator
 *
 */
public class NPSynAccCheckCodeEnterTask implements _IALSynTask
{
    /**
     * 帐号名称
     */
    private String _m_sAccName;
    /**
     * 验证串信息
     */
    private String _m_sCheckCode;
    /**
     * 结果提交对象
     */
    private _IWCGBasicRequestCommiter _m_cCommiter;

    public NPSynAccCheckCodeEnterTask(String _accName, String _checkCode, _IWCGBasicRequestCommiter _commiter)
    {
        _m_sAccName = _accName;
        _m_sCheckCode = _checkCode;
        _m_cCommiter = _commiter;
    }

    @Override
    public void run()
    {
        //先到管理对象中查询是否有对应数据，有则直接处理，无则开启数据库处理
        NPAccInfoMgr.getInstance().getAccInfoByName(_m_sAccName, new _ILoadAccountCallBack()
        {
            @Override
            public void onLoad(boolean isSucc, boolean _isExists, AccountBO _bo)
            {
                if (!isSucc)
                {
                    _m_cCommiter.commitFailRes(LoginErr.LOGIN_CHECK_DB_UNAVAILABLE.getCode());
                    return;
                }

                if (!_isExists)
                {
                    _m_cCommiter.commitFailRes(LoginErr.LOGIN_NO_USER_INFO.getCode());
                    return;
                }

                //验证登录串，如不匹配则直接返回失败
                if (!_bo.getChkKey().equalsIgnoreCase(_m_sCheckCode))
                {
                    //返回失败
                    _m_cCommiter.commitFailRes(LoginErr.LOGIN_CHECK_CODE_ERR.getCode());
                    return;
                }

                BM bmObj = NPLoginCheckServer.getInstance().getBM();
                //重新设置登录串
                _bo.clearMark();
                _bo.setLastLoginTime(bmObj, CommonFunc.getNowTimeSec());
                _bo.setChkKey(bmObj, CommonFunc.genRandomStr(32));
                _bo.saveAllMarked(bmObj);
                //直接处理结果，返回成功
                _m_cCommiter.commitSucRes(NP2LCS_RB_Writer_001_BasicOp.make_001_RetAccInfoSuc(_bo.getAccName(), _bo.getChkKey()));

            }
        });
    }

}
