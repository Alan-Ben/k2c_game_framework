package NPUSServer.Common;

import ALBasicProtocolPack._IALProtocolStructure;
import NP2HS_RB.p001_HSOp.NP2HS_RB_001_004_RetCheckIsInWhiteList;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.Util.CallBack._ICallBackBool;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPPlayerVariableVarType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.Refs.Common.RefTimePrice;
import NPServerProtocolWriter.NP2HS.Np2HS_R_Writer_001_HSOP;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserServer;
import NPUSServer.USLog;
import WCGBasicServerCommon.WCGBasicServerCallbackSys._IWCGCallbackDealer;
import WCGCommon.Enum.NPEnum.ENPSingleServerType;
import WCGCommon.Enum.NPEnum.EServerType;

public class UsFunc
{
    /******
     * 按次数计算价格表价格
     * @param _UserData
     * @param _timesPriceId
     * @param _boughtNum
     * @return
     */
    public static NPCommonCostItem calCostPrice(NPUSUserData _UserData, int _timesPriceId, int _boughtNum)
    {
        RefTimePrice refPrice = RefTimePrice.getMgr().getPrice(_timesPriceId, _boughtNum + 1);
        if (refPrice == null)
        {
            return null;
        }
        NPVarInfo varInfo = new NPVarInfo();
        varInfo.addObj(ENPPlayerVariableVarType.BUY_TIMES.ordinal(), _boughtNum + 1);
        long itemCount = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_UserData, refPrice.cost_item_formula, varInfo);
        return new NPCommonCostItem(refPrice.item, itemCount);
    }

    /**
     * 判断是否是本服玩家
     * @param _cid 玩家cid
     * @return boolean
     */
    public static boolean isLocalCid(NPUserServer _server, Long _cid)
    {
        int usId = CommonFunc.parseServerTypeIdFromCid(_cid);
        //判断是否本服
        if (usId == _server.getServerTypeId())
            return true;

        return false;
    }

    /**
     * 检查是否在白名单中
     * @param _server
     * @param _uid
     * @param _callback
     */
    public static void checkInWhiteAccList(NPUserServer _server, String _uid, _ICallBackBool _callback)
    {
        _server.sendRequestToBSServer(EServerType.SINGLE.ordinal(), ENPSingleServerType.HTTP.ordinal()
                , Np2HS_R_Writer_001_HSOP.make_001_004_ReqCheckIsInWhiteList(_uid), new _IWCGCallbackDealer()
                {
                    @Override
                    public _IALProtocolStructure createProtocolObj()
                    {
                        return new NP2HS_RB_001_004_RetCheckIsInWhiteList();
                    }

                    @Override
                    public void dealSuc(_IALProtocolStructure _retProto)
                    {
                        NP2HS_RB_001_004_RetCheckIsInWhiteList ret = (NP2HS_RB_001_004_RetCheckIsInWhiteList) _retProto;

                        _callback.onRunOver(ret.getIsInWhiteList());
                    }

                    @Override
                    public void dealFail(int _errCode)
                    {
                        USLog.error(_server, "UsFunc checkInWhiteAccList deal fail uid:{} errCode:{}", _uid, _errCode);
                        _callback.onRunOver(false);
                    }
                });
    }
}
