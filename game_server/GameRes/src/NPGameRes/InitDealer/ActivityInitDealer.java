package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.Activity.RefActivity;
import NPGameRes.Refs.GiftPack.RefGiftPack;
import NPGameRes.Refs.GiftPack.RefGiftPackGroup;
import NPGameRes.Refs.Rank.RefRank;
import NPGameRes.Refs.RefGeneral;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class ActivityInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        Map<Long, List<Long>> activityGiftPackMap = new HashMap<>();
        for (RefActivity refActivity : RefActivity.getMgr().getList())
        {
            for (Long giftPackGroupId : refActivity.cash_gift_pack_group_id_list)
            {
                RefGiftPackGroup refGiftPackGroup = RefGiftPackGroup.getMgr().get(giftPackGroupId);
                if (refGiftPackGroup == null)
                {
                    CommLog.error("ActivityInitDealer dealInit lookup refGiftPackGroup fail, activityId: {} giftPackGroupId: {}",
                            refActivity.Id(), refActivity.cash_gift_pack_group_id_list);
                    continue;
                }

                for (Long giftPackId : refGiftPackGroup.gift_pack_id_list)
                {
                    activityGiftPackMap.computeIfAbsent(giftPackId, k -> new ArrayList<>()).add(refActivity.Id());
                }
            }
        }

        activityGiftPackMap.forEach((_giftPackId, _activityIdList) ->
        {
            RefGiftPack refGiftPack = RefGiftPack.getMgr().get(_giftPackId);
            if (refGiftPack == null)
            {
                CommLog.error("ActivityInitDealer dealInit lookup refGiftPack fail, giftPackId: {}", _giftPackId);
                return;
            }

            refGiftPack.setRelativeActivityIdList(_activityIdList);

        });


        Map<Integer,List<Long>> logicEventRankMap = new HashMap<>();
        for (RefRank refRank : RefRank.getMgr().getList())
        {
            logicEventRankMap.computeIfAbsent(refRank.getLogicEventId(), k -> new ArrayList<>()).add(refRank.Id());
        }
        RefGeneral.Ref().logicEventRankMap = logicEventRankMap;
    }
}
