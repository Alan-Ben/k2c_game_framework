package NPUSServer.NPUSUserMgr.GameSystem.BoxSystem;

import ALBasicProtocolPack._IALProtocolStructure;
import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultMgr;
import NPCommon.Util.CallBack._ICallBackResultT;
import NPCommon.Util.CommonFunc;
import NPUSServer.CommBoxMgr.CommBoxInfo;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_008_ReqGetBoxInfo;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_009_ReqAddBoxGainedCid;
import WCGCS2US_R.p003_CommOp.NP2US_R_003_010_ReqRemoveBoxGainedCid;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_008_RetGetBoxInfo;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_009_RetAddBoxGainedCid;
import WCGCS2US_RB.p003_CommOp.NP2US_RB_003_010_RetRemoveBoxGainedCid;
import WCGCommon.Enum.NPEnum.EServerType;

public class BoxSystem
{
    /*********
     * 获取宝箱数据
     * @param _instanceId
     * @param _cid
     * @param _callback
     */
    public static void getBox(NPUserServer _server, long _instanceId, long _cid
            , _ICallBackResultT<NP2US_RB_003_008_RetGetBoxInfo> _callback)
    {
        //宝箱所在US
        int usTypeId = CommonFunc.parseServerTypeIdFromCid(_instanceId);

        if (usTypeId == _server.getServerTypeId()) //本服宝箱
        {
            CommBoxInfo boxInfo = _server.getCommBoxMgr().lookupBox(_instanceId);
            if (null == boxInfo)
            {
                _callback.onRunOver(Result.SUCC, new NP2US_RB_003_008_RetGetBoxInfo());
                return;
            }

            NP2US_RB_003_008_RetGetBoxInfo ret = new NP2US_RB_003_008_RetGetBoxInfo();
            ret.setBoxId(boxInfo.getRef().id);
            ret.setBoxStatus(boxInfo.checkStatus(_cid));
            ret.getGainedCidList().addAll(boxInfo.getGainedCidList());

            _callback.onRunOver(Result.SUCC, ret);
        } else //跨服宝箱
        {
            NP2US_R_003_008_ReqGetBoxInfo proto = new NP2US_R_003_008_ReqGetBoxInfo();
            proto.setInstanceId(_instanceId);
            proto.setCid(_cid);

            _server.sendRequestToBSServer(EServerType.USER.ordinal(), usTypeId, proto
                    , new _IWCGCallbackDealer()
                    {

                        @Override
                        public void dealSuc(_IALProtocolStructure _proto)
                        {
                            NP2US_RB_003_008_RetGetBoxInfo ret = (NP2US_RB_003_008_RetGetBoxInfo) _proto;
                            _callback.onRunOver(Result.SUCC, ret);
                        }

                        @Override
                        public void dealFail(int _errCode)
                        {
                            USLog.error(_server, "BoxSystem.getBox from CrossUser Fail, cid:{} targetUs:{} errCode:{}", _cid, usTypeId, _errCode);
                            _callback.onRunOver(ResultMgr.getInstance().lookupResult(_errCode), null);
                        }

                        @Override
                        public _IALProtocolStructure createProtocolObj()
                        {
                            return new NP2US_RB_003_008_RetGetBoxInfo();
                        }
                    });
        }
    }

    /*******
     * 添加宝箱已领取玩家列表
     * @param _instanceId
     * @param _cid
     * @param _callback
     */
    public static void addBoxGainedCid(NPUserServer _server, long _instanceId, long _cid
            , _ICallBackResultT<NP2US_RB_003_009_RetAddBoxGainedCid> _callback)
    {
        //宝箱所在US
        int usTypeId = CommonFunc.parseServerTypeIdFromCid(_instanceId);

        if (usTypeId == _server.getServerTypeId()) //本服宝箱
        {
            CommBoxInfo boxInfo = _server.getCommBoxMgr().lookupBox(_instanceId);
            if (null == boxInfo)
            {
                _callback.onRunOver(Result.SUCC, new NP2US_RB_003_009_RetAddBoxGainedCid());
                return;
            }

            NP2US_RB_003_009_RetAddBoxGainedCid ret = new NP2US_RB_003_009_RetAddBoxGainedCid();
            ret.setRes(boxInfo.setGainedCid(_cid));
            ret.setBoxId(boxInfo.getRef().id);
            ret.setBoxStatus(boxInfo.checkStatus(_cid));
            ret.getGainedCidList().addAll(boxInfo.getGainedCidList());

            _callback.onRunOver(Result.SUCC, ret);
        } else //跨服宝箱
        {
            NP2US_R_003_009_ReqAddBoxGainedCid proto = new NP2US_R_003_009_ReqAddBoxGainedCid();
            proto.setInstanceId(_instanceId);
            proto.setCid(_cid);

            _server.sendRequestToBSServer(EServerType.USER.ordinal(), usTypeId, proto, new _IWCGCallbackDealer()
            {

                @Override
                public void dealSuc(_IALProtocolStructure _proto)
                {
                    NP2US_RB_003_009_RetAddBoxGainedCid ret = (NP2US_RB_003_009_RetAddBoxGainedCid) _proto;
                    _callback.onRunOver(Result.SUCC, ret);
                }

                @Override
                public void dealFail(int _errCode)
                {
                    USLog.error(_server, "BoxSystem.addBoxGainedCid from CrossUser Fail, cid:{} targetUs:{} errCode:{}", _cid, usTypeId, _errCode);
                    _callback.onRunOver(ResultMgr.getInstance().lookupResult(_errCode), null);
                }

                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new NP2US_RB_003_009_RetAddBoxGainedCid();
                }
            });
        }
    }

    /**********
     * 移除宝箱指定领取玩家记录
     * @param _instanceId
     * @param _cid
     */
    public static void removeBoxGainedCid(NPUserServer _server, long _instanceId, long _cid)
    {
        //宝箱所在US
        int usTypeId = CommonFunc.parseServerTypeIdFromCid(_instanceId);

        if (usTypeId == _server.getServerTypeId()) //本服宝箱
        {
            CommBoxInfo boxInfo = _server.getCommBoxMgr().lookupBox(_instanceId);
            if (null == boxInfo)
            {
                return;
            }

            boxInfo.clearGainedCid(_cid);
        } else //跨服宝箱
        {
            NP2US_R_003_010_ReqRemoveBoxGainedCid proto = new NP2US_R_003_010_ReqRemoveBoxGainedCid();
            proto.setInstanceId(_instanceId);
            proto.setCid(_cid);

            _server.sendRequestToBSServer(EServerType.USER.ordinal(), usTypeId, proto, new _IWCGCallbackDealer()
            {

                @Override
                public void dealSuc(_IALProtocolStructure _proto)
                {
                }

                @Override
                public void dealFail(int _errCode)
                {
                    USLog.error(_server, "BoxSystem.removeBoxGainedCid from CrossUser Fail, us:{} cid:{}", usTypeId, _cid);
                }

                @Override
                public _IALProtocolStructure createProtocolObj()
                {
                    return new NP2US_RB_003_010_RetRemoveBoxGainedCid();
                }
            });
        }
    }
}
