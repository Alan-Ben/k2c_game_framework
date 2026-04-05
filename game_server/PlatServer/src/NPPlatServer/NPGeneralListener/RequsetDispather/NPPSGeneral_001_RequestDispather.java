package NPPlatServer.NPGeneralListener.RequsetDispather;

import NP2PS_R.p001_BasicOp.*;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.PSErr;
import NPPlatServer.NPGeneralListener.Writer.NP2PS_RB_Writer_001_BasicOp;
import NPPlatServer.NPPS_Listener.*;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaInfo;
import NPPlatServer.NPPlatAreaMgr.NPPlatAreaMgr;
import NPPlatServer.NPPlatServerMgr.NPPlatCGSMgr;
import NPPlatServer.NPPlatServerMgr.NPPlatCRSMgr;
import NPPlatServer.NPPlatServerMgr.NPPlatGLSMgr;
import NPPlatServer.NPPlatServerMgr.NPPlatUSMgr;
import NPPlatServer.PlatListenerMgr.PlatListenerMgr;
import WCGBasicPlatServer.BasicServerListener._AWCGBasicServerListener;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

import java.util.List;

public class NPPSGeneral_001_RequestDispather extends NPRequestDispatcher
{
    public static void init(NPPSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_001_RegCS>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_001_RegCS _msg)
            {
                //返回对应协议
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_001_RetRegInfo());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_002_RegGS>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_002_RegGS _msg)
            {
                // 注册服务器的区域类型，并进行处理
                NPPlatAreaInfo areaInfo = NPPlatAreaMgr.getInstance().getAreaInfo(_msg.getAreaTag());
                if (null == areaInfo)
                {
                    _committer.commitFailRes(PSErr.PS_NO_AREA.getCode());
                    return;
                }

                // 获取服务器对象
                PS_GSListener gsListener = (PS_GSListener) _committer.getRequestDealer();
                if (null == gsListener)
                {
                    _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
                    return;
                }

                // 初始化服务器信息
                gsListener.initServerInfo(areaInfo.getAreaSerialize(), _msg.getUserMaxCount(),
                        _msg.getUserHandleWeight(), _msg.getConnectIp(), _msg.getConnectPort());
                // 注册服务器
                areaInfo.regGS(gsListener);

                // 返回对应协议
                _committer
                        .commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_002_RetGSRegInfo(areaInfo));

            }
        });
        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_003_RegLCS>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_003_RegLCS _msg)
            {
                //lcs服务器无区域，使用公用区域log服务器
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_003_RetLCSRegInfo());

            }
        });
        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_004_RegLSServer>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_004_RegLSServer _msg)
            {
                // 注册服务器的区域类型，并进行处理
                NPPlatAreaInfo areaInfo = NPPlatAreaMgr.getInstance().getAreaInfo(_msg.getAreaTag());
                if (null == areaInfo)
                {
                    _committer.commitFailRes(PSErr.PS_NO_AREA.getCode());
                    return;
                }

                // 获取服务器对象
                PS_LSListener lsListener = (PS_LSListener) _committer.getRequestDealer();
                if (null == lsListener)
                {
                    _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
                    return;
                }

                // 初始化服务器信息
                lsListener.initInfo(_msg.getAreaTag(), areaInfo.getAreaSerialize());
                // 注册服务器
                areaInfo.regLS(lsListener);

                // 返回对应协议
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_004_RetLSRegInfo(areaInfo));

            }
        });
        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_007_RegUS>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_007_RegUS _msg)
            {
                // 获取服务器对象
                PS_USListener usListener = (PS_USListener) _committer.getRequestDealer();
                if (null == usListener)
                {
                    _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
                    return;
                }

                //注册服务器
                NPPlatUSMgr.getInstance().regUS(usListener);

                // 返回对应协议
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_007_RetUSRegInfo());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_009_RegCRS>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_009_RegCRS _msg)
            {
                //获取服务器对象
                PS_CRSListener crsListener = (PS_CRSListener) _committer.getRequestDealer();
                if (null == crsListener)
                {
                    _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
                    return;
                }

                //注册服务器
                NPPlatCRSMgr.getInstance().regCRS(crsListener);

                //返回对应协议
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_009_RetCRSRegInfo());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_010_RegCGS>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_010_RegCGS _msg)
            {
                //注册服务器的区域类型，并进行处理
                NPPlatAreaInfo areaInfo = NPPlatAreaMgr.getInstance().getAreaInfo(_msg.getAreaTag());
                if (null == areaInfo)
                {
                    _committer.commitFailRes(PSErr.PS_NO_AREA.getCode());
                    return;
                }
                PS_CGSListener listener = (PS_CGSListener) _committer.getRequestDealer();

                NPPlatCGSMgr.getInstance().regCGS(listener);

                //返回对应协议
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_010_RetCGSRegInfo(areaInfo.getAreaSerialize()));
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_011_ReqOnlineServerList>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_011_ReqOnlineServerList _msg)
            {
                List<_AWCGBasicServerListener> listeners = PlatListenerMgr.getInstance().getListenerListOfType(_msg.getServerType());

                //返回对应协议
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_011_RetOnlineServerList(listeners));
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_012_ReqOnlineRefServerList>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_012_ReqOnlineRefServerList _msg)
            {
                List<_AWCGBasicServerListener> listeners = PlatListenerMgr.getInstance().getAllListenerList();

                //返回对应协议
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_012_RetOnlineRefServerList(listeners));
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_013_RegCrossDataServer>()
        {

            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_013_RegCrossDataServer _msg)
            {
                List<_AWCGBasicServerListener> listeners = PlatListenerMgr.getInstance().getAllListenerList();

                //返回对应协议
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_013_RegCrossDataServer());
            }
        });

        _dispather.regHandler(new NPRequestDealer<NP2PS_R_001_014_RegGameLogicServer>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, NP2PS_R_001_014_RegGameLogicServer _msg)
            {
                PS_GLSListener listener = (PS_GLSListener) _committer.getRequestDealer();

                NPPlatGLSMgr.getInstance().reg(listener);

                //返回对应协议
                _committer.commitSucRes(NP2PS_RB_Writer_001_BasicOp.make_014_RegGameLogicServer());
            }
        });
    }
}
