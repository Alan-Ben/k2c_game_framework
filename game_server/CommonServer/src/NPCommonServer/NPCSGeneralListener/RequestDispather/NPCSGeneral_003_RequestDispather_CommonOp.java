package NPCommonServer.NPCSGeneralListener.RequestDispather;

import NP2CS_R.p003_CommonOp.ToCS_R_003_001_GetShareData;
import NPCommon.Dispather.NPRequestDispatcher;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.Delegate.HandlerTwo;
import NPCommonServer.CachedShareData.CacheShareDataGetter;
import NPCommonServer.CachedShareData.CachedShareData;
import NPCommonServer.CachedShareData.CachedShareDataMgr;
import NPCommonServer.NPCSGeneralListener.Writer.NP2CS_RB_Writer_003_CommonOp;
import WCGBasicServer.WCGBSRecieverListener._AWCGBSReceiverListener;
import WCGBasicServerCommon.WCGBasicServerRequestProtocolSys._IWCGBasicRequestCommiter;

public class NPCSGeneral_003_RequestDispather_CommonOp extends NPRequestDispatcher
{
    public static void init(NPCSGeneralRequestDispather _dispather)
    {
        _dispather.regHandler(new NPRequestDealer<ToCS_R_003_001_GetShareData>()
        {
            @Override
            protected void _dealMessage(_IWCGBasicRequestCommiter _committer, final ToCS_R_003_001_GetShareData _msg)
            {
                //获取对应的处理服务器对象
                _AWCGBSReceiverListener listener = (_AWCGBSReceiverListener) _committer.getRequestDealer();
                if (null == listener)
                {
                    _committer.commitFailRes(CommErr.OBJ_ERR.getCode());
                    return;
                }

                CachedShareDataMgr cachedMgr = CacheShareDataGetter.getInstance().getCachedMgr(_msg.getType());
                cachedMgr.getData(_msg.getSerial(), new HandlerTwo<Boolean, CachedShareData>()
                {
                    @Override
                    public void handle(Boolean _isSucc, CachedShareData _data)
                    {
                        //返回信息
                        if (null == _data)
                        {
                            _committer.commitFailRes(CommErr.SYS_ERR.getCode());
                            return;
                        }

                        _committer.commitSucRes(NP2CS_RB_Writer_003_CommonOp.make_001_RetSpaceSerialize(_data.getData()));
                    }
                });
            }
        });
    }
}
