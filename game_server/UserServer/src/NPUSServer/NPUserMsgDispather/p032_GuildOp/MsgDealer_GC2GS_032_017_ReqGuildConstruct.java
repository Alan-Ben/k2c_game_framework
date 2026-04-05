package NPUSServer.NPUserMsgDispather.p032_GuildOp;

import CommonEnum.ECurrency;
import GC2GS.p032_GuildOp.GC2GS_032_017_ReqGuildConstruct;
import GC2GS.p032_GuildOp.GuildOpStructure.GuildOp_PlayerCname;
import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.ErrMain.CommErr;
import NPEnum.ENPGameEvent;
import NPEnum.ENPItemType;
import NPEnum.ENPPlayerRecordParam;
import NPGameRes.Refs.Guild.RefGuildConstruct;
import NPUSServer.Common.Context.NPPlayerContext;
import NPUSServer.Common.Event.Events.Event_P_GUILD_CONSTRUCT;
import NPUSServer.NPUSUserMgr.ConditionDealer.NPPlayerConditionDealerMgr;
import NPUSServer.NPUSUserMgr.UserMsgMgr.MsgItem._ANPUSUserBasicMsgItem;
import NPUSServer.NPUserMsgDispather.NPUserMsgDealer;

import java.util.ArrayList;
import java.util.List;

public class MsgDealer_GC2GS_032_017_ReqGuildConstruct extends NPUserMsgDealer<GC2GS_032_017_ReqGuildConstruct>
{
    @Override
    protected void _dealMessage(_ANPUSUserBasicMsgItem _committer, GC2GS_032_017_ReqGuildConstruct _msg)
    {
        NPPlayerContext context = NPPlayerContext.createNew(ENPGameEvent.GUILD_CONSTRUCT);

        //查找配置
        RefGuildConstruct refGuildConstruct = RefGuildConstruct.getMgr().get(_msg.getRefId());
        if (refGuildConstruct == null) {
            _committer.commitFailRes(CommErr.REF_NOT_FOUND.getCode());
            return;
        }

        //构造消耗列表
        List<NPCommonCostItem> costList = new ArrayList<>();
        //判断是否需要消耗cd
        long fixCdId = refGuildConstruct.fix_cd_id;
        if (fixCdId != 0)
            costList.add(new NPCommonCostItem(ENPItemType.FIXED_CD, fixCdId, 1));

        //检查是否满足免费条件
        if (refGuildConstruct.free_condition == null || !refGuildConstruct.free_condition.hasCondition()
                || !NPPlayerConditionDealerMgr.IsEnable(refGuildConstruct.free_condition, _committer.getUserData(), null))
            costList.addAll(refGuildConstruct.cost);

        //检查消耗
        if (!_committer.getUserData().hasCostItemList(costList)){
            _committer.commitFailRes(CommErr.ITEM_NOT_ENOUGH.getCode());
            return;
        }
        if (!_committer.getUserData().spendItem(costList, context)){
            _committer.commitFailRes(CommErr.CONSUME_FAIL.getCode());
            return;
        }

        _committer.getUserData().getRecordComponent().addRecord(ENPPlayerRecordParam.GUILD_CONSTRUCT_TIMES, 1, context);

        _committer.getUserData().onLogicEvent(new Event_P_GUILD_CONSTRUCT(context));
        //获取给玩家的奖励
        _committer.getUserData().gainItem(ENPItemType.CURRENCY, ECurrency.GUILD_COIN.ordinal(),
                refGuildConstruct.add_personal_guild_coin, context);

        //建造奖励展示弹窗推送
        _committer.getUserData().sendMsgToGC(context.getCollector().toProto());

        //发送请求到Guild
        GuildOp_PlayerCname addInfo = new GuildOp_PlayerCname();
        addInfo.setCname(_committer.getUserData().getPlayerComponent().getName());

        //转化为统一跨服消息进行处理
        getUSServer().dealGuildMsg(
                _committer,
                _committer.getUserData().getCid(),
                _committer.getUserData().getGuildComponent().getGuildId(),
                _msg,
                addInfo
        );
    }
}
