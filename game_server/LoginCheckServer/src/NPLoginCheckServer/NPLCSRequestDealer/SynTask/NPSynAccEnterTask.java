package NPLoginCheckServer.NPLCSRequestDealer.SynTask;

import ALBasicServer.ALTask._IALAsynCallBackTask;
import ALBasicServer.ALTask._IALSynTask;
import LCSDB.Bo.AccountBO;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.LoginErr;
import NPCommon.Util.CommonFunc;
import NPLoginCheckServer.NPAccInfoMgr.NPAccInfoMgr;
import NPLoginCheckServer.NPAccInfoMgr.NPAccInfoMgr._ILoadAccountCallBack;
import NPLoginCheckServer.NPInternalCheckAccMgr;
import NPLoginCheckServer.NPLoginCheckServer;
import NPServerProtocolWriter.NP2LCS.RequestBack.NP2LCS_RB_Writer_001_BasicOp;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

/*************
 * 帐号进入游戏服务器的处理函数
 * 账号通过各种对应手段获取对应的UID，并返回
 * 最终根据实际的选择进入对应的US服务器
 * @author Administrator
 *
 */
public class NPSynAccEnterTask implements _IALSynTask
{
    /**
     * 帐号名称
     */
    private String _m_sAccName;
    private String _m_sAccPass;
    /**
     * 结果提交对象
     */
    private _IWCGBasicRequestCommiter _m_cCommiter;

    public NPSynAccEnterTask(String _accName, String _accPass, _IWCGBasicRequestCommiter _commiter)
    {
        _m_sAccName = _accName;
        _m_sAccPass = _accPass;
        _m_cCommiter = _commiter;
    }

    @Override
    public void run()
    {
        //检测账号
        if (!NPInternalCheckAccMgr.getInstance().judgeAcc(_m_sAccName, _m_sAccPass))
        {
            _m_cCommiter.commitFailRes(LoginErr.LOGIN_CHECK_CODE_ERR.getCode());
            return;
        }

        //先到管理对象中查询是否有对应数据，有则直接处理，无则开启数据库处理
        NPAccInfoMgr.getInstance().getAccInfoByName(_m_sAccName, new _ILoadAccountCallBack()
        {
            @Override
            public void onLoad(boolean isSucc, boolean _isExists, AccountBO _bo)
            {
                BM bmObj = NPLoginCheckServer.getInstance().getBM();

                if (!isSucc)
                {
                    _m_cCommiter.commitFailRes(LoginErr.LOGIN_CHECK_DB_UNAVAILABLE.getCode());
                }

                if (_isExists)
                {
                    //重新设置登录串
                    _bo.clearMark();
                    _bo.setChkKey(bmObj, CommonFunc.genRandomStr(32));
                    _bo.setLastLoginTime(bmObj, CommonFunc.getNowTimeSec());
                    _bo.saveAllMarked(bmObj);
                    //直接处理结果，返回成功
                    _m_cCommiter.commitSucRes(NP2LCS_RB_Writer_001_BasicOp.make_001_RetAccInfoSuc(_bo.getAccName(), _bo.getChkKey()));

                } else //账号不存在,创建新账号.
                {
                    AccountBO bo = NPAccInfoMgr.getInstance().getCachedAccInfoByName(_m_sAccName);
                    if (null != bo) //名字已经被注册了,此时无法新增用户数据，当做系统失败处理
                    {
                        _m_cCommiter.commitFailRes(LoginErr.LOGIN_NO_USER_INFO.getCode());
                    } else
                    {
                        // 创建临时的验证串
                        String chkKey = CommonFunc.genRandomStr(32);
                        bo = new AccountBO();
                        bo.setAccName(bmObj, _m_sAccName);
                        bo.setChkKey(bmObj, chkKey);
                        bo.setLastLoginTime(bmObj, CommonFunc.getNowTimeSec());

                        bo.insert(bmObj, new _IALAsynCallBackTask<AccountBO>()
                        {

                            @Override
                            public void dealFail()//数据库插入失败
                            {
                                _m_cCommiter.commitFailRes(LoginErr.LOGIN_CHECK_DB_UNAVAILABLE.getCode());
                            }

                            @Override
                            public void dealSuc(AccountBO _bo)
                            {
                                //放入数据管理对象
                                if (!NPAccInfoMgr.getInstance().addAccInfo(_bo))
                                {
                                    _m_cCommiter.commitFailRes(LoginErr.LOGIN_ADD_ACC_INFO_FAIL.getCode());
                                    return;
                                }

                                //提交结果
                                _m_cCommiter.commitSucRes(NP2LCS_RB_Writer_001_BasicOp.make_001_RetAccInfoSuc(_bo.getAccName(), _bo.getChkKey()));
                            }
                        });
                    }
                }
            }
        });
    }

}
