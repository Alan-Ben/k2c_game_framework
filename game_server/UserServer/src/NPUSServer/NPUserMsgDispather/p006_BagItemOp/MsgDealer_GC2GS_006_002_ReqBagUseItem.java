package NPUSServer.NPUserMsgDispather.p006_BagItemOp;

import GC2GS.p006_BagItemOp.GC2GS_006_002_ReqBagUseItem;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.BagItemErr;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerVariableVarType;
import NPGameRes.GameObjs.CommonObj.VarInfo.NPVarInfo;
import NPGameRes.Refs.BagItem.RefBagItemUse;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.EffectDealer.NPPlayerEffectDealer;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.BagItemComp.BagItemInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_006_BagItemOp;
import USLOGDB.OptBo.Opt006002BagUseItemBO;

import java.util.List;

public class MsgDealer_GC2GS_006_002_ReqBagUseItem extends NPUserMsgDealer<GC2GS_006_002_ReqBagUseItem>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _commiter, GC2GS_006_002_ReqBagUseItem _msg)
    {
        //获取用户对象，基类有做空判断，这边不做处理
        NPUSUserData userData = _commiter.getUserData();

        //检查参数
        if(_msg.getCount() < 0 || _msg.getCount() > RefGeneral.Ref().batch_use_item_max_count)
        {
        	_commiter.commitFailRes(CommErr.NUM_REACH_LIMIT.getCode());
        	return;
        }

        //背包物品检查
        BagItemInfo info = userData.getBagItemComponent().getBagItem(_msg.getItemId());
        if (null == info)
        {
            _commiter.commitFailRes(BagItemErr.BAG_ITEM_NOT_EXISTS_ERROR.getCode());
            return;
        }

        RefBagItemUse useRef = info.getBagItemUseRef();
        if (null == useRef)
        {
            _commiter.commitFailRes(BagItemErr.BAG_ITEM_CAN_NOT_USE_ERROR.getCode());
            return;
        }

        //物品数量检查
        if (!userData.hasItem(ENPItemType.BAG_ITEM, _msg.getItemId(), _msg.getCount()))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        //使用条件检查
        if (!NPPlayerConditionDealerMgr.IsEnable(useRef.use_cond, userData, null))
        {
            _commiter.commitFailRes(BagItemErr.BAG_ITEM_USE_CONDITION_ERROR.getCode());
            return;
        }

        //奖励物品数量检查
        if (!useRef.optionCheck(_msg.getSelectedIdx()))
        {
            _commiter.commitFailRes(BagItemErr.BAG_ITEM_USE_OPTION_ERROR.getCode());
            return;
        }

        //使用消耗 乘以使用次数
        List<NPCommonCostItem> costItems = CommonFunc.itemMultiple(useRef.cost_item_list.getItemTypeObjList(), (int) _msg.getCount());
        //消耗物品检查
        if (!userData.hasCostItemList(costItems))
        {
            _commiter.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }

        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.USE_BAG_ITEM);
        if (!userData.spendItem(costItems, context))
        {
            _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        //使用背包物品
        //useRef.use_not_cost字段含义：true-不需要消耗，false（默认）-需要消耗
        if (!useRef.use_not_cost)
        {
            if (!userData.spendItem(ENPItemType.BAG_ITEM, _msg.getItemId(), _msg.getCount(), context))
            {
                _commiter.commitFailRes(CommErr.CONSUME_FAIL.getCode());
                return;
            }
        }

        //批量执行的条件检查，与effects效果一致
        boolean batchCondition = false;
        //只有在效果有效的时候才会触发
        for (int i = 0; i < _msg.getCount(); i++)
        {
            //执行效果（前置效果一定会优先执行）
            boolean preCondition = false;
            if (NPPlayerConditionDealerMgr.IsEnable(useRef.pre_use_cond, userData, null))
            {
                preCondition = true;
                NPPlayerEffectDealer.dealEffect(useRef.pre_effects.getPlayerEffectList(), userData, null, context);
            }
            // true == is_all_effect_deal，是否前置执行效果具有更高优先级，如无则两者一定都会执行，true则优先判断前置是否执行
            // false == is_all_effect_deal，
            if (!useRef.is_pre_high_pri
                    || (useRef.is_pre_high_pri && !preCondition))
            {
                if (NPPlayerConditionDealerMgr.IsEnable(useRef.use_cond, userData, null))
                {
                	batchCondition = true;
                    NPPlayerEffectDealer.dealEffect(useRef.effects, userData, null, context);
                }
            }
        }
        //执行批量效果
        if(batchCondition)
        {
            //数值信息-客户端传入使用次数参数
            NPVarInfo varInfo = new NPVarInfo();
            varInfo.addObj(ENPPlayerVariableVarType.USE_COUNT.ordinal(), _msg.getCount());
        	//执行批量效果
        	NPPlayerEffectDealer.dealEffect(useRef.batch_effects, userData, varInfo, context);
        }
        
        //发送奖励物品，多个的时候按照选择发送，如果只有一个则直接发送
        if (useRef.option_item_list.getItemTypeObjList().size() > 1)
        {
            for (int i = 0; i < _msg.getSelectedIdx().size(); i++)
            {
                NPCommonCostItem obj = useRef.option_item_list.getItemTypeObjList().get(_msg.getSelectedIdx().get(i));
                if (null == obj)
                    continue;

                userData.gainItem(obj.getItemType(), obj.getItemId(), obj.getCount() * _msg.getCount(), context);
            }
        } else if (useRef.option_item_list.getItemTypeObjList().size() == 1)
        {
            NPCommonCostItem obj = useRef.option_item_list.getItemTypeObjList().get(0);
            if (null != obj)
                userData.gainItem(obj.getItemType(), obj.getItemId(), obj.getCount() * _msg.getCount(), context);
        }

        //给予确认的reward奖励
        if (useRef.get_reward_id != 0)
        {
            userData.gainItem(ENPItemType.REWARD, useRef.get_reward_id, _msg.getCount(), context);
        }

        _commiter.commitSucRes(US2GCWriter_006_BagItemOp.make_002_RetBagUseItemSucc(_msg.getItemId(), _msg.getCount(), context.getCollector()));

        //玩家使用道具日志
        Opt006002BagUseItemBO optLog = new Opt006002BagUseItemBO();
        optLog.setItemId(getUSServer().getBM(), _msg.getItemId());
        optLog.setCount(getUSServer().getBM(), _msg.getCount());
        userData.logEvent(optLog, context);
    }
}
