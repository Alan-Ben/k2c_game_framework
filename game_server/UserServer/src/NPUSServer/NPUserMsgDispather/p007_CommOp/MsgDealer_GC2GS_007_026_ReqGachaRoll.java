package NPUSServer.NPUserMsgDispather.p007_CommOp;

import GC2GS.p007_CommOp.GC2GS_007_026_ReqGachaRoll;
import MJLog.MJEventLog;
import NPCommon.ErrMain.CommErr;
import NPCommon.Util.CommonFunc;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.AvatarGacha.RefGachaItem;
import NPGameRes.Refs.AvatarGacha.RefGachaPool;
import NPGameRes.Refs.RefGeneral;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.CommonActivityMgr.Core._AActivityBase;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.NPUSUserData;
import NPUSServer.NPUSUserMgr.UserComp.GachaComp.GachaPoolInfo;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;
import NPUSServer.NPUserMsgDispather.Write.US2GCWriter_007_CommOp;
import NPUSServer.USLog;
import com.google.gson.JsonArray;
import com.google.gson.JsonPrimitive;

import java.util.ArrayList;
import java.util.List;

public class MsgDealer_GC2GS_007_026_ReqGachaRoll extends NPUserMsgDealer<GC2GS_007_026_ReqGachaRoll>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_007_026_ReqGachaRoll _msg)
    {
        NPUSUserData userData = _committer.getUserData();
        if (userData ==null)
            return;

        RefGachaPool refGachaPool = RefGachaPool.getMgr().get(_msg.getPoolId());
        if (refGachaPool == null)
        {
            _committer.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //判断奖池的开启条件
        if (!NPPlayerConditionDealerMgr.IsEnable(refGachaPool.condition, userData, null))
        {
            _committer.commitFailRes(CommErr.SYSTEM_UNLOCK.getCode());
            return;
        }
        if (refGachaPool.activity_id != 0)
        {
            _AActivityBase activity = getUSServer().getCommActivityMgr().lookupActivity(refGachaPool.activity_id);
            if (activity == null || !activity.isPlaying())
            {
                _committer.commitFailRes(CommErr.SYSTEM_UNLOCK.getCode());
                return;
            }
        }

        //上下文
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.AVATAR_GACHA_ROLL);

        //领取奖励逻辑
        GachaPoolInfo poolInfo = userData.getGachaComponent().ensurePool(refGachaPool);
        if (poolInfo == null)
        {
            _committer.commitFailRes(CommErr.SYSTEM_UNLOCK.getCode());
            return;
        }

        //抽卡次数
        int rollTimes;

        //根据是否十连抽，消耗不同的道具数量
        if (_msg.getIsTen())
        {
            boolean hasItem = userData.hasCostItemList(refGachaPool.ten_roll_cost);
            if (!hasItem)
            {
                _committer.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
                return;
            }

            boolean costResult = userData.spendCostItemList(refGachaPool.ten_roll_cost, context);
            if (!costResult)
            {
                _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
                return;
            }

            rollTimes = 10;
        } else
        {
            // 优先消耗固定CD道具（如配置），否则消耗普通道具
            if (refGachaPool.fixed_cd_id != 0 && userData.hasItem(ENPItemType.FIXED_CD, refGachaPool.fixed_cd_id, 1))
            {
                boolean costResult = userData.spendItem(ENPItemType.FIXED_CD, refGachaPool.fixed_cd_id, 1, context);
                if (!costResult)
                {
                    _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
                    return;
                }
            } else
            {
                boolean hasItem = userData.hasCostItemList(refGachaPool.roll_cost);
                if (!hasItem)
                {
                    _committer.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
                    return;
                }

                boolean costResult = userData.spendCostItemList(refGachaPool.roll_cost, context);
                if (!costResult)
                {
                    _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
                    return;
                }
            }

            rollTimes = 1;
        }

        List<Long> itemIdList = new ArrayList<>();
        //构造产出物 JSON 字符串用于日志记录
        JsonArray productJson = new JsonArray();

        //抽卡
        for (int i = 0; i < rollTimes; i++)
        {
            RefGachaItem rollItemRef = poolInfo.roll(null);
            if (rollItemRef == null)
            {
                USLog.error(getUSServer(), "MsgDealer_GC2GS_018_040_ReqGachaRoll roll failed cid:{} poolId:{}", userData.getCid(), _msg.getPoolId());
                continue;
            }

            //获取物品
            userData.gainItem(rollItemRef.item.getItemType(), rollItemRef.item.getItemId(), rollItemRef.item.getCount(),
                    true, 0, context);

            //记录抽取到的物品ID
            productJson.add(new JsonPrimitive(rollItemRef.item.toString()));

            //判断是否需要公屏展示
            if (rollItemRef.if_show)
                getUSServer().getGachaPublicRecordMgr().addRecord(_msg.getPoolId(), rollItemRef.Id(), userData.getPlayerComponent().getName());

            //判断是否需要公屏展示
            if (rollItemRef.if_show_marquee)
            {
                ArrayList<String> paramList = new ArrayList<>();
                paramList.add(userData.getPlayerComponent().getName());
                paramList.add("##COMMON-ITEM##"+rollItemRef.item.getItemType()+"-"+ rollItemRef.item.getItemId());
                paramList.add(String.valueOf(rollItemRef.item.getCount()));

                getUSServer().getMarqueeMgr().cmdAddMarquee(RefGeneral.Ref().marquee_gacha_draw_item_marquee_id, paramList);
            }

            userData.getGachaComponent().ensurePoolRecord(_msg.getPoolId()).record(rollItemRef.Id(), CommonFunc.getNowTimeSec());

            itemIdList.add(rollItemRef.Id());
        }

        //累计奖励点
        poolInfo.incCumulativeRewardPoint(rollTimes);

        //记录抽卡日志
        MJEventLog.logWish(userData, rollTimes, productJson.toString());

        _committer.commitSucRes(US2GCWriter_007_CommOp.make_026_RetGachaRoll(_msg.getIsTen(), itemIdList, context));

        userData.sendMsgToGC(US2GCWriter_007_CommOp.make_074_OnGachaPoolChg(poolInfo.makeProto()));

        //记录次数
        userData.getRecordComponent().addRecord(ENPPlayerRecordParam.AVATAR_GACHA_ROLL, rollTimes, context);
    }
}
