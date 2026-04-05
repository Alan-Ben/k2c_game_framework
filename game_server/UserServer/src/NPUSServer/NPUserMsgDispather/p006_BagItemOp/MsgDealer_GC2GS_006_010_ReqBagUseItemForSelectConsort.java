package NPUSServer.NPUserMsgDispather.p006_BagItemOp;

import Common.BagItemUseEnum.EBagItemUse_ConsortDrawShowType;
import Common.BagItemUseEnum.EBagItemUse_ConsortType;
import GC2GS.p006_BagItemOp.GC2GS_006_010_ReqBagUseItemForSelectConsort;
import NPCommon.CommonObj.ShowItemCollector.Sub.ConsortShowItemCollector;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPEnum.ENPPlayerVariableVarType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.Refs.BagItem.RefBagItemConsort;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_CONSORT_SEND_GIFT;
import NPUSServer.NPUSUserMgr.UserComp.ConsortComp.ConsortInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_006_BagItemOp;
import NPUSServer.USLog;

public class MsgDealer_GC2GS_006_010_ReqBagUseItemForSelectConsort extends NPUserMsgDealer<GC2GS_006_010_ReqBagUseItemForSelectConsort>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_006_010_ReqBagUseItemForSelectConsort _msg)
    {
        //参数物品数量检查
        if(!_commiter.getUserData().checkItemCount(_msg.getCount()))
        {
            _commiter.commitFailRes(CommErr.PARAM_NUM_ERROR.getCode());
            return;
        }
        
        //查找对应配置
        RefBagItemConsort refItem = RefBagItemConsort.getMgr().get(_msg.getItemId());
        if (null == refItem)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //数值信息
        NPVarInfo varInfo = new NPVarInfo();
        varInfo.addObj(ENPPlayerVariableVarType.CONSORT_ID.ordinal(), _msg.getConsortId());

        //消耗道具
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.USE_BAG_ITEM);
        if (!_commiter.getUserData().spendItem(ENPItemType.BAG_ITEM, _msg.getItemId(), _msg.getCount(), context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        ConsortShowItemCollector collector = new ConsortShowItemCollector();

        //由于高级公式内的数值可能是随机值, 所以这边开个循环
        for (int i = 0; i < _msg.getCount(); i++)
        {
            int value = (int) NPPlayerVariableDeal.getInstance().CalculateVariableResult(_commiter.getUserData(), refItem.value, null);

            //计算妃子id
            long consortId = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_commiter.getUserData(), refItem.consort_id, varInfo);
            if (consortId == 0)
            {
                USLog.error(getUSServer(), "GC2GS_006_010_ReqBagUseItemForSelectConsort cal consortId fail cid:{} itemId:{}", _commiter.getUserData().getCid(), _msg.getItemId());
                continue;
            }

            //妃子信息
            ConsortInfo consortInfo = _commiter.getUserData().getConsortComponent().lookup(consortId);
            if (consortInfo == null)
            {
                USLog.error(getUSServer(), "GC2GS_006_010_ReqBagUseItemForSelectConsort cal consortId fail cid:{} itemId:{}", _commiter.getUserData().getCid(), _msg.getItemId());
                continue;
            }

            //区分道具类型加资源
            if (refItem.show_type == EBagItemUse_ConsortType.INTIMACY)
            {
                consortInfo.incrIntimacy(value, context);
                //记录获得道具
                collector.record(consortId, EBagItemUse_ConsortDrawShowType.INTIMACY, value);
            } else if (refItem.show_type == EBagItemUse_ConsortType.CHARM)
            {
                consortInfo.incrCharm(value, context);
                //记录获得道具
                collector.record(consortId, EBagItemUse_ConsortDrawShowType.CHARM, value);
            } else if (refItem.show_type == EBagItemUse_ConsortType.CHARM_POINT)
            {
                consortInfo.incrCharmPoint(value, context);
                //记录获得道具
                collector.record(consortId, EBagItemUse_ConsortDrawShowType.CHARM_POINT, value);
            }
        }

        _commiter.commitSucRes(US2GCWriter_006_BagItemOp.make_010_RetBagUseItemForSelectConsort(collector.makeProto()));

        //记录赠送妃子道具次数
        if (refItem.show_type == EBagItemUse_ConsortType.INTIMACY || refItem.show_type == EBagItemUse_ConsortType.CHARM)
        {
            _commiter.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.CONSORT_SEND_GIFT_TIMES, _msg.getCount(), context);

            _commiter.getUserData().onLogicEvent(new Event_P_CONSORT_SEND_GIFT(context, _msg.getCount()));
        }
    }
}
