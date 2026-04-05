package NPUSServer.GMCommand.Cmds;

import NPCommon.CommonObj.NPCommonCostItem;
import NPCommon.CommonObj.NPDateTimeObj;
import NPCommon.GMCommand.Annotation.ACommand;
import NPCommon.GMCommand.Annotation.ACommander;
import NPCommon.Util.CommonFunc;
import NPGameRes.Refs.RankGift.RefRankGiftPack;
import NPUSServer.GMCommand.UsCmdBase;

@ACommander(comment = "冲榜礼包", name = "rankGiftPack")
public class CmdRankGiftPack extends UsCmdBase
{
    @ACommand(comment = "修改待激活礼包信息[礼包页面加载资源id][折扣][购买上限][名称][消耗道具][原价消耗道具][奖励列表][结束日期 20250101][结束时间 23:59:59]")
    public String modifyGiftPackInfo(long uiResPathId, int sale, int buyLimit, String name, String cost, String oriCost, String rewardList, String endDate, String endTime)
    {
        RefRankGiftPack refRankGiftPack = RefRankGiftPack.getMgr().getList().get(0);
        if (refRankGiftPack == null)
            return "ref not found";

        refRankGiftPack.ui_res_path_id = uiResPathId;
        refRankGiftPack.sale = sale;
        refRankGiftPack.buy_limit = buyLimit;
        refRankGiftPack.name = name;
        NPCommonCostItem costItem = new NPCommonCostItem();
        costItem.parseFromString(cost);
        refRankGiftPack.cost = costItem;
        NPCommonCostItem oriCostItem = new NPCommonCostItem();
        oriCostItem.parseFromString(oriCost);
        refRankGiftPack.ori_cost = oriCostItem;
        refRankGiftPack.reward_item_list = CommonFunc.listStringableFromString(rewardList, NPCommonCostItem.class);
        NPDateTimeObj endDateTime = new NPDateTimeObj();
        endDateTime.parseFromString(endDate + " " + endTime);
        refRankGiftPack.end_time = endDateTime;

        return "done";
    }

    @ACommand(comment = "修改已激活礼包信息[礼包页面加载资源id][折扣][购买上限][名称][消耗道具][原价消耗道具][奖励列表][结束日期 20250101][结束时间 23:59:59]")
    public String modifyActiveGiftPackInfo(long uiResPathId, int sale, int buyLimit, String name, String cost, String oriCost, String rewardList, String endDate, String endTime)
    {
        boolean isSucc = getUserServer().getRankGiftPackMgr().modifyActiveGiftPackInfo(uiResPathId, sale, buyLimit, name, cost, oriCost, rewardList, endDate, endTime);
        if (!isSucc)
            return "active gift pack not found";

        return "done";
    }

    @ACommand(comment = "修改已激活礼包结束时间[结束日期 20250101][结束时间 23:59:59]")
    public String chgActivePackEndTime(String endDate, String endTime)
    {
        boolean isSucc = getUserServer().getRankGiftPackMgr().chgActivePackEndTime(endDate, endTime);
        if (!isSucc)
            return "active gift pack not found";

        return "done";
    }


}
