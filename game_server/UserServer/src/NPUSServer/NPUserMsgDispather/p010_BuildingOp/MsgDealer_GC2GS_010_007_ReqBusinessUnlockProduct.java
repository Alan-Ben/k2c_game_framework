package NPUSServer.NPUserMsgDispather.p010_BuildingOp;

import GC2GS.p010_BuildingOp.GC2GS_010_007_ReqBusinessUnlockProduct;
import NPCommon.ErrMain.BuildingErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.ErrMain.Result.Result;
import NPGameRes.Refs.Building.RefBusinessBuildingProduct;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.BuildingInfo;
import NPUSServer.NPUSUserMgr.UserComp.BuildingComp.Function.BuildingBusinessFunc;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_010_BuildingOp;

public class MsgDealer_GC2GS_010_007_ReqBusinessUnlockProduct extends NPUserMsgDealer<GC2GS_010_007_ReqBusinessUnlockProduct>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_010_007_ReqBusinessUnlockProduct _msg)
    {
        NPUSUserData userData = _commiter.getUserData();

        RefBusinessBuildingProduct refProduct = RefBusinessBuildingProduct.getMgr().get(_msg.getRefId());
        if (null == refProduct)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //获取建筑数据
        BuildingInfo info = userData.getBuildingComponent().lookupBuilding(refProduct.building_id);
        if(null == info)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_NOT_EXIST.getCode());
        	return;
        }
        
        //检查是否经营建筑
        BuildingBusinessFunc businessFunc = info.getBusiness();
        if(null == businessFunc)
        {
        	_commiter.commitFailRes(BuildingErr.BUILDING_NO_TYPE.getCode());
        	return;
        }

        Result result = businessFunc.unlockProduct(_msg.getRefId());
        if (!result.isSucc())
        {
            _commiter.commitFailRes(result.getCode());
            return;
        }

        _commiter.commitSucRes(US2GCWriter_010_BuildingOp.make_007_RetBusinessUnlockProduct());
    }
}