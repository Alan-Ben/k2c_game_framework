package NPUSServer.NPUserMsgDispather.p006_BagItemOp;

import GC2GS.p006_BagItemOp.GC2GS_006_007_ReqConvert;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.DB.BM.BM;
import NPCommon.ErrMain.CommErr;
import NPCommon.NPLogDB.CommLogDB;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPGameRes.Refs.BagItem.RefBagItemConvert;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_006_BagItemOp;
import NPUSServer.USLog;
import USLOGDB.Bo.LogItemConvertBO;

import java.util.List;

/**
 * 准备使用藏宝地图
 */
public class MsgDealer_GC2GS_006_007_ReqConvert extends NPUserMsgDealer<GC2GS_006_007_ReqConvert>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_006_007_ReqConvert _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //参数物品数量检查
        if(!userData.checkItemCount(_msg.getBagItemCount()))
        {
            _commiter.commitFailRes(CommErr.PARAM_NUM_ERROR.getCode());
            return;
        }

        RefBagItemConvert refBagItemConvert = RefBagItemConvert.getMgr().get(_msg.getBagItemId());
        if (refBagItemConvert == null)
        {
            USLog.error(getUSServer(), "NPGC2GS_006_007_ReqConvert refBagItemConvert not found refId:{}", _msg.getBagItemId());
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }
        //检查是否拥有物品
        List<NPCommonCostItem> costItems = refBagItemConvert.calConvertTotalConsume((int) _msg.getBagItemCount());
        if (!userData.hasCostItemList(costItems))
        {
            //物品不足
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }
        //消耗
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.ITEM_CONVERT);
        if (!userData.spendCostItemList(costItems, context))
        {
            //物品不足
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        //发放兑换奖励
        NPCommonCostItem rewardItem = CommonFunc.itemMultiple(new NPCommonCostItem(refBagItemConvert.target_item.getItemType(), refBagItemConvert.target_item.getItemId(), 1), (int) _msg.getBagItemCount());
        userData.gainItem(rewardItem, context);

        //推送获取物品协议
        if (!context.getCollector().isEmpty())
        {
            userData.sendMsgToGC(context.getCollector().toProto());
        }

        _commiter.commitSucRes(US2GCWriter_006_BagItemOp.make_007_RetConvert());

        try
        {
            BM bmObj = userData.getUSServer().getBM();

            //道具合成日志
            LogItemConvertBO logBo = new LogItemConvertBO();
            logBo.setCid(bmObj, userData.getCid());
            logBo.setOriItemInfo(bmObj, String.format("%s-%d:%d", ENPItemType.BAG_ITEM, refBagItemConvert.bag_item_id, refBagItemConvert.ori_item_num * _msg.getBagItemCount()));
            logBo.setTargetItemInfo(bmObj, rewardItem.toString());
            CommLogDB.log(bmObj, logBo, context);
        } catch (Exception e)
        {
            USLog.error(getUSServer(), "", e);
        }
    }
}