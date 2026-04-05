package NPUSServer.GMCommand.Cmds;

import NPCommon.ErrMain.Result.Result;
import NPCommon.ErrMain.Result.ResultOne;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.PushGift.RefPushGiftGroup;
import NPGameRes.Refs.PushGift.RefPushGiftPack;
import NPUSServer.GMCommand.UsCmdBase;
import NPUSServer.NPUSUserMgr.UserComp.OrderComp.OrderGoodsData;
import NPUSServer.NPUSUserMgr.UserComp.PushGiftPackComp.PushGiftGroupInfo;

@ACommander(comment = "订单相关命令", name = "order")
public class CmdOrder extends UsCmdBase
{
    @ACommand(comment = "设置支付[订单号]")
    public String setPay(String _orderId)
    {
        return getOwner().getOrderComponent().gmPay(_orderId, getContext()).toString();
    }

    @ACommand(comment = "创建订单[商品类型,商品ID]")
    public String createOrder(long _goodsId)
    {
        ResultOne<OrderGoodsData> result = getOwner().getOrderComponent().createOrder(_goodsId, getContext());
        if (!result.isSucc())
            return "fail";

        return result.getData().getOrderId();
    }

    @ACommand(comment = "清除礼包购买记录[礼包ID，0表示清除所有]")
    public String clearRecord(long _giftPackId)
    {
        Result result = getOwner().getOrderComponent().getMgr().clearPurchaseRecord(_giftPackId, getContext());
        if (!result.isSucc())
        {
            return "fail: " + result.toString();
        }

        if (_giftPackId == 0)
        {
            return "已清除所有礼包购买记录";
        } else
        {
            return "已清除礼包[" + _giftPackId + "]的购买记录";
        }
    }

    @ACommand(comment = "显示礼包购买记录信息")
    public String info()
    {
        return getOwner().getOrderComponent().getMgr().toString();
    }

    @ACommand(comment = "尝试触发推送礼包组[礼包组ID]")
    public String tryTriggerPushPack(long _groupId)
    {
        RefPushGiftGroup refGroup = RefPushGiftGroup.getMgr().get(_groupId);
        if (refGroup == null)
            return "推送礼包组[" + _groupId + "]配置不存在";

        PushGiftGroupInfo groupInfo = getOwner().getPushGiftPackComponent().ensureGroupInfo(refGroup);

        return groupInfo.tryTrigger(false, getContext()).toString();
    }

    @ACommand(comment = "修改推送礼包组的触发时间[礼包组ID,还有多久过期(秒)]")
    public String chgPushGiftPackGroupTriggerTime(long _groupId, long _remainSeconds)
    {
        PushGiftGroupInfo groupInfo = getOwner().getPushGiftPackComponent().lookupGroupInfo(_groupId);
        if (groupInfo == null)
        {
            return "推送礼包组[" + _groupId + "]不存在";
        }
        long newTriggerTimeMs = CommonFunc.getNowTimeMS() + _remainSeconds * 1000 - groupInfo.getRef().next_trigger_need_seconds * 1000;
        groupInfo.chgTriggerTimeMs(newTriggerTimeMs);
        return "ok";
    }

    @ACommand(comment = "修改推送礼包的激活时间[礼包组ID,还有多久过期(秒)]")
    public String chgPushGiftPackTriggerTime(long _groupId, long _remainSeconds)
    {
        PushGiftGroupInfo groupInfo = getOwner().getPushGiftPackComponent().lookupGroupInfo(_groupId);
        if (groupInfo == null)
        {
            return "推送礼包组[" + _groupId + "]不存在";
        }

        RefPushGiftPack pushGiftRef = groupInfo.getPushGiftRef();
        if (pushGiftRef == null)
        {
            return "推送礼包组[" + _groupId + "]的推送礼包不存在";
        }

        long newActiveTimeMs = CommonFunc.getNowTimeMS() + _remainSeconds * 1000 - pushGiftRef.continue_time * 1000;

        groupInfo.chgActiveTimeMs(newActiveTimeMs);

        return "ok";
    }

    @ACommand(comment = "清除推送礼包组信息[礼包组ID]")
    public String clearPushGiftPackGroupInfo(long _groupId)
    {
        PushGiftGroupInfo groupInfo = getOwner().getPushGiftPackComponent().lookupGroupInfo(_groupId);
        if (groupInfo == null)
        {
            return "推送礼包组[" + _groupId + "]不存在";
        }

        groupInfo.clearGroupInfo();
        return "ok";
    }
}
