package NPUSServer.NPUserMsgDispather.p006_BagItemOp;

import Common.BagItemUseEnum.EBagItemUse_HeroDrawShowType;
import Common.BagItemUseEnum.EBagItemUse_HeroType;
import GC2GS.p006_BagItemOp.GC2GS_006_009_ReqBagUseItemForSelectHero;
import NPCommon.CommonObj.ShowItemCollector.Sub.HeroShowItemCollector;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerVariableVarType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.Refs.BagItem.RefBagItemHero;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.UserComp.HeroComp.HeroInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUSUserMgr.VariableDealer.NPPlayerVariableDeal;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_006_BagItemOp;
import NPUSServer.USLog;

public class MsgDealer_GC2GS_006_009_ReqBagUseItemForSelectHero extends NPUserMsgDealer<GC2GS_006_009_ReqBagUseItemForSelectHero>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_006_009_ReqBagUseItemForSelectHero _msg)
    {
        //参数物品数量检查
        if(!_commiter.getUserData().checkItemCount(_msg.getCount()))
        {
            _commiter.commitFailRes(CommErr.PARAM_NUM_ERROR.getCode());
            return;
        }
        
        //查找对应配置
        RefBagItemHero refItem = RefBagItemHero.getMgr().get(_msg.getItemId());
        if (null == refItem)
        {
            _commiter.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //数值信息
        NPVarInfo varInfo = new NPVarInfo();
        varInfo.addObj(ENPPlayerVariableVarType.HERO_ID.ordinal(), _msg.getHeroId());

        //消耗道具
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.USE_BAG_ITEM);
        if (!_commiter.getUserData().spendItem(ENPItemType.BAG_ITEM, _msg.getItemId(), _msg.getCount(), context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        HeroShowItemCollector collector = new HeroShowItemCollector();

        //由于高级公式内的数值可能是随机值, 所以这边开个循环处理
        for (int i = 0; i < _msg.getCount(); i++)
        {
            long value = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_commiter.getUserData(), refItem.value, null);
            //计算大臣id
            long heroId = NPPlayerVariableDeal.getInstance().CalculateVariableResult(_commiter.getUserData(), refItem.hero_id, varInfo);
            if (heroId == 0)
            {
                USLog.error(getUSServer(), "GC2GS_006_009_ReqBagUseItemForSelectHero cal heroId fail cid:{} itemId:{}", _commiter.getUserData().getCid(), _msg.getItemId());
                continue;
            }

            //大臣信息
            HeroInfo heroInfo = _commiter.getUserData().getHeroComponent().lookupHero(heroId);
            if (heroInfo == null)
            {
                USLog.error(getUSServer(), "GC2GS_006_009_ReqBagUseItemForSelectHero cal heroId fail cid:{} itemId:{}", _commiter.getUserData().getCid(), _msg.getItemId());
                continue;
            }

            //区分道具类型给大臣加资源
            if (refItem.show_type == EBagItemUse_HeroType.POWER)
            {
                heroInfo.addItemAddPower(value, context);

                collector.record(heroId, EBagItemUse_HeroDrawShowType.POWER, (int) value);
            }
        }

        _commiter.commitSucRes(US2GCWriter_006_BagItemOp.make_009_RetBagUseItemForSelectHero(collector.makeProto()));
    }
}
