package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.GiftPack.RefGiftPack;
import NPGameRes.Refs.GiftPack.RefGiftPackExtraGain;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class GiftPackInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        Map<Long, List<RefGiftPackExtraGain>> GiftPackMap = new HashMap<>();
        for (RefGiftPackExtraGain refGiftPackExtraGain : RefGiftPackExtraGain.getMgr().getList())
        {
            GiftPackMap.computeIfAbsent(refGiftPackExtraGain.gift_pack_id,
                    k -> new ArrayList<>()).add(refGiftPackExtraGain);
        }

        GiftPackMap.forEach((_giftPackId, _refGiftPackExtraGainList) ->
        {
            RefGiftPack refGiftPack = RefGiftPack.getMgr().get(_giftPackId);
            if (refGiftPack == null)
            {
                CommLog.error("GiftPackInitDealer dealInit lookup refGiftPack fail, giftPackId: {}", _giftPackId);
                return;
            }

            refGiftPack.setRefGiftPackExtraGainList(_refGiftPackExtraGainList);
        });
    }
}
