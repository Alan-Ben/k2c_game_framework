package USDB.Update;

import ALMySqlCommon.ALMySqlDBConditionObj;
import ALMySqlCommon.ALMySqlDBExcutor.ALMySqlDBExcutor;
import Common.HeroObj.Hero_ArenaShowInfo;
import Common.HeroObj.Hero_ArenaShowList;
import NPCommon.DB.ErrDealer.SelectExceptionDealer;
import NPCommon.DB.UpdateBase;
import NPCommon.DB.WCGDBObj;
import NPCommon.Log.CommLog;
import USDB.Bo.PlayerHeroCacheBO;
import USDB.Bo.PlayerHeroListCacheBO;

import java.util.ArrayList;
import java.util.Hashtable;

/**
 * 将玩家大臣缓存从每个大臣一个数据修改到一个玩家一个数据
 *
 * @author claude
 */
public class Update_1_0_1_8_To_1_0_1_9 extends UpdateBase {

    @Override
    public boolean run(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_8_To_1_0_1_9.run - start: begin player cache hero list info update");

        // 更新player_cache表中的guild_info
        if (!updatePlayerHeroListCache(_dbObj)) {
            CommLog.error("Update_1_0_1_8_To_1_0_1_9.run - player cache hero list info update failed");
            return false;
        }

        CommLog.info("Update_1_0_1_8_To_1_0_1_9.run - complete: player cache hero list info update success");
        return true;
    }

    /**
     * 更新玩家缓存数据中的公会信息
     *
     */
    private boolean updatePlayerHeroListCache(WCGDBObj _dbObj) {
        CommLog.info("Update_1_0_1_8_To_1_0_1_9.updatePlayerHeroListCache - start");

        //取出所有已有玩家缓存
        SelectExceptionDealer heroCacheBoErrDealer = new SelectExceptionDealer();
        PlayerHeroCacheBO heroCacheBo = new PlayerHeroCacheBO();
        ArrayList<PlayerHeroCacheBO> heroCacheBoList
                = (ArrayList<PlayerHeroCacheBO>) ALMySqlDBExcutor.getListByCondition(_dbObj, new ALMySqlDBConditionObj(), heroCacheBo, heroCacheBoErrDealer);
        if (heroCacheBoErrDealer.isHasException())
        {
            CommLog.error("Update_1_0_1_8_To_1_0_1_9.updatePlayerHeroListCache - update failed for get all heroList");
            return false;
        }

        Hashtable<Long, Hero_ArenaShowList> userHeroTable = new Hashtable<Long, Hero_ArenaShowList>();
        for(PlayerHeroCacheBO hBo : heroCacheBoList)
        {
            //确保有一个存储数据
            Hero_ArenaShowList heroList = userHeroTable.get(hBo.getCid());
            if(null == heroList)
            {
                heroList = new Hero_ArenaShowList();

                userHeroTable.put(hBo.getCid(), heroList);
            }

            heroList.getHeroList().add(new Hero_ArenaShowInfo(hBo.getHeroId(), hBo.getSkinId(), hBo.getLevel(), hBo.getPower()));
        }

        //构造新数据，放入新表
        for(Long cid : userHeroTable.keySet())
        {
            Hero_ArenaShowList heroList = userHeroTable.get(cid);

            PlayerHeroListCacheBO newBo = new PlayerHeroListCacheBO();
            newBo.setId(_dbObj.ensureTbIdInfo(newBo.getTbName()).popNewId());
            newBo.setCid(null, cid);
            newBo.setHeroList(null, heroList.makePackage().array());

            //直接插入数据
            ArrayList<byte[]> byteList = new ArrayList<>();
            byteList.add(heroList.makePackage().array());

            ALMySqlDBExcutor.InsertBO(_dbObj, newBo, byteList);
        }

        CommLog.info("Update_1_0_1_8_To_1_0_1_9.updatePlayerHeroListCache - complete: updated data count: " + userHeroTable.size());
        return true;
    }
}
