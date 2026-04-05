package NPGameRes.InitDealer;

import NPCommon.Log.CommLog;
import NPGameRes.Refs.Inn.RefInnStation;
import NPGameRes.Refs.Inn.RefInnStationLevel;
import NPGameRes.Refs.Museum.RefMuseumItem;
import NPGameRes.Refs.Museum.RefMuseumItemLevel;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class InnInitDealer extends _ABasicInitDealer
{
    @Override
    public void dealInit()
    {
        //初始化珍宝等级配表到珍宝配表上
        Map<Long, List<RefMuseumItemLevel>> giftRefMap = new HashMap<>();
        for (RefMuseumItemLevel refGiftLevel : RefMuseumItemLevel.getMgr().getList())
        {
            giftRefMap.computeIfAbsent(refGiftLevel.museum_id, k -> new ArrayList<>()).add(refGiftLevel);
        }
        giftRefMap.forEach((giftId, refLevelList) ->
        {
            RefMuseumItem refMuseumItem = RefMuseumItem.getMgr().get(giftId);
            if (refMuseumItem == null)
            {
                CommLog.error("init inn gift ref fail, not find gift:{} ref.", giftId);
                return;
            }

            refMuseumItem.setLevelList(refLevelList);
        });

        //初始化设施等级配表到设施配表上
        Map<Long, List<RefInnStationLevel>> stationRefMap = new HashMap<>();
        for (RefInnStationLevel refStationLevel : RefInnStationLevel.getMgr().getList())
        {
            stationRefMap.computeIfAbsent(refStationLevel.station_id, k -> new ArrayList<>()).add(refStationLevel);
        }
        stationRefMap.forEach((stationId, refLevelList) ->
        {
            RefInnStation refInnStation = RefInnStation.getMgr().get(stationId);
            if (refInnStation == null)
            {
                CommLog.error("init inn station ref fail, not find station:{} ref.", stationId);
                return;
            }

            refInnStation.setLevelRefList(refLevelList);
        });
    }
}
